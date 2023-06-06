using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MiBud.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(ChangeStatusbar))]
namespace MiBud.Droid
{
    public class ChangeStatusbar : IStatusBarPlatformSpecific
    {
        public ChangeStatusbar()
        {
        }

        public void SetStatusBarColor(Xamarin.Forms.Color color)
        {
            // The SetStatusBarcolor is new since API 21
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var androidColor = color.AddLuminosity(-0.1).ToAndroid();
                //Just use the plugin
                Xamarin.Essentials.Platform.CurrentActivity.Window.SetStatusBarColor(androidColor);
            }
            else
            {
                // Here you will just have to set your 
                // color in styles.xml file as shown below.
            }
        }
    }
}