using AlgorithmsLibrary;
using Common;
using HalconDotNet;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class ThicknessParam
    {
        public bool IsDisplayLable { get; set; }
        public int Size { get; set; }
        public int RowOffset { get; set; }
        public int ColOffset { get; set; }
        public int AveCount { get; set; }
        private enThickMeasureMethod Method { get; set; }
        public ThicknessParam()
        {
            this.AveCount = 1;
            this.Size = 15;
            this.IsDisplayLable = false;
            this.RowOffset = 10;
            this.ColOffset = 0;
            this.Method = enThickMeasureMethod.两点距离和;
        }

        public bool Distance2D(HObjectModel3D objectModel1, HObjectModel3D objectModel2, out userWcsThick wcsThick)
        {
            bool result = false;
            ///////////////////////////
            int length;
            List<double> listThick = new List<double>();
            List<double> listDist1 = new List<double>();
            List<double> listDist2 = new List<double>();
            List<double> listX = new List<double>();
            List<double> listY = new List<double>();
            HTuple X, Y, Z1, X2, Y2, Z2;
            wcsThick = new userWcsThick();
            HalconLibrary ha = new HalconLibrary();
            //////////////////////
            if (objectModel1 == null)
            {
                throw new ArgumentNullException("objectModel1");
            }
            if (objectModel2 == null)
            {
                throw new ArgumentNullException("objectModel2");
            }
            if (!objectModel1.IsInitialized())
            {
                throw new ArgumentException("objectModel1:中未初始化，不包含元素");
            }
            if (!objectModel2.IsInitialized())
            {
                throw new ArgumentException("objectModel2:中未初始化，中不包含元素");
            }
            ///////////////////////////////////////////////////////////////////////    
            switch (this.Method)
            {
                case enThickMeasureMethod.两点距离和:
                    X = objectModel1.GetObjectModel3dParams("point_coord_x");
                    Y = objectModel1.GetObjectModel3dParams("point_coord_y");
                    Z1 = objectModel1.GetObjectModel3dParams("point_coord_z");
                    Z2 = objectModel2.GetObjectModel3dParams("point_coord_z");
                    length = Z1.Length > Z2.Length ? Z2.Length : Z1.Length;
                    for (int ii = 0; ii < length; ii++)
                    {
                        listX.Add(X[ii].D);
                        listY.Add(Y[ii].D);
                        listDist1.Add(Z1[ii].D * 1000);
                        listDist2.Add(Z2[ii].D * 1000);
                        listThick.Add((Z1[ii].D + Z2[ii].D + GlobalVariable.pConfig.Cord_Gap) * 1000);
                    }
                    result = true;
                    break;
                case enThickMeasureMethod.两点距离差:
                    X = objectModel1.GetObjectModel3dParams("point_coord_x");
                    Y = objectModel1.GetObjectModel3dParams("point_coord_y");
                    Z1 = objectModel1.GetObjectModel3dParams("point_coord_z");
                    Z2 = objectModel2.GetObjectModel3dParams("point_coord_z");
                    length = Z1.Length > Z2.Length ? Z2.Length : Z1.Length;
                    if (Z1.Length == Z2.Length)
                    {
                        for (int ii = 0; ii < Z1.Length; ii++)
                        {
                            listX.Add(X[ii].D);
                            listY.Add(Y[ii].D);
                            listDist1.Add(Z1[ii].D * 1000);
                            listDist2.Add(Z2[ii].D * 1000);
                            listThick.Add(Math.Abs(Z1[ii].D - Z2[ii].D) * 1000);
                        }
                    }
                    else
                    {
                        for (int ii = 0; ii < Z1.Length; ii++)
                        {
                            listX.Add(X[ii].D);
                            listY.Add(Y[ii].D);
                            listDist1.Add(Z1[ii].D * 1000);
                            listDist2.Add(Z2[ii].D * 1000);
                            listThick.Add(Math.Abs(Z1[ii].D - Z2[0].D) * 1000);
                        }
                    }
                    result = true;
                    break;
                case enThickMeasureMethod.两线距离和:
                    double start_x1, end_x1, start_y1, end_y1, start_z1, end_z1, start_x2, end_x2, start_y2, end_y2, start_z2, end_z2;
                    ha.FitLine3D(objectModel1, out start_x1, out start_y1, out start_z1, out end_x1, out end_y1, out end_z1);
                    ha.FitLine3D(objectModel2, out start_x2, out start_y2, out start_z2, out end_x2, out end_y2, out end_z2);
                    listX.Add((start_x1 + end_x1) * 0.5);
                    listY.Add((start_y1 + end_y1) * 0.5);
                    listDist1.Add((start_z1 + end_z1) * 0.5 * 1000);
                    listDist2.Add((start_z2 + end_z2) * 0.5 * 1000);
                    listThick.Add(((start_z1 + end_z1) * 0.5 + (start_z2 + end_z2) * 0.5 + GlobalVariable.pConfig.Cord_Gap) * 1000);
                    result = true;
                    break;
                case enThickMeasureMethod.两线距离差:
                    ha.FitLine3D(objectModel1, out start_x1, out start_y1, out start_z1, out end_x1, out end_y1, out end_z1);
                    ha.FitLine3D(objectModel2, out start_x2, out start_y2, out start_z2, out end_x2, out end_y2, out end_z2);
                    listX.Add((start_x1 + end_x1) * 0.5);
                    listY.Add((start_y1 + end_y1) * 0.5);
                    listDist1.Add((start_z1 + end_z1) * 0.5 * 1000);
                    listDist2.Add((start_z2 + end_z2) * 0.5 * 1000);
                    listThick.Add((Math.Abs((start_z1 + end_z1) * 0.5 - (start_z2 + end_z2) * 0.5) + GlobalVariable.pConfig.Cord_Gap) * 1000);
                    result = true;
                    break;
                case enThickMeasureMethod.两平面距离和:
                    userWcsPlane wcsPlane1, wcsPlane2;
                    ha.GetPlaneObjectModel3DPose(objectModel1, out wcsPlane1);
                    ha.GetPlaneObjectModel3DPose(objectModel2, out wcsPlane2);
                    listX.Add(wcsPlane1.X);
                    listY.Add(wcsPlane1.Y);
                    listDist1.Add(wcsPlane1.Z * 1000);
                    listDist2.Add(wcsPlane2.Z * 1000);
                    listThick.Add((Math.Abs(wcsPlane1.Z + wcsPlane2.Z) + GlobalVariable.pConfig.Cord_Gap) * 1000);
                    result = true;
                    break;
                case enThickMeasureMethod.两平面距离差:
                    ha.GetPlaneObjectModel3DPose(objectModel1, out wcsPlane1);
                    ha.GetPlaneObjectModel3DPose(objectModel2, out wcsPlane2);
                    listX.Add(wcsPlane1.X);
                    listY.Add(wcsPlane1.Y);
                    listDist1.Add(wcsPlane1.Z * 1000);
                    listDist2.Add(wcsPlane2.Z * 1000);
                    listThick.Add((Math.Abs(wcsPlane1.Z - wcsPlane2.Z) + GlobalVariable.pConfig.Cord_Gap) * 1000);
                    result = true;
                    break;
                default:
                    break;
            }
            wcsThick = new userWcsThick(listX.Average(), listY.Average(), 0);
            wcsThick.Dist1 = listDist1.Average();
            wcsThick.Dist2 = listDist2.Average();
            wcsThick.Thick = listThick.Average();
            ///////////////////////////////////
            listX.Clear();
            listY.Clear();
            listDist1.Clear();
            listDist2.Clear();
            listThick.Clear();
            return result;
        }
        




    }



}
