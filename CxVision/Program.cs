
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sensor;
using FunctionBlock;
using View;
using System.Threading;
using Common;
using System.IO;
using System.Drawing;


namespace CxVision
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!HslCommunication.Authorization.SetAuthorizationCode("59c4eb88-2135-42b1-a99c-6412884d946c"))
            {
                Console.WriteLine("active failed");
                LoggerHelper.Error("HslCommunication 通信库激活失败! active failed");
            }
            BindExceptionHandler();
            //////////////
            ConsoleHelper.AllocConsole();
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(SystemParamManager.Instance.SysConfigParam.Language);
            //log4net.Config.XmlConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        private static void BindExceptionHandler()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadException);
            // 处理未捕获的异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
        }
        private static void ThreadException(object sender, ThreadExceptionEventArgs e)  //
        {
            try
            {
                LoggerHelper.Fatal(e);
            }
            catch
            {

            }
        }
        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)  
        {
            try
            {
                LoggerHelper.Fatal(e);
            }
            catch
            {

            }
        }


    }
}
