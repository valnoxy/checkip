using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CheckIP.Win11
{
    /// <summary>
    /// Interaktionslogik für W11Main.xaml
    /// </summary>
    public partial class W11Main : Window
    {
        public W11Main()
        {
            InitializeComponent();
        }

        private void RootNavigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate("dashboard");
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
