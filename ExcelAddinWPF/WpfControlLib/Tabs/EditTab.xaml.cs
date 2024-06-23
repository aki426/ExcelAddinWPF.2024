using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace WpfControlLib
{
    /// <summary>
    /// EditTab.xaml の相互作用ロジック
    /// </summary>
    public partial class EditTab : UserControl
    {
        public EditTab()
        {
            InitializeComponent();
        }

        // http://main.tinyjoker.net/Tech/CSharp/WPF/ListBox%A4%CE%A5%A2%A5%A4%A5%C6%A5%E0%A4%F2%C8%BE%C6%A9%CC%C0%A5%B4%A1%BC%A5%B9%A5%C8%A4%C4%A4%AD%A5%C9%A5%E9%A5%C3%A5%B0%A5%A2%A5%F3%A5%C9%A5%C9%A5%ED%A5%C3%A5%D7%A4%C7%CA%C2%A4%D9%C2%D8%A4%A8%A4%EB.html#footer

        //private ListBoxItem dragItem;
        //private Point dragStartPos;
        //private DragAdorner dragGhost;

        //private void listBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    // マウスダウンされたアイテムを記憶
        //    dragItem = sender as ListBoxItem;
        //    // マウスダウン時の座標を取得
        //    dragStartPos = e.GetPosition(dragItem);
        //}

        ///// <summary>
        ///// マウスが動いたときの処理
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void listBoxItem_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    var lbi = sender as ListBoxItem;
        //    if (e.LeftButton == MouseButtonState.Pressed && dragGhost == null && dragItem == lbi)
        //    {
        //        var nowPos = e.GetPosition(lbi);
        //        // 左マウスボタンが押されていて、ゴーストが null、sender が前項のアイテムと同じ、
        //        // かつ、 マウス移動量がシステム設定値以上になったら、ドラッグのフェーズに入る。
        //        if (Math.Abs(nowPos.X - dragStartPos.X) > SystemParameters.MinimumHorizontalDragDistance ||
        //            Math.Abs(nowPos.Y - dragStartPos.Y) > SystemParameters.MinimumVerticalDragDistance)
        //        {
        //            listBox.AllowDrop = true;

        //            // まず、AdornerLayer.GetAdornerLayer メソッド（スタティック）でリストボックスの装飾レイヤーを得る。
        //            var layer = AdornerLayer.GetAdornerLayer(listBox);
        //            // オーナー、装飾オブジェクト、透明度、ドラッグ開始位置を渡して、ゴーストを初期化する。
        //            dragGhost = new DragAdorner(listBox, lbi, 0.5, dragStartPos);
        //            // ゴーストを装飾レイヤーへ追加
        //            layer.Add(dragGhost);
        //            // ドラッグドロップ処理を開始（ここで、ドロップされるまでブロックされる）
        //            DragDrop.DoDragDrop(lbi, lbi, DragDropEffects.Move);

        //            // 後処理
        //            layer.Remove(dragGhost);
        //            dragGhost = null;
        //            dragItem = null;

        //            listBox.AllowDrop = false;
        //        }
        //    }
        //}

        ///// <summary>
        ///// ゴーストの位置をマウスに追従させる
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void listBoxItem_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        //{
        //    if (dragGhost != null)
        //    {
        //        var p = CursorInfo.GetNowPosition(this);
        //        var loc = this.PointFromScreen(listBox.PointToScreen(new Point(0, 0)));
        //        dragGhost.LeftOffset = p.X - loc.X;
        //        dragGhost.TopOffset = p.Y - loc.Y;
        //    }
        //}

        //private void listBox_Drop(object sender, DragEventArgs e)
        //{
        //    // ドロップされた位置を取得しておく。
        //    var dropPos = e.GetPosition(listBox);
        //    // e.Data.GetData でドロップされた ListBoxItem が取得できる。
        //    var lbi = e.Data.GetData(typeof(ListBoxItem)) as ListBoxItem;

        //    // ListBoxItem.DataContext でバインド要素が得られるので、これの元の位置を取得しておく。
        //    var o = lbi.DataContext as SampleListItemViewModel;
        //    var index = SampleListItemViewModel.IndexOf(o);

        //    // 最初の要素からループを回し、ドロップ座標から、アイテムの新しい位置（インデックス）を割り出す。
        //    // 見つかればその時点で要素を入れ替えて抜ける。見つからなければ、一番最後に要素をもってきて終わり。
        //    for (int i = 0; i < MyClasses.Count; i++)
        //    {
        //        // ListBox.Item はバインドされたオブジェクトなので、
        //        // それぞれの ListBoxItem を取得したければ、listBox.ItemContainerGenerator を使う。
        //        var item = listBox.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
        //        // アイテムの左端上下中央の listBox 上での座標を割り出している。
        //        // なぜかというと、ここをアイテムの前に挿入するのか、後ろに挿入するのかの判断ポイントにしているからである。
        //        // ListBoxItem の上半分でドロップされた場合はそのアイテムの前に、下半分の場合はそのアイテムの後ろに追加するようにする。
        //        // なお、今回は縦にアイテムが積まれているリストボックスの場合なので Y 座標のみ見ているが、そうでない場合は、X 座標も評価する必要がある）
        //        var pos = listBox.PointFromScreen(item.PointToScreen(new Point(0, item.ActualHeight / 2)));
        //        if (dropPos.Y < pos.Y)
        //        {
        //            // i が入れ換え先のインデックス
        //            // ここではバインド元（MyClasses）は OvservableCollection<T> クラスであり、
        //            // Move メソッドが用意されていたので、入れ替えはこれを利用した。
        //            // 三項演算子による分岐は要素が下方向に移動されたか上方向に移動されたによるものである。
        //            // 下方向に移動されている場合は、自分の分のインデックスを -1 する必要がある。
        //            MyClasses.Move(index, (index < i) ? i - 1 : i);
        //            return;
        //        }
        //    }
        //    // 最後にもっていく
        //    int last = MyClasses.Count - 1;
        //    MyClasses.Move(index, last);
        //}
    }
}