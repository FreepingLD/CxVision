using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Runtime.InteropServices;


namespace Common
{
    [Serializable]
    /// <summary>
    /// 封装图像数据及图像对应相机的内外参
    /// </summary>
    public class RegionDataClass
    {
        private double grab_x = 100; // 当前采集图像的机台坐标
        private double grab_y = 100;
        private double grab_z;
        private double grab_theta;
        private HRegion region;
        private CameraParam camParams;
        private string _draw = "fill";
        private object _Tag = 1;
        public HRegion Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
            }
        }
        public double Grab_X
        {
            get
            {
                return grab_x;
            }

            set
            {
                grab_x = value;
            }
        }
        public double Grab_Y
        {
            get
            {
                return grab_y;
            }

            set
            {
                grab_y = value;
            }
        }
        public double Grab_Z
        {
            get
            {
                return grab_z;
            }

            set
            {
                grab_z = value;
            }
        }
        public double Grab_Theta
        {
            get
            {
                return grab_theta;
            }
            set
            {
                grab_theta = value;
            }
        }
        public string CamName
        {
            get;
            set;
        }
        public string ViewWindow { set; get; }
        public CameraParam CamParams { get => camParams; set => camParams = value; }
        public string Draw { get => _draw; set => _draw = value; }
        public object Tag { get => this._Tag; set => _Tag = value; }
        public enColor Color { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


        public RegionDataClass()
        {
            this.region = new HRegion();
        }
        public RegionDataClass(HRegion region, double grab_x, double grab_y, double grab_z, CameraParam cameraParam)
        {
            this.region = region;
            this.grab_x = grab_x;
            this.grab_y = grab_y;
            this.grab_z = grab_z;
            this.camParams = cameraParam;
            this.CamName = cameraParam.SensorName;
        }
        public RegionDataClass(HRegion region)
        {
            this.region = region.Clone();
        }
        public RegionDataClass(HRegion region, CameraParam cameraParam)
        {
            this.region = region;
            this.camParams = cameraParam;
        }

        public RegionDataClass Clone()
        {
            if (this == null) return null;
            RegionDataClass regionData = new RegionDataClass();
            regionData.CamParams = this.camParams;
            regionData.Draw = this.Draw;
            regionData.Tag = this.Tag;
            regionData.ViewWindow = this.ViewWindow;
            regionData.Grab_X = this.Grab_X;
            regionData.Grab_Y = this.Grab_Y;
            regionData.Grab_Z = this.Grab_Z;
            regionData.CamName = this.CamName;
            ////////////////////////////////////////////////
            if (this.region != null)
            {
                regionData.Region.GenEmptyObj();
                int num = this.Region.CountObj();
                for (int i = 1; i < num + 1; i++)
                {
                    regionData.Region = regionData.Region.ConcatObj(this.Region.SelectObj(i));
                }
            }
            return regionData;
        }
        public void AddRegion(HRegion hRegion)
        {
            if (this.Region == null)
            {
                this.Region = new HRegion();
            }
            if (!this.Region.IsInitialized())
            {
                this.Region.GenEmptyObj();
            }
            this.Region = this.Region.ConcatObj(hRegion);
        }
        public void Clear()
        {
            this.Region?.Dispose();
        }
        public void RemoveAt(int index)
        {
            if (this.Region != null && this.Region.IsInitialized())
            {
                int num = this.Region.CountObj();
                if (index <= num)
                    this.Region = this.Region.RemoveObj(index);
            }
        }
        public void Dispose()
        {
            if (this.region != null && this.region.IsInitialized())
            {
                this.region.Dispose();
                this.region = null;
            }
        }
        public bool IsInitialized()
        {
            if (this.region != null && this.region.IsInitialized())
            {
                return true;
            }
            else
                return false;
        }




    }


}
