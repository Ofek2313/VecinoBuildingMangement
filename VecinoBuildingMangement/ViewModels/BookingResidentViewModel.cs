using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class BookingResidentViewModel
    {
        public Booking Booking { get; set; }

        public string ResidentName { get; set; }

        public string ResidentImage { get; set; }
    }
}
