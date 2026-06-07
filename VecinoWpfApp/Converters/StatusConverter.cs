using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VecinoBuildingMangement.Models;
using VecinoWpfApp.UserControls;

namespace VecinoWpfApp
{
    public class StatusConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string pollDate = (string)values[0];
            bool IsActive = (bool)values[1];

            if(string.IsNullOrWhiteSpace(pollDate))
                return "Unknown";

            DateTime pollDateTime = DateTime.ParseExact(pollDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
           
            return IsActive && pollDateTime >= DateTime.Today ? "Active" : "Closed";
            
        }

     
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
