using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.DTO
{
    public class VoteDeleteRequest
    {
        public string ResidentId {  get; set; }

        public string PollId { get; set; }
    }
}
