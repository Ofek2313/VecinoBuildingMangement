using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class CityRepository : Repository, IRepository<City>
    {
        public bool Create(City model)
        {
            string sql = @$"Insert Into Cities(CityName)
                            Values(@CityName)";
            this.dbHelperOleDb.AddParameter("@CityName", model.CityName);

            return this.dbHelperOleDb.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = @"Delete from Cities where CityId=@CityId";
            this.dbHelperOleDb.AddParameter("@CityId", id);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }

        public List<City> GetAll()
        {
            string sql = "Select * From Cities";

            List<City> cities = new List<City>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    cities.Add(this.modelCreators.CityCreator.CreateModel(reader));

                }
            }

            return cities;
        }

        public City GetById(string id)
        {
            string sql = "Select * From Cities Where CityId=@CityId";
            dbHelperOleDb.AddParameter("@CityId", id);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                dataReader.Read();
                return this.modelCreators.CityCreator.CreateModel(dataReader);
            }
        }

        public bool Update(City model)
        {
            string sql = @"Update Cities set CityName = @CityName";
            this.dbHelperOleDb.AddParameter("@CityName", model.CityName);

            return this.dbHelperOleDb.Update(sql) > 0;
        }
    }
}
