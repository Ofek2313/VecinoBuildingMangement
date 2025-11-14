using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class NotificationRepository : Repository, IRepository<Notification>
    {
        public NotificationRepository(DbHelperOleDb dbHelperOleDb, ModelCreators modelCreators)
            : base(dbHelperOleDb, modelCreators) { }
        public bool Create(Notification model)
        {
            string sql = @$"Insert Into Notification(NotificationMessage,NotificationTitle,NotificationDate)
                            Values(@NotificationMessage,@NotificationTitle,@NotificationDate)";
            this.dbHelperOleDb.AddParameter("@NotificationMessage", model.NotificationMessage);
            this.dbHelperOleDb.AddParameter("@NotificationTitle", model.NotificationTitle);
            this.dbHelperOleDb.AddParameter("@NotificationDate", model.NotificationDate);
            
            return this.dbHelperOleDb.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            string sql = @"Delete from Notification where NotificationId=@NotificationId";
            this.dbHelperOleDb.AddParameter("@NotificationId", id);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }

        public List<Notification> GetAll()
        {
            string sql = "Select * From Notification";

            List<Notification> notifications = new List<Notification>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    notifications.Add(this.modelCreators.NotificationCreator.CreateModel(reader));

                }
            }

            return notifications;
        }

        public Notification GetById(string id)
        {
            string sql = "Select * From Notification Where NotificationId=@NotificationId";
            dbHelperOleDb.AddParameter("@NotificationId", id);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                dataReader.Read();
                return this.modelCreators.NotificationCreator.CreateModel(dataReader);
            }
        }

        public bool Update(Notification model)
        {
            string sql = @$"Update Notification set NotificationMessage=@NotificationMessage,
                            NotificationTitle=@NotificationTitle,NotificationDate=@NotificationDate";
            this.dbHelperOleDb.AddParameter("@NotificationMessage", model.NotificationMessage);
            this.dbHelperOleDb.AddParameter("@NotificationTitle", model.NotificationTitle);
            this.dbHelperOleDb.AddParameter("@NotificationDate", model.NotificationDate);

            return this.dbHelperOleDb.Update(sql) > 0;
        }
        
    }
}
