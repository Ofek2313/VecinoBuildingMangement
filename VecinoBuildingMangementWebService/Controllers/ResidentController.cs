using BuildingManagementWsClient;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using VecinoBuildingMangement;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoBuildingMangementWebService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ResidentController : ControllerBase
    {
        RepositoryUOW repositoryUOW;

        public ResidentController()
        {
            this.repositoryUOW = new RepositoryUOW();
        }
       
        [HttpGet]
        public ManagePaymentViewModel GetManagePayment(string residentId, int page = 1,int pageUnpaid = 1)
        {
            ManagePaymentViewModel viewModel = new ManagePaymentViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                viewModel.Fees = repositoryUOW.FeeRepository.GetFeesByResidentIdPagePaid(residentId, 5, page);
                viewModel.UnPaid = repositoryUOW.FeeRepository.GetFeesByResidentIdPageUnPaid(residentId, 5, pageUnpaid);
                viewModel.ResidentFeeStats = this.repositoryUOW.FeeRepository.GetResidentFeeStats(residentId);
                viewModel.nextFee = this.repositoryUOW.FeeRepository.GetNextFee(residentId);

            
                viewModel.NumberOfPages = (int)Math.Ceiling(viewModel.ResidentFeeStats.PaidFees / 5.0);
                viewModel.NumberOfPagesUnPaid = (int)Math.Ceiling(viewModel.ResidentFeeStats.UnPaidFees / 5.0);
                viewModel.CurrentPage = page;
                viewModel.CurrentPageUnPaid = pageUnpaid;
                if (viewModel.nextFee != null)
                {
                    if (DateTime.TryParse(viewModel.nextFee.FeeDueDate, out DateTime end))
                    {
                        var span = end - DateTime.Now;
                        viewModel.DaysLeft = (int)Math.Max(0, Math.Ceiling(span.TotalDays));
                    }
                }


                return viewModel;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }


        }

        [HttpPost]
        public bool PayFee([FromBody] PayFeeRequest payFeeRequest)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.FeeRepository.PayFee(payFeeRequest.FeeId);
            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }


        [HttpGet]
        public ServiceRequestViewModel GetServiceRequest(string residentId)
        {
            ServiceRequestViewModel serviceRequestViewModel = new ServiceRequestViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                serviceRequestViewModel.serviceRequests = repositoryUOW.ServiceRequestRepository.GetServiceRequestsByResidentId(residentId);
                serviceRequestViewModel.RequestTypes = repositoryUOW.RequestTypeRepository.GetAll();
                (serviceRequestViewModel.Pending, serviceRequestViewModel.Completed, serviceRequestViewModel.InProgress) =
                    this.repositoryUOW.ServiceRequestRepository.GetServiceRequestSummaryByResidentId(residentId);
               
                
                return serviceRequestViewModel;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }




        [HttpPost]
        public bool OpenServiceRequest([FromBody] ServiceRequest serviceRequest)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();

                return repositoryUOW.ServiceRequestRepository.Create(serviceRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }

        }

        [HttpPost]
        public Resident Login([FromBody] LogInViewModel logInViewModel)
        {

            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                string id = this.repositoryUOW.ResidentRepository.Login(logInViewModel.Email, logInViewModel.Password);
                if (id != null)
                    return this.repositoryUOW.ResidentRepository.GetById(id);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }


        }

        [HttpGet]
        public ViewEventViewModel ViewEvents(string residentId)
        {
            List<Event> events = new List<Event>();
            List<Event> events2 = new List<Event>();
            ViewEventViewModel viewEventViewModel = new ViewEventViewModel();

            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                viewEventViewModel.EventTypes = this.repositoryUOW.EventTypeRepository.GetAll();
                Building building = this.repositoryUOW.BuildingRepository.GetBuildingByResidentId(residentId);
                if (building == null)
                    return null;
                List<EventViewModelResident> eventsview = this.repositoryUOW.EventRepository.GetEventViewModelsByBuildingIdResident(building.BuildingId, residentId);
                viewEventViewModel.CurrEvents = eventsview.Where(e => DateTime.ParseExact(e.Event.EventDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.Today).ToList();
                viewEventViewModel.PreEvents = eventsview.Where(e => DateTime.ParseExact(e.Event.EventDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.Today).ToList();


                return viewEventViewModel;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpPost]
        [Produces("application/json")]
        public bool AttendEvent([FromBody] AttendEvent attendEvent)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.EventRepository.AttendEvent(attendEvent.eventId, attendEvent.residentId);

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpPost]
        public bool UnAttendEvent([FromBody] AttendEvent attendEvent)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.EventRepository.UnAttendEvent(attendEvent.eventId, attendEvent.residentId);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }

        }
        [HttpPost]
        public IActionResult JoinBuilding([FromBody] JoinBuildingRequest joinBuildingRequest)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();

                Building building = this.repositoryUOW.BuildingRepository.GetBuildingByCode(joinBuildingRequest.BuildingCode);
                if (building != null && joinBuildingRequest.UnitNumber <= building.TotalUnits)
                {
                    bool response = this.repositoryUOW.ResidentRepository.JoinBuildingUpdate(joinBuildingRequest.ResidentId, building.BuildingId, joinBuildingRequest.UnitNumber);
                    if (response)
                        return Ok(response);
                    else
                        return BadRequest(new ApiError { ErrorMessage = "Unable To Join Building" });
                }
                   
                else
                    return BadRequest(new ApiError { ErrorMessage = "Unit Number not in building please choose a different number" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError { ErrorMessage = "Internal server error" });
            }
      
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpGet]
        public bool LeaveBuilding(string residentId)
        {

            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();

                bool hasDebt = this.repositoryUOW.FeeRepository.HasDebt(residentId); // cannot leave if fees not paid
                if (hasDebt)
                    return false; 
                Building building = this.repositoryUOW.BuildingRepository.GetBuildingByResidentId(residentId);
                if (building == null || building.BuildingId == "0")
                    return false;

                //Deletes all services request that are related onces the resident left the building
                this.repositoryUOW.ServiceRequestRepository.DeleteByResidentId(residentId);

                //Deletes all notifications that are related to the resident
                this.repositoryUOW.NotificationRepository.DeleteByResidentId(residentId);

                //Set BuildingId to 0 to indicate that resident is not in any building
                this.repositoryUOW.ResidentRepository.UpdateResidentBuilding(residentId, "0");

                this.repositoryUOW.DbHelperOleDb.Commit();
                return true;
            }
            catch (Exception ex)
            {
                this.repositoryUOW.DbHelperOleDb.RollBack();
                return false;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }
        [HttpGet]
        public NotificationsViewModels GetNotifications(string residentId)
        {
            NotificationsViewModels notificationsViewModels = new NotificationsViewModels();

            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                List<Notification> allNotifications = this.repositoryUOW.NotificationRepository.GetNotificationsByResidentId(residentId);
                notificationsViewModels.Notifications = allNotifications.Where(nf => !nf.IsPinned).ToList();
                notificationsViewModels.PinnedNotifications = allNotifications.Where(nf => nf.IsPinned).ToList();
                //this.repositoryUOW.NotificationRepository.GetPinnedNotificationsByResidentId(residentId);
                return notificationsViewModels;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpGet]
        public MainpageViewModel Mainpage(string residentId)
        {
            MainpageViewModel viewModel = new MainpageViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                viewModel.Building = this.repositoryUOW.BuildingRepository.GetBuildingByResidentId(residentId);
                if (viewModel.Building == null)
                    return null;
                viewModel.Notifications = this.repositoryUOW.NotificationRepository.GetAllNotificationsByResidentIdTop(residentId);
                viewModel.events = this.repositoryUOW.EventRepository.GetEventByBuildingIdTop(viewModel.Building.BuildingId);
                return viewModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }
        [HttpGet]
        public List<PollViewModel> PollViewModel(string residentId)
        {


            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                Building building = this.repositoryUOW.BuildingRepository.GetBuildingByResidentId(residentId);
                if (building == null)
                    return null;
                string buildingId = building.BuildingId;

                return this.repositoryUOW.PollRepository.GetPollViewModels(buildingId, residentId);




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }




        [HttpPost]
        public List<OptionViewModel> VoteInPoll(Vote vote)
        {
            List<OptionViewModel> results = new List<OptionViewModel>();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();
                if (this.repositoryUOW.VoteRepository.hasVoted(vote.ResidentId, vote.PollId)) // Creates a new vote record only if the resident has not already voted in this poll.
                    return null;
                Poll poll = this.repositoryUOW.PollRepository.GetById(vote.PollId);
                if (poll == null || !poll.IsActive)
                    return null;
                DateTime pollDate = DateTime.ParseExact(poll.PollDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (pollDate < DateTime.Today)
                    return null;

                bool created = this.repositoryUOW.VoteRepository.Create(vote);
                if (created)
                {
                    results = this.repositoryUOW.PollRepository.GetPollResultById(vote.PollId);
                    this.repositoryUOW.DbHelperOleDb.Commit();
                }
                else
                    this.repositoryUOW.DbHelperOleDb.RollBack();
                return results;
            }
            catch (Exception ex)
            {
                this.repositoryUOW.DbHelperOleDb.RollBack();
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpPost]
        public bool UnVoteInPoll([FromBody] VoteDeleteRequest voteDeleteRequest)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.VoteRepository.DeleteVote(voteDeleteRequest.PollId, voteDeleteRequest.ResidentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }


        [HttpGet]
        public BuildingModel GetBuildingId(string residentId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                Building building = this.repositoryUOW.BuildingRepository.GetBuildingByResidentId(residentId);
                if (building == null)
                    return null;

                BuildingModel buildingModel = new BuildingModel();
                buildingModel.buildingId = building.BuildingId;
                return buildingModel;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpPost]
        public bool UploadPhoto([FromForm] string model, IFormFile file) //Uploads a photo to the webserver and updates the database, since its multipart I need to use FromForm
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();
                ViewModelAvatar viewModelAvatar = JsonSerializer.Deserialize<ViewModelAvatar>(model);
                bool response = this.repositoryUOW.ResidentRepository.UpdatePhotoById(viewModelAvatar.ResidentId, viewModelAvatar.Extension);
                string fileName = viewModelAvatar.ResidentId + "." + viewModelAvatar.Extension;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }
                this.repositoryUOW.DbHelperOleDb.Commit();
                return true;


            }
            catch (Exception ex)
            {
                this.repositoryUOW.DbHelperOleDb.RollBack();
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpGet]
        public IActionResult GetPhoto(string residentId) //Retrieves the photo file name that belongs to the resident, returns  it as a stream so the api client could handle it.
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                string photo = this.repositoryUOW.ResidentRepository.GetPhotoById(residentId);
                string extension = Path.GetExtension(photo).TrimStart('.');
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", photo);


                FileStream stream = System.IO.File.OpenRead(path);

                return File(stream, $"image/{extension}"); //I transfer the image as bytes for added security, accesing the webserver directly is not safe, and can cause problems if its on a private network.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, new ApiError { ErrorMessage = "Image Failed To Load" });
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }
        [HttpGet]
        public IActionResult GetEventPhoto(string eventId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                string photo = this.repositoryUOW.EventRepository.GetPhotoById(eventId);
                string extension = Path.GetExtension(photo).TrimStart('.');
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", photo);


                FileStream stream = System.IO.File.OpenRead(path);

                return File(stream, $"image/{extension}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, new ApiError { ErrorMessage = "Image Failed To Load" });
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpGet]
        public Resident GetResident(string residentId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ResidentRepository.GetById(residentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }
        [HttpPost]
        public IActionResult UpdateResident([FromBody] Resident resident)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                Building building = this.repositoryUOW.BuildingRepository.GetBuildingByResidentId(resident.ResidentId);
                if(building != null)
                {
                    if (resident.UnitNumber > building.TotalUnits)
                    {
                        return BadRequest(new ApiError { ErrorMessage = "Unit Number Not In Range" });
                    }
                    else
                        return Ok(this.repositoryUOW.ResidentRepository.Update(resident));
                }
                return NotFound(new ApiError { ErrorMessage = "Building Not Found" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, new ApiError { ErrorMessage = "Server Error" });
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpPost]
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            TimeSpan t1 = TimeSpan.Parse(booking.EndTime);
            TimeSpan t2 = TimeSpan.Parse(booking.StartTime);
            if (t2 > t1)
                return BadRequest(new ApiError { ErrorMessage = "Start Time must be before End Time" });
            try
            {
                
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                bool response =  this.repositoryUOW.BookingRepository.Create(booking);
                return Ok(response);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return StatusCode(500, new ApiError { ErrorMessage = "Internal server error" });
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }

        }
        [HttpPost]
        public bool PayBooking([FromQuery] string bookingId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                Booking booking = this.repositoryUOW.BookingRepository.GetById(bookingId);
                if (booking == null)
                    return false;
               
                if (booking.BookingStatus != BookingStatus.AWAITING_PAYMENT)
                    return false;

                return this.repositoryUOW.BookingRepository.UpdateBookingStauts(bookingId, BookingStatus.CONFIRMED);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }
        [HttpPost]
        public bool CancelBooking([FromQuery] string bookingId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.BookingRepository.UpdateBookingStauts(bookingId, BookingStatus.CANCELED);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpGet]
        public BookingViewModel GetAllBookings(string residentId,string date)
        {
            BookingViewModel bookingViewModel = new BookingViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                Building building = this.repositoryUOW.BuildingRepository.GetBuildingByResidentId(residentId);
                if (building == null)
                    return null;
                bookingViewModel.Bookings =  this.repositoryUOW.BookingRepository.GetBookingsByBuildingId(building.BuildingId,date);
                bookingViewModel.SelectedDate = date;
                bookingViewModel.MyBookings = this.repositoryUOW.BookingRepository.GetBookingsByResidentId(residentId);
                return bookingViewModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }
        

    }
}
