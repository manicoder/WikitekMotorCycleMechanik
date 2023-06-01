using System;
using System.Collections.Generic;
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
    public partial class MyOrderDetailPage : ContentPage
    {
        MyOrderDetailViewModel viewModel;
        public MyOrderDetailPage(MyOrderListModel selectedOrder)
        {
            InitializeComponent();
            BindingContext = viewModel = new MyOrderDetailViewModel(selectedOrder, this);
        }
    }
}