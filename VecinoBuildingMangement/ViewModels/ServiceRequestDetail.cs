using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class ServiceRequestDetail
    {
        public ServiceRequest ServiceRequest { get; set; } = new ServiceRequest();

        public string ResidentName { get; set; } = string.Empty;

        public string ResidentEmail {  get; set; } = string.Empty;

        public string ResidentImage {  get; set; } = string.Empty;

        public string RequestTypeName { get; set; } = string.Empty;

    }
}
