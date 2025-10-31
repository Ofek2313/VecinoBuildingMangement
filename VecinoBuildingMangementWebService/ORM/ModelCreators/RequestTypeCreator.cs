using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class RequestTypeCreator : IModelCreator<RequestTypes>
    {
        public RequestTypes CreateModel(IDataReader dataReader)
        {
            RequestTypes requestTypes = new RequestTypes();
            requestTypes.RequestTypeId = Convert.ToString(dataReader["RequestTypeId"]);
            requestTypes.RequestTypeName = Convert.ToString(dataReader["RequestTypeName"]);
            return requestTypes;
        }
    }
}
