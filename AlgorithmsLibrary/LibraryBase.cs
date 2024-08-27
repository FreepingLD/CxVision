using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using HalconDotNet;

namespace AlgorithmsLibrary
{
    public class LibraryBase : ILibrary
    {


        #region  halocn接口实现

        public virtual void AffinePoint3D(userWcsRectangle2 rect2, HTuple transPose, out userWcsRectangle2 rect)
        {
            throw new NotImplementedException();
        }

        public virtual void AffinePoint3D(userWcsLine line1, HTuple transPose, out userWcsLine line)
        {
            throw new NotImplementedException();
        }

        public virtual void AffinePoint3D(userWcsCircle circle1, HTuple transPose, out userWcsCircle circle)
        {
            throw new NotImplementedException();
        }

        public virtual void AffinePoint3D(userWcsRectangle1 rect1, HTuple transPose, out userWcsRectangle1 rect)
        {
            throw new NotImplementedException();
        }
        public virtual void UpdataObjectModel(HTuple X, HTuple Y, HTuple Z, HTuple objectModel3D)
        {
            throw new NotImplementedException();
        }
        public virtual void ClearObjectModel3D(HTuple objectModel3D)
        {
            throw new NotImplementedException();
        }

        public virtual void CreateRectangle2Region(userPixRectangle2 rect2, out HObject rectRegion)
        {
            throw new NotImplementedException();
        }

        public virtual void DifferenceRegion(HObject sourceRegion, HObject subRegion, out HObject diffRegion)
        {
            throw new NotImplementedException();
        }

        public virtual void DisplayCircleOnWindow(HTuple window, userWcsCircle wcsCircle, HTuple CamPar, HTuple Pose)
        {
            throw new NotImplementedException();
        }

        public virtual void DisplayLineOnWindow(HTuple window, userWcsLine line, HTuple CamPar, HTuple Pose)
        {
            throw new NotImplementedException();
        }

        public virtual void DisplayRectangle1OnWindow(HTuple window, userWcsRectangle1 rect1, HTuple CamPar, HTuple Pose)
        {
            throw new NotImplementedException();
        }

        public virtual void DisplayRectangle2OnWindow(HTuple window, userWcsRectangle2 rect2, HTuple CamPar, HTuple Pose)
        {
            throw new NotImplementedException();
        }

        public virtual void DrawCircleOnWindow(HTuple window, HTuple CamPar, HTuple Pose, out userWcsCircle wcsCircle)
        {
            throw new NotImplementedException();
        }

        public virtual void DrawPointOnWindow(HTuple window, HTuple CamPar, HTuple Pose,  out userWcsPoint wcsPoint)
        {
            throw new NotImplementedException();
        }
        public virtual void DrawLineOnWindow(HTuple window, HTuple CamPar, HTuple Pose, out userWcsLine wcsLine)
        {
            throw new NotImplementedException();
        }

        public virtual void DrawRectangle1OnWindow(HTuple window, HTuple CamPar, HTuple Pose, out userWcsRectangle1 wcsRect1)
        {
            throw new NotImplementedException();
        }

        public virtual void DrawRectangle2OnWindow(HTuple window, HTuple CamPar, HTuple Pose, out userWcsRectangle2 wcsRect2)
        {
            throw new NotImplementedException();
        }

        public virtual void GetPrimitiveObjectPose3D(HTuple planeObjectModel3D, out HTuple planePose)
        {
            throw new NotImplementedException();
        }

        public virtual void IntersectionRegion(HObject sourceRegion, HObject targetRegion, out HObject intersecRegion)
        {
            throw new NotImplementedException();
        }

        public virtual void ReadObjectModel(string path, double unit, out HTuple objectModel)
        {
            throw new NotImplementedException();
        }

        public virtual void ReduceObjectModel3dByCircle(HTuple objectModel, userWcsCircle wcsCircle, string InsideOrOutside, out HTuple reducedObjectModel3D)
        {
            throw new NotImplementedException();
        }

        public virtual void ReduceObjectModel3dByRectangle1(HTuple objectModel, userWcsRectangle1 wcsRect1, out HTuple reducedObjectModel3D)
        {
            throw new NotImplementedException();
        }

        public virtual void ReduceObjectModel3dByRectangle1(HTuple objectModel, userWcsRectangle1 wcsRect1, string InsideOrOutside, out HTuple reducedObjectModel3D)
        {
            throw new NotImplementedException();
        }

        public virtual void ReduceObjectModel3dByRectangle2(HTuple objectModel, userWcsRectangle2 wcsRect2, string InsideOrOutside, out HTuple reducedObjectModel3D)
        {
            throw new NotImplementedException();
        }

        public virtual void TransformObject3DToRealImage(HTuple objectModel, HTuple imageWidth, out HObject imageX, out HObject imageY, out HObject imageZ)
        {
            throw new NotImplementedException();
        }


        public virtual void TransImageToObject3D(HObject imageX, HObject imageY, HObject imageZ, out HTuple objectModel)
        {
            throw new NotImplementedException();
        }

        public virtual void UnionObjectModel(HTuple objectModel, out HTuple unionObjectModel)
        {
            throw new NotImplementedException();
        }

        public virtual void UnionRegion(HObject Region1, HObject Region2, out HObject unionRegion)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteObjectModelToFile(HTuple objectModel, string path)
        {
            throw new NotImplementedException();
        }

