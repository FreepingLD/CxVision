using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.Data;
using Common;
using System.ComponentModel;
using AlgorithmsLibrary;
using System.Drawing;

namespace FunctionBlock
{
    [Serializable]
    public class ExtractPoint : BaseFunction, IFunction
    {
        private userWcsPoint  wcsPoint;
        private userWcsPoint[] extractRefSource1Data()
        {
            List<userWcsPoint> listPoint = new List<userWcsPoint>();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource1.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource1[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource1[item].GetPropertyValues(item.Split('.')[1]);
                if (object3D != null) // 这样做是为了动态获取名称                   
                {
                    switch (object3D.GetType().Name)
                    {
                        case "userWcsPoint":
                            listPoint.Add(((userWcsPoint)object3D));
                            break;
                        case "userWcsPoint[]":
                            listPoint.AddRange(((userWcsPoint[])object3D));
                            break;                           
                        case "userWcsCircle":
                            listPoint.Add(new userWcsPoint(((userWcsCircle)object3D).X, ((userWcsCircle)object3D).Y, ((userWcsCircle)object3D).Z, ((userWcsCircle)object3D).Grab_x, ((userWcsCircle)object3D).Grab_y,((userWcsCircle)object3D).CamParams));
                            break;
                        case "userWcsCircleSector":
                            listPoint.Add(new userWcsPoint(((userWcsCircleSector)object3D).X, ((userWcsCircleSector)object3D).Y, ((userWcsCircleSector)object3D).Z, ((userWcsCircle)object3D).Grab_x, ((userWcsCircle)object3D).Grab_y, ((userWcsCircleSector)object3D).CamParams));
                            break;
                    }
                }
            }
            if (listPoint.Count == 0)
                listPoint.Add(new userWcsPoint());
            return listPoint.ToArray();
        }

        public ExtractPoint()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }


        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this.wcsPoint = extractRefSource1Data()[0];
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this.wcsPoint.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this.wcsPoint.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this.wcsPoint.Z);
                this.OnExcuteCompleted(this.name, this.wcsPoint);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                this.Result.Succss = false;
            }
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                    return this.name;
                case "点对象":
                    return this.wcsPoint; //
                default:
                    if (this.name == propertyName)
                        return this.wcsPoint;
                    else return null;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                default:
                    return true;
            }
        }
        public void ReleaseHandle()
        {
            // throw new NotImplementedException();
        }
        public void Read(string path)
        {
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }

        #endregion



    }
}
