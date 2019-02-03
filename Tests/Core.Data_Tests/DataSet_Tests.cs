using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TNCode.Core.Data;

namespace Core.Data_Tests
{
    [TestClass]
    public class DataSet_Tests
    {
        [TestMethod]
        public void DataSet_FromArray_Test()
        {
            var expectedName = "test";
            var expectedVariableCount = 2;
            var expectedVariableLength = 3;

            object[,] data = new object[,] { { "A", "B" }, { 3, 4 }, { 5, 6 }, { 7, 8 } };

            List<object[,]> rawData = new List<object[,]> { data };

            var dataSet = new DataSet(rawData, "test");

            dataSet.Variables.Count.Should().Be(expectedVariableCount);
            dataSet.Variables[1].Length.Should().Be(expectedVariableLength);
            dataSet.Name.Should().Be(expectedName);

        }

        [TestMethod]
        public void DataSet_FromList_Test()
        {
            var expectedName = "test";
            var expectedVariableCount = 2;
            var expectedVariableLength = 3;
            var expectedNames = new List<string>() { "A", "B" };

            List<string[]> rawData = new List<string[]>() {
                new string[]{"A", "3","5","7" },
                new string[]{ "B", "4" ,"6","7"}};

            var dataSet = new DataSet(rawData, "test");

            dataSet.Variables.Count.Should().Be(expectedVariableCount);
            dataSet.Variables[1].Length.Should().Be(expectedVariableLength);
            dataSet.VariableNames()[0].Should().Be(expectedNames[0]);
            dataSet.VariableNames()[1].Should().Be(expectedNames[1]);
            dataSet.Name.Should().Be(expectedName);

        }
    }
}
