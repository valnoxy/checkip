using System.Windows;

namespace CheckIP.Common
{
    public class LocalizationManager
    {    
        public static string LocalizeError(string errorMessage)
        {
            switch (errorMessage)
            {
                case "Reserved range":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        errorMessage = (string)Application.Current.MainWindow.FindResource("ErrorReservedRange");
                    });
                    if (string.IsNullOrEmpty(errorMessage))
                        errorMessage = "Error: Reserved range";
                    break;
                case "invalid query":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        errorMessage = (string)Application.Current.MainWindow.FindResource("ErrorInvalidQuery");
                    });
                    if (string.IsNullOrEmpty(errorMessage))
                        errorMessage = "Error: Invalid query";
                    break;
            }
            return errorMessage;

        }

        public static string LocalizeValue(string value)
        {
            var localizedValue = string.Empty;
            switch (value)
            {
                case "True":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        localizedValue = (string)Application.Current.MainWindow.FindResource("True");
                    });
                    break;
                case "False":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        localizedValue = (string)Application.Current.MainWindow.FindResource("False");
                    });
                    break;
                case "All rights reserved.":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        localizedValue = (string)Application.Current.MainWindow.FindResource("AllRightsReserved");
                    });
                    break;
                case "by":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        localizedValue = (string)Application.Current.MainWindow.FindResource("By");
                    });
                    break;
            }
            return string.IsNullOrEmpty(localizedValue) ? value : localizedValue;
        }
    }
}
