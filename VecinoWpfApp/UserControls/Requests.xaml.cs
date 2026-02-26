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
using BuildingManagementWsClient;
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
        public Requests()
        {
            InitializeComponent();
            GetRequestList();
        }

        private async Task GetRequestList()
        {
            ApiClient<ManageServiceRequestViewModel> client = new ApiClient<ManageServiceRequestViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/ManageServiceRequest";
            serviceRequestViewModel = await client.GetAsync();

            listViewRequests.ItemsSource = this.serviceRequestViewModel.serviceRequests;
            this.DataContext = this.serviceRequestViewModel;

        }

        private async void ActionRequest_Click(object sender, RoutedEventArgs e)
        {
            ServiceRequest item = (sender as Button).DataContext as ServiceRequest;
            StatusViewModel viewModel = new StatusViewModel();
            ApiClient<StatusViewModel> client = new ApiClient<StatusViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/ChangeRequestStatus";

            viewModel.RequestId = item.RequestId;

            switch (item.RequestStatus)
            {
                case "Pending":
                    viewModel.Status = "In Progress";
                    break;
                case "In Progress":
                    viewModel.Status = "Completed";
                    break;
            }

            bool response = await client.PostAsync(viewModel);

            if (response)
            {
                MessageBox.Show("Status Changed");
                GetRequestList();
            }
              
           
            else
                MessageBox.Show("Status Didn't Changed");
        }
        private bool? ViewCreateRequestWindow(object sender)
        {
            ServiceRequest serviceRequest = (sender as Button).DataContext as ServiceRequest;
            if (this.requestDetail == null)
                this.requestDetail = new ViewRequestDetail(serviceRequest);
            this.requestDetail.Owner = Window.GetWindow(this);
            bool? response = this.requestDetail.ShowDialog();
            this.requestDetail = null;
            return response;
        }
        private void ViewDetailsClick(object sender, RoutedEventArgs e)
        {
            ViewCreateRequestWindow(sender);
        }
    }
}
