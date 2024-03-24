using CheckIP.Common;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using Wpf.Ui.Controls;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für Main.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set current language model
            var language = Thread.CurrentThread.CurrentCulture.ToString();
            var dict = new ResourceDictionary();
            Debug.WriteLine("Trying to load language: " + language);
            switch (language)
            {
                default:
                case "en-US":
                    dict.Source = new Uri(@"/CheckIP;component/Localization/ResourceDictionary.xaml", UriKind.Relative);
                    break;
                case "de-DE":
                    dict.Source = new Uri(@"/CheckIP;component/Localization/ResourceDictionary.de-DE.xaml", UriKind.Relative);
                    break;
                case "it-IT":
                    dict.Source = new Uri(@"/CheckIP;component/Localization/ResourceDictionary.it-IT.xaml", UriKind.Relative);
                    break;
            }
            Application.Current.Resources.MergedDictionaries.Add(dict);

#if DEBUG
            DebugString.Text = "Debug build";
#endif
        }

        private void MainWindow_OnContentRendered(object sender, EventArgs e)
        {
            RootNavigation.Navigate(typeof(FetchIP));
            TaskBar.Initialize();
        }

        private void ThemeSwitch_Click(object sender, RoutedEventArgs e)
        {
            WindowBackdropType = WindowBackdropType == WindowBackdropType.Mica ? WindowBackdropType.Tabbed : WindowBackdropType.Mica;
        }
    }
}
