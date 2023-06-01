using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using Plugin.CurrentActivity;
using WikitekMotorCycleMechanik.Droid.Dependencies;
using WikitekMotorCycleMechanik.Interfaces;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBarStyleManager))]
namespace WikitekMotorCycleMechanik.Droid.Dependencies
{
    internal class StatusBarStyleManager : IStatusBarStyleManager
    {

        public void ChangeTheme(string user_type)
        {
            var activity = MainActivity.Instance;
            if (string.IsNullOrWhiteSpace(user_type))
            {
                activity.SetTheme(Resource.Style.WikitekTheme);
                activity.SetStatusBarColor(Android.Graphics.Color.ParseColor("#313377"));
            }
            else if (user_type == "wikitekMechanik")
            {
                activity.SetTheme(Resource.Style.WikitekTheme);
                activity.SetStatusBarColor(Android.Graphics.Color.ParseColor("#313377"));
            }
            else if (user_type == "rsangleMechanik")
            {
                activity.SetTheme(Resource.Style.RsangleTheme);
                activity.SetStatusBarColor(Android.Graphics.Color.ParseColor("#2c832c"));
            }
            else if (user_type == "mobitekMechanik")
            {
                activity.SetTheme(Resource.Style.MobitekTheme);
                activity.SetStatusBarColor(Android.Graphics.Color.ParseColor("#934b00"));
            }
        }

        public void SetColoredStatusBar(string statusColor,string navBarColor)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var currentWindow = GetCurrentWindow();
                    currentWindow.DecorView.SystemUiVisibility = 0;
                    currentWindow.SetStatusBarColor(Android.Graphics.Color.ParseColor(statusColor));

                    //MainActivity.Instance.Delegate.SetLocalNightMode(AppCompatDelegate.);
                    //MainActivity.Instance.Recreate();

                    //currentWindow.
                    //ContextThemeWrapper themeWrapper = new ContextThemeWrapper(this, Resource.Color);
                    //LayoutInflater layoutInflater = LayoutInflater.From(themeWrapper);
                    //viewContainer.removeAllViews();
                    //layoutInflater.inflate(R.layout.my_layout, viewContainer, true);


                    //AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);
                    //alertDialogBuilder.set
                    //Application.Current.MainPage.Background==
                });
            }
        }

        public void SetWhiteStatusBar(string hexColor)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var currentWindow = GetCurrentWindow();
                    currentWindow.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
                    currentWindow.SetStatusBarColor(Android.Graphics.Color.ParseColor(hexColor));
                });
            }
        }

        Window GetCurrentWindow()
        {
            var window = CrossCurrentActivity.Current.Activity.Window;

            // clear FLAG_TRANSLUCENT_STATUS flag:
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);

            // add FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS flag to the window
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

            return window;
        }
    }
}