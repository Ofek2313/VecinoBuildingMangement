using Microsoft.AspNetCore.Mvc;
using System.Data;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService.ORM.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IRepository<Booking>
    {
    
        public BookingRepository(DbHelperOleDb dbHelperOleDb, ModelCreator modelCreator)
            : base(dbHelperOleDb, modelCreator) { }

        public bool CreateBooking([FromBody] Booking booking)
        {
            string sql = $@"INSERT INTO Booking (ResidentId, EndTime, StartTime, BookingDate)
                            SELECT 
                                @ResidentId,
                                @EndTime,
                                @StartTime,
                                @BookingDate
                            FROM (SELECT COUNT(*) FROM MSysObjects)
                            WHERE NOT EXISTS (
                                SELECT 1
                                FROM Booking AS B
                                WHERE B.BookingDate = @BookingDate
                                  AND @StartTime < B.EndTime
                                  AND @EndTime > B.StartTime
                            ); ";
            this.dbHelperOleDb.AddParameter("@ResidentId", booking.ResidentId);
            this.dbHelperOleDb.AddParameter("@EndTime", booking.EndTime);
            this.dbHelperOleDb.AddParameter("@StartTime", booking.StartTime);
            this.dbHelperOleDb.AddParameter("@BookingDate", booking.BookingDate);
            this.dbHelperOleDb.AddParameter("@BookingDate", booking.BookingDate);
            this.dbHelperOleDb.AddParameter("@StartTime", booking.StartTime);
            this.dbHelperOleDb.AddParameter("@EndTime", booking.EndTime);
            return this.dbHelperOleDb.Insert(sql) > 0;

        }
        public List<Booking> GetBookingsByBuildingId(string buildingId,string date)
        {
            string sql = $@"SELECT
                        Booking.*
                    FROM
                        Building
                        INNER JOIN (
                            Booking
                            INNER JOIN Resident ON Booking.ResidentId = Resident.ResidentId
                        ) ON Building.BuildingId = Resident.BuildingId
                    WHERE
                        Building.BuildingId = @BuildingId and Booking.BookingDate = @BookingDate; ";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            this.dbHelperOleDb.AddParameter("@BookingDate", date);
            List<Booking> bookings = new List<Booking>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    bookings.Add(this.modelCreator.CreateModel<Booking>(reader));

                }
            }

            return bookings;
        }
    }
}
