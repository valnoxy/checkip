using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Threading;

namespace CheckIP.Common
{
    public class IPManager
    {
        public class IP
        {
            public string City { get; set; }
            public string Country { get; set; }
            public string CountryCode { get; set; }
            public string Postal { get; set; }
            public string Timezone { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Isp { get; set; }
            public string Asn { get; set; }
            public bool Mobile { get; set; }
            public bool Proxy { get; set; }
            public bool Hosting { get; set; }
            public bool ParseSuccess { get; set; }
            public string Status { get; set; }
            public IPAddress IPAddress { get; set; }
        }

        public static IP Parse(string ip)
        {
            var ipData = new IP();

            var validateIp = IPAddress.TryParse(ip, out var ipAddr);
            if (!validateIp)
            {
                ipData.ParseSuccess = false;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var error = (string)Application.Current.MainWindow!.FindResource("ErrorInvalidIP");
                    ipData.Status = !string.IsNullOrEmpty(error) ? error : "Error: This is not a valid IP address.";
                });
                return ipData;
            }

            // Parse data
            string dataJson;
            var url = "http://ip-api.com/json/" + ip + "?fields=status,message,country,countryCode,city,zip,lat,lon,timezone,isp,as,mobile,proxy,hosting";
            try
            {
                var client = new HttpClient();
                using var response = client.GetAsync(url).Result;
                using var content = response.Content;
                dataJson = content.ReadAsStringAsync().Result;
            }
            catch
            {
                ipData.ParseSuccess = false;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var error = (string)Application.Current.MainWindow!.FindResource("ErrorNoConnection");
                    ipData.Status = !string.IsNullOrEmpty(error) ? error : "Error: No connection to server";
                });
                return ipData;
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
                ipData.Status = localizeError;
                ipData.ParseSuccess = false;
                return ipData;
            }

            ipData.City = city;
            ipData.Country = country;
            ipData.CountryCode = countryCode;
            ipData.Postal = postal;
            ipData.Timezone = timezone;
            ipData.Latitude = latitude;
            ipData.Longitude = longitude;
            ipData.Isp = isp;
            ipData.Asn = asn; 
            ipData.Mobile = mobile.Equals("true", System.StringComparison.CurrentCultureIgnoreCase);
            ipData.Proxy = proxy.Equals("true", System.StringComparison.CurrentCultureIgnoreCase);
            ipData.Hosting = hosting.Equals("true", System.StringComparison.CurrentCultureIgnoreCase);
            ipData.IPAddress = ipAddr;
            ipData.ParseSuccess = true;

            return ipData;
        }
    }
}
