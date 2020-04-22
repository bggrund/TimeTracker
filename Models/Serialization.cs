using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace TimeTracker
{
    //public static class Serialization
    //{
    //    write object information into xml serialized data
    //    public static void SerializeObject<T>(T serializableObject, string fileName)
    //    {
    //        if (serializableObject == null) { return; }

    //        try
    //        {
    //            create xml serializer
    //            XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
    //            filestream later associated with xmlwriter
    //            FileStream stream = new FileStream(fileName, FileMode.Create);
    //            xml settings
    //            XmlWriterSettings settings = new XmlWriterSettings();
    //            settings.CloseOutput = true;
    //            settings.Indent = true;
    //            serialize object
    //            using (XmlWriter writer = XmlWriter.Create(stream, settings))
    //            {
    //                xmlserializer.Serialize(writer, serializableObject);
    //                writer.Close();
    //                stream.Close();
    //                File.WriteAllText(fileName, stream.ToString());
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show("Error serializing object of type " + typeof(T).ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    //        }
    //    }

    //    public static T DeSerializeObject<T>(string fileName)
    //    {
    //        if (!File.Exists(fileName)) { return default(T); }

    //        object to be returned
    //        T objectOut = default(T);

    //        try
    //        {
    //            xml serializer object used for deserialization
    //            XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
    //            deserialize object
    //            using (StringReader stream = new StringReader(File.ReadAllText(fileName)))
    //                {
    //                    objectOut = (T)xmlserializer.Deserialize(stream);
    //                }
    //        }
    //        debugging purposes
    //        catch (FileNotFoundException ex)
    //        {
    //            ;

    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show("Error deserializing object", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    //        }

    //        return objectOut;
    //    }

    //    public static void WriteEmptyXML(string fileName)
    //    {
    //        FileStream stream = new FileStream(fileName, FileMode.Create);
    //        serialize object
    //        using (XmlWriter writer = XmlWriter.Create(stream))
    //        {
    //            writer.WriteStartDocument();
    //            writer.Close();
    //            stream.Close();
    //        }
    //    }
    //}
}
