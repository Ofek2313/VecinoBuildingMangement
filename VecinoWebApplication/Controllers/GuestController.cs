using BuildingManagementWsClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using System.Text;
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
        public async Task<IActionResult> ViewBuildingCatalogue(int page = 1) //Getting the building catalog by page
        {
            try
            {
                ApiClient<BuildingCatalouge> client = new ApiClient<BuildingCatalouge>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Guest/GetBuildingCatalogue";

                client.AddParameter("page", page.ToString());
                BuildingCatalouge buildingCatalouge = await client.GetAsync();
                return View(buildingCatalouge);
            }
            catch
            {
                return RedirectToAction("HomePage");
            }
            
        }

        [HttpGet]
        public IActionResult RegisterForm() //Getting the register page
        {
            Resident resident = new Resident();
            return View(resident);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Resident resident) //Register Resident into the system
        {
            resident.IsValidationEnabled = false;
            resident.UnitNumber = 1;
            if (!resident.IsValid) //Validation
                return View("RegisterForm", resident);
     
            ApiClient<Resident> client = new ApiClient<Resident>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269  ;
            client.Path = "api/Guest/Register";

            ApiResponse<Resident> resident1 = await client.PostAsyncReturn<Resident,Resident>(resident);
            if (resident1.Data == null || !resident1.Success )
            {
                TempData["ErrorMessage"] = resident1.ErrorMessage;
                return View("RegisterForm", resident);
            }
            if (resident1.Data.ResidentId != null && resident1.Data.ResidentId != "")
            {
                HttpContext.Session.SetString("residentId", resident1.Data.ResidentId);
                HttpContext.Session.SetString("residentName", resident1.Data.ResidentName);
                ViewBag.IsLoggedIn = true;
                return View("Homepage");
            }
            ViewBag.Error = true;
            return View("RegisterForm", resident);



        }
        public async Task<IActionResult> GetBuildingPhoto(string buildingId) //Getting the photo fo the building with bytes
        {
            ApiClient<bool> client = new ApiClient<bool>(); // The types does not matter since working with a file I dont need any model
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Guest/GetBuildingPhoto";
            client.AddParameter("buildingId", buildingId);
            (byte[] bytes, string contentType) = await client.GetFileAsync();
            if (bytes != null || contentType != null)
                return File(bytes, contentType);
            return BadRequest();
        }

        [HttpGet] // Used To get all the relevant building info for map
        public async Task<IActionResult> GetBuildingsMap()
        {
            ApiClient<List<Building>> apiClient = new ApiClient<List<Building>>();
            apiClient.Host = "localhost";
            apiClient.Port = 5269;
            apiClient.Path = "api/Guest/GetBuildingsMap";
            List<Building> buildings = await apiClient.GetAsync();
            return Json(buildings);
        }


    }
}
