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
using VecinoBuildingMangement.ViewModels;
using VecinoWpfApp.AppWindows;
using VecinoWpfApp;

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Polls.xaml
    /// </summary>
    public partial class Polls : UserControl
    {
        ManagePolls managePolls;
        NewPoll newPoll;
        
        public Polls()
        {
            InitializeComponent();
            GetPollsList();
        }
        private async Task GetPollsList()
        {
            ApiClient<ManagePolls> client = new ApiClient<ManagePolls>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/ManagePolls";
            client.AddParameter("buildingId", "1");
            managePolls = await client.GetAsync();
          
            listViewPolls.ItemsSource = this.managePolls.PollviewModel;
           
            this.DataContext = this.managePolls;
         
        }
        private void ViewCreatePollWindow()
        {
            if(this.newPoll == null)
                this.newPoll = new NewPoll();
            this.newPoll.Owner = Window.GetWindow(this);
            bool? response = this.newPoll.ShowDialog();
            this.newPoll = null;
        }
        private void ButtonPoll_Click(object sender, RoutedEventArgs e)
        {
            
            ViewCreatePollWindow();
        }
    }
}
