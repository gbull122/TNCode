using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock.Layout;

namespace TNCodeApp.Docking
{
    public class DocumentViewModel
    {

        private LayoutAnchorable hostControl;

        public object HostControl
        {
            get
            {
                return hostControl;
            }
            set
            {
                hostControl = value as LayoutAnchorable;
            }
        }
    }
}
