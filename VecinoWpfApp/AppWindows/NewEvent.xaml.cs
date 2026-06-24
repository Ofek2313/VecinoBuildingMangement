using BuildingManagementWsClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using VecinoBuildingMangement.DTO;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoWpfApp.AppWindows
{
    /// <summary>
    /// Interaction logic for NewEvent.xaml
    /// </summary>
    public partial class NewEvent : Window
    {

        CreateEvent createEventView;
        
        string imagePath;
        public NewEvent()
        {
            InitializeComponent();
            GetEventTypes();
            var timeslots = GenerateTimeSlots();
            StartTimeComboBox.ItemsSource = timeslots;
            EndTimeComboBox.ItemsSource = timeslots;
        }
        private async void GetEventTypes()
        {
            ApiClient<CreateEvent> client = new ApiClient<CreateEvent> ();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/GetEventTypes";
            createEventView = await client.GetAsync();
    
            this.createEventView.Event = new Event();
            createEventView.Event.IsValidationEnabled = true;
            TypeComboBox.ItemsSource = createEventView.eventTypes;
            this.DataContext = createEventView;

        }

        private void ButtonSelectImage_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Only(*.jpg, *.jpeg, *.png, *.gif)|*.jpg; *.png; *.jpeg; *.gif;";
            bool? dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == true)
            {
                string fileName = openFileDialog.FileName;
                imagePath = fileName;
                Uri uri = new Uri(fileName);
                BitmapImage bitmapImage = new BitmapImage(uri);

                EventImagePreview.Source = bitmapImage;
                EventImagePreview.Visibility = Visibility.Visible;
            }

        }
        private void RemoveImage(object sender, RoutedEventArgs e)
        {
            EventImagePreview.Visibility = Visibility.Collapsed;
            imagePath = null;
        }
        private List<string> GenerateTimeSlots()
        {
            List<String> timeslots = new List<String>();
            for (int i = 0; i < 24; i++)
            {
                timeslots.Add((i.ToString()) + ":00");
            }
            return timeslots;
        }

        private async void ButtonAddEvent_Click(object sender, RoutedEventArgs e)
        {
           
            ApiResponse<bool> response = new ApiResponse<bool> { Data = false, Success = false };

            createEventView.Event.BuildingId = Session.BuildingId;
            createEventView.Event.EventImage = System.IO.Path.GetExtension(this.imagePath);

            createEventView.Event.Validate();
            if(!createEventView.Event.HasErrors)
            {
                ApiClient<Event> client = new ApiClient<Event>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Admin/AddUpComingEvent";
                if(imagePath != null)
                {
                    Stream stream = new FileStream(this.imagePath, FileMode.Open, FileAccess.Read);

                    response = await client.PostAsyncReturn<Event, bool>(createEventView.Event, stream, imagePath);
                }
                else
                {
                    response = await client.PostAsyncReturn<Event, bool>(createEventView.Event,null,null); //still using the same function to not replicate same function
                }
            }
            else
            {
                
                 MessageBox.Show(" The event details are invalid. Please check your input. ", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                 return;
                
            }
            if (response.Data && response.Success)
            {
                MessageBox.Show("Event Added Successfully");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(response.ErrorMessage);
            }

        }
    }
}
