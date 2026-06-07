using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.DTO
{
    public class JoinBuildingRequest
    {
        public string BuildingCode {  get; set; }
        public string ResidentId { get; set; }  

        public int UnitNumber { get; set; }
    }
}
