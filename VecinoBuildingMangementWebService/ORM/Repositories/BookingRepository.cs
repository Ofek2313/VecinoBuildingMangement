using Microsoft.AspNetCore.Mvc;
using System.Data;
using VecinoBuildingMangement;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangementWebService.ORM.ModelCreators;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VecinoBuildingMangementWebService.ORM.Repositories
{
    public class BookingRepository : GenericRepository<Booking>
    {
    
        public BookingRepository(DbHelperOleDb dbHelperOleDb, ModelCreator modelCreator)
            : base(dbHelperOleDb, modelCreator) { }

        //public bool CreateBooking([FromBody] Booking booking)
        //{
        //    string sql = $@"INSERT INTO Booking (ResidentId, EndTime, StartTime, BookingDate)
        //                    SELECT 
        //                        @ResidentId,
        //                        @EndTime,
        //                        @StartTime,
        //                        @BookingDate
        //                    FROM (SELECT COUNT(*) FROM MSysObjects)
        //                    WHERE NOT EXISTS (
        //                        SELECT 1
        //                        FROM Booking AS B
        //                        WHERE B.BookingDate = @BookingDate
        //                          AND @StartTime < B.EndTime
        //                          AND @EndTime > B.StartTime
        //                    ); ";
        //    this.dbHelperOleDb.AddParameter("@ResidentId", booking.ResidentId);
        //    this.dbHelperOleDb.AddParameter("@EndTime", booking.EndTime);
        //    this.dbHelperOleDb.AddParameter("@StartTime", booking.StartTime);
        //    this.dbHelperOleDb.AddParameter("@BookingDate", booking.BookingDate);
        //    this.dbHelperOleDb.AddParameter("@BookingDate", booking.BookingDate);
        //    this.dbHelperOleDb.AddParameter("@StartTime", booking.StartTime);
        //    this.dbHelperOleDb.AddParameter("@EndTime", booking.EndTime);
        //    return this.dbHelperOleDb.Insert(sql) > 0;

        //}
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
        public bool UpdateBookingStauts(string bookingId,BookingStatus bookingStatus)
        {
            string sql = @"Update Booking Set BookingStatus=@BookingStatus where BookingId = @BookingId";
            this.dbHelperOleDb.AddParameter("@BookingStatus", bookingStatus);
            this.dbHelperOleDb.AddParameter("@BookingId", bookingId);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public List<Booking> GetBookingsByResidentId(string residentId)
        {
            string sql = @"Select * From Booking where ResidentId=@ResidentId and BookingStatus <> @BookingStatus and Cdate(BookingDate) >= Date()";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            this.dbHelperOleDb.AddParameter("@BookingStauts", BookingStatus.CANCELED);
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
        public List<BookingResidentViewModel> GetBookingsByBuildingId(string residentId)
        {
            string sql = @"SELECT
                            Booking.*,
                            Resident.ResidentName,
                            Resident.ResidentImage
                        FROM
                            Booking
                            INNER JOIN (
                                Building
                                INNER JOIN Resident ON Building.BuildingId = Resident.BuildingId
                            ) ON Booking.ResidentId = Resident.ResidentId
                        WHERE
                            Building.BuildingId = @BuildingId;";
            this.dbHelperOleDb.AddParameter("@BuildingId", residentId);
            List<BookingResidentViewModel> bookings = new List<BookingResidentViewModel>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    bookings.Add(new BookingResidentViewModel { Booking = this.modelCreator.CreateModel<Booking>(reader),
                                                                ResidentName = Convert.ToString( reader["ResidentName"])
                                                                ,ResidentImage = Convert.ToString(reader["ResidentImage"]) } );

                }
            }

            return bookings;
        }

        public bool CheckOverLap(string bookingId)
        {
            string sql = @"SELECT 1
                            FROM Booking AS b1, Booking AS b2
                            WHERE b2.BookingId = @BookingId
                            AND CDate(b1.StartTime) < CDate(b2.EndTime)
                            AND CDate(b1.EndTime) > CDate(b2.StartTime)
                            AND b1.BookingDate = b2.BookingDate
                            AND b1.BookingId <> @BookingId 
                            AND b1.BookingStatus <> 0";
                            
            this.dbHelperOleDb.AddParameter("@BookingId", bookingId);
            this.dbHelperOleDb.AddParameter("@BookingId", bookingId);
            return dbHelperOleDb.ExecuteScalar(sql) != null;
        }
        public BookingStatsDto GetBookingsStats(string buildingId)
        {
            string sql = @"SELECT
                            SUM(IIf(Booking.BookingStatus = 0, 1, 0)) AS AwaitingApproval,
                            SUM(IIf(Booking.BookingStatus = 1, 1, 0)) AS AwaitingPayment,
                            SUM(IIf(Booking.BookingStatus = 2, 1, 0)) AS Confirmed,
                            SUM(IIf(Booking.BookingStatus = 4, 1, 0)) AS Rejected
                        FROM
                            Booking
                            INNER JOIN Resident ON Resident.ResidentId = Booking.ResidentId
                        WHERE
                            Resident.BuildingId = @BuildingId;";
                    this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            BookingStatsDto bookingStatsDto = new BookingStatsDto();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    bookingStatsDto = this.modelCreator.CreateModel<BookingStatsDto>(reader);

                }
            }
            return bookingStatsDto;

        }
    }
}
