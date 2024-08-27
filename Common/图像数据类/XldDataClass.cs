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
    /// 
    /// </summary>
    public class XldDataClass
    {
        private double grab_x = 0; // 当前采集图像的机台坐标
        private double grab_y = 0;
        private double grab_z = 0;
        private CameraParam _CamParams;
        private object _Tag = 1;
        [NonSerialized]
        private HXLDCont hXldCont;  
        public HXLDCont HXldCont
        {
            get
            {
                return hXldCont;
            }
            set
            {
                hXldCont = value;
            }
        }
        public CameraParam CamParams 
        { 
            get => _CamParams; 
            set => _CamParams = value;
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

        public string CamName { get; set; }
        public string ViewWindow { get; set; }

        public enColor Color { get; set; }
        public object Tag { get => _Tag; set => _Tag = value; }

        public XldDataClass()
        {
            this.hXldCont = new HXLDCont();
        }
        public XldDataClass(HXLDCont XldCont)
        {
            this.hXldCont = XldCont;
        }
        public XldDataClass(HXLDCont[] XldCont)
        {
            if (XldCont != null)
            {
                if (this.hXldCont == null)
                    this.hXldCont = new HXLDCont();
                else
                    this.hXldCont.Dispose();
                this.hXldCont.GenEmptyObj();
                foreach (var item in XldCont)
                {
                    this.hXldCont = this.hXldCont.ConcatObj(item);
                }
            }
        }

        public XldDataClass(HXLDCont[] XldCont, CameraParam cameraParam)
        {
            if (XldCont != null)
            {
                if (this.hXldCont == null)
                    this.hXldCont = new HXLDCont();
                else
                    this.hXldCont.Dispose();
                this.hXldCont.GenEmptyObj();
                foreach (var item in XldCont)
                {
                    this.hXldCont = this.hXldCont.ConcatObj(item);
                }
            }
            this.CamParams = cameraParam;
        }
        public XldDataClass(HXLDCont XldCont, CameraParam cameraParam)
        {
            this.hXldCont = XldCont;
            this.CamParams = cameraParam;
        }
        public XldDataClass Clone()
        {
            if (this == null) return null;
            XldDataClass xldData = new XldDataClass();
            xldData.CamParams = this._CamParams;
            if (this.hXldCont != null)
            {
                xldData.hXldCont.GenEmptyObj();
                int num = this.hXldCont.CountObj();
                for (int i = 1; i < num + 1; i++)
                {
                    xldData.hXldCont = xldData.hXldCont.ConcatObj(this.hXldCont.SelectObj(i));
                }
            }
            return xldData;
        }
        public void AddXLDCont(HXLDCont xLDCont)
        {
            if (this.hXldCont == null)
            {
                this.hXldCont = new HXLDCont();
            }
            if (!this.hXldCont.IsInitialized())
            {
                this.hXldCont.GenEmptyObj();
            }
            this.hXldCont = this.hXldCont.ConcatObj(xLDCont);
        }
        public void Clear()
        {
            if (this.hXldCont != null && this.hXldCont.IsInitialized())
            {
                int num = this.hXldCont.CountObj();
                for (int i = 1; i < num + 1; i++)
                {
                    this.hXldCont.RemoveObj(i);
                }
            }
        }
        public void RemoveAt(int index)
        {
            if (this.hXldCont != null && this.hXldCont.IsInitialized())
            {
                int num = this.hXldCont.CountObj();
                if (index <= num)
                    this.hXldCont.RemoveObj(index);
            }
        }
        public void Dispose()
        {
            if (this.hXldCont != null && this.hXldCont.IsInitialized())
            {
                this.hXldCont.Dispose();
                this.hXldCont = null;
            }
        }

        public bool IsInitialized()
        {
            if (this.hXldCont != null && this.hXldCont.IsInitialized())
            {
                return true;
            }
            else
                return false;
        }


    }


}
