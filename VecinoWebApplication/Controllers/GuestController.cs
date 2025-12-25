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
        public IActionResult RegisterForm()
        {
            Resident resident = new Resident();
            return View(resident);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Resident resident)
        {
            ApiClient<Resident> client = new ApiClient<Resident>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/Register";

            bool residentAddedResponse = await client.PostAsync(resident);
            if(residentAddedResponse)
            {
                ApiClient<string> client2 = new ApiClient<string>();
                client2.Scheme = "http";
                client2.Host = "localhost";
                client2.Port = 5269;
                client2.Path = "api/Resident/Login";
                client2.AddParameter("email", resident.ResidentEmail);
                client2.AddParameter("password", resident.ResidentPassword);
                string residentId = await client2.GetAsync();
                return RedirectToAction("ViewDashboard", new { residentId = residentId });
            }
            return View("RegisterForm");

        }
    }
}
