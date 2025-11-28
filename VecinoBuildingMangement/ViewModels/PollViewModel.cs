using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class PollViewModel
    {
        public Poll poll { get; set; }
        public List<OptionViewModel> options { get; set; }
        public PollViewModel()
        {
            options = new List<OptionViewModel>();
        }

    }
}
