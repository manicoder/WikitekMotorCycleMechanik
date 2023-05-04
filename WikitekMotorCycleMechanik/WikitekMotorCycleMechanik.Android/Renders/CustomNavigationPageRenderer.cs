using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikitekMotorCycleMechanik.Droid.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationPageRenderer))]
namespace WikitekMotorCycleMechanik.Droid.Renders
{
    class CustomNavigationPageRenderer : NavigationPageRenderer
    {
        public CustomNavigationPageRenderer(Context context) : base(context)
        {

        }

        //private Android.Support.V7.Widget.Toolbar _toolbar;
        AndroidX.AppCompat.Widget.Toolbar _toolbar;



        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);
            if (child.GetType() == typeof(AndroidX.AppCompat.Widget.Toolbar))
            {
                //toolbar = child as Android.Support.V7.Widget.Toolbar;
                //toolbar.ChildViewAdded += Toolbar_ChildViewAdded;
                //var a = toolbar.ChildCount;
                _toolbar = (AndroidX.AppCompat.Widget.Toolbar)child;
                _toolbar.ChildViewAdded += Toolbar_ChildViewAdded;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                //_toolbar.ChildViewAdded -= Toolbar_ChildViewAdded;
            }
        }

        private void Toolbar_ChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {
            var view = e.Child.GetType();

            System.Diagnostics.Debug.WriteLine(view);

            if (e.Child.GetType() == typeof(AndroidX.AppCompat.Widget.AppCompatTextView))
            {
                try
                {
                    var textView = (AndroidX.AppCompat.Widget.AppCompatTextView)e.Child;

                    // TODO: CHANGE VALUES HERE
                    textView.TextSize = 17;
                    //textView.TextAlignment = Android.Views.TextAlignment.Center;
                    //float toolbarCenter = _toolbar.MeasuredWidth / 2;
                    //float titleCenter = textView.MeasuredWidth / 2;
                    //textView.SetX(toolbarCenter - titleCenter);
                    //textView.SetX();
                    textView.SetTypeface(null, TypefaceStyle.Bold);
                    //textView.LayoutParameters = new Android.Support.V7.Widget.Toolbar.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent, (int)GravityFlags.CenterHorizontal);

                    _toolbar.ChildViewAdded -= Toolbar_ChildViewAdded;
                }
                catch (System.Exception ex)
                {
                }
            }
        }
    }
}