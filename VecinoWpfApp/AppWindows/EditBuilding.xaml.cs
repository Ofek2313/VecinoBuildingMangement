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
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;

namespace VecinoWpfApp.AppWindows
{
    /// <summary>
    /// Interaction logic for EditBuilding.xaml
    /// </summary>
    public partial class EditBuilding : Window
    {
        List<City> Cities;
        Building savedBuilding;
        string originalAddress;
        public EditBuilding(Building building)
        {
            InitializeComponent();
            savedBuilding = building;
            this.DataContext = building;
            originalAddress  = building.Address;
            GetCitiesList();
        }
        private async void GetCitiesList()
        {
            ApiClient<List<City>> apiClient = new ApiClient<List<City>>();
            apiClient.Scheme = "http";
            apiClient.Host = "localhost";
            apiClient.Port = 5269;
            apiClient.Path = "api/Admin/GetCities";
            Cities = await apiClient.GetAsync();
            CmbCity.ItemsSource = Cities;
            
        }

        private async void SaveChanges(object sender, RoutedEventArgs e)
        {
            BuildingUpdateDto buildingUpdateDto = new BuildingUpdateDto
            {
                BuildingId = Session.BuildingId,
                Address = savedBuilding.Address,
                CityId = savedBuilding.CityId,
                Floors = savedBuilding.Floors,
                TotalUnits = savedBuilding.TotalUnits,
                EntranceCode = savedBuilding.EntranceCode,
                EntranceName = savedBuilding.EntranceName,
                AddressChangedFlag = originalAddress != savedBuilding.Address
            };
            ApiClient<BuildingUpdateDto> apiClient = new ApiClient<BuildingUpdateDto>();
            apiClient.Scheme = "http";
            apiClient.Host = "localhost";
            apiClient.Port = 5269;
            apiClient.Path = "api/Admin/UpdateBuilding";
            ApiResponse<bool> apiResponse = await apiClient.PostAsyncReturn<BuildingUpdateDto, bool>(buildingUpdateDto);
            if (apiResponse.Success && apiResponse.Data)
            {
                this.DialogResult = true;
                this.Close();
            }
                
            
            
        }
    }
}
