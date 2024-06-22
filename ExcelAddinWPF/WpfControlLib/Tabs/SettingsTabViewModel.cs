using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace WpfControlLib
{
    public class SettingsTabViewModel : INotifyPropertyChanged, IDisposable
    {
        // 継承インターフェイスの実装
        public event PropertyChangedEventHandler PropertyChanged;

        private CompositeDisposable _disposables { get; } = new CompositeDisposable();

        // プロパティ
        public ReactiveProperty<bool> IsLargeFontCheckBoxFlag { get; }

        public SettingsTabViewModel()
        {
            // プロパティの初期化
            IsLargeFontCheckBoxFlag = new ReactiveProperty<bool>(false).AddTo(_disposables);

            // メッセージングモデルの初期化
            IsLargeFontCheckBoxFlag.Subscribe(isChecked =>
            {
                MessageBus.Publish(new CheckBoxStatusChangedMessage(isChecked));
            }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }

    // NOTE: ReactivePropertyのメッセージングモデルを用いてCheckBoxの状態を通知する

    public class CheckBoxStatusChangedMessage
    {
        public bool IsChecked { get; }

        public CheckBoxStatusChangedMessage(bool isChecked)
        {
            IsChecked = isChecked;
        }
    }

    public static class MessageBus
    {
        private static Subject<object> _subject = new Subject<object>();

        public static IObservable<T> Listen<T>() where T : class
        {
            return _subject.OfType<T>();
        }

        public static void Publish<T>(T message) where T : class
        {
            _subject.OnNext(message);
        }
    }
}