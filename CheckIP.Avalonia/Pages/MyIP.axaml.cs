using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using System.IO;

namespace CheckIP.Avalonia.Pages
{
    public partial class MyIP : UserControl
    {
        private static string _myIp;

        public MyIP()
        {
            InitializeComponent();
            _myIp = Task.Run(_GetIPAddress).GetAwaiter().GetResult();
            Task.Run(ParseIpAddress);
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
                ErrorLabel.Content = "Error: This is not a valid IP address";
                return;
            }

            Dispatcher.UIThread.InvokeAsync(() =>
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
                Dispatcher.UIThread.InvokeAsync(() =>
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
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    ErrorLabel.Content = "Error: " + message;
                    IpAddress.Content = "Unknown";
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
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                IpAddress.Content = _myIp;
                ValueCityCountry.Text = city + " / " + country + " (" + countryCode + ")";
                ValuePostal.Text = postal;
                ValueTimezone.Text = timezone;
                ValueLatitude.Text = latitude;
                ValueLongitude.Text = longitude;
                ValueIsp.Text = isp;
                ValueAsn.Text = asn;
                ValueMobile.Text = mobile;
                ValueProxy.Text = proxy;
                ValueHosting.Text = hosting;
            });
        }

        private void ExportBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // Build export string
            var exportString = $@"Report created at {DateTime.Now} for IP {IpAddress.Content}
City / Country: {ValueCityCountry.Text}
Postal: {ValuePostal.Text}
Timezone: {ValueTimezone.Text}
Latitude: {ValueLatitude.Text}
Longitude: {ValueLongitude.Text}
ISP or Organization: {ValueIsp.Text}
ASN: {ValueAsn.Text}
Is Mobile: {ValueMobile.Text}
Is Proxy: {ValueProxy.Text}
Is Hosting: {ValueHosting.Text}
";

            var saveFileDialog = new SaveFileDialog
            {
                Filters =
                {
                    new FileDialogFilter
                    {
                        Name = "Text file",
                        Extensions = new List<string> {"txt"}
                    }
                },
                InitialFileName = "CheckIP_Report.txt"
            };
            var SettingsFileName = saveFileDialog.ShowAsync;

            if (SettingsFileName != null)
                File.WriteAllText(SettingsFileName.ToString(), exportString);
        }
    }
}
