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
        public List<Event> CurrEvents { get; set; }
        public List<Event> PreEvents { get; set; }
    }
}
