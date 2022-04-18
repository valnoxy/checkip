using System.Diagnostics;

namespace CheckIPApp;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
	}

    private void OpenGitHub(object sender, EventArgs e)
    {
        Launcher.OpenAsync("https://github.com/valnoxy/checkip");
    }

    private void OpenHomepage(object sender, EventArgs e)
    {
        Launcher.OpenAsync("https://exploitox.de");
    }

    private void OpenLicense(object sender, EventArgs e)
    {
        Launcher.OpenAsync("https://github.com/valnoxy/checkip/blob/main/LICENSE");
    }
}