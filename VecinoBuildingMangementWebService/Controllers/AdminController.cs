using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;
using System.Transactions;
using VecinoBuildingMangement;
using VecinoBuildingMangement.DTO;
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
                return this.repositoryUOW.EventRepository.Delete(eventId);
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
        public List<string> GetResidentsAttendingEvent(string eventId)
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
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

        [HttpPost]
        public bool ChangeRequestStatus([FromBody] StatusViewModel model)
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

                        FeeTitle = createFee.FeeTitle,
                        FeeAmount = createFee.FeeAmount,
                        FeeDueDate = createFee.FeeDueDate,
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
        public BuildingResponse CreateBuilding([FromForm] string model, IFormFile file)
        {
            Building building = JsonSerializer.Deserialize<Building>(model);
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();
                this.repositoryUOW.BuildingRepository.Create(building);
                string buildingId = this.repositoryUOW.BuildingRepository.GetLastId();
                string ext = Path.GetExtension(file.FileName).TrimStart('.').ToLower();
                string fileName = "building" + buildingId + "." + ext;
                bool response = this.repositoryUOW.BuildingRepository.UpdatePhotoById(buildingId, fileName);
              
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "BuildingImages" , fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }
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

        [HttpGet]
        public AdminMainPage GetAdminMainPage(string residentId)
        {
            AdminMainPage building = new AdminMainPage();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.BuildingRepository.GetAdminOverlay(residentId);
                
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
        public ManagePolls ManagePolls(string buildingId)
        {
            ManagePolls managePolls = new ManagePolls();
            List<PollViewModelAdmin> pollviewModel = new List<PollViewModelAdmin>();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                List<Poll> polls = this.repositoryUOW.PollRepository.GetPollByBuildingId(buildingId);
                foreach (Poll poll in polls)
                {
                    PollViewModelAdmin viewModel = new PollViewModelAdmin();
                    int totalVotes = 0;

                    viewModel.poll = poll;
                    List<Option> options = this.repositoryUOW.OptionRepository.GetOptionsByPollId(poll.PollId);
                    foreach (Option option in options)
                    {
                        totalVotes += this.repositoryUOW.VoteRepository.CountVoteByOption(option.OptionId);
                    }
                    viewModel.TotalVotes = totalVotes;
                    foreach (Option option in options)
                    {
                        OptionViewModel optionViewModel = new OptionViewModel();
                        optionViewModel.option = option;
                        optionViewModel.voted = this.repositoryUOW.VoteRepository.CountVoteByOption(option.OptionId);
                        optionViewModel.residentsVoted = this.repositoryUOW.ResidentRepository.findResidentsByOption(option.OptionId);
                        //optionViewModel.voted = CalcVote(options, option.OptionId);
                        if (totalVotes > 0)
                        {
                            optionViewModel.percentage = (double)optionViewModel.voted / totalVotes * 100;
                        }
                        else
                        {
                            optionViewModel.percentage = 0;
                        }
                        viewModel.options.Add(optionViewModel);
                    }
                    int residents = this.repositoryUOW.ResidentRepository.CountResidentByBuildingId(buildingId);
                    viewModel.ParticipationRate = (int)Math.Round(((double)totalVotes / residents) * 100);
                    pollviewModel.Add(viewModel);


                }
                managePolls.PollviewModel = pollviewModel;
                managePolls.PollNumbers = pollviewModel.Count;

                //int residents = this.repositoryUOW.ResidentRepository.CountResidentByBuildingId(buildingId);
                //int votes = this.repositoryUOW.VoteRepository.CountVotesByBuilding(buildingId);

                double participationRate = 0;

                managePolls.ParticipationRate = participationRate;

                return managePolls;
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

        [HttpDelete]
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

                manageAdminFinance.Transaction = this.repositoryUOW.FeeRepository.GetLastTransactionsByBuildingId(buildingId);

                manageAdminFinance.CollectionRate = (int)((double)manageAdminFinance.TotalPaid / buildingFees.Count * 100);
                manageAdminFinance.TotalCollectedCurrentMonth = 0;

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

        [HttpPost]
        public BuildingResponse CreateBuildingAndRegister([FromForm] string model, IFormFile file)
        {
            CreateBuildingRegister createBuildingRegister = JsonSerializer.Deserialize<CreateBuildingRegister>(model);
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                this.repositoryUOW.DbHelperOleDb.OpenTransaction();
      
                this.repositoryUOW.BuildingRepository.Create(createBuildingRegister.Building);
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

                if(responseRegister && responseBuilding)
                {
                    BuildingResponse buildingResponse = new BuildingResponse
                    {
                        BuildingId = buildingId,
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
        
    }
}

