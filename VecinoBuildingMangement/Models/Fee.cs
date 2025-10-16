using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class Fee
    {
        string feeId;
        string feeTitle;
        double feeAmount;
        string feeDueDate;
        bool isPaid;

        public string FeeId
        {
            get { return this.feeId; }
            set { this.feeId = value; }
        }

        [Required(ErrorMessage = "Fee Title can not be empty")]
        [StringLength(60,MinimumLength =5,ErrorMessage = "Fee Title must be between 5-60 characters")]
        public string FeeTitle
        {
            get { return this.feeTitle; }
            set { this.feeTitle = value; }
        }

        [Required(ErrorMessage = "Fee Amount can not be empty")]
        public double FeeAmount
        {
            get { return this.feeAmount; }
            set { this.feeAmount = value; }
        }

        [Required(ErrorMessage = "Fee Due Date can not be empty")]
        [Date(ErrorMessage = "Date needs to be a valid date")]
        public string FeeDueDate
        {
            get { return this.feeDueDate; }
            set { this.feeDueDate = value; }
        }

        [Required(ErrorMessage = "Is Paid can not be empty")]
        public bool IsPaid
        { 
            get { return this.isPaid; }
            set { this.isPaid = value; }
        }
    }
}
