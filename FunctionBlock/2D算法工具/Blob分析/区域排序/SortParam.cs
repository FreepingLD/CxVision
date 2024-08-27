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

namespace FunctionBlock
{
    [Serializable]
    public class SortParam
    {
        public string SortMode { get; set; }
        public string Order { get; set; }
        public string RowOrCol { get; set; }

        public SortParam()
        {
            SortMode = "first_point";
            Order = "true";
            RowOrCol = "column";
        }

    }
    [Serializable]
    public enum enSortMode
    {
        none,
        character,
        first_point,
        last_point,
        lower_left,
        lower_right,
        upper_left,
        upper_right,
    }

}
