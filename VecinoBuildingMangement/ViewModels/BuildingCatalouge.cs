using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class BuildingCatalouge
    {
        public List<Building> Buildings { get; set; }
        public int Page { get; set; }
    }
}
