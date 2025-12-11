using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangement.Models;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Options;

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
        public ManagePaymentViewModel GetManagePayment(string ResidentId)
        {
            ManagePaymentViewModel viewModel = new ManagePaymentViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                viewModel.Fees = repositoryUOW.FeeRepository.ViewPaidFeesById(ResidentId);
                List<Fee> UnPaid = repositoryUOW.FeeRepository.GetUnPaidFeeById(ResidentId);
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
        public ServiceRequestViewModel GetServiceRequest(string ResidentId)
        {
            ServiceRequestViewModel serviceRequestViewModel = new ServiceRequestViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                serviceRequestViewModel.serviceRequests = repositoryUOW.ServiceRequestRepository.GetServiceRequestsByResidentId(ResidentId);
                serviceRequestViewModel.RequestTypes = repositoryUOW.RequestTypeRepository.GetAll();

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
      


        [HttpGet]
        public ManageServiceRequestViewModel ManageServiceRequest()
        {
            ManageServiceRequestViewModel manageServiceRequestViewModel = new ManageServiceRequestViewModel();
            int requests = 0;
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                manageServiceRequestViewModel.serviceRequests = this.repositoryUOW.ServiceRequestRepository.GetAll();
                foreach (ServiceRequest serviceRequest in manageServiceRequestViewModel.serviceRequests)
                {
                    requests++;
                }
                manageServiceRequestViewModel.ServiceRequestNumber = requests;
                return manageServiceRequestViewModel;
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
        public bool OpenServiceRequest(ServiceRequest serviceRequest)
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

        [HttpGet]
        public string Login(string email, string password)
        {

            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ResidentRepository.Login(email, password);
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
        public List<Event> ViewEvents()
        {
            List<Event> events = new List<Event>();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                events = this.repositoryUOW.EventRepository.GetAll();
                Console.WriteLine(events.Count);
                return events;
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
        public bool JoinBuilding(string residentId, string buildingCode)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                string buildingId = this.repositoryUOW.BuildingRepository.GetBuildingIdByCode(buildingCode);
                if (buildingId != null)
                    return this.repositoryUOW.ResidentRepository.UpdateResidentBuilding(residentId, buildingId);
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

        [HttpPost]
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
                viewModel.events = this.repositoryUOW.EventRepository.GetAll();
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
        public List<PollViewModel> PollViewModel(string buildingId)
        {
            List<PollViewModel> pollviewModel = new List<PollViewModel>();
            
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
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

    }

}
