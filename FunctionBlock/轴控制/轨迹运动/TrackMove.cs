using System;
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
    [DefaultProperty(nameof(MoveParam))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class TrackMove : BaseFunction, IFunction
    {
        public BindingList<TrackParam> TrackList { get; set; }

        [DisplayName("轨迹点")]
        [Description("输出属性")]
        public TrackParam [] MoveParam { get; set; }
        public TrackMove()
        {
            this.TrackList = new BindingList<TrackParam>();
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                MoveCommandParam CommandParam = new MoveCommandParam();
                IMotionControl _card;
                this.MoveParam = new TrackParam[this.TrackList.Count];
                for (int i = 0; i < this.TrackList.Count; i++) //
                {
                    this.MoveParam[i] = this.TrackList[i];
                    _card = MotionCardManage.GetCard(this.TrackList[i].CoordSysName);
                    CommandParam.MoveAxis = this.TrackList[i].MoveAxis;
                    //CommandParam.MoveSpeed = this.TrackList.MaxVel;
                    //CommandParam.StartVel = this.TrackList.StartVel;
                    //CommandParam.StopVel = this.TrackList.StopVel;
                    //CommandParam.Tacc = this.TrackList.Tacc;
                    //CommandParam.Tdec = this.TrackList.Tdec;
                    //CommandParam.S_para = this.TrackList.S_para;
                    CommandParam.IsWait = this.TrackList[i].IsWait;
                    //_card.MoveMultyAxis(CommandParam);
                }
                // 使用发命令的方式来更新视图  
                Result.Succss = true;
                if (Result.Succss)
                    LoggerHelper.Info(this.name + "轨迹配置完成");
                else
                    LoggerHelper.Error(this.name + "轨迹配置失败");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "轨迹配置报错" + e);
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
            return true;
        }

        public void ReleaseHandle()
        {

        }
        public void Read(string path)
        {

        }
        public void Save(string path)
        {

        }

        #endregion


    }
}
