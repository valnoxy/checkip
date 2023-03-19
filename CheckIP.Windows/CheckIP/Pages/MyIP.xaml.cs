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
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAvailabilityChanged;
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
            // Validate IP
            var validateIp = IPAddress.TryParse(_myIp, out var ip);
            if (!validateIp)
            {
                Dispatcher.Invoke(() =>
                {
                    var error = (string)Application.Current.MainWindow.FindResource("ErrorInvalidIP");
                    ErrorLabel.Content = !string.IsNullOrEmpty(error) ? error : "Error: This is not a valid IP address.";
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
                    var error = (string)Application.Current.MainWindow.FindResource("ErrorInvalidIP");
                    ErrorLabel.Content = !string.IsNullOrEmpty(error) ? error : "Error: No connection to server";
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
                var localizeError = Common.LocalizationManager.LocalizeError(message);

                this.Dispatcher.Invoke(() =>
                {
                    ErrorLabel.Content = localizeError;
                    ValueCityCountry.Text = "Unknown";
                    ValuePostal.Text = "Unknown";
                    ValueTimezone.Text = "Unknown";
                    ValueLatitude.Text = "Unknown";
                    ValueLongitude.Text = "Unknown";
                    ValueIsp.Text = "Unknown";
                    ValueAsn.Text = "Unknown";
                    ValueMobile.Text = "Unknown";
                    ValueProxy.Text = "Unknown";
                    ValueHosting.Text = "Unknown";
                });
                return;
            }

            // Set Variable labels
            var localizeMobile = Common.LocalizationManager.LocalizeValue(mobile);
            var localizeProxy = Common.LocalizationManager.LocalizeValue(proxy);
            var localizeHosting = Common.LocalizationManager.LocalizeValue(hosting);
            Dispatcher.Invoke(() =>
            {
                IpAddress.Content = _myIp;
                ValueCityCountry.Text = city + " / " + country + " (" + countryCode + ")";
                ValuePostal.Text = postal;
                ValueTimezone.Text = timezone;
                ValueLatitude.Text = latitude;
                ValueLongitude.Text = longitude;
                ValueIsp.Text = isp;
                ValueAsn.Text = asn;
                ValueMobile.Text = localizeMobile;
                ValueProxy.Text = localizeProxy;
                ValueHosting.Text = localizeHosting;
            });

            // Update NotifyIcon
            Dispatcher.Invoke(() => TaskBar.Update(country, city, ip, countryCode));
        }
        
        private void ExportBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // Build export string
            var reportCreatedMessage = (string)Application.Current.MainWindow.FindResource("ReportCreatedMessage");
            var formattedMessage = string.Format(reportCreatedMessage, DateTime.Now, IpAddress.Content);

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

            var exportString = $@"{formattedMessage}

{cityAndCountry}: {ValueCityCountry.Text}
{postal}: {ValuePostal.Text}
{timezone}: {ValueTimezone.Text}
{latitude}: {ValueLatitude.Text}
{longitude}: {ValueLongitude.Text}
{isp}: {ValueIsp.Text}
{asn}: {ValueAsn.Text}
{mobile}: {ValueMobile.Text}
{proxy}: {ValueProxy.Text}
{hosting}: {ValueHosting.Text}
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
