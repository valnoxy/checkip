using CheckIPApp.Services;
using Sharpnado.MaterialFrame;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

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