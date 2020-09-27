using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TnCode.Core.Data;

namespace Core.Data_Tests
{
    [TestClass]
    public class Variable_Tests
    {

        [TestMethod]
        public void Variable_String_Test()
        {
            var expectedCount = 3;
            var expectedElement = "C";
            var expectedVariableType = Variable.Format.Text;

            List<object> rawData = new List<object>() { "Name", "A", "B", "C", "NaN", "NaN" };
            var variable = new Variable(rawData.ToArray());

            var actualCount = variable.Length;
            var actualElement = variable.Values.ElementAt(2);

            actualCount.Should().Be(expectedCount);
            actualElement.Should().Be(expectedElement);
            variable.DataFormat.Should().Be(expectedVariableType);
        }

        [TestMethod]
        public void Variable_Numeric_Test()
        {
            var expectedCount = 4;
            var expectedElement = 2.2;
            var expectedVariableType = Variable.Format.Numeric;

            List<object> rawData = new List<object>() { "Name", 1.1, 2.2, 3.3, 4.4, double.NaN };
            var variable = new Variable(rawData.ToArray());

            var actualCount = variable.Length;
            var actualElement = variable.Values.ElementAt(1);

            actualCount.Should().Be(expectedCount);
            actualElement.Should().Be(expectedElement);
            variable.DataFormat.Should().Be(expectedVariableType);
        }

        [TestMethod]
        public void Variable_Numeric_Test1()
        {
            var expectedCount = 3;
            var expectedElement = 1.0;
            var expectedVariableType = Variable.Format.Numeric;

            List<object> rawData = new List<object>() { "Name", 1.0, 2.0, 3.0 };
            var variable = new Variable(rawData.ToArray());

            var actualCount = variable.Length;
            var actualElement = variable.Values.ElementAt(0);

            actualCount.Should().Be(expectedCount);
            actualElement.Should().Be(expectedElement);
            variable.DataFormat.Should().Be(expectedVariableType);
        }

        [TestMethod]
        [Ignore("Needs to use time")]
        public void Variable_DateTime_Test()
        {
            var expectedCount = 3;
            var expectedElement = 3.0;
            var expectedVariableType = Variable.Format.DateTime;

            List<object> rawData = new List<object>() { "Name", 1.0, 2.0, 3.0, double.NaN, double.NaN };
            var dataColumn = new Variable(rawData.ToArray());

            var actualCount = dataColumn.Values.Count;
            var actualElement = dataColumn.Values.ElementAt(2);

            actualCount.Should().Be(expectedCount);
            actualElement.Should().Be(expectedElement);
            dataColumn.DataFormat.Should().Be(expectedVariableType);
        }

        [TestMethod]
        public void Variable_Name_Test1()
        {
            var expectedName = "Variable1";

            List<object> rawData = new List<object>() { "Variable1", "A", "B", "C", "NaN", "NaN" };
            var variable = new Variable(rawData.ToArray());

            var actualName = variable.Name;

            actualName.Should().Be(expectedName);
        }

        [TestMethod]
        public void Variable_Name_Test2()
        {
            var expectedName = "V_1Variable";

            List<object> rawData = new List<object>() { "1Variable", "A", "B", "C", "NaN", "NaN" };
            var variable = new Variable(rawData.ToArray());

            var actualName = variable.Name;

            actualName.Should().Be(expectedName);
        }

        [TestMethod]
        public void Variable_Name_Test3()
        {
            var expectedName = "V_1";

            List<object> rawData = new List<object>() { "1", "A", "B", "C", "NaN", "NaN" };
            var variable = new Variable(rawData.ToArray());

            var actualName = variable.Name;

            actualName.Should().Be(expectedName);
        }

        [TestMethod]
        public void Variable_Name_Test4()
        {
            var expectedName = "V_2a_variable";

            List<object> rawData = new List<object>() { "2a variable", "A", "B", "C", "NaN", "NaN" };
            var variable = new Variable(rawData.ToArray());

            var actualName = variable.Name;

            actualName.Should().Be(expectedName);
        }
    }
}
