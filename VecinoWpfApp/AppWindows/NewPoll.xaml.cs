using BuildingManagementWsClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
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
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoWpfApp.AppWindows
{
    /// <summary>
    /// Interaction logic for NewPoll.xaml
    /// </summary>
    public partial class NewPoll : Window
    {
        CreatePollViewModel createPollViewModel = new CreatePollViewModel();
        ObservableCollection<Option> options = new ObservableCollection<Option>();
        public NewPoll()
        {
            InitializeComponent();
            this.DataContext = createPollViewModel;
            LoadOption();
        }

        private void AddOption_Click(object sender, RoutedEventArgs e)
        {
            
            if (options.Count < 4)
            {
                options.Add(
               new Option
               {
                   OptionId = "",
                   OptionText = "",
                   PollId = ""
               });
            }
            else
                MessageBox.Show("Can not add more than 4 options");

           
        }

        private async void CreatePoll_Click(object sender, RoutedEventArgs e)
        {
            
          

            Poll poll = new Poll();
            poll.PollTitle = pollTitleTextBox.Text;
            poll.PollDescription = pollDescription.Text;
            poll.PollDate = pollDate.SelectedDate?.ToString("dd/MM/yyyy");
            poll.IsActive = true;
            poll.BuildingId = Session.BuildingId;
            poll.PollId = ""; // temp value

            
            
           

            createPollViewModel.Poll = poll;
            createPollViewModel.Options = options.ToList();


            createPollViewModel.Poll.Validate();
            createPollViewModel.Options.ForEach(o => o.Validate());

            if(createPollViewModel.Options.Any(o=>o.HasErrors) || createPollViewModel.Poll.HasErrors)
            {
                MessageBox.Show(" The poll details are invalid. Please check your input. ", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }    

            ApiClient<CreatePollViewModel> apiClient = new ApiClient<CreatePollViewModel>();
            apiClient.Scheme = "http";
            apiClient.Host = "localhost";
            apiClient.Port = 5269;
            apiClient.Path = "api/Admin/CreatePoll";
            ApiResponse<bool> response = await apiClient.PostAsyncReturn<CreatePollViewModel,bool>(createPollViewModel);

            if(response.Success && response.Data)
            {
                this.DialogResult = true;
                this.Close();
            }

        }
        private void LoadOption()
        {

            this.ItemControlOptions.ItemsSource = options;
            options.Add(
                new Option
                {
                    OptionId = "",
                    OptionText = "",
                    PollId = ""
            });
            options.Add(
               new Option
               {
                   OptionId = "",
                   OptionText = "",
                   PollId = ""
            });
        }
    }
}
