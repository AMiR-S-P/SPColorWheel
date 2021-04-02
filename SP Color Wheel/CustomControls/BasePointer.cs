using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SP_Color_Wheel.CustomControls
{
    public class BasePointer:Thumb
    {

        public BasePointer()
        {
            Background = Brushes.Transparent;

            HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;
        }


    }
}
