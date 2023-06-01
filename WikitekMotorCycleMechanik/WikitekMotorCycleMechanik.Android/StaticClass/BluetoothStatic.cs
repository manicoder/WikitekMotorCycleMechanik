using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikitekMotorCycleMechanik.Droid.StaticClass
{
    public static class BluetoothStatic
    {
        public static List<BluetoothDevice> bluetoothDevices = new List<BluetoothDevice>();
    }
}