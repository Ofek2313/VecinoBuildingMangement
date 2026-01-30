using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangement.Models;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Options;
using System.Text;
using System.IO;
using System.Text.Json;

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
        private bool IsBefore(string date1, string date2)
        {
            string[] date1Split = date1.Split('/');
            string[] date2Split = date2.Split('/');
            int day1 = Convert.ToInt32(date1Split[0]);
            int month1 = Convert.ToInt32(date1Split[1]);
            int year1 = Convert.ToInt32(date1Split[2]);

            int day2 = Convert.ToInt32(date2Split[0]);
            int month2 = Convert.ToInt32(date2Split[1]);
            int year2 = Convert.ToInt32(date2Split[2]);

            if (year1 < year2)
                return true;
            else if (year2 < year1)
                return false;
            if (month1 < month2)
                return true;
            else if (month2 < month1)
                return false;
            if (day1 < day2)
                return true;

            return false;
        }
        [HttpGet]
        public ManagePaymentViewModel GetManagePayment(string residentId)
        {
            ManagePaymentViewModel viewModel = new ManagePaymentViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                viewModel.Fees = repositoryUOW.FeeRepository.GetFeesById(residentId);
                List<Fee> Paid = this.repositoryUOW.FeeRepository.ViewPaidFeesById(residentId);
                List<Fee> UnPaid = repositoryUOW.FeeRepository.GetUnPaidFeeById(residentId);
                
                double totalFee = 0;
                double unpaidfee = 0;
                foreach (Fee fee in Paid)
                {
                    totalFee += fee.FeeAmount;
                }
                viewModel.TotalPaidFees = totalFee;
                foreach(Fee fee in UnPaid)
                {
                    unpaidfee += fee.FeeAmount;
                }
                viewModel.TotalUnPaidFees = unpaidfee;
                viewModel.paidFees = Paid.Count;
                viewModel.unPaidFees = UnPaid.Count;
                if (UnPaid.Count > 0)
                    viewModel.nextFee = UnPaid[0];
                foreach(Fee fee in UnPaid)
                {
                    if(IsBefore(fee.FeeDueDate, viewModel.nextFee.FeeDueDate))
                        viewModel.nextFee = fee;

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
            catch(Exception ex)
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
                foreach(ServiceRequest serviceRequest in serviceRequestViewModel.serviceRequests)
                {
                    switch(serviceRequest.RequestStatus)
                    {
                        case "Pending":
                            serviceRequestViewModel.Pending += 1;
                            break;
                        case "Completed":
                            serviceRequestViewModel.Completed += 1;
                            break;
                        case "In Progress":
                            serviceRequestViewModel.InProgress += 1;
                            break;
                        default:
                            break;
                    }
                }

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
            catch(Exception ex)
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
                Building building = this.repositoryUOW.BuildingRepository.GetBuildingByResidentId(residentId);
                events = this.repositoryUOW.EventRepository.GetEventByBuildingId(building.BuildingId);
                events2 = this.repositoryUOW.EventRepository.GetPreviousEventsByBuildingId(building.BuildingId);

                foreach (Event e in events)
                {
                    viewEventViewModel.CurrEvents.Add(new EventViewModel
                    {
                        Event = e,
                        Attending = this.repositoryUOW.EventRepository.GetAttendingCount(e.EventId),


                    }
                    );
                }
                foreach (Event e in events2)
                {
                    viewEventViewModel.PreEvents.Add(new EventViewModel
                    {
                        Event = e,
                        Attending = this.repositoryUOW.EventRepository.GetAttendingCount(e.EventId),


                    }
                    );
                }
       
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
        public bool AttendEvent([FromBody] AttendEvent attendEvent )
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.EventRepository.AttendEvent(attendEvent.eventId,attendEvent.residentId);

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
        public bool JoinBuilding([FromBody] JoinBuildingRequest joinBuildingRequest)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                
                string buildingId = this.repositoryUOW.BuildingRepository.GetBuildingIdByCode(joinBuildingRequest.buildingCode);
                if (buildingId != null)
                    return this.repositoryUOW.ResidentRepository.UpdateResidentBuilding(joinBuildingRequest.residentId, buildingId);
                else
                    return false;
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
        [Produces("application/json")]
        public bool LeaveBuilding(string residentId)
        {

            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();
                

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
                notificationsViewModels.Notifications = this.repositoryUOW.NotificationRepository.GetNotificationsByResidentId(residentId);
                notificationsViewModels.PinnedNotifications = this.repositoryUOW.NotificationRepository.GetPinnedNotificationsByResidentId(residentId);
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
                viewModel.Notifications = this.repositoryUOW.NotificationRepository.GetNotificationsByResidentId(residentId);
                viewModel.events = this.repositoryUOW.EventRepository.GetEventByBuildingId(viewModel.Building.BuildingId);
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
        [HttpGet]
        public ViewPollViewModel PollViewModel(string residentId)
        {
            ViewPollViewModel pollviewModel = new ViewPollViewModel();
          
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                Building building = this.repositoryUOW.BuildingRepository.GetBuildingByResidentId(residentId);
                string buildingId = building.BuildingId;
                List<Poll> polls = this.repositoryUOW.PollRepository.GetActivePollsByBuilding(buildingId);
                foreach (Poll poll in polls)
                {
                    
                        PollViewModel viewModel = new PollViewModel();

                        viewModel.poll = poll;
                        List<Option> options = this.repositoryUOW.OptionRepository.GetOptionsByPollId(poll.PollId);
                        foreach (Option option in options)
                        {
                            OptionViewModel optionViewModel = new OptionViewModel();
                            optionViewModel.option = option;
                            optionViewModel.voted = this.repositoryUOW.VoteRepository.CountVoteByOption(option.OptionId);
                            //optionViewModel.voted = CalcVote(options, option.OptionId);
                            viewModel.options.Add(optionViewModel);
                        }
                        pollviewModel.ActivePolls.Add(viewModel);
                    
                   
                    


                }
                List<Poll> polls1 = this.repositoryUOW.PollRepository.GetInActivePollsByBuilding(buildingId);
                foreach (Poll poll in polls1)
                {

                    PollViewModel viewModel = new PollViewModel();

                    viewModel.poll = poll;
                    List<Option> options = this.repositoryUOW.OptionRepository.GetOptionsByPollId(poll.PollId);
                    foreach (Option option in options)
                    {
                        OptionViewModel optionViewModel = new OptionViewModel();
                        optionViewModel.option = option;
                        optionViewModel.voted = this.repositoryUOW.VoteRepository.CountVoteByOption(option.OptionId);
                        //optionViewModel.voted = CalcVote(options, option.OptionId);
                        viewModel.options.Add(optionViewModel);
                    }
                    pollviewModel.InActivePolls.Add(viewModel);





                }

                return pollviewModel;
               
            }
            catch(Exception ex)
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
        public bool VoteInPoll(Vote vote)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                if (!this.repositoryUOW.VoteRepository.hasVoted(vote.ResidentId, vote.PollId))
                    return this.repositoryUOW.VoteRepository.Create(vote);
                else
                    return false;
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

        public BuildingModel GetBuildingId(string residentId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                string id = this.repositoryUOW.BuildingRepository.GetBuildingByResidentId(residentId).BuildingId;
                BuildingModel buildingModel = new BuildingModel();
                buildingModel.buildingId = id;
                return buildingModel;

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
        public bool UploadPhoto([FromForm]string model, IFormFile file)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();
                ViewModelAvatar viewModelAvatar = JsonSerializer.Deserialize<ViewModelAvatar>(model);
                bool response = this.repositoryUOW.ResidentRepository.UpdatePhotoById(viewModelAvatar.ResidentId, viewModelAvatar.Extension);
                string fileName = viewModelAvatar.ResidentId + "." + viewModelAvatar.Extension;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images",fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create,FileAccess.Write))
                {
                    file.CopyTo(fileStream);    
                }
                this.repositoryUOW.DbHelperOleDb.Commit();
                return true;


            }
            catch(Exception ex)
            {
                this.repositoryUOW.DbHelperOleDb.RollBack();
                return false;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }
    }

}
