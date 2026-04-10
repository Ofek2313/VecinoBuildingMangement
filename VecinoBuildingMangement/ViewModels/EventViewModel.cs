using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class EventViewModel
    {
        public Event Event { get; set; }
        public int Attending {  get; set; }

        

        public  EventViewModel Clone()
        {
            return new EventViewModel
            {
                Event = new Event
                {
                    EventId = Event.EventId,
                    EventDate = Event.EventDate,
                    EventTitle = Event.EventTitle,
                    EventDescription = Event.EventDescription,
                    EventTypeId = Event.EventTypeId,
                    EventImage = Event.EventImage,
                    StartTime = Event.StartTime,
                    EndTime = Event.EndTime,
                    BuildingId = Event.BuildingId,
                },
                Attending = this.Attending,
               

            };
        }
    }
}
