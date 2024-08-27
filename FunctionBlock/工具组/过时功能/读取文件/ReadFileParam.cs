using System;
using System.Collections.Generic;
using HalconDotNet;
using System.Windows.Forms;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;
using Sensor;
using System.IO;
using System.Linq;
using System.Text;

namespace FunctionBlock
{
    [Serializable]
    public class ReadFileParam
    {
        public List<string> FilePath { get; set; }
        public string SingleFilePath { get; set; }
        public string FolderPath { get; set; }

        public userWcsVector GrabPoint { get; set; }

        public ReadFileParam()
        {
            this.FilePath = new List<string>();
            this.SingleFilePath = "";
            this.FolderPath = "";
            this.GrabPoint = new userWcsVector(100, 100, 0, 0);
        }

        public CoordPoint[] ReadFile(string path)
        {
            bool result = false;
            string[] extenName = path.Split('.');
            //////////////////////
            List<CoordPoint> coordList = new List<CoordPoint>();
            int unm = 0;
            string line;
            string[] st;
            string rowHead = "";
            int rowIndex = 0;
            CoordPoint coordPoint;
            try
            {
                if (File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(path, Encoding.Default))
                    {
                        while (true)
                        {
                            line = reader.ReadLine();
                            if (line == null || line.Length == 0)
                            {
                                unm++;
                                if (unm > 100) break;
                            }
                            else
                            {
                                unm = 0;
                                st = line.Split(',', '=');
                                if (st[0] == "[GlassMarks]")
                                {
                                    coordPoint = new CoordPoint(4);
                                    coordPoint.Sign = st[0];
                                    coordList.Add(coordPoint);
                                    for (int i = 0; i < 4; i++)
                                    {
                                        line = reader.ReadLine();
                                        st = line.Split(',', '=');
                                        coordPoint.Row[i] = -1;
                                        coordPoint.Col[i] = -1;
                                        coordPoint.X[i] = Convert.ToDouble(st[0]);
                                        coordPoint.Y[i] = Convert.ToDouble(st[1]);
                                    }
                                }
                                else
                                {
                                    switch(st.Length)
                                    {
                                        case 1:
                                            rowHead = st[0];
                                            string ss = rowHead.Split(new string[] { "Row"}, StringSplitOptions.RemoveEmptyEntries)[1].Replace("]","");
                                            rowIndex = Convert.ToInt32(ss);
                                            break;
                                        case 2:
                                            if (st[0] == "Count")
                                            {
                                                coordPoint = new CoordPoint(Convert.ToInt32(st[1]));
                                                coordPoint.Sign = rowHead;
                                                coordList.Add(coordPoint);
                                                for (int i = 0; i < coordPoint.Count; i++)
                                                {
                                                    line = reader.ReadLine();
                                                    st = line.Split(',', '=');
                                                    coordPoint.Row[i] = rowIndex;
                                                    coordPoint.Col[i] = i;
                                                    coordPoint.X[i] = Convert.ToDouble(st[0]);
                                                    coordPoint.Y[i] = Convert.ToDouble(st[1]);
                                                }
                                            }
                                            break;
                                    }
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

            return coordList.ToArray() ;
        }
    }
}
