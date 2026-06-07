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
        public List<Booking> Bookings { get; set; } = new List<Booking>();

        public List<Booking> MyBookings { get; set; } = new List<Booking>();
        public string SelectedDate { get; set; }

        public Booking Booking { get; set; } = new Booking();
    }
}
