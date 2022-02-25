using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace CheckIP.Win11
{
    /// <summary>
    /// Interaktionslogik für W11Main.xaml
    /// </summary>
    public partial class W11Main : Window
    {
        public W11Main()
        {
            WPFUI.Background.Manager.Apply(this);
            InitializeComponent();
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            this.Background = Brushes.Transparent;
            WPFUI.Background.Manager.Apply(WPFUI.Background.BackgroundType.Mica, windowHandle);
        }

        private void RootNavigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate("dashboard");
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
