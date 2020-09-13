using System;
using System.Collections.Generic;
using System.Text;

namespace TnCode.TnCodeApp.Docking
{
    public enum DockingMethod
    {
        Document,
        StatusPanel,
        ControlPanel
    }

    interface IDockingPanel
    {
        string Title { get; }

        DockingMethod Docking { get; }
    }
}
