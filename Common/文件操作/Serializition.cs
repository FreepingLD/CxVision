using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Common
{
    public class Serializition
    {
        /// <summary>
        /// 私有构造
        /// </summary>
        private Serializition() { }

        /// <summary>
        /// 从文件加载对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static object LoadFromFile(System.Type type, string file)
        {
            object obj = new object();

            XmlSerializer xs = new XmlSerializer(type);

            if (File.Exists(file))
            {
                FileStream fs = null;
                try
                {
                    fs = File.Open(file, FileMode.Open, FileAccess.Read);
                }
                catch
                {
                    obj = type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
                }
                try
                {
                    obj = xs.Deserialize(fs);
                    LoggerHelper.Error("指定文件：" + fs + "读取成功");
                }
                catch (Exception e)
                {
                    string s = e.Message;
                    obj = type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
                }
                finally
                {
                    fs.Close();
                }
            }
            else
            {
                LoggerHelper.Error("找不到指定文件：" + file);
                try
                {
                    obj = type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
                    SaveToFile(obj, file);
                }
                catch { }
            }

            return obj;
        }

        /// <summary>
        /// 将对象保存到文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="file"></param>
        public static void SaveToFile(object obj, string file)
        {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            FileStream fs = File.Open(file, FileMode.Create, FileAccess.Write);

            try
            {
                xs.Serialize(fs, obj);
            }
            catch
            {

            }
            finally
            {
                fs.Close();
            }
        }


    }
}
