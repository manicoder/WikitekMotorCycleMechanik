using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Droid.Dependencies;
using WikitekMotorCycleMechanik.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(ToolbarItemBadgeService))]
namespace WikitekMotorCycleMechanik.Droid.Dependencies
{
    public class ToolbarItemBadgeService : IToolbarItemBadgeService
    {
        public void SetBadge(Page page, ToolbarItem item, string value, Color backgroundColor, Color textColor)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                var toolbar = CrossCurrentActivity.Current.Activity.FindViewById(Resource.Id.toolbar) as AndroidX.AppCompat.Widget.Toolbar;
                if (toolbar != null)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        var idx = page.ToolbarItems.IndexOf(item);
                        var size = toolbar.Menu.Size();
                        if (size > idx)
                        {
                            var menuItem = toolbar.Menu.GetItem(idx);
                            BadgeDrawable.SetBadgeText(CrossCurrentActivity.Current.Activity, menuItem, value, backgroundColor.ToAndroid(), textColor.ToAndroid());
                        }
                    }
                    //toolbar.Menu.Close();
                }
                
            });

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    var toolbar = CrossCurrentActivity.Current.Activity.FindViewById(Resource.Id.toolbar) as Android.Support.V7.Widget.Toolbar;

            //    //    var mainActivity = MainActivity.Instance;
            //    //if (mainActivity == null)
            //    //    return;


            //    //if (mainActivity.toolbar != null)
            //    //{
            //    try
            //    {
            //        if (!string.IsNullOrEmpty(value))
            //        {
            //            var idx = page.ToolbarItems.IndexOf(item);
            //            var size = toolbar.Menu.Size();
            //            if (size > idx)
            //            {
            //                var menuItem = toolbar.Menu.GetItem(idx);
            //                BadgeDrawable.SetBadgeText(CrossCurrentActivity.Current.Activity, menuItem, value, backgroundColor.ToAndroid(), textColor.ToAndroid());
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //    //}
            //});

        }
    }
}