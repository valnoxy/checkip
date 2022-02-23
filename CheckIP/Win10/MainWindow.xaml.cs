using ModernWpf.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CheckIP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        FetchIP LoadedFetchIP;
        MyIP LoadedMyIP;
        About LoadedAbout;

        public MainWindow()
        {
            InitializeComponent();
            LoadedFetchIP = new FetchIP();
            LoadedMyIP = new MyIP();
            LoadedAbout = new About();
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            NavView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            // Set the initial SelectedItem
            foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "Page_Settings")
                {
                    NavView.SelectedItem = item;
                    break;
                }
            }
            NavView.SelectedItem = NavView.MenuItems[0];
            ContentFrame.Navigate(LoadedFetchIP);
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItem is TextBlock ItemContent)
            {
                switch (ItemContent.Tag)
                {
                    case "FetchIP":
                        ContentFrame.Navigate(LoadedFetchIP);
                        break;

                    case "MyIP":
                        ContentFrame.Navigate(LoadedMyIP);
                        break;

                    case "About":
                        ContentFrame.Navigate(LoadedAbout);
                        break;
                }
            }
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
