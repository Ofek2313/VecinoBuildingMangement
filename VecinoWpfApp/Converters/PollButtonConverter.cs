using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace VecinoWpfApp
{
    internal class PollButtonConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string pollDate = (string)values[0];
            bool IsActive = (bool)values[1];
            string buttonStatus = (string)parameter;
            DateTime pollDateTime = DateTime.ParseExact(pollDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if(buttonStatus.Equals("Close"))
            {
                if(IsActive && pollDateTime >= DateTime.Today)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
           if(buttonStatus.Equals("Open"))
            {
                if (pollDateTime >= DateTime.Today && !IsActive)
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
            return Visibility.Collapsed;
                
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
