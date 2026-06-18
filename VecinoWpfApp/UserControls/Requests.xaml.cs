using BuildingManagementWsClient;
using System;
using System.Collections;
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
using VecinoBuildingMangement;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoWpfApp.AppWindows;

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Requests.xaml
    /// </summary>
    public partial class Requests : UserControl
    {
        ManageServiceRequestViewModel serviceRequestViewModel;
        ViewRequestDetail requestDetail;
        private DispatcherTimer _dispatcherTimer;
        private string currentFilter = "all";
        public Requests()
        {
            InitializeComponent();
           
            GetRequestList();
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
        private async Task GetRequestList()
        {
            ApiClient<ManageServiceRequestViewModel> client = new ApiClient<ManageServiceRequestViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/ManageServiceRequest";
            client.AddParameter("buildingId", Session.BuildingId);
            serviceRequestViewModel = await client.GetAsync();

            ApplyFilter();
            this.DataContext = this.serviceRequestViewModel;

        }

        private async void ActionRequest_Click(object sender, RoutedEventArgs e)
        {
            ServiceRequestDetail item = (sender as Button).DataContext as ServiceRequestDetail;
            StatusDto viewModel = new StatusDto();
            
            ApiClient<StatusDto> client = new ApiClient<StatusDto>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/ChangeRequestStatus";

            viewModel.RequestId = item.ServiceRequest.RequestId;

            switch (item.ServiceRequest.RequestStatus)
            {
                case RequestStatus.Pending:
                    viewModel.Status = RequestStatus.InProgress;
                    break;
                case RequestStatus.InProgress:
                    viewModel.Status = RequestStatus.Completed;
                    break;
            }

           ApiResponse<bool> response = await client.PostAsyncReturn<StatusDto,bool>(viewModel);

            if (response.Success && response.Data)
            {
                MessageBox.Show("Status Changed");
                await GetRequestList();
            }
            else
                MessageBox.Show("Status Didn't Changed");
            


        }
        private bool? ViewCreateRequestWindow(object sender)
        {
            ServiceRequestDetail serviceRequest = (sender as Button).DataContext as ServiceRequestDetail;
            if (this.requestDetail == null)
                this.requestDetail = new ViewRequestDetail(serviceRequest);
            this.requestDetail.Owner = Window.GetWindow(this);
            bool? response = this.requestDetail.ShowDialog();
            this.requestDetail = null;
            return response;
          
        }
        private async void ViewDetailsClick(object sender, RoutedEventArgs e)
        {
           bool? response = ViewCreateRequestWindow(sender);
            if (response == true)
                await GetRequestList();
            
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {

            string filter = (sender as Button).Tag.ToString();
            currentFilter = filter;
            ApplyFilter();

            Style Active = this.FindResource("FilterPillActive") as Style;
            Style NotActive = this.FindResource("FilterPill") as Style;
            PendingButton.Style = NotActive;
            AllButton.Style = NotActive;
            InProgressButton.Style = NotActive;
            CompletedButton.Style = NotActive;
            (sender as Button).Style = Active;


        }
        private void ApplyFilter()
        {
            if (serviceRequestViewModel == null) return;

            List<ServiceRequestDetail> filteredList;

            switch (currentFilter)
            {
                case "Pending":
                    filteredList = serviceRequestViewModel.serviceRequests
                        .Where(s => s.ServiceRequest.RequestStatus == RequestStatus.Pending)
                        .ToList();
                    break;

                case "Completed":
                    filteredList = serviceRequestViewModel.serviceRequests
                        .Where(s => s.ServiceRequest.RequestStatus == RequestStatus.Completed)
                        .ToList();
                    break;

                case "In Progress":
                    filteredList = serviceRequestViewModel.serviceRequests
                        .Where(s => s.ServiceRequest.RequestStatus == RequestStatus.InProgress)
                        .ToList();
                    break;

                default:
                    filteredList = serviceRequestViewModel.serviceRequests;
                    break;
            }

            listViewRequests.ItemsSource = filteredList;
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            await GetRequestList();
        }
        private void UnLoadedTimer(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();
        }
    }
}
