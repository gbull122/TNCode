using FakeItEasy;
using Microsoft.R.Host.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TnCode.Core.R;

namespace Core.R_Tests
{
    [TestClass]
    public class RManager_Tests
    {

        //[TestMethod]
        //public void ListOfVectorsToObject()
        //{
        //    var rHostSessionCallback = A.Fake<IRHostSessionCallback>();
        //    var rHostSession = A.Fake<IRHostSession>();

        //    var rManager = new RManager(rHostSession, rHostSessionCallback);

        //    List<object> first = new List<object>() { "A", "B", "C" };
        //    List<object> second = new List<object>() { 1.2, 3.4, 5 };

        //    List<List<object>> data = new List<List<object>>() { first, second };
        //    string[] names = new string[2] { "First", "Second" };

        //    var objectArray = rManager.ListOfVectorsToObject(data, names);

        //    objectArray.GetLength(0).Should().Be(4);
        //    objectArray.GetLength(1).Should().Be(2);
        //}

        //[TestMethod]
        //public async Task DeleteVariablesAsync_Test()
        //{
        //    var rHostSessionCallback = A.Fake<IRHostSessionCallback>();
        //    var rHostSession = A.Fake<IRHostSession>();

        //    var rManager = new RManager(rHostSession, rHostSessionCallback);

        //    List<string> rVariables = new List<string>() { "A", "B", "C" };

        //    await rManager.DeleteVariablesAsync(rVariables);

        //    var deleteCommand = "rm(C)";
        //    A.CallTo(() => rOperations.ExecuteAsync(deleteCommand)).MustHaveHappened();
        //}

        //[TestMethod]
        //public async Task GetDataFrameAsync_Test()
        //{
        //    var rHostSessionCallback = A.Fake<IRHostSessionCallback>();
        //    var rHostSession = A.Fake<IRHostSession>();

        //    var rManager = new RManager(rHostSession, rHostSessionCallback);

        //    await rManager.GetDataFrameAsync("test");

        //    A.CallTo(() => rOperations.GetDataFrameAsync("test")).MustHaveHappened();

        //    var rowNames = new List<string>() { "A", "B" };
        //    var headers = new List<string>() { "HA", "HB" };

        //    List<object> first = new List<object>() { "3", "B", "y" };
        //    List<object> second = new List<object>() { 1.2, 3.4, 5 };

        //    List<List<object>> data = new List<List<object>>() { first, second };

        //    DataFrame dataFrame = new DataFrame(rowNames.AsReadOnly(), headers.AsReadOnly(), data.AsReadOnly());

        //    A.CallTo(() => rOperations.GetDataFrameAsync("test")).Returns(dataFrame);

        //    var actualDataFrame = await rManager.GetDataFrameAsync("test");
        //}

        //[TestMethod]
        //public async Task GetRListAsync_Test()
        //{
        //    var rHostSessionCallback = A.Fake<IRHostSessionCallback>();
        //    var rHostSession = A.Fake<IRHostSession>();

        //    var rManager = new RManager(rHostSession, rHostSessionCallback);

        //    var headers = new List<object>() { "HA", "HB" };
        //    A.CallTo(() => rOperations.GetListAsync("test")).Returns(headers);
        //}

        //[TestMethod]
        //public void MaximumLengthOfList_Test()
        //{
        //    var rHostSessionCallback = A.Fake<IRHostSessionCallback>();
        //    var rHostSession = A.Fake<IRHostSession>();

        //    var rManager = new RManager(rHostSession,rHostSessionCallback);

        //    var expectedMaxLength = 5;

        //    List<object> first = new List<object>() { "3", "B", "y", "r" };
        //    List<object> second = new List<object>() { 1.2, 3.4, 5 };
        //    List<object> third = new List<object>() { 1.2, 3.4, 5, 7, 9 };

        //    List<List<object>> data = new List<List<object>>() { first, second, third };

        //    var actualMaxLength = rManager.MaximumLengthOfList(data);

        //    actualMaxLength.Should().Be(expectedMaxLength);
        //}
    }
}
