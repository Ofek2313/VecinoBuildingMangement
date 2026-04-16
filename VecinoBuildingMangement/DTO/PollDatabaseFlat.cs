using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.DTO
{
    public class PollDatabaseFlat
    {
        public string PollId { get; set; }

        public string PollTitle { get; set; }

        public string PollDescription { get; set; }

        public bool IsActive { get; set; }

        public string PollDate { get; set; }

        public string BuildingId { get; set; }

        public string OptionId { get; set; }

        public string OptionText { get; set; }

        public int VoteCount { get; set; }

        public bool HasVoted { get; set; }

    }
}
