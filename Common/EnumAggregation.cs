using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public enum enCalculateCircleMethod
    {
        拟合圆,
        两点计算圆心,
    }

    [Serializable]
    public enum enCalibMethod
    {
        先旋转后平移,
        先平移后旋转,
        仅N点平移,
        标定板N点标定,
        仅旋转,
    }
    [Serializable]
    public enum enFitEllipseAlgorithm
    {
        fitzgibbon,
        fhuber,
        ftukey,
        geometric,
        geohuber,
        geotukey,
        voss,
        focpoints,
        fphuber,
        fptukey,
    }
    [Serializable]
    public enum enTransformationType
    {
        affine,
        projective,
        rigid,
        similarity
    }
    [Serializable]
    public enum enFitCircleAlgorithm
    {
        algebraic,
        ahuber,
        atukey,
        geometric,
        geohuber,
        geotukey,
    }
    [Serializable]
    public enum enOperateSign
    {
        等于,
        不等于,
        大于,
        小于,
    }

    [Serializable]
    public enum enView
    {
        俯视图,
        主视图,
        左视图
    }
    [Serializable]
    public enum enLensType
    {
        FA镜头,
        远心镜头,
    }
    [Serializable]
    public enum enVisualeViewType
    {
        模型视图,
        XLD视图,
        图像视图,
        Region视图,
    }
    [Serializable]
    public enum enIoOutputMode
    {
        NONE,
        脉冲输出,
        双脉冲输出,
        高电平输出,
        低电平输出,
        线性比较IO输出,
    }
    [Serializable]
    public enum enIoPortType
    {
        通用Io端口,
        高速Io端口,
        扩展Io端口,
        计数Io端口,
        反转Io端口,
    }
    [Serializable]
    public enum enAxisReadWriteState
    {
        ReadOnly, // 用于PLC的读地址
        WriteOnly, // 用于PLC的写地址
        ReadWrite, // 用于运动控制卡的读写地址
        Read_X, // 用于PLC的读地址
        Read_Y, // 用于PLC的读地址
        Read_Z, // 用于PLC的读地址
        Read_Theta, // 用于PLC的读地址
        Write_X, // 用于PLC的写地址
        Write_Y, // 用于PLC的写地址
        Write_Z, // 用于PLC的写地址
        Write_Theta, // 用于PLC的写地址
        // 读取写入偏移值
        Offset_X, // 用于PLC的写地址
        Offset_Y, // 用于PLC的写地址
        Offset_Theta, // 用于PLC的写地址
        //WriteOffset_X, // 用于PLC的写地址
        //WriteOffset_Y, // 用于PLC的写地址
        //WriteOffset_Theta, // 用于PLC的写地址
        // 读取写入补偿值
        Compensation_X, // 用于PLC的写地址
        Compensation_Y, // 用于PLC的写地址
        Compensation_Theta, // 用于PLC的写地址
        //WriteCompensation_X, // 用于PLC的写地址
        //WriteCompensation_Y, // 用于PLC的写地址
        //WriteCompensation_Theta, // 用于PLC的写地址
    }

    //数据类型
    [Serializable]
    public enum enDataTypes : short
    {
        Bool = 0,
        Byte = 1,
        Short = 2,
        Ushort = 3,
        Int = 4,
        UInt = 5,
        Long = 6,
        ULong = 7,
        Float = 8,
        Double = 9,
        String = 10,
        Coil = 11, // 线圈
        Discrete = 12,
        // 数组读取
        BoolArray,
        ByteArray,
        ShortArray,
        UshortArray,
        IntArray,
        UIntArray,
        LongArray,
        ULongArray,
        FloatArray,
        DoubleArray,
        StringArray,
        CoilArray, // 线圈
        Grab, // 弃用
        Calib,// 弃用
        GetData,// 弃用
        UpAOICam,// 弃用
        DownAOICam,// 弃用
        UpAlignCam,// 弃用
        DownAlignCam,// 弃用
        SocketCommand,// 弃用
        Trigger,// 弃用
        //DiscreteArray,
        //GrabUpAOICam,
        //GrabDownAOICam,
        //GrabUpAlignCam,
        //GrabDownAlignCam,
        //CalibUpAlignCam,
        //CalibDownAlignCam,
        //GetDataUpAlignCam,
        //GetDataDownAlignCam,
    }


    [Serializable]
    public enum enCoordValueType
    {
        绝对坐标 = 0,  // 机械坐标系中的坐标位置 
        相对坐标, // 相对于某个参考点的位置，如旋转中心
    }
    [Serializable]
    public enum enCoordOriginType
    {
        机械原点,  // 机械坐标系中的坐标位置 
        旋转中心, // 相对于某个参考点的位置，如旋转中心
        IsLoading, // 表示上下料模式,该手眼标定模式是否用于上料，当相机静止时，此时计算出的坐标的原点在机器人法兰中心
        映射变换,
    }
    //[Serializable]
    //public enum enCoordType
    //{
    //    绝对坐标,  
    //    相对坐标, 
    //}
    /// <summary> 标定模式枚举 </summary>
    [Serializable]
    public enum enCamCaliModel
    {
        NONE,
        HandEyeCali = 0,  // 标定相机与机械坐标系的关系
        CaliCaliBoard,
        Cali9PtCali,
        UpDnCamCali, // 标定上下相机的位置关系
        MapCalib,
        NPointCali, // N点标定
        CamParamPose, // 用于3D场景的校准
        HomMat2D, // 用于2D场景的校准
        RefPose, // 用于2D场景的校准
    }
    [Serializable]
    public enum enCalibAxis
    {
        XY轴,
        X轴,
        Y轴,
        单轴,
    }
    [Serializable]
    public enum enCalculateMethod
    {
        拟合圆,
        矩阵变换,
        两点夹角,
        N点矩阵变换,
    }
    [Serializable]
    public enum enInvertAxis
    {
        NONE,
        X轴,
        Y轴,
    }
    [Serializable]
    public enum enRotateDirection
    {
        正向,
        负向,
        双向,
    }
    [Serializable]
    public enum enMoveStage
    {
        PLC,
        Socket,
        Card,
    }
    [Serializable]
    public enum enCalibPlane
    {
        XY,
        XZ,
        YZ,
    }
    [Serializable]
    public enum enCaliStateEnum
    {
        RotCali = 0,
        Cali9Pt,
    }
    [Serializable]
    public enum enCoordSysName
    {
        NONE, // 用于指定Socket的坐标系
        CoordSys_0,
        CoordSys_1,
        CoordSys_2,
        CoordSys_3,
        CoordSys_4,
        CoordSys_5,
        CoordSys_6,
        CoordSys_7,
        CoordSys_8,
        CoordSys_9,
        CoordSys_10,
        VirtualCoordSys, // 用于指定自定义设备的坐标系
        //MemoryInfo,  // 用于从内存中读取信息
    }
    [Serializable]
    public enum enCoordiSensorHandEyeMat
    {
        Coordi0Cam0,
        Coordi0Cam1,
        Coordi0Cam2,
        Coordi0Cam3,
        Coordi0Cam4,
        Coordi0Cam5,
        Coordi0Cam6,
        Coordi0Cam7,

        Coordi1Cam0,
        Coordi1Cam1,
        Coordi1Cam2,
        Coordi1Cam3,
        Coordi1Cam4,
        Coordi1Cam5,
        Coordi1Cam6,
        Coordi1Cam7,


        Coordi2Cam0,
        Coordi2Cam1,
        Coordi2Cam2,
        Coordi2Cam3,
        Coordi2Cam4,
        Coordi2Cam5,
        Coordi2Cam6,
        Coordi2Cam7,

        Coordi3Cam0,
        Coordi3Cam1,
        Coordi3Cam2,
        Coordi3Cam3,
        Coordi3Cam4,
        Coordi3Cam5,
        Coordi3Cam6,
        Coordi3Cam7,

        Coordi4Cam0,
        Coordi4Cam1,
        Coordi4Cam2,
        Coordi4Cam3,
        Coordi4Cam4,
        Coordi4Cam5,
        Coordi4Cam6,
        Coordi4Cam7,


        Coordi5Cam0,
        Coordi5Cam1,
        Coordi5Cam2,
        Coordi5Cam3,
        Coordi5Cam4,
        Coordi5Cam5,
        Coordi5Cam6,
        Coordi5Cam7,

        Coordi0Laser0,
        Coordi0Laser1,
        Coordi0Laser2,
        Coordi0Laser3,
        Coordi0Laser4,
        Coordi0Laser5,
        Coordi0Laser6,
        Coordi0Laser7,

        Coordi1Laser0,
        Coordi1Laser1,
        Coordi1Laser2,
        Coordi1Laser3,
        Coordi1Laser4,
        Coordi1Laser5,
        Coordi1Laser6,
        Coordi1Laser7,

        Coordi2Laser0,
        Coordi2Laser1,
        Coordi2Laser2,
        Coordi2Laser3,
        Coordi2Laser4,
        Coordi2Laser5,
        Coordi2Laser6,
        Coordi2Laser7,
    }
    [Serializable]
    public enum enStructureElement
    {
        DilationCircle,
        DilationRectangle1,
        ErosionCircle,
        ErosionRectangle1,
        ClosingCircle,
        ClosingRectangle1,
        OpeningCircle,
        OpeningRectangle1,
        FillUp,
    }

    [Serializable]
    public enum enSegmentMethod
    {
        lines,
        lines_circles,
        lines_ellipses
    }

    [Serializable]
    public enum enAttributeParam
    {
        edge_direction,
        angle,
        response,
        width_right,
        width_left,
        contrast,
        asymmetry,
        distance,
    }

    [Serializable]
    public enum enOperationParam
    {
        and,
        or,
    }

    [Serializable]
    public enum enFitRect2Method
    {
        regression,
        huber,
        tukey,
    }

    [Serializable]
    public enum enSegmentPolarity
    {
        positive,
        negative,
    }

    [Serializable]
    public enum enRegionType
    {
        circle,
        rect2,
    }

    [Serializable]
    public enum enKeepRegion
    {
        Inside,
        Outside,
    }

    [Serializable]
    public enum enTransformMethod
    {
        rigid_trans_object_model_3d,
        projective_trans_object_model_3d,
        affine_trans_object_model_3d,
    }
    [Serializable]
    public enum enPrimitiveFitAlgorith
    {
        least_squares,
        least_squares_huber,
        least_squares_tukey,
    }
    [Serializable]
    public enum enUserSensorType
    {
        NONE,
        点激光,
        线激光,
        面激光,
        线阵相机,
        面阵相机,
    }

    [Serializable]
    public enum enUserMeasureMode
    {
        Distance,
        Thickness,
    }
    [Serializable]
    public enum enUserPeakMode
    {
        FirstPeak,
        StrongestPeak,
        SecondPeak,
        ThreePeak,
        FourPeak,
        FivePeak,
        LastPeak,
    }

    [Serializable]
    public enum enUserTriggerDirection
    {
        ClockwiseForward,
        AntiClockwiseBackward,
        Both,
    }

    [Serializable]
    public enum enImageRenderType
    {
        彩色渲染,
        黑白渲染,
    }

    [Serializable]
    public enum enUserPeakMode2
    {
        FirstPeak,
        StrongestPeak,
    }

    [Serializable]
    public enum enUserLightMode
    {
        Auto,
        Manual,
    }

    [Serializable]
    public enum enUserTriggerSource
    {
        NONE,
        软触发,
        外部IO触发,
        内部IO触发, // 指由软件来控制的IO触发
        编码器触发,
        Line0,
        Line1,
        Line2,
        Line3,
    }

    [Serializable]
    public enum enUserTrigerMode
    {
        NONE,
        TRS,
        TRE,
        TRN,
        TRG,
    }

    [Serializable]
    public enum enUserExpourseMode
    {
        Auto,
        Manual,
    }

    [Serializable]
    public enum enUserRefractiveMode
    {
        自定义,
        index1 = 1,
        index2 = 2,
        index3 = 3,
        index4 = 4,
        index5 = 5,
        index6 = 6,
        index7 = 7,
        index8 = 8,
        index9 = 9,
        index10 = 10,
        index11 = 11,
        index12 = 12,
        index13 = 13,
        index14 = 14,
        index15 = 15,
        index16 = 16,
        index17 = 17,
        index18 = 18,
        index19 = 19,
        index20 = 20,
    }

    [Serializable]
    public enum enGrabMode
    {
        LiveImage,
        PilImage,
        ZILImage,
        PointCloud,
    }

    [Serializable]
    public enum enExposeMode
    {
        SingleExpose,
        DoubleExpose,
    }

    [Serializable]
    public enum enMeasureLineDirection
    {
        LeftToRight,
        RightToLeft,
        UpToDown,
        DownToUp,
    }

    [Serializable]
    public enum enMeasureCircleDirection
    {
        OutToInner,
        InnerToOut,
    }

    [Serializable]
    public enum enMeasureSelect
    {
        all,
        first,
        last,
    }

    [Serializable]
    public enum enMeasureTransition
    {
        all,
        negative,
        positive,
        uniform,
    }

    [Serializable]
    public enum enViewType
    {
        color1,
        color2,
        color3,
        color4,
        three,
        six,
        twelve,
        twenty_four,
        rainbow,
        temperature,
        linear,
        colored,
    }

    [Serializable]
    public enum enViewQuality
    {
        low,
        high,
    }

    [Serializable]
    public enum enViewIntensity
    {
        coord_x,
        coord_y,
        coord_z,
        none,
    }

    [Serializable]
    public enum enOffsetType
    {
        正向偏置,
        负向偏置,
        对称偏置,
    }

    [Serializable]
    public enum enMinMaxMode
    {
        极大值,
        极小值,
    }

    [Serializable]
    public enum enThickMeasureMethod
    {
        两点距离和,
        两点距离差,
        两线距离和,
        两线距离差,
        两平面距离和,
        两平面距离差,
    }

    [Serializable]
    public enum enArrayDirection   //
    {
        X轴,
        Y轴,
    }

    [Serializable]
    public enum enFittingAlgorithm
    {
        least_squares,
        least_squares_huber,
        least_squares_tukey,
    }

    [Serializable]
    public enum enCameraModel
    {
        area_scan_division,
        area_scan_telecentric_division,
        area_scan_tilt_division,
        area_scan_telecentric_tilt_division,
        area_scan_polynomial,
        area_scan_telecentric_polynomial,
        area_scan_tilt_polynomial,
        area_scan_telecentric_tilt_polynomial,
        HandleEyeCalib,
        // line_scan,
    }

    [Serializable]
    public enum enCamCalibrateType
    {
        单相机标定,
        单相机移动标定,
        多相机标定,
    }

    [Serializable]
    public enum enCalibrationSetupType
    {
        calibration_object,
        hand_eye_moving_cam,
        hand_eye_scara_moving_cam,
        hand_eye_scara_stationary_cam,
        hand_eye_stationary_cam,
    }

    [Serializable]
    public enum enCalculatePoseMethod
    {
        analytic,
        iterative,
        planar_analytic,
    }

    [Serializable]
    public enum enFitLineMethod
    {
        regression,
        huber,
        tukey,
        drop,
        gauss,
    }

    [Serializable]
    public enum enFitCircleMethod
    {
        algebraic,
        ahuber,
        atukey,
        geometric,
        geohuber,
        geotukey,
    }

    [Serializable]
    public enum enFitEllipseMethod
    {
        fitzgibbon,
        fhuber,
        ftukey,
        geometric,
        geohuber,
        geotukey,
        voss,
        focpoints,
        fphuber,
        fptukey,
    }
    [Serializable]
    public enum enConstructionMethod
    {
        线,
        圆_线,
        线_线,
        点_线,
    }

    [Serializable]
    public enum enOrigionType
    {
        圆心,
        交点,
        直线中点,
        直线起点,
        直线终点,
        X轴交点,
        Y轴交点,
    }

    [Serializable]
    public enum enAdjustType
    {
        XYTheta,
        X,
        Y,
        XY,
        Theta,
        NONE,
    }

    [Serializable]
    public enum enAffineTransOrientation
    {
        正向变换,
        反向变换,
    }

    [Serializable]
    public enum enCoordinateSystemType
    {
        当前坐标系,
        变化坐标系,
    }

    [Serializable]
    public enum enImageType
    {
        PointsCloud,
        DeepImage
    }

    [Serializable]
    public enum enMeasureEnvironmentConfig
    {
        影像仪测量,
        一键式测量,
        直线度测量,
        粗糙度测量,
        相机激光测量,
        对射测厚,
    }

    [Serializable]
    public enum enDistOrientation
    {
        XZ,
        Y,
        YZ,
        XYZ,
    }

    [Serializable]
    public enum enColor
    {
        green, // 默认构造函数使用第一个值来初始化
        white,
        red,
        blue,
        gray,
        cyan,
        magenta,
        yellow,
        coral,
        orange,
        pink,
    }
    [Serializable]
    public enum enFontPosition
    {
        左上角,
        右上角,
        左下角,
        右下角

    }

    [Serializable]
    public enum enUserConnectType // 通过映射来连接传感器
    {
        NONE,
        Network,
        USB,
        SerialPort,
        SerialNumber,
        Modbus,
        TcpIp,
        Map,
        DeviceName,
    }


    [Serializable]
    public enum enScanAxis
    {
        X轴,
        Y轴,
        Z轴,
        单点采集
    }

    [Serializable]
    public enum enDeviceMode
    {
        主设备,
        从设备,
    }
    [Serializable]
    public enum enAcqType
    {
        单点采集,
        多点采集
    }
    [Serializable]
    public enum enOperateMethod
    {
        加,
        减,
        乘,
        除,
    }
    [Serializable]
    public enum enExtremePointCoord
    {
        X轴,
        Y轴,
        Z轴,
    }
    [Serializable]
    public enum enExtremePointMode
    {
        极大值,
        极小值,
    }
    [Serializable]
    public enum enCreateShapeModelMethod
    {
        create_aniso_shape_model,
        create_aniso_shape_model_xld,
        create_scaled_shape_model,
        create_scaled_shape_model_xld,
        create_shape_model,
        create_shape_model_xld,
    }
    [Serializable]
    public enum enFindShapeModelMethod
    {
        find_aniso_shape_model,
        find_scaled_shape_model,
        find_shape_model,
    }
    [Serializable]
    public enum enCreateDeformableModelMethod
    {
        create_local_deformable_model,
        create_local_deformable_model_xld,
        create_planar_calib_deformable_model,
        create_planar_calib_deformable_model_xld,
        create_planar_uncalib_deformable_model,
        create_planar_uncalib_deformable_model_xld
    }

    [Serializable]
    public enum enFindDeformableModelMethod
    {
        find_local_deformable_model,
        find_planar_calib_deformable_model,
        find_planar_uncalib_deformable_model,
    }
    [Serializable]
    public enum enProfileSmoothMethod
    {
        均值平滑,
        高斯平滑,
    }
    [Serializable]
    public enum enFitCoordPoint
    {
        XY,
        XZ,
        YZ,
        XYZ,
    }

    [Serializable]
    public enum enUserLevelEdgeFlag
    {
        RISING_EDGE = 0,
        FALLING_EDGE = 1
    }

    [Serializable]
    public enum enLablePosition
    {
        用户定义,
        左上角,
        右上角,
        左下角,
        右下角,
    }

    [Serializable]
    public enum enShapeType
    {
        点,
        线,
        圆,
        椭圆,
        矩形1,
        矩形2,
        多边形,
        多段线,
        NONE,
    }

    [Serializable]
    public enum enInsideOrOutside
    {
        NONE,
        保留,
        移除,
    }




}
