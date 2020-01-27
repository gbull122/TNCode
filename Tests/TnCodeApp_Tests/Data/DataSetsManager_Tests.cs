using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TNCode.Core.Data;
using TNCodeApp.Data;

namespace TnCodeApp_Tests.Data
{
    [TestClass]
    public class DataSetsManager_Tests
    {

        [TestMethod]
        public void SelectedData_ReturnsZero_WhenNothingSelected()
        {
            var expectedCount = 0;
            var selectedVariable = new List<string>();

            var dataSet = A.Fake<IDataSet>();
            A.CallTo(() => dataSet.IsSelected).Returns(false);
            A.CallTo(() => dataSet.SelectedVariableNames()).Returns(selectedVariable);

            var dataSetManager = A.Fake<IDataSetsManager>();
            dataSetManager.DataSets.Add(dataSet);

            var actualSelectedData = dataSetManager.SelectedData();

            actualSelectedData.Count().Should().Be(expectedCount);
        }

        [TestMethod]
        public void SelectedData_ReturnsVariable()
        {
            var expectedCount = 1;
            var selectedVariable = new List<string>() { "Variable1"};

            var dataSet = A.Fake<IDataSet>();
            A.CallTo(() => dataSet.IsSelected).Returns(false);
            A.CallTo(() => dataSet.SelectedVariableNames()).Returns(selectedVariable);
            A.CallTo(() => dataSet.Name).Returns("Name");

            var dataSetManager = new DataSetsManager();
            dataSetManager.DataSets.Add(dataSet);

            var actualSelectedData = dataSetManager.SelectedData();

            actualSelectedData.Count().Should().Be(expectedCount);
        }

    }
}
