using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;

namespace CheckIP
{
    /// <summary>
    /// Interaktionslogik für About.xaml
    /// </summary>
    public partial class About : Page
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
