using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class CityCreator : IModelCreator<City>
    {
        public City CreateModel(IDataReader dataReader)
        {
            City city = new City();
            city.CityId = Convert.ToString(dataReader["CityId"]);
            city.CityName = Convert.ToString(dataReader["CityName"]);



            return city;
        }
    }
}
