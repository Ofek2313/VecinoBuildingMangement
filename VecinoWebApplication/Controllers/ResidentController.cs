using Microsoft.AspNetCore.Mvc;
using VecinoBuildingMangement;
using BuildingManagementWsClient;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangement.Models;
using Microsoft.AspNetCore.Authentication;

namespace VecinoWebApplication.Controllers
{
    public class ResidentController : Controller
    {
     
    
        [HttpGet]
        public async Task<IActionResult> ViewSerivceRequests()
        {
            ApiClient<ServiceRequestViewModel> client = new ApiClient<ServiceRequestViewModel>();
            string residentId = HttpContext.Session.GetString("residentId");
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/GetServiceRequest";
            client.AddParameter("residentId", residentId);
            ServiceRequestViewModel serviceRequestViewModel = await client.GetAsync();
            return View(serviceRequestViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> ViewManagePayment()
        {
            ApiClient<ManagePaymentViewModel> client = new ApiClient<ManagePaymentViewModel>();
            string residentId = HttpContext.Session.GetString("residentId");
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/GetManagePayment";
            client.AddParameter("residentId", residentId);
            ManagePaymentViewModel managePaymentViewModel = await client.GetAsync();
            
            return View(managePaymentViewModel);


        }
        [HttpGet]
        public async Task<IActionResult> ViewEvents()
        {
            ApiClient<ViewEventViewModel> client = new ApiClient<ViewEventViewModel>();
            string residentId = HttpContext.Session.GetString("residentId");
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/ViewEvents";
            client.AddParameter("residentId", residentId);
            ViewEventViewModel viewEventViewModel = await client.GetAsync();
            return View(viewEventViewModel);
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
            Console.WriteLine($"Incoming serviceRequest.RequestId: '{serviceRequest.RequestId}'");

            serviceRequest.ResidentId = HttpContext.Session.GetString("residentId");
            serviceRequest.RequestStatus = "Pending";
            serviceRequest.RequestDate = DateTime.Now.ToShortDateString();
            serviceRequest.RequestId = "";
            ApiClient<ServiceRequest> client = new ApiClient<ServiceRequest>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/OpenServiceRequest";

            bool response = await client.PostAsync(serviceRequest);
           
            
            return RedirectToAction("ViewSerivceRequests");

        
        }
        [HttpGet]
        public async Task<IActionResult> ViewPolls()
        {
            ApiClient<List<PollViewModel>> client = new ApiClient<List<PollViewModel>>();
            string residentId = HttpContext.Session.GetString("residentId");
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/PollViewModel";
            client.AddParameter("residentId", residentId);
            List<PollViewModel> polls = await client.GetAsync();
            return View(polls);
        }
        [HttpGet]
        public async Task<IActionResult> ViewDashboard()
        {

            MainpageViewModel mainpage = await GetTaskMainPage();
            return View(mainpage);

        }
        private async Task<MainpageViewModel> GetTaskMainPage()
        {
            ApiClient<MainpageViewModel> client = new ApiClient<MainpageViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/Mainpage";
            client.AddParameter("residentId", HttpContext.Session.GetString("residentId"));
            return await client.GetAsync();

        }
        public IActionResult JoinBuildingForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> JoinBuilding(string buildingCode)
        {
            
            ApiClient<JoinBuildingRequest> client = new ApiClient<JoinBuildingRequest>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/JoinBuilding";
                
            string residentId = HttpContext.Session.GetString("residentId");
           
            JoinBuildingRequest joinBuildingRequest = new JoinBuildingRequest();
            joinBuildingRequest.buildingCode = buildingCode;
            joinBuildingRequest.residentId = residentId;

            bool response = await client.PostAsync(joinBuildingRequest);

            if (response) return RedirectToAction("ViewDashboard");


          
            return View("JoinBuildingForm"); 
        }

        [HttpGet]
        public IActionResult LoginForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResidentLogIn(LogInViewModel logInViewModel)
        {
            ApiClient<LogInViewModel> client = new ApiClient<LogInViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/Login";
            Resident resident = await client.PostAsyncReturn<LogInViewModel, Resident>(logInViewModel);

            if(resident == null)
                return View("LoginForm");
            
            HttpContext.Session.SetString("residentId", resident.ResidentId);
            ViewBag.IsLoggedIn = true;
            //ApiClient<BuildingModel> client2 = new ApiClient<BuildingModel>();
            //client2.Scheme = "https";
            //client2.Host = "localhost";
            //client2.Port = 5269;
            //client2.Path = "api/Resident/GetBuildingId";
            //client2.AddParameter("residentId", resident.ResidentId);
            //BuildingModel buildingModel = await client2.GetAsync();

             return RedirectToAction("ViewDashboard");
            
             return RedirectToAction("HomePage", "Guest");
            
            

        }

        [HttpGet]
        public async Task<IActionResult> LeaveBuilding()
        {
            ApiClient<bool> client = new ApiClient<bool>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/LeaveBuilding";

            string residentId = HttpContext.Session.GetString("residentId");
            client.AddParameter("residentId", residentId);



            bool leftBuilding = await client.GetAsync();

            if(leftBuilding) return RedirectToAction("Homepage","Guest");
            MainpageViewModel mainpage = await GetTaskMainPage();
            return View("ViewDashboard", mainpage);
          


        }
        [HttpGet]
        public async Task<IActionResult> ViewAnnouncement()
        {
            ApiClient<List<Notification>> client = new ApiClient<List<Notification>>();
            string residentId = HttpContext.Session.GetString("residentId");
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/GetNotifications";
            client.AddParameter("residentId", residentId);
            List<Notification> viewNotification = await client.GetAsync();
            return View(viewNotification);

        }

        [HttpGet]
        public IActionResult Logout()
        {
           
            HttpContext.Session.Remove("residentId");

            return RedirectToAction("HomePage", "Guest");
          
        }
    }
}
