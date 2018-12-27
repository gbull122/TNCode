using Prism.Mvvm;
using TNCodeApp.Menu;

namespace ModuleR.ViewModels
{
    public class RibbonRViewModel : BindableBase, ITnRibbon
    {
        public bool IsMainRibbon => false;
    }
}
