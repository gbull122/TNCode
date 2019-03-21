using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModuleR.Charts.Ggplot.Layer;

namespace ModuleR_Tests
{
    [TestClass]
    public class Aesthetic_Tests
    {
        [TestMethod]
        public void Empty()
        {
            var aesthetic = new Aesthetic();

            var command = aesthetic.Command();

            command.Should().Be("aes()");
        }


        [TestMethod]
        public void Simple()
        {
            var aestheticValue = new AestheticValue();
            aestheticValue.Name = "X";
            aestheticValue.Entry = "Xvar";

            var aesthetic = new Aesthetic();
            aesthetic.AestheticValues.Add(aestheticValue);
            var command = aesthetic.Command();

            command.Should().Be("aes(x=Xvar)");
        }

        [TestMethod]
        public void Factor()
        {
            var aestheticValue = new AestheticValue();
            aestheticValue.Name = "X";
            aestheticValue.Entry = "Xvar";
            aestheticValue.IsFactor = true;

            var aesthetic = new Aesthetic();
            aesthetic.AestheticValues.Add(aestheticValue);

            var command = aesthetic.Command();

            command.Should().Be("aes(x=as.factor(Xvar))");
        }

        [TestMethod]
        public void LowerCase()
        {
            var aestheticValue = new AestheticValue();
            aestheticValue.Name = "X";
            aestheticValue.Entry = "Xvar";
            aestheticValue.IsFactor = true;
            aestheticValue.UseLowerCase = true;

            var aesthetic = new Aesthetic();
            aesthetic.AestheticValues.Add(aestheticValue);

            var command = aesthetic.Command();

            command.Should().Be("aes(x=as.factor(xvar))");
        }

        [TestMethod]
        public void Required_NotSet()
        {
            var aestheticValueX = new AestheticValue();
            aestheticValueX.Name = "X";
            aestheticValueX.Entry = "Xvar";
            aestheticValueX.Required = true;

            var aestheticValueY = new AestheticValue();
            aestheticValueY.Name = "Y";
            aestheticValueY.Required = true;

            var aesthetic = new Aesthetic();
            aesthetic.AestheticValues.Add(aestheticValueX);
            aesthetic.AestheticValues.Add(aestheticValueY);

            var command = aesthetic.Command();

            command.Should().Be("aes(x=Xvar)");
        }

        [TestMethod]
        public void Required_BothSet()
        {
            var aestheticValueX = new AestheticValue();
            aestheticValueX.Name = "X";
            aestheticValueX.Entry = "Xvar";
            aestheticValueX.Required = true;

            var aestheticValueY = new AestheticValue();
            aestheticValueY.Name = "Y";
            aestheticValueY.Entry = "Yvar";
            aestheticValueY.Required = true;

            var aesthetic = new Aesthetic();
            aesthetic.AestheticValues.Add(aestheticValueX);
            aesthetic.AestheticValues.Add(aestheticValueY);

            var command = aesthetic.Command();

            command.Should().Be("aes(x=Xvar,y=Yvar)");
        }

    }
}
