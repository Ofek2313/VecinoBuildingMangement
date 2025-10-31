using System.Data;
using VecinoBuildingMangement.Models;
namespace VecinoBuildingMangementWebService
{
    public class ResidentCreator : IModelCreator<Resident>
    {
        public Resident CreateModel(IDataReader dataReader)
        {
            Resident resident = new Resident();
            resident.ResidentName = Convert.ToString(dataReader["ResidentName"]);
            resident.ResidentName = Convert.ToString(dataReader["ResidentPassword"]);
            resident.ResidentId = Convert.ToString(dataReader["ResidentId"]);
            resident.ResidentPhone = Convert.ToString(dataReader["ResidenPhone"]);
            resident.ResidentEmail = Convert.ToString(dataReader["ResidenEmail"]);
            resident.UnitNumber = Convert.ToInt16(dataReader["UnitNumber"]);
            resident.BuildingId = Convert.ToString(dataReader["BuildingId"]);

            return resident;
        }
    }
}
