using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using VecinoBuildingMangement;

namespace VecinoWpfApp.Converters
{
    public class BookingStatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BookingStatus bookingStatus = (BookingStatus)value;

            if (bookingStatus == BookingStatus.AWAITING_APPROVAL)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;

        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
