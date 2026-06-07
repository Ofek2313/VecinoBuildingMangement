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
    public class RequestStatusConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter is string parm)
            {
                if(parm == "text")
                {
                    if (value is string stats)
                    {
                        
                            switch (stats)
                            {
                                case "Completed":
                                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b8dfc0"));


                                case "In Progress":
                                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c5d8f8"));

                                case "Pending":
                                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffd4a8"));
                        }
                        
                    }
                }
                else if(parm == "background")
                {
                    if (value is string stats)
                    {

                        switch (stats)
                        {
                            case "Completed":
                                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e8f5e9"));


                            case "In Progress":
                                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e8f0fe"));

                            case "Pending":
                                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fff0e0"));
                        }

                    }
                }
            }

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
