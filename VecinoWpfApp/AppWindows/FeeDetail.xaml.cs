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
using System.Windows.Shapes;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoWpfApp.AppWindows
{
    /// <summary>
    /// Interaction logic for FeeDetail.xaml
    /// </summary>
    public partial class FeeDetail : Window
    {
        private bool _IsEditing = false;
        private ResidentFeeViewModel _savedResidentFee;

        public FeeDetail(ResidentFeeViewModel residentFee)
        {
            InitializeComponent();
            this.DataContext = residentFee;
            
        }

        private async void EditFeeButton_Click(object sender, RoutedEventArgs e)
        {
            _IsEditing = !_IsEditing;
            var viewModel = (ResidentFeeViewModel)this.DataContext;
            if (_IsEditing)
            {
                _savedResidentFee = viewModel.Clone();
                FeeTitleTextBox.IsReadOnly = false;
             

               
            }
            else
            {
                ApiClient<Fee> client = new ApiClient<Fee>();
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Admin/UpdateFee";
                ApiResponse<bool> apiResponse = await client.PostAsyncReturn<Fee, bool>(viewModel.Fee);
                if (!apiResponse.Success || !apiResponse.Data)
                {
                    this.DataContext = null;
                    this.DataContext = _savedResidentFee;
                }
                else
                {
                    this.DialogResult = true;
                }
            }
            
        }
    }
}
