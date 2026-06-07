using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VecinoWpfApp
{
    class ButtonRoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ( value is bool IsAdmin)
            {
                if (!IsAdmin)
                    return "Appoint as Admin";
                return "Revoke Admin";
            }
            return "Toggle Admin";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
