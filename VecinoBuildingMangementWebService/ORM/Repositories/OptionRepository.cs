using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService.ORM.Repositories
{
    public class OptionRepository : GenericRepository<Option>
    {
        public OptionRepository(DbHelperOleDb dbHelperOleDb)
            : base(dbHelperOleDb) { }

        public List<Option> GetOptionsByPollId(string pollId)
        {
            string sql = "SELECT Option.OptionId,Option.PollId,Option.OptionText FROM Poll INNER JOIN [Option] ON Poll.PollId = Option.PollId WHERE Option.PollId = @PollId";
            this.dbHelperOleDb.AddParameter("@PollId", pollId);
            List<Option> options = new List<Option>();

            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    options.Add(this.ModelCreator.CreateModel(reader));

                }
            }
           return options;
        }
        public bool DeleteByPollId(string pollId)
        {
            string sql = @"Delete from Option where PollId=@PollId";
            this.dbHelperOleDb.AddParameter("@PollId", pollId);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }
    }
}
