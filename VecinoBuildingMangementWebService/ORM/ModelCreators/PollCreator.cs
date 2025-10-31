using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class PollCreator : IModelCreator<Poll>
    {
       public Poll CreateModel(IDataReader dataReader)
        {
            Poll poll = new Poll();
            poll.PollId = Convert.ToString(dataReader["PollId"]);
            poll.PollTitle = Convert.ToString(dataReader["PollTitle"]);
            poll.PollDate = Convert.ToString(dataReader["PollDate"]);
            poll.BuildingId = Convert.ToString(dataReader["BuildingId"]);

            return poll;
        }
        
    }
}
