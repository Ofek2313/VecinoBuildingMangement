using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.DTO
{
    public class BuildingStats
    {
        public int UnpaidFees {  get; set; }

        public int OpenRequests { get; set; }

        public int OpenPolls { get; set; }

        public int Occupancy {get; set; }

        public int PaidFees { get; set; }

        public int NotificationsSentThisMonth { get; set; }
    }
}
