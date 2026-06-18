using BuildingManagementWsClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
using VecinoBuildingMangement;
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
        private DispatcherTimer _dispatcherTimer;
        private string currentFilter = "all";

        public Finance()
        {
            InitializeComponent();
            LoadFinanceData();
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
            client.Path = "api/Admin/SendNotification"; //Uses The Exisitng Send Notification Method that acceepts a list of ids
            Notification notification = new Notification();  // Deafult Values For A Reminder Notification
            notification.NotificationId = "";
            notification.NotificationMessage = $"Hello {item.ResidentName}, This is a reminder to pay your fee";
            notification.NotificationTitle = "Payment Reminder";
            notification.NotificationDate = DateTime.Now.ToShortDateString();
            notification.Priority = "Urgent";
            notification.IsPinned = false;
            notification.CreatedBy = Session.ResidentId; // The Resident If Of an Admin
            List<string> ids = new List<string>();
            ids.Add(item.Fee.ResidentId);
            sendNotificationViewModel.ResidentIds = ids;
            sendNotificationViewModel.Notification = notification;
            ApiResponse<bool> response = await client.PostAsyncReturn<SendNotificationViewModel,bool>(sendNotificationViewModel);
            if (response.Success && response.Data)
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
        private bool? ViewFeeDetailWindow(ResidentFeeViewModel viewModel)
        {
            
            if (this.feeDetail == null)
                this.feeDetail = new FeeDetail(viewModel);
            this.feeDetail.Owner = Window.GetWindow(this);
            bool? response = this.feeDetail.ShowDialog();
            this.feeDetail = null;
            return response;
        }

        private async void ViewFeeButton_Click(object sender, RoutedEventArgs e)
        {
            ResidentFeeViewModel viewModel = (sender as Button).DataContext as ResidentFeeViewModel;
            bool? response = ViewFeeDetailWindow(viewModel.Clone());

            if (response == true)
                await LoadFinanceData();
           

        }
        private void FilterButton_Click(object sender, RoutedEventArgs e) 
        {

            string filter = (sender as Button).Tag.ToString();
            currentFilter = filter;
            ApplyFilter();


            Style Active = this.FindResource("FilterButtonActive") as Style;
            Style NotActive = this.FindResource("FilterButton") as Style;
            FeeFilterAllButton.Style = NotActive;
            FeeFilterPaidButton.Style = NotActive;
            FeeFilterUnpaidButton.Style = NotActive;

            (sender as Button).Style = Active;


        }
        private void ApplyFilter()
        {
            if (manageAdminFinance == null) return;

            List<ResidentFeeViewModel> filteredList;

            switch (currentFilter)
            {
                case "paid":
                    filteredList = manageAdminFinance.Finances.Where(f => f.Fee.IsPaid).ToList();
                        
                    break;

                case "unpaid":
                    filteredList = manageAdminFinance.Finances.Where(f => !f.Fee.IsPaid ).ToList();
                    break;

                

                default:
                    filteredList = manageAdminFinance.Finances;
                    break;
            }

            listViewFees.ItemsSource = filteredList;
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            await LoadFinanceData();
        }
        private void UnLoadedTimer(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();
        }
    }
}
