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

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Bookings.xaml
    /// </summary>
    public partial class Bookings : UserControl
    {
        List<Booking> BookingsList;
        ManageBookingsViewModel manageBookingsViewModel;
        private DispatcherTimer _dispatcherTimer;
        public Bookings()
        {
            InitializeComponent();
            LoadBookings();
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
        private async Task LoadBookings()
        {
            ApiClient<ManageBookingsViewModel> apiClient = new ApiClient<ManageBookingsViewModel> ();
            apiClient.Path = "api/Admin/GetAllBookings";
            apiClient.AddParameter("buildingId", Session.BuildingId);
            manageBookingsViewModel = await apiClient.GetAsync();
            this.DataContext = manageBookingsViewModel;
            listViewPastBookings.ItemsSource = manageBookingsViewModel.PastBookings;
            listViewUpcomingBookings.ItemsSource = manageBookingsViewModel.UpComingBookings;


        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            await LoadBookings();
        }
        private void UnLoadedTimer(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();
        }
        private async void ApproveBooking(object sender, RoutedEventArgs e)
        {
            BookingResidentViewModel booking = (sender as Button).DataContext as BookingResidentViewModel;
            ApiClient<bool> apiClient = new ApiClient<bool>();
            apiClient.Path = "api/Admin/ApproveBooking";
            apiClient.AddParameter("bookingId", booking.Booking.BookingId);
            ApiResponse<bool> apiResponse = await apiClient.PostAsyncReturn<object, bool>(null);
            if (apiResponse.Success && apiResponse.Data)
                await LoadBookings();
            else
            {
                MessageBox.Show(apiResponse.ErrorMessage, "Booking Overlap", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private async void RejectBooking(object sender, RoutedEventArgs e)
        {
            BookingResidentViewModel booking = (sender as Button).DataContext as BookingResidentViewModel;
            ApiClient<bool> apiClient = new ApiClient<bool>();
            apiClient.Path = "api/Admin/RejectBooking";
            apiClient.AddParameter("bookingId", booking.Booking.BookingId);
            ApiResponse<bool> apiResponse = await apiClient.PostAsyncReturn<object, bool>(null);
            if (apiResponse.Success && apiResponse.Data)
                await LoadBookings();
            else
            {
                MessageBox.Show( "Unable To Reject","Booking Overlap", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        
    }
}
