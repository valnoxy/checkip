﻿using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Media.Imaging;
using Hardcodet.Wpf.TaskbarNotification;

namespace CheckIP.Common
{
    public static class TaskBar
    {
        public static TaskbarIcon TbIcon;

        public static void Initialize()
        {
            TbIcon = new TaskbarIcon();
            TbIcon.IconSource = new BitmapImage(new Uri("pack://application:,,,/Assets/CheckIP.ico"));
            TbIcon.ToolTipText = "CheckIP";
        }

        public static void Update(string country, string city, IPAddress ip, string countryCode)
        {
            if (country == null) throw new ArgumentNullException(nameof(country));
            if (city == null) throw new ArgumentNullException(nameof(city));
            if (ip == null) throw new ArgumentNullException(nameof(ip));
            if (countryCode == null) throw new ArgumentNullException(nameof(countryCode));

            try
            {
                TbIcon.IconSource =
                    new BitmapImage(new Uri($"pack://application:,,,/Assets/{countryCode.ToLower()}.ico"));
                TbIcon.ToolTipText = $"{country} - {city} ({ip})";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static BitmapImage Create(string path)
        {
            var img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            img.EndInit();
            img.Freeze();
            return img;
        }
    }

}
