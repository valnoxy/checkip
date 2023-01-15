using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CheckIP.Common;
using Hardcodet.Wpf.TaskbarNotification;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für Main.xaml
    /// </summary>
    public partial class Main : Wpf.Ui.Controls.UiWindow
    {
        public static Main? ContentMain;

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

#if DEBUG
            debugLabel.Content = "Debug build - This is not a production ready build.";
#endif
        }

        private void RootNavigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate("dashboard");

            TaskBar.Initialize();
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
