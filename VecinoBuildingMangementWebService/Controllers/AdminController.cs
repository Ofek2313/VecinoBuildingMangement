using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using VecinoBuildingMangement;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangementWebService.Helpers;


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

        [HttpGet]
        public List<Notification> GetNotifications(string buildingId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.NotificationRepository.GetNotificationsByBuildingId(buildingId);
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
        public bool SendNotification([FromBody]SendNotificationViewModel sendNotificationViewModel)
        {
            try
            {

                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();
                sendNotificationViewModel.Notification.NotificationDate = DateTime.Now.ToString("dd/MM/yyyy");
                this.repositoryUOW.NotificationRepository.Create(sendNotificationViewModel.Notification);
                string id = this.repositoryUOW.NotificationRepository.GetLastId();
                foreach(string residentId in sendNotificationViewModel.ResidentIds)
                {
                    bool response = this.repositoryUOW.NotificationRepository.CreateResidentNotification(id, residentId);
                    if (!response)
                    {
                        this.repositoryUOW.DbHelperOleDb.RollBack();
                        return false;
                    }
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
        [HttpPost]
        public bool RemoveNotification([FromBody] string notificationId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.NotificationRepository.Delete(notificationId);
            }
            catch(Exception ex)
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
        public ManageEventViewModel ManageEvent(string buildingId)
        {
            ManageEventViewModel manageEventViewModel = new ManageEventViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                List<EventViewModel> events = this.repositoryUOW.EventRepository.GetEventViewModelsByBuildingId(buildingId);
                manageEventViewModel.Events = events.Where(e => (DateTime.ParseExact(e.Event.EventDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.Today)).ToList();
                manageEventViewModel.PastEvents = events.Where(e => (DateTime.ParseExact(e.Event.EventDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.Today)).ToList();
                manageEventViewModel.CurrentMonth = DateTime.Now.ToString("MMMM");
                return manageEventViewModel;
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
        public bool AddUpComingEvent([FromForm] string model, IFormFile file)
        {
            Event @event = JsonSerializer.Deserialize<Event>(model);
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();
                this.repositoryUOW.EventRepository.Create(@event);
                string eventId = this.repositoryUOW.EventRepository.GetLastId();
                string ext = Path.GetExtension(file.FileName).TrimStart('.').ToLower();
                bool response = this.repositoryUOW.EventRepository.UpdatePhotoById(eventId, ext);

                string fileName = "event" + eventId + "." + ext;
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

        [HttpPost]
        public bool RemoveEvent([FromQuery] string eventId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                Event @event = this.repositoryUOW.EventRepository.GetById(eventId);

                if (@event == null) return false;

                bool deleteResponse =  this.repositoryUOW.EventRepository.Delete(eventId);
                if(deleteResponse && !string.IsNullOrWhiteSpace(@event.EventImage))
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", @event.EventImage);
                    if(System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
                return deleteResponse;

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
        public bool UpdateEvent([FromForm] string model, IFormFile? file)
        {
            Event @event = JsonSerializer.Deserialize<Event>(model);
            bool response = false;
            try
            {
                
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                if(file != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", @event.EventImage);
                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                        Console.WriteLine("File deleted successfully.");
                    }
                    string ext = Path.GetExtension(file.FileName).TrimStart('.').ToLower();
                    @event.EventImage = $"event{@event.EventId}.{ext}";
                    response = this.repositoryUOW.EventRepository.Update(@event);
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", @event.EventImage);
                    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                else
                {
                    response = this.repositoryUOW.EventRepository.Update(@event);
                }
                return response;
                
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
        public List<ResidentSummaryDTO> GetResidentsAttendingEvent(string eventId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.EventRepository.GetResidentsAttendingEventByEventId(eventId);
            }
            catch(Exception ex)
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
        public ManageServiceRequestViewModel ManageServiceRequest(string buildingId)
        {
            ManageServiceRequestViewModel manageServiceRequestViewModel = new ManageServiceRequestViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                manageServiceRequestViewModel.serviceRequests = this.repositoryUOW.ServiceRequestRepository.GetRequestsByBuilding(buildingId);

                manageServiceRequestViewModel.ServiceRequestNumber = manageServiceRequestViewModel.serviceRequests.Count;
                (manageServiceRequestViewModel.Pending, manageServiceRequestViewModel.Completed, manageServiceRequestViewModel.InProgress) =
                    this.repositoryUOW.ServiceRequestRepository.GetServiceRequestSummary(buildingId);
                //foreach (ServiceRequest serviceRequest in manageServiceRequestViewModel.serviceRequests)
                //{
                //    switch (serviceRequest.RequestStatus)
                //    {
                //        case "Pending":
                //            manageServiceRequestViewModel.Pending += 1;
                //            break;
                //        case "Completed":
                //            manageServiceRequestViewModel.Completed += 1;
                //            break;
                //        case "In Progress":
                //            manageServiceRequestViewModel.InProgress += 1;
                //            break;
                //        default:
                //            break;
                //    }
                //}
                return manageServiceRequestViewModel;
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
        public bool DeleteServiceRequest(string requestId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ServiceRequestRepository.Delete(requestId);
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
        public bool ChangeRequestStatus([FromBody] StatusDto model)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ServiceRequestRepository.UpdateStatus(model.Status, model.RequestId);
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
        public ManageResidentViewModel ManageResident(string buildingId)
        {
            ManageResidentViewModel viewModel = new ManageResidentViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                viewModel.Residents = this.repositoryUOW.ResidentRepository.GetResidentByBuilding(buildingId);
                viewModel.TotalResidents = viewModel.Residents.Count;
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
        public List<Resident> GetResidents(string buildingId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ResidentRepository.GetResidentByBuilding(buildingId);
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
        public List<ResidentSummaryDTO> GetResidentsSummary(string buildingId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ResidentRepository.GetResidentSummaryByBuilding(buildingId);
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
        public bool RemoveResident(string residentId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ResidentRepository.UpdateResidentBuilding(residentId, "0");
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
        public bool CreateFee([FromBody] CreateFee createFee)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();
                foreach(string id in createFee.FeeRecipientIds)
                {
                    Fee fee = new Fee
                    {

                        FeeTitle = createFee.Fee.FeeTitle,
                        FeeAmount = createFee.Fee.FeeAmount,
                        FeeDueDate = createFee.Fee.FeeDueDate,
                        IsPaid = false,
                        ResidentId = id,
                        PaymentDate = ""

                    };
                    bool response = this.repositoryUOW.FeeRepository.Create(fee);
                    if(!response)
                    {
                        this.repositoryUOW.DbHelperOleDb.RollBack();
                        return false;
                    }
                }
                this.repositoryUOW.DbHelperOleDb.Commit();
                return true;
               
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
        public bool UpdateFee([FromBody] Fee Fee)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.FeeRepository.Update(Fee);
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
        public List<Fee> viewPaidFees()
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.FeeRepository.ViewPaidFees();
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
        public async Task<BuildingResponse> CreateBuilding([FromForm] string model, IFormFile file)
        {
            CreateBuildingDto buildingDto = JsonSerializer.Deserialize<CreateBuildingDto>(model);
            try
            {
                
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();
                CordsDto cords = await GeoCodingHelper.GetCoordinatesAsync(buildingDto.Building.Address);
                buildingDto.Building.Longitude = cords.Longitude;
                buildingDto.Building.Latitude = cords.Latitude;
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        buildingDto.Building.JoinCode = GenerateCode();
                        this.repositoryUOW.BuildingRepository.Create(buildingDto.Building);
                        break;
                    }
                    catch (OleDbException ex) when (ex.ErrorCode == -2147217900)
                    {
                        continue;
                    }
                }
                
                string buildingId = this.repositoryUOW.BuildingRepository.GetLastId();
                string ext = Path.GetExtension(file.FileName).TrimStart('.').ToLower();
                string fileName = "building" + buildingId + "." + ext;
                bool response = this.repositoryUOW.BuildingRepository.UpdatePhotoById(buildingId, fileName);
              
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "BuildingImages" , fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }
                bool response2 = this.repositoryUOW.ResidentRepository.UpdateAdminCreationResidentBuilding(buildingDto.ResidentId, buildingId);
            
               
                BuildingResponse buildingResponse = new BuildingResponse
                {
                    BuildingId = buildingId,
                };
                this.repositoryUOW.DbHelperOleDb.Commit();
                return buildingResponse;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.repositoryUOW.DbHelperOleDb.RollBack();
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        //[HttpGet]
        //public AdminMainPage GetAdminMainPage(string residentId)
        //{
        //    AdminMainPage building = new AdminMainPage();
        //    try
        //    {
        //        this.repositoryUOW.DbHelperOleDb.OpenConnection();
        //        return this.repositoryUOW.BuildingRepository.GetAdminOverlay(residentId);
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return null;
        //    }
        //    finally
        //    {
        //        this.repositoryUOW.DbHelperOleDb.CloseConnection();
        //    }

        //}

        [HttpGet]
        public List<PollViewModelAdmin> ManagePolls(string buildingId)
        {
            ManagePolls managePolls = new ManagePolls();
            List<PollViewModelAdmin> pollviewModel = new List<PollViewModelAdmin>();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                int residents = this.repositoryUOW.ResidentRepository.CountResidentByBuildingId(buildingId);
                List<PollViewModel> baseViewModels = this.repositoryUOW.PollRepository.GetPollViewModels(buildingId);
                pollviewModel = baseViewModels.Select(p => new PollViewModelAdmin
                {
                    poll = p.poll,
                    options = p.options,
                    TotalVotes = p.options.Sum(o => o.voted),
                   


                }).ToList();
                pollviewModel.ForEach(p => p.ParticipationRate = (int)Math.Round(((double)p.TotalVotes / residents) * 100));

                return pollviewModel;

               
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
        public bool CreatePoll(CreatePollViewModel createPollViewModel)
        {

            if (createPollViewModel.Options == null || createPollViewModel.Options.Count < 2)
                return false;
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();
                this.repositoryUOW.PollRepository.Create(createPollViewModel.Poll);
                string pollId = this.repositoryUOW.PollRepository.GetLastId();
                foreach (Option option in createPollViewModel.Options)
                {
                    option.PollId = pollId;
                    this.repositoryUOW.OptionRepository.Create(option);
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

        [HttpPost]
        public bool DeletePoll(string pollId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();


                this.repositoryUOW.VoteRepository.DeleteByPollId(pollId); // delete all votes that are realted to the poll
                this.repositoryUOW.OptionRepository.DeleteByPollId(pollId); // delete all options that are realted to the poll
                this.repositoryUOW.PollRepository.Delete(pollId); //delete Poll
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
        [HttpPost]
        public bool ClosePoll([FromBody] Poll poll)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.PollRepository.UpdatePollStatus(poll.PollId, false);


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
        public bool OpenPoll([FromBody] Poll poll)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.PollRepository.UpdatePollStatus(poll.PollId, true);


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
        public bool GenerateNewBuildingCode(string buildingId)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rand = new Random();
            string code = "";
            for (int i = 0; i < 5; i++)
            {
                code += chars[rand.Next(0, chars.Length)];
            }
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.BuildingRepository.UpdateJoinCode(code, buildingId);
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
        public CreateEvent GetEventTypes()
        {
            CreateEvent createEvent = new CreateEvent();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                createEvent.eventTypes = this.repositoryUOW.EventTypeRepository.GetAll();
                createEvent.Event = null;
                return createEvent;

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
        public Building GetBuilding(string buildingId)
        {
            Building building = new Building();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                building = this.repositoryUOW.BuildingRepository.GetById(buildingId);
                return building;
            }
            catch(Exception ex)
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
        public ManageAdminFinance GetBuildingFinance(string buildingId)
        {

            ManageAdminFinance manageAdminFinance = new ManageAdminFinance();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                List<ResidentFeeViewModel> buildingFees = this.repositoryUOW.FeeRepository.GetFeesByBuildingId(buildingId);
                manageAdminFinance.Finances = buildingFees;
                FeeSummary feeSummary = this.repositoryUOW.FeeRepository.SummarizeFeesByBuilding(buildingId);
                manageAdminFinance.TotalPaid = feeSummary.TotalPaid;
                manageAdminFinance.TotalUnPaid = feeSummary.TotalUnPaid;
                manageAdminFinance.TotalCollected = feeSummary.TotalCollected;
                manageAdminFinance.Outstanding = feeSummary.Outstanding;
                manageAdminFinance.TotalCollectedCurrentMonth = feeSummary.TotalCollectedCurrentMonth;

                manageAdminFinance.Transaction = this.repositoryUOW.FeeRepository.GetLastTransactionsByBuildingId(buildingId);

                manageAdminFinance.CollectionRate = (int)((double)manageAdminFinance.TotalPaid / buildingFees.Count * 100);
        

                manageAdminFinance.Transaction =
                manageAdminFinance.Transaction.OrderByDescending(t => DateTime.ParseExact(t.Fee.PaymentDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .ToList();

                return manageAdminFinance;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        private string GenerateCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rand = new Random();
            string code = "";
            for (int i = 0; i < 5; i++)
            {
                code += chars[rand.Next(0, chars.Length)];
            }
            return code;
        }

        [HttpPost]
        public async  Task<BuildingResponse> CreateBuildingAndRegister([FromForm] string model, IFormFile file)
        {
            CreateBuildingRegister createBuildingRegister = JsonSerializer.Deserialize<CreateBuildingRegister>(model);
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();
                CordsDto cords = await GeoCodingHelper.GetCoordinatesAsync(createBuildingRegister.Building.Address);
                createBuildingRegister.Building.Longitude = cords.Longitude;
                createBuildingRegister.Building.Latitude = cords.Latitude;
                for(int i = 0; i < 10; i++)
                {
                    try
                    {
                        createBuildingRegister.Building.JoinCode = GenerateCode();
                        this.repositoryUOW.BuildingRepository.Create(createBuildingRegister.Building);
                        break;
                    }
                    catch(OleDbException ex) when (ex.ErrorCode == -2147217900)
                    {
                        continue;
                    }
                }
               
                string buildingId = this.repositoryUOW.BuildingRepository.GetLastId();
                string ext = Path.GetExtension(file.FileName).TrimStart('.').ToLower();
                string fileName = "building" + buildingId + "." + ext;
                bool responseBuilding = this.repositoryUOW.BuildingRepository.UpdatePhotoById(buildingId, fileName);

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "BuildingImages", fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }
                createBuildingRegister.Resident.BuildingId = buildingId;
                bool responseRegister = this.repositoryUOW.ResidentRepository.Create(createBuildingRegister.Resident);
              ;
                if (responseRegister && responseBuilding)
                {
                    BuildingResponse buildingResponse = new BuildingResponse
                    {
                        BuildingId = buildingId,
                        ResidentId = this.repositoryUOW.ResidentRepository.GetLastId()
                    };
                    this.repositoryUOW.DbHelperOleDb.Commit();
                    return buildingResponse;
                }
                this.repositoryUOW.DbHelperOleDb.RollBack();
                return null;



            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.repositoryUOW.DbHelperOleDb.RollBack();
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }
        [HttpGet]
        public AdminMainPage GetAdminMainPage(string buildingId,string residentId)
        {
            AdminMainPage adminMainPage = new AdminMainPage();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                adminMainPage = this.repositoryUOW.BuildingRepository.GetAdminOverlay(residentId);
                adminMainPage.BuildingStats = this.repositoryUOW.BuildingRepository.GetBuildingStats(buildingId);
                adminMainPage.NextEvent = this.repositoryUOW.EventRepository.GetNextEvenByBuildingId(buildingId);
                adminMainPage.ActivityViewModels = this.repositoryUOW.BuildingRepository.GetActivityViewModelsByBuildingId(buildingId);

                return adminMainPage;
            }
            catch
            {
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpGet]
        public AnnouncementDetailsViewModel GetResidentsNotification(string notificationId)
        {
            AnnouncementDetailsViewModel viewModel = new AnnouncementDetailsViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                viewModel.Residents =  this.repositoryUOW.NotificationRepository.GetResidentsNotification(notificationId);
                viewModel.ResidentCount = viewModel.Residents.Count;
                viewModel.Admin = this.repositoryUOW.NotificationRepository.GetNotificationSender(notificationId);

                return viewModel;
            }
            catch(Exception ex)
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
        public IActionResult DemoteAdmin([FromBody] AdminToggleDto adminToggleDto) 
        {
            if(adminToggleDto.ResidentId == adminToggleDto.AdminId)
            {
                return BadRequest("You cannot demote YourSelf");
            }
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                bool ok = this.repositoryUOW.ResidentRepository.UpdateAdminRole(adminToggleDto.ResidentId, false);
                if (ok)
                    return Ok();
                return NotFound( "Update failed — resident not found." );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "An internal error occurred.");
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpPost]
        public bool PromoteAdmin([FromBody] AdminToggleDto adminToggleDto)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ResidentRepository.UpdateAdminRole(adminToggleDto.ResidentId, true);
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
        public bool UpdateNotification(Notification notification)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.NotificationRepository.Update(notification);
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
        public List<City> GetCities()
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.CityRepository.GetAll();
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
        public async Task<bool> UpdateBuilding([FromBody] BuildingUpdateDto building)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                CordsDto cords = null;
                if (building.AddressChangedFlag)
                    cords = await GeoCodingHelper.GetCoordinatesAsync(building.Address);
                return this.repositoryUOW.BuildingRepository.UpdateBuildingWithCords(building, cords);

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
        public bool RemoveFee([FromBody] Fee Fee)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.FeeRepository.Delete(Fee.FeeId);

            }
            catch(Exception ex)
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
        public IActionResult ApproveBooking([FromQuery]string bookingId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                bool overLap = this.repositoryUOW.BookingRepository.CheckOverLap(bookingId);
                if (!overLap)
                {
                    bool response = this.repositoryUOW.BookingRepository.UpdateBookingStauts(bookingId, BookingStatus.AWAITING_PAYMENT);
                    if (response)
                        return Ok();
                    else
                        return BadRequest("Update failed");

                }
                else
                    return BadRequest("Booking Slot Is Already Taken");


               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "An internal error occurred.");
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }
        [HttpPost]
        public bool RejectBooking([FromQuery] string bookingId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.BookingRepository.UpdateBookingStauts(bookingId, BookingStatus.REJECTED);
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
        public ManageBookingsViewModel GetAllBookings(string buildingId)
        {
            ManageBookingsViewModel manageBookingsViewModel = new ManageBookingsViewModel();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                List<BookingResidentViewModel> bookings = this.repositoryUOW.BookingRepository.GetBookingsByBuildingId(buildingId);
                BookingStatsDto bookingStatsDto = this.repositoryUOW.BookingRepository.GetBookingsStats(buildingId);
                manageBookingsViewModel.AwaitingApproval = bookingStatsDto.AwaitingApproval;
                manageBookingsViewModel.AwaitingPayment  = bookingStatsDto.AwaitingPayment;
                manageBookingsViewModel.Rejected = bookingStatsDto.Rejected;
                manageBookingsViewModel.Confirmed = bookingStatsDto.Confirmed;
                manageBookingsViewModel.UpComingBookings = bookings.Where( b => DateTime.ParseExact(b.Booking.BookingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.Today).OrderBy(b => DateTime.ParseExact(b.Booking.BookingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                manageBookingsViewModel.PastBookings = bookings.Where(b => DateTime.ParseExact(b.Booking.BookingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.Today).OrderBy(b => DateTime.ParseExact(b.Booking.BookingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                return manageBookingsViewModel;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return manageBookingsViewModel;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }


        }
    }
}

