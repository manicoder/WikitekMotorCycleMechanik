using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikitekMotorCycleMechanik.Droid.Renders;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(MasterDetailView), typeof(CustomFlyoutPageRenderer))]
namespace WikitekMotorCycleMechanik.Droid.Renders
{
    class CustomFlyoutPageRenderer : MasterDetailPageRenderer
    {
        public CustomFlyoutPageRenderer(Context context) : base(context)
        {
        }

        bool firstDone = false;
        public override void AddView(Android.Views.View child)
        {
            if (firstDone)
            {
                LayoutParams p = (LayoutParams)child.LayoutParameters;
                p.Width = Convert.ToInt32(DeviceDisplay.MainDisplayInfo.Width * .63);
                base.AddView(child, p);
            }
            else
            {
                firstDone = true;
                base.AddView(child);
            }
        }
    }
}