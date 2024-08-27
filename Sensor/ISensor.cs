using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using HalconDotNet;

namespace Sensor
{
    /// <summary>
    /// 接口的作用是为了规范化一组行为
    /// </summary>
    public interface ISensor
    {
        event PointCloudAcqCompleteEventHandler PointsCloudAcqComplete;
        event ImageAcqCompleteEventHandler ImageAcqComplete;
        bool Init();

        /// <summary>
        /// 设置传感器参数
        /// </summary>
        /// <returns></returns>
        bool SetParam(object paramType, object value);

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="paramType"></param>
        /// <returns></returns>
        object GetParam(object paramType);

        /// <summary>
        /// 连接控制器
        /// </summary>
        /// <returns></returns>
        bool Connect(SensorConnectConfigParam configParam);

        /// <summary>
        /// 断开传感器连接
        /// </summary>
        /// <returns></returns>
        bool Disconnect();

        /// <summary>
        /// 触发传感器采集
        /// </summary>
        /// <returns></returns>
        bool StartTrigger();

        /// <summary>
        /// 停止触发传感器采集
        /// </summary>
        /// <returns></returns>
        bool StopTrigger();

        /// <summary>
        /// 从控制器中读取采集数据     0:dist;/ 1:dist2; /2:thick;/ 3:intensity;/ 4:x;/ 5:y; /6:z;/ 7:baryCenter
        /// </summary>
        /// <returns></returns>
        //List<object> ReadData();
       Dictionary<enDataItem,object> ReadData();

        string Name { get; set; }

        CameraParam CameraParam { get; set; }
        LaserParam LaserParam { get; set; }
        SensorConnectConfigParam ConfigParam { get; set; }

        HImage GrabImage { get; set; }
        HImage GrabDarkImage { get; set; }
    }

    [Serializable]
    public enum enDataItem
    {
        Image,
        DarkImage,
        Dist1,
        Dist2,
        Dist3,
        Thick,
        Dist1Modle,
        Dist2Modle,
        Dist3Modle,
        ThickModle,
        Intensity,
        Quality,
        BaryCenter,
        X,
        Y,
        Z,
        Theta,
        U,
        V,
        GrabSucceeded,
    }

    [Serializable]
    public enum enSensorParamType
    {
        #region Stil传感器参数
        Stil_曝光,
        Stil_光源亮度,
        Stil_检测阈值,
        Stil_触发模式,
        Stil_测量模式,
        Stil_高度模式的峰值选择,
        Stil_厚度模式下峰值1,
        Stil_厚度模式下峰值2,
        Stil_保存配置,
        Stil_暗黑校正,
        Stil_热校正,
        Stil_光强校正,
        Stil_待处理的峰值数量,
        Stil_测量范围,
        Stil_置零编码器,
        ////  点参数
        Stil_设置预置频率,
        Stil_获取频率列表,
        Stil_获取当前频率,
        Stil_获取光学笔列表,
        Stil_获取当前光学笔,
        Stil_获取最小暗黑频率,
        Stil_获取当前测量峰值,
        Stil_获取当前测量模式,
        Stil_获取当前测量阈值,
        Stil_获取当前光源模式,
        Stil_设置手动光源模式,
        Stil_设置自动光源模式,
        Stil_设置距离模式,
        Stil_设置厚度模式,
        Stil_设置最强峰值,
        Stil_设置第一峰值,
        Stil_设置测量阈值,
        Stil_设置光学笔,
        Stil_暗校正,
        Stil_保存参数,
        Stil_获取光源模式,
        Stil_设置自动光源模式下的亮度,
        Stil_设置手动光源模式下的亮度,
        Stil_获取自动光源模式下的亮度,
        Stil_获取手动光源模式下的亮度,
        #endregion

        #region SmartRay传感器参数
        smartRay_曝光模式,
        smartRay_激光亮度,
        smartRay_曝光时间,
        smartRay_增益,
        smartRay_内部触发频率,
        smartRay_外部触发,
        smartRay_内部触发,
        smartRay_自由触发,
        smartRay_触发参数,
        smartRay_触发模式,
        smartRay_图像采集类型,
        smartRay_激光模式,
        smartRay_兴趣区域,
        smartRay_扫描轮廓数量,
        smartRay_激光线阈值,
        smartRay_多重曝光合并模式,
        #endregion

        #region LiYi传感器参数
        Liyi_曝光,
        Liyi_采集频率,
        Liyi_检测阈值,
        Liyi_触发模式,
        Liyi_触发源,
        Liyi_测量模式,
        Liyi_高度模式的峰值选择,
        Liyi_厚度模式下峰值1,
        Liyi_厚度模式下峰值2,
        Liyi_保存配置,
        Liyi_暗黑校正,
        Liyi_测量范围,
        Liyi_增益,
        Liyi_标准厚度,
        #endregion

        #region BoMing传感器参数

        #endregion

        #region LVM传感器参数
        LVM_曝光,
        LVM_激光亮度,
        LVM_数字增益,
        LVM_模拟增益,
        LVM_触发类型,
        LVM_触发频率,
        LVM_触发分频,
        LVM_采集类型,
        LVM_增益偏置,
        LVM_检测阈值,
        LVM_起始位置,
        LVM_结束位置,
        LVM_脉冲方向,
        LVM_扫描行数,
        #endregion


        #region 大恒相机传感器参数
        DaHeng_曝光,
        DaHeng_增益,
        DaHeng_触发源,
        DaHeng_触发模式,
        DaHeng_触发极性,

        #endregion


        #region 迈德威视相机参数
        MindVision_相机句柄,
        #endregion

        #region 公共传感器参数
        this对象,
        Coom_每线点数,
        Coom_当前采集数量,
        Coom_传感器名称,
        Coom_激光位姿,
        Coom_相机内参,
        Coom_相机外参,
        Coom_激光校准参数,
        Coom_传感器类型,
        Coom_保存参数,
        Coom_相机角度,
        Coom_相机校准档,
        Coom_相机校准档_std_x,
        Coom_相机校准档_std_y,
        Coom_相机校准档_cur_x,
        Coom_相机校准档_cur_y,
        Coom_点云宽度,
        Coom_点云高度,
        Coom_数据类型,
        Coom_测量范围
        #endregion


    }

    [Serializable]
    public enum enAcqState
    {
        NONE,
        Start,
        Continue,
        End,
    }


}
