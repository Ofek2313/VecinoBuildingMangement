using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class FeeRepository : Repository, IRepository<Fee>
    {
        public bool Create(Fee model)
        {
            string sql = @$"Insert Into Fee(FeeTitle,FeeAmount,FeeDueDate,IsPaid,ResidentId)
                            Values(@FeeTitle,@FeeAmount,@FeeDueDate,
                                   @IsPaid,@ResidentId)";
            this.dbHelperOleDb.AddParameter("@FeeTitle", model.FeeTitle);
            this.dbHelperOleDb.AddParameter("@FeeAmount", model.FeeAmount.ToString());
            this.dbHelperOleDb.AddParameter("@FeeDueDate", model.FeeDueDate);
            this.dbHelperOleDb.AddParameter("@IsPaid", model.IsPaid.ToString());
            this.dbHelperOleDb.AddParameter("@ResidentId", model.ResidentId);
           
            return this.dbHelperOleDb.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = @"Delete from Fee where FeeId=@FeeId";
            this.dbHelperOleDb.AddParameter("@FeeId", id);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }

        public List<Fee> GetAll()
        {
            string sql = "Select * From Fee";

            List<Fee> fees = new List<Fee>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    fees.Add(this.modelCreators.FeeCreator.CreateModel(reader));

                }
            }

            return fees;
        }

        public Fee GetById(string id)
        {
            string sql = "Select * From Fee Where FeeId=@FeeId";
            dbHelperOleDb.AddParameter("@FeeId", id);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                dataReader.Read();
                return this.modelCreators.FeeCreator.CreateModel(dataReader);
            }
        }

        public bool Update(Fee model)
        {
            string sql = @$"Update Fee set FeeTitle=@FeeTitle,FeeAmount=@FeeAmount,FeeDueDate=@FeeDueDate,
                            IsPaid=@IsPaid,ResidentId=@ResidentId";
            this.dbHelperOleDb.AddParameter("@FeeTitle", model.FeeTitle);
            this.dbHelperOleDb.AddParameter("@FeeAmount", model.FeeAmount.ToString());
            this.dbHelperOleDb.AddParameter("@FeeDueDate", model.FeeDueDate);
            this.dbHelperOleDb.AddParameter("@IsPaid", model.IsPaid.ToString());
            this.dbHelperOleDb.AddParameter("@ResidentId", model.ResidentId);

            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public List<Fee> GetUnPaidFeeById(string id)
        {
            string sql = "SELECT FeeTitle, FeeAmount, FeeDueDate FROM Fee WHERE IsPaid = False And ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@ResidentId",id);

            List<Fee> fees = new List<Fee>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    fees.Add(this.modelCreators.FeeCreator.CreateModel(reader));

                }
            }

            return fees;
        }
        public List<Fee> ViewPaidFeesById(string id)
        {
            string sql = "Select FeeTitle,FeeAmount From Fee Where IsPaid=True And ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@ResidentId", id);
            List<Fee> fees = new List<Fee>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    fees.Add(this.modelCreators.FeeCreator.CreateModel(reader));

                }
            }

            return fees;
        }
    }
}
