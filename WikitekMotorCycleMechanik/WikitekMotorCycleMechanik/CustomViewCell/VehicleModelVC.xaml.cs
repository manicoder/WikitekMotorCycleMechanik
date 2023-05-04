using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.CustomViewCell
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleModelVC : Grid
    {
        public VehicleModelVC()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnBindingContextChanged()
        {
            try
            {
                var item = BindingContext as VehicleModelResult;

                if (item == null)
                    return;

                txt_model_image.Source = item.model_file;
                txt_model_name.Text = item.name;
                base.OnBindingContextChanged();
            }
            catch (Exception ex)
            {
            }
        }
    }
}