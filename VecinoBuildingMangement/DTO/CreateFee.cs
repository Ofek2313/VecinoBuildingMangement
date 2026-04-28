using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangement.DTO
{
    public class CreateFee
    {
        public List<string> FeeRecipientIds { get; set; } = new List<string>();


        public Fee Fee { get; set; } = new Fee();

       


    }
}
