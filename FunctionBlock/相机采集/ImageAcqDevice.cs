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

namespace FunctionBlock
{
    /// <summary>
    /// 配置成单实例类
    /// </summary>
    [Serializable]
    public class ImageAcqDevice
    {
        private static string ParaPath = @"ConfigParam";
        private static object sycnObj = new object();
        private static ImageAcqDevice _Instance;
        public static ImageAcqDevice Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        //Read();
                        _Instance = new ImageAcqDevice();
                    }
                }
                return _Instance;
            }
        }
        public bool IsLaserSource { get; set; }
        public bool IsCamSource{ get; set; }
        public bool IsFileSource{ get; set; }

        public bool IsDirectorySource { get; set; }
        private ImageAcqDevice()
        {
            this.IsLaserSource = true;
            this.IsCamSource = true;
            this.IsFileSource = false;
            this.IsDirectorySource = false;
        }

        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<ImageAcqDevice>.Save(this, ParaPath + "\\" + "ImageAcqDevice.xml"); // 以类名作为文件名
            return IsOk;
        }

        private static void Read()
        {
            try
            {
                if (File.Exists(ParaPath + "\\" + "ImageAcqDevice.xml"))
                    _Instance = XML<ImageAcqDevice>.Read(ParaPath + "\\" + "ImageAcqDevice.xml");
                else
                    _Instance = new ImageAcqDevice();
            }
            catch
            {
                _Instance = new ImageAcqDevice();
            }
        }





    }
}
