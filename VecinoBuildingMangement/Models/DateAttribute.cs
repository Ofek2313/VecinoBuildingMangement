using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class DateAttribute : ValidationAttribute
    {
        private bool isDigit(string value)
        {
            
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] < '0' || value[i] > '9')
                    return false;
            }
            return true;

        }
        public override bool IsValid(object? value)
        {

            string date = value.ToString();
            string[] dates = date.Split('/');

            if (dates.Length != 3)
                return false;
            if (!(this.isDigit(dates[0]) && this.isDigit(dates[1]) && this.isDigit(dates[2]))) return false;
            if (int.Parse(dates[0]) < 0 || int.Parse(dates[0]) > 31) return false;
            if (int.Parse(dates[1]) < 0 || int.Parse(dates[1]) > 12) return false;
            if (dates[2].Length != 4) return false;

            return true;

            //if (value == null) return false;
            //return DateTime.TryParse(value.ToString(), out _); 


        }
    }
}
