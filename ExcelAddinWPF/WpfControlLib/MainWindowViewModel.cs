using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfControlLib
{
    [INotifyPropertyChanged]
    internal partial class MainWindowViewModel
    {
        [ObservableProperty]
        private string _text = "Hello, World!";
    }
}