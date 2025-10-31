using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class EventTypeCreator : IModelCreator<EventTypes>

    {
        public EventTypes CreateModel(IDataReader dataReader)
        {
            EventTypes eventTypes = new EventTypes();
            eventTypes.EventTypeId = Convert.ToString(dataReader["EventTypeId"]);
            eventTypes.EventTypeName = Convert.ToString(dataReader["EventTypeName"]);

            return eventTypes;
        }

     
    }
}
