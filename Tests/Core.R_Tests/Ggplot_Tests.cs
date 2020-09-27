using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TnCode.Core.R.Charts.Ggplot;
using TnCode.Core.R.Charts.Ggplot.Layer;

namespace Core.R_Tests
{
    [TestClass]
    public class Ggplot_Tests
    {

        [TestMethod]
        public void WhenGgplotHasNoLayers_ThenEmptyCommand()
        {
            var plot = new Ggplot();

            var actualValue = plot.Command();

            actualValue.Should().Be("p<-ggplot()");
        }

        [TestMethod]
        public void WhennGgplotHasEmptyLayer_ThenEmptyCommand()
        {
            var plot = new Ggplot();

            var layer = A.Fake<ILayer>();
            A.CallTo(() => layer.Command()).Returns(string.Empty);

            plot.Layers.Add(layer);

            var actualValue = plot.Command();

            actualValue.Should().Be(@"p<-ggplot()");
        }

        [TestMethod]
        public void WhenGgplotHasLayer_ThenReturnsLayerCommand()
        {
            var plot = new Ggplot();

            var layer = A.Fake<ILayer>();
            A.CallTo(() => layer.ShowInPlot).Returns(true);
            A.CallTo(() => layer.Command()).Returns(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar,y=yvar),stat=""identity"",position=""identity"",show.legend=FALSE)");

            plot.Layers.Add(layer);

            var actualValue = plot.Command();

            actualValue.Should().Be(@"p<-ggplot()+layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar,y=yvar),stat=""identity"",position=""identity"",show.legend=FALSE)");
        }
    }
}
