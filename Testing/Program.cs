using VecinoBuildingMangement.Models;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.Json;
namespace Testing
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Enter City Name: ");
            string city = Console.ReadLine();
            CurrentWeather(city);
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
