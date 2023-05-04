using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.UserType
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserTypePage : ContentPage
    {
        UserTypeViewModel viewModel;
        public UserTypePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new UserTypeViewModel(this);
        }
    }
}