using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Orbio.Services.Utility
{
    /// <summary>
    /// contains methods for xml serialization/deserialization
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// serialize a given object of specified type to string
        /// </summary>
        /// <typeparam name="T">the type</typeparam>
        /// <param name="instance"> the instance of type T</param>
        /// <returns>serialized string</returns>
        public static string GenericSerializer<T>(T instance)
        {
            var serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, instance);
            }
            return sb.ToString();
        }

        /// <summary>
        /// serialize a given object of specified type to string
        /// </summary>
        /// <typeparam name="T">the type</typeparam>
        /// <param name="instance"> the instance of type T</param>
        /// <returns>serialized string</returns>
        public static T GenericDeSerializer<T>(string xml) where T : class, new()
        {
            if (xml == null)
            {
                return null;
            }

            if (xml.Trim().Length == 0)
            {
                return null;
            }

            T result;
            var serializer = new XmlSerializer(typeof(T));
            using (TextReader tr = new StringReader(xml))
            {
                result = (T)serializer.Deserialize(tr);
            }
            return result;
        }

        /// <summary>
        /// deserializes an xml string to given type
        /// </summary>
        /// <typeparam name="T">the type to be serialized to</typeparam>
        /// <param name="xml">the xml string</param>
        /// <returns>instance of given type</returns>
        public static T DeserializeFromXml<T>(string xml) where T : class, new()
        {
            if (xml == null)
            {
                return null;
            }

            if (xml.Trim().Length == 0)
            {
                return null;
            }

            T result;
            var serializer = new XmlSerializer(typeof(T));
            using (TextReader tr = new StringReader(xml))
            {
                result = (T)serializer.Deserialize(tr);
            }
            return result;
        }

        /// <summary>
        /// serializer using data contract
        /// </summary>
        /// <typeparam name="T">type of the object</typeparam>
        /// <param name="instance">instance of the object</param>
        /// <returns>serialized string</returns>
        public static string GenericDataContractSerializer<T>(T instance)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
            {
                serializer.WriteObject(writer, instance);
                writer.Flush();
                return sb.ToString();
            }
        }

        /// <summary>
        /// deserializes using data contract
        /// </summary>
        /// <typeparam name="T">type of the object</typeparam>
        ///<param name="objectXml">xml string</param>
        /// <returns>deserialized object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static T GenericDataContractDeSerializer<T>(string objectXml)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var reader = XmlReader.Create(new StringReader(objectXml)))
            {
                return (T)serializer.ReadObject(reader);
            }
        }
    }
}
