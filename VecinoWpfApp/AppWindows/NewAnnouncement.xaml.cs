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
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoWpfApp.UserControls;

namespace VecinoWpfApp.AppWindows
{
    /// <summary>
    /// Interaction logic for NewAnnouncement.xaml
    /// </summary>
    public partial class NewAnnouncement : Window
    {
        private List<ResidentCheckItem> allResidents = new List<ResidentCheckItem>();
        List<Resident> Residents;
        Notification notification = new Notification();

        public NewAnnouncement()
        {
            InitializeComponent();
            _ = GetResidentsList();

            this.DataContext = notification;
        }
        private async Task GetResidentsList()
        {
            ApiClient<List<Resident>> client = new ApiClient<List<Resident>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/GetResidents";
            client.AddParameter("buildingId", Session.BuildingId);
            Residents = await client.GetAsync();
            allResidents = Residents.Select(r => new ResidentCheckItem
            {
                ResidentId = r.ResidentId,
                ResidentName = r.ResidentName,
                IsChecked = false,
            }).ToList();
            ListViewResidents.ItemsSource = allResidents;
        }
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

            string query = TxtSearch.Text.ToLower();
            ListViewResidents.ItemsSource = allResidents.Where(r => r.ResidentName.ToLower().Contains(query)).ToList();


        }

        private void ChkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
           allResidents.ForEach(r => r.IsChecked = true);
            ListViewResidents.Items.Refresh();
        }

        private void ChkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            allResidents.ForEach(r => r.IsChecked = false);
            ListViewResidents.Items.Refresh();
        }

        private async void BtnPublish_Click(object sender, RoutedEventArgs e)
        {
            SendNotificationViewModel viewModel = new SendNotificationViewModel();

            string AnnouncementTitle = AnnouncementTitleTextBox.Text;
            string AnnouncementMessage = AnnouncementMessageTextBox.Text;
            string Priority = CmbPriority.Text;
            bool IsPinned = ChkPinMessage.IsChecked == true;

            viewModel.ResidentIds = allResidents.Where(r => r.IsChecked).Select(r => r.ResidentId).ToList();
            if (!viewModel.ResidentIds.Any())
            {
                MessageBox.Show("Please select at least one resident", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

           
           
            notification.NotificationId = " ";
            notification.NotificationMessage = AnnouncementMessage;
            notification.NotificationTitle = AnnouncementTitle;
            notification.NotificationDate = " ";
            notification.Priority = Priority;
            notification.IsPinned = IsPinned;
            viewModel.Notification = notification;


            viewModel.Notification.Validate();
            if (viewModel.Notification.HasErrors)
            {
                MessageBox.Show(" The announcement details are invalid. Please check your input. ", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
                ApiClient<SendNotificationViewModel> client = new ApiClient<SendNotificationViewModel>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Admin/SendNotification";

                ApiResponse<bool> apiResponse = await client.PostAsyncReturn<SendNotificationViewModel, bool>(viewModel);

                if (apiResponse.Data && apiResponse.Success)
                {
                    this.DialogResult = true;
                    this.Close();
                }
            
            
        }
        private void SetLoading(bool loading)
        {

            if(loading)
            {
                
            }
        }
    }
}
