using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.DTO
{
    public class CreateBuildingDto
    {
        public Building Building { get; set; }
        
        public string ResidentId { get; set; }


    }
}
