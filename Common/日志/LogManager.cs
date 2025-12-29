using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class LogManager
    {
        private static Dictionary<string, Logger> loggerDic = new Dictionary<string, Logger>();

        public static Logger GetLogger(string name)
        {
            if (loggerDic.ContainsKey(name))
                return loggerDic[name];
            else
            {
                loggerDic.Add(name, new Logger(name));
                return loggerDic[name];
            }
        }
        


    }
}
