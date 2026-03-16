using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.ViewModels
{
    public class ManageAdminFinance
    {
        public List<ResidentFeeViewModel> Finances { get; set; } = new List<ResidentFeeViewModel>();

        public double TotalCollected { get; set; }

        public double TotalCollectedCurrentMonth { get; set; }

        public int TotalPaid { get; set; }

        public int TotalUnPaid { get; set; }

        public int CollectionRate { get; set; }



    }
}
