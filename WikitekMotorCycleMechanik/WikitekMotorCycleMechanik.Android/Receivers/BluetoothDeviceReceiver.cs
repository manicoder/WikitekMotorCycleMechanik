using Android.Bluetooth;
using Android.Content;
using WikitekMotorCycleMechanik.Models;
using System.Linq;
using WikitekMotorCycleMechanik.Droid.StaticClass;

namespace WikitekMotorCycleMechanik.Droid.Receivers
{
    [BroadcastReceiver(Exported = false)]
    public class BluetoothDeviceReceiver : BroadcastReceiver
    {
        public static BluetoothAdapter Adapter => BluetoothAdapter.DefaultAdapter;

        public override void OnReceive(Context context, Intent intent)
        {
            var action = intent.Action;
            //ObservableCollection<BluetoothDevicesModel> list = new ObservableCollection<BluetoothDevicesModel>();
            // Found a device
            switch (action)
            {
                case BluetoothDevice.ActionFound:
                    // Get the device
                    var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);

                    //if (device.Name == "WikitekMotorCycleMechanik")
                    //if (device.Name == "OBDII BT Dongle" || device.Name == "APtBudB" || device.Name == "WikitekMotorCycleMechanik")
                    if (device.Name == App.dongle_type)
                    {
                        var list = App.bluetooth_devices.FirstOrDefault(x => x.mac_address== device.Address);
                        if (list == null)
                        {
                            App.bluetooth_devices.Add(
                                new BluetoothDevicesModel
                                {
                                    mac_address = device.Address,
                                    name= device.Name,
                                });
                        }
                        BluetoothStatic.bluetoothDevices.Add(device);
                    }

                    break;
                case BluetoothDevice.ActionAclDisconnected:
                    if (App.is_global_method)
                    {
                        App.GlobalFuntion();
                    }
                    //Console.WriteLine("ActionAclDisconnected FOUND");
                    break;
                //case BluetoothDevice.ActionAclDisconnectRequested:
                //    Console.WriteLine("ActionAclDisconnectRequested FOUND");
                //    break;
                case BluetoothAdapter.ActionDiscoveryStarted:
                    //MainActivity.GetInstance().UpdateAdapterStatus("Discovery Started...");
                    break;
                case BluetoothAdapter.ActionDiscoveryFinished:
                    App.bt_available = false;
                    //MainActivity.GetInstance().UpdateAdapterStatus("Discovery Finished.");
                    break;
                default:
                    break;
            }
        }
    }
}