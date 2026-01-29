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
        public List<EventViewModel> CurrEvents { get; set; } = new List<EventViewModel>();
        public List<EventViewModel> PreEvents { get; set; } = new List<EventViewModel>();
    }
}
