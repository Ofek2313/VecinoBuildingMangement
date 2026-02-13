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

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Events.xaml
    /// </summary>
    public partial class Events : UserControl
    {
        ManageEventViewModel eventViewModel;
        public Events()
        {
            InitializeComponent();
            GetEventsList();
        }
        private async Task GetEventsList()
        {
            ApiClient<ManageEventViewModel> client = new ApiClient<ManageEventViewModel>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/ManageEvent";
            eventViewModel = await client.GetAsync();

            listViewEvents.ItemsSource = this.eventViewModel.Events;
            this.DataContext = this.eventViewModel.Events;

        }
    }
}
