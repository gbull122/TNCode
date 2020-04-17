namespace TNCode.Core.Data
{
    public interface IXmlConverter
    {
        T ToObject<T>(string xml);
        string ToXml<T>(T objectToConvert);
    }
}