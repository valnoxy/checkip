using Avalonia.Controls;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Navigation;
using System;
using System.Collections.Generic;
using CheckIP.Avalonia.Pages;

namespace CheckIP.Avalonia
{
    public partial class Main : Window
    {
        public NavigationView? NavView;
        public static Main Instance { get; private set; }

        public Main()
        {
            InitializeComponent();
            Instance = this;
#if DEBUG
            DebugLabel.Content = "Avalonia Debug build - This is not a production ready build.";
            DebugLabel.IsVisible = true;
#endif

            // Build the navigation menu
            NavView = this.FindControl<NavigationView>("RootNavigation");

            var navItems = new List<NavigationViewItemBase>();
            var footerNavItems = new List<NavigationViewItemBase>();
            navItems.Add(new NavigationViewItem
            {
                Content = "Fetch",
                IconSource = new SymbolIconSource { Symbol = Symbol.Globe },
                Tag = "FetchIP"
            });
            navItems.Add(new NavigationViewItem
            {
                Content = "My IP",
                IconSource = new SymbolIconSource { Symbol = Symbol.MapPin },
                Tag = "MyIP"
            });

            footerNavItems.Add(new NavigationViewItem
            {
                Content = "About",
                IconSource = new SymbolIconSource { Symbol = Symbol.ContactInfo },
                Tag = "About"
            });

            NavView!.MenuItems = navItems;
            NavView!.FooterMenuItems = footerNavItems;

            NavView!.SelectionChanged += NavigationViewChanged!;
            NavView.SelectedItem = NavView.MenuItems.ElementAt(0);
        }

        private void NavigationViewChanged(object sender, NavigationViewSelectionChangedEventArgs e)
        {
            if (e.SelectedItem is not NavigationViewItem nvi) return;
            var smpPage = $"CheckIP.Avalonia.Pages.{nvi.Tag}";
            var pg = Activator.CreateInstance(Type.GetType(smpPage)!);
            (sender as NavigationView)!.Content = pg;
        }
    }
}