using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VecinoBuildingMangement.Models
{
    public class Vote
    {
        string voteId;
        string voteChoice;
        string voteDate;
        string residentId;
        string pollId;

        public string VoteId
        {
            get { return voteId; }
            set { voteId = value; }
        }
        public string VoteChoice
        {
            get { return voteChoice; }
            set { voteChoice = value; }
        }
        public string VoteDate
        {
            get { return voteDate; }
            set { voteDate = value; }
        }
        public string ResidentId
        {
            get { return residentId; }
            set { residentId = value; }
        }   
        public string PollId
        {
            get { return pollId; }
            set { pollId = value; }
             
        }
    }
}
