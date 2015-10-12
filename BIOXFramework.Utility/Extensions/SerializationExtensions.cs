using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BIOXFramework.Utility.Extensions
{
    public static class SerializationExtensions
    {
        public static string XmlSerialize<T>(this T self)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                XmlWriterSettings sett = new XmlWriterSettings();
                sett.ConformanceLevel = ConformanceLevel.Document;
                sett.Indent = true;
                sett.CheckCharacters = false;
                sett.Encoding = Encoding.UTF8;
                using (XmlWriter writer = XmlWriter.Create(result, sett))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(writer, self);
                    serializer = null;
                }
            }
            catch
            {
                result.Clear();
            }

            return string.IsNullOrWhiteSpace(result.ToString()) ? null : result.ToString();
        }

        public static T XmlDeserialize<T>(this T self, string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return default(T);
            try
            {
                XmlReaderSettings sett = new XmlReaderSettings();
                sett.ConformanceLevel = ConformanceLevel.Document;
                sett.CheckCharacters = false;
                using (XmlReader reader = XmlReader.Create(new StringReader(xml), sett))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    if (serializer.CanDeserialize(reader))
                        self = (T)serializer.Deserialize(reader);
                    else
                        return default(T);
                    serializer = null;
                }
            }
            catch
            {
                return default(T);
            }

            return self;
        }
    }
}