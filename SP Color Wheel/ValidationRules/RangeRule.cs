using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SP_Color_Wheel.ValidationRules
{
    public class RangeRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int val = 0;
            int.TryParse(value.ToString(), out val);

            if(val>Max || val<Min)
            {
                return new ValidationResult(false, "Value is out of range!");
            }
            return new ValidationResult(true,"");

        }
    }
}
