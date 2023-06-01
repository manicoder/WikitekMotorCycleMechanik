using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.ExpertList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpertListPage : ContentPage
    {
        ExpertListViewModel viewModel;
        public ExpertListPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ExpertListViewModel(this);
        }
    }
}