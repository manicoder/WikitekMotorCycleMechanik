using WikitekMotorCycleMechanik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.Otp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MobileNumberPage : ContentPage
	{
		MobileNumberViewModel viewModel;
		public MobileNumberPage ()
		{
			InitializeComponent ();
			BindingContext = viewModel = new MobileNumberViewModel(this);
		}
	}
}