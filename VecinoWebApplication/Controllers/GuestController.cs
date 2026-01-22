using BuildingManagementWsClient;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoWebApplication.Controllers
{
    public class GuestController : Controller
    {
        public IActionResult HomePage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewBuildingCatalogue(string cityId = null,int page = 0)
        {
            ApiClient<BuildingCatalouge> client = new ApiClient<BuildingCatalouge>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Guest/GetBuildingCatalogue";
            if (cityId != null) 
                client.AddParameter("cityId", cityId);
            client.AddParameter("page", page.ToString());
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
        public async Task<IActionResult> Register(Resident resident/*,IFormFile file*/)
        {
            if (!resident.IsValid)
                return View("RegisterForm", resident);
     
            ApiClient<Resident> client = new ApiClient<Resident>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Guest/Register";

            Resident resident1 = await client.PostAsyncReturn<Resident,Resident>(resident);
            if (resident1 == null)
                return View("RegisterForm");
            if (resident1.ResidentId != null && resident1.ResidentId != "")
            {
                HttpContext.Session.SetString("residentId", resident1.ResidentId);
                ViewBag.IsLoggedIn = true;
                return View("Homepage");
            }
            ViewBag.Error = true;
            return View("RegisterForm", resident);



        }
       
    }
}
