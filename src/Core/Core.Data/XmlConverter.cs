using System;
using System.IO;
using System.Xml.Serialization;

namespace TNCode.Core.Data
{
    public class XmlConverter : IXmlConverter
    {
        public string ToXml<T>(T objectToConvert)
        {
            try
            {
                using (var stringwriter = new StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(T));
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    serializer.Serialize(stringwriter, objectToConvert, ns);
                    return stringwriter.ToString();
                }
            }
            catch
            {
                throw new Exception("Failed to convert object to xml.");
            }
        }

        public T ToObject<T>(string xml)
        {
            //try
            //{
                using (var stringReader = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader);
                }
            //}
            //catch
            //{
            //    throw new Exception("Failed to convert xml to object.");
            //}
        }

    }
}
