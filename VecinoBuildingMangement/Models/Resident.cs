
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

        public Resident() 
        {
          
        }


        public string? ResidentSalt { get; set; }



        public string? ResidentId
        {
            get { return residentId; }
            set { residentId = value;
                
            }
        }

        [Required(ErrorMessage = "Name Can Not Be Empty")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Name Must Be Bettwen 2-20 Characters")]
        [FirstLetterCapital(ErrorMessage = "The First Letter Must Be Capitalized")]
        public string ResidentName
        {
            get { return residentName; }
            set { residentName = value;
                ValidateProperty(value, "ResidentName");
            }
        }

        [Required(ErrorMessage ="Password Can Not Be Empty")]
        [StringLength(15,MinimumLength =8,ErrorMessage ="Password Must Be At Least 8 Characters")]
        [CapitalLetter(ErrorMessage ="Password Must Include At Least 1 Capital Letter")]
        [ContainSymbol(ErrorMessage = "Password Must Include At Least 1 Symbol")]
       
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

        [Required(ErrorMessage ="Unit Number Can Not Be Empty")]
        public int UnitNumber
        {
            get { return unitNumber; }
            set { unitNumber = value;
                ValidateProperty(value, "UnitNumber");
            }
        }
        public string? BuildingId
        {
            get { return buildingId; }
            set { buildingId = value; }
        }


    }
}


