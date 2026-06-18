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
using System.Windows.Threading;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoWpfApp.AppWindows;

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Events.xaml
    /// </summary>
    public partial class Events : UserControl
    {
        ManageEventViewModel eventViewModel;
        NewEvent newEvent;
        EventDetails eventDetails;
        private DispatcherTimer _dispatcherTimer;

        public Events()
        {
            InitializeComponent();
            GetEventsList();
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
        private async void Timer_Tick(object sender, EventArgs e)
        {
            await GetEventsList();
        }
        private void UnLoadedTimer(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();
        }
        private async Task GetEventsList()
        {
            ApiClient<ManageEventViewModel> client = new ApiClient<ManageEventViewModel>(); //Uses Apiclient to contact the api that is hosted on our localhost
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/ManageEvent";
            client.AddParameter("buildingId", Session.BuildingId);
            eventViewModel = await client.GetAsync();

            listViewEvents.ItemsSource = null;

            listViewEvents.ItemsSource = this.eventViewModel.Events;
            listViewPastEvents.ItemsSource = this.eventViewModel.PastEvents;
            this.DataContext = this.eventViewModel.Events;

        }
        private bool? ViewCreateEventWindow()
        {
            if (this.newEvent == null)
                this.newEvent = new NewEvent();
            this.newEvent.Owner = Window.GetWindow(this);
            bool? response = this.newEvent.ShowDialog();
            this.newEvent = null;
            return response;
        }

        private async void ButtonAddEvent_Click(object sender, RoutedEventArgs e)
        {
            bool? response = ViewCreateEventWindow();
         
            if (response == true)
                await GetEventsList();

        }
        private bool? ViewEventDetailWindow(EventViewModel model)
        {
            if (this.eventDetails == null)
                this.eventDetails = new EventDetails(model);
            this.eventDetails.Owner = Window.GetWindow(this);
            bool? response = this.eventDetails.ShowDialog();
            this.eventDetails = null;
            return response;
        }
        private async void ViewEventDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            EventViewModel model = (sender as Button).DataContext as EventViewModel;
            bool? response = ViewEventDetailWindow(model.Clone());

            if (response == true)
                await GetEventsList();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
      "Are you sure you want to permanently delete this Event?",
      "Confirm Delete",
      MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;
            EventViewModel model = (sender as Button).DataContext as EventViewModel;
            string eventId = model.Event.EventId;
            //RemoveEvent

            ApiClient<bool> client = new ApiClient<bool>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/RemoveEvent";
            client.AddParameter("eventId", eventId);
            ApiResponse<bool> apiResponse = await client.PostAsyncReturn<object, bool>(null);
            if (apiResponse.Success && apiResponse.Data)
            {
                MessageBox.Show("Deleted");
                await GetEventsList();
            }
              
        }
        
    }
}
