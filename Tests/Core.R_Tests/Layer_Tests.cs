using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TnCode.Core.R.Charts.Ggplot.Layer;

namespace Core.R_Tests
{
    [TestClass]
    public class Layer_Tests
    {
        [TestMethod]
        public void Empty_returns_minimum()
        {
            string expectedCommand = @"layer(data=,geom=""point"",mapping=aes(),stat=""identity"",position_identity(),show.legend=TRUE)";
            var aesthetic = new Aesthetic();
            var stat = new Stat();
            stat.Name = "identity";
            var pos = new Position();
            pos.Name = "identity";

            var layer = new Layer("point", aesthetic, stat,pos);

            var actualCommand = layer.Command();

            actualCommand.Should().Be(expectedCommand);
        }

        [TestMethod]
        public void SetX_value_returns_layer()
        {
            var stat = new Stat();
            stat.Name = "identity";
            var pos = new Position();
            pos.Name = "identity";

            var aesthetic = new Aesthetic();
            var aVal1 = new AestheticValue();
            aVal1.Name = "X";
            aVal1.Entry = "xvar";
            aesthetic.AestheticValues.Add(aVal1);

            aesthetic.DefaultStat = "identity";
            aesthetic.DefaultPosition = "identity";

            var layer = new Layer("point", aesthetic, stat, pos);
            layer.Data = "DataFrame";
            layer.Aes = aesthetic;

            var actualValue = layer.Command();

            actualValue.Should().Be(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar),stat=""identity"",position_identity(),show.legend=TRUE)");
        }


        [TestMethod]
        public void SetXY_values_returns_layer()
        {
            var stat = new Stat();
            stat.Name = "identity";
            var pos = new Position();
            pos.Name = "identity";

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

            var layer = new Layer("point", aesthetic, stat, pos);
            layer.Data = "DataFrame";
            layer.Aes = aesthetic;

            var actualValue = layer.Command();

            actualValue.Should().Be(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar,y=yvar),stat=""identity"",position_identity(),show.legend=TRUE)");
        }

        [TestMethod]
        public void Unset_value_not_in_layer()
        {
            var stat = new Stat();
            stat.Name = "identity";
            var pos = new Position();
            pos.Name = "identity";

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

            var layer = new Layer("point", aesthetic, stat, pos);
            layer.Data = "DataFrame";
            layer.Aes = aesthetic;

            var actualValue = layer.Command();

            actualValue.Should().Be(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar),stat=""identity"",position_identity(),show.legend=TRUE)");
        }

        [TestMethod]
        public void Unset_required_returns_empty()
        {
            var stat = new Stat();
            var pos = new Position();

            var aesthetic = new Aesthetic();
            var aVal1 = new AestheticValue();
            aVal1.Name = "X";
            aVal1.Required = true;

            var aVal2 = new AestheticValue();
            aVal2.Name = "Y";
            aVal2.Entry = "yvar";

            aesthetic.AestheticValues.Add(aVal1);
            aesthetic.AestheticValues.Add(aVal2);

            var layer = new Layer("point", aesthetic, stat, pos);
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

            var stat = new Stat();
            stat.Name = "identity";

            var pos = new Position();
            pos.Name  = "identity";

            var layer = new Layer("point",aesthetic,stat,pos);
            layer.Data = "DataFrame";
            layer.Aes = aesthetic;
            layer.ShowInPlot = true;
            layer.ShowLegend = true;
            layer.Statistic = stat;
            layer.Pos = pos;

            var actualValue = layer.Command();

            actualValue.Should().Be(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar,y=yvar),stat=""identity"",position_identity(),show.legend=TRUE)");
        }

        [TestMethod]
        public void Required_set_returns_layer1()
        {
            var stat = new Stat();
            stat.Name = "identity";
            var pos = new Position();
            pos.Name = "identity";

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

            var layer = new Layer("point",aesthetic,stat,pos);
            layer.Data = "DataFrame";
            layer.Aes = aesthetic;

            var actualValue = layer.Command();

            actualValue.Should().Be(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar),stat=""identity"",position_identity(),show.legend=TRUE)");
        }

        [TestMethod]
        public void AesHasParam()
        {
            var stat = new Stat();
            stat.Name = "identity";
            var pos = new Position();
            pos.Name = "identity";

            var prop = new Property();
            prop.Name = "Prop";
            prop.Value = "1.2";

            var aesthetic = new Aesthetic();
            aesthetic.Values.Add(prop);

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

            var layer = new Layer("point", aesthetic, stat, pos);
            layer.Data = "DataFrame";
            layer.Aes = aesthetic;

            var actualValue = layer.Command();

            actualValue.Should().Be(@"layer(data=DataFrame,geom=""point"",mapping=aes(x=xvar),stat=""identity"",position_identity(),params=list(Prop=1.2),show.legend=TRUE)");
        }
    }
}
