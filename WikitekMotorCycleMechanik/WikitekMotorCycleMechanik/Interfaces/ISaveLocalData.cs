﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikitekMotorCycleMechanik.Interfaces
{
    public interface ISaveLocalData
    {
        string GetData(string file_name);
        Task SaveData(string file_name, string Data);
    }
}
