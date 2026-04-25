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
using VecinoWpfApp;
using VecinoWpfApp.AppWindows;

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Polls.xaml
    /// </summary>
    public partial class Polls : UserControl
    {
        ManagePolls managePolls;
        NewPoll newPoll;
        PollDetail pollDetail;
        
        public Polls()
        {
            InitializeComponent();
            _ = GetPollsList();
        }
        private async Task GetPollsList()
        {
            ApiClient<ManagePolls> client = new ApiClient<ManagePolls>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/ManagePolls";
            client.AddParameter("buildingId", Session.BuildingId);
            managePolls = await client.GetAsync();
          
            listViewPolls.ItemsSource = this.managePolls.PollviewModel;
           
            this.DataContext = this.managePolls;
         
        }
        private bool? ViewCreatePollWindow()
        {
            if(this.newPoll == null)
                this.newPoll = new NewPoll();
            this.newPoll.Owner = Window.GetWindow(this);
            bool? response = this.newPoll.ShowDialog();
            this.newPoll = null;
            return response;
        }
        private  async void ButtonPoll_Click(object sender, RoutedEventArgs e)
        {
            bool? response = ViewCreatePollWindow();
            if (response == true)
                await GetPollsList();
        }


        private bool? ViewPollDetailWindow(object sender)
        {
            PollViewModel pollViewModel = (sender as Button).DataContext as PollViewModel;
            if (this.pollDetail == null)
                this.pollDetail = new PollDetail(pollViewModel);
            this.pollDetail.Owner = Window.GetWindow(this);
            bool? response = this.pollDetail.ShowDialog();
            this.pollDetail = null;
            return response;

        }

        private void ViewPollDetail_Click(object sender, RoutedEventArgs e)
        {
            bool? response = ViewPollDetailWindow(sender);
        }

      

        private async void ClosePollButton_Click(object sender, RoutedEventArgs e)
        {
            PollViewModel pollViewModel = (sender as Button).DataContext as PollViewModel;
            ApiClient<Poll> client = new ApiClient<Poll>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/ClosePoll";
            ApiResponse<bool> apiResponse = await client.PostAsyncReturn<Poll, bool>(pollViewModel.poll);
            if (apiResponse.Success && apiResponse.Data)
                await GetPollsList();
            else
                MessageBox.Show("Unable To Close","Error",MessageBoxButton.OK,MessageBoxImage.Error);



        }
    }
}
