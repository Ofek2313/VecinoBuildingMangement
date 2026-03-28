using BuildingManagementWsClient;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        private readonly string _imagepath;
        private readonly CreateBuildingRegister _createBuildingRegister;

        public Register(string imagepath,CreateBuildingRegister createBuildingRegister)
        {
            InitializeComponent();
            _imagepath = imagepath;
            _createBuildingRegister = createBuildingRegister;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Resident resident = new Resident();
            resident.ResidentName = NameTextBox.Text;
            resident.ResidentPassword = PasswordTextBox.Password;
            resident.ResidentPhone = PhoneTextBox.Text;
            resident.ResidentEmail = EmailTextBox.Text;
            resident.UnitNumber = 0;
            resident.IsAdmin = true;

            _createBuildingRegister.Resident = resident;
            ApiClient<CreateBuildingRegister> client = new ApiClient<CreateBuildingRegister>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/CreateBuildingAndRegister";

            Stream stream = new FileStream(_imagepath, FileMode.Open, FileAccess.Read);
            ApiResponse<BuildingResponse> apiResponse = await client.PostAsyncReturn<CreateBuildingRegister, BuildingResponse>(_createBuildingRegister, stream, _imagepath);

            if (apiResponse.Data != null && apiResponse.Success)
            {
                Session.BuildingId = apiResponse.Data.BuildingId;
                NavigationService.Navigate(new MainWindow());
            }

        }
    }
}
