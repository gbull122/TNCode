using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.Core.Utilities;

namespace Core.R_Tests
{
    [TestClass]
    public class Position_Tests
    {

        [TestMethod]
        public void WhenPositionIdentity_ThenCommand_ReturnsEmpty()
        {
            var expectedCommand = "position_identity()";

            var pos = new Position();
            pos.Name = "identity";

            var actualCommand = pos.Command();

            actualCommand.Should().Be(expectedCommand);
        }

        [TestMethod]
        public void WhenPositionDodge2_ThenCommand_()
        {
            var txt = System.IO.File.ReadAllText("..\\..\\..\\..\\..\\..\\src\\Core\\R\\Charts\\Ggplot\\Definitions\\Positions\\pos_dodge2.xml");

            var xmlConverter = new XmlConverter();
            var position = xmlConverter.ToObject<Position>(txt);

            var expectedCommand = "position_dodge2(width=NULL,padding=0.1,reverse=FALSE,preserve=c(\"total\",\"single\"))";

            var actualCommand = position.Command();

            actualCommand.Should().Be(expectedCommand);
        }
    }
}
