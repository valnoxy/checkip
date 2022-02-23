using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckIP
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            int build = Environment.OSVersion.Version.Build;

            if (build <= 22000)
            {
                var application = new W10();
                application.InitializeComponent();
                application.Run();
            }
            if (build >= 22000)
            {
                var application = new W11();
                application.InitializeComponent();
                application.Run();
            }
        }
    }
}
