using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für FetchIP.xaml
    /// </summary>
    public partial class FetchIP : System.Windows.Controls.Page
    {
        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        public FetchIP()
        {
            InitializeComponent();
        }

        private void FetchIP_Click(object sender, RoutedEventArgs e)
        {
            ParseIPaddr();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ParseIPaddr();
            }
        }

        private void ParseIPaddr()
        {
            progress.IsActive = true;
            progress.Visibility = Visibility.Visible;

            // Validate IP
            IPAddress ip;
            bool ValidateIP = IPAddress.TryParse(IPaddr.Text, out ip);
            if (!ValidateIP)
            {
                mw.errorLabel.Content = "Error: This is not a valide IP address";
                progress.IsActive = false;
                progress.Visibility = Visibility.Collapsed; return;
            }
            else
            {
                mw.errorLabel.Content = string.Empty;
            }

            string dataJson = string.Empty; ;
            string strIpLocation = string.Empty;
            string url = "http://ip-api.com/json/" + IPaddr.Text + "?fields=status,message,country,countryCode,city,zip,lat,lon,timezone,isp,as,mobile,proxy,hosting";
            try
            {
                HttpClient client = new HttpClient();
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        dataJson = content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch
            {
                mw.errorLabel.Content = "Error: No connection to server";
            }

            var dictionary = JsonConvert.DeserializeObject<IDictionary>(dataJson);
            foreach (var key in dictionary.Keys)
            {
                Console.WriteLine(key);
                strIpLocation += key.ToString() + ": " + dictionary[key] + "\r\n";
            }
            dynamic data = JObject.Parse(dataJson);

            // Parse data
            string status = data.status;
            string message = data.message;
            string country = data.country;
            string country_code = data.countryCode;
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
                mw.errorLabel.Content = "Error: " + message;
                valueCityCountry.Content = "Unknown";
                valuePostal.Content = "Unknown";
                valueTimezone.Content = "Unknown";
                valueLatitude.Content = "Unknown";
                valueLongitude.Content = "Unknown";
                valueISP.Content = "Unknown";
                valueASN.Content = "Unknown";
                valueMobile.Content = "Unknown";
                valueProxy.Content = "Unknown";
                valueHosting.Content = "Unknown";

                progress.IsActive = false;
                progress.Visibility = Visibility.Collapsed;
                return;
            }

            // Set Variable labels
            valueCityCountry.Content = city + " / " + country + " (" + country_code + ")";
            valuePostal.Content = postal;
            valueTimezone.Content = timezone;
            valueLatitude.Content = latitude;
            valueLongitude.Content = longitude;
            valueISP.Content = isp;
            valueASN.Content = asn;
            valueMobile.Content = mobile;
            valueProxy.Content = proxy;
            valueHosting.Content = hosting;

            // Disable progressring
            progress.IsActive = false;
            progress.Visibility = Visibility.Collapsed;
        }
    }
}
