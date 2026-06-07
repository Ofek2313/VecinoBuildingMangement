using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VecinoBuildingMangement;

namespace VecinoWpfApp.Converters
{
    public class BookingStatusToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BookingStatus bookingStatus = (BookingStatus)value;
            switch (bookingStatus)
            {
                case BookingStatus.AWAITING_APPROVAL:
                    return "Awaiting Approval";
                case BookingStatus.AWAITING_PAYMENT:
                    return "Awaiting Payment";
                case BookingStatus.CONFIRMED:
                    return "Booked";
                case BookingStatus.CANCELED:
                    return "Canceled";
                case BookingStatus.REJECTED:
                    return "Rejected";
                default:
                    return "Pending";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
