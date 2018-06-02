using System;
using System.IO;
using System.Xml.Serialization;

namespace Rpgtutorial
{
    /// <summary>
    /// Loads and saves xml data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Serializer<T>
    {
        public Type Type;

        /// <summary>
        /// Basic constructor
        /// </summary>
        public Serializer()
        {
            Type = typeof(T);
        }

        /// <summary>
        /// Loads xml file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public T Load(String path)
        {
            T instance;
            using (TextReader reader = new StreamReader(path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                instance = (T)xml.Deserialize(reader);
            }
            return instance;
        }

        /// <summary>
        /// Pushes xml to file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        public void Save(string path, object obj)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                xml.Serialize(writer, obj);
            }
        }
    }
}