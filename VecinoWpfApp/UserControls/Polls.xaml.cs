using BuildingManagementWsClient;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Threading;
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
        List<PollViewModelAdmin> polls;
        private DispatcherTimer _dispatcherTimer;
        private string currentFilter;
        public Polls()
        {
            InitializeComponent();
            
            _ = GetPollsList();
            InitializetTimer();
            this.Unloaded += UnLoadedTimer;
        }
        private void InitializetTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(30);
            _dispatcherTimer.Tick += Timer_Tick;
            _dispatcherTimer.Start();
        }
        private async Task GetPollsList()
        {
            ApiClient<List<PollViewModelAdmin>> client = new ApiClient<List<PollViewModelAdmin>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/ManagePolls";
            client.AddParameter("buildingId", Session.BuildingId);
            polls = await client.GetAsync();
          
            listViewPolls.ItemsSource = polls;
           
     
         
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
        private async void OpenPollButton_Click(object sender, RoutedEventArgs e)
        {
            PollViewModel pollViewModel = (sender as Button).DataContext as PollViewModel;
            ApiClient<Poll> client = new ApiClient<Poll>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/OpenPoll";
            ApiResponse<bool> apiResponse = await client.PostAsyncReturn<Poll, bool>(pollViewModel.poll);
            if (apiResponse.Success && apiResponse.Data)
                await GetPollsList();
            else
                MessageBox.Show("Unable To Open", "Error", MessageBoxButton.OK, MessageBoxImage.Error);



        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {

            string filter = (sender as Button).Tag.ToString();
            currentFilter = filter;
            ApplyFilter();
          

            Style Active = this.FindResource("FilterButtonActive") as Style;
            Style NotActive = this.FindResource("FilterButton") as Style;
            FilterAllButton.Style = NotActive;
            FilterOpenButton.Style = NotActive;
            FilterClosedButton.Style = NotActive;
     
            (sender as Button).Style = Active;


        }
        private void ApplyFilter()
        {
            if (polls == null) return;

            List<PollViewModelAdmin> filteredList;

            switch (currentFilter)
            {
                case "open":
                    filteredList = polls.Where(p => p.poll.IsActive && DateTime.ParseExact(p.poll.PollDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.Today).ToList();
                    break;

                case "close":
                    filteredList = polls.Where(p => !p.poll.IsActive || DateTime.ParseExact(p.poll.PollDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.Today).ToList();
                    break;


                default:
                    filteredList = polls;
                    break;
            }

            listViewPolls.ItemsSource = filteredList;
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            await GetPollsList();
        }
        private void UnLoadedTimer(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();
        }
    }
}
