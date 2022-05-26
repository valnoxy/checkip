using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System.Net.Http;

namespace CheckIPApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FetchIP : ContentPage
    {
        public FetchIP()
        {
            InitializeComponent();
        }

        private void FetchIPBtn_Clicked(object sender, EventArgs e)
        {
            // Validate IP
            IPAddress ip;
            bool ValidateIP = IPAddress.TryParse(ipAddress.Text, out ip);
            if (!ValidateIP)
            {
                DisplayAlert("Error", "This is not a valid IP address.", "OK");
                return;
            }

            string dataJson = string.Empty;
            string strIpLocation = string.Empty;
            string url = "http://ip-api.com/json/" + ipAddress.Text + "?fields=status,message,country,countryCode,city,zip,lat,lon,timezone,isp,as,mobile,proxy,hosting";
            try
            {
                dataJson = ReadResponseFromUrl(url);
            }
            catch (Exception ex)
            {
#if DEBUG
                DisplayAlert("Error", $"An error has occurred. See Debug console for more infomation.\n\nException message: {ex.Message}", "OK");
                Console.WriteLine("[!] Error while connecting with server:\n" + ex);
#else
                DisplayAlert("Error", $"No connection to server.\nException message: {ex.Message}", "OK");
#endif                
                return;
            }

            // Check if returned data is empty
            if (dataJson == string.Empty)
            {
                DisplayAlert("Error", "No data returned.", "OK");
                return;
            }

            var dictionary = JsonConvert.DeserializeObject<IDictionary>(dataJson);
            foreach (var key in dictionary.Keys)
            {
                Console.WriteLine(key);
                strIpLocation += key.ToString() + ": " + dictionary[key] + "\r\n";
            }
            dynamic jData = JObject.Parse(dataJson);

            // Parse data
            string status = jData.status;
            string message = jData.message;
            string country = jData.country;
            string country_code = jData.countryCode;
            string city = jData.city;
            string postal = jData.zip;
            string timezone = jData.timezone;
            string latitude = jData.lat;
            string longitude = jData.lon;
            string isp = jData.isp;
            string asn = jData.@as;
            string mobile = jData.mobile;
            string proxy = jData.proxy;
            string hosting = jData.hosting;

            // Check status
            if (status != "success")
            {
                DisplayAlert("Error", message, "OK");
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
                return;
            }

            // Set Variable labels
            valueCityCountry.Text = city + " / " + country + " (" + country_code + ")";
            valuePostal.Text = postal;
            valueTimezone.Text = timezone;
            valueLatitude.Text = latitude;
            valueLongitude.Text = longitude;
            valueISP.Text = isp;
            valueASN.Text = asn;
            valueMobile.Text = mobile;
            valueProxy.Text = proxy;
            valueHosting.Text = hosting;
        }

        string ReadResponseFromUrl(string url)
        {
            var httpClientHandler = new HttpClientHandler();
            var httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(url)
            };
            using (var response = httpClient.GetAsync(url))
            {
                string responseBody = response.Result.Content.ReadAsStringAsync().Result;
                return responseBody;
            }
        }

    }
}