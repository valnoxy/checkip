using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Svg.Skia;
using Svg.Skia;
using System.Diagnostics;
using System.Reflection;

namespace CheckIP.Avalonia.Pages
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();

            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            ValueVersion.Content = $"{versionInfo.ProductName} V. {versionInfo.ProductVersion}";
            ValueCopyright.Content = versionInfo.LegalCopyright;
        }
    }
}
