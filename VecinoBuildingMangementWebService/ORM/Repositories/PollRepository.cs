using System.Collections.Generic;
using System.Data;
using System.Globalization;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{
    public class PollRepository : GenericRepository<Poll>, IRepository<Poll>
    {
        public PollRepository(DbHelperOleDb dbHelperOleDb, ModelCreator modelCreator)
            : base(dbHelperOleDb, modelCreator) { }
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

                    polls.Add(this.modelCreator.CreateModel<Poll>(reader));

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

                    polls.Add(this.modelCreator.CreateModel<Poll>(reader));

                }
            }

            return polls;
        }
        public List<Poll> GetInActivePollsByBuilding(string buildingId)
        {
            string sql = "Select * From Poll where BuildingId = @BuildingId AND IsActive = False";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);

            List<Poll> polls = new List<Poll>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    polls.Add(this.modelCreator.CreateModel<Poll>(reader));

                }
            }

            return polls;
        }
       public List<PollViewModel> GetPollViewModels(string buildingId,string residentId)
        {
            string sql =  @$"SELECT
                            Poll.PollId,
                            Poll.PollTitle,
                            Poll.PollDescription,
                            Poll.IsActive,
                            Poll.PollDate,
                            Poll.BuildingId,
                            o.OptionId,
                            o.OptionText,
                            Count(v.VoteId) AS VoteCount,
                            MAX(IIf(v.ResidentId = @ResidentId, 1, 0)) AS HasVoted
                        FROM
                            (
                                Poll
                                INNER JOIN [Option] o ON o.PollId = Poll.PollId
                            )
                            LEFT JOIN Vote v ON v.OptionId = o.OptionId
                        Where Poll.BuildingId = @BuildingId
                        GROUP BY
                            Poll.PollId,
                            Poll.PollTitle,
                            Poll.PollDescription,
                            Poll.IsActive,
                            Poll.PollDate,
                            Poll.BuildingId,
                            o.OptionId,
                            o.OptionText;";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            List<PollDatabaseFlat> flatRows = new List<PollDatabaseFlat>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    flatRows.Add(this.modelCreator.CreateModel<PollDatabaseFlat>(reader)); //new PollDatabaseFlat
                    //{
                    //    PollId = Convert.ToString(reader["PollId"]),
                    //    PollTitle = Convert.ToString(reader["PollTitle"]),
                    //    PollDescription = Convert.ToString(reader["PollDescription"]),
                    //    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    //    PollDate = Convert.ToString(reader["PollDate"]),
                    //    BuildingId = Convert.ToString(reader["BuildingId"]),
                    //    OptionId = Convert.ToString(reader["OptionId"]),
                    //    OptionText = Convert.ToString(reader["OptionText"]),
                    //    VoteCount = Convert.ToInt32(reader["VoteCount"]),
                    //    HasVoted = Convert.ToBoolean(reader["HasVoted"])
                    //});

                }

            }

            return flatRows.GroupBy(r => r.PollId).Select(group =>
            { 
                int totalVotes = group.Sum(r => r.VoteCount);
                return new PollViewModel
                {

                    poll = new Poll
                    {
                        PollId = group.Key,
                        PollTitle = group.First().PollTitle,
                        PollDescription = group.First().PollDescription,
                        IsActive = group.First().IsActive,
                        PollDate = group.First().PollDate,
                        BuildingId = group.First().BuildingId,

                    },
                    options = group.Select(r => new OptionViewModel
                    {
                        option = new Option
                        {
                            OptionId = r.OptionId,
                            OptionText = r.OptionText,
                            PollId = r.PollId,
                        },
                        voted = r.VoteCount,
                        percentage = totalVotes > 0 ? (int)Math.Round((double)r.VoteCount / totalVotes * 100) : 0
                    }).ToList(),
                    HasVoted = group.Any(r => r.HasVoted),

                };
                }).OrderBy(r => DateTime.ParseExact(r.poll.PollDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();


        }
        public List<OptionViewModel> GetPollResultById(string pollId)
        {
            string sql = $@"SELECT
                            o.OptionId,
                            o.PollId,
                            o.OptionText,
                            Count(v.VoteId) AS VoteCount
                        FROM
                            [Option] o
                            LEFT JOIN Vote v ON o.OptionId = v.OptionId
                        WHERE
                            o.PollId = @PollId
                        GROUP BY
                            o.OptionId,
                            o.PollId,
                            o.OptionText;";

            this.dbHelperOleDb.AddParameter("@PollId", pollId);
            List < OptionViewModel> options = new List < OptionViewModel>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    options.Add(new OptionViewModel
                    {
                        option = this.modelCreator.CreateModel<Option>(reader),
                       
                        voted = Convert.ToInt32(reader["VoteCount"]),


                    });

                }

            }
            int totalVotes = options.Sum(o => o.voted);
            options.ForEach(o => o.percentage = totalVotes > 0 ? (int)Math.Round((double)o.voted / totalVotes * 100) : 0);

            return options;

        }
        public bool UpdatePollStatus(string pollId,bool IsActive)
        {
            string sql = "Update Poll Set IsActive = @IsActive WHERE PollId = @PollId";

            this.dbHelperOleDb.AddParameter("@IsActive", IsActive);
            this.dbHelperOleDb.AddParameter("@PollId", pollId);

            return this.dbHelperOleDb.Update(sql) > 0;
        }

    }
  
       
}

