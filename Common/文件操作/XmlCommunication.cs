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
            bool result = false;
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
                    result = true;
                    LoggerHelper.Info(fileName + " 保存成功"); //Application.StartupPath +  ":文件：" +
                }
                catch (Exception e)
                {
                    LoggerHelper.Error(fileName + " 保存失败"); //Application.StartupPath + ":文件：" + 
                    //new UserMessageForm().ShowDialog(fileName + @"写入失败! " + e.ToString());
                    new Common.UserMessageForm(fileName + @" 写入失败! " + e.ToString()).ShowDialog();
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
                return result;
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
                        LoggerHelper.Info(fileName + " 读取成功"); //Application.StartupPath + ":文件：" +
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Info(fileName + " 读取失败" + ex.ToString()); //Application.StartupPath + ":文件：" +
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
                    LoggerHelper.Error("找不到指定文件：" + fileName); //Application.StartupPath + 
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
