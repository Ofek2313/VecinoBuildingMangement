using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class AdminMainPage
    {
       

        public Building Building { get; set; }

        public string ResidentName { get; set; }

        public string CityName { get; set; }

        public BuildingStats BuildingStats { get; set; }

        public NextEventViewModel NextEvent { get; set; }

        public List<ActivityViewModel> ActivityViewModels { get; set; }


    }
}
