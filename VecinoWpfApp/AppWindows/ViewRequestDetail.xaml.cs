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
using System.Windows.Shapes;
using VecinoBuildingMangement.Models;

namespace VecinoWpfApp.AppWindows
{
    /// <summary>
    /// Interaction logic for ViewRequestDetail.xaml
    /// </summary>
    public partial class ViewRequestDetail : Window
    {
        public ViewRequestDetail(ServiceRequest serviceRequest)
        {
            InitializeComponent();
            this.DataContext = serviceRequest;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
    }
}
