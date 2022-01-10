using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using Prism.Regions;
using System;
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
            IEventAggregator eventAggr = A.Fake<IEventAggregator>();
            IRegionManager regMngr = A.Fake<IRegionManager>();
            IRService rSer = A.Fake<IRService>();
            IDataSetsManager setsManager = A.Fake<IDataSetsManager>();
            IProgressService progService = A.Fake<IProgressService>();
            IXmlService xmlService = A.Fake<IXmlService>();
            //ILoggerFacade loggerFacade = A.Fake<ILoggerFacade>();


           //A.CallTo(()=> xmlService.LoadAesthetic(A._string)
            //var ggplotBuilder = new GgplotBuilderViewModel(eventAggr, regMngr, rSer, setsManager, progService, xmlService, loggerFacade);

            //ggplotBuilder.NewLayerCommand.Execute();
        }
    }
}
