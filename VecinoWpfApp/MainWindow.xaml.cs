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
        Announcement announcement;
        public MainWindow()
        {
            InitializeComponent();
            ViewDashboard();
            setActive(dashboardNav);
        }
        private void setActive(Border activeBorder)
        {
           
          

            ResetBorder(requestNav);
            ResetBorder(dashboardNav);
            ResetBorder(financeNav);
            ResetBorder(annNav);
            ResetBorder(eventNav);
            ResetBorder(pollNav);
            ResetBorder(resNav);

            activeBorder.Style = (Style)FindResource("navItemActive");

            
            StackPanel activeStack = activeBorder.Child as StackPanel;
            Path activePath = ((Viewbox)activeStack.Children[0]).Child as Path;
            TextBlock activeText = activeStack.Children[1] as TextBlock;
            Hyperlink hyperlink = activeText.Inlines.FirstInline as Hyperlink;
            activePath.Fill = Brushes.White;
            hyperlink.Foreground = Brushes.White;
        }
        private void ResetBorder(Border resetBorder)
        {
            resetBorder.Style = (Style)FindResource("navItem");
            StackPanel activeStack = resetBorder.Child as StackPanel;
            Path activePath = ((Viewbox)activeStack.Children[0]).Child as Path;
            TextBlock activeText = activeStack.Children[1] as TextBlock;
            Hyperlink hyperlink = activeText.Inlines.FirstInline as Hyperlink;
            if(resetBorder.Name == "annNav")
                activePath.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748b"));
            else
                activePath.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748b"));
            hyperlink.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748b"));
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
        private void ViewAnnouncements()
        {
            if(this.announcement==null)
                announcement = new Announcement();
            this.frameMain.Content = announcement;
        }
        private void HyperlinkRequests_Click(object sender, RoutedEventArgs e)
        {
            ViewRequests();
            setActive(requestNav);
        }

        private void HyperlinkDashboard_Click(object sender, RoutedEventArgs e)
        {
            ViewDashboard();
            setActive(dashboardNav);

        }

        private void HyperlinkFinance_Click(object sender, RoutedEventArgs e)
        {
            setActive(financeNav);
        }

        private void HyperLinkAnnouncments_Click(object sender, RoutedEventArgs e)
        {
            ViewAnnouncements();
            setActive(annNav);
        }

        private void HyperlinkEvent_Click(object sender, RoutedEventArgs e)
        {
            setActive(eventNav);
        }

        private void HyperlinkPoll_Click(object sender, RoutedEventArgs e)
        {
            setActive(pollNav);
        }

        private void Hyperlinkresident_Click(object sender, RoutedEventArgs e)
        {
            setActive(resNav);
        }
    }
}