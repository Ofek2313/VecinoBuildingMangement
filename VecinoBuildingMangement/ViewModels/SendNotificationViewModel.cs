using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class SendNotificationViewModel
    {
        public Notification Notification { get; set; }
        public List<string> ResidentIds { get; set; }
    }
}
