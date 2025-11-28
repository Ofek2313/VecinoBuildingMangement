using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class VoteRepository : Repository, IRepository<Vote>
    {
        public VoteRepository(DbHelperOleDb dbHelperOleDb, ModelCreators modelCreators)
            : base(dbHelperOleDb, modelCreators) { }
        public bool Create(Vote model)
        {
            string sql = @$"Insert Into Vote(OptionId,VoteDate,ResidentId,PollId)
                            Values(@OptionId,@VoteDate,@ResidentId,@PollId)";
            this.dbHelperOleDb.AddParameter("@OptionId", model.OptionId);
            this.dbHelperOleDb.AddParameter("@VoteDate", model.VoteDate);
            this.dbHelperOleDb.AddParameter("@ResidentId", model.ResidentId);
            this.dbHelperOleDb.AddParameter("@PollId", model.PollId);

            return this.dbHelperOleDb.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = @"Delete from Vote where VoteId=@VoteId";
            this.dbHelperOleDb.AddParameter("@VoteId", id);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }

        public List<Vote> GetAll()
        {
            string sql = "Select * From Vote";

            List<Vote> votes = new List<Vote>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    votes.Add(this.modelCreators.VoteCreator.CreateModel(reader));

                }
            }

            return votes;
        }

        public Vote GetById(string id)
        {
            string sql = "Select * From Vote Where VoteId=@VoteId";
            dbHelperOleDb.AddParameter("@VoteId", id);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                dataReader.Read();
                return this.modelCreators.VoteCreator.CreateModel(dataReader);
            }
        }

        public bool Update(Vote model)
        {
            string sql = @"Update Vote set VoteChoice = @VoteChoice,VoteDate = @VoteDate
                           ResidentId = @ResidentId,PollId = @PollId";
            this.dbHelperOleDb.AddParameter("@OptionId", model.OptionId);
            this.dbHelperOleDb.AddParameter("@VoteDate", model.VoteDate);
            this.dbHelperOleDb.AddParameter("@ResidentId", model.ResidentId);
            this.dbHelperOleDb.AddParameter("@PollId", model.PollId);
            
            return this.dbHelperOleDb.Update(sql) > 0;
        }
        public int CountVoteByOption(string optionId)
        {
            string sql = "SELECT COUNT(*) FROM Vote where OptionId=@OptionId";
            this.dbHelperOleDb.AddParameter("@OptionId", optionId);
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    return Convert.ToInt32(reader[0]);

                }
            }
            return 0;
        }
    }
}
