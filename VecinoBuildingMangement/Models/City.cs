using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class City
    {
        string cityId;
        string cityName;

        [Required(ErrorMessage = "City Id is required")]
        [RegularExpression(@"[0-9]", ErrorMessage = "City Id Must Be Digits")]
        [NotZero(ErrorMessage ="City Id must be bigger than zero")]
        public string CityId
        {
            get { return  cityId; }
            set { cityId = value; }
        }

        [Required(ErrorMessage = "City Name is required")]
        [StringLength(50,MinimumLength =3,ErrorMessage ="City Name must be at least 3 characters")]
        public string CityName
        {
            get { return cityName; }
            set { cityName = value; }
        }
    }
}
