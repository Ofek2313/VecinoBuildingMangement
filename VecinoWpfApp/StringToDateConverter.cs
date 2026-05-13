using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VecinoWpfApp
{
    public class StringToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && DateTime.TryParseExact(s, "dd/MM/yyyy",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d))
                return d;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime d)
                return d.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            return string.Empty; 
        }
    }
}
