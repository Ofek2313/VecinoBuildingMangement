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
        private async Task GetEventTypes()
        {
            ApiClient<CreateEvent> client = new ApiClient<CreateEvent> ();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/GetEventTypes";
            createEventView = await client.GetAsync();
            this.createEventView.Event = new Event();
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
                
                //ImageEvent.Source = bitmapImage;
            }

        }
     

        private async void ButtonAddEvent_Click(object sender, RoutedEventArgs e)
        {
            Event Newevent = new Event();
            bool response = false;
            Newevent.EventTitle = TitleInput.Text;
            Newevent.EventDescription = DescriptionInput.Text;
            Newevent.EventDate = EventDatePicker.SelectedDate.Value.ToString("dd/MM/yyyy");
            Newevent.EventImage = System.IO.Path.GetExtension(this.imagePath);
            Newevent.EventTypeId = TypeComboBox.SelectedValue.ToString();
            Newevent.BuildingId = "1";

            Newevent.Validate();
            if(!Newevent.HasErrors)
            {
                ApiClient<Event> client = new ApiClient<Event>();
                client.Scheme = "http";
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Admin/AddUpComingEvent";
                Stream stream = new FileStream(this.imagePath,FileMode.Open,FileAccess.Read);
                
                response = await client.PostAsync(Newevent,stream);
            }
            if(response)
            {
                MessageBox.Show("Event Added Successfully");
                this.DialogResult = true;
                this.Close();
            }
            

        }
    }
}
