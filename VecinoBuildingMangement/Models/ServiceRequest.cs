using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class ServiceRequest : Model
    {
        string requestId;
        string requestTitle;
        string requestMessage;
        string requestTypeId;
        string requestDate;
        string requestStatus;
        string residentId;

        public ServiceRequest() { }

        public string RequestId
        {
            get { return requestId; }
            set {
                if (value == null)
                    requestId = "";
                else
                    requestId = value;
            }
        }

        [Required(ErrorMessage ="Title can not be empty")]
        [StringLength(60,MinimumLength =5,ErrorMessage = "Title needs to be between 5-40 characters")]
        public string RequestTitle 
        {  
            get { return requestTitle; }
            set { requestTitle = value;
                ValidateProperty(value, "RequestTitle");
            }
        }

        [Required(ErrorMessage = "Message can not be empty")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Message needs to be between 10-200 characters")]
        public string RequestMessage
        {
            get { return requestMessage; }
            set { requestMessage = value;
                ValidateProperty(value, "RequestMessage");
            }
        }

  
        public string RequestTypeId
        { 
            get { return requestTypeId; }
            set { requestTypeId = value; }
        }

        [Required(ErrorMessage = "Request Date can not be empty")]
       
        public string RequestDate
        {
            get { return requestDate; }
            set {
               
                   
                requestDate = value;
                ValidateProperty(value, "RequestDate");
            }
        }

        [Required(ErrorMessage = "Request Status can not be empty")]
        public string RequestStatus
        {
            get { return requestStatus; }
            set { 
                
                   requestStatus = value;
                ValidateProperty(value, "RequestStatus");
            }
        }

      
        public string ResidentId
        {
            get { return residentId; }
            set { residentId = value; }
        }

    }
}
