using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.Core.Utilities;

namespace Core.Utilities_Tests
{
    [TestClass]
    public class XmlConverter_Tests
    {

        
        [TestMethod]
        public void WhenStatBin_XmlConverted()
        {
            var txt = System.IO.File.ReadAllText("..\\..\\..\\..\\..\\src\\Core\\R\\Charts\\Ggplot\\Definitions\\Stats\\stat_bin.xml");

            var xmlConverter = new XmlConverter();
            var statistic = xmlConverter.ToObject<Stat>(txt);

            Assert.IsNotNull(statistic);
        }

        [TestMethod]
        public void WhenPositionJitter_XmlConverted()
        {
            var txt = System.IO.File.ReadAllText("..\\..\\..\\..\\..\\src\\Core\\R\\Charts\\Ggplot\\Definitions\\Positions\\pos_jitter.xml");

            var xmlConverter = new XmlConverter();
            var position = xmlConverter.ToObject<Position>(txt);

            Assert.IsNotNull(position);
        }
    }
}
