using System.Data;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{
    public class NotificationRepository : GenericRepository<Notification>, IRepository<Notification>
    {
        public NotificationRepository(DbHelperOleDb dbHelperOleDb, ModelCreator modelCreator)
            : base(dbHelperOleDb, modelCreator) { }
        //public bool Create(Notification model)
        //{
        //    string sql = @$"Insert Into Notification(NotificationMessage,NotificationTitle,NotificationDate)
        //                    Values(@NotificationMessage,@NotificationTitle,@NotificationDate)";
        //    this.dbHelperOleDb.AddParameter("@NotificationMessage", model.NotificationMessage);
        //    this.dbHelperOleDb.AddParameter("@NotificationTitle", model.NotificationTitle);
        //    this.dbHelperOleDb.AddParameter("@NotificationDate", model.NotificationDate);
            
        //    return this.dbHelperOleDb.Insert(sql) > 0;
        //}

        //public bool Delete(string id)
        //{
        //    string sql = @"Delete from Notification where NotificationId=@NotificationId";
        //    this.dbHelperOleDb.AddParameter("@NotificationId", id);
        //    return this.dbHelperOleDb.Delete(sql) > 0;
        //}

        //public List<Notification> GetAll()
        //{
        //    string sql = "Select * From Notification";

        //    List<Notification> notifications = new List<Notification>();
        //    using (IDataReader reader = this.dbHelperOleDb.Select(sql))
        //    {
        //        while (reader.Read())
        //        {

        //            notifications.Add(this.modelCreators.NotificationCreator.CreateModel(reader));

        //        }
        //    }

        //    return notifications;
        //}

        //public Notification GetById(string id)
        //{
        //    string sql = "Select * From Notification Where NotificationId=@NotificationId";
        //    dbHelperOleDb.AddParameter("@NotificationId", id);

        //    using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
        //    {
        //        dataReader.Read();
        //        return this.modelCreators.NotificationCreator.CreateModel(dataReader);
        //    }
        //}

        //public bool Update(Notification model)
        //{
        //    string sql = @$"Update Notification set NotificationMessage=@NotificationMessage,
        //                    NotificationTitle=@NotificationTitle,NotificationDate=@NotificationDate";
        //    this.dbHelperOleDb.AddParameter("@NotificationMessage", model.NotificationMessage);
        //    this.dbHelperOleDb.AddParameter("@NotificationTitle", model.NotificationTitle);
        //    this.dbHelperOleDb.AddParameter("@NotificationDate", model.NotificationDate);

        //    return this.dbHelperOleDb.Update(sql) > 0;
        //}

        public List<Notification> GetNotificationsByResidentId(string residentId)
        {
            string sql = "SELECT Notification.NotificationId,NotificationMessage,NotificationTitle,NotificationDate,Priority,IsPinned FROM Notification INNER JOIN ResidentNotification rn ON Notification.NotificationId = rn.NotificationId WHERE rn.ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);

            List<Notification> notifications = new List<Notification>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    notifications.Add(this.modelCreator.CreateModel<Notification>(reader));

                }
            }

            return notifications;
        }

        public List<Notification> GetPinnedNotificationsByResidentId(string residentId)
        {
            string sql = "SELECT Notification.NotificationId,NotificationMessage,NotificationTitle,NotificationDate,Priority,IsPinned FROM Notification INNER JOIN ResidentNotification rn ON Notification.NotificationId = rn.NotificationId WHERE rn.ResidentId = @ResidentId AND IsPinned=true";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);

            List<Notification> notifications = new List<Notification>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    notifications.Add(this.modelCreator.CreateModel<Notification>(reader));

                }
            }

            return notifications;
        }
        public List<Notification> GetAllNotificationsByResidentId(string residentId)
        {
            string sql = "SELECT Notification.NotificationId,NotificationMessage,NotificationTitle,NotificationDate,Priority,IsPinned FROM Notification INNER JOIN ResidentNotification rn ON Notification.NotificationId = rn.NotificationId WHERE rn.ResidentId = @ResidentId";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);

            List<Notification> notifications = new List<Notification>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    notifications.Add(this.modelCreator.CreateModel<Notification>(reader));

                }
            }

            return notifications;
        }

        public bool DeleteByResidentId(string residentId)
        {
            string sql = "DELETE FROM ResidentNotification WHERE ResidentId = @ResidentId;";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }
        public List<Notification> GetNotificationsByBuildingId(string buildingId)
        {
            string sql = @$"SELECT DISTINCT Notification.* FROM Resident
                        INNER JOIN (Notification INNER JOIN ResidentNotification ON Notification.NotificationId = ResidentNotification.NotificationId )
                        ON Resident.ResidentId = ResidentNotification.ResidentId WHERE Resident.BuildingId = @BuildingId";
            this.dbHelperOleDb.AddParameter("@BuildingId", buildingId);
            List<Notification> notifications = new List<Notification>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    notifications.Add(this.modelCreator.CreateModel<Notification>(reader));

                }
            }

            return notifications;
        }

        public bool CreateResidentNotification(string notificationId, string residentId)
        {
            string sql = $@"Insert Into ResidentNotification(ResidentId,NotificationId) VALUES (@ResidentId,@NotificationId)";
            this.dbHelperOleDb.AddParameter("@ResidentId", residentId);
            this.dbHelperOleDb.AddParameter("@NotificationId", notificationId);
            return this.dbHelperOleDb.Insert(sql) > 0;
        }
    }
}
