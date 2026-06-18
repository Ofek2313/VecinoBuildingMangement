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
using VecinoBuildingMangement;
using VecinoBuildingMangement.DTO;
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
        private async void ActionRequest_Click(object sender, RoutedEventArgs e) // Change Service Request Status Based On Current Status 
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
                this.DialogResult = true;
                this.Close();
            }


            else
                MessageBox.Show("Status Didn't Changed");



        }

    }
}
