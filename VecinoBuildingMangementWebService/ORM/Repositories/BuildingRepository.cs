using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Data;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{

    public class BuildingRepository : GenericRepository<Building>, IRepository<Building>
    {
        public BuildingRepository(DbHelperOleDb dbHelperOleDb,ModelCreator modelCreator)
            : base(dbHelperOleDb, modelCreator) { } 

        //public bool Create(Building model)
        //{
        //    string sql = @$"Insert Into Building(CityId,Address,EntranceCode,TotalUnits,Floors,JoinCode)
        //                    Values(@CityId,@Address,@EntranceCode,
        //                           @TotalUnits,@Floors,@JoinCode)";
        //    this.dbHelperOleDb.AddParameter("@CityId", model.CityId);
        //    this.dbHelperOleDb.AddParameter("@Address", model.Address);
        //    this.dbHelperOleDb.AddParameter("@EntranceCode", model.EntranceCode);
        //    this.dbHelperOleDb.AddParameter("@TotalUnits", model.TotalUnits);
        //    this.dbHelperOleDb.AddParameter("@Floors", model.Floors);
        //    this.dbHelperOleDb.AddParameter("@JoinCode", model.JoinCode);
        //    return this.dbHelperOleDb.Insert(sql) > 0;
        //}

        //public bool Delete(string id)
        //{
        //    string sql = @"Delete from Building where BuildingId=@BuildingId";
        //    this.dbHelperOleDb.AddParameter("@BuildingId", id);
        //    return this.dbHelperOleDb.Delete(sql) > 0;
        //}

        //public List<Building> GetAll()
        //{
        //    string sql = "Select * From Building";

        //    List<Building> buildings = new List<Building>();
        //    using (IDataReader reader = this.dbHelperOleDb.Select(sql))
        //    {
        //        while (reader.Read())
        //        {

        //            buildings.Add(this.modelCreators.BuildingCreator.CreateModel(reader));

        //        }
        //    }

        //    return buildings;
        //}

        //public Building GetById(string id)
        //{
        //    string sql = "Select * From Building Where BuildingId=@BuildingId";
        //    dbHelperOleDb.AddParameter("@BuildingId", id);

        //    using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
        //    {
        //        dataReader.Read();
        //        return this.modelCreators.BuildingCreator.CreateModel(dataReader);
        //    }
        //}

        //public bool Update(Building model)
        //{
        //    string sql = @"Update Building set CityId = @CityId,Address = @Address
        //                   EntranceCode = @EntranceCode,TotalUnits = @TotalUnits,Floors=@Floors,JoinCode = @JoinCode";
        //    this.dbHelperOleDb.AddParameter("@CityId", model.CityId);
        //    this.dbHelperOleDb.AddParameter("@Address", model.Address);
        //    this.dbHelperOleDb.AddParameter("@EntranceCode", model.EntranceCode);
        //    this.dbHelperOleDb.AddParameter("@TotalUnits", model.TotalUnits);
        //    this.dbHelperOleDb.AddParameter("@Floors", model.Floors);
        //    this.dbHelperOleDb.AddParameter("@JoinCode", model.JoinCode);
        //    return this.dbHelperOleDb.Update(sql) > 0;
        //}
        public List<Building> GetByCityId(string cityId)
        {
            string sql = "Select * From Building where CityId = @CityId";
            this.dbHelperOleDb.AddParameter("@CityId",cityId);

            List<Building> buildings = new List<Building>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    buildings.Add(this.modelCreator.CreateModel<Building>(reader));

                }
            }

            return buildings;
        }
        public List<Building> GetBuildingByPage(int page)
        {
            int buildingPerPage = 10;
            List<Building> buildings = this.GetAll();
            return buildings.Skip(buildingPerPage * (page-1)).Take(buildingPerPage).ToList();
        }
        public Building GetBuildingByResidentId(string residentId)
        {
            string sql = "SELECT b.BuildingId, b.CityId,b.Address,b.EntranceCode,b.TotalUnits,b.Floors,b.JoinCode,b.BuildingImage,b.EntranceName FROM Building b INNER JOIN Resident ON b.BuildingId = Resident.BuildingId WHERE Resident.ResidentId = @ResidentId;";
            dbHelperOleDb.AddParameter("@ResidentId", residentId);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                dataReader.Read();
                return this.modelCreator.CreateModel<Building>(dataReader);
            }
        }
        public bool UpdateJoinCode(string code,string buildingId)
        {
            string sql = @"UPDATE Building set JoinCode=@JoinCode Where BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@JoinCode", code);
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public string GetBuildingIdByCode(string buildingCode)
        {
            string sql = "Select * From Building Where JoinCode=@JoinCode";
            dbHelperOleDb.AddParameter("@JoinCode", buildingCode);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                if(dataReader.Read())
                    return dataReader["BuildingId"].ToString();
                else
                    return null;
            }

        }
        public int GetBuildingCount()
        {
            string sql = "Select Count(*) From Building";
            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                dataReader.Read();
                return Convert.ToInt32(dataReader[0]);
            }
        }

        public bool UpdatePhotoById(string buildingId, string fileName)
        {
            string sql = $"Update Building SET BuildingImage = @BuildingImage WHERE BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingImage", fileName);
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            return this.dbHelperOleDb.Update(sql) > 0;
         
        }

        public AdminMainPage GetAdminOverlay(string residentId)
        {
            AdminMainPage viewmodel = new AdminMainPage();
            string sql = @"SELECT
                        Building.*,
                        Resident.ResidentName,
                        Cities.CityName
                        FROM
                        Cities
                        INNER JOIN (
                            Building
                            INNER JOIN Resident ON Building.BuildingId = Resident.BuildingId
                        ) ON Cities.CityId = Building.CityId
                        WHERE
                        (((Resident.ResidentId) = @ResidentId));";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                
                if (dataReader.Read())
                {
                    viewmodel.Building = this.modelCreator.CreateModel<Building>(dataReader);
                    viewmodel.ResidentName = Convert.ToString(dataReader["ResidentName"]);
                    viewmodel.CityName = Convert.ToString(dataReader["CityName"]);
                }
              
                
            }
            return viewmodel;
        }
        public BuildingStats GetBuildingStats(string buildingId)
        {
            string sql = @"SELECT
                        (
                            SELECT
                                COUNT(*)
                            FROM
                                Fee
                            WHERE
                                IsPaid = 0
                                AND ResidentId IN (
                                    SELECT
                                        ResidentId
                                    FROM
                                        Resident
                                    WHERE
                                        BuildingId = b.BuildingId
                                )
                        ) AS UnpaidFees,
                        (
                            SELECT
                                COUNT(*)
                            FROM
                                ServiceRequest
                            WHERE
                                RequestStatus = 'Pending'
                                AND ResidentId IN (
                                    SELECT
                                        ResidentId
                                    FROM
                                        Resident
                                    WHERE
                                        BuildingId = b.BuildingId
                                )
                        ) AS OpenRequests,
                        (
                            SELECT
                                count(*)
                            FROM
                                Poll
                            WHERE
                                Poll.BuildingId = b.BuildingId
                                AND Poll.IsActive = True
                                AND Cdate (Poll.PollDate) >= Date()
                        ) AS OpenPolls,
                        (
                            SELECT
                                Count(*)
                            FROM
                                Resident
                            WHERE
                                Resident.BuildingId = b.BuildingId
                        ) AS Occupancy,
                        (
                            SELECT
                                COUNT(*)
                            FROM
                                Fee
                            WHERE
                                IsPaid = 1
                                AND ResidentId IN (
                                    SELECT
                                        ResidentId
                                    FROM
                                        Resident
                                    WHERE
                                        BuildingId = b.BuildingId
                                )
                        ) AS PaidFees,
                        (
                            SELECT
                                COUNT(*)
                            FROM
                                (
                                    ResidentNotification AS rn
                                    INNER JOIN Resident AS r ON rn.ResidentId = r.ResidentId
                                )
                                INNER JOIN Notification AS n ON rn.NotificationId = n.NotificationId
                            WHERE
                                r.BuildingId = b.BuildingId
                                AND Month(CDate (n.NotificationDate)) = Month(Date())
                                AND Year(CDate (n.NotificationDate)) = Year(Date())
                        ) AS NotificationsSentThisMonth
                    FROM
                        Building AS b
                    WHERE
                        (((b.BuildingId) = @BuildingId));";

            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            BuildingStats buildingStats = new BuildingStats();
            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                if (dataReader.Read())
                {
                    buildingStats = this.modelCreator.CreateModel<BuildingStats>(dataReader);
                }


            }
            return buildingStats;
        }

        public List<ActivityViewModel> GetActivityViewModelsByBuildingId(string buildingId)
        {
            string sql = @"SELECT
                    TOP 5 *
                FROM
                    (
                        SELECT
                            FeeTitle AS ActivityTitle,
                            PaymentDate AS ActivityDate,
                            'Fee' AS ActivityType,
                            Resident.ResidentName
                        FROM
                            Fee
                            INNER JOIN Resident ON Resident.ResidentId = Fee.ResidentId
                        WHERE
                            Resident.BuildingId = @BuildingId
                            AND IsPaid = True
                        UNION ALL
                        SELECT
                            RequestTitle AS ActivityTitle,
                            RequestDate AS ActivityDate,
                            'Request' AS ActivityType,
                            ResidentName
                        FROM
                            ServiceRequest
                            INNER JOIN Resident ON Resident.ResidentId = ServiceRequest.ResidentId
                        WHERE
                            Resident.BuildingId = @BuildingId
                        UNION ALL
                        SELECT
                            'Voted' AS ActivityTitle,
                            VoteDate AS ActivityDate,
                            'Vote' AS ActivityType,
                            ResidentName
                        FROM
                            Vote
                            INNER JOIN Resident ON Resident.ResidentId = Vote.ResidentId
                        WHERE
                            Resident.BuildingId = @BuildingId
                    )
                ORDER BY
                    Cdate (ActivityDate) DESC;";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            List<ActivityViewModel> activityViewModels = new List<ActivityViewModel>();
            using(IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                while(dataReader.Read())
                {
                    activityViewModels.Add(this.modelCreator.CreateModel<ActivityViewModel>(dataReader,new List<string>{ "ActivityDescription"}));
                }
            }
            return activityViewModels;
        }
    }

    
}
