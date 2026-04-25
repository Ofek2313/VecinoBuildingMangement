using System.Data;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{
    public class ServiceRequestRepository : GenericRepository<ServiceRequest>, IRepository<ServiceRequest>
    {
        public ServiceRequestRepository(DbHelperOleDb dbHelperOleDb,ModelCreator modelCreator)
            : base(dbHelperOleDb, modelCreator) { }
        //public bool Create(ServiceRequest model)
        //{
        //    //string sql = @$"Insert Into Resident(ResidentName,ResidentPassword,ResidentPhone,ResidentEmail,UnitNumber,BuildingId)
        //    //                Values('{model.ResidentName}','{model.ResidentPassword}','{model.ResidentPhone}',
        //    //                       '{model.ResidentEmail}',{model.UnitNumber},{model.BuildingId})";

        //    string sql = @$"Insert Into ServiceRequest(RequestTitle,RequestMessage,RequestTypeId,RequestDate,RequestStatus,ResidentId)
        //                    Values(@RequestTitle,@RequestMessage,@RequestTypeId,
        //                           @RequestDate,@RequestStatus,@ResidentId)";
        //    this.dbHelperOleDb.AddParameter("@RequestTitle", model.RequestTitle);
        //    this.dbHelperOleDb.AddParameter("@RequestMessage", model.RequestMessage);
        //    this.dbHelperOleDb.AddParameter("@RequestTypeId", model.RequestTypeId);
        //    this.dbHelperOleDb.AddParameter("@RequestDate", model.RequestDate);
        //    this.dbHelperOleDb.AddParameter("@RequestStatus", model.RequestStatus);
        //    this.dbHelperOleDb.AddParameter("@ResidentId", model.ResidentId);
        //    return this.dbHelperOleDb.Insert(sql) > 0;
        //}

        //public bool Delete(string id)
        //{

        //    string sql = @"Delete from ServiceRequest where RequestId=@RequestId";
        //    this.dbHelperOleDb.AddParameter("@RequestId", id);
        //    return this.dbHelperOleDb.Delete(sql) > 0;
        //}

        //public List<ServiceRequest> GetAll()
        //{
        //    string sql = "Select * From ServiceRequest";

        //    List<ServiceRequest> serviceRequests = new List<ServiceRequest>();
        //    using (IDataReader reader = this.dbHelperOleDb.Select(sql))
        //    {
        //        while (reader.Read())
        //        {

        //            serviceRequests.Add(this.modelCreators.ServiceRequestCreator.CreateModel(reader));

        //        }
        //    }

        //    return serviceRequests;
        //}
        public List<ServiceRequest> GetByStatus(string status)
        {
            string sql = @$"select * From ServiceRequest Where RequestStatus=@RequestStatus";
            this.dbHelperOleDb.AddParameter("@RequestStatus", status);
            List<ServiceRequest> serviceRequests = new List<ServiceRequest>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    serviceRequests.Add(this.modelCreator.CreateModel<ServiceRequest>(reader));

                }
            }

            return serviceRequests;
        }
        
        //public ServiceRequest GetById(string id)
        //{

        //    string sql = "Select * From ServiceRequest Where RequestId=@RequestId";
        //    dbHelperOleDb.AddParameter("@RequestId", id);

        //    using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
        //    {
        //        dataReader.Read();
        //        return this.modelCreators.ServiceRequestCreator.CreateModel(dataReader);
        //    }

        //}


        //public bool Update(ServiceRequest model)
        //{
        //    string sql = @"Update ServiceRequest set RequestTitle = @RequestTitle,RequestMessage = @RequestMessage
        //                   RequestTypeId = @RequestTypeId,RequestDate = @RequestDate,RequestStatus=@RequestStatus,ResidentId = @ResidentId";
        //    this.dbHelperOleDb.AddParameter("@RequestTitle", model.RequestTitle);
        //    this.dbHelperOleDb.AddParameter("@RequestMessage", model.RequestMessage);
        //    this.dbHelperOleDb.AddParameter("@RequestTypeId", model.RequestTypeId);
        //    this.dbHelperOleDb.AddParameter("@RequestDate", model.RequestDate);
        //    this.dbHelperOleDb.AddParameter("@RequestStatus", model.RequestStatus);
        //    this.dbHelperOleDb.AddParameter("@ResidentId", model.ResidentId);
        //    return this.dbHelperOleDb.Update(sql) > 0;
        //}
        public List<ServiceRequest> GetServiceRequestsByResidentId(string residentId)
        {
            string sql = "Select * From ServiceRequest WHERE ResidentId=@ResidentId";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);

            List<ServiceRequest> serviceRequests = new List<ServiceRequest>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    serviceRequests.Add(this.modelCreator.CreateModel<ServiceRequest>(reader));

                }
            }

            return serviceRequests;
        }
        public bool UpdateStatus(string status,string requestId)
        {
            string sql = @"Update ServiceRequest set RequestStatus=@RequestStatus where RequestId=@RequestId";
            this.dbHelperOleDb.AddParameter("@RequestStatus", status);
            this.dbHelperOleDb.AddParameter("@RequestId", requestId);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public bool DeleteByResidentId(string residentId)
        {

            string sql = @"Delete from ServiceRequest where ResidentId=@ResidentId";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }
        public List<ServiceRequestDetail> GetRequestsByBuilding(string buildingId)
        {
            string sql = @"SELECT
                            ServiceRequest.*,
                            RequestTypeName,
                            Resident.ResidentName,
                            Resident.ResidentEmail,
                            Resident.ResidentImage
                            FROM
                            RequestTypes
                            INNER JOIN (
                                Resident
                                INNER JOIN ServiceRequest ON Resident.ResidentId = ServiceRequest.ResidentId
                            ) ON RequestTypes.RequestTypeId = ServiceRequest.RequestTypeId
                             WHERE
                            (((Resident.BuildingId) = @BuildingId));";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);

            List<ServiceRequestDetail> serviceRequests = new List<ServiceRequestDetail>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    serviceRequests.Add(new ServiceRequestDetail {
                                            ServiceRequest = this.modelCreator.CreateModel<ServiceRequest>(reader),
                                            RequestTypeName = Convert.ToString(reader["RequestTypeName"]),
                                            ResidentName = Convert.ToString(reader["ResidentName"]),
                                            ResidentEmail = Convert.ToString(reader["ResidentEmail"]),
                                            ResidentImage = Convert.ToString(reader["ResidentImage"]),
                    } );

                }
            }

            return serviceRequests;

        }

        public (int Pending, int Completed, int InProgress) GetServiceRequestSummary(string buildingId)
        {
            string sql = @$"SELECT Count(IIF(RequestStatus = 'Pending', 1, NULL)) AS PendingCount,
                            Count(IIF(RequestStatus = 'Completed', 1, NULL)) AS CompletedCount,
                            Count(IIF(RequestStatus = 'In Progress', 1, NULL)) AS InProgressCount FROM  ServiceRequest INNER JOIN Resident ON ServiceRequest.ResidentId = Resident.ResidentId
                             WHERE BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                if (reader.Read())
                {

                    return (Convert.ToInt32(reader["PendingCount"]), Convert.ToInt32(reader["CompletedCount"]), Convert.ToInt32(reader["InProgressCount"]));
                }
                else
                    return (0, 0, 0);
            }
        }
    }
}
