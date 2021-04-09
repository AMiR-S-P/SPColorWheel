using SP_Color_Wheel.UserControls.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace SP_Color_Wheel.Behaviors
{
   
    class ColorCardLockUnlockBehavior:Behavior<Grid>// should be colorcard Container    like grid
    {
        string tempColor;
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Initialized += AssociatedObject_Initialized1; ;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Initialized -= AssociatedObject_Initialized1;
        }


        private void AssociatedObject_Initialized1(object sender, EventArgs e)
        {
            if (AssociatedObject.Children.Count > 0)
            {
                ColorCard main = AssociatedObject.Children[0] as ColorCard;
                ColorCard c1 = AssociatedObject.Children[1] as ColorCard;
                ColorCard c2 = AssociatedObject.Children[2] as ColorCard;
                ColorCard c3 = AssociatedObject.Children[3] as ColorCard;
                ColorCard c4 = AssociatedObject.Children[4] as ColorCard;

                main.IsLockedChanged += Main_IsLockedChanged;
                c1.IsLockedChanged += Main_IsLockedChanged;
                c2.IsLockedChanged += Main_IsLockedChanged;
                c3.IsLockedChanged += Main_IsLockedChanged;
                c4.IsLockedChanged += Main_IsLockedChanged;
            }
        }

      

        private void Main_IsLockedChanged(object sender, EventArguments.IsLockedChanged e)
        {
            if(e.IsLocked)
            {
                tempColor = ((ColorCard)sender).ColorCode;
                ((ColorCard)sender).DataContext = null;
                ((ColorCard)sender).CurrentColor = (Color)ColorConverter.ConvertFromString(tempColor?.ToString());
            }
            else
            {
                ((ColorCard)sender).DataContext = AssociatedObject.DataContext;
                //((ColorCard)sender).CurrentColor= (Color)ColorConverter.ConvertFromString(tempColor?.ToString());
            }
        }
    }
}
