using Common;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{

    /// <summary>
    /// 运动命令类,发送命令来控制机台运动 
    /// </summary>
    public class TriggerControlCommand : CommandBase
    {
        private enUserTriggerSource triggerSource = enUserTriggerSource.NONE;
        private int triggerPort = 0;
        private double lineTrigGap = 1;
        private bool eableTrigger = false;
        private enIoOutputMode _IoOutPutType = enIoOutputMode.NONE;
        private IMotionControl _card; //
        private ISensor _sensor;
        public enUserTriggerSource TriggerSource { get => triggerSource; set => triggerSource = value; }
        public int TriggerPort { get => triggerPort; set => triggerPort = value; }
        public double LineTrigGap { get => lineTrigGap; set => lineTrigGap = value; }
        public bool EableTrigger { get => eableTrigger; set => eableTrigger = value; }
        public enIoOutputMode IoOutPutType { get => _IoOutPutType; set => _IoOutPutType = value; }

        public TriggerControlCommand(IMotionControl card, ISensor sensor)
        {
            this._card = card;
            this._sensor = sensor;
        }

        /// <summary>
        /// 因为传感器采集可以是实时采集、软触发、或内部Io触发、外部Io触发，所以需要持有：运动控制卡对象,该命令需要两个执行者
        /// </summary>
        /// <param name="card"></param>
        /// <param name="sensor"></param>
        /// <param name="triggerSource"></param>
        /// <param name="IoType"></param>
        /// <param name="IoPort"></param>
        /// <param name="state"></param>
        /// <param name="lineTrigGap"></param>
        public TriggerControlCommand(IMotionControl card, ISensor sensor, enUserTriggerSource triggerSource, enIoOutputMode IoOutPutType, int IoPort, bool eableTrigger, double lineTrigGap)
        {
            this._IoOutPutType = IoOutPutType;
            this._card = card;
            this._sensor = sensor;
            this.triggerPort = IoPort;
            this.eableTrigger = eableTrigger;
            this.triggerSource = triggerSource;
            this.lineTrigGap = lineTrigGap;
        }

        public override void execute()
        {
            switch (triggerSource)
            {
                case enUserTriggerSource.软触发:
                    if (this._sensor != null)
                        this._sensor.StartTrigger();
                    break;
                case enUserTriggerSource.外部IO触发:
                    if (this._sensor != null)
                        this._sensor.StartTrigger();
                    break;
                case enUserTriggerSource.内部IO触发:
                    if (this._sensor != null)
                        this._sensor.StartTrigger();
                    if (this._card!= null)
                        this._card.SetIoOutputBit(enIoOutputMode.脉冲输出, this.triggerPort, this.eableTrigger); // 使用true跟fasle来
                    break;
                case enUserTriggerSource.NONE:
                    if (this._sensor != null) // 触发需要时间
                        this._sensor.StartTrigger();
                    break;
                //case enUserTriggerSource.线性比较触发:
                //    if (this._sensor != null) // 触发需要时间
                //        this._sensor.StartTrigger(triggerSource);
                //    if (this._card != null)
                //    {
                //        this._card.SetParam(enParamType.触发间隔, this.lineTrigGap); // 放在Io前即可
                //        this._card.SetIoOutput(enIoOutputType.线性比较触发, this.triggerPort, this.eableTrigger);
                //    }
                //    break;
                default:
                    break;
            }
        }




    }




}
