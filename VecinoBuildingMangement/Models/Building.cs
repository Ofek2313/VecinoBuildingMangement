using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VecinoBuildingMangement.Models
{
    public class Building
    {
        string buildingId;
        string cityId;
        string address;
        string entranceCode;
        int totalUnits;
        int floors;
        string joinCode;

        public string BuildingId
        {
            get { return buildingId; }
            set { buildingId = value; }
        }


        public string CityId
        { 
            get { return cityId; }
            set { cityId = value; } 
        }

        [Required(ErrorMessage ="Address Can Not Be Empty")]
        [StringLength(100,MinimumLength =5,ErrorMessage ="Address Can Not Be Over 100 Characters")]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        [Required(ErrorMessage = "Entrance Code Can Not Be Empty")]
        public string EntranceCode
        {
            get { return entranceCode; }
            set { entranceCode = value; }
        }
        [Required(ErrorMessage = "Total Units Can Not Be Empty")]
        public int TotalUnits
        {
            get { return totalUnits; }
            set { totalUnits = value; }
        }
        [Required(ErrorMessage = "Number Of Floors Can Not Be Empty")]
        [Range(1,200,ErrorMessage = "Number Of Floors Must Be Bettwen 1-200")]
        public int Floors
        {
            get { return floors; }
            set { floors = value; }

        }

        [Required(ErrorMessage = "Join Code Can Not Be Empty")]
        public string JoinCode
        {
            get { return joinCode; }
            set { joinCode = value; }
        }
    }
}
