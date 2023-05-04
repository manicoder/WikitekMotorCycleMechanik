using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Interfaces
{
    public interface IVersionAndBuildNumber
    {
        string GetVersionNumber();
        string GetBuildNumber();
    }
}
