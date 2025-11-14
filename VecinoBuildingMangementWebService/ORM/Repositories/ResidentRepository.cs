using System.Collections.Generic;
using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class ResidentRepository : GenericRepository<Resident>, IRepository<Resident>
    {
        public ResidentRepository(DbHelperOleDb dbHelperOleDb)
            : base(dbHelperOleDb) { }
        //public bool Create(Resident model)
        //{
        //    //string sql = @$"Insert Into Resident(ResidentName,ResidentPassword,ResidentPhone,ResidentEmail,UnitNumber,BuildingId)
        //    //                Values('{model.ResidentName}','{model.ResidentPassword}','{model.ResidentPhone}',
        //    //                       '{model.ResidentEmail}',{model.UnitNumber},{model.BuildingId})";

        //    string sql = @$"Insert Into Resident(ResidentName,ResidentPassword,ResidentPhone,ResidentEmail,UnitNumber,BuildingId)
        //                    Values(@ResidentName,@ResidentPassword,@ResidentPhone,
        //                           @ResidentEmail,@UnitNumber,@BuildingId)";
        //    this.dbHelperOleDb.AddParameter("@ResidentName", model.ResidentName);
        //    this.dbHelperOleDb.AddParameter("@ResidentPassword", model.ResidentPassword);
        //    this.dbHelperOleDb.AddParameter("@ResidentPhone", model.ResidentPhone);
        //    this.dbHelperOleDb.AddParameter("@ResidentEmail",model.ResidentEmail);
        //    this.dbHelperOleDb.AddParameter("@UnitNumber", model.UnitNumber);
        //    this.dbHelperOleDb.AddParameter("@BuildingId", model.BuildingId);
        //    return this.dbHelperOleDb.Insert(sql) > 0;

        //}

        //public bool Delete(string id)
        //{
        //    string sql = @"Delete from Resident where ResidentId=@ResidentId";
        //    this.dbHelperOleDb.AddParameter("@ResidentId", id);
        //    return this.dbHelperOleDb.Delete(sql) > 0;
        //}

        //public List<Resident> GetAll()
        //{
        //    string sql = "Select * From Resident";
            
        //    List <Resident> residents = new List<Resident>();
        //    using(IDataReader reader = this.dbHelperOleDb.Select(sql))
        //    {
        //        while (reader.Read())
        //        {

        //            residents.Add(this.modelCreators.ResidentCreator.CreateModel(reader));

        //        }
        //    }
            
        //    return residents;
        //}

        //public Resident GetById(string id)
        //{
        //    string sql = "Select * From Resident Where ResidentId=@ResidentId";
        //    dbHelperOleDb.AddParameter("@ResidentId", id);

        //    using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
        //    {
        //        dataReader.Read();
        //        return this.modelCreators.ResidentCreator.CreateModel(dataReader);
        //    }
            
        //}

        //public bool Update(Resident model)
        //{
        //    string sql = @"Update Resident set ResidentName = @ResidentName,ResidentPassword = @ResidentPassword
        //                   ResidentPhone = @ResidentPhone,ResidentEmail = @ResidentEmail,UnitNumber=@UnitNumber,BuildingId = @BuildingId";
        //    this.dbHelperOleDb.AddParameter("@ResidentName", model.ResidentName);
        //    this.dbHelperOleDb.AddParameter("@ResidentPassword", model.ResidentPassword);
        //    this.dbHelperOleDb.AddParameter("@ResidentPhone", model.ResidentPhone);
        //    this.dbHelperOleDb.AddParameter("@ResidentEmail", model.ResidentEmail);
        //    this.dbHelperOleDb.AddParameter("@UnitNumber", model.UnitNumber);
        //    this.dbHelperOleDb.AddParameter("@BuildingId", model.BuildingId);
        //    return this.dbHelperOleDb.Update(sql) > 0;
        //}
        public string Login(string username, string password)
        {
            string sql = "Select ResidentId From Resident where ResidentName = @ResidentName and ResidentPassword = @ResidentPassword";
            this.dbHelperOleDb.AddParameter("@ResidentName", username);
            this.dbHelperOleDb.AddParameter("@ResidentPassword", password);
            using(IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
              
                if (dataReader.Read())
                {
                    return dataReader["ResidentId"].ToString();
                }
                return null;
                  
            }   
        }
        public List<Resident> GetResidentByBuilding(string buildingId)
        {
            string sql = "SELECT * From Resident Where BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            List<Resident> residents = new List<Resident>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    residents.Add(this.ModelCreator.CreateModel(reader));

                }
            }

            return residents;
        }
    }
}
