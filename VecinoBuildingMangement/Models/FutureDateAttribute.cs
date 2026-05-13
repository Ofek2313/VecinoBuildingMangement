using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VecinoBuildingMangement.Models
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return true;
            }
            if (DateTime.TryParseExact(value.ToString(), "dd/MM/yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture,
                                           System.Globalization.DateTimeStyles.None,
                                           out DateTime parsedDate))
            {
                return parsedDate.Date >= DateTime.Today;
            }


            return false;
        }
    }
}
