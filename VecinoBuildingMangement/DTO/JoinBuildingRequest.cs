using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.DTO
{
    public class JoinBuildingRequest
    {
        public string BuildingCode {  get; set; }

        public string ResidentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Unit number must be bigger than 0")]
        public int UnitNumber { get; set; }
    }
}
