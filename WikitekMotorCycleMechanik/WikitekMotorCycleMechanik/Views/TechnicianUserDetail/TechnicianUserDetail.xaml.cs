using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.TechnicianUserDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TechnicianUserDetail : ContentPage
    {
        TechnicianUserDetailViewModel viewModel;
        public TechnicianUserDetail()
        {
            InitializeComponent();
            BindingContext = viewModel = new TechnicianUserDetailViewModel(this, null);
        }
    }
}