using BuildingManagementWsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoWpfApp.AppWindows;

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Announcement.xaml
    /// </summary>
    public partial class Announcement : UserControl
    {
        //List<Notification> annoucnemnetslist;
        NewAnnouncement newAnnouncement;
        AnnouncementDetails announcementDetails;
        private DispatcherTimer _dispatcherTimer;

        public Announcement()
        {
            InitializeComponent();
          
            _ =  GetAnnouncementList();
            InitializetTimer();
            this.Unloaded += UnLoadedTimer;
        }
        private void InitializetTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(30);
            _dispatcherTimer.Tick += Timer_Tick;
            _dispatcherTimer.Start();
        }
        private async Task GetAnnouncementList()
        {

            ApiClient<List<Notification>> client = new ApiClient<List<Notification>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/GetNotifications";
            client.AddParameter("buildingId", Session.BuildingId);
            listViewAnnc.ItemsSource = await client.GetAsync();

           
          
        }

        private async void btnAddAnnouncement_Click(object sender, RoutedEventArgs e)
        {
            bool? response = CreateNewAnnouncement();
            if (response == true)
                await GetAnnouncementList();
        }
        private bool? CreateNewAnnouncement()
        {
            if (this.newAnnouncement == null)
                this.newAnnouncement = new NewAnnouncement();
            this.newAnnouncement.Owner = Window.GetWindow(this);
            bool? response = this.newAnnouncement.ShowDialog();
            this.newAnnouncement = null;
            return response;
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Notification model = (sender as Button).DataContext as Notification;
            string notificationIdId = model.NotificationId;
            

            ApiClient<string> client = new ApiClient<string>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/RemoveNotification";
            //client.AddParameter("notificationId", notificationIdId);
            ApiResponse<bool> apiResponse = await client.PostAsyncReturn<string, bool>(notificationIdId);
            if (apiResponse.Success && apiResponse.Data)
            {
                MessageBox.Show("Deleted");
                await GetAnnouncementList();
            }
            else
                MessageBox.Show("Unable To Delete","Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private bool? OpenViewDetailsWindow(Notification model)
        {
            if (this.announcementDetails == null)
                this.announcementDetails = new AnnouncementDetails(model);
            this.announcementDetails.Owner = Window.GetWindow(this);
            bool? response = this.announcementDetails.ShowDialog();
            this.announcementDetails = null;
            return response;
        }
        private async void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            Notification model = (sender as Button).DataContext as Notification;
            bool? response = OpenViewDetailsWindow(model.Clone());

            if (response == true)
                await GetAnnouncementList();



        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            await GetAnnouncementList();
        }
        private void UnLoadedTimer(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();
        }
    }
}
