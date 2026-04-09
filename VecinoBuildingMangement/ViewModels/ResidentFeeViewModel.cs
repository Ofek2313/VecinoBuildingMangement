using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.ViewModels
{
    public class ResidentFeeViewModel
    {

    
        public string ResidentName { get; set; }

        public string ResidentImage { get; set; }

        public int UnitNumber { get; set; }

        public Fee Fee { get; set; }

        public ResidentFeeViewModel Clone()
        {
            return new ResidentFeeViewModel
            {
                Fee = new Fee
                {
                    FeeId = Fee.FeeId,
                    FeeTitle = Fee.FeeTitle,
                    FeeAmount = Fee.FeeAmount,
                    FeeDueDate = Fee.FeeDueDate,
                    IsPaid = Fee.IsPaid,
                    ResidentId = Fee.ResidentId,
                    PaymentDate = Fee.PaymentDate
                },
                ResidentName = ResidentName,
                ResidentImage = ResidentImage,
                UnitNumber = UnitNumber,

            };
        }

    }
}
