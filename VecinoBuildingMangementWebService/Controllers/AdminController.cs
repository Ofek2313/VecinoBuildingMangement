using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoBuildingMangementWebService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

   
    public class AdminController : ControllerBase
    {
        RepositoryUOW repositoryUOW;

        public AdminController()
        {
            this.repositoryUOW = new RepositoryUOW();
        }


        [HttpPost]
        public bool SendNotification(Notification notification)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.NotificationRepository.Create(notification);
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
        public ManageEventViewModel ManageEvent()
        {
            ManageEventViewModel manageEventViewModel = new ManageEventViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                manageEventViewModel.Events = this.repositoryUOW.EventRepository.GetAll();
                manageEventViewModel.CurrentMonth = DateTime.Now.ToString("MMMM");
                return manageEventViewModel;
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
        public bool AddUpComingEvent(Event @event)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.EventRepository.Create(@event);
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

        [HttpDelete]
        public bool RemoveEvent(string eventId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.EventRepository.Delete(eventId);
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
        public ManageServiceRequestViewModel ManageServiceRequest()
        {
            ManageServiceRequestViewModel manageServiceRequestViewModel = new ManageServiceRequestViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                manageServiceRequestViewModel.serviceRequests = this.repositoryUOW.ServiceRequestRepository.GetAll();
                int count = 0;
                foreach(ServiceRequest serviceRequest in manageServiceRequestViewModel.serviceRequests)
                {
                    count++;
                }
                manageServiceRequestViewModel.ServiceRequestNumber = count;
                return manageServiceRequestViewModel;
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

        [HttpDelete]
        public bool DeleteServiceRequest(string requestId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ServiceRequestRepository.Delete(requestId);
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

        [HttpPut]
        public bool ChangeRequestStatus(string status,string requestId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ServiceRequestRepository.UpdateStatus(status, requestId);
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
        public ManageResidentViewModel ManageResident()
        {
            ManageResidentViewModel viewModel = new ManageResidentViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                viewModel.Residents = this.repositoryUOW.ResidentRepository.GetAll();
                viewModel.TotalResidents = viewModel.Residents.Count;
                return viewModel;
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
        [HttpPut]
        public bool RemoveResident(string residentId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ResidentRepository.UpdateResidentBuilding(residentId,"0");
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
        public bool CreateFee(Fee fee)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.FeeRepository.Create(fee);
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
        public List<Fee> viewPaidFees()
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.FeeRepository.ViewPaidFees();
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
        public bool CreateBuilding(Building building)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.BuildingRepository.Create(building);
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

