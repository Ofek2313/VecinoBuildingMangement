using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.DTO
{
    public class UpdateBookingStatusDto
    {
        public string Bookingid { get; set; }

        public BookingStatus BookingStatus { get; set; }
    }
}
