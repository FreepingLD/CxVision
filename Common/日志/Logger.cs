using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.CompilerServices;

namespace Common
{
    public class Logger
    {
        private string locaLcatalog = "D:\\应用程序日志";
        private TraceSource mySource;
        private Dictionary<string, ListView> myListView;
        private ConsoleTraceListener console;
        private TextWriterTraceListener textListener;
        private EventLogTraceListener _eventLog;
        private string fileName = "";
        private bool isDebugEnabled = true;
        private bool isInfoEnabled = true;
        private bool isWarnEnabled = true;
        private bool isErrorEnabled = true;
        private bool isFatalEnabled = true;
        private bool isListBox = false;

        public bool IsDebugEnabled { get => isDebugEnabled; set => isDebugEnabled = value; }
        public bool IsInfoEnabled { get => isInfoEnabled; set => isInfoEnabled = value; }
        public bool IsWarnEnabled { get => isWarnEnabled; set => isWarnEnabled = value; }
        public bool IsErrorEnabled { get => isErrorEnabled; set => isErrorEnabled = value; }
        public bool IsFatalEnabled { get => isFatalEnabled; set => isFatalEnabled = value; }
        public bool IsListView { get => isListBox; set => isListBox = value; }
        public Dictionary<string, ListView> MyListView { get => myListView; set => myListView = value; }

