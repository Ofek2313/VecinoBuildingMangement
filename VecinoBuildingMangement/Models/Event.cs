using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class Event : Model
    {
        string eventId;
        string eventDate;
        string eventTitle;
        string eventDescription;
        string eventTypeId;
        string eventImage;
        string startTime;
        string endTime;
        string buildingId;
        

        public Event() { }
        public string EventId
        {
            get { return eventId; }
            set { if (value == null)
                    eventId = "";
                else
                   eventId = value; 
            }
        }

        [Required(ErrorMessage ="Event Date can not be empty")]
        [Date(ErrorMessage ="Date must be a valid date")]
        [FutureDate(ErrorMessage = "Please enter a date that is today or in the future")]
        public string EventDate
        {
            get { return eventDate; }
            set {
               
                    eventDate = value;
                if (IsValidationEnabled)
                    ValidateProperty(value, "EventDate");
            }
        }
        [StringLength(50,MinimumLength =5,ErrorMessage ="Title must be between 5-50 characters")]
        [Required(ErrorMessage = "Event Title can not be empty")]
        public string EventTitle
        {
            get { return eventTitle; }
            set { eventTitle = value;
                ValidateProperty(value, "EventTitle");
            }
        }

        [StringLength(70, MinimumLength = 5, ErrorMessage = "Description must be between 5-70 characters")]
        [Required(ErrorMessage = "Event Description can not be empty")]
        public string EventDescription
        {
            get { return eventDescription; }
            set { eventDescription = value;
                ValidateProperty(value, "EventDescription");
            }
        }


        [Required(ErrorMessage ="Please Choose An Event Type")]
        public string EventTypeId
        {
            get { return eventTypeId; }
            set { eventTypeId = value; }
        }

        public string EventImage
        {
            get { return eventImage; }
            set {
                if (value == null)
                    eventImage = "event0.png";
                else
                    eventImage = value;
                }
        }

        [Required(ErrorMessage = "Please Choose a Start Time")]
        public string StartTime
        {
            get { return startTime; }
            set { this.startTime = value; ValidateProperty(value, "StartTime"); }
        }

        [Required(ErrorMessage = "Please Choose an End Time")]
        public string EndTime
        {
            get{ return endTime; }
            set { this.endTime = value; ValidateProperty(value, "EndTime"); }
        }

        public string BuildingId
        {
            get { return buildingId; }
            set { buildingId = value; }
        }

       
    }
}
