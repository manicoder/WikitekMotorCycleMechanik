using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;

namespace WikitekMotorCycleMechanik.Interfaces
{
    public interface IPayment
    {
        Task<bool> StartPaymet(GenerateOrderIdResponseModel model);
    }
}
