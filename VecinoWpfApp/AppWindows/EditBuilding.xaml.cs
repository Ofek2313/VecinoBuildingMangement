using BuildingManagementWsClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string _imagepath;
        bool _photoremoved;
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
        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Only(*.jpg, *.jpeg, *.png, *.gif)|*.jpg; *.png; *.jpeg; *.gif;";
            bool? dialogResult = openFileDialog.ShowDialog();
            _photoremoved = false;
            if (dialogResult == true)
            {
                string fileName = openFileDialog.FileName;
                _imagepath = fileName;
                Uri uri = new Uri(fileName);
                BitmapImage bitmapImage = new BitmapImage(uri);
                BuildingImagePreview.Source = bitmapImage;
                BuildingImagePreview.Visibility = Visibility.Visible;


            }

        }
        private void RemoveImage(object sender, RoutedEventArgs e)
        {
            _photoremoved = true;
            BuildingImagePreview.Visibility = Visibility.Collapsed;
            _imagepath = null;
        }

        private async void SaveChanges(object sender, RoutedEventArgs e)
        {
            BuildingUpdateDto buildingUpdateDto = new BuildingUpdateDto
            {
                BuildingId = Session.BuildingId,
                BuildingImage = savedBuilding.BuildingImage,
                Address = savedBuilding.Address,
                CityId = savedBuilding.CityId,
                Floors = savedBuilding.Floors,
                TotalUnits = savedBuilding.TotalUnits,
                EntranceCode = savedBuilding.EntranceCode,
                EntranceName = savedBuilding.EntranceName,
                AddressChangedFlag = originalAddress != savedBuilding.Address,
                PhotoRemoved = _photoremoved

            };
            ApiClient<BuildingUpdateDto> apiClient = new ApiClient<BuildingUpdateDto>();
            ApiResponse<bool> apiResponse;
            apiClient.Scheme = "http";
            apiClient.Host = "localhost";
            apiClient.Port = 5269;
            apiClient.Path = "api/Admin/UpdateBuilding";
            if(_imagepath == null)
            {
               apiResponse = await apiClient.PostAsyncReturn<BuildingUpdateDto, bool>(buildingUpdateDto,null,null);
            }
            else
            {
                Stream stream = new FileStream(_imagepath, FileMode.Open, FileAccess.Read);
                apiResponse = await apiClient.PostAsyncReturn<BuildingUpdateDto, bool>(buildingUpdateDto, stream, _imagepath);
            }

            if (apiResponse.Success && apiResponse.Data)
            {
                this.DialogResult = true;
                this.Close();
            }
                
            
            
        }
        private void CloseButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
