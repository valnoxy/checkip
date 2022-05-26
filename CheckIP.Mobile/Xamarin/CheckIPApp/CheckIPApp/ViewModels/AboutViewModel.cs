using CheckIPApp.Services;
using Sharpnado.MaterialFrame;
using System;
using System.Windows.Input;
using Microsoft.Maui.Essentials;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace CheckIPApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://github.com/valnoxy/checkip"));
        }

        public ICommand OpenWebCommand { get; }
    }
}