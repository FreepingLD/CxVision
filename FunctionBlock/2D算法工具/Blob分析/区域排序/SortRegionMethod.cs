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
    public class SortRegionMethod
    {
        private static object lockState = new object();
        private static SortRegionMethod _Instance = null;
        private SortRegionMethod()
        {

        }
        public static SortRegionMethod Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        if (_Instance == null)
                            _Instance = new SortRegionMethod();
                    }
                }
                return _Instance;
            }
        }

        public HRegion SortRegion(HRegion hRegion, SortParam sortParam)
        {
            HRegion sortRegion = new HRegion();
            //sortRegion.GenEmptyObj(); // 这个指令不会创建一个面积为0的空对象
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            if (hRegion.IsInitialized())
            {
                switch(sortParam.SortMode)
                {
                    case nameof(enSortMode.none):
                        sortRegion = hRegion;
                        break;
                    default:
                        sortRegion = hRegion.SortRegion(sortParam.SortMode, sortParam.Order, sortParam.RowOrCol);
                        break;
                }
                sortRegion = hRegion.SortRegion(sortParam.SortMode, sortParam.Order, sortParam.RowOrCol);
            }
            return sortRegion;
        }
         

    }


}
