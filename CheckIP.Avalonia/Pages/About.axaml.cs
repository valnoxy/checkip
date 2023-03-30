using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Svg.Skia;
using Svg.Skia;
using System;
using System.Diagnostics;
using System.Reflection;
using Avalonia;

namespace CheckIP.Avalonia.Pages
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();

            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location);
            var avaloniaInformationalVersion = typeof(AvaloniaObject).Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
            var avaloniaVersion = avaloniaInformationalVersion![..avaloniaInformationalVersion.IndexOf("+", StringComparison.Ordinal)];

            var fluentAvaloniaVersion = typeof(FluentAvalonia.UI.Controls.NavigationView).Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

            var newtonSoftInformationalVersion = typeof(Newtonsoft.Json.JsonConvert).Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
            var newtonsoftVersion = newtonSoftInformationalVersion![..newtonSoftInformationalVersion.IndexOf("+", StringComparison.Ordinal)];

            ValueVersion.Content = $"{versionInfo.ProductName} V. {versionInfo.ProductVersion}";
            ValueCopyright.Content = versionInfo.LegalCopyright;
            AvaloniaVersion.Content = $"Avalonia V. {avaloniaVersion}";
            FluentAvaloniaVersion.Content = $"FluentAvalonia V. {fluentAvaloniaVersion}";
            NewtonsoftVersion.Content = $"Newtonsoft.Json V. {newtonsoftVersion}";

        }
    }
}
