﻿using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CheckIP.Common;
using System.Net.NetworkInformation;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für MyIP.xaml
    /// </summary>
    public partial class MyIP : Page
    {
        private static string _myIp;

        public MyIP()
        {
            InitializeComponent();

            _myIp = Task.Run(_GetIPAddress).GetAwaiter().GetResult();
            Task.Run(ParseIpAddress);

            // Trigger NetworkChange
            NetworkChange.NetworkAddressChanged +=
                new NetworkAddressChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
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
                try
                {
                    result = await Task.Run(() => new WebClient().DownloadString("https://api.ipify.org"));
                }
                catch
                {
                    
                }
            }

            return result;
        }

        private void ParseIpAddress()
        {
            // Validate IP
            var validateIp = IPAddress.TryParse(_myIp, out var ip);
            if (!validateIp)
            {
                Dispatcher.Invoke(() =>
                {
                    ErrorLabel.Content = "Error: This is not a valid IP address";
                });
                return;
            }

            Dispatcher.Invoke(() =>
            {
                ErrorLabel.Content = string.Empty;
            });

            var dataJson = string.Empty;
            var url = "http://ip-api.com/json/" + _myIp + "?fields=status,message,country,countryCode,city,zip,lat,lon,timezone,isp,as,mobile,proxy,hosting";
            try
            {
                var client = new HttpClient();
                using var response = client.GetAsync(url).Result;
                using var content = response.Content;
                dataJson = content.ReadAsStringAsync().Result;
            }
            catch
            {
                this.Dispatcher.Invoke(() =>
                {
                    ErrorLabel.Content = "Error: No connection to server";
                });
            }
            dynamic data = JObject.Parse(dataJson);

            // Parse data
            string status = data.status;
            string message = data.message;
            string country = data.country;
            string countryCode = data.countryCode;
            string city = data.city;
            string postal = data.zip;
            string timezone = data.timezone;
            string latitude = data.lat;
            string longitude = data.lon;
            string isp = data.isp;
            string asn = data.@as;
            string mobile = data.mobile;
            string proxy = data.proxy;
            string hosting = data.hosting;

            // Check status
            if (status != "success")
            {
                this.Dispatcher.Invoke(() =>
                {
                    ErrorLabel.Content = "Error: " + message;
                    valueCityCountry.Text = "Unknown";
                    valuePostal.Text = "Unknown";
                    valueTimezone.Text = "Unknown";
                    valueLatitude.Text = "Unknown";
                    valueLongitude.Text = "Unknown";
                    valueISP.Text = "Unknown";
                    valueASN.Text = "Unknown";
                    valueMobile.Text = "Unknown";
                    valueProxy.Text = "Unknown";
                    valueHosting.Text = "Unknown";
                });
                return;
            }

            // Set Variable labels
            Dispatcher.Invoke(() =>
            {
                IpAddress.Content = _myIp;
                valueCityCountry.Text = city + " / " + country + " (" + countryCode + ")";
                valuePostal.Text = postal;
                valueTimezone.Text = timezone;
                valueLatitude.Text = latitude;
                valueLongitude.Text = longitude;
                valueISP.Text = isp;
                valueASN.Text = asn;
                valueMobile.Text = mobile;
                valueProxy.Text = proxy;
                valueHosting.Text = hosting;
            });

            // Update NotifyIcon
            Dispatcher.Invoke(() => TaskBar.Update(country, city, ip, countryCode));
        }
        
        private void ExportBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // Build export string
            var exportString = $@"Report created at {DateTime.Now} for IP {IpAddress.Content}

City / Country: {valueCityCountry.Text}
Postal: {valuePostal.Text}
Timezone: {valueTimezone.Text}
Latitude: {valueLatitude.Text}
Longitude: {valueLongitude.Text}
ISP or Organization: {valueISP.Text}
ASN: {valueASN.Text}
Is Mobile: {valueMobile.Text}
Is Proxy: {valueProxy.Text}
Is Hosting: {valueHosting.Text}
";

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt",
                FileName = "CheckIP_Report.txt"
            };
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, exportString);
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, EventArgs e)
        {
            _myIp = Task.Run(_GetIPAddress).GetAwaiter().GetResult();
            Task.Run(ParseIpAddress);
        }
    }
}
