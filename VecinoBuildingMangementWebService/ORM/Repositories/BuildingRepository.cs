using Swashbuckle.AspNetCore.SwaggerUI;
using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{

    public class BuildingRepository : Repository, IRepository<Building>
    {
        public BuildingRepository(DbHelperOleDb dbHelperOleDb, ModelCreators modelCreators)
            : base(dbHelperOleDb, modelCreators) { } 

        public bool Create(Building model)
        {
            string sql = @$"Insert Into Building(CityId,Address,EntranceCode,TotalUnits,Floors,JoinCode)
                            Values(@CityId,@Address,@EntranceCode,
                                   @TotalUnits,@Floors,@JoinCode)";
            this.dbHelperOleDb.AddParameter("@CityId", model.CityId);
            this.dbHelperOleDb.AddParameter("@Address", model.Address);
            this.dbHelperOleDb.AddParameter("@EntranceCode", model.EntranceCode);
            this.dbHelperOleDb.AddParameter("@TotalUnits", model.TotalUnits);
            this.dbHelperOleDb.AddParameter("@Floors", model.Floors);
            this.dbHelperOleDb.AddParameter("@JoinCode", model.JoinCode);
            return this.dbHelperOleDb.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = @"Delete from Building where BuildingId=@BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", id);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }

        public List<Building> GetAll()
        {
            string sql = "Select * From Building";

            List<Building> buildings = new List<Building>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    buildings.Add(this.modelCreators.BuildingCreator.CreateModel(reader));

                }
            }

            return buildings;
        }

        public Building GetById(string id)
        {
            string sql = "Select * From Building Where BuildingId=@BuildingId";
            dbHelperOleDb.AddParameter("@BuildingId", id);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                dataReader.Read();
                return this.modelCreators.BuildingCreator.CreateModel(dataReader);
            }
        }

        public bool Update(Building model)
        {
            string sql = @"Update Building set CityId = @CityId,Address = @Address
                           EntranceCode = @EntranceCode,TotalUnits = @TotalUnits,Floors=@Floors,JoinCode = @JoinCode";
            this.dbHelperOleDb.AddParameter("@CityId", model.CityId);
            this.dbHelperOleDb.AddParameter("@Address", model.Address);
            this.dbHelperOleDb.AddParameter("@EntranceCode", model.EntranceCode);
            this.dbHelperOleDb.AddParameter("@TotalUnits", model.TotalUnits);
            this.dbHelperOleDb.AddParameter("@Floors", model.Floors);
            this.dbHelperOleDb.AddParameter("@JoinCode", model.JoinCode);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public List<Building> GetByCityId(string cityId)
        {
            string sql = "Select * From Building where CityId = @CityId";
            this.dbHelperOleDb.AddParameter("@CityId",cityId);

            List<Building> buildings = new List<Building>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    buildings.Add(this.modelCreators.BuildingCreator.CreateModel(reader));

                }
            }

            return buildings;
        }
        public List<Building> GetBuildingByPage(int page)
        {
            int buildingPerPage = 2;
            List<Building> buildings = this.GetAll();
            return buildings.Skip(buildingPerPage * (page-1)).Take(buildingPerPage).ToList();
        }
    }
}
