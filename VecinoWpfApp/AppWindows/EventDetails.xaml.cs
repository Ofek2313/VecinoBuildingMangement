using BuildingManagementWsClient;
using Microsoft.Win32;
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
    /// Interaction logic for EventDetails.xaml
    /// </summary>
    public partial class EventDetails : Window
    {
        private bool _IsEditing = false;
       
        private string _eventTitle;
        private string _eventDescription;
       

        public EventDetails(EventViewModel @event)
        {
            InitializeComponent();
            LoadAttending(@event.Event.EventId);
            this.DataContext = @event;

        }
    
        private async void ButtonEditEvent_Click(object sender, RoutedEventArgs e)
        {
            _IsEditing = !_IsEditing;
            if(_IsEditing)
            {
                EventDescriptionText.IsReadOnly = false;
                EventTitleText.IsReadOnly=false;
                UploadImage.Visibility = Visibility.Visible;
                MainScrollbar.ScrollToHome();
                MainButton.Text = "Update";


            }
            else
            {
                EventDescriptionText.IsReadOnly = true;
                EventTitleText.IsReadOnly = true;
                UploadImage.Visibility = Visibility.Collapsed;
                MainButton.Text = "Edit Event";
                var viewModel = (EventViewModel)this.DataContext;

                ApiClient<Event> client = new ApiClient<Event>();
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Admin/UpdateEvent";
                ApiResponse<bool> apiResponse = await client.PostAsyncReturn<Event, bool>(viewModel.Event);
                MessageBox.Show(viewModel.Event.EventDescription);
                //UpdateEvent
            }
        }
     
        private async void LoadAttending(string eventId)
        {
           
            ApiClient<List<string>> client = new ApiClient<List<string>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/GetResidentsAttendingEvent";
            client.AddParameter("eventId", eventId);
            AttendeesListControl.ItemsSource = await client.GetAsync();

        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Only(*.jpg, *.jpeg, *.png, *.gif)|*.jpg; *.png; *.jpeg; *.gif;";
            bool? dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == true)
            {
                string fileName = openFileDialog.FileName;
                //imagePath = fileName;
                //Uri uri = new Uri(fileName);
                //BitmapImage bitmapImage = new BitmapImage(uri);

                //ImageEvent.Source = bitmapImage;
            }

        }
    }
}
