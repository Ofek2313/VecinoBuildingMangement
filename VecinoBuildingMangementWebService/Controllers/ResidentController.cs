using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangement.Models;

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

        [HttpGet]
        public string Login(string username, string password)
        {

            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ResidentRepository.Login(username, password);
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
        public List<Notification> GetNotifications()
        {
            List<Notification> list = new List<Notification>();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                list = this.repositoryUOW.NotificationRepository.
            }
        }

    }
    
}
