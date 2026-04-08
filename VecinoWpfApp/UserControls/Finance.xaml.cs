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
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoWpfApp.AppWindows;

namespace VecinoWpfApp.UserControls
{
   
    /// <summary>
    /// Interaction logic for Finance.xaml
    /// </summary>
    public partial class Finance : UserControl
    {
        ManageAdminFinance manageAdminFinance;
        NewFee newFee;
        FeeDetail feeDetail;
        public Finance()
        {
            InitializeComponent();
            LoadFinanceData();
        }
        private async Task LoadFinanceData()
        {

            ApiClient<ManageAdminFinance> client = new ApiClient<ManageAdminFinance>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/GetBuildingFinance";
            client.AddParameter("buildingId", Session.BuildingId);
            manageAdminFinance = await client.GetAsync();
            this.listViewFees.ItemsSource = manageAdminFinance.Finances;
            this.ListViewTransaction.ItemsSource = manageAdminFinance.Transaction;
            this.DataContext = manageAdminFinance;
        }

        private async void SendReminder_Click(object sender, RoutedEventArgs e)
        {
            
            ResidentFeeViewModel item = (sender as Button).DataContext as ResidentFeeViewModel;
            if(item.Fee.IsPaid)
            {
                MessageBox.Show("Already Paid");
                return;
            }    
            SendNotificationViewModel sendNotificationViewModel = new SendNotificationViewModel();
            ApiClient<SendNotificationViewModel> client = new ApiClient<SendNotificationViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/SendNotification";
            Notification notification = new Notification();
            notification.NotificationId = "";
            notification.NotificationMessage = $"Hello {item.ResidentName}, This is a reminder to pay your fee";
            notification.NotificationTitle = "Payment Reminder";
            notification.NotificationDate = DateTime.Now.ToShortDateString();
            notification.Priority = "Urgent";
            notification.IsPinned = false;
            List<string> ids = new List<string>();
            ids.Add(item.Fee.ResidentId);
            sendNotificationViewModel.ResidentIds = ids;
            sendNotificationViewModel.Notification = notification;
            bool response = await client.PostAsync(sendNotificationViewModel);
            if (response)
                MessageBox.Show("Reminder Sent");
            else
                MessageBox.Show("Failed");

        }
        private bool? ViewCreateFeeWindow()
        {
            if (this.newFee == null)
                this.newFee = new NewFee();
            this.newFee.Owner = Window.GetWindow(this);
            bool? response = this.newFee.ShowDialog();
            this.newFee = null;
            return response;
        }
        private async void AddFeeButton_Click(object sender, RoutedEventArgs e)
        {
            bool? response = ViewCreateFeeWindow();
            if (response == true)
                await LoadFinanceData();
        }
        private bool? ViewFeeDetailWindow()
        {
            if(this.feeDetail == null)
                this.feeDetail = new FeeDetail();
            this.feeDetail.Owner = Window.GetWindow(this);
            bool? response = this.feeDetail.ShowDialog();
            this.feeDetail = null;
            return response;
        }

        private void ViewFeeButton_Click(object sender, RoutedEventArgs e)
        {
            bool? response = ViewFeeDetailWindow();
        }
    }
}
