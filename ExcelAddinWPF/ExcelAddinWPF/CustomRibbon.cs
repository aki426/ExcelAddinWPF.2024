using Microsoft.Office.Tools;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;

using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelAddinWPF
{
    public partial class CustomRibbon
    {
        /// <summary>
        /// リボンがロードされた際に実行されるイベントハンドラーです。
        /// Word起動->リボンをロード->TshiAddIn.Startupの順で実行される。
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void CustomRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            // NOTE: Nlogなどアドインを開くウィンドウに依存しない、アプリケーションの全体設定はここに書くと良い。
        }

        /// <summary>
        /// リボンがクローズされた際に実行されるイベントハンドラーです。
        /// ThisAddIn.Shutdown->リボンをクローズ->Word終了の順で実行される。
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void CustomRibbon_Close(object sender, EventArgs e)
        {
            // NOTE: アドインソリューション全体で最後に1回実行される。
            // Logger関係の後片付けなどはここに書くと良い。
        }

        /// <summary>
        /// リボンに登録したアドインスタートボタンをクリックした際に実行されるイベントハンドラーです。
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void buttonStartAddin_Click(object sender, RibbonControlEventArgs e)
        {
            // アドインの動作の前提として何らかのドキュメントを開いているかどうかが問題となるケースがある。
            // その場合は

            try
            {
                if (Globals.ThisAddIn.Application.ActiveWindow == null)
                {
                    // ドキュメントが開かれていない場合の処理
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }

            // 現在のアクティブウィンドウに対して、アドインインスタンスが起動中かどうかをチェックする。
            IEnumerable<CustomTaskPane> enumerable = Globals.ThisAddIn.CustomTaskPanes
                .Where(i => i != null && i.Window != null && (i.Window as Excel.Window) != null)
                .Where(i => (i.Window as Excel.Window).Hwnd == Globals.ThisAddIn.Application.ActiveWindow.Hwnd);

            if (!enumerable.Any())
            {
                // 現在のWindowsにカスタム作業ウィンドウが無い場合は新規作成する。
                string title = ApplicationDeployment.IsNetworkDeployed ?
                    $"Sample Addin - {ApplicationDeployment.CurrentDeployment.CurrentVersion}" :
                    $"Sample Addin - DEBUG version.";

                // NOTE: カスタム作業ウィンドウに与えるWinFormsユーザーコントロールはVSTOプロジェクトで用意し、
                // WPFのユーザーコントロールはWinFormsユーザーコントロールから呼び出す。

                // NOTE: WinFormsユーザコントロールとWPFユーザコントロールは個々のプロジェクト内で通常通り作成する。
                // WinFormsのプロジェクトからWPFプロジェクトへの参照を追加すると、
                // WinFormユーザコントロールのビジュアルエディタのツールボックスにWPFユーザコントロールが表示される。
                // この状態でD＆Dすれば、WinFormsユーザコントロールにWPFユーザコントロールを埋め込むことができる。
                // しかし、MaterialDesignなどを適用している場合、ビジュアルエディタでの表示が正しく行われないことがある。
                // ビジュアルエディタでの表示はあくまでVSの都合なので、これを回避するために一旦MaterialDesignを使用わない
                // WPFユーザコントロールを作成し、それをWinFormsユーザコントロールに埋め込む。するとうまくいく。
                // 一旦埋め込んだ後は、ビジュアルエディタで埋め込んでいるWPFユーザコントロールを切り替えることができるので、
                // 任意のWPFユーザコントロールへ切り替える。この手順を踏むとなぜかをMaterialDesignを使用した
                // WPFユーザコントロールでも表示時エラーが発生しなくなる。

                // コード上はカスタム作業ウィンドウにWinFormsユーザーコントロールを追加するだけ。
                MainUserControl mainUserControl = new MainUserControl();
                CustomTaskPane customTaskPane = Globals.ThisAddIn.CustomTaskPanes.Add(
                    mainUserControl,
                    title,
                    Globals.ThisAddIn.Application.ActiveWindow);
                customTaskPane.Width = 600;
                customTaskPane.Visible = true;

                // カスタム作業ウィンドウを閉じる際のイベントハンドラを登録する。
                // 先にVisibleをtrueにしているので、次に変更されるのは閉じるとき。
                customTaskPane.VisibleChanged += CustomTaskPaneClose_Click;
            }
        }

        // NOTE: 【カスタム作業ウィンドウの横幅の最小値について】
        // 例えば、カスタム作業ウィンドウの横幅を600pixel以下に縮めることができないように規制する方法は提供されていない。
        // CustomTaskPaneにMinWidthプロパティは無く、またWidhChangedイベントも提供されていない。
        // ThisAddinなどにTimerをセットして600pixel以下になったら600pixelに戻す、という方法もあるがスマートではない。
        // UserControlのMinimumSizeでコンテンツの表示は最小横幅を設定可能なので一旦それで対応するのが良さそう。

        /// <summary>
        /// カスタム作業ウィンドウを閉じる際のイベントハンドラです。
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void CustomTaskPaneClose_Click(object sender, EventArgs e)
        {
            // NOTE: Officeアプリケーションを閉じるイベントや、アドインを終了するイベントとイコールではない点に注意。
            // カスタム作業ウィンドウを閉じずにOfficeアプリケーションを閉じた場合、
            // VisibleChangedも発火するためこのイベントハンドラも実行される。

            CustomTaskPane customTaskPane = sender as CustomTaskPane;
            MainUserControl mainUserControl = customTaskPane.Control as MainUserControl;

            // カスタム作業ウィンドウのVisibleChangedがアプリケーションウィンドウのCloseより後で発火していないかのチェック。
            if (customTaskPane.Window != null)
            {
            }
            else
            {
                // 複数のOfficeのウィンドウが開かれており、複数のカスタム作業ウィンドウが表示されている常体で
                // アプリケーションウィンドウを閉じた場合、WindowがNullのカスタム作業ウィンドウ、というケースが発生することがある。
                // Wordの場合、最後のWord Windowを閉じてWordが全て終了する場合、VisibleChangedは発火しない。
            }

            // NOTE: カスタム作業ウィンドウの「ｘ」ボタンを押しただけではVisibleがFalseにセットされるだけ。
            // カスタム作業ウィンドウは破棄されないので、破棄するコードが必要。
            Globals.ThisAddIn.CustomTaskPanes.Remove(customTaskPane);
        }
    }
}