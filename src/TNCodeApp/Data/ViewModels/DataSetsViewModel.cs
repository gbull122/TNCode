using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.PropertyGrid;
using System.ComponentModel.Composition;

namespace TNCodeApp.Data.ViewModels
{
    [Export(typeof(IPropertyGrid))]
    public class DataSetsViewModel : Tool, IPropertyGrid
    {
        public object SelectedObject { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override PaneLocation PreferredLocation => throw new System.NotImplementedException();
    }
}
