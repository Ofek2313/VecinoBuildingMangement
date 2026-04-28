using VecinoBuildingMangementWebService.ORM.ModelCreators;
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

        ModelCreator modelCreator;
        public RepositoryUOW()
        {
            this.dbHelperOleDb = new DbHelperOleDb();
            this.modelCreator = new ModelCreator();
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
                        buildingRepository = new BuildingRepository(this.dbHelperOleDb,this.modelCreator);
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
                    eventRepository = new EventRepository(this.dbHelperOleDb,this.modelCreator);
                return eventRepository;
            }
        }

        public EventTypeRepository EventTypeRepository
        {
            get
            {
                if (eventTypeRepository == null)
                    eventTypeRepository = new EventTypeRepository(this.dbHelperOleDb, this.modelCreator);
                return eventTypeRepository;
            }
        }

        public FeeRepository FeeRepository
        {
            get
            {
                if (feeRepository == null)
                    feeRepository = new FeeRepository(this.dbHelperOleDb, this.modelCreator);
                return feeRepository;
            }
        }

        public NotificationRepository NotificationRepository
        {
            get
            {
                if (notificationRepository == null)
                    notificationRepository = new NotificationRepository(this.dbHelperOleDb, this.modelCreator);
                return notificationRepository;
            }
        }

        public PollRepository PollRepository
        {
            get
            {
                if (pollRepository == null)
                    pollRepository = new PollRepository(this.dbHelperOleDb, this.modelCreator);
                return pollRepository;
            }
        }

        public RequestTypeRepository RequestTypeRepository
        {
            get
            {
                if (requestTypeRepository == null)
                    requestTypeRepository = new RequestTypeRepository(this.dbHelperOleDb, this.modelCreator);
                return requestTypeRepository;
            }
        }

        public ResidentRepository ResidentRepository
        {
            get
            {
                if (residentRepository == null)
                    residentRepository = new ResidentRepository(this.dbHelperOleDb, this.modelCreator);
                return residentRepository;
            }
        }

        public ServiceRequestRepository ServiceRequestRepository
        {
            get
            {
                if (serviceRequestRepository == null)
                    serviceRequestRepository = new ServiceRequestRepository(this.dbHelperOleDb, this.modelCreator);
                return serviceRequestRepository;
            }
        }

        public VoteRepository VoteRepository
        {
            get
            {
                if (voteRepository == null)
                    voteRepository = new VoteRepository(this.dbHelperOleDb, this.modelCreator);
                return voteRepository;
            }
        }
        public OptionRepository OptionRepository
        {
            get
            {
                if (optionRepository == null)
                    optionRepository = new OptionRepository(this.dbHelperOleDb, this.modelCreator);
                return optionRepository;
            }
        }


    }
}
