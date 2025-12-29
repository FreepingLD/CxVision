using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class ContourExportMethod
    {

        public static bool ExportTrack(userWcsPoint[] wcsPoint, ContourExportParam param)
        {
            bool result = false;
            if (wcsPoint == null) throw new ArgumentNullException("wcsPoint");
            if (param == null) throw new ArgumentNullException("param");
            if (wcsPoint.Length == 0) return false;
            switch(param.DataType)
            {
                case "xyz":
                    using (StreamWriter sw = new StreamWriter(param.Path, false))
                    {
                        foreach (var item in wcsPoint)
                        {
                            sw.WriteLine(string.Join(",", item.X, item.Y, item.Z));
                        }
                    }
                    break;
                case "xyzuvw":
                case "xyzabc":
                    using (StreamWriter sw = new StreamWriter(param.Path, false))
                    {
                        foreach (var item in wcsPoint)
                        {
                            sw.WriteLine(string.Join(",", item.X, item.Y, item.Z,item.U, item.V, item.Theta));
                        }
                    }
                    break;
                case "all":
                    using (StreamWriter sw = new StreamWriter(param.Path, false))
                    {
                        foreach (var item in wcsPoint)
                        {
                            sw.WriteLine(string.Join(",", item.X, item.Y, item.Z, item.U, item.V, item.Theta));
                        }
                    }
                    break;
            }
            result = true;
            return result;
        }


    }

}
