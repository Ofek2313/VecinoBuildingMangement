using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class Notification : Model
    {
        string notificationId;
        string notificationTitle;
        string notificationMessage;
        string notificationDate;
        string priority;
        bool isPinned;

        public Notification() { }

        public string NotificationId
        {
            get { return notificationId; }
            set { notificationId = value;
                
            }
        }

        [StringLength(60,MinimumLength =5,ErrorMessage ="Title must be between 5-20 characters")]
        [Required(ErrorMessage = "Title can not be empty")]
        public string NotificationTitle
        {
            get { return notificationTitle; }
            set { notificationTitle = value;
                ValidateProperty(value, "NotificationTitle");
            }
        }
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Message must be between 5-100 characters")]
        [Required(ErrorMessage = "Message can not be empty")]
        public string NotificationMessage
        {
            get { return notificationMessage; }
            set { notificationMessage = value;
                ValidateProperty(value, "NotificationMessage");
            }
        }

        [Required(ErrorMessage = "Date can not be empty")]
        [Date(ErrorMessage ="Date must be a vaild date")]
        public string NotificationDate
        {
            get { return notificationDate; }
            set { notificationDate = value; }
        }

        [Required(ErrorMessage = "Priority can not be empty")]
        public string Priority
        {

            get { return this.priority; }
            set { this.priority = value; }
        }

        [Required(ErrorMessage = "IsPinned can not be empty")]
        public bool IsPinned
        {
            get { return this.isPinned; }
            set { this.isPinned = value; }
        }
    }
}
