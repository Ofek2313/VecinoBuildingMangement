using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement
{
    public class CreatePollViewModel
    {
        public List<Option> Options { get; set; } = new List<Option>();
        
        public Poll Poll { get; set; } 

       
    }
}
