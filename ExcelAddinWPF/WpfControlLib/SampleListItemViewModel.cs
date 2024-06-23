using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;

namespace WpfControlLib
{
    public class SampleListItemViewModel : INotifyPropertyChanged, IDisposable
    {
        // 継承インターフェイスの実装
        public event PropertyChangedEventHandler PropertyChanged;

        private CompositeDisposable _disposables { get; } = new CompositeDisposable();

        // プロパティ
        public ReactiveProperty<string> ItemName { get; }

        public ReactiveProperty<bool> IsCheckedFlag { get; }

        public SampleListItemViewModel(string name)
        {
            // プロパティの初期化
            ItemName = new ReactiveProperty<string>(name).AddTo(_disposables);
            IsCheckedFlag = new ReactiveProperty<bool>(false).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}