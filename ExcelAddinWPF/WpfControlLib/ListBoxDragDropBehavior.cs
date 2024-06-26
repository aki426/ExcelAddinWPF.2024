﻿using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfControlLib
{
    // NOTE: D&D中にゴーストを表示させる方法は次のページが詳しい。
    // http://main.tinyjoker.net/Tech/CSharp/WPF/ListBox%A4%CE%A5%A2%A5%A4%A5%C6%A5%E0%A4%F2%C8%BE%C6%A9%CC%C0%A5%B4%A1%BC%A5%B9%A5%C8%A4%C4%A4%AD%A5%C9%A5%E9%A5%C3%A5%B0%A5%A2%A5%F3%A5%C9%A5%C9%A5%ED%A5%C3%A5%D7%A4%C7%CA%C2%A4%D9%C2%D8%A4%A8%A4%EB.html#footer

    /// <summary>
    /// ListBoxにドラッグ＆ドロップ機能を追加するビヘイビア
    /// </summary>
    public class ListBoxDragDropBehavior : Behavior<ListBox>
    {
        private Point startPoint;
        private object draggedItem;

        // NOTE:
        // 1. ユーザーがListBoxのアイテムをクリックし、ドラッグを開始すると、ListBox_PreviewMouseMoveメソッドがそれを検出します。
        // 2. ドラッグが一定距離を超えると、DragDrop.DoDragDropメソッドが呼び出され、ドラッグ操作が開始されます。
        // 3. ユーザーがアイテムをドロップすると、ListBox_Dropメソッドが呼び出されます。
        // このメソッドは、ドロップされた位置に基づいてアイテムの新しい位置を計算し、リスト内でアイテムを移動します。
        // 4. FindAncestorとGetNearestItemメソッドは、ビジュアルツリー内の要素を探索するためのヘルパーメソッドです。
        // これらは、ドラッグされているアイテムやドロップ位置を正確に特定するのに役立ちます。

        /// <summary>
        /// ビヘイビアがListBoxにアタッチされたときに呼ばれるメソッド
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            // 必要なイベントハンドラを登録
            AssociatedObject.PreviewMouseLeftButtonDown += ListBox_PreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseMove += ListBox_PreviewMouseMove;
            AssociatedObject.Drop += ListBox_Drop;
            AssociatedObject.AllowDrop = true; // ドロップを許可
        }

        /// <summary>
        /// ビヘイビアがListBoxからデタッチされるときに呼ばれるメソッド
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            // 登録したイベントハンドラを解除
            AssociatedObject.PreviewMouseLeftButtonDown -= ListBox_PreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseMove -= ListBox_PreviewMouseMove;
            AssociatedObject.Drop -= ListBox_Drop;
        }

        /// <summary>
        /// DragHandleアイテムかどうかを判定する。
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool IsDragHandle(DependencyObject element)
        {
            while (element != null && !(element is ListBoxItem))
            {
                if (element is FrameworkElement fe && fe.Name == "DragHandle")
                {
                    return true;
                }
                element = VisualTreeHelper.GetParent(element);
            }
            return false;
        }

        /// <summary>
        /// D&D対象のListBoxオブジェクトを取得する関数。
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private object FindListBoxItem(DependencyObject element)
        {
            while (element != null && !(element is ListBoxItem))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            if (element is ListBoxItem listBoxItem)
            {
                return (AssociatedObject as ListBox).ItemContainerGenerator.ItemFromContainer(listBoxItem);
            }

            return null;
        }

        /// <summary>
        /// マウスの左ボタンが押されたときのイベントハンドラ
        /// </summary>
        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDragHandle(e.OriginalSource as DependencyObject))
            {
                // ドラッグ開始位置を記録
                startPoint = e.GetPosition(null);
                draggedItem = FindListBoxItem(e.OriginalSource as DependencyObject);
            }
            else
            {
                draggedItem = null;
            }
        }

        /// <summary>
        /// マウスが移動したときのイベントハンドラ
        /// </summary>
        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && draggedItem != null)
            {
                Point mousePos = e.GetPosition(null);
                Vector diff = startPoint - mousePos;

                // 最小ドラッグ距離を超えた場合、ドラッグ処理を開始
                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    //ListBox listBox = sender as ListBox;
                    ListBoxItem listBoxItem = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);

                    if (listBoxItem != null)
                    {
                        // ドラッグするアイテムを特定し、ドラッグ処理を開始
                        DragDrop.DoDragDrop(listBoxItem, draggedItem, DragDropEffects.Move);
                    }
                }
            }
        }

        /// <summary>
        /// アイテムがドロップされたときのイベントハンドラ
        /// </summary>
        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            if (draggedItem != null)
            {
                ListBox listBox = sender as ListBox;
                object data = e.Data.GetData(draggedItem.GetType());
                if (data != null)
                {
                    // ドラッグ元とドロップ先のインデックスを取得
                    int removeIndex = listBox.Items.IndexOf(draggedItem);
                    object nearestItem = FindListBoxItem(e.OriginalSource as DependencyObject);
                    int insertIndex = nearestItem != null ? listBox.Items.IndexOf(nearestItem) : -1;

                    // Drop先のインデックス正常に取得できており、Drop元と異なる場合のみ、アイテムを移動
                    if (0 <= insertIndex && removeIndex != insertIndex)
                    {
                        if (listBox.ItemsSource is System.Collections.IList list)
                        {
                            list.Remove(draggedItem);
                            list.Insert(insertIndex, draggedItem);
                        }
                    }
                }
            }

            draggedItem = null;
        }

        /// <summary>
        /// 指定された型Tの最も近い祖先要素を探す
        /// </summary>
        /// <typeparam name="T">探す要素の型</typeparam>
        /// <param name="current">探索を開始する要素</param>
        /// <returns>見つかった祖先要素、見つからない場合はnull</returns>
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
    }
}