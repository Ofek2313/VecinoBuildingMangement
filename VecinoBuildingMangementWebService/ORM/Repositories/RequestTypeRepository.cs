using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class RequestTypeRepository : Repository, IRepository<RequestTypes>
    {
        public bool Create(RequestTypes model)
        {

            string sql = @$"Insert Into RequestTypes(RequestTypeName)
                            Values(@RequestTypeName)";
            this.dbHelperOleDb.AddParameter("@RequestTypeName", model.RequestTypeName);

            return this.dbHelperOleDb.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = @"Delete from RequestTypes where RequestTypeId=@RequestTypeId";
            this.dbHelperOleDb.AddParameter("@RequestTypeId", id);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }

        public List<RequestTypes> GetAll()
        {
            string sql = "Select * From RequestTypes";

            List<RequestTypes> requestTypes = new List<RequestTypes>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    requestTypes.Add(this.modelCreators.RequestTypeCreator.CreateModel(reader));

                }
            }

            return requestTypes;
        }

        public RequestTypes GetById(string id)
        {

            string sql = "Select * From RequestTypes Where RequestTypeId=@RequestTypeId";
            dbHelperOleDb.AddParameter("@RequestTypeId", id);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                dataReader.Read();
                return this.modelCreators.RequestTypeCreator.CreateModel(dataReader);
            }
        }

        public bool Update(RequestTypes model)
        {

            string sql = @"Update RequestTypes set RequestTypeName = @RequestTypeName";
            this.dbHelperOleDb.AddParameter("@RequestTypeName", model.RequestTypeName);

            return this.dbHelperOleDb.Update(sql) > 0;
        }
    }
}
