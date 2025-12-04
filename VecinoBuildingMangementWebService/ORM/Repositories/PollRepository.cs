using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class PollRepository : GenericRepository<Poll>, IRepository<Poll>
    {
        public PollRepository(DbHelperOleDb dbHelperOleDb)
            : base(dbHelperOleDb) { }
        //public bool Create(Poll model)
        //{
        //    string sql = @$"Insert Into Poll(PollTitle,PollDate,BuildingId)
        //                    Values(@PollTitle,@PollDate,@BuildingId)";
        //    this.dbHelperOleDb.AddParameter("@PollTitle", model.PollTitle);
        //    this.dbHelperOleDb.AddParameter("@PollDate", model.PollDate);
        //    this.dbHelperOleDb.AddParameter("@BuildingId", model.BuildingId);

        //    return this.dbHelperOleDb.Insert(sql) > 0;
        //}

        //public bool Delete(string id)
        //{
        //    string sql = @"Delete from Poll where PollId=@PollId";
        //    this.dbHelperOleDb.AddParameter("@PollId", id);
        //    return this.dbHelperOleDb.Delete(sql) > 0;
        //}

        //public List<Poll> GetAll()
        //{
        //    string sql = "Select * From Poll";

        //    List<Poll> polls = new List<Poll>();
        //    using (IDataReader reader = this.dbHelperOleDb.Select(sql))
        //    {
        //        while (reader.Read())
        //        {

        //            polls.Add(this.modelCreators.PollCreator.CreateModel(reader));

        //        }
        //    }

        //    return polls;
        //}

        //public Poll GetById(string id)
        //{
        //    string sql = "Select * From Poll Where PollId=@PollId";
        //    dbHelperOleDb.AddParameter("@PollId", id);

        //    using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
        //    {
        //        dataReader.Read();
        //        return this.modelCreators.PollCreator.CreateModel(dataReader);
        //    }
        //}

        //public bool Update(Poll model)
        //{
        //    throw new NotImplementedException();
        //}

        public List<Poll> GetPollByBuildingId(string buildingId)
        {
            string sql = "Select * From Poll where BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);

            List<Poll> polls = new List<Poll>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    polls.Add(this.ModelCreator.CreateModel(reader));

                }
            }

            return polls;
        }
        public List<Poll> GetActivePollsByBuilding(string buildingId)
        {
            string sql = "Select * From Poll where BuildingId = @BuildingId AND IsActive = True";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);

            List<Poll> polls = new List<Poll>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    polls.Add(this.ModelCreator.CreateModel(reader));

                }
            }

            return polls;
        }
    }
}
