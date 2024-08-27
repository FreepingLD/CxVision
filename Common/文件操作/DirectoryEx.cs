using System;
using System.Windows.Forms;
using System.IO;


namespace Common
{
    public static class DirectoryEx
    {
        public static string ErrorMessage = "";

        public static bool Exist(string strPath, bool isShowMsgBox = false)
        {
            try
            {
                if (!System.IO.Directory.Exists(strPath))
                {
                    if (isShowMsgBox)
                        MessageBox.Show("目录: " + strPath + "不存在!");
                    return false;
                }
                bool ISNo = System.IO.Directory.Exists(strPath);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                ErrorMessage = ex.Message.ToString();
                return false;
            }

            return true;
        }
        public static bool Create(string strPath)
        {
            try
            {
                System.IO.Directory.CreateDirectory(strPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                ErrorMessage = ex.Message.ToString();
                return false;
            }
            return true;
        }
        public static bool Delete(string strPath)
        {

            try
            {
                // System.IO.Directory.CreateDirectory(strPath);
                System.IO.Directory.Delete(strPath+@"\", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                ErrorMessage = ex.Message.ToString();
                return false;
            }
            return true;

        }




        public static void CopyFolder(string strFromPath, string strToPath)
        {
            //如果源文件夹不存在，则创建
            if (!Directory.Exists(strFromPath))
            {
                Directory.CreateDirectory(strFromPath);
            }
            //取得要拷贝的文件夹名
            string strFolderName = strFromPath.Substring(strFromPath.LastIndexOf("\\") +
              1, strFromPath.Length - strFromPath.LastIndexOf("\\") - 1);
            //如果目标文件夹中没有源文件夹则在目标文件夹中创建源文件夹
            if (!Directory.Exists(strToPath + "\\" + strFolderName))
            {
                Directory.CreateDirectory(strToPath + "\\" + strFolderName);
            }
            //创建数组保存源文件夹下的文件名
            string[] strFiles = Directory.GetFiles(strFromPath);
            //循环拷贝文件
            for (int i = 0; i < strFiles.Length; i++)
            {
                //取得拷贝的文件名，只取文件名，地址截掉。
                string strFileName = strFiles[i].Substring(strFiles[i].LastIndexOf("\\") + 1, strFiles[i].Length - strFiles[i].LastIndexOf("\\") - 1);
                //开始拷贝文件,true表示覆盖同名文件
                File.Copy(strFiles[i], strToPath + "\\" + strFolderName + "\\" + strFileName, true);
            }
            //创建DirectoryInfo实例
            DirectoryInfo dirInfo = new DirectoryInfo(strFromPath);
            //取得源文件夹下的所有子文件夹名称
            DirectoryInfo[] ZiPath = dirInfo.GetDirectories();
            for (int j = 0; j < ZiPath.Length; j++)
            {
                //获取所有子文件夹名
                string strZiPath = strFromPath + "\\" + ZiPath[j].ToString();
                //把得到的子文件夹当成新的源文件夹，从头开始新一轮的拷贝
                CopyFolder(strZiPath, strToPath + "\\" + strFolderName);
            }
        }



        
        /// 清空指定的文件夹，但不删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        DeleteFolder(d1.FullName);////递归删除子文件夹
                    }
                    Directory.Delete(d);
                }
            }
        }        
        /// <summary>
                 /// 删除文件夹及其内容
                 /// </summary>
                 /// <param name="dir"></param>
        public static void DeleteFolder1(string dir)
        {
            try
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;
                        File.Delete(d);//直接删除其中的文件  
                    }
                    else
                        DeleteFolder(d);////递归删除子文件夹
                    Directory.Delete(d);
                }

            }
            catch
            { }


        }



    }


}
