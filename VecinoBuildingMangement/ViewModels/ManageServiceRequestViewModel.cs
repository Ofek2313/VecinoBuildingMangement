using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class ManageServiceRequestViewModel
    {
        public List<ServiceRequest> serviceRequests {  get; set; }
        int ServiceRequestNumber;
    }
}
