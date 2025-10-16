using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class ServiceRequest
    {
        string requestId;
        string requestTitle;
        string requestMessage;
        string requestTypeId;
        string requestDate;
        string requestStatus;
        string residentId;

        public string RequestId
        {
            get { return requestId; }
            set { requestId = value; }
        }

        [Required(ErrorMessage ="Title can not be empty")]
        [StringLength(60,MinimumLength =5,ErrorMessage = "Title needs to be between 5-40 characters")]
        public string RequestTitle 
        {  
            get { return requestTitle; }
            set { requestTitle = value; }
        }

        [Required(ErrorMessage = "Message can not be empty")]
        [StringLength(200, MinimumLength = 15, ErrorMessage = "Message needs to be between 10-200 characters")]
        public string RequestMessage
        {
            get { return requestMessage; }
            set { requestMessage = value; }
        }

        [Required(ErrorMessage = "Request Type Id can not be empty")]
        [RegularExpression(@"[0-9]", ErrorMessage = "Request Type Id Must Be Digits")]
        public string RequestTypeId
        { 
            get { return requestTypeId; }
            set { requestTypeId = value; }
        }

        [Required(ErrorMessage = "Request Date can not be empty")]
        [Date(ErrorMessage ="Date must be a valid date")]
        public string RequestDate
        {
            get { return requestDate; }
            set { requestDate = value; }
        }

        [Required(ErrorMessage = "Request Status can not be empty")]
        public string RequestStatus
        {
            get { return requestStatus; }
            set { requestStatus = value; }
        }

        [Required(ErrorMessage = "ResidentId can not be empty")]
        public string ResidentId
        {
            get { return residentId; }
            set { residentId = value; }
        }

    }
}
