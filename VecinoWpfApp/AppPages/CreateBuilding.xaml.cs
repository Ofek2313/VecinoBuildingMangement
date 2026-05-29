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

namespace VecinoWpfApp.AppPages
{
    /// <summary>
    /// Interaction logic for CreateBuilding.xaml
    /// </summary>
    public partial class CreateBuilding : Page
    {
        string imagePath;
        Building buildingCreation = new Building();
        public CreateBuilding()
        {
            InitializeComponent();
            GetCitiesList();
            this.DataContext = buildingCreation;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void GetCitiesList()
        {
            ApiClient<List<City>> apiClient = new ApiClient<List<City>>();
            apiClient.Scheme = "http";
            apiClient.Host = "localhost";
            apiClient.Port = 5269;
            apiClient.Path = "api/Admin/GetCities";
            CmbCity.ItemsSource = await apiClient.GetAsync();
             

        }
        private void ImageUploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Only(*.jpg, *.jpeg, *.png, *.gif)|*.jpg; *.png; *.jpeg; *.gif;";
            bool? dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == true)
            {
                string fileName = openFileDialog.FileName;
                imagePath = fileName;
                Uri uri = new Uri(fileName);
                BitmapImage bitmapImage = new BitmapImage(uri);
                ImagePreview.Source = bitmapImage;
                ImagePlaceholder.Visibility = Visibility.Collapsed;
                ImagePreview.Visibility = Visibility.Visible;
            }
        }

        private async void CreateBuildingButton_Click(object sender, RoutedEventArgs e)
        {


            buildingCreation.BuildingImage = " ";

            buildingCreation.Validate();
            CreateBuildingDto buildingDto = new CreateBuildingDto
            {
                Building = buildingCreation,
                ResidentId = Session.ResidentId
            };

                if (!buildingCreation.HasErrors)
                {

                    if(Session.HasAccount)
                    {
                        ApiClient<CreateBuildingDto> client = new ApiClient<CreateBuildingDto>();
                        client.Scheme = "http";
                        client.Host = "localhost";
                        client.Port = 5269;
                        client.Path = "api/Admin/CreateBuilding";

                        Stream stream = new FileStream(this.imagePath, FileMode.Open, FileAccess.Read);
                        ApiResponse<BuildingResponse> apiResponse = await client.PostAsyncReturn<CreateBuildingDto, BuildingResponse>(buildingDto, stream, imagePath);
                        if (apiResponse.Data != null && apiResponse.Success)
                        {
                            MessageBox.Show("Success");
                            Session.BuildingId = apiResponse.Data.BuildingId;
                            NavigationService.Navigate(new MainWindow());
                        }
                    }
                    else
                    {
                        CreateBuildingRegister createBuildingRegister = new CreateBuildingRegister();

                        createBuildingRegister.Building = buildingCreation;
                        Session.PendingImagePath = imagePath;
                        NavigationService.Navigate(new Register(imagePath, createBuildingRegister));
                    }
                }

        }
    }
}
