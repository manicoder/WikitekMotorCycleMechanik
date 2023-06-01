using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Interfaces
{
    public interface IStatusBarStyleManager
    {
        void SetColoredStatusBar(string statusColor, string navBarColor);
        void SetWhiteStatusBar(string hexColor);
        void ChangeTheme(string user_type);
    }
}
