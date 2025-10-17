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

            TopicList();
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
        static async Task TopicList()
        {



            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://currency-conversion-and-exchange-rates.p.rapidapi.com/convert?from=USD&to=EUR&amount=750"),
                Headers =
    {
        { "x-rapidapi-key", "2e7ae9391bmsh986aa861799c8eap1309edjsn48487fbebaa3" },
        { "x-rapidapi-host", "currency-conversion-and-exchange-rates.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {

                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                Currency curr = JsonSerializer.Deserialize<Currency>(body);
                Console.WriteLine($"{curr.query.amount} {curr.query.from} = {curr.result}{curr.query.to}");
            }

        }
    }
}
