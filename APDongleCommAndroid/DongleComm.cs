using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Util;
using Android.Widget;
using APDongleCommAndroid.Helper;
using APDongleCommAnroid;
using APDongleCommAnroid.ENUMS;
using APDongleCommAnroid.Helper;
using APDongleCommAnroid.Models;
using Hoho.Android.UsbSerial.Driver;
using Hoho.Android.UsbSerial.Extensions;
using Java.IO;

namespace APDongleCommAndroid
{
    //----------------------------------------------------------------------------
    // Namespace        : AutoELM327
    // Class Name       : BluetoothHandler
    // Description      : This class implements IBluetoothHandler interface
    // Author           : Autopeepal  
    // Date             : 20-08-20
    // Notes            : 
    // Revision History : 
    //----------------------------------------------------------------------------

    public class DongleComm : ICANCommands, IWifiUSBHandler, IDongleHandler
    {
        #region Properties
        private TaskCompletionSource<object> tskCmSsrc = null;
        private Protocol protocol;
        private string DebugTag = "ELM-DEBUG";
        private ResponseArrayStatus responseStructure;

        BluetoothSocket BluetoothSocket = null;
        SerialInputOutputManager SerialInputOutputManager = null;
        UsbSerialPort USBport = null;
        TcpClient TcpClient = null;

        public Platform Platform =Platform.None;
        public Connectivity Connectivity = Connectivity.None;
        private NetworkStream Stream = null;

        #endregion

        #region Ctor
        public DongleComm()
        {
            Debug.WriteLine("Inside DongleComm", DebugTag);
        }
        public void InitializePlatform(Platform platform, Connectivity connectivity)
        {
            Platform = platform;
            Connectivity = connectivity;
        }
        public DongleComm(BluetoothSocket socket, Protocol protocolVersion, UInt32 txHeader, UInt32 rxHeader, UInt16 paddingByte, UInt16 p2max, UInt16 blkseqcntr, UInt16 septime)
        {
            Debug.WriteLine("Inside BluetoothSocket CTOR", DebugTag);
            BluetoothSocket = socket;
            protocol = protocolVersion;

        }

        public DongleComm(TcpClient client, NetworkStream networkStream, Protocol protocolVersion, UInt32 txHeader, UInt32 rxHeader, UInt16 paddingByte, UInt16 p2max, UInt16 blkseqcntr, UInt16 septime)
        {
            Debug.WriteLine("Inside ELM327 TcpClient CTOR", DebugTag);
            TcpClient = client;
            protocol = protocolVersion;
            Stream = networkStream;
        }

        public DongleComm(SerialInputOutputManager serialInputOutputManager, UsbSerialPort port, Protocol protocolVersion, UInt32 txHeader, UInt32 rxHeader, UInt16 paddingByte, UInt16 p2max, UInt16 blkseqcntr, UInt16 septime)
        {
            Debug.WriteLine("Inside ELM327 SimpleTcpClient CTOR", DebugTag);
            SerialInputOutputManager = serialInputOutputManager;
            USBport = port;
            protocol = protocolVersion;

            if (tskCmSsrc == null)
            {
                tskCmSsrc = new TaskCompletionSource<object>();
            }

            //SerialInputOutputManager.DataReceived += (sender, e) =>
            //{
            //    tskCmSsrc?.TrySetResult(e.Data);
            //    Debug.WriteLine("SerialInputOutputManager.DataReceived"+ ByteArrayToString(e.Data), DebugTag);

            //};
        }

        #endregion

        #region Methods

