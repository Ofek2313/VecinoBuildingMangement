using VecinoBuildingMangement.Models;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.Json;
using VecinoBuildingMangementWebService;
using System.Data.SqlTypes;
using System.Data;
using System.Security.Cryptography;
using System.Text;
namespace Testing
{
    internal class Program
    {
        static void CheckCreator()
        {
            string sql = "Select * from Resident where ResidentId = 1";
            DbHelperOleDb dbHelperOleDb = new DbHelperOleDb();
            dbHelperOleDb.OpenConnection();
            IDataReader dataReader = dbHelperOleDb.Select(sql);
            dataReader.Read();
            ModelCreators modelCreators = new ModelCreators();
            Resident resident = modelCreators.ResidentCreator.CreateModel(dataReader);
            dbHelperOleDb.CloseConnection();
            Console.WriteLine($"{resident.ResidentName}, {resident.ResidentEmail}");

        }
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
        static void Main(string[] args)
        {

            //CheckCreate();
            //CheckCreator();
            //Console.WriteLine("Enter City Name: ");
            //string city = Console.ReadLine();
            //CurrentWeather(city);
            for (int i = 1; i <= 10; i++) {
                Console.WriteLine("Insert password");
                string password = Console.ReadLine();
                string salt = GetSalt(GetRandomNumber());
                string hash = GetHash(password, salt);
                Console.WriteLine("salt: " + salt);
                Console.WriteLine("hash: " + hash);
            }
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

        static async Task CurrentWeather(string city)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://open-weather13.p.rapidapi.com/city?city={city}&lang=EN"),
                Headers =
    {
        { "x-rapidapi-key", "2e7ae9391bmsh986aa861799c8eap1309edjsn48487fbebaa3" },
        { "x-rapidapi-host", "open-weather13.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
  
                WeatherResponse weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(body);

                Console.WriteLine($"Weather: {weatherResponse.weather[0].main}, {weatherResponse.weather[0].description}, Tempeture: {Math.Round((weatherResponse.main.temp-32)/1.8)}");
            }
        }
    }
}