        public virtual bool CreatePose3D(double T_x, double T_y, double T_z, double R_x, double R_y, double R_z, string orderOfTransform, string orderOfRotation, string viewOfTransform, bool isInvert, out HTuple pose)
        {
            throw new NotImplementedException();
        }

        public virtual void CalculatePlaneness(HTuple ObjectModel3D, out HTuple planeness)
        {
            throw new NotImplementedException();
        }

        public virtual void FilterObject3D(HTuple objectModel, HTuple distPtoP, HTuple MinPointNum, HTuple MaxPointNum, out HTuple objectModel3DOut)
        {
            throw new NotImplementedException();
        }

        public virtual void CalculatePointToFaceDistance(HTuple targetObjectModel1, HTuple refObjectModel, HTuple meanCount, out HTuple MaxMinMeanValue)
        {
            throw new NotImplementedException();
        }

        public virtual void Transform3DProfileTo2DProfile(HTuple objectModel, out HTuple data_x, out HTuple data_y)
        {
            throw new NotImplementedException();
        }

        public virtual void SampleObjectModel3D(HTuple objectModel, HTuple sampleMethod, HTuple sampleDist, out HTuple sampleObjectModel3D)
        {
            throw new NotImplementedException();
        }

        public virtual void Get2DProfileFromObjectModel3D(HTuple objectModel, HTuple dist_offset, userWcsLine wcsLine, out HTuple objectModel2D)
        {
            throw new NotImplementedException();
        }

        public virtual void SegmentObjectModel3DByPlane(HTuple targetObjectModel, HTuple segmentPlane, HTuple polarity, out HTuple segmentObjectModel3D)
        {
            throw new NotImplementedException();
        }
        public virtual void SelectObjectModel3D(HTuple objectModel, HTuple minValue, HTuple maxValue, out HTuple selectObjectModel)
        {
            throw new NotImplementedException();
        }

        public virtual void AlignObjectModel3D(HTuple objectModel, HTuple transPose, string transMethod, bool isInvertPose, out HTuple transformObjectModel3D)
        {
            throw new NotImplementedException();
        }
        public virtual void SmoothObjectModel3D(HTuple objectModel, HTuple paramName, HTuple paramValue, out HTuple smoothObjectModel)
        {
            throw new NotImplementedException();
        }

        public virtual void CalculateVolumeRelativePlane(HTuple targetObjectModel, HTuple planeObjectModel, out HTuple volume)
        {
            throw new NotImplementedException();
        }

        public virtual void TriangulateObjectModel3d(HTuple objectModel, HTuple method, HTuple paramName, HTuple paramValue, out HTuple triangulatedObjectModel3D)
        {
            throw new NotImplementedException();
        }

        public virtual void SurfaceMatch(HTuple surfaceModelID, HTuple objectModel3D, HTuple rel_samp_Dist, HTuple key_point_fraction, HTuple minScore, HTuple returnResultHandle, HTuple paramName, HTuple paramValue, out HTuple matchPose, out HTuple score, out HTuple surfaceMatchResultID)
        {
            throw new NotImplementedException();
        }

        public virtual void RegisterMatch(HTuple Match_ObjectModel, HTuple ref_ObjectModel, HTuple paramName, HTuple paramValue, out HTuple matchPose, out HTuple score)
        {
            throw new NotImplementedException();
        }
        public virtual void CalculaePointToLineDistance(HTuple tar_objectModel, HTuple line_objcetModel, HTuple aveNum, out HTuple dist)
        {
            throw new NotImplementedException();
        }
        public virtual void SmoothProfileModel2D(HTuple profileModel2D, string method, HTuple smooth, out HTuple profileModel2DOut)
        {
            throw new NotImplementedException();
        }

        public virtual void Smooth2DPoint(double[] value, out double smoothValue)
        {
            throw new NotImplementedException();
        }
        public virtual void CalculaePointToPointDistance(HTuple pointObjectModel_1, HTuple pointObjectModel_2, out HTuple distValue)
        {
            throw new NotImplementedException();
        }

        public virtual void SmoothProfileModel3D(HTuple profileModel3D, string method, HTuple smooth, out HTuple profileModel3DOut)
        {
            throw new NotImplementedException();
        }
        public virtual void Transform3DProfileTo2DProfile(HTuple objectModel3D, out HTuple profileModel2D)
        {
            throw new NotImplementedException();
        }
        public virtual void FitPlaneObject3D(HTuple planeObjectModel3D, out HTuple primitiveParams)
        {
            throw new NotImplementedException();
        }
        public virtual void FitSphereObject3D(HTuple sphereObjectModel3D, out HTuple primitiveParams)
        {
            throw new NotImplementedException();
        }
        public virtual void FitCylinderObject3D(HTuple cylinderObjectModel3D, out HTuple primitiveParams)
        {
            throw new NotImplementedException();
        }
        public virtual void SampleProfileModel2D(HTuple profileModel2D, string sampleMethod, HTuple sampleDist, out HTuple profileModel2DOut)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdataObjectModel(HTuple objectModel3D, HTuple objectModel3DOut)
        {
            throw new NotImplementedException();
        }

        public virtual void GetValueOnWindow(HTuple HalconWindow, HTuple pose, out HTuple grayValue)
        {
            throw new NotImplementedException();
        }

        public virtual void CalculateProfileObjectThickness(HTuple ref_objectModel, HTuple tar_objcetModel, HTuple measurePos, int fitLineCount, out HTuple thickness)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
