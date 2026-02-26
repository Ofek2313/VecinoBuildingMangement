using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VecinoWpfApp
{
    public class RequestStatusConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if(value is string Status)
            {
                switch(Status)
                {
                    case "Pending":
                        return "Start Work";
                    case "In Progress":
                        return "Finish Work";
                    case "Completed":
                        return "Archive";
                }
            }
           
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
