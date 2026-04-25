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

namespace VecinoWpfApp.AppWindows
{
    /// <summary>
    /// Interaction logic for ViewRequestDetail.xaml
    /// </summary>
    public partial class ViewRequestDetail : Window
    {
        public ViewRequestDetail(ServiceRequestDetail serviceRequest)
        {
            InitializeComponent();
            this.DataContext = serviceRequest;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private async void ActionRequest_Click(object sender, RoutedEventArgs e)
        {
            ServiceRequestDetail item = (sender as Button).DataContext as ServiceRequestDetail;
            StatusViewModel viewModel = new StatusViewModel();

            ApiClient<StatusViewModel> client = new ApiClient<StatusViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/ChangeRequestStatus";

            viewModel.RequestId = item.ServiceRequest.RequestId;

            switch (item.ServiceRequest.RequestStatus)
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
                this.DialogResult = true;
                this.Close();
            }


            else
                MessageBox.Show("Status Didn't Changed");



        }

    }
}
