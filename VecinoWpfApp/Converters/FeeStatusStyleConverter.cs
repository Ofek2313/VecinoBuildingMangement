using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace VecinoWpfApp
{
    public class FeeStatusStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           if(value is bool status)
            {
                string paramter = parameter.ToString();

                if (parameter.Equals("Badge"))
                {
                    switch (status)
                    {
                        case true:
                            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dcfce7"));


                        case false:
                            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fee2e2"));
                    }
                }
                else if(parameter.Equals("Text"))
                {
                    switch (status)
                    {
                        case true:
                            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#16a34a"));
                        case false:
                            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e11d48"));

                    }
                }
                else if (parameter.Equals("Label"))
                {
                    return status ? "Paid" : "Unpaid";
                }

            }
            return DependencyProperty.UnsetValue;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
