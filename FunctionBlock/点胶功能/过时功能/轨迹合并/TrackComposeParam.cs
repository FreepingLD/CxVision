using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Data;
using Light;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    public class TrackComposeParam
    {
        public enComposeMethod ComposeMethod { get; set; }
        public TrackComposeParam()
        {
            this.ComposeMethod = enComposeMethod.合并Z坐标;
        }
    }
    public enum enComposeMethod
    {
        合并XY坐标,
        合并Z坐标,
        合并XYZ坐标,
    }

}
