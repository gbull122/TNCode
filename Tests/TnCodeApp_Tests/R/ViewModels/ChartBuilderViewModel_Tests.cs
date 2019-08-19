using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using Prism.Regions;
using TNCode.Core.Data;
using TNCodeApp.R;
using TNCodeApp.R.ViewModels;

namespace TnCodeApp_Tests.R.ViewModels
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
