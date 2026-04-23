using Microsoft.Extensions.Options;
using System.Data;
using System.Text;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{
    public class EventRepository : GenericRepository<Event>, IRepository<Event>
    {
        public EventRepository(DbHelperOleDb dbHelperOleDb, ModelCreator modelCreator)
            : base(dbHelperOleDb,modelCreator) { }
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
        public bool AttendEvent(string eventId,string residentId)
        {
            string sql = @"INSERT INTO EventAttendance (ResidentId, EventId) VALUES (@ResidentId, @EventId)";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            this.dbHelperOleDb.AddParameter("@EventId", eventId);
            return this.dbHelperOleDb.Insert(sql) > 0;
        }
        public bool UnAttendEvent(string eventId, string residentId)
        {
            string sql = @"Delete From EventAttendance where ResidentId = @ResidentId AND EventId = @EventId";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            this.dbHelperOleDb.AddParameter("@EventId", eventId);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }
        public List<Event> GetEventByBuildingId(string buildingId)
        {
            string sql = "Select * From Event Where BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);

            List<Event> events = new List<Event>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {
                    if (!HasHappend(this.modelCreator.CreateModel<Event>(reader).EventDate))
                        events.Add(this.modelCreator.CreateModel<Event>(reader));

                }
            }

            return events;
        }
        private bool HasHappend(string date)
        {
            string[] date1Split = date.Split('/');
            string[] date2Split = DateTime.Now.ToString("dd/MM/yyyy").Split('/');

            int day1 = Convert.ToInt32(date1Split[0]);
            int month1 = Convert.ToInt32(date1Split[1]);
            int year1 = Convert.ToInt32(date1Split[2]);

            int day2 = Convert.ToInt32(date2Split[0]);
            int month2 = Convert.ToInt32(date2Split[1]);
            int year2 = Convert.ToInt32(date2Split[2]);

            if (year1 < year2)
                return true;
            else if (year2 < year1)
                return false;
            if (month1 < month2)
                return true;
            else if (month2 < month1)
                return false;
            if (day1 < day2)
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
                    if(HasHappend(this.modelCreator.CreateModel<Event>(reader).EventDate))
                        events.Add(this.modelCreator.CreateModel<Event>(reader));

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
                    if (!HasHappend(this.modelCreator.CreateModel<Event>(reader).EventDate))
                        events.Add(this.modelCreator.CreateModel<Event>(reader));

                }
            }

            return events;
        }

        public int GetAttendingCount(string eventId)
        {
            string sql = "SELECT COUNT(*) FROM EventAttendance WHERE EventId = @EventId";
            this.dbHelperOleDb.AddParameter("@EventId", eventId);
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    return Convert.ToInt32(reader[0]);

                }
            }
            return 0;
        }
        public string GetPhotoById(string eventId)
        {
            string sql = "Select EventImage From [Event] Where EventId = @EventId";
            this.dbHelperOleDb.AddParameter("@EventId", eventId);
            using (IDataReader dataRedaer = this.dbHelperOleDb.Select(sql))
            {
                if (dataRedaer.Read())
                {
                    return Convert.ToString(dataRedaer["EventImage"]);
                }
                return null;
            }
        }
        public bool UpdatePhotoById(string eventId, string extension)
        {
            string sql = $"Update Event SET EventImage = 'event{eventId}.{extension}' WHERE EventId = @EventId";
            this.dbHelperOleDb.AddParameter("@EventId", eventId);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public List<string> GetResidentsAttendingEventByEventId(string eventId)
        {
            string sql = $"SELECT Resident.ResidentName FROM Resident INNER JOIN (Event INNER JOIN EventAttendance ON Event.EventId = EventAttendance.EventId) ON Resident.ResidentId = EventAttendance.ResidentId WHERE  Event.EventId = @EventId";
            this.dbHelperOleDb.AddParameter("@EventId", eventId);
            List<string> ResidentNames = new List<string>();
            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                while (dataReader.Read())
                {
                    ResidentNames.Add(Convert.ToString(dataReader["ResidentName"]));
                }
            }
            return ResidentNames;
        }

        public List<EventViewModelResident> GetEventViewModelsByBuildingIdResident(string buildingId, string residentId)
        {
            string sql = @"SELECT e.EventId,e.EventDate, e.EventTitle, e.EventDescription, e.EventTypeId, e.EventImage, e.StartTime, e.EndTime, e.BuildingId, COUNT(a.EventId) AS AttendingCount, MAX(IIf(a.ResidentId = @ResidentId, 1, 0)) AS IsAttending FROM Event e LEFT JOIN EventAttendance a ON e.EventId = a.EventId WHERE e.BuildingId = @BuildingId GROUP BY e.EventId, e.EventDate, e.EventTitle, e.EventDescription, e.EventTypeId, e.EventImage, e.StartTime, e.EndTime, e.BuildingId";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            List<EventViewModelResident> eventViewModels = new List<EventViewModelResident>();
            using(IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                while(dataReader.Read())
                {
                    EventViewModelResident eventViewModel = new EventViewModelResident
                    {
                        Event = this.modelCreator.CreateModel<Event>(dataReader),
                        Attending = Convert.ToInt32(dataReader["AttendingCount"]),
                        IsAttending = Convert.ToBoolean(dataReader["IsAttending"])
                       
                    };
                    eventViewModels.Add(eventViewModel);
                }
            }
            return eventViewModels;
        }

        public List<EventViewModel> GetEventViewModelsByBuildingId(string buildingId)
        {
            string sql = @"SELECT e.EventId,e.EventDate, e.EventTitle, e.EventDescription, e.EventTypeId, e.EventImage, e.StartTime, e.EndTime, e.BuildingId, COUNT(a.EventId) AS AttendingCount FROM Event e LEFT JOIN EventAttendance a ON e.EventId = a.EventId WHERE e.BuildingId = @BuildingId GROUP BY e.EventId, e.EventDate, e.EventTitle, e.EventDescription, e.EventTypeId, e.EventImage, e.StartTime, e.EndTime, e.BuildingId";
        
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            List<EventViewModel> eventViewModels = new List<EventViewModel>();
            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                while (dataReader.Read())
                {
                    EventViewModel eventViewModel = new EventViewModel
                    {
                        Event = this.modelCreator.CreateModel<Event>(dataReader),
                        Attending = Convert.ToInt32(dataReader["AttendingCount"]),
                        
                        
                    };
                    eventViewModels.Add(eventViewModel);
                }
            }
            return eventViewModels;
        }
        
    }
    
}
