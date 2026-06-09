using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class OptionViewModel
    {
        public Option option { get; set; }

        public int voted { get; set; } = 0;

        public double percentage { get; set; } = 0;

        public List<ResidentSummaryDTO> residentsVoted { get; set; } = new List<ResidentSummaryDTO>();
    }
}
