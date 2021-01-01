using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TnCode.Core.R.Charts.Ggplot.Layer;

namespace Core.R_Tests
{
    [TestClass]
    public class Parameter_Tests
    {

        [TestMethod]
        public void WhenDefaultCommandDefault()
        {
            string name = "Name";
            string value = "value";

            string expectedCommand = "Name=value";
            Parameter param = new Parameter(name,value);

            string actualCommand = param.Command();

            actualCommand.Should().Be(expectedCommand);
        }

        [TestMethod]
        public void WhenCapitiliseCommandCapitilised()
        {
            string name = "Name";
            string value = "value";

            string expectedCommand = "Name=VALUE";
            Parameter param = new Parameter(name, value,true);

            string actualCommand = param.Command();

            actualCommand.Should().Be(expectedCommand);
        }

        [TestMethod]
        public void WhenQuotesCommandQuotes()
        {
            string name = "Name";
            string value = "value";

            string expectedCommand = "Name=\"value\"";
            Parameter param = new Parameter(name, value,false,true);

            string actualCommand = param.Command();

            actualCommand.Should().Be(expectedCommand);
        }

        [TestMethod]
        public void WhenCapitiliseQuotesCommandCapitilisedQuotes()
        {
            string name = "Name";
            string value = "value";

            string expectedCommand = "Name=\"VALUE\"";
            Parameter param = new Parameter(name, value, true, true);

            string actualCommand = param.Command();

            actualCommand.Should().Be(expectedCommand);
        }
    }
}
