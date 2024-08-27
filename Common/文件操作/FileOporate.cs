using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace Common
{

    public class FileOperate
    {

        /// <summary>
        /// 保存一个数组中的数据到CSV文件中
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data"></param>
        /// <param name="OutPutMode"></param>
        /// 
        public void WriteCSV(string Path, double[] Data, string OutPutMode)
        {
            try
            {
                switch (OutPutMode)
                {
                    case "Row":
                        using (StreamWriter sw = new StreamWriter(Path, true))
                        {
                            string st = String.Join(",", Data);
                            sw.WriteLine(st);
                        }
                        break;
                    case "Col":
                        using (StreamWriter sw = new StreamWriter(Path, false))
                        {
                            for (int i = 0; i < Data.Length; i++)
                            {
                                sw.WriteLine(Data[i]);
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
            catch { }
        }
        public void WriteCSV(string Path, object[] Data, string OutPutMode)
        {
            try
            {
                switch (OutPutMode)
                {
                    case "Row":
                        using (StreamWriter sw = new StreamWriter(Path, true))
                        {
                            string st = String.Join(",", Data);
                            sw.WriteLine(st);
                        }
                        break;
                    case "Col":
                        using (StreamWriter sw = new StreamWriter(Path, false))
                        {
                            for (int i = 0; i < Data.Length; i++)
                            {
                                sw.WriteLine(Data[i]);
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
            catch { }
        }

        /// <summary>
        /// 从CSV文件中读取数据到一个数组中
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data"></param>
        public void ReadCSV(string Path, out double[] Data)
        {
            Data = null;
            List<double> Da = new List<double>();
            if (Path == null || Path.Trim().Length == 0) return;
            using (StreamReader reader = new StreamReader(Path))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] st = line.Split(',');//如果其他分隔符替换掉就OK了 
                    foreach (string s in st)
                    {
                        if (s.Length != 0)  //当字符串不为空时才转换
                            Da.Add(Convert.ToDouble(s));
                    }
                    line = reader.ReadLine();
                }
            }
            Data = Da.ToArray();
        }


        public void ReadCSV(string Path, out double[] X, out double[] Y, out double[] Z)
        {
            List<double> Da_x = new List<double>();
            List<double> Da_y = new List<double>();
            List<double> Da_z = new List<double>();
            X = null;
            Y = null;
            Z = null;
            using (StreamReader reader = new StreamReader(Path))
            {
                int num = 0;
                string[] st;
                string line;
                while (true)
                {
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        num++;
                        if (num > 10) break;
                        else
                            continue;
                    }
                    num = 0;
                    st = line.Split(',', '\t');//如果其他分隔符替换掉就OK了 
                    if (st.Length < 3) return;
                    Da_x.Add(Convert.ToDouble(st[0]));
                    Da_y.Add(Convert.ToDouble(st[1]));
                    Da_z.Add(Convert.ToDouble(st[2]));
                    // line = reader.ReadLine();
                }
            }
            X = Da_x.ToArray();
            Y = Da_y.ToArray();
            Z = Da_z.ToArray();
        }


        /// <summary>
        /// 从CSV文件中读取数据到一个数组中
        /// </summary>
        /// <param name="Path">路径</param>
        /// <param name="Index">读取指定的行</param>
        /// <param name="Data">输出的数据</param> 
        public void ReadCSV(string Path, int Index, out double[] Data)
        {
            List<double> Da = new List<double>();
            using (StreamReader reader = new StreamReader(Path))
            {
                string line = "";
                for (int i = 1; i <= Index; i++)
                {
                    line = reader.ReadLine();
                }
                while (line != null)
                {
                    string[] st = line.Split(',');//如果其他分隔符替换掉就OK了 
                    foreach (string s in st)
                    {
                        if (s.Length != 0)  //当字符串不为空时才转换
                            Da.Add(Convert.ToDouble(s));
                    }
                    line = null;
                }

            }
            Data = Da.ToArray();
        }

        /// <summary>
        /// 保存数据到CSV中
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data"></param>
        /// <param name="OutPutMode"></param>
        public void WriteCSV(string Path, float[] Data, string OutPutMode)
        {
            try
            {
                switch (OutPutMode)
                {
                    case "Row":
                        using (StreamWriter sw = new StreamWriter(Path, true))
                        {
                            string st = String.Join(",", Data);
                            sw.WriteLine(st);
                        }
                        break;
                    case "Col":
                        using (StreamWriter sw = new StreamWriter(Path, false))
                        {
                            for (int i = 0; i < Data.Length; i++)
                            {
                                sw.WriteLine(Data[i]);
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
            catch { }
        }

        /// <summary>
        /// 从CSV中读取数据
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data"></param>
        public void ReadCSV(string Path, out float[] Data)
        {
            List<float> Da = new List<float>();
            if (File.Exists(Path))
            {
                using (StreamReader reader = new StreamReader(Path))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        string[] st = line.Split(',');//如果其他分隔符替换掉就OK了 
                        foreach (string s in st)
                        {
                            if (s.Length != 0)  //当字符串不为空时才转换
                                Da.Add(Convert.ToSingle(s));
                        }
                        line = reader.ReadLine();
                    }
                }

            }
            else
            {
                Console.WriteLine("文件不存在");
            }
            Data = Da.ToArray();
        }

        /// <summary>
        /// 从CSV中读取数据
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data1"></param>
        public void ReadTxt(string Path, out double[] Data)
        {
            List<double> Da = new List<double>();
            if (File.Exists(Path))
            {
                using (StreamReader reader = new StreamReader(Path))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        string[] st = line.Split(',', ';');//如果其他分隔符替换掉就OK了 
                        foreach (string s in st)
                        {
                            if (s.Length != 0)  //当字符串不为空时才转换
                                Da.Add(Convert.ToSingle(s));
                        }
                        line = reader.ReadLine();
                    }
                }

            }
            else
            {
                //for (int i = 0; i < 10000000; i++)
                //{
                //    Da.Add(0);
                //}
                //Console.WriteLine("文件不存在");
            }
            Data = Da.ToArray();
        }

        public void ReadTxt(string Path, out string[] Data)
        {
            List<string> Da = new List<string>();
            if (File.Exists(Path))
            {
                using (StreamReader reader = new StreamReader(Path))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        Da.Add(line);
                        line = reader.ReadLine();
                    }
                }
            }
            else
            {

            }
            Data = Da.ToArray();
        }
        public string OpenXmlFile()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "xml文件(*.xml)|*.ply|Txt文件(*.txt)|*.txt|om3文件(*.om3)|*.om3|STL文件(*.stl)|*.stl|obj文件(*.obj)|*.obj|off文件(*.off)|*.off|dxf文件(*.dxf)|*.dxf|csv文件(.csv)|*.csv|所有文件(*.*)|*.**";
                ofd.RestoreDirectory = false;
                ofd.FilterIndex = 2;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        /// <summary>
        /// 从CSV中读取数据
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data1"></param>
        public void ReadTxt(string Path, out double[] Data1, out double[] Data2)
        {
            List<double> Da1 = new List<double>();
            List<double> Da2 = new List<double>();
            if (File.Exists(Path))
            {
                using (StreamReader reader = new StreamReader(Path))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        string[] st = line.Split(',', ';', '\t');//如果其他分隔符替换掉就OK了 

                        if (st.Length > 1)
                        {
                            Da1.Add(Convert.ToSingle(st[0]));
                            Da2.Add(Convert.ToSingle(st[1]));
                        }
                        line = reader.ReadLine();
                    }
                }

            }
            Data1 = Da1.ToArray();
            Data2 = Da2.ToArray();
        }
        /// <summary>
        /// 保存数据到Txt中
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data"></param>
        /// <param name="OutPutMode"></param>
        public void WriteTxt(string Path, string StringData, bool isAppend)
        {
            if (!File.Exists(Path))
            {
                FileInfo fileInfo = new FileInfo(Path);
                if (!Directory.Exists(fileInfo.DirectoryName))
                    Directory.CreateDirectory(fileInfo.DirectoryName);
                using (StreamWriter sw = new StreamWriter(Path, false))
                {
                    sw.WriteLine(StringData);
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(Path, isAppend))
                {
                    sw.WriteLine(StringData);
                }
            }
        }

        public void WriteTxt(string Path, string[] StringData)
        {
            if (!File.Exists(Path))
            {
                FileInfo fileInfo = new FileInfo(Path);
                if (!Directory.Exists(fileInfo.DirectoryName))
                    Directory.CreateDirectory(fileInfo.DirectoryName);
                using (StreamWriter sw = new StreamWriter(Path, false))
                {
                    foreach (var item in StringData)
                    {
                        sw.WriteLine(item);
                    }
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(Path, false))
                {
                    foreach (var item in StringData)
                    {
                        sw.WriteLine(item);
                    }
                }
            }

        }

        /// <summary>
        /// 从Txt中读取数据
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data"></param>
        public void ReadTxt(string Path, out string StringData)
        {
            if (File.Exists(Path))
            {
                using (StreamReader reader = new StreamReader(Path))
                {
                    StringData = reader.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("文件不存在");
                StringData = "请输入传感器名称";
            }
        }
        /// <summary>
        /// 从Txt中读取数据
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data"></param>
        public void ReadTxt(string Path, out HTuple Data)
        {
            Data = new HTuple();
            if (File.Exists(Path))
            {
                using (StreamReader reader = new StreamReader(Path))
                {
                    Data = new HTuple(double.Parse(reader.ReadLine().Trim()));
                }
            }
            else
            {
                Data = new HTuple(0); // 如果不存，则值为0
                Console.WriteLine("文件不存在");
            }
        }
        /// <summary>
        /// 统计一个文件中有多少行
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public int ReadFileNumber(string Path)
        {
            int num = 0;
            if (File.Exists(Path))
            {
                using (StreamReader reader = new StreamReader(Path))
                {
                    while (reader.ReadLine() != null)
                    {
                        num++;
                    }
                }
            }
            else
            {
                Console.WriteLine("文件不存在");
            }
            return num;
        }

        /// <summary>
        /// 保存数据到CSV中
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data"></param>
        /// <param name="OutPutMode"></param>
        public void WriteTxt(string Path, double[] Data, bool isAppend)
        {
            int length = Data.Length;
            try
            {
                if (!File.Exists(Path))
                {
                    FileInfo fileInfo = new FileInfo(Path);
                    if (!Directory.Exists(fileInfo.DirectoryName))
                        Directory.CreateDirectory(fileInfo.DirectoryName);
                    using (StreamWriter sw = new StreamWriter(Path, false))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            sw.WriteLine(Data[i]);
                        }
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(Path, isAppend))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            sw.WriteLine(Data[i]);
                        }
                    }
                }

            }
            catch
            {
            }
        }
        public void WriteTxt(string Path, object[] Data1, object[] Data2)
        {
            int length = Math.Min(Data1.Length, Data2.Length);
            try
            {
                if (!File.Exists(Path))
                {
                    FileInfo fileInfo = new FileInfo(Path);
                    if (!Directory.Exists(fileInfo.DirectoryName))
                        Directory.CreateDirectory(fileInfo.DirectoryName);
                    using (StreamWriter sw = new StreamWriter(Path, false))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            sw.WriteLine("{0} \t{1}", Data1[i], Data2[i]);
                        }
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(Path, true))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            sw.WriteLine("{0} \t{1}", Data1[i], Data2[i]);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 保存数据到CSV中
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data"></param>
        /// <param name="OutPutMode"></param>
        public void WriteTxt(string Path, double[] DataX, double[] DataY, double[] DataZ)
        {
            int length = DataZ.Length;
            try
            {
                if (!File.Exists(Path))
                {
                    FileInfo fileInfo = new FileInfo(Path);
                    if (!Directory.Exists(fileInfo.DirectoryName))
                        Directory.CreateDirectory(fileInfo.DirectoryName);
                    using (StreamWriter sw = new StreamWriter(Path, false))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            sw.WriteLine("{0} \t{1} \t{2}", DataX[i], DataY[i], DataZ[i]);
                        }
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(Path, true))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            sw.WriteLine("{0} \t{1} \t{2}", DataX[i], DataY[i], DataZ[i]);
                        }
                    }
                }

            }
            catch
            {
            }
        }

        /// <summary>
        /// 保存数据到CSV中
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data"></param>
        /// <param name="OutPutMode"></param>
        public void WriteTxt(string Path, double[] DataX, double[] DataY, double[] DataZ, double[] Intensity)
        {
            int length = DataZ.Length;
            try
            {
                if (!File.Exists(Path))
                {
                    FileInfo fileInfo = new FileInfo(Path);
                    if (!Directory.Exists(fileInfo.DirectoryName))
                        Directory.CreateDirectory(fileInfo.DirectoryName);
                    using (StreamWriter sw = new StreamWriter(Path, false))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            sw.WriteLine("{0:f15} \t{1:f15} \t{2:f15}  \t{3}", DataX[i], DataY[i], DataZ[i], Intensity[i]);
                        }
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(Path, true))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            sw.WriteLine("{0:f15} \t{1:f15} \t{2:f15}  \t{3}", DataX[i], DataY[i], DataZ[i], Intensity[i]);
                        }
                    }
                }

            }
            catch
            {
            }


            //            字母 含义
            //C或c Currency 货币格式
            //D或d Decimal 十进制格式（十进制整数，不要和.Net的Decimal数据类型混淆了） 
            //E或e Exponent 指数格式
            //F或f Fixed point 固定精度格式
            //G或g General 常用格式
            //N或n 用逗号分割千位的数字，比如1234将会被变成1,234
            //P或p Percentage 百分符号格式
            //R或r Round - trip  圆整（只用于浮点数）保证一个数字被转化成字符串以后可以再被转回成同样的数字
            //  X或x Hex 16进制格式

        }
        /// <summary>
        /// 保存数据到CSV中
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Data"></param>
        /// <param name="OutPutMode"></param>
        public void WriteTxt(string Path, float[] Data)
        {
            int length = Data.Length;
            try
            {
                if (!File.Exists(Path))
                {
                    FileInfo fileInfo = new FileInfo(Path);
                    if (!Directory.Exists(fileInfo.DirectoryName))
                        Directory.CreateDirectory(fileInfo.DirectoryName);
                    using (StreamWriter sw = new StreamWriter(Path, false))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            sw.WriteLine(Data[i]);
                        }
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(Path, true))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            sw.WriteLine(Data[i]);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 将当前系统时间以字符串的形式返回
        /// </summary>
        /// <returns></returns>
        public string DataTimeToString()
        {
            return DateTime.Now.ToString().Replace("/", "").Replace(":", "");
        }


        /// <summary>
        /// 从文本中读取数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Dictionary<string, string> ReadTxt(string path)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            if (File.Exists(path))
            {
                string[] name;
                string single = "";
                using (StreamReader reader = new StreamReader(path))
                {
                    while (true)
                    {
                        single = reader.ReadLine();
                        if (single == null) break;
                        name = single.Split('=');
                        if (name.Length > 0)
                        {
                            param.Add(name[0].Trim(), name[1].Trim());
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("文件不存在");
            }
            return param;
        }

        /// <summary>
        /// 保存数据到文本文件中
        /// </summary>
        /// <param name="dictionary"></param>
        public void WriteTxt(Dictionary<string, string> dictionary, string path)
        {
            int length = dictionary.Count;
            string[] keys = new string[length];
            string[] value = new string[length];
            dictionary.Keys.CopyTo(keys, 0);
            dictionary.Values.CopyTo(value, 0);
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    for (int i = 0; i < length; i++)
                    {
                        sw.WriteLine("{0}{1}{2}", keys[i].Trim(), "=", value[i].Trim());
                    }
                }
            }
            catch
            {
                MessageBox.Show("参数保存成功");
            }
        }

        /// <summary>
        /// 打开一个文件
        /// </summary>
        /// <param name="cons"></param>
        public string OpenFile()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "ply文件(*.ply)|*.ply|Txt文件(*.txt)|*.txt|om3文件(*.om3)|*.om3|STL文件(*.stl)|*.stl|obj文件(*.obj)|*.obj|off文件(*.off)|*.off|dxf文件(*.dxf)|*.dxf|csv文件(.csv)|*.csv|所有文件(*.*)|*.**";
                ofd.RestoreDirectory = false;
                ofd.FilterIndex = 2;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public string SaveCameParam()
        {
            try
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.Filter = "ply文件(*.dat)|*.dat|所有文件(*.*)|*.**";
                ofd.RestoreDirectory = false;
                ofd.FilterIndex = 0;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public string OpenCameParam()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "ply文件(*.dat)|*.dat|所有文件(*.*)|*.**";
                ofd.RestoreDirectory = false;
                ofd.FilterIndex = 0;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        public void ReadCameParam(string path, out HTuple camPara)
        {
            camPara = null;
            try
            {
                HOperatorSet.ReadCamPar(path, out camPara);
            }
            catch
            {
            }
        }

        public void ReadHmatrix(string path, out HMatrix hMatrix)
        {
            hMatrix = new HMatrix();
            try
            {
                hMatrix.ReadMatrix(path);
            }
            catch
            {
            }
        }
        public void SaveCameParam(string path, HTuple camPara)
        {
            camPara = null;
            try
            {
                HOperatorSet.WriteCamPar(path, camPara);
            }
            catch
            {
            }
        }
        public void ReadCamePose(string path, out HTuple camPose)
        {
            camPose = null;
            try
            {
                HOperatorSet.ReadPose(path, out camPose);
            }
            catch
            {
                camPose = new HTuple(0, 0, 0, 0, 0, 0, 0);
            }
        }
        public void SaveCamePose(string path, HTuple camPose)
        {
            if (camPose == null) throw new ArgumentNullException("camPose");
            HOperatorSet.WritePose(path, camPose);
        }
        public string OpenCalibObjDescr()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "ply文件(*.descr)|*.descr|所有文件(*.*)|*.**";
                ofd.RestoreDirectory = false;
                ofd.FilterIndex = 0;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        public string OpenImage()
        {
            try
            {

                // hobj, .ima, .tif, .tiff, .gif, .bmp, .jpg, .jpeg, .jp2, .jxr, .png, .pcx, .ras, .xwd, .pbm, .pnm, .pgm, .ppm
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "bmp文件(*.bmp)|*.bmp|hobj文件(*.hobj)|*.hobj|tiff文件(*.tiff)|*.tiff|jpg文件(*.jpg)|*.jpg|jpeg文件(*.jpeg)|*.jpeg|png文件(*.png)|*.png|ras文件(*.ras)|*.ras|dxf文件(.dxf)|*.dxf|hdev文件(.hdev)|*.hdev|所有文件(*.*)|*.**";
                ofd.RestoreDirectory = false;
                ofd.FilterIndex = 0;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public string OpenXldCont()
        {
            try
            {

                // hobj, .ima, .tif, .tiff, .gif, .bmp, .jpg, .jpeg, .jp2, .jxr, .png, .pcx, .ras, .xwd, .pbm, .pnm, .pgm, .ppm
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "dxf文件(*.dxf)|*.dxf|hobj文件(*.hobj)|*.hobj|tiff文件(*.tiff)|*.tiff|jpg文件(*.jpg)|*.jpg|jpeg文件(*.jpeg)|*.jpeg|png文件(*.png)|*.png|ras文件(*.ras)|*.ras|dxf文件(.dxf)|*.dxf|所有文件(*.*)|*.**";
                ofd.RestoreDirectory = false;
                ofd.FilterIndex = 0;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        public string SaveImage()
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "bmp文件(*.bmp)|*.bmp|hobj文件(*.hobj)|*.hobj|tiff文件(*.tiff)|*.tiff|jpg文件(*.jpg)|*.jpg|jpeg文件(*.jpeg)|*.jpeg|png文件(*.png)|*.png|ras文件(*.ras)|*.ras|dxf文件(.dxf)|*.dxf|所有文件(*.*)|*.**";
            fileDialog.RestoreDirectory = false;
            fileDialog.FilterIndex = 0;
            fileDialog.DefaultExt = ".bmp";
            fileDialog.ShowDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
                return fileDialog.FileName;
            else
                return null;
        }

        public string SaveXLdCont()
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "dxf文件(*.dxf)|*.bmp|hobj文件(*.hobj)|*.hobj|tiff文件(*.tiff)|*.tiff|jpg文件(*.jpg)|*.jpg|jpeg文件(*.jpeg)|*.jpeg|png文件(*.png)|*.png|ras文件(*.ras)|*.ras|dxf文件(.dxf)|*.dxf|所有文件(*.*)|*.**";
            fileDialog.RestoreDirectory = false;
            fileDialog.FilterIndex = 0;
            fileDialog.DefaultExt = ".bmp";
            fileDialog.ShowDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
                return fileDialog.FileName;
            else
                return null;
        }
        public string OpenFolder()
        {
            string path = "";
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            path = folderBrowserDialog.SelectedPath;
            if (folderBrowserDialog.SelectedPath.Contains("任务"))
                path = new FileInfo(folderBrowserDialog.SelectedPath).DirectoryName;  // 不能到任务那一级
            return path;
        }
        public string OpenFile(int index)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "ply文件(*.ply)|*.ply|Txt文件(*.txt)|*.txt|om3文件(*.om3)|*.om3|STL文件(*.stl)|*.stl|obj文件(*.obj)|*.obj|off文件(*.off)|*.off|dxf文件(*.dxf)|*.dxf|csv文件(.csv)|*.csv|所有文件(*.*)|*.**";
                ofd.RestoreDirectory = false;
                ofd.FilterIndex = index;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        public string SaveFile()
        {
            string path = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".om3";
            sfd.Filter = "ply文件(*.ply)|*.ply|Txt文件(*.txt)|*.txt|om3文件(*.om3)|*.om3|STL文件(*.stl)|*.stl|obj文件(*.obj)|*.obj|off文件(*.off)|*.off|dxf文件(*.dxf)|*.dxf|csv文件(.csv)|*.csv|所有文件(*.*)|*.**";
            sfd.RestoreDirectory = false;
            sfd.FilterIndex = 3;
            if (sfd.ShowDialog() == DialogResult.OK)
                path = sfd.FileName;
            return path;
        }
        public string SaveFile(int index)
        {
            string path = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".om3";
            sfd.Filter = "ply文件(*.ply)|*.ply|Txt文件(*.txt)|*.txt|om3文件(*.om3)|*.om3|STL文件(*.stl)|*.stl|obj文件(*.obj)|*.obj|off文件(*.off)|*.off|dxf文件(*.dxf)|*.dxf|csv文件(.csv)|*.csv|所有文件(*.*)|*.**";
            sfd.RestoreDirectory = false;
            sfd.FilterIndex = index;
            if (sfd.ShowDialog() == DialogResult.OK)
                path = sfd.FileName;
            return path;
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="DataX"></param>
        /// <param name="DataY"></param>
        /// <param name="DataZ"></param>
        public void ReadTxt(string Path, double unit, out double[] DataX, out double[] DataY, out double[] DataZ)
        {
            List<double> DaX = new List<double>();
            List<double> DaY = new List<double>();
            List<double> DaZ = new List<double>();
            int unm = 0;
            string line;
            string[] st;
            try
            {
                if (File.Exists(Path))
                {
                    using (StreamReader reader = new StreamReader(Path, Encoding.Default))
                    {
                        while (true)
                        {
                            line = reader.ReadLine();
                            if (line == null)
                            {
                                unm++;
                                if (unm > 100) break;
                            }
                            else
                            {
                                st = line.Split('\t', ' ', ';');//如果其他分隔符替换掉就OK了  , ',',' ',';'
                                if (st.Length >= 3)
                                {
                                    double value;
                                    DaX.Add(Convert.ToDouble(st[0]) * unit);
                                    DaY.Add(Convert.ToDouble(st[1]) * unit);
                                    if (double.TryParse(st[2], out value))
                                        DaZ.Add(value * unit);
                                    else
                                        DaZ.Add(0);
                                }
                                else
                                {
                                    MessageBox.Show("数据分割符错误");
                                }
                            }
                        }

                    }
                }

                else
                {
                    Console.WriteLine("文件不存在");
                }
            }
            catch
            {

            }
            DataX = DaX.ToArray();
            DataY = DaY.ToArray();
            DataZ = DaZ.ToArray();
        }

        /// <summary>
        ///  找开程序
        /// </summary>
        /// <returns></returns>
        public List<TreeNode> OpenProgram(string fileName)
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            try
            {
                using (Stream fStream = File.OpenRead(fileName))
                {
                    return (List<TreeNode>)binFormat.Deserialize(fStream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// 保存程序
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="items"></param>
        public bool SaveProgram(string fileName, List<TreeNode> items)
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            try
            {
                using (FileStream fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    binFormat.Serialize(fStream, items);
                }
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                return false;
            }
        }
        public bool SaveProgram(string fileName, object items)
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            try
            {
                using (FileStream fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    binFormat.Serialize(fStream, items);
                }
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                return false;
            }
        }

        /// <summary>
        /// 程序另存为
        /// </summary>
        /// <param name="items"></param>
        public void SaveAsProgram(List<TreeNode> items)
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            string fileName = SaveFile(2);
            try
            {
                using (FileStream fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    binFormat.Serialize(fStream, items);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        /// <summary>
        ///  读取配置参数,如果没有指定参数文件的名称，则使用默认的名称："paramConfig.txt"
        /// </summary>
        /// <returns></returns>
        public object ReadConfigParam(string configFileName)
        {
            string path = configFileName;
            if (!File.Exists(path)) return null;
            BinaryFormatter binFormat = new BinaryFormatter();
            try
            {
                using (Stream fStream = File.OpenRead(path))
                {
                    return binFormat.Deserialize(fStream);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// 保存程序
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="config"></param>
        public void SaveConfigParam(string configFileName, object config)
        {
            string path = configFileName;
            BinaryFormatter binFormat = new BinaryFormatter();
            try
            {
                using (FileStream fStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    binFormat.Serialize(fStream, config);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }




    }
}
