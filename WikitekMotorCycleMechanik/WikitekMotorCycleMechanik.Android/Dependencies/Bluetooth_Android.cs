using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using APDiagnosticAndroid;
using APDiagnosticAndroid.Enums;
using AutoELM327;
using Java.IO;
using Java.Util;
using WikitekMotorCycleMechanik.Droid.Receivers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Droid.Dependencies;
using WikitekMotorCycleMechanik.Droid.StaticClass;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using Xamarin.Forms;
using APDongleCommAndroid;

[assembly: Dependency(typeof(Bluetooth_Android))]
namespace WikitekMotorCycleMechanik.Droid.Dependencies
{
    public class Bluetooth_Android : IBlueToothDevices
    {
        BluetoothDeviceReceiver receiver;
        BluetoothSocket socket = null;
        OutputStreamInvoker outStream = null;
        BufferedReader bufferedReader = null;
        InputStreamReader inputStreamReader = null;
        APELMDongleComm dongleCommWin;
        DongleComm dongleCommWin24;
        UDSDiagnostic dSDiagnosticELM;
        APDiagnosticsAndroid24.UDSDiagnostic dSDiagnostic24;
        string dongle_type = string.Empty;

        private CancellationTokenSource _ct { get; set; }

        public Task SearchBlueTooth()
        {
            try
            {
                receiver = new BluetoothDeviceReceiver();
                RegisterBluetoothReceiver();
                StartScanning();
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void RegisterBluetoothReceiver()
        {
            try
            {
                var context = Android.App.Application.Context;
                context.RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionFound));
                //context.RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionAclDisconnectRequested));
                context.RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionAclDisconnected));
                context.RegisterReceiver(receiver, new IntentFilter(BluetoothAdapter.ActionDiscoveryStarted));
                context.RegisterReceiver(receiver, new IntentFilter(BluetoothAdapter.ActionDiscoveryFinished));
            }
            catch (Exception ex)
            {
            }
        }

        public void UnRegisterBluetoothReceiver()
        {
            try
            {
                var context = Android.App.Application.Context;
                context.UnregisterReceiver(receiver);
                //context.RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionAclDisconnectRequested));
                //context.RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionAclDisconnected));
                //context.RegisterReceiver(receiver, new IntentFilter(BluetoothAdapter.ActionDiscoveryStarted));
                //context.RegisterReceiver(receiver, new IntentFilter(BluetoothAdapter.ActionDiscoveryFinished));
            }
            catch (Exception ex)
            {
            }
        }

        private static void StartScanning()
        {
            if (!BluetoothDeviceReceiver.Adapter.IsDiscovering)
                BluetoothDeviceReceiver.Adapter.StartDiscovery();
        }

        public void CancleScanning()
        {
            try
            {
                BluetoothDeviceReceiver.Adapter.CancelDiscovery();
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<string> ConnectionBT(string name, int sleepTime, bool readAsCharArray)
        {
            //Task.Run(() => Connect(name, sleepTime, readAsCharArray));
            //await Connect(name, sleepTime, readAsCharArray);
            socket = null;
            if (socket == null)
            {
                await Task.Delay(100);
                var isCompletedSuccessfully = Task.Run(async () => loop(name, sleepTime, readAsCharArray))
                    .Wait(TimeSpan.FromSeconds(30));
                _ct.Cancel();
                if (isCompletedSuccessfully)
                {
                    return "connected";
                }
                else
                {
                    return "not connected";
                }
                //Task.Factory.StartNew(async () =>
                //{
                //    Connect(name, sleepTime, readAsCharArray);
                //}).Result.ConfigureAwait(false);
            }
            else
            {
                socket.Close();
                socket = null;
                return "not connected";
            }
        }
        private async Task loop(string address, int sleepTime, bool readAsCharArray)
        {
            BluetoothDevice device = null;
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            BluetoothServerSocket bthServerSocket = null;

            _ct = new CancellationTokenSource();
            while (_ct.IsCancellationRequested == false)
            {
                try
                {
                    Thread.Sleep(sleepTime);

                    adapter = BluetoothAdapter.DefaultAdapter;

                    //List<BluetoothDevice> L = new List<BluetoothDevice>();
                    //foreach (BluetoothDevice d in adapter.BondedDevices)
                    //{
                    //    System.Diagnostics.Debug.WriteLine("D: " + d.Name + " " + d.Address + " " + d.BondState.ToString());
                    //    L.Add(d);
                    //}
                    //device = L.Find(j => j.Address == name);

                    device = BluetoothStatic.bluetoothDevices.FirstOrDefault(x => x.Address == address);//bluetoothDevice;
                    BluetoothDevice paired_device = BluetoothAdapter.DefaultAdapter.BondedDevices.
                        FirstOrDefault(x => x.Address == address);

                    if (paired_device == null)
                    {

                        var returnValue = device.CreateBond();
                        var state = device.BondState;
                    }
                    if (device == null)
                    {
                        //System.Diagnostics.Debug.WriteLine("Named device not found.");
                        //Xamarin.Forms.MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "TerminalLog", "Named device not found.");
                    }
                    else
                    {
                        UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                        //bthServerSocket = adapter.ListenUsingRfcommWithServiceRecord("TLCI Barcode Scanner", uuid);

                        UUID uuids = device.GetUuids()[0].Uuid;

                        if ((int)Android.OS.Build.VERSION.SdkInt >= 10) // Gingerbread 2.3.3 2.3.4
                            socket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);
                        else
                            socket = device.CreateRfcommSocketToServiceRecord(uuid);

                        //  socket = bthServerSocket.Accept();

                        //   adapter.CancelDiscovery();
                        if (socket != null)
                        {
                            //await Task.Delay(2000);
                            socket.Connect();
                            if (socket.IsConnected)
                            {
                                outStream = (OutputStreamInvoker)socket.OutputStream;
                                inputStreamReader = new InputStreamReader(socket.InputStream);
                                bufferedReader = new BufferedReader(inputStreamReader);
                                break;
                            }
                        }
                        else
                            System.Diagnostics.Debug.WriteLine("BthSocket = null");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("EXCEPTION: " + ex.Message);
                }
                finally
                {
                    //if (socket != null)
                    //    socket.Close();
                    //device = null;
                    //adapter = null;
                }
            }
            System.Diagnostics.Debug.WriteLine("Exit the external loop");
        }

        public async Task<string> GetFirmwareVersion(string dongle_type, string txHeader, string rxHeader, string protocol, bool is_padding)
        {
            //byte[] ba = Encoding.Default.GetBytes("500C47568AFE56214E238000FFC3");
            //byte[] ba = Encoding.Default.GetBytes("500C65EFA86574FE9A890018CE16");
            //SendCommand("500C47568AFE56214E238000FFC3");

            var firmware_version = await DongleCommunication("500C47568AFE56214E238000FFC3", dongle_type, txHeader, rxHeader, protocol,  is_padding);
            return firmware_version;
        }

        private async Task<string> DongleCommunication(string sec_cmd, string dongle_type, string txHeader, string rxHeader, string protocol, bool is_padding)
        {
            try
            {
                string firmware_version = string.Empty;
                this.dongle_type = dongle_type;
                if (dongle_type == "24v")
                {
                    //APDongleCommAnroid.Protocol index = (APDongleCommAnroid.Protocol)protocol;
                    APDongleCommAnroid.Protocol index = (APDongleCommAnroid.Protocol)Enum.Parse(typeof(APDongleCommAnroid.Protocol), protocol);
                    dongleCommWin24 = new DongleComm(socket, index, Convert.ToUInt32(txHeader, 16), Convert.ToUInt32(rxHeader, 16), 0x00, 0x10, 0x10, 0x10);
                    dongleCommWin24.InitializePlatform(APDongleCommAnroid.ENUMS.Platform.Android, APDongleCommAnroid.ENUMS.Connectivity.Bluetooth);
                    dSDiagnostic24 = new APDiagnosticsAndroid24.UDSDiagnostic(dongleCommWin24);

                    var securityAccess = await dongleCommWin24.SecurityAccess();
                    var securityResult = (byte[])securityAccess;
                    var securityResponse = ByteArrayToString(securityResult);
                    int x = (int)index;
                    var setProtocol = await dongleCommWin24.Dongle_SetProtocol(x);

                    var setHeader = await dongleCommWin24.CAN_SetTxHeader(txHeader);

                    var setHeaderMask = await dongleCommWin24.CAN_SetRxHeaderMask(rxHeader);

                    var setpadding = await dongleCommWin24.CAN_StartPadding("00");

                    var firmwareVersion = await dongleCommWin24.Dongle_GetFimrwareVersion();
                    var firmwareResult = (byte[])firmwareVersion;
                    firmware_version = firmwareResult[3].ToString("D2") + "." + firmwareResult[4].ToString("D2") + "." + firmwareResult[5].ToString("D2");
                }
                else
                {
                    AutoELM327.Enums.ProtocolEnum index1 = (AutoELM327.Enums.ProtocolEnum)Enum.Parse(typeof(AutoELM327.Enums.ProtocolEnum), protocol);
                    //socket.InputStream.IsDataAvailable();
                    dongleCommWin = new APELMDongleComm(socket, index1);
                    dongleCommWin.InitializePlatform(AutoELM327.Enums.Platform.Android, AutoELM327.Enums.Connectivity.Bluetooth);
                    dSDiagnosticELM = new UDSDiagnostic(dongleCommWin);

                    var setProtocol = await dongleCommWin.Dongle_SetProtocol(index1);

                    if (protocol.Contains("CAN"))
                    {
                        var setHeader = await dongleCommWin.CAN_SetTxHeader(txHeader);

                        var setHeaderMask = await dongleCommWin.CAN_SetRxHeaderMask(rxHeader);
                    }
                    else
                    {
                        var setHeader = await dongleCommWin.ISOK_SetHeader(txHeader, rxHeader);
                    }

                    if (is_padding)
                    {
                        var setpadding = await dongleCommWin.CAN_StartPadding();
                    }

                    //if (is_padding)
                    //{
                    //    var setpadding = await dongleCommWin.CAN_StartPadding();
                    //}
                    //else
                    //{
                    //    var setpadding = await dongleCommWin.CAN_StopPadding();
                    //}

                    var firmwareVersion = await dongleCommWin.Dongle_GetFimrwareVersion();
                    firmware_version = firmwareVersion.ToString();
                }

                //dongleCommWin = new DongleCommWin(socket, APDongleCommAnroid.Protocol.ISO15765_500KB_11BIT_CAN,0x7E0, 0x7E8, 0x00, 0x10, 0x10, 0x10);
                //dongleCommWin.InitializePlatform(APDongleCommAnroid.ENUMS.Platform.Android, APDongleCommAnroid.ENUMS.Connectivity.Bluetooth);
                //dSDiagnostic = new UDSDiagnostic(dongleCommWin);



                //var ProtocolResult = (byte[])setProtocol;
                //var ProtocolResponse = ByteArrayToString(ProtocolResult);

                //var HeaderResult = (byte[])setHeader;
                //var HeaderResponse = ByteArrayToString(HeaderResult);

                //var HeaderMarkResult = (byte[])setHeaderMask;
                //var HeaderMarkResponse = ByteArrayToString(HeaderMarkResult);

                //var ver = firmwareResult[3].ToString("D2") + "." + firmwareResult[4].ToString("D2") + "." + firmwareResult[5].ToString("D2");

                return firmware_version;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        //private async Task<string> DongleCommunication(string sec_cmd, string dongle_type, string txHeader, string rxHeader, string protocol,bool is_padding)
        //{
        //    try
        //    {
        //        string firmware_version = string.Empty;
        //        this.dongle_type = dongle_type;
        //        if (dongle_type == "24v")
        //        {
        //            //APDongleCommAnroid.Protocol index = (APDongleCommAnroid.Protocol)protocol;
        //            APDongleCommAnroid.Protocol index = (APDongleCommAnroid.Protocol)Enum.Parse(typeof(APDongleCommAnroid.Protocol), protocol);
        //            dongleCommWin24 = new DongleCommWin(socket, index, Convert.ToUInt32(txHeader, 16), Convert.ToUInt32(rxHeader, 16), 0x00, 0x10, 0x10, 0x10);
        //            dongleCommWin24.InitializePlatform(APDongleCommAnroid.ENUMS.Platform.Android, APDongleCommAnroid.ENUMS.Connectivity.Bluetooth);
        //            dSDiagnostic24 = new APDiagnosticsAndroid24.UDSDiagnostic(dongleCommWin24);

        //            var securityAccess = await dongleCommWin24.SecurityAccess();
        //            var securityResult = (byte[])securityAccess;
        //            var securityResponse = ByteArrayToString(securityResult);
        //            int x = (int)index;
        //            var setProtocol = await dongleCommWin24.Dongle_SetProtocol(x);

        //            var setHeader = await dongleCommWin24.CAN_SetTxHeader(txHeader);

        //            var setHeaderMask = await dongleCommWin24.CAN_SetRxHeaderMask(rxHeader);

        //            var setpadding = await dongleCommWin24.CAN_StartPadding("00");

        //            var firmwareVersion = await dongleCommWin24.Dongle_GetFimrwareVersion();
        //            var firmwareResult = (byte[])firmwareVersion;
        //            firmware_version = firmwareResult[3].ToString("D2") + "." + firmwareResult[4].ToString("D2") + "." + firmwareResult[5].ToString("D2");
        //        }
        //        else
        //        {
        //            AutoELM327.Enums.ProtocolEnum index1 = (AutoELM327.Enums.ProtocolEnum)Enum.Parse(typeof(AutoELM327.Enums.ProtocolEnum), protocol);
        //            //socket.InputStream.IsDataAvailable();
        //            dongleCommWin = new APELMDongleComm(socket, index1);
        //            dongleCommWin.InitializePlatform(AutoELM327.Enums.Platform.Android, AutoELM327.Enums.Connectivity.Bluetooth);
        //            dSDiagnosticELM = new UDSDiagnostic(dongleCommWin);

        //            var setProtocol = await dongleCommWin.Dongle_SetProtocol(index1);

        //            if(protocol.Contains("CAN"))
        //            {
        //                var setHeader = await dongleCommWin.CAN_SetTxHeader(txHeader);

        //                var setHeaderMask = await dongleCommWin.CAN_SetRxHeaderMask(rxHeader);
        //            }
        //            else
        //            {
        //                var setHeader = await dongleCommWin.ISOK_SetHeader(txHeader,rxHeader);

        //            }

        //            //var setHeader1 = await dongleCommWin.CAN_DisableFormatting();
        //            //if (is_padding)
        //            //{
        //            //    var setpadding = await dongleCommWin.CAN_StartPadding();
        //            //}
        //            //else
        //            //{
        //            //    var setpadding = await dongleCommWin.CAN_StopPadding();
        //            //}

        //            var firmwareVersion = await dongleCommWin.Dongle_GetFimrwareVersion();
        //            firmware_version = firmwareVersion.ToString();
        //        }

        //        //dongleCommWin = new DongleCommWin(socket, APDongleCommAnroid.Protocol.ISO15765_500KB_11BIT_CAN,0x7E0, 0x7E8, 0x00, 0x10, 0x10, 0x10);
        //        //dongleCommWin.InitializePlatform(APDongleCommAnroid.ENUMS.Platform.Android, APDongleCommAnroid.ENUMS.Connectivity.Bluetooth);
        //        //dSDiagnostic = new UDSDiagnostic(dongleCommWin);



        //        //var ProtocolResult = (byte[])setProtocol;
        //        //var ProtocolResponse = ByteArrayToString(ProtocolResult);

        //        //var HeaderResult = (byte[])setHeader;
        //        //var HeaderResponse = ByteArrayToString(HeaderResult);

        //        //var HeaderMarkResult = (byte[])setHeaderMask;
        //        //var HeaderMarkResponse = ByteArrayToString(HeaderMarkResult);

        //        //var ver = firmwareResult[3].ToString("D2") + "." + firmwareResult[4].ToString("D2") + "." + firmwareResult[5].ToString("D2");

        //        return firmware_version;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //}

        public async void SendHeader(string txHeader, string rxHeader)
        {
            try
            {
                if (dongle_type == "24v")
                {
                    var setHeader = await dongleCommWin24.CAN_SetTxHeader(txHeader);
                    var setHeaderMask = await dongleCommWin24.CAN_SetRxHeaderMask(rxHeader);
                }
                else
                {
                    var setHeader = await dongleCommWin.CAN_SetTxHeader(txHeader);
                    var setHeaderMask = await dongleCommWin.CAN_SetRxHeaderMask(rxHeader);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<Models.ReadDtcResponseModel> ReadDtc(string dtc_index)
        {
            try
            {
                //var dongleCommWin = new APELMDongleComm(socket, AutoELM327.Enums.ProtocolEnum.ISO_15765_4_CAN_11BIT_500K_BAUD);
                //dongleCommWin.InitializePlatform(AutoELM327.Enums.Platform.Android, AutoELM327.Enums.Connectivity.Bluetooth);
                //var dSDiagnostic = new UDSDiagnostic(dongleCommWin);

                APDiagnosticAndroid.Models.ReadDtcResponseModel readDTC = new APDiagnosticAndroid.Models.ReadDtcResponseModel();
                if (App.dongle == "24v")
                {
                    ReadDTCIndex index = (ReadDTCIndex)Enum.Parse(typeof(ReadDTCIndex), dtc_index);
                    readDTC = await dSDiagnostic24.ReadDTC(index);
                }
                else
                {
                    ReadDTCIndex index = (ReadDTCIndex)Enum.Parse(typeof(ReadDTCIndex), dtc_index);
                    readDTC = await dSDiagnosticELM.ReadDTC(index);
                }
                string dtcCode = "";
                if (readDTC.dtcs != null)
                {
                    var responseLlength = readDTC.dtcs.GetLength(0);
                    for (int i = 0; i <= responseLlength - 1; i++)
                    {
                        dtcCode += readDTC.dtcs[i, 0].ToString() + " - ";
                    }
                }

                Models.ReadDtcResponseModel readDtcResponseModel = new Models.ReadDtcResponseModel();
                readDtcResponseModel.dtcs = readDTC.dtcs;
                readDtcResponseModel.status = readDTC.status;
                return readDtcResponseModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> ClearDtc(string dtc_index)
        {
            try
            {
                //if (dtc_index == "UDS-4BYTES")
                //{
                //    dtc_index = "UDS_4BYTES";
                //}

                string status = string.Empty;
                object result = new object();
                ClearDTCIndex index = (ClearDTCIndex)Enum.Parse(typeof(ClearDTCIndex), dtc_index);
                await Task.Run(async () =>
                {
                    if (App.dongle == "24v")
                    {
                        result = await dSDiagnostic24.ClearDTC(index);
                    }
                    else
                    {
                        result = await dSDiagnosticELM.ClearDTC(index);
                    }
                    //result = await dSDiagnostic.ClearDTC(index);
                    var res = JsonConvert.SerializeObject(result);
                    var Response = JsonConvert.DeserializeObject<ClearDtcResponseModel>(res);
                    status = Response.ECUResponseStatus;
                });
                return status;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string[]> GetGenericObdPid()
        {

            try
            {
                if (App.dongle == "24v")
                {
                    var generin_pid = await dSDiagnostic24.genericOBDSupportedPidList();
                    return generin_pid;
                }
                else
                {
                    var generin_pid = await dSDiagnosticELM.genericOBDSupportedPidList();
                    return generin_pid;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<Models.ReadPidPresponseModel>> ReadPid(ObservableCollection<PidCode> pidList)
        {
            try
            {
                object result = new object();
                ObservableCollection<APDiagnosticAndroid.Models.ReadParameterPID> list = new ObservableCollection<APDiagnosticAndroid.Models.ReadParameterPID>();
                foreach (var item in pidList)
                {
                    var MessageValueList = new List<APDiagnosticAndroid.Models.SelectedParameterMessage>();
                    if (item.messages != null)
                    {
                        foreach (var MessageItem in item.messages)
                        {
                            MessageValueList.Add(new APDiagnosticAndroid.Models.SelectedParameterMessage { code = MessageItem.code, message = MessageItem.message });
                        }
                    }

                    list.Add(
                        new APDiagnosticAndroid.Models.ReadParameterPID
                        {
                            datatype = item.message_type,
                            IsBitcoded = item.bitcoded,
                            noofBits = item.end_bit_position.GetValueOrDefault() - item.start_bit_position.GetValueOrDefault() + 1,
                            noOfBytes = item.length,
                            offset = item.offset,
                            pid = item.code,
                            resolution = item.resolution,
                            startBit = Convert.ToInt32(item.start_bit_position),
                            startByte = item.byte_position,
                            //totalBytes= item.totalBytes,
                            totalLen = item.code.Length / 2,
                            pidNumber = item.id,
                            pidName = item.short_name,
                            messages = MessageValueList

                        });
                }
                var respo = await Task.Run(async () =>
                {
                    if (App.dongle == "24v")
                    {
                        result = await dSDiagnostic24.ReadParameters(list.Count, list);
                    }
                    else
                    {
                        result = await dSDiagnosticELM.ReadParameters(list.Count, list);
                    }
                    //result = await dSDiagnostic.ReadParameters(pidList.Count, list);
                    var res = JsonConvert.SerializeObject(result);
                    var res_list = JsonConvert.DeserializeObject<ObservableCollection<ReadPidPresponseModel>>(res);
                    return res_list;
                    //status = Response.ECUResponseStatus;
                });

                return respo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        static string ConvertedByteToString(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                using (var stremReader = new StreamReader(stream))
                {
                    return stremReader.ReadToEnd();
                }
            }
        }

        public async Task<string> SetProtocol(string protocol)
        {
            try
            {
                AutoELM327.Enums.ProtocolEnum index1 = (AutoELM327.Enums.ProtocolEnum)Enum.Parse(typeof(AutoELM327.Enums.ProtocolEnum), protocol);
                dongleCommWin = new APELMDongleComm(socket, index1);
                dongleCommWin.InitializePlatform(AutoELM327.Enums.Platform.Android, AutoELM327.Enums.Connectivity.Bluetooth);
                dSDiagnosticELM = new UDSDiagnostic(dongleCommWin);

                var setProtocol = await dongleCommWin.Dongle_SetProtocol(index1);
                var setCan = await dongleCommWin.Dongle_SetCan();
                var findProtocol= await dongleCommWin.Dongle_FindProtocol();

                return (string)findProtocol;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public void DisconnectDongle()
        {
            try
            {   
                //socket.Dispose();
                socket.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    socket.Close();
                    //socket.Dispose();
                }
                catch (Exception EX)
                {
                }
            }
        }
    }
}