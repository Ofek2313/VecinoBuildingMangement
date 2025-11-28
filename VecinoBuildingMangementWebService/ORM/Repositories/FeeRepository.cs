using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class FeeRepository : GenericRepository<Fee>, IRepository<Fee>
    {
        public FeeRepository(DbHelperOleDb dbHelperOleDb)
            : base(dbHelperOleDb) { }
        //public bool Create(Fee model)
        //{
        //    string sql = @$"Insert Into Fee(FeeTitle,FeeAmount,FeeDueDate,IsPaid,ResidentId)
        //                    Values(@FeeTitle,@FeeAmount,@FeeDueDate,
        //                           @IsPaid,@ResidentId)";
        //    this.dbHelperOleDb.AddParameter("@FeeTitle", model.FeeTitle);
        //    this.dbHelperOleDb.AddParameter("@FeeAmount", model.FeeAmount);
        //    this.dbHelperOleDb.AddParameter("@FeeDueDate", model.FeeDueDate);
        //    this.dbHelperOleDb.AddParameter("@IsPaid", model.IsPaid);
        //    this.dbHelperOleDb.AddParameter("@ResidentId", model.ResidentId);
           
        //    return this.dbHelperOleDb.Insert(sql) > 0;
        //}

        //public bool Delete(string id)
        //{
        //    string sql = @"Delete from Fee where FeeId=@FeeId";
        //    this.dbHelperOleDb.AddParameter("@FeeId", id);
        //    return this.dbHelperOleDb.Delete(sql) > 0;
        //}

        //public List<Fee> GetAll()
        //{
        //    string sql = "Select * From Fee";

        //    List<Fee> fees = new List<Fee>();
        //    using (IDataReader reader = this.dbHelperOleDb.Select(sql))
        //    {
        //        while (reader.Read())
        //        {

        //            fees.Add(this.modelCreators.FeeCreator.CreateModel(reader));

        //        }
        //    }

        //    return fees;
        //}

        //public Fee GetById(string id)
        //{
        //    string sql = "Select * From Fee Where FeeId=@FeeId";
        //    dbHelperOleDb.AddParameter("@FeeId", id);

        //    using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
        //    {
        //        dataReader.Read();
        //        return this.modelCreators.FeeCreator.CreateModel(dataReader);
        //    }
        //}

        //public bool Update(Fee model)
        //{
        //    string sql = @$"Update Fee set FeeTitle=@FeeTitle,FeeAmount=@FeeAmount,FeeDueDate=@FeeDueDate,
        //                    IsPaid=@IsPaid,ResidentId=@ResidentId WHERE FeeId=@FeeId";
        //    this.dbHelperOleDb.AddParameter("@FeeTitle", model.FeeTitle);
        //    this.dbHelperOleDb.AddParameter("@FeeAmount", model.FeeAmount);
        //    this.dbHelperOleDb.AddParameter("@FeeDueDate", model.FeeDueDate);
        //    this.dbHelperOleDb.AddParameter("@IsPaid", model.IsPaid);
        //    this.dbHelperOleDb.AddParameter("@ResidentId", model.ResidentId);
        //    this.dbHelperOleDb.AddParameter("@FeeId", model.FeeId);
        //    return this.dbHelperOleDb.Update(sql) > 0;
        //}
        public List<Fee> GetUnPaidFeeById(string id)
        {
            string sql = "SELECT * FROM Fee WHERE IsPaid = False And ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@ResidentId",id);

            List<Fee> fees = new List<Fee>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    fees.Add(this.ModelCreator.CreateModel(reader));

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

                    fees.Add(this.ModelCreator.CreateModel(reader));

                }
            }

            return fees;
        }
        public List<Fee> ViewPaidFees()
        {
            string sql = "Select * From Fee WHERE IsPaid=True";

            List<Fee> fees = new List<Fee>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    fees.Add(this.ModelCreator.CreateModel(reader));

                }
            }

            return fees;
        }
        public bool PayFee(string feeId)
        {
            string sql = @"UPDATE Fee SET IsPaid = True WHERE FeeId = @FeeId";
            this.dbHelperOleDb.AddParameter("@FeeId", feeId);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
    }
}
