using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class LoggerHelper
    {
        //创建默认类型的日志对象（传递对象类型typeof(Program)可以自动记录类名称）// 默认的日志对象只输出文件
        //private static ILog logger = LogManager.GetLogger(typeof(LoggerHelper));
        //创建指定类型的日志对象
        //private static ILog logger = LogManager.GetLogger("loggerColoredConsole");

        private static Logger  logger = new Logger();
        public static void Debug(object message)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug(message);
            }
        }
        public static void Debug(object message, Exception exception)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug(message, exception);
            }
        }
        public static void Info(object message)
        {
            if (logger.IsInfoEnabled)
            {
                logger.Info(message);
            }
        }
        public static void Info(object message, Exception exception)
        {
            if (logger.IsInfoEnabled)
            {
                logger.Info(message, exception);
            }
        }
        public static void Warn(object message)
        {
            if (logger.IsWarnEnabled)
            {
                logger.Warn(message);
            }
        }
        public static void Warn(object message, Exception exception)
        {
            if (logger.IsWarnEnabled)
            {
                logger.Warn(message, exception);
            }
        }
        public static void Error(object message)
        {
            if (logger.IsErrorEnabled)
            {
                logger.Error(message);
            }

        }
        public static void Error(object message, Exception exception)
        {
            if (logger.IsErrorEnabled)
            {
                logger.Error(message, exception);
            }
        }
        public static void Fatal(object message)
        {
            if (logger.IsFatalEnabled)
            {
                logger.Fatal(message);
            }

        }
        public static void Fatal(object message, Exception exception)
        {
            if (logger.IsFatalEnabled)
            {
                logger.Fatal(message, exception);
            }
        }





    }
}
