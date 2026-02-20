using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class CreateEvent
    {
        public Event Event { get; set; }
        public List<EventTypes> eventTypes { get; set; }
    }
}
