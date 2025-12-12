using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class EventTypes : Model
    {
        string eventTypeId;
        string eventTypeName;

        public EventTypes() { }

        public string EventTypeId 
        {
            get { return eventTypeId; }
            set { eventTypeId = value; }
        }

        [Required(ErrorMessage ="Event Type Name can not be empty")]
        [StringLength(30,MinimumLength =3,ErrorMessage ="Event Type Name needs to be between 3-30 characters")]
        public string EventTypeName
        {
            get { return eventTypeName; }
            set { eventTypeName = value; }
        }
    }
}
