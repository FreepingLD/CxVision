using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Common
{
    public class Logger
    {
        private string locaLcatalog = "D:\\应用程序日志";
        private TraceSource mySource ;
        private ConsoleTraceListener console;
        private TextWriterTraceListener textListener;
        private string fileName = "";
        private bool isDebugEnabled = true;
        private bool isInfoEnabled = true;
        private bool isWarnEnabled = true;
        private bool isErrorEnabled = true;
        private bool isFatalEnabled = true;

        public bool IsDebugEnabled { get => isDebugEnabled; set => isDebugEnabled = value; }
        public bool IsInfoEnabled { get => isInfoEnabled; set => isInfoEnabled = value; }
        public bool IsWarnEnabled { get => isWarnEnabled; set => isWarnEnabled = value; }
        public bool IsErrorEnabled { get => isErrorEnabled; set => isErrorEnabled = value; }
        public bool IsFatalEnabled { get => isFatalEnabled; set => isFatalEnabled = value; }

        public Logger()
        {
            mySource = new TraceSource("程序日志", SourceLevels.All);
            mySource.Listeners.Remove("Default");
            // 添加控制台侦听器
            console = new ConsoleTraceListener(false);
            console.Filter = new EventTypeFilter(SourceLevels.All);
            console.Name = "console";
            mySource.Listeners.Add(console);
            // 添加文本侦听器
            if (!Directory.Exists(locaLcatalog))
                Directory.CreateDirectory(locaLcatalog);
            if (!Directory.Exists(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd")))
                Directory.CreateDirectory(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd"));
            //////////////////////////////////////////
            textListener = new TextWriterTraceListener(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + "程序日志.txt"); 
            textListener.Filter = new EventTypeFilter(SourceLevels.All);
            textListener.Name = "text";
            mySource.Listeners.Add(textListener);
        }
        public Logger(string  name )
        {
            fileName = name;
            mySource = new TraceSource(name, SourceLevels.All);
            mySource.Listeners.Remove("Default");
            // 添加控制台侦听器
            ConsoleTraceListener console = new ConsoleTraceListener(false);
            console.Filter = new EventTypeFilter(SourceLevels.All);
            console.Name = "console";
            mySource.Listeners.Add(console);
            // 添加文本侦听器
            if (!Directory.Exists(locaLcatalog))
                Directory.CreateDirectory(locaLcatalog);
            if (!Directory.Exists(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd")))
                Directory.CreateDirectory(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd"));
            ////////////////////////////////////////////////////
            textListener = new TextWriterTraceListener(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + name + ".txt");
            textListener.Filter = new EventTypeFilter(SourceLevels.All);
            textListener.Name = "text";
            mySource.Listeners.Add(textListener);
        }

        private void CreateTextListener()
        {
            if (!Directory.Exists(locaLcatalog))
                Directory.CreateDirectory(locaLcatalog);
            if (!Directory.Exists(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd")))
            {
                Directory.CreateDirectory(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd"));
                if (mySource.Listeners.Contains(textListener))
                {
                    mySource.Listeners.Remove(textListener);
                    textListener.Dispose();
                }
                if (string.IsNullOrEmpty(this.fileName))
                    textListener = new TextWriterTraceListener(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + "程序日志.txt");
                else
                    textListener = new TextWriterTraceListener(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + this.fileName + ".txt");
                textListener.Filter = new EventTypeFilter(SourceLevels.All);
                textListener.Name = "text";
                mySource.Listeners.Add(textListener);
            }
        }
        public  void Debug(object message)
        {
            if (this.IsDebugEnabled)
            {
                CreateTextListener();
                mySource.TraceData(TraceEventType.Verbose, 0, DateTime.Now.ToString("yy//MM//dd//hh:mm:ss:ff")+ "-"+ message);
                mySource.Flush();
            }             
        }
        public  void Debug(object message, Exception exception)
        {
            if (this.IsDebugEnabled)
            {
                CreateTextListener();
                mySource.TraceData(TraceEventType.Verbose, 0, DateTime.Now.ToString("yy//MM//dd//hh:mm:ss:ff"), "-", message, exception);
                mySource.Flush();
            }
        }
        public  void Info(object message)
        {
            if (this.IsInfoEnabled)
            {
                CreateTextListener();
                mySource.TraceData(TraceEventType.Information, 0, DateTime.Now.ToString("yy//MM//dd//hh:mm:ss:ff") + "-" + message);
                mySource.Flush();
            }
        }
        public  void Info(object message, Exception exception)
        {
            if (this.IsInfoEnabled)
            {
                CreateTextListener();
                mySource.TraceData(TraceEventType.Information, 0, DateTime.Now.ToString("yy//MM//dd//hh:mm:ss:ff"), "-", message, exception);
                mySource.Flush();
            }
        }
        public  void Warn(object message)
        {
            if (this.IsWarnEnabled)
            {
                CreateTextListener();
                mySource.TraceData(TraceEventType.Warning, 0, DateTime.Now.ToString("yy//MM//dd//hh:mm:ss:ff") + "-" + message);
                mySource.Flush();
            }
        }
        public  void Warn(object message, Exception exception)
        {
            if (this.IsWarnEnabled)
            {
                CreateTextListener();
                mySource.TraceData(TraceEventType.Warning, 0, DateTime.Now.ToString("yy//MM//dd//hh:mm:ss:ff"), "-", message, exception);
                mySource.Flush();
            }
        }
        public  void Error(object message)
        {
            if (this.IsErrorEnabled)
            {
                CreateTextListener();
                mySource.TraceData(TraceEventType.Error, 0, DateTime.Now.ToString("yy//MM//dd//hh:mm:ss:ff") + "-" + message);
                mySource.Flush();
            }
        }
        public  void Error(object message, Exception exception)
        {
            if (this.IsErrorEnabled)
            {
                CreateTextListener();
                mySource.TraceData(TraceEventType.Error, 0, DateTime.Now.ToString("yy//MM//dd//hh:mm:ss:ff"), "-", message, exception);
                mySource.Flush();
            }
        }
        public  void Fatal(object message)
        {
            if (this.IsFatalEnabled)
            {
                CreateTextListener();
                mySource.TraceData(TraceEventType.Critical, 0, DateTime.Now.ToString("yy//MM//dd//hh:mm:ss:ff") + "-" + message);
                mySource.Flush();
            }
        }
        public  void Fatal(object message, Exception exception)
        {
            if (this.IsFatalEnabled)
            {
                CreateTextListener();
                mySource.TraceData(TraceEventType.Critical, 0, DateTime.Now.ToString("yy//MM//dd//hh:mm:ss:ff"), "-", message, exception);
                mySource.Flush();
            }
        }



    }
}
