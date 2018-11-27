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
        public void DataSet_Test()
        {
            var expectedName = "test";

            object[,] data = new object[,] { { "A", "B" }, { 3, 4 }, { 5, 6 }, { 7, 8 } };

            List<object[,]> rawData = new List<object[,]> { data };

            var dataSet = new DataSet(rawData, "test");

            dataSet.Variables.Count.Should().Be(2);
            dataSet.Name.Should().Be(expectedName);

        }
    }
}
