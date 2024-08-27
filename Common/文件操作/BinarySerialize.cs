using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class BinarySerialize
    {
        public static bool Save<T>(T entry, string fileName)
        {
            BinaryFormatter binFormat = new BinaryFormatter(); //
            try
            {
                using (FileStream fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    binFormat.Serialize(fStream, entry);
                }
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                LoggerHelper.Error("保存相机校准文件失败", ex);
                throw;
                //return false;
            }
        }
        public static T Read<T>(string fileName)
        {
            string path = fileName;
            T entry = default(T);
            if (!File.Exists(path)) return entry;
            BinaryFormatter binFormat = new BinaryFormatter();
            try
            {
                using (Stream fStream = File.OpenRead(path))
                {
                    return (T)binFormat.Deserialize(fStream);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                LoggerHelper.Error("读取相机校准文件失败", ex);
                throw;
                //return null;
            }
        }



    }
}
