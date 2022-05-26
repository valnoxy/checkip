using CheckIPApp.Services;
using CheckIPApp.Views;
using Sharpnado.MaterialFrame;
using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

[assembly: ExportFont("materialdesignicons.ttf", Alias = "MaterialIcons")]

namespace CheckIPApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();

            // Initialize MaterialFrame
            Initializer.Initialize(loggerEnable: false, debugLogEnable: false);

            // Set MaterialFrame theme
            ThemeManager();

            // Respond if the theme changes
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                ThemeManager();
            };
        }

        public void ThemeManager()
        {
            OSAppTheme currentTheme = Application.Current.RequestedTheme;
            if (currentTheme == OSAppTheme.Dark)
            {
                // Set the theme to dark
                MaterialFrame.ChangeGlobalTheme(MaterialFrame.Theme.Dark);
            }
            else
            {
                // Set the theme to light
                MaterialFrame.ChangeGlobalTheme(MaterialFrame.Theme.Light);
            }
        }
        
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
