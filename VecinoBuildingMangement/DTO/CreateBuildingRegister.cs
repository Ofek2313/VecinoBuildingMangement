using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.DTO
{
    public class CreateBuildingRegister
    {

        public Building Building { get; set; } = new Building();

        public Resident Resident { get; set; }


    }
}
