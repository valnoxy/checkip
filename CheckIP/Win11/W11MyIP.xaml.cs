using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace CheckIP.Win11
{
    /// <summary>
    /// Interaktionslogik für W11MyIP.xaml
    /// </summary>
    public partial class W11MyIP : Page
    {
        private string myip;
        public W11MyIP()
        {
            InitializeComponent();

            // Get IP
            myip = new WebClient().DownloadString("https://ifconfig.me/ip");
            IPaddr.Content = myip;
            RequestData();
        }

        private void RequestData()
        {
            // Validate IP
            IPAddress ip;
            bool ValidateIP = IPAddress.TryParse(myip, out ip);
            if (!ValidateIP)
            {
                errorLabel.Content = "Error: This is not a valide IP address";
                return;
            }
            else
            {
                errorLabel.Content = string.Empty;
            }

            string dataJson = string.Empty; ;
            string strIpLocation = string.Empty;
            string url = "http://ip-api.com/json/" + myip + "?fields=status,message,country,countryCode,city,zip,lat,lon,timezone,isp,as,mobile,proxy,hosting";
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
                errorLabel.Content = "Error: No connection to server";
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
                errorLabel.Content = "Error: " + message;
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
        }
    }
}
