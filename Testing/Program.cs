using VecinoBuildingMangement.Models;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.Json;
using VecinoBuildingMangementWebService;
using System.Data.SqlTypes;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using BuildingManagementWsClient;
using VecinoBuildingMangement.ViewModels;

namespace Testing
{
    internal class Program
    {
        //static void CheckCreator()
        //{
        //    string sql = "Select * from Resident where ResidentId = 1";
        //    DbHelperOleDb dbHelperOleDb = new DbHelperOleDb();
        //    dbHelperOleDb.OpenConnection();
        //    IDataReader dataReader = dbHelperOleDb.Select(sql);
        //    dataReader.Read();
        //    ModelCreators modelCreators = new ModelCreators();
        //    Resident resident = modelCreators.ResidentCreator.CreateModel(dataReader);
        //    dbHelperOleDb.CloseConnection();
        //    Console.WriteLine($"{resident.ResidentName}, {resident.ResidentEmail}");

        //}
        static void CheckInsert()
        {

            Console.WriteLine("Enter City Name");
            string city = Console.ReadLine();
            DbHelperOleDb dbHelperOleDb = new DbHelperOleDb();
            string sql = $"Insert into Cities(CityName) Values('{city}')";
            dbHelperOleDb.OpenConnection();
            
            dbHelperOleDb.Insert(sql);
            
            
            dbHelperOleDb.CloseConnection();
        }
        static void CheckCreate()
        {
            RepositoryUOW repositoryUOW = new RepositoryUOW();


            
            repositoryUOW.DbHelperOleDb.OpenConnection();
            List<Resident> residents = repositoryUOW.ResidentRepository.GetResidentByBuilding("1");
            foreach(Resident res in residents)
            {
                Console.WriteLine($"{res.ResidentName}, {res.UnitNumber}");
            }
            repositoryUOW.DbHelperOleDb.CloseConnection();


        }
        static string GetSalt(int length)
        {
            byte[] bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);

            return Convert.ToBase64String(bytes);
        }
        static string GetHash(string password, string salt)
        {
            string combine = password + salt;
            byte[] bytes = Encoding.UTF8.GetBytes(combine);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        static int GetRandomNumber()
        {
            Random rand = new Random();
            return rand.Next(8, 16);
        }
        static async void TestVecinoClient(string residentId,string buildingCode)
        {
           
            ApiClient<object> client = new ApiClient<object>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/JoinBuilding";

            client.AddParameter("residentId", residentId);
            client.AddParameter("JoinBuilding", buildingCode);

            bool response = await client.PostAsync(null);


            Console.WriteLine(response);

        }
        public static void Foo( ref int x)
        {
            x += 1;
            Console.WriteLine(x);
        }
        static void Main(string[] args)
        {





            int x = 5;
            Foo(ref x);
            Console.WriteLine(x);
            
            Console.ReadLine();

        }

        static void TestResident()
        {

            Resident resident = new Resident();
            resident.ResidentId = "1";
            resident.ResidentName = "Ofek Cohen";
            resident.ResidentPhone = "1234567890";
            resident.ResidentEmail = "example@gmail.com";
            resident.UnitNumber = 1;
            resident.BuildingId = "1";

            if (resident.HasErrors)
            {
                foreach (KeyValuePair<string, List<string>> keyValuePair in resident.AllErrors())
                {
                    Console.WriteLine(keyValuePair.Key);
                    foreach (string str in keyValuePair.Value)
                    {
                        Console.WriteLine($"         {str}");
                    }
                    Console.WriteLine("-----------------------------------");
                }
            }

        }   

       
       
    }
}
