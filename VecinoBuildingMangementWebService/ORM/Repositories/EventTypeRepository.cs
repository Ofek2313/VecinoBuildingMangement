using System.Data;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{
    public class EventTypeRepository : GenericRepository<EventTypes>
    {
        public EventTypeRepository(DbHelperOleDb dbHelperOleDb, ModelCreator modelCreator)
            : base(dbHelperOleDb, modelCreator) { }
     
    }
}
