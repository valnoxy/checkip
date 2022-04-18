using System.Collections.ObjectModel;
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
        }

        private void ShowDialog_Libs(object sender, System.Windows.RoutedEventArgs e)
        {
            (((Main)System.Windows.Application.Current.MainWindow)!).LibDialog.Show = true;
        }
    }
}
