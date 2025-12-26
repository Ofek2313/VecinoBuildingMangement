using Microsoft.AspNetCore.Mvc;
using VecinoBuildingMangement;
using BuildingManagementWsClient;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangement.Models;

namespace VecinoWebApplication.Controllers
{
    public class ResidentController : Controller
    {
     
    
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
        [HttpGet]
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
        [HttpGet]
        public IActionResult CreateServiceRequestForm()
        {
            ServiceRequest serviceRequest = new ServiceRequest();
            return View(serviceRequest);

        }
        [HttpPost]
        public async Task<IActionResult> CreateServiceRequest(ServiceRequest serviceRequest)
        {

            ApiClient<ServiceRequest> client = new ApiClient<ServiceRequest>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/OpenServiceRequest";

            bool response = await client.PostAsync(serviceRequest);
            
            if(response)
                return RedirectToAction("CreateServiceRequestForm", new {residentid = serviceRequest.ResidentId});

            return View(serviceRequest);
        }
        [HttpGet]
        public async Task<IActionResult> ViewPolls(string buildingId)
        {
            ApiClient<List<PollViewModel>> client = new ApiClient<List<PollViewModel>>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/PollViewModel";
            client.AddParameter("buildingId", buildingId);
            List<PollViewModel> polls = await client.GetAsync();
            return View(polls);
        }
        [HttpGet]
        public async Task<IActionResult> ViewDashboard(string residentId)
        {
            ApiClient<MainpageViewModel> client = new ApiClient<MainpageViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/MainpageViewModel";
            client.AddParameter("residentId", residentId);
            MainpageViewModel mainpage = await client.GetAsync();
            return View(mainpage);

        }

        public IActionResult JoinBuildingForm()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> JoinBuilding(string residentId,string buildingCode)
        {
            
            ApiClient<bool> client = new ApiClient<bool>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/JoinBuilding";

            client.AddParameter("residentId", residentId);
            client.AddParameter("buildingCode", buildingCode);

            bool response = await client.GetAsync();

            if (response) return RedirectToAction("ViewDashboard", new { residentId = residentId });


            ViewBag["Error"] = true;
            return RedirectToAction("JoinBuildingForm"); 
        }

        [HttpGet]
        public IActionResult LoginForm()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> ResidentLogIn(LogInViewModel logInViewModel)
        //{
        //    ApiClient<bool> client = new ApiClient<bool>();
        //    client.Scheme = "http";
        //    client.Host = "localhost";
        //    client.Port = 5269;
        //    client.Path = "api/Resident/Login";
           
        //    bool response = await client.PostAsync();
        //    if(residentId != null)
        //    {
        //        HttpContext.Session.SetString("residentId", residentId);
        
        //        TempData["residentId"] = residentId;
        //        return RedirectToAction("ViewDashboard", new { residentId = residentId });
        //    }

        //    ViewBag["Error"] = true;
        //    return RedirectToAction("LoginForm");
        //}

        [HttpGet]
        public async Task<IActionResult> LeaveBuilding(string residentId)
        {
            ApiClient<bool> client = new ApiClient<bool>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/LeaveBuilding";
            client.AddParameter("residentId", residentId);

            bool leftBuilding = await client.GetAsync();

            if(leftBuilding) return RedirectToAction("ViewDashboard", new { residentId = residentId });

            ViewBag["Error"] = true;
            return RedirectToAction("JoinBuildingForm");


        }
    }
}
