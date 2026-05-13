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
        public CreateBuilding()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
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
            
                Building building = new Building();
                building.BuildingId = " ";
                building.Address = AddressTextBox.Text;
                building.CityId = "1";
                building.EntranceCode = CodeTextBox.Text;
                building.TotalUnits = Convert.ToInt32(TotalUnitsTextBox.Text);
                building.Floors = Convert.ToInt32(FloorsTextBox.Text);
                building.JoinCode = "Test";
                building.EntranceName = "A";
                building.BuildingImage = " ";

                building.Validate();

                if (!building.HasErrors)
                {

                    if(Session.HasAccount)
                    {
                        ApiClient<Building> client = new ApiClient<Building>();
                        client.Scheme = "http";
                        client.Host = "localhost";
                        client.Port = 5269;
                        client.Path = "api/Admin/CreateBuilding";

                        Stream stream = new FileStream(this.imagePath, FileMode.Open, FileAccess.Read);
                        ApiResponse<BuildingResponse> apiResponse = await client.PostAsyncReturn<Building, BuildingResponse>(building, stream, imagePath);
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

                        createBuildingRegister.Building = building;
                        Session.PendingImagePath = imagePath;
                        NavigationService.Navigate(new Register(imagePath, createBuildingRegister));
                    }
                }

        }
    }
}
