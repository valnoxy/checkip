using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CheckIP.Pages;

namespace CheckIP
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new Main();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void NativeMenuItem_OnClick(object? sender, EventArgs e)
        {
            var window = new Window() 
            {
                Title = "About CheckIP",
                Content = new About(),
                MaxHeight = 500,
                MaxWidth = 400,
                MinHeight = 500,
                MinWidth = 400,
                CanResize = false
            };
            window.Show();
        }
    }
}