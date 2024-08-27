using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Threading.Tasks;


//using System.Linq;

namespace Common
{
    public abstract class XML<T>
    {
        private static object rwLock = new object();
        public static bool Save(T entry, string fileName)
        {
            lock (rwLock)
            {
                MemoryStream stream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(stream, null);
                writer.Formatting = Formatting.Indented;
                StreamReader reader = null;
                FileStream fileStream = null;
                StreamWriter fileWriter = null;
                try
                {
                    if (entry == null) return true;
                    XmlSerializer xml = new XmlSerializer(entry.GetType());
                    xml.Serialize(writer, entry);
                    reader = new StreamReader(stream);
                    fileStream = new FileStream(fileName, FileMode.Create);
                    fileWriter = new StreamWriter(fileStream);
                    stream.Position = 0;
                    fileWriter.Write(reader.ReadToEnd());
                    LoggerHelper.Info("文件：" + fileName + "保存成功");
                }
                catch (Exception e)
                {
                    LoggerHelper.Error("文件：" + fileName + "保存失败");
                    MessageBox.Show(fileName + @"写入失败! " + e.ToString());
                }
                finally
                {
                    if (reader != null) reader.Close();
                    stream.Close();
                    //if(writer.WriteState.)
                    string str = writer.WriteState.ToString();
                    try
                    {
                        writer.Close();
                    }
                    catch
                    { }
                    if (fileWriter != null)
                    {
                        fileWriter.Flush();
                        fileWriter.Close();
                        fileStream.Close();
                    }
                }
                return true;
            }
        }

        public static T Read(string fileName)
        {
            lock (rwLock)
            {
                T entry = default(T);
                StreamReader reader = null;
                XmlTextReader xmlTextReader = null;
                if (File.Exists(fileName))
                {
                    try
                    {                       
                        //reader = new StreamReader(fileName);
                        XmlSerializer xs = new XmlSerializer(typeof(T));
                        xmlTextReader = new XmlTextReader(fileName);
                        xmlTextReader.Namespaces = false;
                        entry = (T)xs.Deserialize(xmlTextReader);
                        LoggerHelper.Info("文件：" + fileName + "读取成功");
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Info("文件：" + fileName + "读取失败" + ex.ToString());
                    }
                    finally
                    {
                        try
                        {
                            xmlTextReader?.Close();
                            reader?.Close();
                        }
                        catch
                        {
                            xmlTextReader = null;
                            reader = null;
                        }
                    }                  
                }
                else
                {
                    LoggerHelper.Error("找不到指定文件：" + fileName);
                }
                return entry;
            }
        }

        public static T Clone(T RealObject)
        {
            using (Stream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, RealObject);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)serializer.Deserialize(stream);
            }
        }



    }
}
