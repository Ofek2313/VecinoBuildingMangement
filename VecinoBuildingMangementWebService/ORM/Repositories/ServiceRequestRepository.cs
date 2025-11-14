using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class ServiceRequestRepository : Repository, IRepository<ServiceRequest>
    {
        public ServiceRequestRepository(DbHelperOleDb dbHelperOleDb, ModelCreators modelCreators)
            : base(dbHelperOleDb, modelCreators) { }
        public bool Create(ServiceRequest model)
        {
            //string sql = @$"Insert Into Resident(ResidentName,ResidentPassword,ResidentPhone,ResidentEmail,UnitNumber,BuildingId)
            //                Values('{model.ResidentName}','{model.ResidentPassword}','{model.ResidentPhone}',
            //                       '{model.ResidentEmail}',{model.UnitNumber},{model.BuildingId})";

            string sql = @$"Insert Into ServiceRequest(RequestTitle,RequestMessage,RequestTypeId,RequestDate,RequestStatus,ResidentId)
                            Values(@RequestTitle,@RequestMessage,@RequestTypeId,
                                   @RequestDate,@RequestStatus,@ResidentId)";
            this.dbHelperOleDb.AddParameter("@RequestTitle", model.RequestTitle);
            this.dbHelperOleDb.AddParameter("@RequestMessage", model.RequestMessage);
            this.dbHelperOleDb.AddParameter("@RequestTypeId", model.RequestTypeId);
            this.dbHelperOleDb.AddParameter("@RequestDate", model.RequestDate);
            this.dbHelperOleDb.AddParameter("@RequestStatus", model.RequestStatus);
            this.dbHelperOleDb.AddParameter("@ResidentId", model.ResidentId);
            return this.dbHelperOleDb.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {

            string sql = @"Delete from ServiceRequest where RequestId=@RequestId";
            this.dbHelperOleDb.AddParameter("@RequestId", id);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }

        public List<ServiceRequest> GetAll()
        {
            string sql = "Select * From ServiceRequest";

            List<ServiceRequest> serviceRequests = new List<ServiceRequest>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    serviceRequests.Add(this.modelCreators.ServiceRequestCreator.CreateModel(reader));

                }
            }

            return serviceRequests;
        }
        public List<ServiceRequest> GetByStatus(string status)
        {
            string sql = @$"select * From ServiceRequest Where RequestStatus=@RequestStatus";
            this.dbHelperOleDb.AddParameter("@RequestStatus", status);
            List<ServiceRequest> serviceRequests = new List<ServiceRequest>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    serviceRequests.Add(this.modelCreators.ServiceRequestCreator.CreateModel(reader));

                }
            }

            return serviceRequests;
        }
        public ServiceRequest GetById(string id)
        {

            string sql = "Select * From ServiceRequest Where RequestId=@RequestId";
            dbHelperOleDb.AddParameter("@RequestId", id);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                dataReader.Read();
                return this.modelCreators.ServiceRequestCreator.CreateModel(dataReader);
            }

        }


        public bool Update(ServiceRequest model)
        {
            string sql = @"Update ServiceRequest set RequestTitle = @RequestTitle,RequestMessage = @RequestMessage
                           RequestTypeId = @RequestTypeId,RequestDate = @RequestDate,RequestStatus=@RequestStatus,ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@RequestTitle", model.RequestTitle);
            this.dbHelperOleDb.AddParameter("@RequestMessage", model.RequestMessage);
            this.dbHelperOleDb.AddParameter("@RequestTypeId", model.RequestTypeId);
            this.dbHelperOleDb.AddParameter("@RequestDate", model.RequestDate);
            this.dbHelperOleDb.AddParameter("@RequestStatus", model.RequestStatus);
            this.dbHelperOleDb.AddParameter("@ResidentId", model.ResidentId);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
    }
}
