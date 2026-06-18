using BuildingManagementWsClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VecinoBuildingMangement;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoWebApplication.Controllers
{
    // Handles all resident-related UI actions and communicates with the backend Web API
    public class ResidentController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> ViewSerivceRequests()
        {
            try
            {
                ApiClient<ServiceRequestViewModel> client = new ApiClient<ServiceRequestViewModel>();
                string residentId = HttpContext.Session.GetString("residentId");

                // Configure API endpoint for service requests
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/GetServiceRequest";
                client.AddParameter("residentId", residentId);

                ServiceRequestViewModel serviceRequestViewModel = await client.GetAsync();
                if (serviceRequestViewModel != null)
                    return View(serviceRequestViewModel);

                TempData["ErrorMessage"] = "Unable To Load Page";
                return RedirectToAction("ViewDashboard");
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("ViewDashboard");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewManagePayment(int page = 1,int pageUnpaid = 1)
        {
            try
            {
                ApiClient<ManagePaymentViewModel> client = new ApiClient<ManagePaymentViewModel>();
                string residentId = HttpContext.Session.GetString("residentId");

                // Payment pagination request
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/GetManagePayment";
                client.AddParameter("residentId", residentId);
                client.AddParameter("page", page.ToString());
                client.AddParameter("pageUnpaid", pageUnpaid.ToString());
                ManagePaymentViewModel managePaymentViewModel = await client.GetAsync();
                if (managePaymentViewModel != null)
                    return View(managePaymentViewModel);

                TempData["ErrorMessage"] = "Unable To Load Page";
                return RedirectToAction("ViewDashboard");
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("ViewDashboard");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewEvents()
        {
            try
            {
                ApiClient<ViewEventViewModel> client = new ApiClient<ViewEventViewModel>();
                string residentId = HttpContext.Session.GetString("residentId");

                // Load events for current resident
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/ViewEvents";
                client.AddParameter("residentId", residentId);

                ViewEventViewModel viewEventViewModel = await client.GetAsync();
                if (viewEventViewModel != null)
                    return View(viewEventViewModel);

                TempData["ErrorMessage"] = "Unable To Load Page";
                return RedirectToAction("ViewDashboard");
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("ViewDashboard");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEventPhoto(string eventId)
        {
            try
            {
                ApiClient<bool> client = new ApiClient<bool>();

                // Fetch event image from API
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/GetEventPhoto";
                client.AddParameter("eventId", eventId);

                (byte[] bytes, string contentType) = await client.GetFileAsync();
                if (bytes != null || contentType != null)
                    return File(bytes, contentType);

                return BadRequest();
            }
            catch
            {
                // Fallback image if API is unreachable
                string fallbackPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "event0.png");
                byte[] fallbackBytes = await System.IO.File.ReadAllBytesAsync(fallbackPath);
                return File(fallbackBytes, "image/png");
            }
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
            serviceRequest.ResidentId = HttpContext.Session.GetString("residentId");

            // Default values before sending to API
            serviceRequest.RequestStatus = RequestStatus.Pending;
            serviceRequest.RequestDate = DateTime.Now.ToShortDateString();
            serviceRequest.RequestId = "";

            try
            {
                ApiClient<ServiceRequest> client = new ApiClient<ServiceRequest>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/OpenServiceRequest";

                bool response = await client.PostAsync(serviceRequest);

                return RedirectToAction("ViewSerivceRequests");
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("CreateServiceRequestForm");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewPolls()
        {
            try
            {
                ApiClient<List<PollViewModel>> client = new ApiClient<List<PollViewModel>>();
                string residentId = HttpContext.Session.GetString("residentId");

                // Load polls for resident
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/PollViewModel";
                client.AddParameter("residentId", residentId);

                List<PollViewModel> polls = await client.GetAsync();
                if (polls != null)
                    return View(polls);

                TempData["ErrorMessage"] = "Unable To Load Page";
                return RedirectToAction("ViewDashboard");
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("ViewDashboard");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewDashboard()
        {
            try
            {
                MainpageViewModel mainpage = await GetTaskMainPage();

                // If session or data invalid, force logout flow
                if (mainpage != null)
                    return View(mainpage);
                else
                {
                    HttpContext.Session.Clear();
                    TempData["ErrorMessage"] = "Unable To Load Page";
                    return RedirectToAction("HomePage", "Guest");
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("HomePage", "Guest");
            }
        }

        [HttpGet]
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
        public async Task<IActionResult> JoinBuilding(JoinBuildingRequest buildingRequest)
        {
            try
            {
                ApiClient<JoinBuildingRequest> client = new ApiClient<JoinBuildingRequest>();

                // Attach current resident to request
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/JoinBuilding";

                string residentId = HttpContext.Session.GetString("residentId");
                buildingRequest.ResidentId = residentId;

                bool response = await client.PostAsync(buildingRequest);

                if (response) return RedirectToAction("ViewDashboard");

                return View("JoinBuildingForm");
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("JoinBuildingForm");
            }
        }

        [HttpGet]
        public IActionResult LoginForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResidentLogIn(LogInViewModel logInViewModel)
        {
            try
            {
                ApiClient<LogInViewModel> client = new ApiClient<LogInViewModel>();

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/Login";

                ApiResponse<Resident> resident =
                    await client.PostAsyncReturn<LogInViewModel, Resident>(logInViewModel);

                if (resident == null || !resident.Success)
                    return View("LoginForm", logInViewModel);

                // Store session data after successful login
                HttpContext.Session.SetString("residentId", resident.Data.ResidentId);
                HttpContext.Session.SetString("residentName", resident.Data.ResidentName);

                ViewBag.IsLoggedIn = true;

                // Redirect based on building assignment
                if (resident.Data.BuildingId != "0")
                    return RedirectToAction("ViewDashboard");
                else
                    return RedirectToAction("HomePage", "Guest");
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("LoginForm");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LeaveBuilding()
        {
            try
            {
                ApiClient<bool> client = new ApiClient<bool>();

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/LeaveBuilding";

                string residentId = HttpContext.Session.GetString("residentId");
                client.AddParameter("residentId", residentId);

                bool leftBuilding = await client.GetAsync();

                if (leftBuilding) return RedirectToAction("Homepage", "Guest");

                MainpageViewModel mainpage = await GetTaskMainPage();
                if (mainpage != null)
                    return View("ViewDashboard", mainpage);

                TempData["ErrorMessage"] = "Unable To Load Page";
                return RedirectToAction("ViewDashboard");
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("ViewDashboard");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewAnnouncement()
        {
            try
            {
                ApiClient<NotificationsViewModels> client = new ApiClient<NotificationsViewModels>();
                string residentId = HttpContext.Session.GetString("residentId");

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/GetNotifications";
                client.AddParameter("residentId", residentId);

                NotificationsViewModels viewNotification = await client.GetAsync();
                if (viewNotification != null)
                    return View(viewNotification);

                TempData["ErrorMessage"] = "Unable To Load Page";
                return RedirectToAction("ViewDashboard");
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("ViewDashboard");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("residentId");
            return RedirectToAction("HomePage", "Guest");
        }

    

        public async Task<IActionResult> GetBookingPage(string date)
        {
            try
            {
                ApiClient<BookingViewModel> client = new ApiClient<BookingViewModel>();
                string residentId = HttpContext.Session.GetString("residentId");

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/GetAllBookings";
                client.AddParameter("residentId", residentId);
                client.AddParameter("date", date);

                BookingViewModel bookings = await client.GetAsync();
                if (bookings != null)
                    return View(bookings);

                TempData["ErrorMessage"] = "Unable To Load Page";
                return RedirectToAction("ViewDashboard");
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("ViewDashboard");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AttendEvent([FromQuery] string eventId)
        {
            try
            {
                ApiClient<AttendEvent> client = new ApiClient<AttendEvent>();
                string residentId = HttpContext.Session.GetString("residentId");

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/AttendEvent";

                AttendEvent attendEvent = new AttendEvent { eventId = eventId, residentId = residentId };

                ApiResponse<bool> result =
                    await client.PostAsyncReturn<AttendEvent, bool>(attendEvent);

                return Json(new { success = result.Data && result.Success });
            }
            catch
            {
                return Json(new { success = false, message = "Network Error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UnAttendEvent([FromQuery] string eventId)
        {
            try
            {
                ApiClient<AttendEvent> client = new ApiClient<AttendEvent>();
                string residentId = HttpContext.Session.GetString("residentId");

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/UnAttendEvent";

                AttendEvent attendEvent = new AttendEvent { eventId = eventId, residentId = residentId };

                ApiResponse<bool> result =
                    await client.PostAsyncReturn<AttendEvent, bool>(attendEvent);

                return Json(new { success = result.Data && result.Success });
            }
            catch
            {
                return Json(new { success = false, message = "Network Error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> VoteInPoll([FromBody] VoteRequest voteRequest)
        {
            try
            {
                ApiClient<Vote> client = new ApiClient<Vote>();

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/VoteInPoll";

                string residentId = HttpContext.Session.GetString("residentId");

                //Construct Vote Object
                Vote vote = new Vote
                {
                    PollId = voteRequest.PollId,
                    OptionId = voteRequest.OptionId,
                    ResidentId = residentId,
                    VoteDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    VoteId = "0" //Temp Value
                };

                ApiResponse<List<OptionViewModel>> apiResponse =
                    await client.PostAsyncReturn<Vote, List<OptionViewModel>>(vote);

                if (apiResponse.Success && apiResponse.Data != null)
                    return Json(new { success = true, data = apiResponse.Data });

                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false, message = "Network Error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UnVoteInPoll([FromQuery] string pollId)
        {
            try
            {
                ApiClient<VoteDeleteRequest> client = new ApiClient<VoteDeleteRequest>();

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/UnVoteInPoll";

                VoteDeleteRequest voteDeleteRequest = new VoteDeleteRequest
                {
                    PollId = pollId,
                    ResidentId = HttpContext.Session.GetString("residentId")
                };

                ApiResponse<bool> apiResponse =
                    await client.PostAsyncReturn<VoteDeleteRequest, bool>(voteDeleteRequest);

                if (apiResponse.Success)
                    return Json(new { success = apiResponse.Data });

                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false, message = "Network Error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PayFee([FromBody] PayFeeRequest payFeeRequest)
        {
            try
            {
                ApiClient<PayFeeRequest> client = new ApiClient<PayFeeRequest>();

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/PayFee";

                bool result = await client.PostAsync(payFeeRequest);
                return Json(new { success = result });
            }
            catch
            {
                return Json(new { success = false, message = "Network Error" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewProfile()
        {
            try
            {
                ApiClient<Resident> client = new ApiClient<Resident>();
                string residentId = HttpContext.Session.GetString("residentId");

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/GetResident";
                client.AddParameter("residentId", residentId);

                Resident resident = await client.GetAsync();
                return View(resident);
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("ViewDashboard");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            string residentId = HttpContext.Session.GetString("residentId");

            ApiClient<ViewModelAvatar> client = new ApiClient<ViewModelAvatar>();

            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/UploadPhoto";

            ViewModelAvatar viewModelAvatar = new ViewModelAvatar
            {
                ResidentId = residentId,
                Extension = Path.GetExtension(file.FileName).TrimStart('.')
            };

            try
            {
                ApiResponse<bool> result =
                    await client.PostAsyncReturn<ViewModelAvatar, bool>(
                        viewModelAvatar,
                        file.OpenReadStream(),
                        file.FileName);

                if (!result.Success || !result.Data)
                    TempData["ErrorMessage"] = "Upload failed. The server rejected the file.";
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while connecting to the server.";
            }

            return RedirectToAction("ViewProfile");
        }

        [HttpGet]
        public async Task<IActionResult> GetPhoto()
        {
            try
            {
                string residentId = HttpContext.Session.GetString("residentId");
                ApiClient<bool> client = new ApiClient<bool>();

                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Resident/GetPhoto";
                client.AddParameter("residentId", residentId);

                (byte[] bytes, string contentType) = await client.GetFileAsync();
                if (bytes != null || contentType != null)
                    return File(bytes, contentType);

                return BadRequest();
            }
            catch
            {
                // fallback avatar if API fails
                string fallbackPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "image.png");
                byte[] fallbackBytes = await System.IO.File.ReadAllBytesAsync(fallbackPath);
                return File(fallbackBytes, "image/png");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Resident resident)
        {
            if (!ModelState.IsValid)
                return View("ViewProfile", resident);

            try
            {
                ApiClient<Resident> client2 = new ApiClient<Resident>();

                client2.Scheme = "http";
                client2.Host = "localhost";
                client2.Port = 5269;
                client2.Path = "api/Resident/UpdateResident";

                ApiResponse<bool> response =
                    await client2.PostAsyncReturn<Resident, bool>(resident);

                if (response.Success && response.Data)
                {
                    // keep session name in sync with updated profile
                    HttpContext.Session.SetString("residentName", resident.ResidentName);
                }
                else
                {
                    TempData["ErrorMessage"] = "Update failed";
                }

                return RedirectToAction("ViewProfile");
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("ViewProfile");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(Booking booking)
        {
            booking.ResidentId = HttpContext.Session.GetString("residentId");
            
            // normalize date format for backend consistency
            if (DateTime.TryParse(booking.BookingDate, out DateTime parsedDate))
            {
                booking.BookingDate = parsedDate.ToString("dd/MM/yyyy");
            }

            booking.BookingStatus = BookingStatus.AWAITING_APPROVAL;
            
            try
            {
                ApiClient<Booking> apiClient = new ApiClient<Booking>();

                apiClient.Scheme = "http";
                apiClient.Host = "localhost";
                apiClient.Port = 5269;
                apiClient.Path = "api/Resident/CreateBooking";

                ApiResponse<bool> apiResponse =
                    await apiClient.PostAsyncReturn<Booking, bool>(booking);

                if (!apiResponse.Success || !apiResponse.Data)
                    TempData["ErrorMessage"] = "Unable To Create Booking";

                return RedirectToAction("GetBookingPage", new { date = booking.BookingDate });
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("GetBookingPage", new { date = booking.BookingDate });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelBooking(string bookingId, string date)
        {
            try
            {
                ApiClient<Booking> apiClient = new ApiClient<Booking>();

                apiClient.Scheme = "http";
                apiClient.Host = "localhost";
                apiClient.Port = 5269;
                apiClient.Path = "api/Resident/CancelBooking";
                apiClient.AddParameter("bookingId", bookingId);

                ApiResponse<bool> apiResponse =
                    await apiClient.PostAsyncReturn<object, bool>(null);

                if (!apiResponse.Success || !apiResponse.Data)
                    TempData["ErrorMessage"] = "Unable To Cancel Booking";

                return RedirectToAction("GetBookingPage", new { date = date });
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("GetBookingPage", new { date = date });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PayBooking(string bookingId, string date)
        {
            try
            {
                ApiClient<Booking> apiClient = new ApiClient<Booking>();

                apiClient.Scheme = "http";
                apiClient.Host = "localhost";
                apiClient.Port = 5269;
                apiClient.Path = "api/Resident/PayBooking";
                apiClient.AddParameter("bookingId", bookingId);

                ApiResponse<bool> apiResponse =
                    await apiClient.PostAsyncReturn<object, bool>(null);

                if (!apiResponse.Success || !apiResponse.Data)
                    TempData["ErrorMessage"] = "Unable To Pay Booking";

                return RedirectToAction("GetBookingPage", new { date = date });
            }
            catch
            {
                TempData["ErrorMessage"] = "NetWork Error";
                return RedirectToAction("GetBookingPage", new { date = date });
            }
        }
    }
}