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
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class PointMoveParam : MoveParam
    {
        public BindingList<PointTrackParam> TrackParam { get; set; }

        public PointMoveParam()
        {
            this.TrackParam = new BindingList<PointTrackParam>();
        }

    }


}
