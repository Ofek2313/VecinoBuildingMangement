using BuildingManagementWsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Residents.xaml
    /// </summary>
    public partial class Residents : UserControl
    {
        ManageResidentViewModel viewModel;
        List<Resident> residentsList;
        private DispatcherTimer _dispatcherTimer;

        public Residents()
        {
            InitializeComponent();
            
            GetResidentsList();
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
        private async Task GetResidentsList()
        {
            try
            {
                ApiClient<ManageResidentViewModel> client = new ApiClient<ManageResidentViewModel>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Admin/ManageResident";
                client.AddParameter("buildingId", Session.BuildingId);
                viewModel = await client.GetAsync();

                residentsList = viewModel.Residents;
                ListViewResidents.ItemsSource = residentsList;
                this.DataContext = viewModel;
            }

            catch
            {
                MessageBox.Show("Error");
            }
            



        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = searchBox.Text.ToLower();

            List<Resident> filteredList = residentsList.Where(r => r.ResidentName.ToLower().Contains(query)).ToList();
            ListViewResidents.ItemsSource = filteredList;
        }

        private async void RemoveResident(object sender, RoutedEventArgs e)
        {
            ApiClient<bool> client = new ApiClient<bool>();

            Resident resident = (sender as Button).DataContext as Resident;
           
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/LeaveBuilding";
            client.AddParameter("residentId", resident.ResidentId);
            bool response = await client.GetAsync();
            if (response)
            {
                MessageBox.Show("Removed Resident");
                await GetResidentsList();
            }
            else
                MessageBox.Show("Failure To Remove Resident");
              
        }
        private async void ToggleAdmin(object sender, RoutedEventArgs e)
        {
            Resident resident = (sender as Button).DataContext as Resident;
            AdminToggleDto toggleDto = new AdminToggleDto();
            toggleDto.ResidentId = resident.ResidentId;
            toggleDto.AdminId = Session.ResidentId;
            ApiClient<AdminToggleDto> client = new ApiClient<AdminToggleDto>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;

            if (resident.IsAdmin)
                client.Path = "api/Admin/DemoteAdmin";
            else
                client.Path = "api/Admin/PromoteAdmin";

            ApiResponse<bool> apiResponse = await client.PostAsyncReturn<AdminToggleDto, bool>(toggleDto);
            if (apiResponse.Success && apiResponse.Data)
                await GetResidentsList();
            else
                MessageBox.Show(apiResponse.ErrorMessage);
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            await GetResidentsList();
        }
        private void UnLoadedTimer(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();
        }
    }
}
