using FakeItEasy;
using FluentAssertions;
using Microsoft.R.Host.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModuleR.R;
using Prism.Logging;
using System.Collections.Generic;

namespace ModuleR_Tests
{
    [TestClass]
    public class RManager_Tests
    {

        [TestMethod]
        public void ListOfVectorsToObject()
        {
            var rHostSession = A.Fake<IRHostSessionCallback>();
            var logger = A.Fake<ILoggerFacade>();

            var rManager = new RManager(rHostSession,logger);
            List<object> first = new List<object>() { "A","B","C"};
            List<object> second = new List<object>() { 1.2, 3.4, 5};

            List<List<object>> data = new List<List<object>>() { first, second };
            string[] names = new string[2] { "First", "Second" };

            var objectArray = rManager.ListOfVectorsToObject(data, names);

            objectArray.GetLength(0).Should().Be(4);
            objectArray.GetLength(1).Should().Be(2);
        }

        [TestMethod]
        public void test()
        {
            var rHostSession = A.Fake<IRHostSessionCallback>();
            var logger = A.Fake<ILoggerFacade>();

            var rManager = new RManager(rHostSession, logger);
        }
    }
}
