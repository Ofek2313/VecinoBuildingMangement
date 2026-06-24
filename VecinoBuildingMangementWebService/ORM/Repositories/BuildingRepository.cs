using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Data;
using VecinoBuildingMangement;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{

    public class BuildingRepository : GenericRepository<Building>
    {
        public BuildingRepository(DbHelperOleDb dbHelperOleDb, ModelCreator modelCreator)
            : base(dbHelperOleDb, modelCreator) { }

       
        public List<Building> GetByCityId(string cityId)
        {
            string sql = "Select * From Building where CityId = @CityId";
            this.dbHelperOleDb.AddParameter("@CityId", cityId);

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
            return buildings.Skip(buildingPerPage * (page - 1)).Take(buildingPerPage).ToList();
        }
        public Building GetBuildingByResidentId(string residentId)
        {
            string sql = "SELECT b.* FROM Building b INNER JOIN Resident ON b.BuildingId = Resident.BuildingId WHERE Resident.ResidentId = @ResidentId;";
            dbHelperOleDb.AddParameter("@ResidentId", residentId);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                dataReader.Read();
                return this.modelCreator.CreateModel<Building>(dataReader);
            }
        }
        public bool UpdateJoinCode(string code, string buildingId)
        {
            string sql = @"UPDATE Building set JoinCode=@JoinCode Where BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@JoinCode", code);
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public Building GetBuildingByCode(string buildingCode)
        {
            string sql = "Select * From Building Where JoinCode=@JoinCode";
            dbHelperOleDb.AddParameter("@JoinCode", buildingCode);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                if (dataReader.Read())
                    return this.modelCreator.CreateModel<Building>(dataReader);
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
                        City.CityName
                        FROM
                        City
                        INNER JOIN (
                            Building
                            INNER JOIN Resident ON Building.BuildingId = Resident.BuildingId
                        ) ON City.CityId = Building.CityId
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
            string sql = $@"SELECT
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
                                RequestStatus = {(int)RequestStatus.Pending}
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
            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                while (dataReader.Read())
                {
                    activityViewModels.Add(this.modelCreator.CreateModel<ActivityViewModel>(dataReader, new List<string> { "ActivityDescription" }));
                }
            }
            return activityViewModels;
        }
        public string GetBuildingPhotoById(string buildingId)
        {
            string sql = "Select BuildingImage From Building Where BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                if (dataReader.Read())
                    return Convert.ToString(dataReader["BuildingImage"]);
                else
                    return "";
            }
        }
        public bool UpdateBuildingWithCords(BuildingUpdateDto buildingUpdateDto, CordsDto cordsDto)
        {
            string sql = @"UPDATE Building 
                       SET CityId = @CityId, 
                       Address = @Address, 
                       EntranceCode = @EntranceCode, 
                       EntranceName = @EntranceName,
                       TotalUnits = @TotalUnits, 
                       Floors = @Floors";

            this.dbHelperOleDb.AddParameter("@CityId", buildingUpdateDto.CityId);
            this.dbHelperOleDb.AddParameter("@Address", buildingUpdateDto.Address);
            this.dbHelperOleDb.AddParameter("@EntranceCode", buildingUpdateDto.EntranceCode);
            this.dbHelperOleDb.AddParameter("@EntranceName", buildingUpdateDto.EntranceName);
            this.dbHelperOleDb.AddParameter("@TotalUnits", buildingUpdateDto.TotalUnits);
            this.dbHelperOleDb.AddParameter("@Floors", buildingUpdateDto.TotalUnits);

            if (buildingUpdateDto.AddressChangedFlag)
            {
                if (cordsDto != null)
                {
                    sql += ", Latitude = @Latitude, Longitude = @Longitude";

                    this.dbHelperOleDb.AddParameter("@Latitude", cordsDto.Latitude);
                    this.dbHelperOleDb.AddParameter("@Longitude", cordsDto.Longitude);
                }

            }
            sql += " WHERE BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingUpdateDto.BuildingId);

            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public int GetBuildingStats()
        {
            string sql = @"Select Count(*) From Building";
            return Convert.ToInt32(this.dbHelperOleDb.ExecuteScalar(sql));
        }
        public List<BuildingViewModel> GetBuildingsPerPage( int pageNumber, int FeesPerPage)
        {
            string sql;
            if (pageNumber > 1)
            {
                sql = $@"SELECT
                            TOP {FeesPerPage}
                            Building.*,
                            City.CityName
                        FROM
                            Building
                        INNER JOIN
                            City
                        ON
                            Building.CityId = City.CityId
                        WHERE
                            Building.BuildingId NOT IN (
                                SELECT TOP {(pageNumber - 1) * FeesPerPage}
                                    BuildingId
                                FROM
                                    Building
                                ORDER BY
                                    BuildingId
                            )
                        ORDER BY
                            Building.BuildingId;";
                
            }
            else
            {
                sql = $@"SELECT TOP {FeesPerPage}
                        Building.*,
                        City.CityName
                    FROM
                        Building
                    INNER JOIN
                        City ON Building.CityId = City.CityId
                    Where Building.BuildingId <> 0
                    ORDER BY
                        Building.BuildingId;";
                
            }

            List<BuildingViewModel> buildings = new List<BuildingViewModel>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    buildings.Add(new BuildingViewModel 
                    { 
                        Building = this.modelCreator.CreateModel<Building>(reader), 
                        CityName = Convert.ToString(reader["CityName"]) 
                    });

                }
            }

            return buildings;
        }
    }

    
}
