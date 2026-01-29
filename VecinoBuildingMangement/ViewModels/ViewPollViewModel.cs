using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.ViewModels
{
    public class ViewPollViewModel
    {
        public List<PollViewModel> ActivePolls { get; set; } = new List<PollViewModel>();
        public List<PollViewModel> InActivePolls { get; set; } = new List<PollViewModel>();
    }
}
