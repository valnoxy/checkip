using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Net.NetworkInformation;
using System.ComponentModel;
using CheckIP.Common;
using System.Collections.Generic;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für MyIP.xaml
    /// </summary>
    public partial class MyIP
    {
        private static string _myIp;
        private readonly Queue<Action> workQueue = new Queue<Action>();
        private readonly object queueLock = new object();
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public MyIP()
        {
            InitializeComponent();

            _myIp = Task.Run(_GetIPAddress).GetAwaiter().GetResult();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            // Trigger NetworkChange
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAvailabilityChanged;

            EnqueueWork(FetchAndParse);
        }

        private void FetchAndParse()
        {
            Dispatcher.Invoke(() =>
            {
                InfoPanel.Visibility = Visibility.Collapsed;
                BusyPanel.Visibility = Visibility.Visible;
            });

            Task.Run(ParseIpAddress).Wait();

            Dispatcher.Invoke(() =>
            {
                InfoPanel.Visibility = Visibility.Visible;
                BusyPanel.Visibility = Visibility.Collapsed;
            });
        }

        [Obsolete("Will be replaced to a async version soon.")]
        private static async Task<string> _GetIPAddress()
        {
            string result = null;
            try
            {
                result = await Task.Run(() => new WebClient().DownloadString("https://ifconfig.me/ip"));
            }
            catch
            {
                try // Alternative source
                {
                    result = await Task.Run(() => new WebClient().DownloadString("https://api.ipify.org"));
                }
                catch
                {
                    // ignored
                }
            }

            return result;
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
                IpAddress.Text = _myIp;
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
                TaskBar.Update(data.Country, data.City, data.IPAddress, data.CountryCode);
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

        private void EnqueueWork(Action work)
        {
            lock (queueLock)
            {
                workQueue.Enqueue(work);
                if (!worker.IsBusy)
                {
                    worker.RunWorkerAsync();
                }
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Action work = null;
            lock (queueLock)
            {
                if (workQueue.Count > 0)
                {
                    work = workQueue.Dequeue();
                }
            }

            work?.Invoke();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (queueLock)
            {
                if (workQueue.Count > 0)
                {
                    worker.RunWorkerAsync();
                }
            }
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, EventArgs e)
        {
            EnqueueWork(() =>
            {
                _myIp = Task.Run(() => _GetIPAddress()).GetAwaiter().GetResult();
                FetchAndParse();
            });
        }
    }
}
