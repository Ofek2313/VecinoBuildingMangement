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
          
            var viewModel = (ResidentFeeViewModel)this.DataContext;
            viewModel.Fee.IsValidationEnabled = true;
            if (!_IsEditing)
            {
                _IsEditing = true;
                _savedResidentFee = viewModel.Clone();
                FeeTitleTextBox.IsReadOnly = false;
                FeeAmountTextBox.IsReadOnly = false;
                TxtDueDate.Visibility = Visibility.Collapsed;
                DueDatePicker.Visibility = Visibility.Visible;


            }
            else
            {
                if(DueDatePicker.SelectedDate.HasValue)
                    viewModel.Fee.FeeDueDate = DueDatePicker.SelectedDate?.ToString("dd/MM/yyyy");

                viewModel.Fee.Validate();
                if(!viewModel.Fee.HasErrors)
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
                        _IsEditing = true;
                       

                    }
                    else
                    {
                        FeeTitleTextBox.IsReadOnly = true;
                        FeeAmountTextBox.IsReadOnly = true;
                        _IsEditing = false;
                        this.DialogResult = true;
                        viewModel.Fee.IsValidationEnabled  = false;
                    }
                }
                else
                {
                    _IsEditing = true;
                    this.DataContext = _savedResidentFee;
                }
              
            }
            
        }

        private async void DeleteFeeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
        "Are you sure you want to permanently delete this fee?", 
        "Confirm Delete",                                       
        MessageBoxButton.YesNo, MessageBoxImage.Warning );

            if (result != MessageBoxResult.Yes) return;

            ResidentFeeViewModel viewModel = this.DataContext as ResidentFeeViewModel;
            viewModel.Fee.IsValidationEnabled = false;
            if(viewModel != null)
            {
                ApiClient<Fee> client = new ApiClient<Fee>();
                client.Host = "localhost";
                client.Port = 5269;
                client.Path = "api/Admin/RemoveFee";
                ApiResponse<bool> apiResponse = await client.PostAsyncReturn<Fee, bool>(viewModel.Fee);
                if (apiResponse.Success && apiResponse.Data)
                {
                    this.DialogResult = true;
                    this.Close();
                }
                else
                    this.DialogResult = false;

            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
