using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class Booking : Model
    {
        string bookingId;
        string residentId;
        string endTime;
        string startTime;
        string bookingDate;

        public string? BookingId
        {
            get { return this.bookingId; }
            set { this.bookingId = value; }
        }

        [Required]
        public string ResidentId
        {
            get { return this.residentId; }
            set { this.residentId = value; }
        }

        public string EndTime
        {
            get { return this.endTime; }
            set { this.endTime = value; }
        }

        public string StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }

        }

        public string BookingDate
        {
            get { return this.bookingDate; }
            set
            {
                this.bookingDate = value;
            }
        }
        public BookingStatus BookingStatus { get; set; }

        [Required]
        public string BookingNote { get; set; }
    }
}