using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService.ORM.Repositories
{
    public class OptionRepository : GenericRepository<Option>
    {
        public OptionRepository(DbHelperOleDb dbHelperOleDb)
            : base(dbHelperOleDb) { }

        public override bool Create(Option model)
        {
            string sql = @$"Insert Into [Option](PollId,OptionText)
                            Values(@PollId,@OptionText)";
            this.dbHelperOleDb.AddParameter("@PollId", model.PollId);
            this.dbHelperOleDb.AddParameter("@OptionText", model.OptionText);
            return this.dbHelperOleDb.Insert(sql) > 0;
        }

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
            string sql = @"Delete FROM [Option] where PollId=@PollId";
            this.dbHelperOleDb.AddParameter("@PollId", pollId);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }
    }
}
