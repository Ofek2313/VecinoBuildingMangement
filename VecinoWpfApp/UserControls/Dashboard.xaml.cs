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
using System.Windows.Threading;
using VecinoBuildingMangement.ViewModels;
using VecinoWpfApp.AppWindows;

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        AdminMainPage adminMainPage;
        EditBuilding editBuilding;
        private DispatcherTimer _dispatcherTimer;

        public Dashboard()
        {
            InitializeComponent();
            LoadBuilding();
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
        private async Task LoadBuilding()
        {
            ApiClient<AdminMainPage> apiClient = new ApiClient<AdminMainPage>();
            apiClient.Scheme = "http";
            apiClient.Host = "localhost";
            apiClient.Port = 5269;
            apiClient.Path = "api/Admin/GetAdminMainPage";
            apiClient.AddParameter("buildingId", Session.BuildingId);
            apiClient.AddParameter("residentId", Session.ResidentId);
            adminMainPage = await apiClient.GetAsync();
            this.DataContext = adminMainPage;
           
        }
       

        private void CopyButton(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(adminMainPage.Building.JoinCode);
        }
        private async void RefreshButton(object sender, RoutedEventArgs e)
        {
            ApiClient<string> apiClient = new ApiClient<string>();
            apiClient.Scheme = "http";
            apiClient.Host = "localhost";
            apiClient.Port = 5269;
            apiClient.Path = "api/Admin/GenerateNewBuildingCode";
            apiClient.AddParameter("buildingId", Session.BuildingId);
            ApiResponse<bool> apiResponse = await apiClient.PostAsyncReturn<object, bool>(null);
            if (apiResponse.Success && apiResponse.Data)
                await LoadBuilding();
        }
        private async void OpenEditWindow(object sender, RoutedEventArgs e)
        {

            if (this.editBuilding == null)
                this.editBuilding = new EditBuilding(adminMainPage.Building);
            this.editBuilding.Owner = Window.GetWindow(this);
            bool? response = this.editBuilding.ShowDialog();
            this.editBuilding = null;
           
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            await LoadBuilding();
        }
        private void UnLoadedTimer(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();
        }

    }
}
