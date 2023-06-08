using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using WikitekMotorCycleMechanik.Views.Otp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.AssignTechnicianVehicle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssignTechnicians : ContentPage
    {
        AssignTechniciansViewModel viewModel;
        public AssignTechnicians()
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new AssignTechniciansViewModel(this, null);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            progressPanel.IsVisible = true;
            progress.IsRunning = true;
            await Task.Delay(100);
            await viewModel.Init();
            progressPanel.IsVisible = false;
            progress.IsRunning = false;
        }
        private void otp1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length == 1)
            {
                otp2.Focus();
            }
        }

        private void otp2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length == 1)
            {
                otp3.Focus();
            }
        }

        private void otp3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length == 1)
            {
                otp4.Focus();
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            otpPanel.IsVisible = true;
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.startDate = string.Empty;
            viewModel.startDate = e.NewDate.ToString("yyyy-MM-dd");
            viewModel.SelectedStartDate = viewModel.startDate + " " + viewModel.startTime;
        }

        private void TimePicker_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                viewModel.startTime = string.Empty;
                viewModel.startTime = (sender as TimePicker).Time.ToString();
                viewModel.SelectedStartDate = viewModel.startDate + " " + viewModel.startTime;
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            startDatePicker.Date = DateTime.Now;
            startTimePicker.Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            viewModel.SelectedStartDate = DateTime.Now.ToString("yyyy-MM-dd") + " " + startTimePicker.Time;
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePicker_DateSelected_1(object sender, DateChangedEventArgs e)
        {
            viewModel.endDate = string.Empty;
            viewModel.endDate = e.NewDate.ToString("yyyy-MM-dd");
            viewModel.SelectedEndDate = viewModel.endDate + " " + viewModel.endTime;
        }

        private void TimePicker_PropertyChanged_1(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                viewModel.endTime = string.Empty;
                viewModel.endTime = (sender as TimePicker).Time.ToString();
                viewModel.SelectedEndDate = viewModel.endDate + " " + viewModel.endTime;
            }
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            endDatePicker.Date = DateTime.Now;
            endTimePicker.Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            viewModel.SelectedEndDate = DateTime.Now.ToString("yyyy-MM-dd") + " " + endTimePicker.Time;
        }
    }
}