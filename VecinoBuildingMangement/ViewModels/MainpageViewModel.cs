using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class MainpageViewModel
    {
        public Building Building { get; set; }
        public List<Notification> Notifications { get; set; } 
        public List<Event> events { get; set; }
    }
}
