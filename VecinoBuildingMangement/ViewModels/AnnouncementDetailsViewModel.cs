using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class AnnouncementDetailsViewModel
    {
        public List<ResidentSummaryDTO> Residents { get; set; } = new List<ResidentSummaryDTO>();

        public int ResidentCount { get; set; }


        public ResidentSummaryDTO Admin { get; set; } = new ResidentSummaryDTO();

        public Notification Notification { get; set; }
    }
}
