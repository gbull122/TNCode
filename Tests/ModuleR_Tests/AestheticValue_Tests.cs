using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModuleR.Charts.Ggplot.Layer;

namespace ModuleR_Tests
{
    [TestClass]
    public class AestheticValue_Tests
    {
        private string name = "Colour";
        private string entry = "Variable";

        [TestMethod]
        public void Empty()
        {
            var aestheticValue = new AestheticValue();

            var actualValue = aestheticValue.ReadValue();

            actualValue.Should().Be(string.Empty);
        }

        [TestMethod]
        public void Basic()
        {
            var aestheticValue = new AestheticValue();

            aestheticValue.Name = name;
            aestheticValue.Entry = entry;
            var actualValue = aestheticValue.ReadValue();

            actualValue.Should().Be("colour=Variable");
        }

        [TestMethod]
        public void Factor()
        {
            var aestheticValue = new AestheticValue();

            aestheticValue.Name = name;
            aestheticValue.Entry = entry;
            aestheticValue.IsFactor = true;

            var actualValue = aestheticValue.ReadValue();

            actualValue.Should().Be("colour=as.factor(Variable)");
        }

        [TestMethod]
        public void Factor_LowerCase()
        {
            var aestheticValue = new AestheticValue();

            aestheticValue.Name = name;
            aestheticValue.Entry = entry;
            aestheticValue.IsFactor = true;
            aestheticValue.UseLowerCase = true;

            var actualValue = aestheticValue.ReadValue();

            actualValue.Should().Be("colour=as.factor(variable)");
        }

        [TestMethod]
        public void Factor_LowerCase_Quotes()
        {
            var aestheticValue = new AestheticValue();

            aestheticValue.Name = name;
            aestheticValue.Entry = entry;
            aestheticValue.IsFactor = true;
            aestheticValue.UseLowerCase = true;
            aestheticValue.FormatString = "\"{0}\"";
            var actualValue = aestheticValue.ReadValue();

            actualValue.Should().Be(@"colour=as.factor(""variable"")");
        }
    }
}
