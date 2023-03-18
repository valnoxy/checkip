using System;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CheckIP.Common;
using Hardcodet.Wpf.TaskbarNotification;
using Windows.Globalization;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für Main.xaml
    /// </summary>
    public partial class Main : Wpf.Ui.Controls.UiWindow
    {
        public Main()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                Wpf.Ui.Appearance.Watcher.Watch(
                  this,                                 // Window class
                  Wpf.Ui.Appearance.BackgroundType.Mica, // Background type
                  true                                  // Whether to change accents automatically
                );
            };

            // Set current language model
            var language = Thread.CurrentThread.CurrentCulture.ToString();
            var dict = new ResourceDictionary();
            switch (language)
            {
                default:
                case "en-US":
                    dict.Source = new Uri(@"/CheckIP;component/Localization/ResourceDictionary.xaml", UriKind.Relative);
                    break;
                case "de-DE":
                    dict.Source = new Uri(@"/CheckIP;component/Localization/ResourceDictionary.de-DE.xaml", UriKind.Relative);
                    break;
            }
            Application.Current.Resources.MergedDictionaries.Add(dict);

#if DEBUG
            DebugLabel.Content = "Debug build - This is not a production ready build.";
#endif
        }

        private void RootNavigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate("dashboard");

            TaskBar.Initialize();
        }
    }
}
