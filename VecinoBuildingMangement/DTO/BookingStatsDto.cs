using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.DTO
{
    public class BookingStatsDto
    {
        public int AwaitingApproval {  get; set; }

        public int AwaitingPayment { get; set; }

        public int Confirmed { get; set; }

        public int Rejected { get; set; }
    }
}
