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
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoWpfApp.AppWindows
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Window
    {
        public static string BuildingId;
        public StartPage()
        {
            InitializeComponent();
        }

        private async void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            LogInViewModel logInViewModel = new LogInViewModel();

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
                return;
            if (string.IsNullOrWhiteSpace(txtPassword.Password))
                return;


            logInViewModel.Email = txtEmail.Text;
            logInViewModel.Password = txtPassword.Password;
            
            
            ApiClient<LogInViewModel> client = new ApiClient<LogInViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Resident/Login";
            ApiResponse<Resident> apiResponse = await client.PostAsyncReturn<LogInViewModel, Resident>(logInViewModel);
         
            if (apiResponse.Success && apiResponse.Data != null)
            {
                if(!apiResponse.Data.IsAdmin)
                {
                    MessageBox.Show("Only Admins are allowed Entrances");
                    return;
                }
                Application.Current.Properties.Add("buildingId", apiResponse.Data.BuildingId);
                Application.Current.Properties.Add("residentName", apiResponse.Data.ResidentName);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Unsuccessful");
            }
           

        }
    }
}
