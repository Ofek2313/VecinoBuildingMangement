using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class ServiceRequestCreator : IModelCreator<ServiceRequest>
    {
        public ServiceRequest CreateModel(IDataReader dataReader)
        {
            ServiceRequest request = new ServiceRequest();
            request.RequestId = Convert.ToString(dataReader["RequestId"]);
            request.RequestTitle = Convert.ToString(dataReader["RequestTitle"]);
            request.RequestMessage = Convert.ToString(dataReader["RequestMessage"]);
            request.RequestTypeId = Convert.ToString(dataReader["RequestTypeId"]);
            request.RequestDate = Convert.ToString(dataReader["RequestDate"]);
            request.RequestStatus = Convert.ToString(dataReader["RequestStatus"]);
            request.ResidentId = Convert.ToString(dataReader["ResidentId"]);

            return request;
        }
    }
}
