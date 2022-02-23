using System.Diagnostics;
using System.Windows.Controls;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für About.xaml
    /// </summary>
    public partial class About : Page
    {
        public About()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigateToGithub(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/valnoxy/checkip",
                UseShellExecute = true
            });
        }

        private void Hyperlink_RequestNavigateToHomepage(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://exploitox.de",
                UseShellExecute = true
            });
        }

        private void Hyperlink_RequestNavigateToLicense(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://exploitox.de/license/checkip",
                UseShellExecute = true
            });
        }
    }
}
