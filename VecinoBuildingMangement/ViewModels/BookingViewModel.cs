using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class BookingViewModel
    {
        public List<Booking> Bookings { get; set; }
        public string SelectedDate { get; set; }
    }
}
