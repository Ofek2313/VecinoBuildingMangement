using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.DTO
{
    public class CreateBuildingDto
    {
        public string Address { get; set; }    

        public string CityId { get; set; }

        public string BuildingImage { get; set; }

        public int TotalUnits { get; set; }

        public int Floors { get; set; }

        public string Entrance { get; set; }

        public string Code { get; set; }   


    }
}
