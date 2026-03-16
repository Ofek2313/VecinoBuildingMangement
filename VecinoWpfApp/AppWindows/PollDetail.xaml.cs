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
using VecinoBuildingMangement.ViewModels;

namespace VecinoWpfApp.AppWindows
{
    /// <summary>
    /// Interaction logic for PollDetail.xaml
    /// </summary>
    public partial class PollDetail : Window
    {
        public PollDetail(PollViewModel pollViewModel)
        {
            InitializeComponent();
            this.DataContext = pollViewModel;

            listViewPollOptions.ItemsSource = pollViewModel.options;
        }
        
    }
}
