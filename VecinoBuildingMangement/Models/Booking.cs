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
        string bookingNote;

        public string? BookingId
        {
            get { return this.bookingId; }
            set { this.bookingId = value; }
        }

  
        public string ResidentId
        {
            get { return this.residentId; }
            set { this.residentId = value; }
        }

        [Required(ErrorMessage = "End Time is Required")]
        public string EndTime
        {
            get { return this.endTime; }
            set { this.endTime = value; ValidateProperty(value, "EndTime"); }
        }

        [Required(ErrorMessage = "Start Time is Required")]
        public string StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; ValidateProperty(value, "StartTime"); }

        }

        [Required(ErrorMessage = "Date is Required")]
        [FutureDate(ErrorMessage = "Please enter a date that is today or in the future")]
        public string BookingDate
        {
            get { return this.bookingDate; }
            set
            {
                if (IsValidationEnabled)
                    ValidateProperty(value, "BookingDate");
                this.bookingDate = value;
            }
        }
        public BookingStatus BookingStatus { get; set; }

        [Required(ErrorMessage = "Note Is Required")]
        public string BookingNote
        {
            get { return this.bookingNote; }
            set
            {
                
                ValidateProperty(value,"BookingNote");
                this.bookingNote = value;
            }
        }
    }
}