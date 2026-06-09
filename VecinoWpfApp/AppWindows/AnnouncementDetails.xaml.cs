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
using System.Windows.Shapes;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoWpfApp.UserControls;

namespace VecinoWpfApp.AppWindows
{
    /// <summary>
    /// Interaction logic for AnnouncementDetails.xaml
    /// </summary>
    public partial class AnnouncementDetails : Window
    {
        private AnnouncementDetailsViewModel viewModel;
        private bool _IsEditing = false;
        public AnnouncementDetails(Notification notification)
        {
            InitializeComponent();
  
            _ = GetAnnouncementDetails(notification);
           
        }

        private void btnCloseFooter_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private async Task GetAnnouncementDetails(Notification notification)
        {
            try
            {
                ApiClient<AnnouncementDetailsViewModel> client = new ApiClient<AnnouncementDetailsViewModel>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Admin/GetResidentsNotification";
                client.AddParameter("notificationId", notification.NotificationId);
                viewModel = await client.GetAsync();
                viewModel.Notification = notification;
                ResidentsListView.ItemsSource = viewModel.Residents;
                this.DataContext = viewModel;
            }
            catch
            {
                MessageBox.Show($"Error loading announcement");
            }
          
        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e) 
        {
            if (!_IsEditing) //Switch Ediitng Mode
            {
                _IsEditing = true;
            
                NotificationTitleTextBox.IsReadOnly = false;
                NotificationMessageTextBox.IsReadOnly = false;



            }
            else // If In Edit Mode Update in database
            {
                viewModel.Notification.Validate();
                if(!viewModel.Notification.HasErrors)
                {
                    ApiClient<Notification> client = new ApiClient<Notification>();
                    client.Host = "localhost";
                    client.Port = 5269;
                    client.Path = "api/Admin/UpdateNotification";
                    ApiResponse<bool> apiResponse = await client.PostAsyncReturn<Notification, bool>(viewModel.Notification);
                    if (!apiResponse.Success || !apiResponse.Data)
                    {
                       
                        _IsEditing = true;
                        //If Update Fail Keep Editing

                    }
                    else
                    {
                        _IsEditing = false;

                        NotificationTitleTextBox.IsReadOnly = false;
                        NotificationMessageTextBox.IsReadOnly = false;
                        this.DialogResult = true;
                        //If Update Success Return To View Model
                    }
                }
               
                
            }
        }

        private async void DeleteAnnouncement(object sender, RoutedEventArgs e)
        {
          
            string notificationId = viewModel.Notification.NotificationId;


            ApiClient<string> client = new ApiClient<string>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/RemoveNotification";
            ApiResponse<bool> apiResponse = await client.PostAsyncReturn<string, bool>(notificationId);
            if (apiResponse.Success && apiResponse.Data)
            {
                MessageBox.Show("Deleted");
                this.DialogResult = true;
                
            }
        }
    }
}
