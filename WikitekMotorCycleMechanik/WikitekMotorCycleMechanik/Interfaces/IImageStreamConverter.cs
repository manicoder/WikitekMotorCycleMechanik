using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikitekMotorCycleMechanik.Interfaces
{
    public interface IImageStreamConverter
    {
        Task<byte[]> ImageStream();
    }
}
