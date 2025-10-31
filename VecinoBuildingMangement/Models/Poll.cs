using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class Poll : Model
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


        [Required(ErrorMessage = "Poll Title can not be empty")]
        [StringLength(100,MinimumLength =10,ErrorMessage = "Poll Title needs to be at least 10 characters")]
        public string PollTitle
        { 
            get { return pollTitle; } 
            set { pollTitle = value;
                ValidateProperty(value, "PollTitle");
            }
        }

        [Date(ErrorMessage = "Date needs to be a valid date")]
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
