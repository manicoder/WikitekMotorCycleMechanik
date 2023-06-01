using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.Interfaces
{
    public interface IToolbarItemBadgeService
    {
        void SetBadge(Page page, ToolbarItem item, string value, Color backgroundColor, Color textColor);
    }
}
