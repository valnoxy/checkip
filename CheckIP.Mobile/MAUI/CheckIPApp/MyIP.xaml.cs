using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Net;

namespace CheckIPApp;

public partial class MyIP : ContentPage
{
    private string myip;

    public MyIP()
	{
		InitializeComponent();

        // Get IP Address
        myip = ReadResponseFromUrl("https://ifconfig.me/ip");
        ipAddress.Text = myip;
        FetchIP(myip);
    }

    private void FetchIP(string IP)
    {
        // Validate IP
        IPAddress ip;
        bool ValidateIP = IPAddress.TryParse(IP, out ip);
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
            DisplayAlert("Error", $"No connection to server.\n{ex}", "OK");
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