using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModuleR.R;
using ModuleR.ViewModels;
using Prism.Events;
using Prism.Regions;
using TNCode.Core.Data;

namespace ModuleR_Tests
{
    [TestClass]
    public class ChartBuilderViewModel_Tests
    {

        [TestMethod]
        public void Test()
        {

            var eventAggregator = A.Fake<IEventAggregator>();
            var regionManager = A.Fake<IRegionManager>();
            var rManager = A.Fake<IRManager>();
            var xmlConverter = A.Fake<IXmlConverter>();

            var chartBuilderViewModel = new ChartBuilderViewModel(eventAggregator, regionManager, rManager, xmlConverter);
        }
    }
}
