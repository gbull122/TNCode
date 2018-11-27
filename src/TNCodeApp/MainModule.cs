using Gemini.Framework;
using System.ComponentModel.Composition;
using System.Windows;

namespace TNCodeApp
{
    [Export(typeof(IModule))]
    public class MainModule : ModuleBase
    {

        [ImportingConstructor]
        public MainModule()
        {

        }

        public override void Initialize()
        {
            Shell.ShowFloatingWindowsInTaskbar = true;
            Shell.ToolBars.Visible = true;

            MainWindow.WindowState = WindowState.Maximized;
            MainWindow.Title = "TNCode";

        }
    }
}
