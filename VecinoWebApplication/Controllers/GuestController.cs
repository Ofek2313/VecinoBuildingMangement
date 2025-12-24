using BuildingManagementWsClient;
using Microsoft.AspNetCore.Mvc;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoWebApplication.Controllers
{
    public class GuestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewBuildingCatalogue(string cityId = null,string page = "0")
        {
            ApiClient<BuildingCatalouge> client = new ApiClient<BuildingCatalouge>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/GetServiceRequest";
            client.AddParameter("cityId", cityId);
            client.AddParameter("page", page);
            BuildingCatalouge buildingCatalouge = await client.GetAsync();
            return View(buildingCatalouge);
        }
        [HttpGet]
        public IActionResult Register()
        {
            Resident resident = new Resident();
            return View(resident);
        }

        //[HttpPost] - Fix Needed
        //public async Task<IActionResult> Register(Resident resident)
        //{
        //    ApiClient<Resident> client = new ApiClient<Resident>();
        //    client.Scheme = "http";
        //    client.Host = "localhost";
        //    client.Port = 5269;
        //    client.Path = "api/Resident/Register";

        //    bool response = await client.PostAsync(resident);

         
        //}
    }
}
