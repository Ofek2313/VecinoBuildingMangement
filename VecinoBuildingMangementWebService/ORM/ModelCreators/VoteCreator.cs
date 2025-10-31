using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class VoteCreator : IModelCreator<Vote>
    {
        public Vote CreateModel(IDataReader dataReader)
        {
            Vote vote = new Vote();
            vote.VoteId = Convert.ToString(dataReader["VoteId"]);
            vote.VoteChoice = Convert.ToString(dataReader["VoteChoice"]);
            vote.VoteDate = Convert.ToString(dataReader["VoteDate"]);
            vote.ResidentId = Convert.ToString(dataReader["ResidentId"]);
            vote.PollId = Convert.ToString(dataReader["PollId"]);

            return vote;
        }
    }
}
