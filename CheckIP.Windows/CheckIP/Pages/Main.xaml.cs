using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public ObservableCollection<string> ThirdPartyList { get; set; }

        public Main()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                WPFUI.Appearance.Watcher.Watch(
                  this,                                 // Window class
                  WPFUI.Appearance.BackgroundType.Mica, // Background type
                  true                                  // Whether to change accents automatically
                );
            };

            ThirdPartyList = new ObservableCollection<string>()
            {
                "Somewhere over the rainbow",
                "Way up high",
                "And the dreams that you dream of",
                "Once in a lullaby, oh"
            };

#if DEBUG
            debugLabel.Content = "Debug Build";
#endif
        }

        private void RootNavigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate("dashboard");
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        
        private void LibDialog_RightButtonClick(object sender, RoutedEventArgs e)
        {
            LibDialog.Show = false;
        }
    }
}
