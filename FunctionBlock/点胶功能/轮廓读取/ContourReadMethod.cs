using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using View;

namespace FunctionBlock
{
    [Serializable]
    public class ContourReadMethod
    {

        public static bool ReadTrack(ContourReadParam param, out userWcsPolyLine wcsPolyLine)
        {
            bool result = false;
            if (param == null) throw new ArgumentNullException("param");
            wcsPolyLine = new userWcsPolyLine(new CameraParam());
            switch (param.Extension)
            {
                case "*.txt":
                case "*.csv":
                    using (StreamReader sr = new StreamReader(param.Path))
                    {
                        int count = 0;
                        double x, y, z, u, v, w;
                        while (true)
                        {
                            string line = sr.ReadLine();
                            if (line != null)
                            {
                                string[] data = line.Split(',', ';');
                                if (data.Length > 0)
                                    x = Convert.ToDouble(data[0].Trim());
                                else
                                    x = 0;
                                if (data.Length > 1)
                                    y = Convert.ToDouble(data[1].Trim());
                                else
                                    y = 0;
                                if (data.Length > 2)
                                    z = Convert.ToDouble(data[2].Trim());
                                else
                                    z = 0;
                                if (data.Length > 3)
                                    u = Convert.ToDouble(data[3].Trim());
                                else
                                    u = 0;
                                if (data.Length > 4)
                                    v = Convert.ToDouble(data[4].Trim());
                                else
                                    v = 0;
                                if (data.Length > 5)
                                    w = Convert.ToDouble(data[5].Trim());
                                else
                                    w = 0;
                                wcsPolyLine.Add(x, y, z, u, v, w);
                            }
                            else
                                count++;
                            if (count > 10) break;
                        }
                    }
                    result = true;
                    break;
                case "*.dxf":
                    HXLDCont hXLDCont = new HalconDotNet.HXLDCont();
                    hXLDCont.ReadContourXldDxf(param.Path, "max_approx_error", 0.005);
                    HTuple row, col;
                    if (hXLDCont != null && hXLDCont.IsInitialized())
                    {
                        int num = hXLDCont.CountObj();
                        for (int i = 1; i <= num; i++)
                        {
                            hXLDCont.SelectObj(i).GetContourXld(out row, out col);
                            if (row != null && row.Length > 0)
                            {
                                wcsPolyLine.AddRange(col.DArr, row.DArr);
                            }
                        }
                    }
                    result = true;
                    break;
                default:
                    break;
            }
            return result;
        }

        public static bool ReadTrack(ContourReadParam param, userWcsCoordSystem[] wcsCoordSystems, out userWcsPolyLine wcsPolyLine)
        {
            bool result = false;
            if (param == null) throw new ArgumentNullException("param");
            wcsPolyLine = new userWcsPolyLine(new CameraParam());
            switch (param.Extension)
            {
                case "*.txt":
                case "*.csv":
                    using (StreamReader sr = new StreamReader(param.Path))
                    {
                        int count = 0;
                        double x, y, z, u, v, w;
                        while (true)
                        {
                            string line = sr.ReadLine();
                            if (line != null)
                            {
                                string[] data = line.Split(',', ';');
                                if (data.Length > 0)
                                    x = Convert.ToDouble(data[0].Trim());
                                else
                                    x = 0;
                                if (data.Length > 1)
                                    y = Convert.ToDouble(data[1].Trim());
                                else
                                    y = 0;
                                if (data.Length > 2)
                                    z = Convert.ToDouble(data[2].Trim());
                                else
                                    z = 0;
                                if (data.Length > 3)
                                    u = Convert.ToDouble(data[3].Trim());
                                else
                                    u = 0;
                                if (data.Length > 4)
                                    v = Convert.ToDouble(data[4].Trim());
                                else
                                    v = 0;
                                if (data.Length > 5)
                                    w = Convert.ToDouble(data[5].Trim());
                                else
                                    w = 0;
                                wcsPolyLine.Add(x, y, z, u, v, w);
                            }
                            else
                                count++;
                            if (count > 10) break;
                        }
                    }
                    result = true;
                    break;
                case "*.dxf":
                    HXLDCont hXLDCont = new HalconDotNet.HXLDCont();
                    hXLDCont.ReadContourXldDxf(param.Path, "max_approx_error", 0.005);
                    HTuple row, col;
                    if (hXLDCont != null && hXLDCont.IsInitialized())
                    {
                        int num = hXLDCont.CountObj();
                        for (int i = 1; i <= num; i++)
                        {
                            hXLDCont.SelectObj(i).GetContourXld(out row, out col);
                            if (row != null && row.Length > 0)
                            {
                                wcsPolyLine.AddRange(col.DArr, row.DArr);
                            }
                        }
                    }
                    result = true;
                    break;
                default:
                    break;
            }



            return result;
        }


    }

}
