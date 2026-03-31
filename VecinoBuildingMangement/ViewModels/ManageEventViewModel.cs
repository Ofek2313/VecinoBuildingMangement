using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class ManageEventViewModel
    {
        public List<EventViewModel> Events { get; set; } = new List<EventViewModel>();

        public List<EventViewModel> PastEvents { get; set; } = new List<EventViewModel>();

        public string CurrentMonth { get; set; }
    }
}
