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

        public BuildingRepository BuildingRepository
        {
            get
            {
                if(buildingRepository == null)
                    buildingRepository = new BuildingRepository();
                return buildingRepository;
            }
        }

        public CityRepository CityRepository
        {
            get
            {
                if (cityRepository == null)
                    cityRepository = new CityRepository();
                return cityRepository;
            }
        }

        public EventRepository EventRepository
        {
            get
            {
                if (eventRepository == null)
                    eventRepository = new EventRepository();
                return eventRepository;
            }
        }

        public EventTypeRepository EventTypeRepository
        {
            get
            {
                if (eventTypeRepository == null)
                    eventTypeRepository = new EventTypeRepository();
                return eventTypeRepository;
            }
        }

        public FeeRepository FeeRepository
        {
            get
            {
                if (feeRepository == null)
                    feeRepository = new FeeRepository();
                return feeRepository;
            }
        }

        public NotificationRepository NotificationRepository
        {
            get
            {
                if (notificationRepository == null)
                    notificationRepository = new NotificationRepository();
                return notificationRepository;
            }
        }

        public PollRepository PollRepository
        {
            get
            {
                if (pollRepository == null)
                    pollRepository = new PollRepository();
                return pollRepository;
            }
        }

        public RequestTypeRepository RequestTypeRepository
        {
            get
            {
                if (requestTypeRepository == null)
                    requestTypeRepository = new RequestTypeRepository();
                return requestTypeRepository;
            }
        }

        public ResidentRepository ResidentRepository
        {
            get
            {
                if (residentRepository == null)
                    residentRepository = new ResidentRepository();
                return residentRepository;
            }
        }

        public ServiceRequestRepository ServiceRequestRepository
        {
            get
            {
                if (serviceRequestRepository == null)
                    serviceRequestRepository = new ServiceRequestRepository();
                return serviceRequestRepository;
            }
        }

        public VoteRepository VoteRepository
        {
            get
            {
                if (voteRepository == null)
                    voteRepository = new VoteRepository();
                return voteRepository;
            }
        }


    }
}
