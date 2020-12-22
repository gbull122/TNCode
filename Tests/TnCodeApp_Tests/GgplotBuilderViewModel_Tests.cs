using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Prism.Regions;
using System;
using TnCode.Core.R.Charts.Ggplot;
using TnCode.Core.Utilities;
using TnCode.TnCodeApp.Data;
using TnCode.TnCodeApp.Progress;
using TnCode.TnCodeApp.R;
using TnCode.TnCodeApp.R.ViewModels;

namespace TnCodeApp_Tests
{
    [TestClass]
    public class GgplotBuilderViewModel_Tests
    {
        [TestMethod]
        [STAThread]
        public void NewLayer_GeneratesPointLayer()
        {
            IContainerExtension container = A.Fake<IContainerExtension>();
            IEventAggregator eventAggr = A.Fake<IEventAggregator>();
            IRegionManager regMngr = A.Fake<IRegionManager>();
            IRService rSer = A.Fake<IRService>();
            IDataSetsManager setsManager = A.Fake<IDataSetsManager>();
            IProgressService progService = A.Fake<IProgressService>();
            IXmlService xmlService = A.Fake<IXmlService>();
            ILoggerFacade loggerFacade = A.Fake<ILoggerFacade>();

            var ggplotBuilder = new GgplotBuilderViewModel(container, eventAggr, regMngr, rSer, setsManager, progService, xmlService, loggerFacade);

            ggplotBuilder.NewLayerCommand.Execute();
        }
    }
}
