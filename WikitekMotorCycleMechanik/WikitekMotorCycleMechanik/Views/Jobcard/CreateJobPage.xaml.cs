using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.Jobcard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateJobPage : ContentPage
    {
        ViewModels.CreateJobcardViewModel viewModel;
        public CreateJobPage()
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new ViewModels.CreateJobcardViewModel(this);
            }
            catch (Exception ex)
            {
            }
        }
    }
}