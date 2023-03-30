using Avalonia.Controls;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using System;

namespace CheckIP.Avalonia
{
    public partial class MainWindow : CoreWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var nv = this.FindControl<NavigationView>("RootNavigation");
            nv.SelectionChanged += OnNVSample2SelectionChanged;
            nv.SelectedItem = nv.MenuItems.ElementAt(0);
        }

        private void OnNVSample2SelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
        {
            if (e.SelectedItem is NavigationViewItem nvi)
            {
                var smpPage = $"CheckIP.Avalonia.Pages.{nvi.Tag}";
                var pg = Activator.CreateInstance(Type.GetType(smpPage));
                (sender as NavigationView).Content = pg;
            }
        }
    }
}