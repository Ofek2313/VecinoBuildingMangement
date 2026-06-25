using Microsoft.Extensions.Options;
using System.Data;
using System.Text;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{
    public class EventRepository : GenericRepository<Event>
    {
        public EventRepository(DbHelperOleDb dbHelperOleDb, ModelCreator modelCreator)
            : base(dbHelperOleDb,modelCreator) { }
       
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
        public List<Event> GetEventByBuildingIdTop(string buildingId)
        {
            string sql = "Select TOP 5 * From Event Where BuildingId = @BuildingId";
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
        public List<ResidentSummaryDTO> GetResidentsAttendingEventByEventId(string eventId)
        {
            string sql = @$"SELECT Resident.ResidentId,Resident.ResidentName,Resident.ResidentImage FROM Resident
                        INNER JOIN (Event INNER JOIN EventAttendance ON Event.EventId = EventAttendance.EventId)
                        ON Resident.ResidentId = EventAttendance.ResidentId WHERE  Event.EventId = @EventId";
            this.dbHelperOleDb.AddParameter("@EventId", eventId);
            List<ResidentSummaryDTO> residents = new List<ResidentSummaryDTO>();
            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                while (dataReader.Read())
                {
                    residents.Add(new ResidentSummaryDTO()
                    {
                        ResidentName = Convert.ToString(dataReader["ResidentName"]),
                        ResidentImage = Convert.ToString(dataReader["ResidentImage"])
                    });
                }
            }
            return residents;
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
        public NextEventViewModel GetNextEvenByBuildingId(string buildingId)
        {
            string sql = @"SELECT
                        TOP 1 Event.EventTitle,
                        Event.EventDate
                    FROM
                        [Event]
                    WHERE
                        BuildingId = @BuildingId
                        AND CDate (EventDate) >= Date()
                    ORDER BY
                        CDate (EventDate) ASC;";

            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            NextEventViewModel nextEvent = new NextEventViewModel();
            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                if (dataReader.Read())
                {
                    return this.modelCreator.CreateModel<NextEventViewModel>(dataReader);
                }

            }
            return nextEvent;
        }
    }
    
}
