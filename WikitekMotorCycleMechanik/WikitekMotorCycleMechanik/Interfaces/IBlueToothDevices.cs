using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;

namespace WikitekMotorCycleMechanik.Interfaces
{
    public interface IBlueToothDevices
    {
        Task SearchBlueTooth();
        void UnRegisterBluetoothReceiver();

        void CancleScanning();

        Task<string> ConnectionBT(string name, int sleepTime, bool readAsCharArray);

        Task<string> GetFirmwareVersion(string dongle_type, string txHeader, string rxHeader, string protocol, bool is_padding);

        void SendHeader(string txHeader,string rxHeader);

        Task<ReadDtcResponseModel> ReadDtc(string indexKey);

        Task<string> ClearDtc(string dtc_index);

        Task<string[]> GetGenericObdPid();

        Task<ObservableCollection<ReadPidPresponseModel>> ReadPid(ObservableCollection<PidCode> pidList);

        Task<string> SetProtocol(string protocol);


        void DisconnectDongle();
    }
}
