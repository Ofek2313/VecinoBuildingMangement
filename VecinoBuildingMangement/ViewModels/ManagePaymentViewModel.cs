using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.DTO;

namespace VecinoBuildingMangement.ViewModels
{
    public class ManagePaymentViewModel
    {

        public ResidentFeeStats ResidentFeeStats { get; set; }

       
        public List<Fee> Fees{ get; set; } = new List<Fee>();

        public List<Fee> UnPaid { get; set; } = new List<Fee>();
        
        public Fee? nextFee { get; set; }

        public int NumberOfPages { get; set; }

        public int NumberOfPagesUnPaid { get; set; }

        public int CurrentPage { get; set; }

        public int CurrentPageUnPaid { get; set; }

        public int DaysLeft { get; set; }
    }
}
