using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für About.xaml
    /// </summary>
    public partial class About : Page
    {
        private class ThirdParty
        {
            internal string Author { get; set; }
            internal string Product { get; set; }
        }

        public About()
        {
            InitializeComponent();

            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            ValueVersion.Content = $"{versionInfo.ProductName} V. {versionInfo.ProductVersion}";

            var legalCopyright = versionInfo.LegalCopyright;
            var localizedAllRightsReserved = Common.LocalizationManager.LocalizeValue("All rights reserved.");
            var localizedCopyright = legalCopyright!.Replace("All rights reserved.", localizedAllRightsReserved);
            ValueCopyright.Content = localizedCopyright;
            
            var thirdPartyList = new List<ThirdParty>
            {
                new ThirdParty
                {
                    Author = "Lecopo",
                    Product = "WPF-UI"
                },
                new ThirdParty
                {
                    Author = "Hardcodet",
                    Product = "Hardcodet.NotifyIcon.Wpf"
                },
                new ThirdParty
                {
                    Author = "Icons8",
                    Product = "Flag Icons"
                },
                new ThirdParty
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
    }
}
