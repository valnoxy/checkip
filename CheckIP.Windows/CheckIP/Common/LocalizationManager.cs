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
    }
}
