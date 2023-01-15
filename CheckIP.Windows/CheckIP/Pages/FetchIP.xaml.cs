using Microsoft.Win32;
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
using System.Windows.Input;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für FetchIP.xaml
    /// </summary>
    public partial class FetchIP : Page
    {
        private static string _myIp;
        
        public FetchIP()
        {
            InitializeComponent();
        }

        private void FetchIP_Click(object sender, RoutedEventArgs e)
        {
            _myIp = IpAddress.Text;
            Task.Run(ParseIpAddress);
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return) return;
            _myIp = IpAddress.Text;
            Task.Run(ParseIpAddress);
        }

        private void ParseIpAddress()
        {
            // Validate IP
            var validateIp = IPAddress.TryParse(_myIp, out var ip);
            if (!validateIp)
            {
                Dispatcher.Invoke(() =>
                {
                    errorLabel.Content = "Error: This is not a valid IP address";
                });
                return;
            }

            Dispatcher.Invoke(() =>
            {
                errorLabel.Content = string.Empty;
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
                    errorLabel.Content = "Error: No connection to server";
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
                Dispatcher.Invoke(() =>
                {
                    errorLabel.Content = "Error: " + message;
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
        }

        private void ExportBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // Build export string
            var exportString = $@"Report created at {DateTime.Now} for IP {IpAddress.Text}

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
    }
}
