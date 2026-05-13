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
using VecinoBuildingMangement.ViewModels;

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadBuilding();
        }
        private async void LoadBuilding()
        {
            ApiClient<AdminMainPage> apiClient = new ApiClient<AdminMainPage>();
            apiClient.Scheme = "http";
            apiClient.Host = "localhost";
            apiClient.Port = 5269;
            apiClient.Path = "api/Admin/GetAdminMainPage";
            apiClient.AddParameter("buildingId", Session.BuildingId);
            apiClient.AddParameter("residentId", Session.ResidentId);
            this.DataContext = await apiClient.GetAsync();
           
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
