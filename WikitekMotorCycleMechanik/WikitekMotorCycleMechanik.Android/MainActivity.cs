using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using System.Net;
using AndroidX.Core.Content;
using AndroidX.Core.App;
using Acr.UserDialogs;
using Plugin.CurrentActivity;
using Firebase;
using Android.Bluetooth;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Content;
using Android.Util;
using Rg.Plugins.Popup.Services;
using Com.Razorpay;
using System.Net.Http;
using FFImageLoading;
using FFImageLoading.Config;
//using FFImageLoading;
//using FFImageLoading.Forms.Platform;

namespace WikitekMotorCycleMechanik.Droid
{
    [Activity(Label = "wikitek1", Icon = "@drawable/ic_wikitek1", Theme = "@style/WikitekTheme", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPaymentResultWithDataListener
    {
        public string fcm_err = string.Empty;
        static readonly string TAG = "MainActivity";

        internal static readonly string CHANNEL_ID = "wikitek_dtool";
        internal static readonly int NOTIFICATION_ID = 100;

        public const int REQUEST_CHECK_SETTINGS = 0x1;

        //private BluetoothDeviceReceiver _receiver;
        const int RequestLocationId = 0;
        //public Android.Support.V7.Widget.Toolbar toolbar;
        public static MainActivity Instance { get; private set; }

        readonly string[] LocationPermissions =
        {
             Manifest.Permission.AccessCoarseLocation ,
             Manifest.Permission.AccessFineLocation ,
             Manifest.Permission.BluetoothPrivileged,
             Manifest.Permission.Bluetooth,
             Manifest.Permission.BluetoothAdmin,
             Manifest.Permission.ReadExternalStorage,
             Manifest.Permission.WriteExternalStorage,
             Manifest.Permission.ReadPhoneState,
             Manifest.Permission.Internet
        };


        protected override void OnCreate(Bundle savedInstanceState)
        {
            //TabLayoutResource = Resource.Layout.Tabbar;
            //ToolbarResource = Resource.Layout.Toolbar;

            ServicePointManager
           .ServerCertificateValidationCallback +=
           (sender, cert, chain, sslPolicyErrors) => true;

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            FFImageLoading.Forms.Platform.CachedImageRenderer.InitImageViewHandler();
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) => true;

            var client = new HttpClient(handler);
            ImageService.Instance.Initialize(new Configuration
            {
                HttpClient = client
            });



            Instance = this;

            var scale = Resources.DisplayMetrics.Density;//density i.e., pixels per inch or cms  
                                                         //var widthPixels = Resources.DisplayMetrics.WidthPixels;
                                                         //App. = (double)((widthPixels - 0.5f) / scale);
            var heightPixels = Resources.DisplayMetrics.HeightPixels;////getting the height in pixels  
            App.ScreenHeight = (double)((heightPixels - 0.5f) / scale);

            base.OnCreate(savedInstanceState);

            var coarseLocationPermissionGranted = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation);
            var fineLocationPermissionGranted = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation);
            var InternetPermissionGranted = ContextCompat.CheckSelfPermission(this, Manifest.Permission.Internet);
            if (coarseLocationPermissionGranted == Permission.Denied || fineLocationPermissionGranted == Permission.Denied || InternetPermissionGranted == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, LocationPermissions, RequestLocationId);
            }
            else
            { }

            DisplayLocationSettingsRequest();

            //Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token);
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            UserDialogs.Init(this);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);
            //Xamarin.FormsMaps.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //_receiver = new BluetoothDeviceReceiver();
            LoadApplication(new App());

            //if (Intent.Extras != null)
            //{
            //    foreach (var key in Intent.Extras.KeySet())
            //    {
            //        var value = Intent.Extras.GetString(key);
            //        Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
            //    }
            //}

            //FirebaseApp.InitializeApp(this);

            //IsPlayServicesAvailable();

            //CreateNotificationChannel();

            Android.Bluetooth.BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            // is bluetooth enabled?
            if (!bluetoothAdapter.IsEnabled)
            {
                try
                {
                    bluetoothAdapter.Enable();
                }
                catch (Exception ex)
                {

                }
            }
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            if (intent != null)
            {
                var args = intent.GetStringExtra("args");
                // startActivityForResult(new Intent(Android.settings.NOTIFICATION_POLICY_ACCESS_SETTINGS), 0);
                //PushNotificationManager.ProcessIntent(this, intent);
            }
        }


        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
                PopupNavigation.Instance.PopAsync();
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }

        private void DisplayLocationSettingsRequest()
        {
            var googleApiClient = new GoogleApiClient.Builder(this).AddApi(LocationServices.API).Build();
            googleApiClient.Connect();

            var locationRequest = LocationRequest.Create();
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            locationRequest.SetInterval(10000);
            locationRequest.SetFastestInterval(10000 / 2);

            var builder = new LocationSettingsRequest.Builder().AddLocationRequest(locationRequest);
            builder.SetAlwaysShow(true);

            var result = LocationServices.SettingsApi.CheckLocationSettings(googleApiClient, builder.Build());
            result.SetResultCallback((LocationSettingsResult callback) =>
            {
                switch (callback.Status.StatusCode)
                {
                    case LocationSettingsStatusCodes.Success:
                        {
                            //DoStuffWithLocation();
                            break;
                        }
                    case LocationSettingsStatusCodes.ResolutionRequired:
                        {
                            try
                            {
                                // Show the dialog by calling startResolutionForResult(), and check the result
                                // in onActivityResult().
                                callback.Status.StartResolutionForResult(this, REQUEST_CHECK_SETTINGS);
                            }
                            catch (IntentSender.SendIntentException e)
                            {
                            }

                            break;
                        }
                    default:
                        {
                            // If all else fails, take the user to the android location settings
                            StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                            break;
                        }
                }
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            switch (requestCode)
            {
                case REQUEST_CHECK_SETTINGS:
                    {
                        //switch (resultCode)
                        //{
                        //    case Android.App.Result.Ok:
                        //        {
                        //            DoStuffWithLocation();
                        //            break;
                        //        }
                        //    case Android.App.Result.Canceled:
                        //        {
                        //            //No location
                        //            break;
                        //        }
                        //}
                        break;
                    }
            }

            if (requestCode == RequestLocationId)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
                {
                    // Permissions granted - display a message.
                }
                else
                {
                    // Permissions denied - display a message.
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }

            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnStart()
        {
            base.OnStart();
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    // Permissions already granted - display a message.
                }
            }
        }

        public void OnPaymentError(int p0, string p1, PaymentData p2)
        {
            Console.WriteLine("error in payment");
        }

        public void OnPaymentSuccess(string p0, PaymentData p1)
        {
            //PaymentResponseModel model = new PaymentResponseModel
            //{
            App.paymentResponseModel.Data = p1.Data;
            App.paymentResponseModel.ExternalWallet = p1.ExternalWallet;
            App.paymentResponseModel.OrderId = p1.OrderId;
            App.paymentResponseModel.PaymentId = p1.PaymentId;
            App.paymentResponseModel.Signature = p1.Signature;
            App.paymentResponseModel.UserContact = p1.UserContact;
            App.paymentResponseModel.UserEmail = p1.UserEmail;
            //};
            Console.WriteLine("success");

            //MessagingCenter.Send<MainActivity, string>(this, "payment", "");
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", e.Exception);
            //LogUnhandledException(newExc);
        }

        ///
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    fcm_err = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                    fcm_err = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                fcm_err = "Google Play Services is available.";
                return true;
            }
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }
}