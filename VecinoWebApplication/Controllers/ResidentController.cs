using Microsoft.AspNetCore.Mvc;
using VecinoBuildingMangement;
using BuildingManagementWsClient;
using VecinoBuildingMangement.ViewModels;

namespace VecinoWebApplication.Controllers
{
    public class ResidentController : Controller
    {
        [HttpGet]
        public IActionResult ResidentBuildingPage(string residentId)
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewSerivceRequests(string residentId)
        {
            ApiClient<ServiceRequestViewModel> client = new ApiClient<ServiceRequestViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/GetServiceRequest";
            client.AddParameter("residentId", residentId);
            ServiceRequestViewModel serviceRequestViewModel = await client.GetAsync();
            return View(serviceRequestViewModel);
        }
        public async Task<IActionResult> ViewManagePayment(string residentId)
        {
            ApiClient<ManagePaymentViewModel> client = new ApiClient<ManagePaymentViewModel>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/GetManagePayment";
            client.AddParameter("residentId", residentId);
            ManagePaymentViewModel managePaymentViewModel = await client.GetAsync();
            return View(managePaymentViewModel);


        }
    }
}
