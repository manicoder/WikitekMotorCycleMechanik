using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.RemoteRequest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RemoteRequestPage : ContentPage
    {
        RemoteRequestViewModel viewModel;
        public RemoteRequestPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new RemoteRequestViewModel(this);
        }
    }
}