using System.Data;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{
    public class FeeRepository : GenericRepository<Fee>, IRepository<Fee>
    {
        public FeeRepository(DbHelperOleDb dbHelperOleDb, ModelCreator modelCreator)
            : base(dbHelperOleDb,modelCreator) { }
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

        public List<Fee> GetFeesById(string residentId)
        {
            string sql = @"SELECT * FROM Fee WHERE  ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);

            List<Fee> fees = new List<Fee>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    fees.Add(this.modelCreator.CreateModel<Fee>(reader));

                }
            }

            return fees;
        }
        public List<Fee> GetUnPaidFeeById(string id)
        {
            string sql = @"SELECT * FROM Fee WHERE IsPaid = False And ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@ResidentId",id);

            List<Fee> fees = new List<Fee>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    fees.Add(this.modelCreator.CreateModel<Fee>(reader));

                }
            }

            return fees;
        }
        public List<Fee> ViewPaidFeesById(string id)
        {
            string sql = @"Select * From Fee Where IsPaid=True And ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@ResidentId", id);
            List<Fee> fees = new List<Fee>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    fees.Add(this.modelCreator.CreateModel<Fee>(reader));

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

                    fees.Add(this.modelCreator.CreateModel<Fee>(reader));

                }
            }

            return fees;
        }
        public bool PayFee(string feeId)
        {
            string sql = @"UPDATE Fee SET IsPaid = True, PaymentDate=@PaymentDate WHERE FeeId = @FeeId";
            this.dbHelperOleDb.AddParameter("@PaymentDate", DateTime.Now.ToString("dd/MM/yyyy"));
            this.dbHelperOleDb.AddParameter("@FeeId", feeId);
            
            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public List<ResidentFeeViewModel> GetFeesByBuildingId(string buildingId)
        {
            string sql = $@"SELECT
                        Fee.*,
                        Resident.ResidentName,
                        Resident.ResidentImage,
                        Resident.UnitNumber
                    FROM
                        Resident
                        INNER JOIN Fee ON Resident.ResidentId = Fee.ResidentId
                    WHERE
                        resident.BuildingId = @BuildingId;";
            //string sql = @"SELECT Fee.* FROM Resident INNER JOIN Fee ON Resident.ResidentId = Fee.ResidentId WHERE Resident.BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            List<ResidentFeeViewModel> fees = new List<ResidentFeeViewModel>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    ResidentFeeViewModel residentFee = new ResidentFeeViewModel
                    {
                        Fee = this.modelCreator.CreateModel<Fee>(reader),
                        ResidentName = reader["ResidentName"].ToString(),
                        ResidentImage = reader["ResidentImage"].ToString(),
                        UnitNumber = Convert.ToInt32(reader["UnitNumber"])

                    };
                    fees.Add(residentFee);
                }
            }

            return fees;
        }
        public int TotalFeesNumberByPayment(string buildingId,bool isPaid)
        {
            string sql = @"Select Count(*) FROM Fee INNER JOIN Resident ON Fee.ResidentId = Resident.ResidentId Where BuildingId = @BuildingId And IsPaid = @IsPaid";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            this.dbHelperOleDb.AddParameter("@IsPaid", isPaid);
            return Convert.ToInt32(this.dbHelperOleDb.ExecuteScalar(sql));
        }
        public double TotalFeesAmountByPayment(string buildingId,bool isPaid)
        {
            string sql = $"Select SUM(FeeAmount) From Fee INNER JOIN Resident ON Fee.ResidentId = Resident.ResidentId Where BuildingId = @BuildingId And IsPaid = @IsPaid";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            this.dbHelperOleDb.AddParameter("@IsPaid", isPaid);
            return Convert.ToDouble(this.dbHelperOleDb.ExecuteScalar(sql));
        }

        public List<TransactionViewModel> GetLastTransactionsByBuildingId(string buildingId)
        {
            string sql = @"SELECT TOP 10 Fee.*, Resident.ResidentName
                   FROM Fee INNER JOIN Resident ON Fee.ResidentId = Resident.ResidentId
                   WHERE Resident.BuildingId = @BuildingId AND Fee.IsPaid = True";

            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);

            List<TransactionViewModel> transactionViewModels = new List<TransactionViewModel>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {
                    transactionViewModels.Add(new TransactionViewModel
                    {
                        Fee = this.modelCreator.CreateModel<Fee>(reader),
                        ResidentName = Convert.ToString(reader["ResidentName"]),
                    }
                    );
                  

                }
            }

            return transactionViewModels;
        }

        public FeeSummary SummarizeFeesByBuilding(string buildingId)
        {
            string sql = $@"SELECT
                            COUNT(IIF(IsPaid = True, 1, NULL)) AS TotalPaid,
                            COUNT(IIF(IsPaid = False, 1, NULL)) AS TotalUnPaid,
                            SUM(IIF(IsPaid = True, FeeAmount, 0)) AS TotalCollected,
                            SUM(IIF(IsPaid = False, FeeAmount, 0)) AS Outstanding
                        FROM
                            Fee
                            INNER JOIN Resident ON Fee.ResidentId = Resident.ResidentId
                        WHERE
                            Resident.BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);

            FeeSummary feeSummary = new FeeSummary();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                if(reader.Read())
                {

                    feeSummary = this.modelCreator.CreateModel<FeeSummary>(reader);
                   
                }
            }
            return feeSummary;

            
        }

       
    }
       
    
}
