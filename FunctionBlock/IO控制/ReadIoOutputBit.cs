﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using Common;
using System.IO;
using Sensor;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class ReadIoOutputBit : BaseFunction, IFunction
    {

        private IMotionControl _card;
        private ushort ioOutputPort = 0;
        private ushort ioOutputLevel = 1;
        private enIoPortType ioPortType = enIoPortType.通用Io端口;
        private DataTable levelDataTable = new DataTable();
        public ushort IoOutputPort { get => ioOutputPort; set => ioOutputPort = value; }
        public ushort IoOutputLevel { get => ioOutputLevel; set => ioOutputLevel = value; }
        public enIoPortType IoPortType { get => ioPortType; set => ioPortType = value; }
        public DataTable LevelDataTable { get => levelDataTable; set => levelDataTable = value; }

        public ReadIoOutputBit(IMotionControl _card)
        {
            if (_card != null)
                this._card = MotionCardManage.GetCard(_card.Name);
            levelDataTable.Columns.AddRange(new DataColumn[] { new DataColumn("端口号"), new DataColumn("电平") });
        }




        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            object [] value ;
            try
            {
                if (this._card == null) throw new ArgumentNullException();
                this.levelDataTable.Rows.Clear();
                this._card.ReadIoOutputBit(this.ioPortType,this.ioOutputPort, out value);
                this.IoOutputLevel =Convert.ToUInt16(value[0]);
                this.levelDataTable.Rows.Add(this.ioOutputPort,this.IoOutputLevel);
                // 使用发命令的方式来更新视图  
                Result.Succss = true;
                if (Result.Succss)
                    LoggerHelper.Info(this.name + "读取Io输出位完成");
                else
                    LoggerHelper.Error(this.name + "读取Io输出位失败");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "读取Io输出位报错" + e);
                return Result;
            }
            return Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                default:
                    return this.name;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            string name = "";
            if (value != null)
                name = value[0].ToString();
            switch (propertyName)
            {
                default:
                case "名称":
                    this.name = value[0].ToString();
                    return true;
            }
        }

        public void ReleaseHandle()
        {

        }
        public void Read(string path)
        {
           // throw new NotImplementedException();
        }
        public void Save(string path)
        {
           // throw new NotImplementedException();
        }

        #endregion



    }

}
