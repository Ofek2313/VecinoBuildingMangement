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

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Requests.xaml
    /// </summary>
    public partial class Requests : UserControl
    {
        ManageServiceRequestViewModel serviceRequestViewModel;
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
    }
}
