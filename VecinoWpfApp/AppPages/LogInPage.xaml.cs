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
                    NavigationService.Navigate(new CreateBuilding());
                    
                    return;
                }
                  
                if (!apiResponse.Data.IsAdmin)
                {
                    MessageBox.Show("Only Admins are allowed Entrances");
       
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


        }

        private void CreateBuildingButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateBuilding());
        }
    }
}
