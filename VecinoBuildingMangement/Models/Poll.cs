using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class Poll
    {
        string pollId;
        string pollTitle;
        string pollDate;
        string buildingId;

        public string PollId
        {
            get { return pollId; }
            set { pollId = value; }
        }
        [Required(ErrorMessage = "Poll Title Can not be empty")]
        public string PollTitle
        { 
            get { return pollTitle; } 
            set { pollTitle = value; }
        }
        [Required]
        public string PollDate
        {
            get { return pollDate; }
            set { pollDate = value; }
        }
        public string BuildingId
        {
            get { return buildingId; }
            set { buildingId = value; }
        }
    }
}
