using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionControlCard
{
    public class CoordSysConfigParamManger
    {
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static CoordSysConfigParamManger _Instance;
        public static CoordSysConfigParamManger Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new CoordSysConfigParamManger();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<CoordSysConfigParam> _coordSysConfigParamList;
        public BindingList<CoordSysConfigParam> CoordSysConfigParamList { get => _coordSysConfigParamList; set => _coordSysConfigParamList = value; }

        public string GetCardName(string coordSysName)
        {
            if (_coordSysConfigParamList != null)
            {
                foreach (var item in _coordSysConfigParamList)
                {
                    if (item.CoordSysName.ToString() == coordSysName) // 一个坐标系一般只对应一个卡
                        return item.CardName;
                }
            }
            return "";
        }

        public CoordSysConfigParam[] GetCoordSysConfigParam(string coordSysName)
        {
            List<CoordSysConfigParam> CoordList = new List<CoordSysConfigParam>();
            foreach (var item in _coordSysConfigParamList)
            {
                if (item.CoordSysName.ToString() == coordSysName)
                    CoordList.Add(item);
            }
            return CoordList.ToArray();
        }
        public CoordSysConfigParam GetCoordSysConfigParam(enCoordSysName coordSysName, enAxisName axisName)
        {
            if (_coordSysConfigParamList != null)
            {
                foreach (var item in _coordSysConfigParamList)
                {
                    if (item.CoordSysName == coordSysName && item.AxisName == axisName)
                        return item;
                }
            }
            return null;
        }
        public CoordSysConfigParam GetCoordSysConfigParam(string axisLable, enAxisName axisName)
        {
            if (_coordSysConfigParamList != null)
            {
                foreach (var item in _coordSysConfigParamList)
                {
                    if (item.AxisLable == axisLable && item.AxisName == axisName)
                        return item;
                }
            }
            return null;
        }
        public CoordSysConfigParam GetCoordSysConfigParam(enCoordSysName coordSysName)
        {
            if (_coordSysConfigParamList != null)
            {
                foreach (var item in _coordSysConfigParamList)
                {
                    if (item.CoordSysName == coordSysName)
                        return item;
                }
            }
            return null;
        }

        public CoordSysConfigParam GetCoordSysConfigParam(enCoordSysName coordSysName, enAxisName axisName, enAxisReadWriteState axisReadWriteState)
        {
            if (_coordSysConfigParamList != null)
            {
                foreach (var item in _coordSysConfigParamList)
                {
                    if (item.CoordSysName == coordSysName && item.AxisName == axisName && item.AxisReadWriteState == axisReadWriteState)
                        return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取指定坐标系指定轴的地址
        /// </summary>
        /// <param name="coordSysName"></param>
        /// <param name="axisName"></param>
        /// <returns></returns>
        public int GetAxisAdress(enCoordSysName coordSysName, enAxisName axisName)
        {
            foreach (var item in _coordSysConfigParamList)
            {
                if (item.CoordSysName == coordSysName && item.AxisName == axisName)
                {
                    return Convert.ToInt32(item.AxisAddress);
                }
            }
            return -1;
        }
        public CoordSysConfigParam GetCoordSysParam(enCoordSysName coordSysName, enAxisName axisName)
        {
            foreach (var item in _coordSysConfigParamList)
            {
                if (item.CoordSysName == coordSysName && item.AxisName == axisName)
                    return item;
            }
            return null;
        }
        public CoordSysConfigParam GetCoordSysParam(enCoordSysName coordSysName)
        {
            foreach (var item in _coordSysConfigParamList)
            {
                if (item.CoordSysName == coordSysName)
                    return item;
            }
            return null;
        }

        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<CoordSysConfigParam>>.Save(_coordSysConfigParamList, ParaPath + "\\" + "CoordSysConfigParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {

            if (File.Exists(ParaPath + "\\" + "CoordSysConfigParam.xml"))
                this._coordSysConfigParamList = XML<BindingList<CoordSysConfigParam>>.Read(ParaPath + "\\" + "CoordSysConfigParam.xml");
            if (this._coordSysConfigParamList == null)
            {
                this._coordSysConfigParamList = new BindingList<CoordSysConfigParam>();
                // 如果为空，则自动创建
                this._coordSysConfigParamList.Add(new CoordSysConfigParam(enAxisName.X轴, 0));
                this._coordSysConfigParamList.Add(new CoordSysConfigParam(enAxisName.Y轴, 1));
                this._coordSysConfigParamList.Add(new CoordSysConfigParam(enAxisName.Z轴, 2));
                this._coordSysConfigParamList.Add(new CoordSysConfigParam(enAxisName.Theta轴, 3));
                this._coordSysConfigParamList.Add(new CoordSysConfigParam(enAxisName.U轴, 4));
                this._coordSysConfigParamList.Add(new CoordSysConfigParam(enAxisName.V轴, 5));
            }

        }




    }
}
