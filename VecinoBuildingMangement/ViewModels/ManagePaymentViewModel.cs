using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class ManagePaymentViewModel
    {
        public double TotalUnPaidFees { get; set; } // Amount of fees t

        public double TotalPaidFees { get; set; }

        public int paidFees { get; set; } // Number of fees that have already been paid
        public int unPaidFees { get; set; }// Number of fees that have not been paid

        public List<Fee> Fees{ get; set; } = new List<Fee>();
        
        public Fee nextFee { get; set; }
    }
}