        #region SendCommand
        public async Task<object> ReadData()
        {
            Debug.WriteLine("------Read Again Data------", DebugTag);
            if (Platform == Platform.Android && Connectivity == Connectivity.Bluetooth)
            {
                return await GetBTCommand();
            }
            if (Platform == Platform.Android && Connectivity == Connectivity.USB)
            {
                
                if (SerialInputOutputManager.IsOpen)
                {
                      return await GetUSBCommand();
                }
            }
            if (Platform == Platform.Android && Connectivity == Connectivity.WiFi)
            {
                return await GetWifiCommand();
            }
            Debug.WriteLine("------END Read Again Data------", DebugTag);
            return null;
        }
        public async Task<object> SendCommand(string randomCommand)
        {
            Debug.WriteLine("------SendCommand------", DebugTag);
            object response = null;

            string command = randomCommand.ToString();
            var bytesCommand = HexStringToByteArray(command);

            byte[] sendBytes = HexStringToByteArray(command);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); }); ;

            return response;

        }

        public async Task<object> SendCommand(byte[] command, Action<string> onDataRecevied)
        {
            if (Platform == Platform.Android && Connectivity == Connectivity.Bluetooth)
            {
                Debug.WriteLine("Command BT Send =  " + ByteArrayToString(command), DebugTag);
                Debug.WriteLine("--------- BT Command Send TIME-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);
                BluetoothSocket.OutputStream.Write(command, 0, command.Length);
                BluetoothSocket.OutputStream.Flush();
                //BluetoothSocket.OutputStream.Close();
                //Thread.Sleep(100);
                var response = await GetBTCommand();

                Debug.WriteLine("Command BT RESPONSE =  " + ByteArrayToString(response), DebugTag);
                Debug.WriteLine("--------- BT Command RESPONSE TIME-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);
                return response;               
            }
            
            if (Platform == Platform.Android && Connectivity == Connectivity.USB)
            {
                Debug.WriteLine("Command USB Send =  " + ByteArrayToString(command), DebugTag);
                Debug.WriteLine("--------- USB Command Send TIME-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);

                if (SerialInputOutputManager.IsOpen)
                {
                    var byteRes = new byte[1024];
                    USBport.Write(command, 0);//TBD CHECK
                    var response = await GetUSBCommand();
                    if (response != null)
                    {
                        var byteResponse = (byte[])response;
                        Debug.WriteLine("Command USB RESPONSE =  " + ByteArrayToString(byteResponse), DebugTag);
                        Debug.WriteLine("--------- USB Command RESPONSE TIME-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);

                        return response;
                    }
                }
            }

            if (Platform == Platform.Android && Connectivity == Connectivity.WiFi)
            {
                Debug.WriteLine("Command WiFi Send =  " + ByteArrayToString(command), DebugTag);

                //00 000e 00000000 00000000 500c47568afe56214e238000ffc3 ce2c

                //00 - Tx sequence counter
                //000e - length of the command(command length + 2)
                //00000000 - Timestamp - hhmmssmm
                //00000000 - Reserved - always 00
                //500c47568afe56214e238000ffc3 - actual command - payload
                //ce2c - crc of entire length from tx counter to the payload.

                var byte1 = "00";
                var byte2 = command.Length.ToString("X4");
                var byte3 = DateTime.Now.ToString("hhmmssff");
                var byte4 = DateTime.Now.ToString("00000000");
                var byte5 = ByteArrayToString(command);
                var dataBytes = HexStringToByteArray(byte1 + byte2 + byte3 + byte4 + byte5);
                var byte6Checksum = Crc16CcittKermit.ComputeChecksum(dataBytes);

                var bytes = ByteArrayToString(dataBytes) + byte6Checksum.ToString();
                var byteData = HexStringToByteArray(bytes);
                Stream = TcpClient.GetStream();
                Stream.Write(byteData, 0, byteData.Length);

                return GetWifiCommand();
            }
            return null;
        }

        public async Task<byte[]> GetWifiCommand()
        {
            byte[] rbuffer = new byte[1024];
            byte[] RetArray = new byte[] { };
            byte[] ActualBytes = new byte[] { };

            Debug.WriteLine("---------INSIDE READ DATA-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);
            int readByte = await Stream.ReadAsync(rbuffer, 0, rbuffer.Length);

            RetArray = new byte[readByte];
            Array.Copy(rbuffer, 0, RetArray, 0, readByte);

            Debug.WriteLine("--------- READ DATA RESPONSE-------" + ByteArrayToString(RetArray), DebugTag);

            var byteLength = RetArray.Length.ToString("d4");
            string hexValue = RetArray[1].ToString() + RetArray[2].ToString();
            int decValue = Convert.ToInt32(hexValue, 16);

            Array.Copy(RetArray, 12, ActualBytes, 0, decValue);
            Debug.WriteLine("--------- DATA RESPONSE-------" + ByteArrayToString(ActualBytes), DebugTag);
            return ActualBytes;
        }

        #region Original Code
        public async Task<byte[]> GetBTCommand()
        {
            try
            {
                byte[] rbuffer = new byte[1024];
                byte[] RetArray = new byte[] { };

                Debug.WriteLine("---------BT INSIDE READ DATA-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);
                // Read data from the device
                while (!BluetoothSocket.InputStream.CanRead)
                {
                    Debug.WriteLine("------------------------------------------------");
                }
               
                Debug.WriteLine("---------Bluetooth Socket READ Buffer-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);
                Stream inStream = BluetoothSocket.InputStream;
                int readByte = inStream.Read(rbuffer, 0, rbuffer.Length);

                //int readByte = await BluetoothSocket.InputStream.ReadAsync(rbuffer, 0, rbuffer.Length);
                //int readByte =  BluetoothSocket.InputStream.Read(rbuffer, 0, rbuffer.Length);
                RetArray = new byte[readByte];
                Array.Copy(rbuffer, 0, RetArray, 0, readByte);

                //Console.WriteLine(Array2Text(RetArray, false) + "\n" + "[" + RetArray.Length.ToString() + "]");

                Debug.WriteLine("--------- BT READ DATA RESPONSE-------" + ByteArrayToString(RetArray), DebugTag);
                // Debug.WriteLine("--------- BT READ DATA RESPONSE TIME-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);

                return RetArray;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        //public async Task<byte[]> GetBTCommand()
        //{
        //    byte[] rbuffer = new byte[1024];
        //    char[] chr = new char[100];
        //    byte[] RetArray = new byte[] { };

        //    Debug.WriteLine("---------BT INSIDE READ DATA-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);
        //    // Read data from the device
        //    while (!BluetoothSocket.InputStream.CanRead)
        //    {
        //        Debug.WriteLine("------------------------------------------------");
        //    }

        //    Debug.WriteLine("---------Bluetooth Socket READ Buffer-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);
        //    var mReader = new InputStreamReader(BluetoothSocket.InputStream);
        //    var buffer = new BufferedReader(mReader);
        //    //Stream inStream = BluetoothSocket.InputStream;
        //    int readByte = buffer.Read(chr, 0, chr.Length);

        //    //int readByte = await BluetoothSocket.InputStream.ReadAsync(rbuffer, 0, rbuffer.Length);
        //    //int readByte =  BluetoothSocket.InputStream.Read(rbuffer, 0, rbuffer.Length);
        //    RetArray = new byte[readByte];
        //    Array.Copy(rbuffer, 0, RetArray, 0, readByte);

        //    //Console.WriteLine(Array2Text(RetArray, false) + "\n" + "[" + RetArray.Length.ToString() + "]");

        //    Debug.WriteLine("--------- BT READ DATA RESPONSE-------" + ByteArrayToString(RetArray), DebugTag);
        //    // Debug.WriteLine("--------- BT READ DATA RESPONSE TIME-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);

        //    return RetArray;
        //}

        public async Task<byte[]> GetUSBCommand()
        {
            byte[] rbuffer = new byte[1024];
           

            Debug.WriteLine("---------USB INSIDE READ DATA-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);

            //var ss = USBport.Read(byteRes, 100);
           
            //var response = await tskCmSsrc.Task;

            // handle incoming data.
            var len = USBport.Read(rbuffer, 0);//TBD CHECK
            if (len > 0)
            {
                byte[] RetArray = new byte[len];
                Array.Copy(rbuffer, RetArray, len);

                //DataReceived.Raise(this, new SerialDataReceivedArgs(data));
                Debug.WriteLine("---------USB READ DATA RESPONSE-------" + ByteArrayToString(RetArray), DebugTag);
               // Debug.WriteLine("---------USB READ DATA RESPONSE TIME-------" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), DebugTag);
                return RetArray;
            }

            return null;
        }
        public bool USB_Disconnect()
        {
            return true;
        }
        public bool Wifi_Disconnect()
        {
            return true;
        }

        #endregion

        #region Helper
        //Convert a string of hex digits (example: E1 FF 1B) to a byte array. 
        //The string containing the hex digits (with or without spaces)
        //Returns an array of bytes. </returns>
        //private byte[] HexStringToByteArray(string s)
        //{
        //    s = s.Replace(" ", "");
        //    byte[] buffer = new byte[s.Length / 2];
        //    for (int i = 0; i < s.Length; i += 2)
        //        buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
        //    return buffer;
        //}
        static byte[] HexToBytes(string input)
        {
            byte[] result = new byte[input.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(input.Substring(2 * i, 2), 16);
            }
            return result;
        }

        private void WriteConsole(string input, string output)
        {
            Debug.WriteLine("Command = " + input + "\n" + "Output = " + output, DebugTag);
        }

        private byte[] HexStringToByteArray(String hex)
        {
            hex = hex.Replace(" ", "");
            int numberChars = hex.Length;
            if (numberChars % 2 != 0)
            {
                hex = "0" + hex;
                numberChars++;
            }
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        //----------------------------------------------------------------------------
        // Method Name   : ByteArrayToString
        // Input         : byte array
        // Output        : string
        // Purpose       : Function to convert byte array to string 
        // Date          : 27Sept16
        //----------------------------------------------------------------------------
        private string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
        //----------------------------------------------------------------------------
        // Method Name   : BT_GetDevices
        // Input         : NA
        // Output        : Collection of Bluetoothdevices with their status,name, address.
        // Purpose       : Get list of all devices with a specific BT/BLE name and with status paired and unpaired
        // Date          : 20-08-20
        //----------------------------------------------------------------------------

        #endregion

        #region WifiServices
        private object Wifi_GetDevices()
        {
            throw new NotImplementedException();
        }

        //private object Wifi_ConnectAP()
        //{
        //    tcpClient.StringEncoder = Encoding.UTF8;
        //    tcpClient.DataReceived += TcpClient_DataReceived;

        //    var clientStatus = tcpClient.Connect("Host", Convert.ToInt32("Port"));
        //    if (clientStatus.TcpClient.Connected == true)
        //    {
        //        ActiveConnection = Wifi;
        //        return true;
        //    }
        //    return false;
        //}

        private object Wifi_WriteSSIDPW()
        {
            throw new NotImplementedException();
        }

        private object Wifi_ConnectStation()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DongleServices

        public async Task<object> SecurityAccess()
        {
            Debug.WriteLine("------SecurityAccess------", DebugTag);

            string command = "500C47568AFE56214E238000FFC3";
            var bytesCommand = HexStringToByteArray(command);

            //SerialPortDataReceived(this, null);
            var response = await SendCommand(bytesCommand, (obj) => { WriteConsole(command, obj); }); ;

            return response;
        }
        //----------------------------------------------------------------------------
        // Method Name   : Dongle_Reset
        // Input         : NA
        // Output        : object
        // Purpose       : Reset all vehicle Communication related parameters to its default value
        // Date          : 20-08-20
        //----------------------------------------------------------------------------
        public async Task<object> Dongle_Reset()
        {
            Debug.WriteLine("------Inside Dongle_Reset------", DebugTag);
            //"0x20-0x01-0x01"
            string command = "200301";
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2]).ToString("x2");
            byte[] sendBytes = HexStringToByteArray(command + crc);
            if (crc.Length == 3)
                crc = "0" + crc;
            var response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        //----------------------------------------------------------------------------
        // Method Name   : Dongle_SetProtocol
        // Input         : NA
        // Output        : object
        // Purpose       : Set Vehicle Communication Protocol
        // Date          : 20-08-20
        //----------------------------------------------------------------------------
        public async Task<object> Dongle_SetProtocol(int protocolVersion)
        {
            object response = null;
            //   "0x20-0x02-0x02-0x < protocol > "

            Debug.WriteLine("------Dongle_SetProtocol------", DebugTag);
            string command = "200402" + protocolVersion.ToString("D2");
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2], bytesCommand[3]).ToString("x2");
            if (crc.Length == 3)
                crc = "0" + crc;
            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        //----------------------------------------------------------------------------
        // Method Name   : Dongle_GetProtocol
        // Input         : NA
        // Output        : object
        // Purpose       : Get Vehicle Communication Protocol
        // Date          : 20-08-20
        //----------------------------------------------------------------------------
        public async Task<object> Dongle_GetProtocol()
        {
            Debug.WriteLine("------Dongle_GetProtocol------", DebugTag);
            object response = null;
            //"0x20-0x01-0x03"

            string command = "200303";
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2]).ToString("x2");
            if (crc.Length == 3)
                crc = "0" + crc;
            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        //----------------------------------------------------------------------------
        // Method Name   : Dongle_GetFimrwareVersion
        // Input         : NA
        // Output        : object
        // Purpose       : Get firmware version
        // Date          : 20-08-20
        //----------------------------------------------------------------------------
        public async Task<object> Dongle_GetFimrwareVersion()
        {
            Debug.WriteLine("------Dongle_GetFimrwareVersion------", DebugTag);
            object response = null;
            string command = "200314";
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2]).ToString("x2");
            if (crc.Length == 3)
                crc = "0" + crc;
            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }


        #endregion

        #region CANMethods
        public async Task<object> CAN_SetTxHeader(string txHeader)
        {
            Debug.WriteLine("------CAN_SetTxHeader------", DebugTag);
            //            "if protocol is one of the following: ISO15765-250KB-11BIT-CAN, ISO15765-500KB-11BIT-CAN, ISO15765-1MB-11BIT-CAN, 250KB-11BIT-CAN, 500KB-11BIT-CAN, 1MB-11BIT-CAN, OE-IVN-250KBPS-11BIT-CAN, OE-IVN-500KBPS-11BIT-CAN,OE-IVN-1MBPS-11BIT-CAN 
            //0x20-0x03-0x04-0xxx-0xyy

            //if protocol is one of the following: ISO15765 - 250KB - 29BIT - CAN, ISO15765 - 500KB - 29BIT - CAN, ISO15765 - 1MB - 29BIT - CAN, 250KB - 29BIT - CAN, 500KB - 29BIT - CAN, 1MB - 29BIT - CAN, OE - IVN - 250KBPS - 29BIT - CAN, OE - IVN - 500KBPS - 29BIT - CAN,OE - IVN - 1MBPS - 29BIT - CAN
            //0x20-0x05-0x04-0xpp-0xqq-0xrr-0xss"

            object response = null;
            string command = string.Empty;
            byte[] sendBytes = null;
            if (protocol == Protocol.ISO15765_250KB_11BIT_CAN || protocol == Protocol.ISO15765_500KB_11BIT_CAN || protocol == Protocol.ISO15765_1MB_11BIT_CAN || protocol == Protocol.I250KB_11BIT_CAN || protocol == Protocol.I500KB_11BIT_CAN || protocol == Protocol.I1MB_11BIT_CAN || protocol == Protocol.OE_IVN_250KBPS_11BIT_CAN || protocol == Protocol.OE_IVN_500KBPS_11BIT_CAN || protocol == Protocol.OE_IVN_1MBPS_11BIT_CAN)
            {
                command = "200504" + txHeader.ToString();
                var bytesCommand = HexStringToByteArray(command);
                string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2], bytesCommand[3], bytesCommand[4]).ToString("x2");
                if (crc.Length == 3)
                    crc = "0" + crc;
                sendBytes = HexStringToByteArray(command + crc);
            }
            else if (protocol == Protocol.ISO15765_250Kb_29BIT_CAN || protocol == Protocol.ISO15765_500KB_29BIT_CAN || protocol == Protocol.ISO15765_1MB_29BIT_CAN || protocol == Protocol.I250Kb_29BIT_CAN || protocol == Protocol.I500KB_29BIT_CAN || protocol == Protocol.I1MB_29BIT_CAN || protocol == Protocol.OE_IVN_250KBPS_29BIT_CAN || protocol == Protocol.OE_IVN_500KBPS_29BIT_CAN || protocol == Protocol.OE_IVN_1MBPS_29BIT_CAN)
            {
                command = "200704" + txHeader.ToString();
                var bytesCommand = HexStringToByteArray(command);
                string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2], bytesCommand[3], bytesCommand[4], bytesCommand[5], bytesCommand[6]).ToString("x2");
                if (crc.Length == 3)
                    crc = "0" + crc;
                sendBytes = HexStringToByteArray(command + crc);
            }
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });

            return response;
        }

        public async Task<object> CAN_GetTxHeader()
        {
            Debug.WriteLine("------Dongle_SetProtocol------", DebugTag);
            object response = null;
            //"0x20-0x01-0x05"
            string command = "200305";
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2]).ToString("x2");
            if (crc.Length == 3)
                crc = "0" + crc;
            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> CAN_SetRxHeaderMask(string rxhdrmsk)
        {
            Debug.WriteLine("------CAN_SetRxHeaderMask------", DebugTag);
            object response = null;
            //            "if protocol is one of the following: ISO15765-250KB-11BIT-CAN, ISO15765-500KB-11BIT-CAN, ISO15765-1MB-11BIT-CAN, 250KB-11BIT-CAN, 500KB-11BIT-CAN, 1MB-11BIT-CAN, OE-IVN-250KBPS-11BIT-CAN, OE-IVN-500KBPS-11BIT-CAN,OE-IVN-1MBPS-11BIT-CAN 
            //0x20-0x03-0x06-0xxx-0xyy

            //if protocol is one of the following: ISO15765 - 250KB - 29BIT - CAN, ISO15765 - 500KB - 29BIT - CAN, ISO15765 - 1MB - 29BIT - CAN, 250KB - 29BIT - CAN, 500KB - 29BIT - CAN, 1MB - 29BIT - CAN, OE - IVN - 250KBPS - 29BIT - CAN, OE - IVN - 500KBPS - 29BIT - CAN,OE - IVN - 1MBPS - 29BIT - CAN
            //0x20-0x05-0x06-0xpp-0xqq-0xrr-0xss"

            string command = string.Empty;
            byte[] sendBytes = null;
            if (protocol == Protocol.ISO15765_250KB_11BIT_CAN || protocol == Protocol.ISO15765_500KB_11BIT_CAN || protocol == Protocol.ISO15765_1MB_11BIT_CAN || protocol == Protocol.I250KB_11BIT_CAN || protocol == Protocol.I500KB_11BIT_CAN || protocol == Protocol.I1MB_11BIT_CAN || protocol == Protocol.OE_IVN_250KBPS_11BIT_CAN || protocol == Protocol.OE_IVN_500KBPS_11BIT_CAN || protocol == Protocol.OE_IVN_1MBPS_11BIT_CAN)
            {
                command = "200506" + rxhdrmsk.ToString();
                var bytesCommand = HexStringToByteArray(command);
                string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2], bytesCommand[3], bytesCommand[4]).ToString("x2");
                if (crc.Length == 3)
                    crc = "0" + crc;
                sendBytes = HexStringToByteArray(command + crc);
            }
            else if (protocol == Protocol.ISO15765_250Kb_29BIT_CAN || protocol == Protocol.ISO15765_500KB_29BIT_CAN || protocol == Protocol.ISO15765_1MB_29BIT_CAN || protocol == Protocol.I250Kb_29BIT_CAN || protocol == Protocol.I500KB_29BIT_CAN || protocol == Protocol.I1MB_29BIT_CAN || protocol == Protocol.OE_IVN_250KBPS_29BIT_CAN || protocol == Protocol.OE_IVN_500KBPS_29BIT_CAN || protocol == Protocol.OE_IVN_1MBPS_29BIT_CAN)
            {
                command = "200706" + rxhdrmsk.ToString();
                var bytesCommand = HexStringToByteArray(command);
                string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2], bytesCommand[3], bytesCommand[4], bytesCommand[5], bytesCommand[6]).ToString("x2");
                if (crc.Length == 3)
                    crc = "0" + crc;
                sendBytes = HexStringToByteArray(command + crc);
            }
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;

        }

        public async Task<object> CAN_GetRxHeaderMask()
        {
            Debug.WriteLine("------CAN_GetRxHeaderMask------", DebugTag);
            object response = null;
            //"0x20-0x01-0x07"

            string command = "200307";
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2]).ToString("x2");
            if (crc.Length == 3)
                crc = "0" + crc;
            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> CAN_SetP1Min(string p1min)
        {
            Debug.WriteLine("------CAN_GetRxHeaderMask------", DebugTag);
            object response = null;
            //"0x20-0x02-0x0c-0x < xx > "

            string command = "20040c" + p1min.ToString();
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2], bytesCommand[3]).ToString("x2");
            if (crc.Length == 3)
                crc = "0" + crc;
            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> CAN_GetP1Min()
        {
            Debug.WriteLine("------CAN_GetP1Min------", DebugTag);
            object response = null;
            //"0x20-0x01-0x0d"

            string command = "20030d";
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2]).ToString("x2");
            byte[] sendBytes = HexStringToByteArray(command + crc);
            if (crc.Length == 3)
                crc = "0" + crc;
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> CAN_SetP2Max(string p2max)
        {
            Debug.WriteLine("------CAN_SetP2Max------", DebugTag);
            object response = null;
            //"0x20-0x03-0x0e-0x < xx >-0x < yy > "

            string command = "20050e" + p2max.ToString();
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2], bytesCommand[3], bytesCommand[4]).ToString("x2");
            if (crc.Length == 3)
                crc = "0" + crc;
            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> CAN_GetP2Max()
        {
            Debug.WriteLine("------CAN_GetP2Max------", DebugTag);
            object response = null;
            //"0x20-0x01-0x0f"

            string command = "20030f";
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2]).ToString("x2");
            if (crc.Length == 3)
                crc = "0" + crc;
            byte[] sendBytes = HexStringToByteArray(command + crc);

            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> CAN_StartTP()
        {
            Debug.WriteLine("------CAN_StartTP------", DebugTag);
            object response = null;
            //"0x20-0x01-0x10"

            string command = "200310";
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2]).ToString("x2");
            if (crc.Length == 3)
                crc = "0" + crc;
            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> CAN_StopTP()
        {
            Debug.WriteLine("------CAN_StopTP------", DebugTag);
            object response = null;
            //"0x20,0x01, 0x11"

            string command = "200311";
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2]).ToString("x2");
            if (crc.Length == 3)
                crc = "0" + crc;
            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> CAN_StartPadding(string paddingByte)
        {
            Debug.WriteLine("------CAN_StartPadding------", DebugTag);
            object response = null;
            //"0x20,0x02,0x12, 0x < xx > "

            string command = "200412" + paddingByte.ToString();
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2], bytesCommand[3]).ToString("x2");
            if (crc.Length == 3)
                crc = "0" + crc;

            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> CAN_StopPadding()
        {
            Debug.WriteLine("------CAN_StopPadding------", DebugTag);
            object response = null;
            //"0x20- 0x01-0x13"

            string command = "200313";
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2]).ToString("x2");
            if (crc.Length == 3)
                crc = "0" + crc;

            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> CAN_TxData(string txdata)
        {
            Debug.WriteLine("------CAN_StartPadding------", DebugTag);
            object response = null;
            ////data requests are of 2 types. If length of message <1000> then use 4x command, if not, use 1x command
            //0x4 < l >
            //0x < ll >
            string command = "40" + txdata.ToString();
            var bytesCommand = HexStringToByteArray(command);
            var crcBytesComputation = HexStringToByteArray(txdata);
            string crc = Crc16CcittKermit.ComputeChecksum(crcBytesComputation).ToString("x2");
            if (crc.Length == 3)
            {
                crc = "0" + crc;
            }
            byte[] sendBytes = HexStringToByteArray(command + crc);

            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;

        }

        //public async Task<object> CAN_RxData()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<ResponseArrayStatus> CAN_TxRx(int framelength, string txdata)
        {
            Debug.WriteLine("------Start CAN_TxRx------", DebugTag);
            object response = null;
            int dataLength = framelength + 2; //crc
            string command = string.Empty;
          
            var firstbyte = 0x40 | ((dataLength >> 8) & 0x0f);
            var secondbyte = dataLength & 0xff;

            command = firstbyte.ToString("X2") + secondbyte.ToString("X2") + txdata.ToString();
            var bytesCommand = HexStringToByteArray(command);
            var crcBytesComputation = HexStringToByteArray(txdata);
            string crc = Crc16CcittKermit.ComputeChecksum(crcBytesComputation).ToString("X2");
            if (crc.Length == 3)
            {
                crc = "0" + crc;
            }
            if (crc.Length == 2)
            {
                crc = "00" + crc;
            }
            if (crc.Length == 1)
            {
                crc = "000" + crc;
            }
            Debug.WriteLine("CRC =" + crc, DebugTag);
            byte[] sendBytes = HexStringToByteArray(command + crc);

            UInt16 nooftimessent = 0;

        STARTOVERAGAIN:
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            nooftimessent++;

            if (response != null)
            {
                var ecuResponseBytes = (byte[])response;

                ResponseArrayDecoding.CheckResponse(ecuResponseBytes, sendBytes, out byte[] actualDataBytes, out string dataStatus);

                if (dataStatus == "SENDAGAIN")
                {
                    while (dataStatus == "SENDAGAIN")
                    {
                        Debug.WriteLine("------SENDAGAIN ------" + Convert.ToString(nooftimessent), DebugTag);
                        
                        if(nooftimessent <=5)
                        {
                            goto STARTOVERAGAIN;
                        }
                        else
                        {
                            /* stop sending again - Problem with the device */
                            responseStructure = new ResponseArrayStatus
                            {
                                ECUResponse = ecuResponseBytes,
                                ECUResponseStatus = "DONGLEERROR_SENDAGAINTHRESHOLDCROSSED",
                                ActualDataBytes = actualDataBytes
                            };
                            break;
                        }
                        
                        //response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
                        //if (response != null)
                        //{
                        //    var ecuResponseBytes2 = (byte[])response;
                        //    ResponseArrayDecoding.CheckResponse(ecuResponseBytes2, sendBytes, out byte[] actualDataBytes2, out string dataStatus2);
                        //    dataStatus = dataStatus2;
                        //    responseStructure = new ResponseArrayStatus
                        //    {
                        //        ECUResponse = ecuResponseBytes2,
                        //        ECUResponseStatus = dataStatus2,
                        //        ActualDataBytes = actualDataBytes2
                        //    };
                        //    Debug.WriteLine("------SENDAGAIN DATA START ------", DebugTag);
                        //    if (responseStructure?.ECUResponse != null)
                        //        Debug.WriteLine("------ECUResponse ------" + ByteArrayToString(ecuResponseBytes2), DebugTag);
                        //    if (responseStructure?.ActualDataBytes != null)
                        //        Debug.WriteLine("------ActualDataBytes ------" + ByteArrayToString(responseStructure?.ActualDataBytes), DebugTag);
                        //    Debug.WriteLine("------ECUResponseStatus ------" + responseStructure.ECUResponseStatus, DebugTag);
                        //    Debug.WriteLine("------SENDAGAIN DATA END ------", DebugTag);
                        //}
                    }
                }
                else if (dataStatus == "READAGAIN")
                {
                    while (dataStatus == "READAGAIN")
                    {
                        var responseReadAgain = await ReadData();
                        var ecuResponseReadBytes = (byte[])responseReadAgain;
                        ResponseArrayDecoding.CheckResponse(ecuResponseReadBytes, sendBytes, out byte[] actualReadBytes, out string dataReadStatus);
                        dataStatus = dataReadStatus;
                        responseStructure = new ResponseArrayStatus
                        {
                            ECUResponse = ecuResponseReadBytes,
                            ECUResponseStatus = dataReadStatus,
                            ActualDataBytes = actualReadBytes
                        };
                        Debug.WriteLine("------EXTRA READ DATA START ------", DebugTag);
                        if (responseStructure?.ECUResponse != null)
                            Debug.WriteLine("------ECUResponse ------" + ByteArrayToString(ecuResponseReadBytes), DebugTag);
                        if (responseStructure?.ActualDataBytes != null)
                            Debug.WriteLine("------ActualDataBytes ------" + ByteArrayToString(responseStructure?.ActualDataBytes), DebugTag);
                        Debug.WriteLine("------ECUResponseStatus ------" + responseStructure.ECUResponseStatus, DebugTag);
                        Debug.WriteLine("------EXTRA READ DATA END ------", DebugTag);
                    }
                }
                else
                {
                    responseStructure = new ResponseArrayStatus
                    {
                        ECUResponse = ecuResponseBytes,
                        ECUResponseStatus = dataStatus,
                        ActualDataBytes = actualDataBytes
                    };
                    Debug.WriteLine("------ECU RESPONE START ------", DebugTag);
                    if (responseStructure?.ECUResponse != null)
                        Debug.WriteLine("------ECUResponse ------" + ByteArrayToString(responseStructure?.ECUResponse), DebugTag);
                    if (responseStructure?.ActualDataBytes != null)
                        Debug.WriteLine("------ActualDataBytes ------" + ByteArrayToString(responseStructure?.ActualDataBytes), DebugTag);
                    Debug.WriteLine("------ECUResponseStatus ------" + responseStructure.ECUResponseStatus, DebugTag);
                    Debug.WriteLine("------ECU RESPONE END ------", DebugTag);
                }
            }
            return responseStructure;

        }

        //public async Task<ResponseArrayStatus> CAN_TxRx(int framelength, string txdata)
        //{
        //    Debug.WriteLine("------Start CAN_TxRx------", DebugTag);
        //    object response = null;
        //    int dataLength = framelength + 2;
        //    string command = "40" + dataLength.ToString("X2") + txdata.ToString();
        //    var bytesCommand = HexStringToByteArray(command);
        //    var crcBytesComputation = HexStringToByteArray(txdata);
        //    string crc = Crc16CcittKermit.ComputeChecksum(crcBytesComputation).ToString("X2");
        //    if (crc.Length == 3)
        //    {
        //        crc = "0" + crc;
        //    }
        //    byte[] sendBytes = HexStringToByteArray(command + crc);

        //    response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });

        //    if (response != null)
        //    {
        //        var ecuResponseBytes = (byte[])response;
        //        ResponseArrayDecoding.CheckResponse(ecuResponseBytes, sendBytes, out byte[] actualDataBytes, out string dataStatus);
        //        if (dataStatus == "READAGAIN")
        //            while (dataStatus == "READAGAIN")
        //            {
        //                var responseReadAgain = await ReadData();
        //                var ecuResponseReadBytes = (byte[])responseReadAgain;
        //                ResponseArrayDecoding.CheckResponse(ecuResponseReadBytes, sendBytes, out byte[] actualReadBytes, out string dataReadStatus);
        //                dataStatus = dataReadStatus;
        //                responseStructure = new ResponseArrayStatus
        //                {
        //                    ECUResponse = ecuResponseReadBytes,
        //                    ECUResponseStatus = dataReadStatus,
        //                    ActualDataBytes = actualReadBytes


        //                };
        //                Debug.WriteLine("------READ DATA START ------", DebugTag);
        //                if (responseStructure?.ECUResponse != null)
        //                    Debug.WriteLine("------ECUResponse ------" + ByteArrayToString(ecuResponseReadBytes), DebugTag);
        //                if (responseStructure?.ActualDataBytes != null)
        //                    Debug.WriteLine("------ActualDataBytes ------" + ByteArrayToString(responseStructure?.ActualDataBytes), DebugTag);
        //                Debug.WriteLine("------ECUResponseStatus ------" + responseStructure.ECUResponseStatus, DebugTag);
        //                Debug.WriteLine("------READ DATA END ------", DebugTag);
        //            }
        //        else
        //        {

        //            responseStructure = new ResponseArrayStatus
        //            {
        //                ECUResponse = ecuResponseBytes,
        //                ECUResponseStatus = dataStatus,
        //                ActualDataBytes = actualDataBytes
        //            };
        //            Debug.WriteLine("------ECU RESPONE WITHOUT READ START ------", DebugTag);
        //            if (responseStructure?.ECUResponse != null)
        //                Debug.WriteLine("------ECUResponse ------" + ByteArrayToString(responseStructure?.ECUResponse), DebugTag);
        //            if (responseStructure?.ActualDataBytes != null)
        //                Debug.WriteLine("------ActualDataBytes ------" + ByteArrayToString(responseStructure?.ActualDataBytes), DebugTag);
        //            Debug.WriteLine("------ECUResponseStatus ------" + responseStructure.ECUResponseStatus, DebugTag);
        //            Debug.WriteLine("------ECU RESPONE WITHOUT READ END ------", DebugTag);

        //        }
        //    }

        //    return responseStructure;

        //}

        //public async Task<object> CAN_TxRx(int frameLength, string txdata)
        //{
        //    Debug.WriteLine("------CAN_StartPadding------", DebugTag);
        //    object response = null;

        //    string command = "40" + txdata.ToString();
        //    var bytesCommand = HexStringToByteArray(command);
        //    string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2], bytesCommand[3], bytesCommand[4]).ToString("x2");
        //    byte[] sendBytes = HexStringToByteArray(command + crc);

        //    response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); }); ;

        //    return response;

        //}   

        public async Task<object> SetBlkSeqCntr(string blklen)
        {
            Debug.WriteLine("------SetBlkSeqCntr------", DebugTag);
            object response = null;
            //"0x20- 0x02 - 0x08- 0xxx(0x00 to 0x40)"

            string command = "200408" + blklen.ToString();
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2], bytesCommand[3]).ToString("x2");
            if (crc.Length == 3)
            {
                crc = "0" + crc;
            }
            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> GetBlkSeqCntr()
        {
            Debug.WriteLine("------GetBlkSeqCntr------", DebugTag);
            object response = null;
            //"0x20- 0x01-0x09"
            string command = "200309";
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2]).ToString("x2");
            if (crc.Length == 3)
            {
                crc = "0" + crc;
            }
            byte[] sendBytes = HexStringToByteArray(command + crc);

            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> SetSepTime(string septime)
        {
            Debug.WriteLine("------SetSepTime------", DebugTag);
            object response = null;
            //"0x20-0x02-0x0A-0xxx"

            string command = "20040A" + septime.ToString();
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2], bytesCommand[3]).ToString("x2");
            if (crc.Length == 3)
            {
                crc = "0" + crc;
            }
            byte[] sendBytes = HexStringToByteArray(command + crc);

            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); });
            return response;
        }

        public async Task<object> GetSepTime()
        {
            Debug.WriteLine("------GetSepTime------", DebugTag);
            object response = null;
            //"0x20-0x01-0x0B"
            string command = "20030B";
            var bytesCommand = HexStringToByteArray(command);
            string crc = Crc16CcittKermit.ComputeChecksum(bytesCommand[2]).ToString("x2");
            if (crc.Length == 3)
            {
                crc = "0" + crc;
            }
            byte[] sendBytes = HexStringToByteArray(command + crc);
            response = await SendCommand(sendBytes, (obj) => { WriteConsole(command, obj); }); ;

            return response;
        }
        #endregion

        #endregion
    }
}
