using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class ServiceRequestRepository : GenericRepository<ServiceRequest>, IRepository<ServiceRequest>
    {
        public ServiceRequestRepository(DbHelperOleDb dbHelperOleDb)
            : base(dbHelperOleDb) { }
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

                    serviceRequests.Add(this.ModelCreator.CreateModel(reader));

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

                    serviceRequests.Add(this.ModelCreator.CreateModel(reader));

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
    }
}
