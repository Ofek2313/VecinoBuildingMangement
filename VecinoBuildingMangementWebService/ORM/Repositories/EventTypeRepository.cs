using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class EventTypeRepository : Repository, IRepository<EventTypes>
    {
        public EventTypeRepository(DbHelperOleDb dbHelperOleDb, ModelCreators modelCreators)
            : base(dbHelperOleDb, modelCreators) { }
        public bool Create(EventTypes model)
        {
            string sql = @$"Insert Into EventTypes(EventTypeName)
                            Values(@EventTypeName)";
            this.dbHelperOleDb.AddParameter("@EventTypeName", model.EventTypeName);

            return this.dbHelperOleDb.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = @"Delete from EventTypes where EventTypeId=@EventTypeId";
            this.dbHelperOleDb.AddParameter("@EventTypeId", id);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }

        public List<EventTypes> GetAll()
        {
            string sql = "Select * From EventTypes";

            List<EventTypes> eventTypes= new List<EventTypes>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    eventTypes.Add(this.modelCreators.EventTypeCreator.CreateModel(reader));

                }
            }

            return eventTypes;
        }

        public EventTypes GetById(string id)
        {
            string sql = "Select * From EventTypes Where EventTypeId=@EventTypeId";
            dbHelperOleDb.AddParameter("@EventTypeId", id);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                dataReader.Read();
                return this.modelCreators.EventTypeCreator.CreateModel(dataReader);
            }
        }

        public bool Update(EventTypes model)
        {
            string sql = @"Update EventTypes set EventTypeName = @EventTypeName";
            this.dbHelperOleDb.AddParameter("@EventTypeName", model.EventTypeName);

            return this.dbHelperOleDb.Update(sql) > 0;
        }
    }
}
