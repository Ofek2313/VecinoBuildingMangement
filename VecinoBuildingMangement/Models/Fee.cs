using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class Fee : Model
    {
        string feeId;
        string feeTitle;
        double feeAmount;
        string feeDueDate;
        bool isPaid;
        string residentId;
        string paymentDate;

        public Fee() { }
        public string? FeeId
        {
            get { return this.feeId; }
            set { this.feeId = value; }
        }

        [Required(ErrorMessage = "Fee Title can not be empty")]
        [StringLength(60,MinimumLength =5,ErrorMessage = "Fee Title must be between 5-60 characters")]
        public string FeeTitle
        {
            get { return this.feeTitle; }
            set { this.feeTitle = value;
                ValidateProperty(value, "FeeTitle");
            }
            
        }

        [Required(ErrorMessage = "Fee Amount can not be empty")]
        [IsDigit(ErrorMessage = "Fee Amount Must Be A valid Number")]
        public double FeeAmount
        {
            get { return this.feeAmount; }
            set { this.feeAmount = value;
                ValidateProperty(value, "FeeAmount");
            }
        }

        [Required(ErrorMessage = "Fee Due Date can not be empty")]
        //[Date(ErrorMessage = "Date needs to be a valid date")]
        [FutureDate(ErrorMessage = "Please enter a date that is today or in the future")]
        public string FeeDueDate
        {
            get { return this.feeDueDate; }
            set { this.feeDueDate = value;
                if (IsValidationEnabled)
                    ValidateProperty(value, "FeeDueDate");
            }
        }

        [Required(ErrorMessage = "Is Paid can not be empty")]
        public bool IsPaid
        { 
            get { return this.isPaid; }
            set { this.isPaid = value; }
        }
        public string ResidentId
        {
            get { return this.residentId; }
            set { this.residentId = value; }
        }

        public string PaymentDate
        {
            get { return this.paymentDate; }
            set { this.paymentDate = value; }
        }
    }
}
