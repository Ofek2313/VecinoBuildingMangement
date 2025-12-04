using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class ManagePolls
    {
        public int PollNumbers { get; set; }
        public double ParticipationRate { get; set; }

        public List<PollViewModel> PollviewModel { get; set; }


       
        
    }
}
