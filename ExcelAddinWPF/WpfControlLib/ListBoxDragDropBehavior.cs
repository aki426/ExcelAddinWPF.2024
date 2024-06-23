using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfControlLib
{
    public class ListBoxDragDropBehavior : Behavior<ListBox>
    {
        private Point startPoint;
        private object draggedItem;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseLeftButtonDown += ListBox_PreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseMove += ListBox_PreviewMouseMove;
            AssociatedObject.Drop += ListBox_Drop;
            AssociatedObject.AllowDrop = true;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseLeftButtonDown -= ListBox_PreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseMove -= ListBox_PreviewMouseMove;
            AssociatedObject.Drop -= ListBox_Drop;
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePos = e.GetPosition(null);
                Vector diff = startPoint - mousePos;

                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    ListBox listBox = sender as ListBox;
                    ListBoxItem listBoxItem = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);

                    if (listBoxItem != null)
                    {
                        draggedItem = listBox.ItemContainerGenerator.ItemFromContainer(listBoxItem);
                        DragDrop.DoDragDrop(listBoxItem, draggedItem, DragDropEffects.Move);
                    }
                }
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            object data = e.Data.GetData(draggedItem.GetType());
            if (data != null)
            {
                int removeIndex = listBox.Items.IndexOf(draggedItem);
                int insertIndex = listBox.Items.IndexOf(GetNearestItem(e.GetPosition(listBox)));

                if (removeIndex != insertIndex)
                {
                    if (listBox.ItemsSource is System.Collections.IList list)
                    {
                        list.Remove(draggedItem);
                        list.Insert(insertIndex, draggedItem);
                    }
                }
            }
        }

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

        private object GetNearestItem(Point position)
        {
            UIElement element = AssociatedObject.InputHitTest(position) as UIElement;
            while (element != null)
            {
                object item = AssociatedObject.ItemContainerGenerator.ItemFromContainer(element);
                if (item != DependencyProperty.UnsetValue)
                {
                    return item;
                }
                element = VisualTreeHelper.GetParent(element) as UIElement;
            }
            return null;
        }
    }
}