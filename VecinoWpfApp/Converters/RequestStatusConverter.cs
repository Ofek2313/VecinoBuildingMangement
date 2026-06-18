using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using VecinoBuildingMangement;

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
                    if (value is RequestStatus stats)
                    {
                        
                            switch (stats)
                            {
                                case RequestStatus.Completed:
                                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b8dfc0"));


                                case RequestStatus.InProgress:
                                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c5d8f8"));

                                case RequestStatus.Pending:
                                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffd4a8"));
                        }
                        
                    }
                }
                else if(parm == "background")
                {
                    if (value is RequestStatus stats)
                    {

                        switch (stats)
                        {
                            case RequestStatus.Completed:
                                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e8f5e9"));


                            case RequestStatus.InProgress:
                                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e8f0fe"));

                            case RequestStatus.Pending:
                                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fff0e0"));
                        }

                    }
                }
            }

            if(value is RequestStatus Status)
            {
                switch(Status)
                {
                    case RequestStatus.Pending:
                        return "Start Work";
                    case RequestStatus.InProgress:
                        return "Finish Work";
                    case RequestStatus.Completed:
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
