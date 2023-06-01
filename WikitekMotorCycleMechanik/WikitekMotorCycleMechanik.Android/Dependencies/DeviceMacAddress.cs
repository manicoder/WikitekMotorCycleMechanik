using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Droid.Dependencies;
using WikitekMotorCycleMechanik.Interfaces;
using Xamarin.Forms;
//using static Android.Provider.Settings;
//using static Android.Provider.Settings;

[assembly: Dependency(typeof(DeviceMacAddress))]
namespace WikitekMotorCycleMechanik.Droid.Dependencies
{
    public class DeviceMacAddress : IDeviceMacAddress
    {
        public async Task<string> GetDeviceUniqueId()
        {
            var context = Android.App.Application.Context;
            string id = Android.Provider.Settings.Secure.GetString(Forms.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            await Task.Delay(500);
            return id;

            //verify permission
            //var status = await CrossPermissions.Current.CheckPermissionStatusAsync<PhonePermission>();
            //if (status != PermissionStatus.Granted)
            //{
            //    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Phone))
            //    {
            //        await App.Current.MainPage.DisplayAlert("Alert", "Need permission", "OK");
            //    }

            //    status = await CrossPermissions.Current.RequestPermissionAsync<PhonePermission>();
            //}

            //if (status == PermissionStatus.Granted)
            //{
            //    //Query permission
            //    try
            //    {
            //        Android.Telephony.TelephonyManager mTelephonyMgr = (Android.Telephony.TelephonyManager)Forms.Context.GetSystemService(Android.Content.Context.TelephonyService);
            //        //var temp1 = mTelephonyMgr.GetMeid(0);
            //        //var temp2 = mTelephonyMgr.GetMeid(1);
            //        var temp3 = mTelephonyMgr.DeviceId;
            //        //if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            //        //    // TODO: Some phones has more than 1 SIM card or may not have a SIM card inserted at all
            //        //    return mTelephonyMgr.GetMeid(0);
            //        //else

            //        //    return mTelephonyMgr.DeviceId;
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}

            //return null;

        }

        #region OLD CODE

        public async Task<string> GetMacAddress()
        {
            string ip = string.Empty;
            try
            {
                //    var ni = NetworkInterface.GetAllNetworkInterfaces().OrderBy(intf => intf.NetworkInterfaceType)
                //.FirstOrDefault(intf => intf.OperationalStatus == OperationalStatus.Up
                // && (intf.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                // || intf.NetworkInterfaceType == NetworkInterfaceType.Ethernet));
                //    {
                //        var hw = ni.GetPhysicalAddress();
                //        ip = string.Join(":", (from ma in hw.GetAddressBytes() select ma.ToString("X2")).ToArray());
                //    }
                //    await Task.Delay(500);
                //    return ip;

                string sMacAddress = string.Empty;
                var con = Android.App.Application.Context;
                WifiManager wifiManager = (WifiManager)con.GetSystemService(Context.WifiService);
                var is_wifiManager = wifiManager.IsWifiEnabled;

                if (!is_wifiManager)
                {
                    var r = wifiManager.SetWifiEnabled(true);
                    if (r)
                    {
                        sMacAddress = await ReturnMacAddress();
                        wifiManager.SetWifiEnabled(false);
                    }
                    else
                    {
                        sMacAddress = "Please on your device wifi network and again open appliaction";
                    }

                }
                else
                {
                    sMacAddress = await ReturnMacAddress();
                }

                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();


                return sMacAddress;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<string> ReturnMacAddress()
        {
            try
            {

                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                string sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (adapter.Description == "wlan0")
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                    if (adapter.Description == "ccmni1")
                    {
                    }
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                return sMacAddress;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        //public async Task<string> GetUniqID()
        //{
        //    try
        //    {
        //        var con = Android.App.Application.Context;
        //        //TelephonyManager mngr = (TelephonyManager)getSystemService(Context.TELEPHONY_SERVICE);
        //        //WifiManager wm = (TelephonyManager)con.GetSystemService(Context.WifiAwareService);
        //        //TelephonyManager mngr = (TelephonyManager)con.GetSystemService(Context.TelephonyService);
        //        BluetoothManager mngr = (BluetoothManager)con.GetSystemService(Context.BluetoothService);
        //        var info = mngr.Adapter.Address;
        //        //var res = mngr.GetDeviceId(1);
        //        //String imei = SystemProperties.get("ro.gsm.imei")
        //        //var res2 = mngr.;

        //        //String SECURE_SETTINGS_BLUETOOTH_ADDRESS = "bluetooth_address";

        //        //String macAddress = Settings.Secure.getString(getContentResolver(), SECURE_SETTINGS_BLUETOOTH_ADDRESS);

        //        //var res = Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, SECURE_SETTINGS_BLUETOOTH_ADDRESS);


        //        WifiManager wifiManager = (WifiManager)con.GetSystemService(Context.WifiService);
        //        var wInfo = wifiManager.DhcpInfo.Gateway;
        //        //String res = wInfo.MacAddress;

        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //} 
        #endregion
    }
}