using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class Option
    {
        string optionId;
        string pollId;
        string optionText;

        public string OptionId
        {
            get { return this.optionId; }
            set { this.optionId = value; }
        }

        public string PollId
        {
            get { return this.pollId; }
            set { this.pollId = value; }
        }

        [Required(ErrorMessage = "Poll Option can not be empty")]
        public string OptionText
        {
            get { return this.optionText; }
            set { this.optionText = value; }
        }
    }
}
