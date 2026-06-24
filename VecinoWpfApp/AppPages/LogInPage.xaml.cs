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

namespace VecinoWpfApp.AppPages
{
    /// <summary>
    /// Interaction logic for LogInPage.xaml
    /// </summary>
    public partial class LogInPage : Page
    {
        public LogInPage()
        {
            InitializeComponent();
        }
        private async void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            btnSignIn.IsEnabled = false;
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
                Session.HasAccount = true;
                if(apiResponse.Data.BuildingId == "0")
                {
                    Session.ResidentId = apiResponse.Data.ResidentId;
                    NavigationService.Navigate(new CreateBuilding());
                    btnSignIn.IsEnabled = true;
                    return;
                }
                  
                if (!apiResponse.Data.IsAdmin)
                {
                    MessageBox.Show("Only Admins are allowed Entrances");
                    btnSignIn.IsEnabled = true;
                    return;
                }
                Session.BuildingId = apiResponse.Data.BuildingId;
                Session.ResidentId = apiResponse.Data.ResidentId;

              
                NavigationService.Navigate(new MainWindow());
            }
            else
            {

                MessageBox.Show("Unsuccessful");
            }

            btnSignIn.IsEnabled = true;
        }

        private void CreateBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateBuilding());
        }
    }
}
