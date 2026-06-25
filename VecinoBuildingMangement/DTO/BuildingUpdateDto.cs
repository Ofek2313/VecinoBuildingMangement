using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.DTO
{
    public class BuildingUpdateDto
    {
        public string BuildingId { get; set; }

        public string Address {  get; set; }

        public string CityId { get; set; }

        public int Floors { get; set; }

        public int TotalUnits { get; set; }

        public string EntranceCode { get; set; }

        public string EntranceName { get; set; }

        public string BuildingImage { get; set; }

        public bool AddressChangedFlag { get; set; }

        public bool PhotoRemoved {  get; set; } 
    }
}
