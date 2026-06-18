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
using System.IO;
using System.Windows.Shapes;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;
using System.Runtime.CompilerServices;

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
                    response = await client.PostAsyncReturn<Event, bool>(createEventView.Event, null, null);
                }
            }
            if(response.Data && response.Success)
            {
                MessageBox.Show("Event Added Successfully");
                this.DialogResult = true;
                this.Close();
            }
            

        }
    }
}
