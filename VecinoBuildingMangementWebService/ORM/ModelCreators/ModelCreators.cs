using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{
    public class ModelCreators
    {

        BuildingCreator buildingCreator;
        CityCreator cityCreator;
        EventCreator eventCreator;
        EventTypeCreator eventTypeCreator;
        FeeCreator feeCreator;
        NotificationCreator notificationCreator;
        PollCreator pollCreator;
        RequestTypeCreator requestTypeCreator;
        ResidentCreator residentCreator;
        ServiceRequestCreator serviceRequestCreator;
        VoteCreator voteCreator;
 

        public BuildingCreator BuildingCreator
        {
            get
            {
                if (this.buildingCreator == null)
                    this.buildingCreator = new BuildingCreator();

                return this.buildingCreator;
            }

        }

        public CityCreator CityCreator
        {
            get
            {
                if(this.cityCreator == null)
                    this.cityCreator = new CityCreator();
                return this.cityCreator;
            }
        }
        public EventCreator EventCreator
        {
            get
            {
                if(this.eventCreator == null)
                    this.eventCreator = new EventCreator();
                return this.eventCreator;
            }

        }
       public EventTypeCreator EventTypeCreator
        {
            get
            {
                if( this.eventTypeCreator == null)
                    this.eventTypeCreator = new EventTypeCreator();
                return this.eventTypeCreator;
            }
        }
        public FeeCreator FeeCreator
        {
            get
            {
                if(this.feeCreator == null)
                    this.feeCreator = new FeeCreator();
                return this.feeCreator;
            }
        }
        public NotificationCreator NotificationCreator
        {
            get
            {
                if( this.notificationCreator == null)
                    this.notificationCreator = new NotificationCreator();
                return this.notificationCreator;
            }

        }
        public PollCreator PollCreator
        {
            get
            {
                if (this.pollCreator == null)
                    this.pollCreator = new PollCreator();
                return this.pollCreator;
            }
        }
        public RequestTypeCreator RequestTypeCreator
        {
            get
            {
                if(this.requestTypeCreator == null)
                    this.requestTypeCreator = new RequestTypeCreator();
                return this.requestTypeCreator;
            }
        }
        public ResidentCreator ResidentCreator
        {
            get
            {
                if(this.residentCreator == null)
                    this.residentCreator = new ResidentCreator();
                return this.residentCreator;
            }
        }
        public ServiceRequestCreator ServiceRequestCreator
        {
            get
            {
                if(this.serviceRequestCreator == null)
                    this.serviceRequestCreator = new ServiceRequestCreator();
                return this.serviceRequestCreator;
            }
        }
        public VoteCreator VoteCreator
        {
            get
            {
                if(this.voteCreator == null)
                    this.voteCreator = new VoteCreator();
                return this.voteCreator;
            }
        }
     

    }
}
