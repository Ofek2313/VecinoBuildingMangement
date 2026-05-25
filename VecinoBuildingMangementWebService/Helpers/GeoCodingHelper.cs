using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Testing;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
namespace VecinoBuildingMangementWebService.Helpers
{
    public static class GeoCodingHelper
    {
         private static readonly HttpClient _httpClient = new HttpClient();
        private static string _apiKey;

        public static void Init(IConfiguration configuration)
        {
            _apiKey = configuration["API_KEY"];
        }
        public static async Task<CordsDto> GetCoordinatesAsync(string address)
        {
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.geoapify.com/v1/geocode/search?text={Uri.EscapeDataString(address)}&apiKey={_apiKey}")
            };
            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode(); 
                var body = await response.Content.ReadAsStringAsync();

                GeoApifyReponse apifyReponse = JsonSerializer.Deserialize<GeoApifyReponse>(body);
                if(apifyReponse != null)
                {
                    if (apifyReponse.features != null && apifyReponse.features.Count > 0  )
                    {
                        
                        if (apifyReponse.features[0].geometry?.coordinates != null && apifyReponse.features[0].geometry.coordinates.Count >= 2)
                        {
                            return new CordsDto
                            {
                                Longitude = apifyReponse.features[0].geometry.coordinates[0],
                                Latitude = apifyReponse.features[0].geometry.coordinates[1]
                            };
                        }
                    }
                       
                }
                
                
            }
            return null;
        }
    }
}
