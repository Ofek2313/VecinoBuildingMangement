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
using VecinoBuildingMangement;
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoWpfApp.AppWindows
{
    /// <summary>
    /// Interaction logic for NewFee.xaml
    /// </summary>
    public partial class NewFee : Window
    {
        List<Resident> Residents;
        private List<ResidentCheckItem> allResidents = new List<ResidentCheckItem>();
        CreateFee createFee = new CreateFee();
        public NewFee()
        {
            InitializeComponent();
            _ = GetResidentsList();
            this.DataContext = createFee.Fee;
        }
        private async Task GetResidentsList()
        {
            ApiClient<List<Resident>> client = new ApiClient<List<Resident>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/GetResidents";
            client.AddParameter("buildingId", Session.BuildingId);
            Residents = await client.GetAsync();
            allResidents = Residents.Select(r => new ResidentCheckItem
            {
                ResidentId = r.ResidentId,
                ResidentName = r.ResidentName,
                IsChecked=false,
            }).ToList();
            ListViewResidents.ItemsSource = allResidents;
        }
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

            string query = TxtSearch.Text.ToLower();
            ListViewResidents.ItemsSource = allResidents.Where(r => r.ResidentName.ToLower().Contains(query)).ToList();


        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           
            createFee.FeeRecipientIds = allResidents.Where(r => r.IsChecked).Select(r => r.ResidentId).ToList();
            
            if(!createFee.FeeRecipientIds.Any())
            {
                MessageBox.Show("Please select at least one","Alert",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }

            createFee.Fee.FeeId = "";
            createFee.Fee.IsPaid = false;
            createFee.Fee.ResidentId = "";
            createFee.Fee.PaymentDate = "";
            createFee.Fee.FeeDueDate = DpDueDate.SelectedDate?.ToString("DD/MM/YYYY");


            createFee.Fee.Validate();

            if (createFee.Fee.HasErrors)
            {
                MessageBox.Show(" The fee details are invalid. Please check your input. ", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
           

            ApiClient<CreateFee> client = new ApiClient<CreateFee>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/CreateFee";
            ApiResponse<bool> result = await client.PostAsyncReturn<CreateFee, bool>(createFee);
            if (result.Success && result.Data)
            {
                this.DialogResult = true;
                this.Close();
            }

            else
                MessageBox.Show("Failure");
            
            

           

           
        }
    }
}
