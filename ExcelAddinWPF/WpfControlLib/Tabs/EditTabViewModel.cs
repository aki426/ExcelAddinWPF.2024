using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace WpfControlLib
{
    public class EditTabViewModel : INotifyPropertyChanged, IDisposable
    {
        // 継承インターフェイスの実装
        public event PropertyChangedEventHandler PropertyChanged;

        private CompositeDisposable _disposables { get; } = new CompositeDisposable();

        // プロパティ
        public ReactiveProperty<string> SettingStatusMessage { get; }

        public ReactiveProperty<bool> CheckBoxStatus { get; }

        // コマンド
        public ReactiveCommand ReloadButtonClickCommand { get; }

        public EditTabViewModel()
        {
            // プロパティの初期化
            SettingStatusMessage = new ReactiveProperty<string>("").AddTo(_disposables);

            // 設定画面のチェックボックス状態を監視。
            CheckBoxStatus = new ReactiveProperty<bool>(false).AddTo(_disposables);

            MessageBus.Listen<CheckBoxStatusChangedMessage>()
                .Select(msg => msg.IsChecked)
                .Subscribe(isChecked => CheckBoxStatus.Value = isChecked)
                .AddTo(_disposables);

            // コマンドの初期化
            ReloadButtonClickCommand = new ReactiveCommand()
                .WithSubscribe(() =>
                {
                    // 更新処理
                    this.ChangeStatusMessage();
                }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void ChangeStatusMessage()
        {
            if (CheckBoxStatus.Value)
            {
                SettingStatusMessage.Value = "設定画面のチェックボックスがONになりました。";
            }
            else
            {
                SettingStatusMessage.Value = "設定画面のチェックボックスがOFFになりました。";
            }
        }
    }
}