using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class Event
    {
        string eventId;
        string eventDate;
        string eventTitle;
        string eventDescription;
        string eventTypeId;
        string buildingId;

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
            set { eventDate = value; }
        }
        [StringLength(50,MinimumLength =5,ErrorMessage ="Title must be between 5-50 characters")]
        [Required(ErrorMessage = "Event Title can not be empty")]
        public string EventTitle
        {
            get { return eventTitle; }
            set { eventTitle = value; }
        }
        [StringLength(70, MinimumLength = 5, ErrorMessage = "Description must be between 5-70 characters")]
        [Required(ErrorMessage = "Event Description can not be empty")]
        public string EventDescription
        {
            get { return eventDescription; }
            set { eventDescription = value; }
        }

        [RegularExpression(@"[0-9]", ErrorMessage = "Event Type Id Must Be Digits")]
        [NotZero(ErrorMessage ="Event Type Id needs to be bigger than 0")]
        [Required(ErrorMessage = "Event Type Id can not be empty")]
        public string EventTypeId
        {
            get { return eventTypeId; }
            set { eventTypeId = value; }
        }

        [RegularExpression(@"[0-9]", ErrorMessage = "Building Id Must Be Digits")]
        [Required(ErrorMessage = "Building Id can not be empty")]
        [NotZero(ErrorMessage = "Building Id needs to be bigger than 0")]
        public string BuildingId
        {
            get { return buildingId; }
            set { buildingId = value; }
        }

    }
}
