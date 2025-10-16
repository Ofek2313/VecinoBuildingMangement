using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class ManageFinnaceViewModel
    {
        public double FeesAmount { get; set; }
        public List<Fee> Fees { get; set; }

    }
}
