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
    public class PrecentageToWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
          
            if(value is double percentage)
            {
                if (parameter?.ToString() == "Second")
                    return new GridLength(100 - percentage, GridUnitType.Star);
                return new GridLength(percentage, GridUnitType.Star);
            }
         
            return "60*";
            
        }
       
            
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
