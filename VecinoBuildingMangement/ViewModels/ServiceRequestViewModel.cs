using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class ServiceRequestViewModel
    {

        public List<RequestTypes> RequestTypes { get; set; }
        public List<ServiceRequest> serviceRequests { get; set; }

        public int Pending { get; set; } = 0;
        public int InProgress { get; set; } = 0;
        public int Completed { get; set; } = 0;
    }
}
