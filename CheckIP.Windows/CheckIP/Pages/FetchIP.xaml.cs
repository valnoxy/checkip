using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CheckIP.Common;
using System.ComponentModel;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für FetchIP.xaml
    /// </summary>
    public partial class FetchIP
    {
        private static string _myIp;
        private static BackgroundWorker _worker;
        
        public FetchIP()
        {
            InitializeComponent();
            _worker = new BackgroundWorker();
            _worker.DoWork += FetchAndParse;
            _worker.RunWorkerCompleted += NotBusy;
        }

        private void NotBusy(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                InfoPanel.Visibility = Visibility.Visible;
                BusyPanel.Visibility = Visibility.Collapsed;
                FetchBtn.IsEnabled = true;
            });
        }

        private void FetchAndParse(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                InfoPanel.Visibility = Visibility.Collapsed;
                BusyPanel.Visibility = Visibility.Visible;
                FetchBtn.IsEnabled = false;
            });

            Task.Run(ParseIpAddress).Wait();
        }

        private void FetchIP_Click(object sender, RoutedEventArgs e)
        {
            _myIp = IpAddress.Text;
            _worker.RunWorkerAsync();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return) return;
            _myIp = IpAddress.Text;
            _worker.RunWorkerAsync();
        }

        private void ParseIpAddress()
        {
            var data = IPManager.Parse(_myIp);

            Dispatcher.Invoke(() =>
            {
                ErrorLabel.Text = string.Empty;
            });
            
            if (!data.ParseSuccess)
            {
                Dispatcher.Invoke(() =>
                {
                    ErrorLabel.Text = data.Status;
                    ValueCityCountry.Text = "Unknown";
                    ValuePostal.Text = "Unknown";
                    ValueTimezone.Text = "Unknown";
                    ValueLatitude.Text = "Unknown";
                    ValueLongitude.Text = "Unknown";
                    ValueIsp.Text = "Unknown";
                    ValueAsn.Text = "Unknown";
                    ValueMobile.IsChecked = false;
                    ValueProxy.IsChecked = false;
                    ValueHosting.IsChecked = false;
                });
                return;
            }

            Dispatcher.Invoke(() =>
            {
                ValueCityCountry.Text = data.City + " / " + data.Country + " (" + data.CountryCode + ")";
                ValuePostal.Text = data.Postal;
                ValueTimezone.Text = data.Timezone;
                ValueLatitude.Text = data.Latitude;
                ValueLongitude.Text = data.Longitude;
                ValueIsp.Text = data.Isp;
                ValueAsn.Text = data.Asn;
                ValueMobile.IsChecked = data.Mobile;
                ValueProxy.IsChecked = data.Proxy;
                ValueHosting.IsChecked = data.Hosting;
            });
        }

        private void ExportBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // Build export string
            var reportCreatedMessage = (string)Application.Current.MainWindow!.FindResource("ReportCreatedMessage");
            var formattedMessage = string.Format(reportCreatedMessage, DateTime.Now, IpAddress.Text);

            var cityAndCountry = (string)Application.Current.MainWindow.FindResource("CityAndCountry");
            var postal = (string)Application.Current.MainWindow.FindResource("Postal");
            var timezone = (string)Application.Current.MainWindow.FindResource("Timezone");
            var latitude = (string)Application.Current.MainWindow.FindResource("Latitude");
            var longitude = (string)Application.Current.MainWindow.FindResource("Longitude");
            var isp = (string)Application.Current.MainWindow.FindResource("ISPOrOrganization");
            var asn = (string)Application.Current.MainWindow.FindResource("ASN");
            var mobile = (string)Application.Current.MainWindow.FindResource("IsMobile");
            var proxy = (string)Application.Current.MainWindow.FindResource("IsProxy");
            var hosting = (string)Application.Current.MainWindow.FindResource("IsHosting");

            var exportString = $"""
                                {formattedMessage}

                                {cityAndCountry}: {ValueCityCountry.Text}
                                {postal}: {ValuePostal.Text}
                                {timezone}: {ValueTimezone.Text}
                                {latitude}: {ValueLatitude.Text}
                                {longitude}: {ValueLongitude.Text}
                                {isp}: {ValueIsp.Text}
                                {asn}: {ValueAsn.Text}
                                {mobile}: {ValueMobile.IsChecked}
                                {proxy}: {ValueProxy.IsChecked}
                                {hosting}: {ValueHosting.IsChecked}

                                """;

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt",
                FileName = "CheckIP_Report.txt"
            };
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, exportString);
        }
    }
}
