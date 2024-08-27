using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Sensor;
using Common;
using System.IO;
using System.Windows.Forms;

namespace FunctionBlock
{
    public class AcqSourceManage
    {
        private static string ParaPath = @"ConfigParam";
        private static object sycnObj = new object();
        private static AcqSourceManage _Instance;
        private BindingList<AcqSource> acqSourceList = new BindingList<AcqSource>();
        private AcqSource currentAcqSource;
        public BindingList<AcqSource> AcqSourceList { get => acqSourceList; set => acqSourceList = value; }
        public AcqSource CurrentAcqSource { get => currentAcqSource; set => currentAcqSource = value; }



        public static AcqSourceManage Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new AcqSourceManage();
                    }
                }
                return _Instance;
            }
        }

        public AcqSource GetAcqSource(string acqSourceName)
        {
            foreach (var item in acqSourceList)
            {
                if (item.Name == acqSourceName || item.Sensor?.Name == acqSourceName)
                    return item;
            }
            return null;
        }

        public AcqSource GetCamAcqSource(string acqSourceName)
        {
            if(acqSourceName != null)
            {
                foreach (var item in acqSourceList)
                {
                    if (item.Name == acqSourceName || item.Sensor?.Name == acqSourceName)
                        return item;
                }
            }
            return null;
        }
        //public AcqSource GetCamAcqSource(string camName)
        //{
        //    foreach (var item in acqSourceList)
        //    {
        //        if (item.Name == acqSourceName)
        //            return item;
        //    }
        //    return null;
        //}
        public AcqSource GetLaserAcqSource(string laserName)
        {
            foreach (var item in acqSourceList)
            {
                if (item.Sensor.Name == laserName)
                    return item;
            }
            return null;
        }

        public BindingList<AcqSource> GetCamAcqSourceList()
        {
            BindingList<AcqSource> listAcq = new BindingList<AcqSource>();
            foreach (var item in acqSourceList)
            {
                if (item.Sensor != null)
                {
                    switch (item.Sensor.ConfigParam.SensorType)
                    {
                        case enUserSensorType.面阵相机:
                        case enUserSensorType.线阵相机:
                            listAcq.Add(item);
                            break;
                    }
                }
            }
            return listAcq;
        }

        public string[] GetAcqSourceName()
        {
            List<string> listName = new List<string>();
            listName.Add("NONE");
            if (acqSourceList != null)
            {
                foreach (var item in acqSourceList)
                {
                    listName.Add(item.Name);
                }
            }
            return listName.ToArray();
        }

        public BindingList<AcqSource> LaserAcqSourceList()
        {
            BindingList<AcqSource> listAcq = new BindingList<AcqSource>();
            foreach (var item in acqSourceList)
            {
                switch (item.Sensor.ConfigParam.SensorType)
                {
                    case enUserSensorType.点激光:
                    case enUserSensorType.线激光:
                    case enUserSensorType.面激光:
                        listAcq.Add(item);
                        break;

                }
            }
            return listAcq;
        }

        public bool ContainsName(string name)
        {
            foreach (var item in this.acqSourceList)
            {
                if (item.Name == name) return true;
            }
            return false;
        }


        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<AcqSource>>.Save(acqSourceList, ParaPath + "\\" + "AcqSourceConfig.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            try
            {
                if (File.Exists(ParaPath + "\\" + "AcqSourceConfig.xml"))
                    this.acqSourceList = XML<BindingList<AcqSource>>.Read(ParaPath + "\\" + "AcqSourceConfig.xml");
                else
                    this.acqSourceList = new BindingList<AcqSource>();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("读取采集配置源文件失败！" + ex);
            }
            if (this.acqSourceList == null)
                this.acqSourceList = new BindingList<AcqSource>();
        }



    }
}
