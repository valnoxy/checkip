using Avalonia.Controls;
using System;
using System.Reflection;
using Avalonia;

namespace CheckIP.Pages
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();

            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                var productName = Assembly.GetExecutingAssembly().GetName().Name;
                ValueVersion.Content = $"{productName} V. {version}";
                ValueCopyright.Content = GetCopyright();
                
                var avaloniaInformationalVersion = typeof(AvaloniaObject).Assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
                var avaloniaVersion =
                    avaloniaInformationalVersion![
                        ..avaloniaInformationalVersion.IndexOf("+", StringComparison.Ordinal)];

                var fluentAvaloniaVersion = typeof(FluentAvalonia.UI.Controls.NavigationView).Assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

                var newtonSoftInformationalVersion = typeof(Newtonsoft.Json.JsonConvert).Assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
                var newtonsoftVersion =
                    newtonSoftInformationalVersion![
                        ..newtonSoftInformationalVersion.IndexOf("+", StringComparison.Ordinal)];

                AvaloniaVersion.Content = $"Avalonia V. {avaloniaVersion}";
                FluentAvaloniaVersion.Content = $"FluentAvalonia V. {fluentAvaloniaVersion}";
                NewtonsoftVersion.Content = $"Newtonsoft.Json V. {newtonsoftVersion}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        private string GetCopyright()
        {
            var asm = Assembly.GetExecutingAssembly();
            var obj = asm.GetCustomAttributes(false);
            foreach (var o in obj)
            {
                if (o.GetType() !=
                    typeof(System.Reflection.AssemblyCopyrightAttribute)) continue;
                var aca = (AssemblyCopyrightAttribute) o;
                return aca.Copyright;
            }
            return string.Empty;
        }
        
        private string GetVersion()
        {
            var asm = Assembly.GetExecutingAssembly();
            var obj = asm.GetCustomAttributes(false);
            foreach (var o in obj)
            {
                if (o.GetType() !=
                    typeof(System.Reflection.AssemblyVersionAttribute)) continue;
                var aca = (AssemblyVersionAttribute) o;
                return aca.Version;
            }
            return string.Empty;
        }
    }
}
