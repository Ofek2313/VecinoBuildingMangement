using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class BuildingCreator : IModelCreator<Building>
    {
        public Building CreateModel(IDataReader dataReader)
        {
            Building building = new Building();
            building.BuildingId = Convert.ToString(dataReader["BuildingId"]);
            building.CityId = Convert.ToString(dataReader["CityId"]);
            building.Address = Convert.ToString(dataReader["Address"]);
            building.EntranceCode = Convert.ToString(dataReader["EntranceCode"]);
            building.TotalUnits = Convert.ToInt16(dataReader["TotalUnits"]);
            building.Floors = Convert.ToInt16(dataReader["Floors"]);
            building.JoinCode = Convert.ToString(dataReader["JoinCode"]);

            return building;
        }
    }
}
