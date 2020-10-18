using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.Core.Utilities;

namespace TnCode.TnCodeApp.R
{
    public interface IXmlService
    {
        Aesthetic LoadAesthetic(string geom);
        Position LoadPosition(string pos);
        Stat LoadStat(string stat);
    }

    public class XmlService : IXmlService
    {
        private IXmlConverter xmlConverter;

        public XmlService(IXmlConverter converter)
        {
            xmlConverter = converter;
        }

        public Aesthetic LoadAesthetic(string geom)
        {
            var aestheticXml = Properties.Resources.ResourceManager.GetObject("geom_" + geom.ToLower());
            var aesthetic = xmlConverter.ToObject<Aesthetic>(aestheticXml.ToString());
            return aesthetic;
        }

        public Position LoadPosition(string pos)
        {
            var posXml = Properties.Resources.ResourceManager.GetObject("pos_" + pos.ToLower());
            var position = xmlConverter.ToObject<Position>(posXml.ToString());
            return position;
        }

        public Stat LoadStat(string stat)
        {
            var statXml = Properties.Resources.ResourceManager.GetObject("stat_" + stat.ToLower());
            var statistic = xmlConverter.ToObject<Stat>(statXml.ToString());
            return statistic;
        }
    }
}
