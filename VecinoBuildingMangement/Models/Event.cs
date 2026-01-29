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
        string buildingId;
        

        public Event() { }
        public string EventId
        {
            get { return eventId; }
            set { eventId = value; }
        }

        [Required(ErrorMessage ="Event Date can not be empty")]
        [Date(ErrorMessage ="Date must be a valid date")]
        public string EventDate
        {
            get { return eventDate; }
            set { eventDate = value;}
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

        
       
        public string EventTypeId
        {
            get { return eventTypeId; }
            set { eventTypeId = value; }
        }

   
        public string BuildingId
        {
            get { return buildingId; }
            set { buildingId = value; }
        }

       
    }
}
