using BuildingManagementWsClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using VecinoBuildingMangement;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

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
            ApiClient<ViewPollViewModel> client = new ApiClient<ViewPollViewModel>();
            string residentId = HttpContext.Session.GetString("residentId");
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/PollViewModel";
            client.AddParameter("residentId", residentId);
            ViewPollViewModel polls = await client.GetAsync();
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
            HttpContext.Session.SetString("residentName", resident.ResidentName);
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
            ApiClient<NotificationsViewModels> client = new ApiClient<NotificationsViewModels>();
            string residentId = HttpContext.Session.GetString("residentId");
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/GetNotifications";
            client.AddParameter("residentId", residentId);
            NotificationsViewModels viewNotification = await client.GetAsync();
            return View(viewNotification);

        }

        [HttpGet]
        public IActionResult Logout()
        {
           
            HttpContext.Session.Remove("residentId");

            return RedirectToAction("HomePage", "Guest");
          
        }

        [HttpGet]
        public async Task<IActionResult> AttendEvent(string eventId)
        {
            ApiClient<AttendEvent> client = new ApiClient<AttendEvent>();
            string residentId = HttpContext.Session.GetString("residentId");
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/AttendEvent";
            AttendEvent attendEvent = new AttendEvent();
            attendEvent.eventId = eventId;
            attendEvent.residentId = residentId;

            bool result = await client.PostAsyncReturn<AttendEvent,bool>(attendEvent);
            if(result)
            {
                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> VoteInPoll([FromBody] VoteRequest voteRequest)
        {
            ApiClient<Vote> client = new ApiClient<Vote>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/VoteInPoll";
            string residentId = HttpContext.Session.GetString("residentId");
            Vote vote = new Vote();
            vote.PollId = voteRequest.PollId;
            vote.OptionId = voteRequest.OptionId;
            vote.ResidentId = residentId;
            vote.VoteDate = DateTime.Now.ToShortDateString();
            vote.VoteId = "0";
            bool result = await client.PostAsync(vote);
            if (result)
            {
                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> PayFee([FromBody] PayFeeRequest payFeeRequest)
        {
            ApiClient<PayFeeRequest> client = new ApiClient<PayFeeRequest>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/PayFee";
            bool result = await client.PostAsync(payFeeRequest);
            if (result)
            {
                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }
    }
}