        public Logger()
        {
            mySource = new TraceSource("程序日志", SourceLevels.All);
            mySource.Listeners.Remove("Default");
            // 添加控制台侦听器
            //console = new ConsoleTraceListener(false);
            //console.Filter = new EventTypeFilter(SourceLevels.All);
            //console.Name = "console";
            //mySource.Listeners.Add(console);
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
        public Logger(string fileName)
        {
            this.fileName = fileName;
            mySource = new TraceSource(fileName, SourceLevels.All);
            mySource.Listeners.Remove("Default");
            // 添加控制台侦听器
            //ConsoleTraceListener console = new ConsoleTraceListener(false);
            //console.Filter = new EventTypeFilter(SourceLevels.All);
            //console.Name = "console";
            //mySource.Listeners.Add(console);
            // 添加文本侦听器
            if (!Directory.Exists(locaLcatalog))
                Directory.CreateDirectory(locaLcatalog);
            if (!Directory.Exists(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd")))
                Directory.CreateDirectory(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd"));
            ////////////////////////////////////////////////////
            textListener = new TextWriterTraceListener(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + fileName + ".txt");
            textListener.Filter = new EventTypeFilter(SourceLevels.All);
            textListener.Name = "text";
            mySource.Listeners.Add(textListener);
        }

        private void CreateTextListener(string fileName = null)
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
                textListener = new TextWriterTraceListener(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + "程序日志.txt");
                //if (string.IsNullOrEmpty(fileName))
                //    textListener = new TextWriterTraceListener(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + "程序日志.txt");
                //else
                //    textListener = new TextWriterTraceListener(locaLcatalog + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + fileName + ".txt");
                textListener.Filter = new EventTypeFilter(SourceLevels.All);
                textListener.Name = "text";
                mySource.Listeners.Add(textListener);
            }
        }

        public void Debug(object message, string fileName = null, string camName = "NONE")
        {
            if (this.IsDebugEnabled)
            {
                CreateTextListener(fileName);
                mySource.TraceData(TraceEventType.Verbose, (int)TraceEventType.Verbose, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff  ") + message); //+ " [Debug] "
                //mySource.TraceInformation("{0}{1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"), " [Debug] " , message);
                mySource.Flush();
            }
            if (IsListView)
            {
                if (this.myListView == null || this.myListView.Count == 0 || camName == null) return;
                if (this.myListView.ContainsKey(camName))
                {
                    if (this.myListView[camName] == null) return;
                    this.myListView[camName].BeginInvoke(new Action(() =>
                    {
                        string dateTm = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"));
                        ListViewItem item = new ListViewItem();
                        item.ForeColor = System.Drawing.Color.Black;
                        item.Text = " [Debug] ";
                        item.SubItems.Add(dateTm + " " + message);
                        this.myListView[camName].Items.Add(item);
                        int index = this.myListView[camName].Items.Count;
                        this.myListView[camName].EnsureVisible(index - 1);
                        if (this.myListView[camName].Items.Count > 2000)
                            this.myListView[camName].Items.RemoveAt(0);
                    }));
                }
            }
        }
        public void Debug(object message, Exception exception, string fileName = null, string camName = "NONE")
        {
            if (this.IsDebugEnabled)
            {
                CreateTextListener(fileName);
                mySource.TraceData(TraceEventType.Verbose, (int)TraceEventType.Verbose, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff  ") + message, exception); //+ " [Debug] "
                //mySource.TraceInformation("{0}{1}{2}{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"), " [Debug] ", message, exception);
                mySource.Flush();
            }
            if (IsListView)
            {
                if (this.myListView == null || this.myListView.Count == 0 || camName == null) return;
                if (this.myListView.ContainsKey(camName))
                {
                    if (this.myListView[camName] == null) return;
                    this.myListView[camName].BeginInvoke(new Action(() =>
                    {
                        string dateTm = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"));
                        ListViewItem item = new ListViewItem();
                        item.ForeColor = System.Drawing.Color.Black;
                        item.Text = " [Debug] ";
                        item.SubItems.Add(dateTm + " " + message + exception.ToString());
                        this.myListView[camName].Items.Add(item);
                        int index = this.myListView[camName].Items.Count;
                        this.myListView[camName].EnsureVisible(index - 1);
                        if (this.myListView[camName].Items.Count > 2000)
                            this.myListView[camName].Items.RemoveAt(0);
                    }));
                }
            }
        }
        public void Info(object message, string fileName = null, string camName = "NONE")
        {
            if (this.IsInfoEnabled)
            {
                CreateTextListener(fileName);
                mySource.TraceData(TraceEventType.Information, (int)TraceEventType.Information, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff  ") + message); //+ " [Info] "
                //mySource.TraceInformation("{0}{1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"), " [Info] " , message);
                mySource.Flush();
            }
            if (IsListView)
            {
                if (this.myListView == null || this.myListView.Count == 0 || camName == null) return;
                if (this.myListView.ContainsKey(camName))
                {
                    if (this.myListView[camName] == null) return;
                    this.myListView[camName].BeginInvoke(new Action(() =>
                    {
                        string dateTm = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"));
                        ListViewItem item = new ListViewItem();
                        item.ForeColor = System.Drawing.Color.Black;
                        item.Text = " [Info] ";
                        item.SubItems.Add(dateTm + " " + message);
                        this.myListView[camName].Items.Add(item);
                        int index = this.myListView[camName].Items.Count;
                        this.myListView[camName].EnsureVisible(index - 1);
                        if (this.myListView[camName].Items.Count > 2000)
                            this.myListView[camName].Items.RemoveAt(0);
                    }));
                }

            }
        }
        public void Info(object message, Exception exception, string fileName = null, string camName = "NONE")
        {
            if (this.IsInfoEnabled)
            {
                CreateTextListener(fileName);
                mySource.TraceData(TraceEventType.Information, (int)TraceEventType.Information, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff  ") + message, exception); //+ " [Info] "
                //mySource.TraceInformation("{0}{1}{2}{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"), " [Info] ", message, exception);
                mySource.Flush();
            }
            if (IsListView)
            {
                if (this.myListView == null || this.myListView.Count == 0 || camName == null) return;
                if (this.myListView.ContainsKey(camName))
                {
                    if (this.myListView[camName] == null) return;
                    this.myListView[camName].BeginInvoke(new Action(() =>
                    {
                        string dateTm = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"));
                        ListViewItem item = new ListViewItem();
                        item.ForeColor = System.Drawing.Color.Black;
                        item.Text = " [Info] ";
                        item.SubItems.Add(dateTm + " " + message + exception.ToString());
                        this.myListView[camName].Items.Add(item);
                        int index = this.myListView[camName].Items.Count;
                        this.myListView[camName].EnsureVisible(index - 1);
                        if (this.myListView[camName].Items.Count > 2000)
                            this.myListView[camName].Items.RemoveAt(0);
                    }));
                }

            }
        }
        public void Warn(object message, string fileName = null, string camName = "NONE")
        {
            if (this.IsWarnEnabled)
            {
                CreateTextListener(fileName);
                mySource.TraceData(TraceEventType.Warning, (int)TraceEventType.Warning, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff  ") + message); //+ " [Warn] "
                //mySource.TraceInformation("{0}{1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"), " [Warn] " , message);
                mySource.Flush();
            }
            if (IsListView)
            {
                if (this.myListView == null || this.myListView.Count == 0 || camName == null) return;
                if (this.myListView.ContainsKey(camName))
                {
                    if (this.myListView[camName] == null) return;
                    this.myListView[camName].BeginInvoke(new Action(() =>
                    {
                        string dateTm = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"));
                        ListViewItem item = new ListViewItem();
                        item.ForeColor = System.Drawing.Color.Orange;
                        item.Text = " [Warn] ";
                        item.SubItems.Add(dateTm + " " + message);
                        this.myListView[camName].Items.Add(item);
                        int index = this.myListView[camName].Items.Count;
                        this.myListView[camName].EnsureVisible(index - 1);
                        if (this.myListView[camName].Items.Count > 2000)
                            this.myListView[camName].Items.RemoveAt(0);
                    }));
                }
            }
        }
        public void Warn(object message, Exception exception, string fileName = null, string camName = "NONE")
        {
            if (this.IsWarnEnabled)
            {
                CreateTextListener(fileName);
                mySource.TraceData(TraceEventType.Warning, (int)TraceEventType.Warning, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff  ") + message, exception); //+ " [Warn] "
                //mySource.TraceInformation("{0}{1}{2}{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"), " [Warn] ", message, exception);
                mySource.Flush();
            }
            if (IsListView)
            {
                if (this.myListView == null || this.myListView.Count == 0 || camName == null) return;
                if (this.myListView.ContainsKey(camName))
                {
                    if (this.myListView[camName] == null) return;
                    this.myListView[camName].BeginInvoke(new Action(() =>
                    {
                        string dateTm = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"));
                        ListViewItem item = new ListViewItem();
                        item.ForeColor = System.Drawing.Color.Orange;
                        item.Text = " [Warn] ";
                        item.SubItems.Add(dateTm + " " + message + exception.ToString());
                        this.myListView[camName].Items.Add(item);
                        int index = this.myListView[camName].Items.Count;
                        this.myListView[camName].EnsureVisible(index - 1);
                        if (this.myListView[camName].Items.Count > 2000)
                            this.myListView[camName].Items.RemoveAt(0);
                    }));
                }
            }
        }
        public void Error(object message, string fileName = null, string camName = "NONE")
        {
            if (this.IsErrorEnabled)
            {
                CreateTextListener(fileName);
                mySource.TraceData(TraceEventType.Error, (int)TraceEventType.Error, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff  ") + message); //+ " [Error] "
                //mySource.TraceInformation("{0}{1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"), " [Error] " , message);
                mySource.Flush();
            }
            if (IsListView)
            {
                if (this.myListView == null || this.myListView.Count == 0 || camName == null) return;
                if (this.myListView.ContainsKey(camName))
                {
                    if (this.myListView[camName] == null) return;
                    this.myListView[camName].BeginInvoke(new Action(() =>
                    {
                        string dateTm = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"));
                        ListViewItem item = new ListViewItem();
                        item.ForeColor = System.Drawing.Color.Red;
                        item.Text = " [Error] ";
                        item.SubItems.Add(dateTm + " " + message);
                        this.myListView[camName].Items.Add(item);
                        int index = this.myListView[camName].Items.Count;
                        this.myListView[camName].EnsureVisible(index - 1);
                        if (this.myListView[camName].Items.Count > 2000)
                            this.myListView[camName].Items.RemoveAt(0);
                    }));
                }
            }
        }
        public void Error(object message, Exception exception, string fileName = null, string camName = "NONE")
        {
            if (this.IsErrorEnabled)
            {
                CreateTextListener(fileName);
                mySource.TraceData(TraceEventType.Error, (int)TraceEventType.Error, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff  ") + message, exception); //+ " [Error] "
                //mySource.TraceInformation("{0}{1}{2}{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"), " [Error] ", message, exception);
                mySource.Flush();
            }
            if (IsListView)
            {
                if (this.myListView == null || this.myListView.Count == 0 || camName == null) return;
                if (this.myListView.ContainsKey(camName))
                {
                    if (this.myListView[camName] == null) return;
                    this.myListView[camName].BeginInvoke(new Action(() =>
                    {
                        string dateTm = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"));
                        ListViewItem item = new ListViewItem();
                        item.ForeColor = System.Drawing.Color.Red;
                        item.Text = " [Error] ";
                        item.SubItems.Add(dateTm + " " + message + exception.ToString());
                        this.myListView[camName].Items.Add(item);
                        int index = this.myListView[camName].Items.Count;
                        this.myListView[camName].EnsureVisible(index - 1);
                        if (this.myListView[camName].Items.Count > 2000)
                            this.myListView[camName].Items.RemoveAt(0);
                    }));
                }
            }
        }
        public void Fatal(object message, string fileName = null, string camName = "NONE")
        {
            if (this.IsFatalEnabled)
            {
                CreateTextListener(fileName);
                mySource.TraceData(TraceEventType.Critical, (int)TraceEventType.Critical, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff  ") + message); // + " [Fatal] "
                //mySource.TraceInformation("{0}{1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"), " [Fatal] " , message);
                mySource.Flush();
            }
            if (IsListView)
            {
                if (this.myListView == null || this.myListView.Count == 0 || camName == null) return;
                if (this.myListView.ContainsKey(camName))
                {
                    if (this.myListView[camName] == null) return;
                    this.myListView[camName].BeginInvoke(new Action(() =>
                    {
                        string dateTm = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"));
                        ListViewItem item = new ListViewItem();
                        item.ForeColor = System.Drawing.Color.Red;
                        item.Text = " [Fatal] ";
                        item.SubItems.Add(dateTm + " " + message);
                        this.myListView[camName].Items.Add(item);
                        int index = this.myListView[camName].Items.Count;
                        this.myListView[camName].EnsureVisible(index - 1);
                        if (this.myListView[camName].Items.Count > 2000)
                            this.myListView[camName].Items.RemoveAt(0);
                    }));
                }
            }
        }
        public void Fatal(object message, Exception exception, string fileName = null, string camName = "NONE")
        {
            if (this.IsFatalEnabled)
            {
                CreateTextListener(fileName);
                mySource.TraceData(TraceEventType.Critical, (int)TraceEventType.Critical, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff  ") + message, exception); //+ " [Fatal] "
                //mySource.TraceInformation("{0}{1}{2}{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"), " [Fatal] ", message, exception);
                mySource.Flush();
            }
            if (IsListView)
            {
                if (this.myListView == null || this.myListView.Count == 0 || camName == null) return;
                if (this.myListView.ContainsKey(camName))
                {
                    if (this.myListView[camName] == null) return;
                    this.myListView[camName].BeginInvoke(new Action(() =>
                    {
                        string dateTm = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"));
                        ListViewItem item = new ListViewItem();
                        item.ForeColor = System.Drawing.Color.Red;
                        item.Text = " [Fatal] ";
                        item.SubItems.Add(dateTm + " " + message + exception.ToString());
                        this.myListView[camName].Items.Add(item);
                        int index = this.myListView[camName].Items.Count;
                        this.myListView[camName].EnsureVisible(index - 1);
                        if (this.myListView[camName].Items.Count > 2000)
                            this.myListView[camName].Items.RemoveAt(0);
                    }));
                }
            }
        }



    }
}
