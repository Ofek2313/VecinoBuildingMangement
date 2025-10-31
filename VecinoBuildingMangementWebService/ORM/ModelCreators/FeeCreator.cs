using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class FeeCreator : IModelCreator<Fee>
    {
        public Fee CreateModel(IDataReader dataReader)
        {
            Fee fee = new Fee();
            fee.FeeId = Convert.ToString(dataReader["FeeId"]);
            fee.FeeTitle = Convert.ToString(dataReader["FeeTitle"]);
            fee.FeeDueDate = Convert.ToString(dataReader["FeeDueDate"]);
            fee.IsPaid = Convert.ToBoolean(dataReader["IsPaid"]);
            fee.ResidentId = Convert.ToString(dataReader["ResidentId"]);

            return fee;
        }

    }
}
