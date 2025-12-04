using VecinoBuildingMangementWebService.ORM.Repositories;

namespace VecinoBuildingMangementWebService
{
    public class RepositoryUOW
    {
        BuildingRepository buildingRepository;
        CityRepository cityRepository;
        EventRepository eventRepository;
        EventTypeRepository eventTypeRepository;
        FeeRepository feeRepository;
        NotificationRepository notificationRepository;
        PollRepository pollRepository;
        RequestTypeRepository requestTypeRepository;
        ResidentRepository residentRepository;
        ServiceRequestRepository serviceRequestRepository;
        VoteRepository voteRepository;
        OptionRepository optionRepository;

        DbHelperOleDb dbHelperOleDb;
        ModelCreators modelCreators;

        public RepositoryUOW()
        {
            this.dbHelperOleDb = new DbHelperOleDb();
            this.modelCreators = new ModelCreators();
        }
        public DbHelperOleDb DbHelperOleDb
        {
            get { return this.dbHelperOleDb; }

        }

        public BuildingRepository BuildingRepository
        {
            get
            {
                if(buildingRepository == null)
                        buildingRepository = new BuildingRepository(this.dbHelperOleDb);
                    return buildingRepository;
            }
        }

        public CityRepository CityRepository
        {
            get
            {
                if (cityRepository == null)
                    cityRepository = new CityRepository(this.dbHelperOleDb, this.modelCreators);
                return cityRepository;
            }
        }

        public EventRepository EventRepository
        {
            get
            {
                if (eventRepository == null)
                    eventRepository = new EventRepository(this.dbHelperOleDb);
                return eventRepository;
            }
        }

        public EventTypeRepository EventTypeRepository
        {
            get
            {
                if (eventTypeRepository == null)
                    eventTypeRepository = new EventTypeRepository(this.dbHelperOleDb, this.modelCreators);
                return eventTypeRepository;
            }
        }

        public FeeRepository FeeRepository
        {
            get
            {
                if (feeRepository == null)
                    feeRepository = new FeeRepository(this.dbHelperOleDb);
                return feeRepository;
            }
        }

        public NotificationRepository NotificationRepository
        {
            get
            {
                if (notificationRepository == null)
                    notificationRepository = new NotificationRepository(this.dbHelperOleDb);
                return notificationRepository;
            }
        }

        public PollRepository PollRepository
        {
            get
            {
                if (pollRepository == null)
                    pollRepository = new PollRepository(this.dbHelperOleDb);
                return pollRepository;
            }
        }

        public RequestTypeRepository RequestTypeRepository
        {
            get
            {
                if (requestTypeRepository == null)
                    requestTypeRepository = new RequestTypeRepository(this.dbHelperOleDb, this.modelCreators);
                return requestTypeRepository;
            }
        }

        public ResidentRepository ResidentRepository
        {
            get
            {
                if (residentRepository == null)
                    residentRepository = new ResidentRepository(this.dbHelperOleDb);
                return residentRepository;
            }
        }

        public ServiceRequestRepository ServiceRequestRepository
        {
            get
            {
                if (serviceRequestRepository == null)
                    serviceRequestRepository = new ServiceRequestRepository(this.dbHelperOleDb);
                return serviceRequestRepository;
            }
        }

        public VoteRepository VoteRepository
        {
            get
            {
                if (voteRepository == null)
                    voteRepository = new VoteRepository(this.dbHelperOleDb);
                return voteRepository;
            }
        }
        public OptionRepository OptionRepository
        {
            get
            {
                if (optionRepository == null)
                    optionRepository = new OptionRepository(this.dbHelperOleDb);
                return optionRepository;
            }
        }


    }
}
