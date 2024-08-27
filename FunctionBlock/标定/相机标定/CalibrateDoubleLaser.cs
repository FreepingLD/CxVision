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
    public class CalibrateDoubleLaser:INotifyPropertyChanged
    {
        private double coordDist;
        private double laser1Value;
        private double laser2Value;
        private AcqSource _laserAcqSource1;
        private AcqSource _laserAcqSource2;
        public AcqSource LaserAcqSource1
        {
            get
            {
                return _laserAcqSource1;
            }

            set
            {
                _laserAcqSource1 = value;
            }
        }
        public AcqSource LaserAcqSource2
        {
            get
            {
                return _laserAcqSource2;
            }

            set
            {
                _laserAcqSource2 = value;
            }
        }
        public double CoordDist
        {
            get
            {
                return coordDist;
            }

            set
            {
                coordDist = value;
            }
        }


        public double Laser1Value
        {
            get
            {
                return laser1Value;
            }

            set
            {
                laser1Value = value;
                if (PropertyChanged != null)
                    PropertyChanged(this,new PropertyChangedEventArgs("Laser1Value"));
            }
        }
        public double Laser2Value
        {
            get
            {
                return laser2Value;
            }

            set
            {
                laser2Value = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Laser2Value"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CalibrateDoubleLaser(AcqSource [] acqSources)
        {
            if (acqSources == null) return;
            switch(acqSources.Length)
            {
                case 0:
                    _laserAcqSource1 = null;
                    _laserAcqSource2 = null;
                    break;
                case 1:
                    _laserAcqSource1 = acqSources[0];
                    _laserAcqSource2 = null;
                    break;
                default:
                case 2:
                    _laserAcqSource1 = acqSources[0];
                    _laserAcqSource2 = acqSources[1];
                    break;
            }
        }


        public void Calibrate()
        {
            Dictionary<enDataItem, object> list1;
            Dictionary<enDataItem, object> list2;
            TransformLaserPointData pointData1 = new TransformLaserPointData();
            TransformLaserPointData pointData2 = new TransformLaserPointData();
            try
            {
                pointData1.Clear();
                pointData2.Clear();
                this._laserAcqSource1.StartTrigger();
                this._laserAcqSource2.StartTrigger();
                if (this._laserAcqSource1 != null)
                    this._laserAcqSource1.SetIoOutput( true);
                Thread.Sleep(_laserAcqSource1.Sensor.LaserParam.WaiteTime);
                if (this._laserAcqSource1 != null)
                    this._laserAcqSource1.SetIoOutput( false);
                this._laserAcqSource1.StopTrigger();
                this._laserAcqSource2.StopTrigger();
                list1 = this._laserAcqSource1.getData();
                list2 = this._laserAcqSource2.getData();
                pointData1.TransformData(list1,1,new double[3] { 0, 0, 0 });
                pointData2.TransformData(list2, 1, new double[3] { 0, 0, 0 });
                /////////////////////////////////////////////
                //更新激光1数据
                this.Laser1Value = pointData1.Dist1Value[0];
                this.Laser2Value = pointData2.Dist1Value[0];
                this.CoordDist= GlobalVariable.pConfig.StandardThickValue- (this.laser1Value + this.laser2Value);
                GlobalVariable.pConfig.Cord_Gap = this.CoordDist;
            }
            catch (Exception e)
            {
                MessageBox.Show("标定失败" + e.ToString());
            }
        }



    }

}
