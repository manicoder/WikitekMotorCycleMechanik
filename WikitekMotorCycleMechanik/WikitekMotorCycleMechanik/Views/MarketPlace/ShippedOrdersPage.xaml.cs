﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.MarketPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShippedOrdersPage : ContentPage
    {
        MyOrdersViewModel viewModel;
        public ShippedOrdersPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MyOrdersViewModel(this);
        }
    }
}