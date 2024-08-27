using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsLibrary
{
    // 怎样在不同的库中做到接口统一
    // 不同的库中类型不一致，与C#没法做到类型的统一，
    // 在接口使用使用方法的重载，用一个基类来实现接口，各子类重写基类中的虚方法


    public interface ILibrary
    {
        /// <summary>
        /// 接口的定义要按功能来划分，一个功能一个接口
        /// </summary>
        /// <param name="planeObjectModel3D"></param>
        /// <param name="planePose"></param>
        #region  halcon库接口
        void GetPrimitiveObjectPose3D(HTuple planeObjectModel3D, out HTuple planePose);
        void DifferenceRegion(HObject sourceRegion, HObject subRegion, out HObject diffRegion);
        void IntersectionRegion(HObject sourceRegion, HObject targetRegion, out HObject intersecRegion);
        void UnionRegion(HObject Region1, HObject Region2, out HObject unionRegion);
        void CreateRectangle2Region(userPixRectangle2 rect2, out HObject rectRegion);
        void UpdataObjectModel(HTuple X, HTuple Y, HTuple Z, HTuple objectModel3D);
        void ClearObjectModel3D(HTuple objectModel3D);
        void UnionObjectModel(HTuple objectModel, out HTuple unionObjectModel);
        void DrawRectangle1OnWindow(HTuple window, HTuple CamPar, HTuple Pose, out userWcsRectangle1 wcsRect1);
        void DrawRectangle2OnWindow(HTuple window, HTuple CamPar, HTuple Pose, out userWcsRectangle2 wcsRect2);
        void DrawCircleOnWindow(HTuple window, HTuple CamPar, HTuple Pose, out userWcsCircle wcsCircle);
        void DrawLineOnWindow(HTuple window, HTuple CamPar, HTuple Pose, out userWcsLine wcsLine);  // 直线需不需角度信息？
        void DisplayRectangle1OnWindow(HTuple window, userWcsRectangle1 rect1, HTuple CamPar, HTuple Pose);
        void DisplayRectangle2OnWindow(HTuple window, userWcsRectangle2 rect2, HTuple CamPar, HTuple Pose);
        void DrawPointOnWindow(HTuple window, HTuple CamPar, HTuple Pose, out userWcsPoint wcsPoint);
        void DisplayCircleOnWindow(HTuple window, userWcsCircle wcsCircle, HTuple CamPar, HTuple Pose);
        void DisplayLineOnWindow(HTuple window, userWcsLine line, HTuple CamPar, HTuple Pose);
        void ReduceObjectModel3dByRectangle1(HTuple objectModel, userWcsRectangle1 wcsRect1, out HTuple reducedObjectModel3D);
        void ReduceObjectModel3dByRectangle1(HTuple objectModel, userWcsRectangle1 wcsRect1, string InsideOrOutside, out HTuple reducedObjectModel3D);
        void ReduceObjectModel3dByRectangle2(HTuple objectModel, userWcsRectangle2 wcsRect2, string InsideOrOutside, out HTuple reducedObjectModel3D);
        void ReduceObjectModel3dByCircle(HTuple objectModel, userWcsCircle wcsCircle, string InsideOrOutside, out HTuple reducedObjectModel3D);
        void AffinePoint3D(userWcsRectangle1 rect1, HTuple transPose, out userWcsRectangle1 rect);
        void AffinePoint3D(userWcsRectangle2 rect2, HTuple transPose, out userWcsRectangle2 rect);
        void AffinePoint3D(userWcsCircle circle1, HTuple transPose, out userWcsCircle circle);
        void AffinePoint3D(userWcsLine line1, HTuple transPose, out userWcsLine line);
        void ReadObjectModel(string path, double unit, out HTuple objectModel);
        void WriteObjectModelToFile(HTuple objectModel, string path);
        void TransformObject3DToRealImage(HTuple objectModel, HTuple imageWidth, out HObject imageX, out HObject imageY, out HObject imageZ);
        void TransImageToObject3D(HObject imageX, HObject imageY, HObject imageZ, out HTuple objectModel);
        bool CreatePose3D(double T_x, double T_y, double T_z, double R_x, double R_y, double R_z, string orderOfTransform, string orderOfRotation, string viewOfTransform, bool isInvert, out HTuple pose);
        void CalculatePlaneness(HTuple ObjectModel3D, out HTuple planeness);
        void FilterObject3D(HTuple objectModel, HTuple distPtoP, HTuple MinPointNum, HTuple MaxPointNum, out HTuple objectModel3DOut);
        void CalculatePointToFaceDistance(HTuple targetObjectModel1, HTuple refObjectModel, HTuple meanCount, out HTuple MaxMinMeanValue);
        void Transform3DProfileTo2DProfile(HTuple objectModel, out HTuple data_x, out HTuple data_y);
        void SampleObjectModel3D(HTuple objectModel, HTuple sampleMethod, HTuple sampleDist, out HTuple sampleObjectModel3D);
        void Get2DProfileFromObjectModel3D(HTuple objectModel, HTuple dist_offset, userWcsLine wcsLine, out HTuple objectModel2D);
        void SegmentObjectModel3DByPlane(HTuple targetObjectModel, HTuple segmentPlane, HTuple polarity, out HTuple segmentObjectModel3D);
        void SelectObjectModel3D(HTuple objectModel, HTuple minValue, HTuple maxValue, out HTuple selectObjectModel);
        void AlignObjectModel3D(HTuple objectModel, HTuple transPose, string transMethod, bool isInvertPose, out HTuple transformObjectModel3D);
        void SmoothObjectModel3D(HTuple objectModel, HTuple paramName, HTuple paramValue, out HTuple smoothObjectModel);
        void CalculateVolumeRelativePlane(HTuple targetObjectModel, HTuple planeObjectModel, out HTuple volume);
        void TriangulateObjectModel3d(HTuple objectModel, HTuple method, HTuple paramName, HTuple paramValue, out HTuple triangulatedObjectModel3D);
        void RegisterMatch(HTuple Match_ObjectModel, HTuple ref_ObjectModel, HTuple paramName, HTuple paramValue, out HTuple matchPose, out HTuple score);
        void SurfaceMatch(HTuple surfaceModelID, HTuple objectModel3D, HTuple rel_samp_Dist, HTuple key_point_fraction, HTuple minScore, HTuple returnResultHandle, HTuple paramName, HTuple paramValue, out HTuple matchPose, out HTuple score, out HTuple surfaceMatchResultID);
        void CalculaePointToLineDistance(HTuple tar_objectModel, HTuple line_objcetModel, HTuple aveNum, out HTuple dist);
        void SmoothProfileModel2D(HTuple profileModel2D, string method, HTuple smooth, out HTuple profileModel2DOut);
        void CalculaePointToPointDistance(HTuple pointObjectModel_1, HTuple pointObjectModel_2, out HTuple distValue);
        void SmoothProfileModel3D(HTuple profileModel3D, string method, HTuple smooth, out HTuple profileModel3DOut);
        void Transform3DProfileTo2DProfile(HTuple objectModel3D, out HTuple profileModel2D);
        void FitPlaneObject3D(HTuple planeObjectModel3D, out HTuple primitiveParams);
        void FitSphereObject3D(HTuple sphereObjectModel3D, out HTuple primitiveParams);
        void FitCylinderObject3D(HTuple cylinderObjectModel3D, out HTuple primitiveParams);
        void SampleProfileModel2D(HTuple profileModel2D, string sampleMethod, HTuple sampleDist, out HTuple profileModel2DOut);
        void Smooth2DPoint(double[] value, out double smoothValue);
        void UpdataObjectModel(HTuple objectModel3D, HTuple objectModel3DOut);
        void GetValueOnWindow(HTuple HalconWindow, HTuple pose, out HTuple grayValue);
        void CalculateProfileObjectThickness(HTuple ref_objectModel, HTuple tar_objcetModel, HTuple measurePos, int fitLineCount, out HTuple thickness);


        #endregion

    }

    public enum enLibraryType
    {
        halocn,
        opencv,
    }
}
