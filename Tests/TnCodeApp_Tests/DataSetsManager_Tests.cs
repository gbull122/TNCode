using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TnCode.TnCodeApp.Data;

namespace TnCodeApp_Tests
{
    [TestClass]
    public class DataSetsManager_Tests
    {
        [TestMethod]
        public void WhenSingleRow_ThenRowWiseToColumnWiseReturnsSingleColumn()
        {
            var expectedElement1 = new string[] { "1" };
            var expectedElement2 = new string[] { "2" };
            var expectedElement3 = new string[] { "3" };

            var expectedData = new List<string[]>() { expectedElement1, expectedElement2 , expectedElement3 };

            var dataSetsManager = new DataSetsManager();
            var testData = new string[] { "1", "2", "3" };
            var rowWiseData = new List<string[]>() { testData };
            var actualData = dataSetsManager.RowWiseToColumnWise(rowWiseData);

            actualData.Should().NotBeEquivalentTo(rowWiseData);
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [TestMethod]
        public void WhenTwoRows_ThenRowWiseToColumnWiseReturnsTwoColumns()
        {
            var expectedElement1 = new string[] { "1","4" };
            var expectedElement2 = new string[] { "2","5" };
            var expectedElement3 = new string[] { "3","6" };

            var expectedData = new List<string[]>() { expectedElement1, expectedElement2, expectedElement3 };

            var dataSetsManager = new DataSetsManager();
            var testRow1 = new string[] { "1", "2", "3" };
            var testRow2 = new string[] { "4", "5", "6" };
            var rowWiseData = new List<string[]>() { testRow1, testRow2 };
            var actualData = dataSetsManager.RowWiseToColumnWise(rowWiseData);

            actualData.Should().NotBeEquivalentTo(rowWiseData);
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [TestMethod]
        public void WhenSingleColumn_ThenColumnWiseToRiseWiseReturnsSingleRow()
        {
            var expectedData = new List<string[]>() { new string[] { "1", "2", "3" } };

            var dataSetsManager = new DataSetsManager();

            var testElement1 = new string[] { "1" };
            var testElement2 = new string[] { "2" };
            var testElement3 = new string[] { "3" };
            var colWiseData = new List<string[]>() { testElement1, testElement2, testElement3 };

            var actualData = dataSetsManager.ColumnWiseToRowWise(colWiseData);

            actualData.Should().NotBeEquivalentTo(colWiseData);
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [TestMethod]
        public void WhenTwoColumns_ThenColumnWiseToRowWiseReturnsTwoRows()
        {
            var expectedRow1 = new string[] { "1", "2", "3" };
            var expectedRow2 = new string[] { "4", "5", "6" };

            var expectedData = new List<string[]>() { expectedRow1, expectedRow2};

            var dataSetsManager = new DataSetsManager();
           
            var testRow1 = new string[] { "1", "4" };
            var testRow2 = new string[] { "2", "5" };
            var testRow3 = new string[] { "3", "6" };

            var colWiseData = new List<string[]>() { testRow1, testRow2, testRow3 };
            var actualData = dataSetsManager.ColumnWiseToRowWise(colWiseData);

            actualData.Should().NotBeEquivalentTo(colWiseData);
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [TestMethod]
        public void TwoVariables_AsArray()
        {
            double[,] expected = { { 1, 2, 3 }, { 4, 5, 6 } };
        }
    }
}
