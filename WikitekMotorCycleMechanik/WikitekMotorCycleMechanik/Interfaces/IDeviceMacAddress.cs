using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikitekMotorCycleMechanik.Interfaces
{
    public interface IDeviceMacAddress
    {
        Task<string> GetDeviceUniqueId();
        //Task<string> GetMacAddress();
        //Task<string> ReturnMacAddress();
        //Task<string> GetUniqID();
    }
}
