using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.DTO
{
    public class FeeSummary
    {
        public int TotalPaid { get; set; } 

        public int TotalUnPaid { get; set; }    

        public double TotalCollected { get; set; }

        public double Outstanding { get; set; }
    }
}
