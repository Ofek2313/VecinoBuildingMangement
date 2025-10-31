using System.Data;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService
{
    public class NotificationCreator : IModelCreator<Notification>
    {
        public Notification CreateModel(IDataReader dataReader)
        {
            Notification notification = new Notification();
            notification.NotificationId = Convert.ToString(dataReader["NotificationId"]);
            notification.NotificationTitle = Convert.ToString(dataReader["NotificationTitle"]);
            notification.NotificationMessage = Convert.ToString(dataReader["NotificationMessage"]);
            notification.NotificationDate = Convert.ToString(dataReader["NotificationDate"]);

            return notification;
        }
    }
}
