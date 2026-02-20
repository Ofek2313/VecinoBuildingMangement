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
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoWpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for Announcement.xaml
    /// </summary>
    public partial class Announcement : UserControl
    {
        List<Notification> annoucnemnetslist;
        public Announcement()
        {
            InitializeComponent();
            GetAnnouncementList();
        }
        private async void GetAnnouncementList()
        {

            ApiClient<List<Notification>> client = new ApiClient<List<Notification>>();
            client.Scheme = "http";
            client.Host = "localhost";
            client.Port = 5269;
            client.Path = "api/Admin/GetNotifications";
            annoucnemnetslist = await client.GetAsync();

            listViewAnnc.ItemsSource = this.annoucnemnetslist;
          
        }
    }
}
