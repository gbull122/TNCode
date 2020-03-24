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
            var selectedVariable = new List<string>() { "Variable1" };

            var dataSet = A.Fake<IDataSet>();
            A.CallTo(() => dataSet.IsSelected).Returns(false);
            A.CallTo(() => dataSet.SelectedVariableNames()).Returns(selectedVariable);
            A.CallTo(() => dataSet.Name).Returns("Name");

            var dataSetManager = new DataSetsManager();
            dataSetManager.DataSets.Add(dataSet);

            var actualSelectedData = dataSetManager.SelectedData();

            actualSelectedData.Count().Should().Be(expectedCount);
        }

        [TestMethod]
        public void RowWiseToColumnWise_Test()
        {
            var expectedData = new List<string[]>()
            {
                new string[]{"Header1", "R1C1","R2C1","R3C1" },
                new string[]{"Header2", "R1C2","R2C2","R3C2" },
                new string[]{"Header3", "R1C3","R2C3","R3C3" },
                new string[]{"Header4", "R1C4", "R2C4", "R3C4" }
            };

            var testData = new List<string[]>()
            {
                new string[]{"Header1","Header2","Header3","Header4" },
                new string[]{"R1C1","R1C2","R1C3","R1C4" },
                new string[]{ "R2C1", "R2C2", "R2C3", "R2C4" },
                new string[]{ "R3C1", "R3C2", "R3C3", "R3C4" }
            };

            var dataSetManager = new DataSetsManager();
            var actualData = dataSetManager.RowWiseToColumnWise(testData);

            actualData[0].Should().Equal(expectedData[0]);
            actualData[1].Should().Equal(expectedData[1]);
            actualData[2].Should().Equal(expectedData[2]);
            actualData[3].Should().Equal(expectedData[3]);
        }

    }
}
