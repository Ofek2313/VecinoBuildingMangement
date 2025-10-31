using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class EventCreator : IModelCreator<Event>
    {
        public Event CreateModel(IDataReader dataReader) 
        {
         
            Event @event = new Event();
            @event.EventId = Convert.ToString(dataReader["EventId"]);
            @event.EventTitle = Convert.ToString(dataReader["EventTitle"]);
            @event.EventDescription = Convert.ToString(dataReader["EventDescription"]);
            @event.EventTypeId = Convert.ToString(dataReader["EventTypeId"]);
            @event.BuildingId = Convert.ToString(dataReader["BuildingId"]);

            return @event;

        }
    }
}
