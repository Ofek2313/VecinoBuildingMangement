using BuildingManagementWsClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using VecinoWpfApp.UserControls;

namespace VecinoWpfApp.AppWindows
{
    /// <summary>
    /// Interaction logic for EventDetails.xaml
    /// </summary>
    public partial class EventDetails : Window
    {
        private bool _IsEditing = false;
        private EventViewModel _savedEventView;
        private string _eventTitle;
        private string _eventDescription;
        private string _imagePath;
       

        public EventDetails(EventViewModel @event)
        {
            InitializeComponent();

    
            LoadAttending(@event.Event.EventId);
  
            this.DataContext = @event;
            

        }
        //private void BackUpEvent(EventViewModel @event)
        //{
        //    _savedEventView = new EventViewModel
        //    {
        //        Event = new Event{
        //              EventTitle = @event.Event.EventTitle,
        //              EventDescription = @event.Event.EventDescription,
        //              EventDate = @event.Event.EventDate,
        //              EventImage = @event.Event.EventImage,
        //              StartTime = @event.Event.StartTime,
        //              EndTime = @event.Event.EndTime,
        //        },
        //       Attending = @event.Attending,

        //    };
        //}
        private async void ButtonEditEvent_Click(object sender, RoutedEventArgs e)
        {
            _IsEditing = !_IsEditing;

            var viewModel = (EventViewModel)this.DataContext;
            if (_IsEditing)
            {
                _savedEventView = viewModel.Clone();
                
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

                ApiResponse<bool> apiResponse;

                ApiClient<Event> client = new ApiClient<Event>();
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Admin/UpdateEvent";

                if(_imagePath != null)
                {
                    Stream stream = new FileStream(_imagePath, FileMode.Open, FileAccess.Read);
                    apiResponse = await client.PostAsyncReturn<Event, bool>(viewModel.Event, stream, _imagePath);
                }
                else
                {
                    apiResponse = await client.PostAsyncReturn<Event, bool>(viewModel.Event,null,null);
                }

                if (!apiResponse.Success || !apiResponse.Data)
                {
                    this.DataContext = null;
                    this.DataContext = _savedEventView;
                }
                else
                {
                    this.DialogResult = true;
                }
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
                _imagePath = fileName;
                Uri uri = new Uri(fileName);
                BitmapImage bitmapImage = new BitmapImage(uri);
                EventImage.Source = bitmapImage;
                
            }

        }

        private void CloseEvenDetailButton_Click(object sender, RoutedEventArgs e)
        {
      

            this.Close();

        }
    }
}
