using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class ViewEventViewModel
    {
        public List<EventViewModelResident> CurrEvents { get; set; } = new List<EventViewModelResident>();
        public List<EventViewModelResident> PreEvents { get; set; } = new List<EventViewModelResident>();

        public List<EventTypes> EventTypes { get; set; }
    }
}
