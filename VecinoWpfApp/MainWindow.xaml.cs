using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VecinoWpfApp.UserControls;

namespace VecinoWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dashboard dashboard;
        Requests requests;
        public MainWindow()
        {
            InitializeComponent();
            ViewDashboard();
        }
        private void ViewDashboard()
        {
            if(this.dashboard == null)
                dashboard = new Dashboard();
            this.frameMain.Content = dashboard;
        }
        private void ViewRequests()
        {
            if (this.requests == null)
                requests = new Requests();
            this.frameMain.Content = requests;
        }

        private void HyperlinkRequests_Click(object sender, RoutedEventArgs e)
        {
            ViewRequests();
        }

        private void HyperlinkDashboard_Click(object sender, RoutedEventArgs e)
        {
            ViewDashboard();
        }
    }
}