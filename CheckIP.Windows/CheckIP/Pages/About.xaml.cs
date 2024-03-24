using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für About.xaml
    /// </summary>
    public partial class About
    {
        private class ThirdParty
        {
            internal string Author { get; init; }
            internal string Product { get; init; }
        }

        public About()
        {
            InitializeComponent();

            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location);
            ValueVersion.Content = $"{versionInfo.ProductName} v{versionInfo.ProductVersion}";
            ValueCopyright.Content = versionInfo.LegalCopyright;

            var legalCopyright = versionInfo.LegalCopyright;
            var localizedAllRightsReserved = Common.LocalizationManager.LocalizeValue("All rights reserved.");
            var localizedCopyright = legalCopyright!.Replace("All rights reserved.", localizedAllRightsReserved);
            ValueCopyright.Content = localizedCopyright;
            
            var thirdPartyList = new List<ThirdParty>
            {
                new()
                {
                    Author = "Lecopo",
                    Product = "WPF-UI"
                },
                new()
                {
                    Author = "Hardcodet",
                    Product = "Hardcodet.NotifyIcon.Wpf"
                },
                new()
                {
                    Author = "Icons8",
                    Product = "Flag Icons"
                },
                new()
                {
                    Author = "James Newton-King",
                    Product = "Newtonsoft.Json"
                }
            };

            foreach (var thirdParty in thirdPartyList)
            {
                var localizedBy = Common.LocalizationManager.LocalizeValue("by");
                var formattedMessage = string.Format(localizedBy, thirdParty.Product, thirdParty.Author);

                var tb = new TextBlock
                {
                    Text = $"• {formattedMessage}",
                    TextWrapping = TextWrapping.Wrap
                };
                ThirdPartyList.Children.Add(tb);
            }
        }

        private void Homepage_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo() { FileName = "https://exploitox.de", UseShellExecute = true });
            }
            catch
            {
                // ignored
            }
        }

        private void Donate_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo() { FileName = "https://paypal.me/valnoxy", UseShellExecute = true });
            }
            catch
            {
                // ignored
            }
        }

        private void SourceCode_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo() { FileName = "https://github.com/valnoxy/checkip", UseShellExecute = true });
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }
    }
}
