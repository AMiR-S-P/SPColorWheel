using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Color_Wheel.Interfaces
{
    public interface ISettingsHelper
    {
        void SaveSettings();
        void LoadSettings();
    }
}
