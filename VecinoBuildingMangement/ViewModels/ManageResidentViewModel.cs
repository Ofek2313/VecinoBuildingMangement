using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class ManageResidentViewModel
    {
        public List<Resident> Residents { get; set; }
        public int TotalResidents { get; set; }
        public int TotalUnits {  get; set; }
    }
}
