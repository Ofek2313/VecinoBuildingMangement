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

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Residents.xaml
    /// </summary>
    public partial class Residents : UserControl
    {
        ManageResidentViewModel viewModel;
        List<Resident> residentsList;
        public Residents()
        {
            InitializeComponent();
            GetResidentsList();
        }
        private async Task GetResidentsList()
        {
            ApiClient<ManageResidentViewModel> client = new ApiClient<ManageResidentViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/ManageResident";
            client.AddParameter("buildingId", "1");
            viewModel = await client.GetAsync();

            residentsList = viewModel.Residents;
            ListViewResidents.ItemsSource = residentsList;
            this.DataContext = viewModel;
            
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
    }
}
