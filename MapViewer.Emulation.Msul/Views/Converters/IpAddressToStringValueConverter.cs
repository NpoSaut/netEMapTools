using System;
using System.Globalization;
using System.Net;
using System.Windows.Data;

namespace MapViewer.Emulation.Msul.Views.Converters
{
    public class IpAddressToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return IPAddress.Parse((string)value); }
    }
}
