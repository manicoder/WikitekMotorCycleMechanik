using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.MarketPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartsFilterPage : ContentPage
    {
        PartsFilterViewModel viewModel;
        public PartsFilterPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new PartsFilterViewModel(this);
        }
    }
}