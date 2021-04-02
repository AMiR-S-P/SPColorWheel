using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace XamlAnalyzer.Behaviors
{
    public class XamlCommitedUIUpdateBehavior:Behavior<ContentPresenter>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            
        }
    }
}
