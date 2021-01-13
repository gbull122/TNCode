using TnCode.Core.R;
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
        private readonly IXmlConverter xmlConverter;
        private readonly IRManager rManger;

        public XmlService(IXmlConverter converter,IRManager manager)
        {
            xmlConverter = converter;
            rManger = manager;
        }

        public Aesthetic LoadAesthetic(string geom)
        {
            var aestheticXml = rManger.GetDefinition("geom",geom);
            var aesthetic = xmlConverter.ToObject<Aesthetic>(aestheticXml.ToString());
            return aesthetic;
        }

        public Position LoadPosition(string pos)
        {
            var posXml = rManger.GetDefinition("pos" , pos);
            var position = xmlConverter.ToObject<Position>(posXml.ToString());
            return position;
        }

        public Stat LoadStat(string stat)
        {
            var statXml = rManger.GetDefinition("stat" , stat);
            var statistic = xmlConverter.ToObject<Stat>(statXml.ToString());
            return statistic;
        }
    }
}
