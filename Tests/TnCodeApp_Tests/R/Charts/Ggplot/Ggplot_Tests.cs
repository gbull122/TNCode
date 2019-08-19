using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TNCodeApp.R.Charts.Ggplot.Layer;

namespace TnCodeApp_Tests.R.Charts.Ggplot
{
    [TestClass]
    public class Ggplot_Tests
    {

        [TestMethod]
        public void Empty()
        {
            var plot = new TNCodeApp.R.Charts.Ggplot.Ggplot();

            var actualValue = plot.Command();

            actualValue.Should().Be("p<-ggplot()");
        }

        [TestMethod]
        public void Empty_layer_returns_empty()
        {
            var plot = new TNCodeApp.R.Charts.Ggplot.Ggplot();

            var layer = A.Fake<ILayer>();
            A.CallTo(() => layer.Command()).Returns(string.Empty);

            plot.Layers.Add(layer);

            var actualValue = plot.Command();

            actualValue.Should().Be(@"p<-ggplot()");
        }

        [TestMethod]
        public void Valid_layer_returns_plot()
        {
            var plot = new TNCodeApp.R.Charts.Ggplot.Ggplot();

            var layer = A.Fake<ILayer>();
            A.CallTo(() => layer.Command()).Returns(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar,y=yvar),stat=""identity"",position=""identity"",show.legend=FALSE)");

            plot.Layers.Add(layer);

            var actualValue = plot.Command();

            actualValue.Should().Be(@"p<-ggplot()+layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar,y=yvar),stat=""identity"",position=""identity"",show.legend=FALSE)");
        }
    }
}
