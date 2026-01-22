using System.Data;
using System.Text;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class EventRepository : GenericRepository<Event>, IRepository<Event>
    {
        public EventRepository(DbHelperOleDb dbHelperOleDb)
            : base(dbHelperOleDb) { }
        //public bool Create(Event model)
        //{
        //    string sql = @$"Insert Into Event(EventDate,EventTitle,EventDescription,EventTypeId,BuildingId)
        //                    Values(@EventDate,@EventTitle,@EventDescription,
        //                           @EventTypeId,@BuildingId)";
        //    this.dbHelperOleDb.AddParameter("@EventDate", model.EventDate);
        //    this.dbHelperOleDb.AddParameter("@EventTitle", model.EventTitle);
        //    this.dbHelperOleDb.AddParameter("@EventDescription", model.EventDescription);
        //    this.dbHelperOleDb.AddParameter("@EventTypeId", model.EventTypeId);
        //    this.dbHelperOleDb.AddParameter("@BuildingId", model.BuildingId);
           
        //    return this.dbHelperOleDb.Insert(sql) > 0;
        //}

        //public bool Delete(string id)
        //{
        //    string sql = @"Delete from Event where EventId=@EventId";
        //    this.dbHelperOleDb.AddParameter("@EventId", id);
        //    return this.dbHelperOleDb.Delete(sql) > 0;
        //}

        //public List<Event> GetAll()
        //{
        //    string sql = "Select * From Event";

        //    List<Event> events = new List<Event>();
        //    using (IDataReader reader = this.dbHelperOleDb.Select(sql))
        //    {
        //        while (reader.Read())
        //        {

        //            events.Add(this.modelCreators.EventCreator.CreateModel(reader));

        //        }
        //    }

        //    return events;
        //}

        //public Event GetById(string id)
        //{
        //    string sql = "Select * From Event Where EventId=@EventId";
        //    dbHelperOleDb.AddParameter("@EventId", id);

        //    using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
        //    {
        //        dataReader.Read();
        //        return this.modelCreators.EventCreator.CreateModel(dataReader);
        //    }
        //}

        //public bool Update(Event model)
        //{
        //    string sql = @$"Update Event set EventDate = @EventDate,EventTitle=@EventTitle,
        //                    EventDescription=@EventDescription,EventTypeId=@EventTypeId,BuildingId=@BuildingId";
        //    this.dbHelperOleDb.AddParameter("@EventDate", model.EventDate);
        //    this.dbHelperOleDb.AddParameter("@EventTitle", model.EventTitle);
        //    this.dbHelperOleDb.AddParameter("@EventDescription", model.EventDescription);
        //    this.dbHelperOleDb.AddParameter("@EventTypeId", model.EventTypeId);
        //    this.dbHelperOleDb.AddParameter("@BuildingId", model.BuildingId);

        //    return this.dbHelperOleDb.Update(sql) > 0;
        //}
        public List<Event> GetEventByBuildingId(string buildingId)
        {
            string sql = "Select * From Event Where BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);

            List<Event> events = new List<Event>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    events.Add(this.ModelCreator.CreateModel(reader));

                }
            }

            return events;
        }
        private bool HasHappend(string date)
        {
            string[] date1Split = date.Split('/');
            string[] date2Split = DateTime.Now.ToString("dd/MM/yyyy").Split('/');

            if (Convert.ToUInt32(date1Split[0]) < Convert.ToUInt32(date2Split[0]))
                return true;
            if (Convert.ToUInt32(date1Split[1]) < Convert.ToUInt32(date2Split[1]))
                return true;
            if (Convert.ToUInt32(date1Split[2]) < Convert.ToUInt32(date2Split[2]))
                return true;
            return false;
        }
        public List<Event> GetPreviousEventsByBuildingId(string buildingId)
        {
            string sql = "Select * From Event Where BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);

            List<Event> events = new List<Event>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {
                    if(HasHappend(this.ModelCreator.CreateModel(reader).EventDate))
                        events.Add(this.ModelCreator.CreateModel(reader));

                }
            }

            return events;
        }
        public List<Event> GetCurrentEventsByBuildingId(string buildingId)
        {
            string sql = "Select * From Event Where BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);

            List<Event> events = new List<Event>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {
                    if (!HasHappend(this.ModelCreator.CreateModel(reader).EventDate))
                        events.Add(this.ModelCreator.CreateModel(reader));

                }
            }

            return events;
        }
    }
    
}
