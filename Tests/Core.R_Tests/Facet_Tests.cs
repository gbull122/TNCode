using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TnCode.Core.R.Charts.Ggplot.Layer;

namespace Core.R_Tests
{
    [TestClass]
    public class Facet_Tests
    {

        [TestMethod]
        public void WhenNotSetThenCommandEmpty()
        {
            Facet facet = new Facet();

            string expectedCommand = string.Empty;

            string actualCommand = facet.PlotCommand();

            actualCommand.Should().Be(expectedCommand);
        }

        [TestMethod]
        public void WhenXvarialeSetThenCommandCorrect()
        {
            Facet facet = new Facet();
            facet.XVariable = "xvar";
            string expectedCommand = "+facet_grid(xvar~.)";

            string actualCommand = facet.PlotCommand();

            actualCommand.Should().Be(expectedCommand);
        }

        [TestMethod]
        public void WhenYvarialeSetThenCommandCorrect()
        {
            Facet facet = new Facet();
            facet.YVariable = "yvar";
            string expectedCommand = "+facet_grid(.~yvar)";

            string actualCommand = facet.PlotCommand();

            actualCommand.Should().Be(expectedCommand);
        }

        [TestMethod]
        public void WhenXAndYvarialeSetThenCommandCorrect()
        {
            Facet facet = new Facet();
            facet.XVariable = "xvar";
            facet.YVariable = "yvar";
            string expectedCommand = "+facet_grid(xvar~yvar)";

            string actualCommand = facet.PlotCommand();

            actualCommand.Should().Be(expectedCommand);
        }
    }
}
