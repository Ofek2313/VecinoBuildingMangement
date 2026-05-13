using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.DTO
{
    public class ResidentFeeStats
    {
        public double TotalUnPaidFees { get; set; }

        public double TotalPaidFees { get; set; }

        public int PaidFees { get; set; }

        public int UnPaidFees { get; set; }
    }
}
