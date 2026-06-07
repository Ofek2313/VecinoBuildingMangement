using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{
    public class ResidentRepository :GenericRepository<Resident>
    {
        public ResidentRepository(DbHelperOleDb dbHelperOleDb, ModelCreator modelCreator)
            :base(dbHelperOleDb, modelCreator) { }
       
        public override bool Create(Resident model)
        {   
                              

            string sql = @$"Insert Into Resident(ResidentName,ResidentPassword,ResidentPhone,ResidentEmail,UnitNumber,BuildingId,ResidentSalt,ResidentImage)
                            Values(@ResidentName,@ResidentPassword,@ResidentPhone,
                                   @ResidentEmail,@UnitNumber,@BuildingId,@ResidentSalt,@ResidentImage)";
            string salt = GetSalt(GetRandomNumber());
            this.dbHelperOleDb.AddParameter("@ResidentName", model.ResidentName);
            this.dbHelperOleDb.AddParameter("@ResidentPassword", GetHash(model.ResidentPassword, salt));
            this.dbHelperOleDb.AddParameter("@ResidentPhone", model.ResidentPhone);
            this.dbHelperOleDb.AddParameter("@ResidentEmail", model.ResidentEmail);
            this.dbHelperOleDb.AddParameter("@UnitNumber", model.UnitNumber);
            this.dbHelperOleDb.AddParameter("@BuildingId", model.BuildingId);
            this.dbHelperOleDb.AddParameter("@ResidentSalt",salt);
            this.dbHelperOleDb.AddParameter("@ResidentImage", model.ResidentImage);
         

            return this.dbHelperOleDb.Insert(sql) > 0;

        }
    
        private string GetHash(string password, string salt)
        {
            string combine = password + salt;
            byte[] bytes = Encoding.UTF8.GetBytes(combine);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
      

       
        public string Login(string email, string password)
        {
            string sql = "Select * From Resident where ResidentEmail=@ResidentEmail";
            this.dbHelperOleDb.AddParameter("@ResidentEmail", email);
         
            using(IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
              
                if (dataReader.Read()) // Hashes the input password and compares with the hased password in the database
                {
                    string salt = dataReader["ResidentSalt"].ToString();
                    string hash = dataReader["ResidentPassword"].ToString();
                    string calcHash = GetHash(password, salt);
                    Console.WriteLine(calcHash);
                    if(calcHash == hash)
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

                    residents.Add(this.modelCreator.CreateModel<Resident>(reader));

                }
            }

            return residents;
        }
        public List<ResidentSummaryDTO> GetResidentSummaryByBuilding(string buildingId) // Gets a list of the resident name, resident image, and id, usefull for not transporting the entire resident model and only the necessary parts.
        {
            string sql = "SELECT ResidentId,ResidentName,ResidentImage From Resident Where BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            List<ResidentSummaryDTO> residents = new List<ResidentSummaryDTO>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    residents.Add(this.modelCreator.CreateModel<ResidentSummaryDTO>(reader));

                }
            }

            return residents;
        }
        public bool UpdateResidentBuilding(string residentId,string buildingId)
        {
            string sql = @"UPDATE Resident set BuildingId=@BuildingId Where ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public bool UpdateAdminCreationResidentBuilding(string residentId, string buildingId)
        {
            string sql = @"UPDATE Resident set BuildingId=@BuildingId, IsAdmin = True Where ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public int CountResidentByBuildingId(string buildingId)
        {
            string sql = "SELECT COUNT(*) FROM Resident where BuildingId=@BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    return Convert.ToInt32(reader[0]);

                }
            }
            return 0;
        }
        private string GetSalt(int length)
        {
            byte[] bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }
        private int GetRandomNumber()
        {
            Random rand = new Random();
            return rand.Next(8,16);
        }
        public bool UpdatePhotoById(string residentId,string extension)
        {
            string sql = $"Update Resident SET ResidentImage = '{residentId}.{extension}' WHERE ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public string GetPhotoById(string residentId)
        {
            string sql = "Select ResidentImage From Resident Where ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            using(IDataReader dataRedaer = this.dbHelperOleDb.Select(sql))
            {
                if(dataRedaer.Read())
                {
                    return Convert.ToString(dataRedaer["ResidentImage"]);
                }
                return null;
            }
        }
        public List<Resident> findResidentsByOption(string optionId)
        {
            string sql = @"Select Resident.* From Resident INNER JOIN Vote On Resident.ResidentId = Vote.ResidentId WHERE OptionId = @OptionId";
            this.dbHelperOleDb.AddParameter("@OptionId", optionId);
            List<Resident> residents = new List<Resident>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {
                    residents.Add(this.modelCreator.CreateModel<Resident>(reader));
                }
            }
            return residents;
        }
        public bool UpdateAdminRole(string residentId,bool IsAdmin)
        {
            string sql = @$"Update Resident Set IsAdmin=@IsAdmin Where ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@IsAdmin", IsAdmin);
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public bool JoinBuildingUpdate(string residentId,string buildingId, int unitNumber)
        {
            string sql = @"Update Resident Set UnitNumber = @UnitNumber, BuildingId = @BuildingId where ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@UnitNumber", unitNumber);
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            return this.dbHelperOleDb.Update(sql) > 0;
        }

    }
}
