using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TNCodeApp.R.Charts.Ggplot.Layer;

namespace TnCodeApp_Tests.R.Charts.Ggplot.Layer
{
    [TestClass]
    public class Layer_Tests
    {
        [TestMethod]
        public void Empty_returns_empty()
        {
            var layer = new TNCodeApp.R.Charts.Ggplot.Layer.Layer("point");

            var actualValue = layer.Command();

            actualValue.Should().Be(string.Empty);
        }

        [TestMethod]
        public void Set_value_returns_layer()
        {
            var aesthetic = new Aesthetic();
            var aVal1 = new AestheticValue();
            aVal1.Name = "X";
            aVal1.Entry = "xvar";
            aesthetic.AestheticValues.Add(aVal1);

            aesthetic.DefaultStat = "identity";
            aesthetic.DefaultPosition = "identity";

            var layer = new TNCodeApp.R.Charts.Ggplot.Layer.Layer("point");
            layer.Data = "DataFrame";
            layer.Aes = aesthetic;

            var actualValue = layer.Command();

            actualValue.Should().Be(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar),stat=""identity"",position=""identity"",show.legend=FALSE)");
        }


        [TestMethod]
        public void Set_values_returns_layer()
        {
            var aesthetic = new Aesthetic();
            var aVal1 = new AestheticValue();
            aVal1.Name = "X";
            aVal1.Entry = "xvar";

            var aVal2 = new AestheticValue();
            aVal2.Name = "Y";
            aVal2.Entry = "yvar";

            aesthetic.AestheticValues.Add(aVal1);
            aesthetic.AestheticValues.Add(aVal2);
            aesthetic.DefaultStat = "identity";
            aesthetic.DefaultPosition = "identity";

            var layer = new TNCodeApp.R.Charts.Ggplot.Layer.Layer("point");
            layer.Data = "DataFrame";
            layer.Aes = aesthetic;

            var actualValue = layer.Command();

            actualValue.Should().Be(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar,y=yvar),stat=""identity"",position=""identity"",show.legend=FALSE)");
        }

        [TestMethod]
        public void Unset_value_not_in_layer()
        {
            var aesthetic = new Aesthetic();
            var aVal1 = new AestheticValue();
            aVal1.Name = "X";
            aVal1.Entry = "xvar";

            var aVal2 = new AestheticValue();
            aVal2.Name = "Y";

            aesthetic.AestheticValues.Add(aVal1);
            aesthetic.AestheticValues.Add(aVal2);
            aesthetic.DefaultStat = "identity";
            aesthetic.DefaultPosition = "identity";

            var layer = new TNCodeApp.R.Charts.Ggplot.Layer.Layer("point");
            layer.Data = "DataFrame";
            layer.Aes = aesthetic;

            var actualValue = layer.Command();

            actualValue.Should().Be(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar),stat=""identity"",position=""identity"",show.legend=FALSE)");
        }

        [TestMethod]
        public void Unset_required_returns_empty()
        {
            var aesthetic = new Aesthetic();
            var aVal1 = new AestheticValue();
            aVal1.Name = "X";
            aVal1.Required = true;

            var aVal2 = new AestheticValue();
            aVal2.Name = "Y";
            aVal2.Entry = "yvar";

            aesthetic.AestheticValues.Add(aVal1);
            aesthetic.AestheticValues.Add(aVal2);

            var layer = new TNCodeApp.R.Charts.Ggplot.Layer.Layer("point");
            layer.Data = "DataFrame";
            layer.Aes = aesthetic;

            var actualValue = layer.Command();

            actualValue.Should().Be(string.Empty);
        }

        [TestMethod]
        public void Required_set_returns_layer()
        {
            var aesthetic = new Aesthetic();
            var aVal1 = new AestheticValue();
            aVal1.Name = "X";
            aVal1.Entry = "xvar";
            aVal1.Required = true;

            var aVal2 = new AestheticValue();
            aVal2.Name = "Y";
            aVal2.Entry = "yvar";
            aVal2.Required = true;

            aesthetic.AestheticValues.Add(aVal1);
            aesthetic.AestheticValues.Add(aVal2);
            aesthetic.DefaultStat = "identity";
            aesthetic.DefaultPosition = "identity";

            var layer = new TNCodeApp.R.Charts.Ggplot.Layer.Layer("point");
            layer.Data = "DataFrame";
            layer.Aes = aesthetic;

            var actualValue = layer.Command();

            actualValue.Should().Be(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar,y=yvar),stat=""identity"",position=""identity"",show.legend=FALSE)");
        }

        [TestMethod]
        public void Required_set_returns_layer1()
        {
            var aesthetic = new Aesthetic();
            var aVal1 = new AestheticValue();
            aVal1.Name = "X";
            aVal1.Entry = "xvar";
            aVal1.Required = true;

            var aVal2 = new AestheticValue();
            aVal2.Name = "Y";

            aesthetic.AestheticValues.Add(aVal1);
            aesthetic.AestheticValues.Add(aVal2);
            aesthetic.DefaultStat = "identity";
            aesthetic.DefaultPosition = "identity";

            var layer = new TNCodeApp.R.Charts.Ggplot.Layer.Layer("point");
            layer.Data = "DataFrame";
            layer.Aes = aesthetic;

            var actualValue = layer.Command();

            actualValue.Should().Be(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar),stat=""identity"",position=""identity"",show.legend=FALSE)");
        }
    }
}
