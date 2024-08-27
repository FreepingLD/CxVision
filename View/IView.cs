using Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace View
{
    public interface IView
    {
        #region halcon接口  // object是所有类的基类

        void MoveObject(object Object, object Tx, object Ty);
        void DisplayObject(object Object);      
        void DisplayObject(params object [] Object);
        void DisplayObject(object  Object1, object Object2);
        void ZoomObject(object Object, object CenterX, object CenterY, double scale);
        void View3D(object Object);
        void SetViewParam(enViewParamType paramType, object paramValue);
        object GetViewParam(enViewParamType paramType);
        void InitView(object view);
        void ClearView();

        #endregion


        #region chart图表接口,pitrueBox控件图表
       // void Display(double[] X, double[] Y);
       // void SetViewParam(enViewParamType paramType, object paramValue);

        #endregion
    }

    public enum enViewParamType
    {
        相机内参,
        相机外参,
        X轴平移,
        Y轴平移,
        Z轴平移,
        X轴旋转角度,
        Y轴旋转角度,
        Z轴旋转角度,
        视图类型,
        设置查找表,
        是否显示坐标系,
        图表标题,
        X轴标题,
        Y轴标题,
        X轴最大范围,
        Y轴最大范围,
        曲线颜色,
        曲线类型,
        视图名字,
        镜头类型,
    }

    public enum enViewType
    {
        高度图,
        灰度图,
        颜色灰度图,

    }


}
