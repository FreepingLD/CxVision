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
    [DefaultProperty(nameof(PointParam))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class PointMove : BaseFunction, IFunction
    {
        [Description("输出属性")]
        public PointMoveParam PointParam { get; set; }
        public PointMove()
        {
            this.PointParam = new PointMoveParam();
        }

        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                MoveCommandParam CommandParam = new MoveCommandParam();
                IMotionControl _card;
                for (int i = 0; i < this.PointParam.TrackParam.Count; i++) //
                {
                    _card = MotionCardManage.GetCard(this.PointParam.CoordSysName);
                    CommandParam.MoveAxis = this.PointParam.TrackParam[i].MoveAxis;
                    CommandParam.MoveSpeed = this.PointParam.MaxVel;
                    CommandParam.StartVel = this.PointParam.StartVel;
                    CommandParam.StopVel = this.PointParam.StopVel;
                    CommandParam.Tacc = this.PointParam.Tacc;
                    CommandParam.Tdec = this.PointParam.Tdec;
                    CommandParam.S_para = this.PointParam.S_para;
                    CommandParam.IsWait = this.PointParam.TrackParam[i].IsWait;
                    switch (CommandParam.MoveAxis)
                    {
                        case enAxisName.X轴:
                        case enAxisName.XYZTheta轴:
                            CommandParam.AxisParam = new CoordSysAxisParam(this.PointParam.TrackParam[i].X, this.PointParam.TrackParam[i].Y, 
                                                                           this.PointParam.TrackParam[i].Z, this.PointParam.TrackParam[i].Theta,0,0);
                           _card.MoveMultyAxis(CommandParam);
                            break;
                        default:
                            throw new Exception("点位运动不支持该模式");
                    }
                }
                // 使用发命令的方式来更新视图  
                Result.Succss = true;
                if (Result.Succss)
                    LoggerHelper.Info(this.name + "运动完成");
                else
                    LoggerHelper.Error(this.name + "运动失败");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "运动报错" + e);
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
