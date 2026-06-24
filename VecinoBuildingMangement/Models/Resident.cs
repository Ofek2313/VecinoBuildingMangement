
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class Resident : Model
    {

        string residentId;
        string residentName;
        string residentPassword;
        string residentPhone;
        string residentEmail;
        int unitNumber;
        string buildingId;
        string residentImage;
        bool isAdmin;

        public Resident() 
        {
          
        }

        public string? ResidentId
        {
            get { return residentId; }
            set
            {
                
                    residentId = value;

            }
        }

        [Required(ErrorMessage = "Name Can Not Be Empty")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Name Must Be Bettwen 2-20 Characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ' -]+$", ErrorMessage = "Name can only contain letters, spaces, hyphens, and apostrophes")]
        public string ResidentName
        {
            get { return residentName; }
            set { residentName = value;
                ValidateProperty(value, "ResidentName");
            }
        }

        [Required(ErrorMessage ="Password Can Not Be Empty")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Name Must Be Bettwen 8-16 Characters")]

        public string ResidentPassword
        {
            get { return residentPassword; }
            set { residentPassword = value;
                ValidateProperty(value, "ResidentPassword");
            }
        }

        [Required(ErrorMessage = "Phone Number Can Not Be Empty")]
        [Phone(ErrorMessage ="Phone Number Must Be Valid")]
        public string ResidentPhone
        {
            get { return residentPhone; }
            set { residentPhone = value;
                ValidateProperty(value, "ResidentPhone");
            }

        }


        [Required(ErrorMessage = "Email Address Can Not Be Empty")]
        [EmailAddress(ErrorMessage ="Email Addreess Must Be Valid")]
        public string ResidentEmail
        {
            get { return residentEmail; }
            set { residentEmail = value;
                ValidateProperty(value, "ResidentEmail");
            }
        }

        [Range(1, int.MaxValue, ErrorMessage = "Unit number cannot be negative or zero.")]
        public int UnitNumber
        {
            get { return unitNumber; }
            set { unitNumber = value;
                if(IsValidationEnabled)
                    ValidateProperty(value, "UnitNumber");
            }
        }
        public string? BuildingId
        {
            get { return buildingId; }
            set {
                if (value == null)
                    buildingId = "0";
                else
                    buildingId = value;     
            }
        }
        public string? ResidentSalt { get; set; } = "";


        public string ResidentImage
        {
            get { return residentImage;}
            set
            {
                if (value == null)
                    residentImage = "0.jpg";
                else
                    residentImage = value;
            }
        }
        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; }
        }

    }
}


