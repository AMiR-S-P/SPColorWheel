using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace SP_Color_Wheel.Extensions
{
    public class EnumToListExtension : MarkupExtension
    {
        private Type type;

        public EnumToListExtension(Type type)
        {
            this.type = type;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(type)
                    .Cast<object>()
                    .Select(x => new { Value = x, DisplayName = x.ToString(),Index = (int)x });
        }
    }
}
