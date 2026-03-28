using BuildingManagementWsClient;
using System;
using System.Collections.Generic;
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
        int count = 0;
        public NewPoll()
        {
            InitializeComponent();
        }

        private void AddOption_Click(object sender, RoutedEventArgs e)
        {
            
            if (count < 2)
            {
                TextBox textBox = new TextBox
                {
                    Height = 38,
                    Style = (Style)FindResource("InputField"),
                    Margin = new Thickness(0, 0, 0, 8)
                };

                int buttonIndex = optionsPanel.Children.IndexOf(addOptionButton);
                optionsPanel.Children.Insert(buttonIndex, textBox);
                ++count;
            }
            else
                MessageBox.Show("Can not add more than 4 options");

           
        }

        private async void CreatePoll_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox textBox in optionsPanel.Children.OfType<TextBox>())
            {
                
                Option option = new Option();
                option.OptionText = textBox.Text;
                option.OptionId = "";
                option.PollId = "";
                createPollViewModel.Options.Add(option);
               

               
            }
            Poll poll = new Poll();
            poll.PollTitle = pollTitle.Text;
            poll.PollDescription = pollDescription.Text;
            poll.PollDate = pollDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            poll.IsActive = true;
            poll.BuildingId = Session.BuildingId;
            poll.PollId = ""; // temp value
            createPollViewModel.Poll = poll;

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
    }
}
