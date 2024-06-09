# ExcelAddinWPF.2024

VSTO Addin for Excel with WPF sample Ver.2024

## Versions

- C# 10.0
- .Net Framework 4.8.1
- VSTO Addin
- CommunityToolkit

## Project settings

- WpfControlLib
  - C# LangVersion : 10.0
  - Nullable : enable

## 手順

1. VSTOアドインプロジェクト（以下、VSTOプロジェクト）をVSの機能で作成する。
2. WPFユーザコントロールプロジェクト（以下、WPFプロジェクト）をVSの機能で作成する。
3. WPFプロジェクトでWPFユーザコントロールを作成する。
4. VSTOプロジェクトでWinFormsユーザコントロールを作成し、リボンにボタンなどを登録し、ボタン押下時にカスタム作業ウィンドウへWinFormsユーザコントロールを登録・表示するコードを追加する。
5. WinFormsユーザコントロールへWPFユーザコントロールを登録する。
   1. ビジュアルエディタを用いる場合は、MaterialDesignThemeなどを使用しているWPFユーザコントロールを登録しようとすると参照エラーが出てうまくいかない。
   2. そこで、一旦MaterialDesignThemeなどのリソースを使用していない単純なWPFユーザコントロールを作成し、それを登録する。
   3. 登録時にエラーが発生しなかったら、元々登録したかった任意のWPFユーザコントロールへ変更する。
6. Officeの操作は、別にOfficeアプリ操作用のライブラリプロジェクトを用意し、それにThisAddInの持つMicrosoft.Office.Interop.*のインスタンスを渡すようにする。
   1. 参照関係として、VSTOプロジェクト -> WPFプロジェクト -> Officeライブラリプロジェクトという関係にする。
   2. WPFプロジェクトからはOfficeライブラリプロジェクトを操作するだけにするとよい。どのみちMicrosoft.Office.Interop.*はGlobalsから取得できるグローバル変数のような扱いであり、GCやリソース管理についてあまり深く考えなくても使用できる。

## メモ

CommunityToolkitを使う予定だったが、ReactiveCollectionに相当する機能が無いらしく悩ましい。

## 参考

- [WPF Tips - Material Designにする](https://qiita.com/yossihard/items/df994b9e4005c3b46da0)
- [Material Design In XAML Toolkit 4.9→5.0 バージョンアップ時のエラーと解決策](https://qiita.com/programing_diy_kanrinin/items/d7d550a83b48a54bafd0)
- [MaterialDesignInXamlToolkit Getting Started](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/wiki/Getting-Started)
- [CommunityToolkit.Mvvm V8 入門](https://qiita.com/kk-river/items/d974b02f6c4010433a9e)
- [[WPF] Collection への要素の追加を即時に ListBox に反映する方法](https://mseeeen.msen.jp/wpf-mvvm-for-listbox/)
