using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangement.Models;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Options;
using System.Text;

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
        public ManagePaymentViewModel GetManagePayment(string residentId)
        {
            ManagePaymentViewModel viewModel = new ManagePaymentViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                viewModel.Fees = repositoryUOW.FeeRepository.ViewPaidFeesById(residentId);
                List<Fee> UnPaid = repositoryUOW.FeeRepository.GetUnPaidFeeById(residentId);
                viewModel.UnPaidFees = UnPaid;
                double totalFee = 0;
                foreach (Fee fee in UnPaid)
                {
                    totalFee += fee.FeeAmount;
                }
                viewModel.TotalFees = totalFee;
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
        public bool PayFee(string feeId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.FeeRepository.PayFee(feeId);
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
            ViewEventViewModel viewEventViewModel = new ViewEventViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                Building building = this.repositoryUOW.BuildingRepository.GetBuildingByResidentId(residentId);

                viewEventViewModel.CurrEvents = this.repositoryUOW.EventRepository.GetEventByBuildingId(building.BuildingId);
                viewEventViewModel.PreEvents = this.repositoryUOW.EventRepository.GetPreviousEventsByBuildingId(building.BuildingId);
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
        public List<Notification> GetNotifications(string residentId)
        {
            List<Notification> list = new List<Notification>();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                list = this.repositoryUOW.NotificationRepository.GetNotificationsByResidentId(residentId);
                return list;
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
        public List<PollViewModel> PollViewModel(string residentId)
        {
            List<PollViewModel> pollviewModel = new List<PollViewModel>();
          
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
                    pollviewModel.Add(viewModel);


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
    }

}
