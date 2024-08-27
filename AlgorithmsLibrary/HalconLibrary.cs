using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace AlgorithmsLibrary
{
    public class HalconLibrary
    {

        #region 2D算子库


        public void GenCoordSystem(userPixScaleVector pixPoint, double axisLength, double HeadLength, double HeadWidth, out HObject Arrow)
        {
            HTuple k1, k2, b1, b2, row_h, col_h, row_v, col_v;
            k1 = new HTuple(pixPoint.Rad).TupleTan() * -1;  // 因为图像坐标与世界坐标Y轴是反向的，所以这里斜率要取反
            k2 = new HTuple(pixPoint.Rad + Math.PI / 2.0).TupleTan() * -1;
            ///////
            b1 = pixPoint.Row - k1 * pixPoint.Col;
            b2 = pixPoint.Row - k2 * pixPoint.Col;
            row_h = k1 * (pixPoint.Col + axisLength) + b1; // 表示水平方向上的行
            col_h = pixPoint.Col + axisLength;
            ///////////////
            row_v = pixPoint.Row - axisLength; // 表示垂直方向上的行
            col_v = (row_v - b2) / k2;
            /////////////
            HTuple rows1 = new HTuple(pixPoint.Row, pixPoint.Row);
            HTuple cols1 = new HTuple(pixPoint.Col, pixPoint.Col);
            HTuple rows2 = new HTuple(row_h, row_v);
            HTuple cols2 = new HTuple(col_h, col_v);
            gen_arrow_contour_xld(out Arrow, rows1, cols1, rows2, cols2, HeadLength, HeadWidth);
        }
        public void GenCoordSystem(userPixVector pixPoint, double axisLength, double HeadLength, double HeadWidth, out HObject Arrow)
        {
            HTuple k1, k2, b1, b2, row_h, col_h, row_v, col_v;
            k1 = new HTuple(pixPoint.Rad).TupleTan() * -1;  // 因为图像坐标与世界坐标Y轴是反向的，所以这里斜率要取反
            k2 = new HTuple(pixPoint.Rad + Math.PI / 2.0).TupleTan() * -1;
            ///////
            b1 = pixPoint.Row - k1 * pixPoint.Col;
            b2 = pixPoint.Row - k2 * pixPoint.Col;
            row_h = k1 * (pixPoint.Col + axisLength) + b1; // 表示水平方向上的行
            col_h = pixPoint.Col + axisLength;
            ///////////////
            row_v = pixPoint.Row - axisLength; // 表示垂直方向上的行
            col_v = (row_v - b2) / k2;
            /////////////
            HTuple rows1 = new HTuple(pixPoint.Row, pixPoint.Row);
            HTuple cols1 = new HTuple(pixPoint.Col, pixPoint.Col);
            HTuple rows2 = new HTuple(row_h, row_v);
            HTuple cols2 = new HTuple(col_h, col_v);
            gen_arrow_contour_xld(out Arrow, rows1, cols1, rows2, cols2, HeadLength, HeadWidth);
        }
        public void GenPixCoordSystem(userWcsVector wcsVector, double axisLength, double HeadLength, double HeadWidth, out HObject Arrow)
        {
            userPixVector pixPoint = wcsVector.GetPixVector();
            HTuple k1, k2, b1, b2, row_h, col_h, row_v, col_v;
            k1 = new HTuple(pixPoint.Rad).TupleTan() * -1;  // 因为图像坐标与世界坐标Y轴是反向的，所以这里斜率要取反
            k2 = new HTuple(pixPoint.Rad + Math.PI / 2.0).TupleTan() * -1;
            ///////
            b1 = pixPoint.Row - k1 * pixPoint.Col;
            b2 = pixPoint.Row - k2 * pixPoint.Col;
            row_h = k1 * (pixPoint.Col + axisLength) + b1; // 表示水平方向上的行
            col_h = pixPoint.Col + axisLength;
            ///////////////
            row_v = pixPoint.Row - axisLength; // 表示垂直方向上的行
            col_v = (row_v - b2) / k2;
            /////////////
            HTuple rows1 = new HTuple(pixPoint.Row, pixPoint.Row);
            HTuple cols1 = new HTuple(pixPoint.Col, pixPoint.Col);
            HTuple rows2 = new HTuple(row_h, row_v);
            HTuple cols2 = new HTuple(col_h, col_v);
            gen_arrow_contour_xld(out Arrow, rows1, cols1, rows2, cols2, HeadLength, HeadWidth);
        }
        public void GenWcsCoordSystem(userWcsVector wcsVector, double axisLength, double HeadLength, double HeadWidth, out HObject Arrow)
        {
            HTuple k1, k2, b1, b2, row_h, col_h, row_v, col_v;
            k1 = new HTuple(wcsVector.Angle).TupleRad().TupleTan() * 1;  // 因为图像坐标与世界坐标Y轴是反向的，所以这里斜率要取反
            k2 = new HTuple(wcsVector.Angle + 90).TupleRad().TupleTan() * 1;
            ///////
            b1 = wcsVector.Y - k1 * wcsVector.X;
            b2 = wcsVector.Y - k2 * wcsVector.X;
            row_h = k1 * (wcsVector.X + axisLength) + b1; // 表示水平方向上的行
            col_h = wcsVector.X + axisLength;
            ///////////////
            row_v = wcsVector.Y + axisLength; // 表示垂直方向上的行
            col_v = (row_v - b2) / k2;
            /////////////
            HTuple rows1 = new HTuple(wcsVector.Y, wcsVector.Y);
            HTuple cols1 = new HTuple(wcsVector.X, wcsVector.X);
            HTuple rows2 = new HTuple(row_h, row_v);
            HTuple cols2 = new HTuple(col_h, col_v);
            gen_arrow_contour_xld(out Arrow, rows1 * -1, cols1, rows2 * -1, cols2, HeadLength, HeadWidth);
        }
        public void Gen3DCoordSystem(HTuple hv_WindowHandle, HTuple hv_CamParam, HTuple hv_Pose, HTuple hv_CoordAxesLength)
        {
            disp_3d_coord_system(hv_WindowHandle, hv_CamParam, hv_Pose, hv_CoordAxesLength);
        }
        private void disp_3d_coord_system(HTuple hv_WindowHandle, HTuple hv_CamParam, HTuple hv_Pose, HTuple hv_CoordAxesLength)
        {
            // Local iconic variables 
            HObject ho_Arrows;
            // Local control variables 
            HTuple hv_TransWorld2Cam = null, hv_OrigCamX = null;
            HTuple hv_OrigCamY = null, hv_OrigCamZ = null, hv_Row0 = null;
            HTuple hv_Column0 = null, hv_X = null, hv_Y = null, hv_Z = null;
            HTuple hv_RowAxX = null, hv_ColumnAxX = null, hv_RowAxY = null;
            HTuple hv_ColumnAxY = null, hv_RowAxZ = null, hv_ColumnAxZ = null;
            HTuple hv_Distance = null, hv_HeadLength = null, hv_Red = null;
            HTuple hv_Green = null, hv_Blue = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Arrows);
            if ((int)(new HTuple((new HTuple(hv_Pose.TupleLength())).TupleNotEqual(7))) != 0)
            {
                ho_Arrows.Dispose();

                return;
            }
            if ((int)((new HTuple(((hv_Pose.TupleSelect(2))).TupleEqual(0.0))).TupleAnd(new HTuple(((hv_CamParam.TupleSelect(
                0))).TupleNotEqual(0)))) != 0)
            {
                ho_Arrows.Dispose();
                return;
            }
            //Convert to pose to a transformation matrix
            HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_TransWorld2Cam);
            //Project the world origin into the image
            HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, 0, out hv_OrigCamX,
                out hv_OrigCamY, out hv_OrigCamZ);
            HOperatorSet.Project3dPoint(hv_OrigCamX, hv_OrigCamY, hv_OrigCamZ, hv_CamParam,
                out hv_Row0, out hv_Column0);
            //Project the coordinate axes into the image
            HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, hv_CoordAxesLength, 0, 0,
                out hv_X, out hv_Y, out hv_Z);
            HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxX, out hv_ColumnAxX);
            HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, hv_CoordAxesLength, 0,
                out hv_X, out hv_Y, out hv_Z);
            HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxY, out hv_ColumnAxY);
            HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, hv_CoordAxesLength,
                out hv_X, out hv_Y, out hv_Z);
            HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxZ, out hv_ColumnAxZ);
            //
            //Generate an XLD contour for each axis
            HOperatorSet.DistancePp(((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(hv_Row0),
                ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0), ((hv_RowAxX.TupleConcat(
                hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(hv_ColumnAxY))).TupleConcat(
                hv_ColumnAxZ), out hv_Distance);
            hv_HeadLength = (((((((hv_Distance.TupleMax()) / 12.0)).TupleConcat(5.0))).TupleMax()
                )).TupleInt();
            ho_Arrows.Dispose();
            gen_arrow_contour_xld(out ho_Arrows, ((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(
                hv_Row0), ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0),
                ((hv_RowAxX.TupleConcat(hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(
                hv_ColumnAxY))).TupleConcat(hv_ColumnAxZ), hv_HeadLength, hv_HeadLength);
            //
            //Display coordinate system
            HOperatorSet.DispXld(ho_Arrows, hv_WindowHandle);
            //
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red.TupleSelect(0), hv_Green.TupleSelect(
                0), hv_Blue.TupleSelect(0));
            HOperatorSet.SetTposition(hv_WindowHandle, hv_RowAxX + 3, hv_ColumnAxX + 3);
            HOperatorSet.WriteString(hv_WindowHandle, "X");
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red.TupleSelect(1 % (new HTuple(hv_Red.TupleLength()
                ))), hv_Green.TupleSelect(1 % (new HTuple(hv_Green.TupleLength()))), hv_Blue.TupleSelect(
                1 % (new HTuple(hv_Blue.TupleLength()))));
            HOperatorSet.SetTposition(hv_WindowHandle, hv_RowAxY + 3, hv_ColumnAxY + 3);
            HOperatorSet.WriteString(hv_WindowHandle, "Y");
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red.TupleSelect(2 % (new HTuple(hv_Red.TupleLength()
                ))), hv_Green.TupleSelect(2 % (new HTuple(hv_Green.TupleLength()))), hv_Blue.TupleSelect(
                2 % (new HTuple(hv_Blue.TupleLength()))));
            HOperatorSet.SetTposition(hv_WindowHandle, hv_RowAxZ + 3, hv_ColumnAxZ + 3);
            HOperatorSet.WriteString(hv_WindowHandle, "Z");
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            ho_Arrows.Dispose();

            return;
        }
        private void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            HObject ho_TempArrow = null;

            HTuple hv_Length = null, hv_ZeroLengthIndices = null;
            HTuple hv_DR = null, hv_DC = null, hv_HalfHeadWidth = null;
            HTuple hv_RowP1 = null, hv_ColP1 = null, hv_RowP2 = null;
            HTuple hv_ColP2 = null, hv_Index = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);
            //
            ho_Arrow.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            //
            //Calculate the arrow length
            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
            //
            //Mark arrows with identical start and end point
            //(set Length to -1 to avoid division-by-zero exception)
            hv_ZeroLengthIndices = hv_Length.TupleFind(0);
            if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
            {
                if (hv_Length == null)
                    hv_Length = new HTuple();
                hv_Length[hv_ZeroLengthIndices] = -1;
            }
            //
            //Calculate auxiliary variables.
            hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
            hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
            hv_HalfHeadWidth = hv_HeadWidth / 2.0;
            //
            //Calculate end points of the arrow head.
            hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
            hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
            hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
            hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
            //
            //Finally create output XLD contour for each input point pair
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                {
                    //Create_ single points for arrows with identical start and end point
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(hv_Index),
                        hv_Column1.TupleSelect(hv_Index));
                }
                else
                {
                    //Create arrow contour
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                        hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                        ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                        hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out ExpTmpOutVar_0);
                    ho_Arrow.Dispose();
                    ho_Arrow = ExpTmpOutVar_0;
                }
            }
            ho_TempArrow.Dispose();

            return;
        }

        public void DrawRectangle2OnWindow(HTuple window, out userPixRectangle2 rect2)
        {
            HObject rect2Region;
            HTuple row, col, phi, length1, length2;
            HOperatorSet.SetColor(window, "red");
            HOperatorSet.DrawRectangle2(window, out row, out col, out phi, out length1, out length2);
            HOperatorSet.SetDraw(window, "margin");
            HOperatorSet.GenRectangle2(out rect2Region, row, col, phi, length1, length2);
            HOperatorSet.DispObj(rect2Region, window);
            rect2 = new userPixRectangle2(row, col, phi, length1, length2);
        }

        public bool ReadHXLDCon(out HXLDCont hObject, string path)
        {
            HObject Object = null;
            hObject = null;
            HTuple state;
            if (path.Trim().Length == 0) return false;
            HOperatorSet.ReadContourXldDxf(out Object, path, "max_approx_error", 0.001, out state);
            hObject = new HXLDCont(Object);
            return true;
        }



        #endregion

        #region 3D算子库
        public bool RenderObjectModel3DTo3ImageModify(HObjectModel3D[] objectModel, double resolution_x, double resolution_y, out ImageDataClass image1, out ImageDataClass image2, out ImageDataClass image3)
        {
            image1 = null;
            image2 = null;
            image3 = null;
            bool result = false;
            HImage image, image_1, image_2, image_3;
            HObjectModel3D unionObjectModel = null;
            HTuple camPara, camPose;
            ///////////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel.Length == 0)
            {
                throw new ArgumentException("objectModel：对象中不包含元素");
            }
            unionObjectModel = HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface");
            InitcamParam(new HObjectModel3D[] { unionObjectModel }, resolution_x, resolution_y, out camPara);
            InitCamPose(new HObjectModel3D[] { unionObjectModel }, camPara, out camPose);
            //////////////////////
            image = unionObjectModel.RenderObjectModel3d(new HCamPar(camPara), new HPose(camPose), new HTuple("lut", "intensity"), new HTuple("rainbow", "coord_z"));
            image_1 = image.Decompose3(out image_2, out image_3);
            image1 = new ImageDataClass(new HImage(image_1), new CameraParam(camPara, camPose), 0, 0, 0, 0);
            image2 = new ImageDataClass(new HImage(image_2), new CameraParam(camPara, camPose), 0, 0, 0, 0);
            image3 = new ImageDataClass(new HImage(image_3), new CameraParam(camPara, camPose), 0, 0, 0, 0);
            if (unionObjectModel != null)
                unionObjectModel.Dispose();
            result = true;
            return result;
        }

        public bool TransformObject3DToRealImage(HObjectModel3D objectModel, double resolution_x, double resolution_y, out ImageDataClass imageData)
        {
            bool result = false;
            HTuple coord_x, coord_y, coord_z;
            HTuple imageHeigth = 0;
            HalconLibrary ha = new HalconLibrary();
            /////////////////////////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel.GetObjectModel3dParams("num_points").I == 0)
            {
                throw new ArgumentException("objectModel:对象中不包含点");
            }
            //////////////////////
            coord_x = objectModel.GetObjectModel3dParams("point_coord_x");
            coord_y = objectModel.GetObjectModel3dParams("point_coord_y");
            coord_z = objectModel.GetObjectModel3dParams("point_coord_z");
            HTuple max_x = coord_x.TupleMax();
            HTuple min_x = coord_x.TupleMin();
            HTuple max_y = coord_y.TupleMax();
            HTuple min_y = coord_y.TupleMin();
            /////////////////////
            HTuple camPara = new HTuple(0.0, 0.0, resolution_x, resolution_y, 800.0, 800.0, 1600, 1600);
            HTuple camPose = new HTuple(0, 0, 500, 180, 0, 0, 0);
            camPara[4] = (max_x - min_x) / camPara[2].D * 0.5;
            camPara[5] = (max_y - min_y) / camPara[3].D * 0.5;
            camPara[6] = (int)(camPara[4].D * 2 + 1);
            camPara[7] = (int)(camPara[5].D * 2 + 1);
            //////////////////////////////
            HTuple X, Y, PoseNewOrigin, HomMat3D, zz, Qx, Qy;
            HOperatorSet.ImagePointsToWorldPlane(new HCamPar(camPara), new HPose(camPose), 0, 0, "m", out X, out Y);
            //HMisc.ImagePointsToWorldPlane();
            HOperatorSet.SetOriginPose(camPose, X - min_x, Y - max_y, 0, out PoseNewOrigin);
            HOperatorSet.CamParPoseToHomMat3d(camPara, PoseNewOrigin, out HomMat3D);
            HOperatorSet.TupleGenConst(coord_x.Length, 0, out zz);
            HOperatorSet.ProjectPointHomMat3d(HomMat3D, coord_x, coord_y, zz, out Qx, out Qy);
            //////////////////////
            HImage image = new HImage("real", camPara[6], camPara[7]);
            if (Qy.Length != coord_z.Length)
            {
                throw new ArgumentException("XYZ:数组长度不相等");
            }
            image.SetGrayval(Qy, Qx, coord_z.TupleAbs());
            imageData = new ImageDataClass(image, new CameraParam(camPara, camPose));
            result = true;
            return result;
        }
        public bool TransformObject3DToRealImageModify(HObjectModel3D objectModel, double resolution_x, double resolution_y, out ImageDataClass imageData)
        {
            bool result = false;
            HTuple coord_x, coord_y, coord_z, camPara, camPose;
            HTuple imageHeigth = 0;
            HalconLibrary ha = new HalconLibrary();
            /////////////////////////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel.GetObjectModel3dParams("num_points").I == 0)
            {
                throw new ArgumentException("objectModel:对象中不包含点");
            }
            //////////////////////
            InitcamParam(new HObjectModel3D[] { objectModel }, resolution_x, resolution_y, out camPara);
            InitCamPose(new HObjectModel3D[] { objectModel }, camPara, out camPose);
            coord_x = objectModel.GetObjectModel3dParams("point_coord_x");
            coord_y = objectModel.GetObjectModel3dParams("point_coord_y");
            coord_z = objectModel.GetObjectModel3dParams("point_coord_z");
            //////////////////////////////
            HTuple X, Y, HomMat3D, zz, Qx, Qy;
            HOperatorSet.CamParPoseToHomMat3d(camPara, camPose, out HomMat3D);
            HOperatorSet.TupleGenConst(coord_x.Length, 0, out zz);
            HOperatorSet.ProjectPointHomMat3d(HomMat3D, coord_x, coord_y, coord_z, out Qx, out Qy);
            //////////////////////
            HImage image = new HImage("real", Convert.ToInt32(camPara[6].D), Convert.ToInt32(camPara[7].D));
            if (Qy.Length != coord_z.Length)
            {
                throw new ArgumentException("XYZ:数组长度不相等");
            }
            image.SetGrayval(Qy, Qx, coord_z.TupleAbs());
            imageData = new ImageDataClass(image, new CameraParam(camPara, camPose));
            result = true;
            return result;
        }

        #region 计算相机内外参
        private void InitcamParam(HObjectModel3D[] objectModel, double resolution_x, double resolution_y, out HTuple camPara)
        {
            HTuple ParamValue;
            double maxDiameter, x_range, y_range;
            int imageWidth = 1000, imageHeight = 800;
            //////////////////////////////////////////////
            camPara = new HTuple(0.0, 0.0, resolution_x, resolution_y, imageWidth * 0.5, imageHeight * 0.5, imageWidth, imageHeight);
            /////////////////////////////// 根据对象的大小来调整像元大小
            int length = objectModel.Length;
            List<double> x_value = new List<double>();
            List<double> y_value = new List<double>();
            HTuple num = objectModel[0].GetObjectModel3dParams("num_points");
            if (num.I == 0) return;
            ParamValue = HObjectModel3D.GetObjectModel3dParams(objectModel, "bounding_box1"); // 如果是多个对象，获取的是每个对象的值
            if (ParamValue == null || ParamValue.Length == 0 || ParamValue.Length != length * 6) return;
            for (int i = 0; i < length; i++)
            {
                x_value.Add(ParamValue[i * 6].D);
                x_value.Add(ParamValue[i * 6 + 3].D);
                y_value.Add(ParamValue[i * 6 + 1].D);
                y_value.Add(ParamValue[i * 6 + 4].D);
            }
            x_range = (x_value.Max() - x_value.Min()) * 1.2;
            y_range = (y_value.Max() - y_value.Min()) * 1.2;
            maxDiameter = Math.Max(x_range, y_range);
            if (maxDiameter < 1)
                maxDiameter = 1;
            imageWidth = (int)(x_range / resolution_x);
            imageHeight = (int)(y_range / resolution_x);
            camPara = new HTuple(0.0, 0.0, resolution_x, resolution_y, imageWidth * 0.5, imageHeight * 0.5, imageWidth, imageHeight);
        }
        private void InitCamPose(HObjectModel3D[] objectModel, HTuple camParam, out HTuple camPose)
        {
            //////////////////////////////////////////////
            camPose = new HTuple();
            HTuple center, PoseEstimated, X, Y, Z;
            if (objectModel == null || objectModel.Length == 0) return;
            HTuple num = objectModel[0].GetObjectModel3dParams("num_points");
            if (num.I == 0) return;
            GetObjectModelsCenter(objectModel, out center);
            X = -center.TupleSelect(0);
            Y = -center.TupleSelect(1);
            Z = -center.TupleSelect(2);
            HOperatorSet.CreatePose(X, Y, Z, 0, 0, 0, "Rp+T", "gba", "point", out camPose);
            // 调整到一个合适的位姿，主要用于计算Z方向距离
            DetermineOptimumPoseDistance(objectModel, camParam, 0.9, camPose, out PoseEstimated);
            // 将位姿绕当前坐标系旋转180度，这样就和世界坐标系一致了
            HTuple homMat3D, phi, homMat3DRotate;
            HOperatorSet.PoseToHomMat3d(PoseEstimated, out homMat3D);
            HOperatorSet.TupleRad(180, out phi);
            HOperatorSet.HomMat3dRotate(homMat3D, phi, "x", 0, 0, 0, out homMat3DRotate);   // 如果绕固定坐标系旋转，那么中心Y需要取反
            // HOperatorSet.HomMat3dRotateLocal(homMat3D, phi, "x", out homMat3DRotate);   // 如果绕当前坐标系旋转，那么中心Y不需要取反
            HOperatorSet.HomMat3dToPose(homMat3DRotate, out PoseEstimated);
            // 给原始位姿与当前位姿赋值
            camPose = PoseEstimated;
            if (camPose[2].D < 1000)
                camPose[2] = 1000;
            else
                camPose[2] = Math.Abs(camPose[2].D) * 5;
        }
        private void DetermineOptimumPoseDistance(HObjectModel3D[] ObjectModel3DID, HTuple CamParam, HTuple ImageCoverage, HTuple PoseIn, out HTuple PoseOut)
        {
            HTuple hv_NumModels = null, hv_Rows = null;
            HTuple hv_Cols = null, hv_MinMinZ = null, hv_BB = null;
            HTuple hv_Seq = null, hv_DXMax = null, hv_DYMax = null;
            HTuple hv_DZMax = null, hv_Diameter = null, hv_ZAdd = null;
            HTuple hv_IBB = null, hv_BB0 = null, hv_BB1 = null, hv_BB2 = null;
            HTuple hv_BB3 = null, hv_BB4 = null, hv_BB5 = null, hv_X = null;
            HTuple hv_Y = null, hv_Z = null, hv_PoseInter = null, hv_HomMat3D = null;
            HTuple hv_CX = null, hv_CY = null, hv_CZ = null, hv_DR = null;
            HTuple hv_DC = null, hv_MaxDist = null, hv_HomMat3DRotate = new HTuple();
            HTuple hv_MinImageSize = null, hv_Zs = null, hv_ZDiff = null;
            HTuple hv_ScaleZ = null, hv_ZNew = null;
            //
            hv_NumModels = new HTuple(ObjectModel3DID.Length);
            hv_Rows = new HTuple();
            hv_Cols = new HTuple();
            hv_MinMinZ = 1e30;
            hv_BB = HObjectModel3D.GetObjectModel3dParams(ObjectModel3DID, "bounding_box1");
            //Calculate diameter over all objects to be visualized
            hv_Seq = HTuple.TupleGenSequence(0, (new HTuple(hv_BB.TupleLength())) - 1, 6);
            hv_DXMax = (((hv_BB.TupleSelect(hv_Seq + 3))).TupleMax()) - (((hv_BB.TupleSelect(hv_Seq))).TupleMin());
            hv_DYMax = (((hv_BB.TupleSelect(hv_Seq + 4))).TupleMax()) - (((hv_BB.TupleSelect(hv_Seq + 1))).TupleMin());
            hv_DZMax = (((hv_BB.TupleSelect(hv_Seq + 5))).TupleMax()) - (((hv_BB.TupleSelect(hv_Seq + 2))).TupleMin());
            hv_Diameter = ((((hv_DXMax * hv_DXMax) + (hv_DYMax * hv_DYMax)) + (hv_DZMax * hv_DZMax))).TupleSqrt();
            if ((int)(new HTuple(((((hv_BB.TupleAbs())).TupleSum())).TupleEqual(0.0))) != 0)
            {
                hv_BB = new HTuple();
                hv_BB = hv_BB.TupleConcat(-((new HTuple(HTuple.TupleRand(3) * 1e-20)).TupleAbs()));
                hv_BB = hv_BB.TupleConcat((new HTuple(HTuple.TupleRand(3) * 1e-20)).TupleAbs());
            }
            hv_ZAdd = 0.0;
            if ((int)(new HTuple(((hv_Diameter.TupleMax())).TupleLess(1e-10))) != 0)
            {
                hv_ZAdd = 0.01;
            }
            if ((int)(new HTuple(((hv_Diameter.TupleMin())).TupleLess(1e-10))) != 0)
            {
                hv_Diameter = hv_Diameter - (((((((hv_Diameter - 1e-10)).TupleSgn()) - 1)).TupleSgn()) * 1e-10);
            }
            hv_IBB = HTuple.TupleGenSequence(0, (new HTuple(hv_BB.TupleLength())) - 1, 6);
            hv_BB0 = hv_BB.TupleSelect(hv_IBB);
            hv_BB1 = hv_BB.TupleSelect(hv_IBB + 1);
            hv_BB2 = hv_BB.TupleSelect(hv_IBB + 2);
            hv_BB3 = hv_BB.TupleSelect(hv_IBB + 3);
            hv_BB4 = hv_BB.TupleSelect(hv_IBB + 4);
            hv_BB5 = hv_BB.TupleSelect(hv_IBB + 5);
            hv_X = new HTuple();
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_Y = new HTuple();
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Z = new HTuple();
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_PoseInter = PoseIn.TupleReplace(2, (-(hv_Z.TupleMin())) + (2 * (hv_Diameter.TupleMax())));
            HOperatorSet.PoseToHomMat3d(hv_PoseInter, out hv_HomMat3D);
            //Determine the maximum extention of the projection
            HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_X, hv_Y, hv_Z, out hv_CX, out hv_CY, out hv_CZ);
            HOperatorSet.Project3dPoint(hv_CX, hv_CY, hv_CZ, CamParam, out hv_Rows, out hv_Cols);
            hv_MinMinZ = hv_CZ.TupleMin();
            hv_DR = hv_Rows - (CamParam.TupleSelect((new HTuple(CamParam.TupleLength())) - 3));
            hv_DC = hv_Cols - (CamParam.TupleSelect((new HTuple(CamParam.TupleLength())) - 4));
            hv_DR = (hv_DR.TupleMax()) - (hv_DR.TupleMin());
            hv_DC = (hv_DC.TupleMax()) - (hv_DC.TupleMin());
            hv_MaxDist = (((hv_DR * hv_DR) + (hv_DC * hv_DC))).TupleSqrt();
            //
            if ((int)(new HTuple(hv_MaxDist.TupleLess(1e-10))) != 0)
            {
                HOperatorSet.HomMat3dRotateLocal(hv_HomMat3D, (new HTuple(90)).TupleRad(), "x", out hv_HomMat3DRotate);
                HOperatorSet.AffineTransPoint3d(hv_HomMat3DRotate, hv_X, hv_Y, hv_Z, out hv_CX, out hv_CY, out hv_CZ);
                HOperatorSet.Project3dPoint(hv_CX, hv_CY, hv_CZ, CamParam, out hv_Rows, out hv_Cols);
                hv_DR = hv_Rows - (CamParam.TupleSelect((new HTuple(CamParam.TupleLength()
                    )) - 3));
                hv_DC = hv_Cols - (CamParam.TupleSelect((new HTuple(CamParam.TupleLength()
                    )) - 4));
                hv_DR = (hv_DR.TupleMax()) - (hv_DR.TupleMin());
                hv_DC = (hv_DC.TupleMax()) - (hv_DC.TupleMin());
                hv_MaxDist = ((hv_MaxDist.TupleConcat((((hv_DR * hv_DR) + (hv_DC * hv_DC))).TupleSqrt()
                    ))).TupleMax();
            }
            //
            hv_MinImageSize = ((((CamParam.TupleSelect((new HTuple(CamParam.TupleLength())) - 2))).TupleConcat(CamParam.TupleSelect((new HTuple(CamParam.TupleLength())) - 1)))).TupleMin();
            //
            hv_Z = hv_PoseInter[2];
            hv_Zs = hv_MinMinZ.Clone();
            hv_ZDiff = hv_Z - hv_Zs;
            hv_ScaleZ = hv_MaxDist / (((0.5 * hv_MinImageSize) * ImageCoverage) * 2.0);
            hv_ZNew = ((hv_ScaleZ * hv_Zs) + hv_ZDiff) + hv_ZAdd;
            PoseOut = hv_PoseInter.TupleReplace(2, hv_ZNew);
        }
        private void GetObjectModelsCenter(HObjectModel3D[] hv_ObjectModel3DID, out HTuple hv_Center)
        {
            HTuple hv_Diameter = new HTuple(), hv_MD = new HTuple();
            HTuple hv_Weight = new HTuple(), hv_SumW = new HTuple();
            HTuple hv_Index = new HTuple(), hv_ObjectModel3DIDSelected = new HTuple();
            HTuple hv_C = new HTuple(), hv_InvSum = new HTuple();
            // Initialize local and output iconic variables 
            hv_Center = new HTuple();
            HObjectModel3D unionObjectModel3D = null;
            // Compute the mean of all model centers (weighted by the diameter of the object models)
            if (hv_ObjectModel3DID != null && hv_ObjectModel3DID.Length > 0)
            {
                unionObjectModel3D = HObjectModel3D.UnionObjectModel3d(hv_ObjectModel3DID, "points_surface"); // 这里是否需要合并
                hv_Diameter = unionObjectModel3D.GetObjectModel3dParams("diameter_axis_aligned_bounding_box");
                // Normalize Diameter to use it as weights for a weighted mean of the individual centers
                hv_MD = hv_Diameter.TupleMean();
                if ((int)(new HTuple(hv_MD.TupleGreater(1e-10))) != 0)
                {
                    hv_Weight = hv_Diameter / hv_MD;
                }
                else
                {
                    hv_Weight = hv_Diameter.Clone();
                }
                hv_SumW = hv_Weight.TupleSum();
                if ((int)(new HTuple(hv_SumW.TupleLess(1e-10))) != 0)
                {
                    hv_Weight = HTuple.TupleGenConst(new HTuple(hv_Weight.TupleLength()), 1.0);
                    hv_SumW = hv_Weight.TupleSum();
                }
                hv_Center = new HTuple();
                hv_Center[0] = 0;
                hv_Center[1] = 0;
                hv_Center[2] = 0;
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ObjectModel3DID.Length)) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_ObjectModel3DIDSelected = hv_ObjectModel3DID[hv_Index];
                    HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DIDSelected, "center", out hv_C);
                    if (hv_Center == null)
                        hv_Center = new HTuple();
                    hv_Center[0] = (hv_Center.TupleSelect(0)) + ((hv_C.TupleSelect(0)) * (hv_Weight.TupleSelect(hv_Index)));
                    if (hv_Center == null)
                        hv_Center = new HTuple();
                    hv_Center[1] = (hv_Center.TupleSelect(1)) + ((hv_C.TupleSelect(1)) * (hv_Weight.TupleSelect(hv_Index)));
                    if (hv_Center == null)
                        hv_Center = new HTuple();
                    hv_Center[2] = (hv_Center.TupleSelect(2)) + ((hv_C.TupleSelect(2)) * (hv_Weight.TupleSelect(hv_Index)));
                }
                hv_InvSum = 1.0 / hv_SumW;
                if (hv_Center == null)
                    hv_Center = new HTuple();
                hv_Center[0] = (hv_Center.TupleSelect(0)) * hv_InvSum;
                if (hv_Center == null)
                    hv_Center = new HTuple();
                hv_Center[1] = (hv_Center.TupleSelect(1)) * hv_InvSum;
                if (hv_Center == null)
                    hv_Center = new HTuple();
                hv_Center[2] = (hv_Center.TupleSelect(2)) * hv_InvSum;
            }
            else
            {
                hv_Center = new HTuple();
            }
            return;
        }
        #endregion
        public void TransformModel3DTo2DPoint(HObjectModel3D lineObjcetModel, int DataDimension, out double[] X, out double[] Y)
        {
            if (lineObjcetModel == null)
            {
                throw new ArgumentNullException("lineObjcetModel"); //num_points
            }
            int count = lineObjcetModel.GetObjectModel3dParams("num_points");
            if (count == 0)
            {
                throw new ArgumentException("对象中不包含数据点", "lineObjcetModel");
            }
            HTuple line_x, line_y, line_z;
            double[] x, y;
            line_z = lineObjcetModel.GetObjectModel3dParams("point_coord_z");
            // 数据维度分为1维、2维、3维（2维：XY平面;3维：XYZ平面）
            switch (DataDimension)
            {
                case 2:
                    line_x = lineObjcetModel.GetObjectModel3dParams("point_coord_x");
                    line_y = lineObjcetModel.GetObjectModel3dParams("point_coord_y");
                    X = line_x.DArr;
                    Y = line_y.DArr;
                    break;
                default:
                case 3:
                    line_x = lineObjcetModel.GetObjectModel3dParams("point_coord_x"); //要把三维的图形转换成二维图形
                    line_y = lineObjcetModel.GetObjectModel3dParams("point_coord_y");
                    line_z = lineObjcetModel.GetObjectModel3dParams("point_coord_z");
                    x = new double[line_x.Length];
                    y = new double[line_x.Length];
                    HTuple min_x = line_x.TupleMin();
                    HTuple min_y = line_y.TupleMin();
                    for (int j = 0; j < line_x.Length; j++)
                    {
                        x[j] = HMisc.DistancePp(line_y[j].D, line_x[j].D, min_y.D, min_x.D);//将二维的XY坐标转换为一维的X坐标
                        y[j] = line_z[j].D;
                    }
                    X = x;
                    Y = y;
                    break;
            }
        }

        public bool AngleLineLine(HObjectModel3D[] line1_objectModel, HObjectModel3D[] line2_objectModel, out double[] deg)
        {
            bool result = false;
            deg = new double[0];
            if (line1_objectModel == null)
            {
                throw new ArgumentNullException("line1_objectModel");
            }
            if (line2_objectModel == null)
            {
                throw new ArgumentNullException("line2_objcetModel");
            }
            if (line1_objectModel.Length != line2_objectModel.Length)
            {
                throw new ArgumentException("两参数的长度不一致");
            }
            ///////////////////////////
            double line1_x, line1_y, line1_z, line1_x2, line1_y2, line1_z2, line2_x, line2_y, line2_z, line2_x2, line2_y2, line2_z2;
            //////////////////////
            deg = new double[line1_objectModel.Length];
            for (int i = 0; i < line1_objectModel.Length; i++)
            {
                int line1Num = line1_objectModel[i].GetObjectModel3dParams("num_points").I;
                int line2Num = line2_objectModel[i].GetObjectModel3dParams("num_points").I;
                if (line1Num < 2 || line2Num < 2)
                {
                    deg[i] = -1000;
                }
                else
                {
                    FitLine3D(line1_objectModel[i], out line1_x, out line1_y, out line1_z, out line1_x2, out line1_y2, out line1_z2);
                    FitLine3D(line2_objectModel[i], out line2_x, out line2_y, out line2_z, out line2_x2, out line2_y2, out line2_z2);
                    /////////////////////////////////////////////////////////////////
                    double angle = HMisc.AngleLl(line1_x, line1_z, line1_x2, line2_z2, line2_x, line2_y, line2_x2, line2_y2);
                    if (Math.Abs(180 / Math.PI * angle) > 90)
                        deg[i] = 180 - Math.Abs(180 / Math.PI * angle);
                    else
                        deg[i] = Math.Abs(180 / Math.PI * angle);
                }
            }
            result = true;
            return result;
        }

        public void FitLine3D(HObjectModel3D lineObjcetModel, int DataDimension, out double RowBegin, out double ColBegin, out double RowEnd, out double ColEnd)
        {
            if (lineObjcetModel == null)
            {
                throw new ArgumentNullException("lineObjcetModel"); //num_points
            }
            int count = lineObjcetModel.GetObjectModel3dParams("num_points");
            if (count < 2)
            {
                throw new ArgumentException("对象中包含的数据点小于 2 ", "lineObjcetModel");
            }
            HTuple line_x, line_y, line_z;
            double[] x, y;
            line_z = lineObjcetModel.GetObjectModel3dParams("point_coord_z");
            // 数据维度分为1维、2维、3维（2维：XY平面;3维：XYZ平面）
            switch (DataDimension)
            {
                case 2:
                    line_x = lineObjcetModel.GetObjectModel3dParams("point_coord_x");
                    line_y = lineObjcetModel.GetObjectModel3dParams("point_coord_y");
                    break;
                default:
                case 3:
                    line_x = lineObjcetModel.GetObjectModel3dParams("point_coord_x"); //要把三维的图形转换成二维图形
                    line_y = lineObjcetModel.GetObjectModel3dParams("point_coord_y");
                    line_z = lineObjcetModel.GetObjectModel3dParams("point_coord_z");
                    x = new double[line_x.Length];
                    y = new double[line_x.Length];
                    HTuple min_x = line_x.TupleMin();
                    HTuple min_y = line_y.TupleMin();
                    for (int j = 0; j < line_x.Length; j++)
                    {
                        x[j] = HMisc.DistancePp(line_y[j].D, line_x[j].D, min_y.D, min_x.D);//将二维的XY坐标转换为一维的X坐标
                        y[j] = line_z[j].D;
                    }
                    line_x = new HTuple(x);
                    line_y = new HTuple(y);
                    break;
            }
            // 拟合直线
            double Nr, Nc, Dist;
            new HXLDCont(line_y, line_x).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
        }
        public void FitLine3D(double[] X, double[] Y, double[] Z, int DataDimension, out double RowBegin, out double ColBegin, out double RowEnd, out double ColEnd)
        {
            if (X == null)
            {
                throw new ArgumentNullException("X"); //num_points
            }
            if (Y == null)
            {
                throw new ArgumentNullException("Y"); //num_points
            }
            if (Z == null)
            {
                throw new ArgumentNullException("Z"); //num_points
            }
            if (X.Length != Y.Length || X.Length != Z.Length)
            {
                throw new ArgumentNullException("长度不相等!"); //num_points
            }
            int count = X.Length;
            if (count < 2)
            {
                throw new ArgumentException("对象中包含的数据点小于 2 ", "lineObjcetModel");
            }
            HTuple line_x, line_y, line_z;
            double[] x, y;
            line_z = Z;
            // 数据维度分为1维、2维、3维（2维：XY平面;3维：XYZ平面）
            switch (DataDimension)
            {
                case 2:
                    line_x = X;
                    line_y = Y;
                    break;
                default:
                case 3:
                    line_x = X; //要把三维的图形转换成二维图形
                    line_y = Y;
                    line_z = Z;
                    x = new double[line_x.Length];
                    y = new double[line_x.Length];
                    HTuple min_x = line_x.TupleMin();
                    HTuple min_y = line_y.TupleMin();
                    for (int j = 0; j < line_x.Length; j++)
                    {
                        x[j] = HMisc.DistancePp(line_y[j].D, line_x[j].D, min_y.D, min_x.D);//将二维的XY坐标转换为一维的X坐标
                        y[j] = line_z[j].D;
                    }
                    line_x = new HTuple(x);
                    line_y = new HTuple(y);
                    break;
            }
            // 拟合直线
            double Nr, Nc, Dist;
            new HXLDCont(line_y, line_x).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
        }
        public void FitLine3D(HObjectModel3D lineObjcetModel, out double start_x, out double start_y, out double start_z, out double end_x, out double end_y, out double end_z)
        {
            if (lineObjcetModel == null)
            {
                throw new ArgumentNullException("lineObjcetModel"); //num_points
            }
            int count = lineObjcetModel.GetObjectModel3dParams("num_points");
            if (count < 2)
            {
                throw new ArgumentException("对象中包含的数据点小于 2", "lineObjcetModel");
            }
            HalconLibrary ha = new HalconLibrary();
            HTuple coord_x, coord_y, coord_z, sort_x, sort_y;
            double[] x, y;
            coord_x = lineObjcetModel.GetObjectModel3dParams("point_coord_x");
            coord_y = lineObjcetModel.GetObjectModel3dParams("point_coord_y");
            coord_z = lineObjcetModel.GetObjectModel3dParams("point_coord_z");
            x = new double[coord_x.Length];
            y = new double[coord_x.Length];
            // 拟合XY直线
            double Nr, Nc, Dist, x1, y1, z1, x2, y2, z2, Pro_x, Pro_y;
            //if ((coord_x.TupleMax() - coord_x.TupleMin()) > (coord_y.TupleMax() - coord_y.TupleMin()))
            //    ha.sortPairs(coord_x, coord_y, 1, out sort_x, out sort_y);
            //else
            //    ha.sortPairs(coord_x, coord_y, 2, out sort_x, out sort_y);
            //new HXLDCont(sort_x, sort_y).FitLineContourXld("tukey", -1, 0, 5, 2, out x1, out y1, out x2, out y2, out Nr, out Nc, out Dist);
            new HXLDCont(coord_x, coord_y).FitLineContourXld("tukey", -1, 0, 5, 2, out x1, out y1, out x2, out y2, out Nr, out Nc, out Dist); // 坐标系要符合笛卡尔坐标系
            start_x = x1;
            start_y = y1;
            end_x = x2;
            end_y = y2;
            // 将离散的XY点转换到拟合的直线上，通过点到直线的投影
            for (int i = 0; i < coord_x.Length; i++)
            {
                HMisc.ProjectionPl(coord_x[i].D, coord_y[i].D, x1, y1, x2, y2, out Pro_x, out Pro_y);
                x[i] = Pro_x;
                y[i] = Pro_y;
            }
            // 将三维轮廓转换为二维轮廓
            double[] single_x = new double[x.Length];
            double[] single_z = new double[x.Length];
            double min_x = x.Min();
            double min_y = y.Min();
            for (int j = 0; j < x.Length; j++)
            {
                single_x[j] = HMisc.DistancePp(x[j], y[j], x[0], y[0]); //将二维的XY坐标转换为一维的X坐标
                single_z[j] = coord_z[j].D;
            }
            // 拟合直线
            new HXLDCont(single_x, single_z).FitLineContourXld("tukey", -1, 0, 5, 2, out x1, out z1, out x2, out z2, out Nr, out Nc, out Dist);
            start_z = z1;
            end_z = z2;
        }

        public void FitLine3D(double[] X, double[] Y, double[] Z, out double start_x, out double start_y, out double start_z, out double end_x, out double end_y, out double end_z)
        {
            if (X == null)
            {
                throw new ArgumentNullException("X"); //num_points
            }
            if (Y == null)
            {
                throw new ArgumentNullException("Y"); //num_points
            }
            if (Z == null)
            {
                throw new ArgumentNullException("Z"); //num_points
            }
            if (X.Length != Y.Length || X.Length != Z.Length)
            {
                throw new ArgumentNullException("长度不相等!"); //num_points
            }
            int count = X.Length;
            if (count < 2)
            {
                throw new ArgumentException("对象中包含的数据点小于 2", "lineObjcetModel");
            }
            HalconLibrary ha = new HalconLibrary();
            HTuple coord_x, coord_y, coord_z, sort_x, sort_y;
            double[] x, y;
            coord_x = X;
            coord_y = Y;
            coord_z = Z;
            x = new double[coord_x.Length];
            y = new double[coord_x.Length];
            // 拟合XY直线
            double Nr, Nc, Dist, x1, y1, z1, x2, y2, z2, Pro_x, Pro_y;
            //if ((coord_x.TupleMax() - coord_x.TupleMin()) > (coord_y.TupleMax() - coord_y.TupleMin()))
            //    ha.sortPairs(coord_x, coord_y, 1, out sort_x, out sort_y);
            //else
            //    ha.sortPairs(coord_x, coord_y, 2, out sort_x, out sort_y);
            //new HXLDCont(sort_x, sort_y).FitLineContourXld("tukey", -1, 0, 5, 2, out x1, out y1, out x2, out y2, out Nr, out Nc, out Dist);
            new HXLDCont(coord_x, coord_y).FitLineContourXld("tukey", -1, 0, 5, 2, out x1, out y1, out x2, out y2, out Nr, out Nc, out Dist); // 坐标系要符合笛卡尔坐标系
            start_x = x1;
            start_y = y1;
            end_x = x2;
            end_y = y2;
            // 将离散的XY点转换到拟合的直线上，通过点到直线的投影
            for (int i = 0; i < coord_x.Length; i++)
            {
                HMisc.ProjectionPl(coord_x[i].D, coord_y[i].D, x1, y1, x2, y2, out Pro_x, out Pro_y);
                x[i] = Pro_x;
                y[i] = Pro_y;
            }
            // 将三维轮廓转换为二维轮廓
            double[] single_x = new double[x.Length];
            double[] single_z = new double[x.Length];
            double min_x = x.Min();
            double min_y = y.Min();
            for (int j = 0; j < x.Length; j++)
            {
                single_x[j] = HMisc.DistancePp(x[j], y[j], x[0], y[0]); //将二维的XY坐标转换为一维的X坐标
                single_z[j] = coord_z[j].D;
            }
            // 拟合直线
            new HXLDCont(single_x, single_z).FitLineContourXld("tukey", -1, 0, 5, 2, out x1, out z1, out x2, out z2, out Nr, out Nc, out Dist);
            start_z = z1;
            end_z = z2;
        }
        public void FitLine3D(HObjectModel3D lineObjcetModel, string FitCoord, out double start_x, out double start_y, out double start_z, out double end_x, out double end_y, out double end_z)
        {
            if (lineObjcetModel == null)
            {
                throw new ArgumentNullException("lineObjcetModel"); //num_points
            }
            int count = lineObjcetModel.GetObjectModel3dParams("num_points");
            if (count == 0)
            {
                throw new ArgumentException("对象中不包含数据点", "lineObjcetModel");
            }
            HalconLibrary ha = new HalconLibrary();
            HTuple coord_x, coord_y, coord_z, sort_x, sort_y, sort_z;
            double[] x, y;
            coord_x = lineObjcetModel.GetObjectModel3dParams("point_coord_x");
            coord_y = lineObjcetModel.GetObjectModel3dParams("point_coord_y");
            coord_z = lineObjcetModel.GetObjectModel3dParams("point_coord_z");
            double Nr, Nc, Dist, RowBegin, ColBegin, RowEnd, ColEnd, Pro_x, Pro_y;
            switch (FitCoord)
            {
                default:
                case "XYZ":
                    x = new double[coord_x.Length];
                    y = new double[coord_x.Length];
                    // 拟合XY直线
                    if ((coord_x.TupleMax() - coord_x.TupleMin()) > (coord_y.TupleMax() - coord_y.TupleMin()))
                        ha.sortPairs(coord_x, coord_y, 1, out sort_x, out sort_y);
                    else
                        ha.sortPairs(coord_x, coord_y, 2, out sort_x, out sort_y);
                    new HXLDCont(sort_y, sort_x).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                    start_x = ColBegin;
                    start_y = RowBegin;
                    end_x = ColEnd;
                    end_y = RowEnd;
                    // 将离散的XY点转换到拟合的直线上，通过点到直线的投影
                    for (int i = 0; i < coord_x.Length; i++)
                    {
                        HMisc.ProjectionPl(coord_y[i].D, coord_x[i].D, RowBegin, ColBegin, RowEnd, ColEnd, out Pro_y, out Pro_x);
                        x[i] = Pro_x;
                        y[i] = Pro_y;
                    }
                    // 将三维轮廓转换为二维轮廓
                    double[] single_x = new double[x.Length];
                    double[] single_z = new double[x.Length];
                    double min_x = x.Min();
                    double min_y = y.Min();
                    for (int j = 0; j < x.Length; j++)
                    {
                        single_x[j] = HMisc.DistancePp(y[j], x[j], y[0], x[0]);//将二维的XY坐标转换为一维的X坐标
                        single_z[j] = coord_z[j].D;
                    }
                    // 拟合直线
                    new HXLDCont(single_z, single_x).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                    start_z = RowBegin;
                    end_z = RowEnd;
                    break;
                case "XY":
                    // 拟合XY直线
                    if ((coord_x.TupleMax() - coord_x.TupleMin()) > (coord_y.TupleMax() - coord_y.TupleMin()))
                        ha.sortPairs(coord_x, coord_y, 1, out sort_x, out sort_y);
                    else
                        ha.sortPairs(coord_x, coord_y, 2, out sort_x, out sort_y);
                    new HXLDCont(sort_y, sort_x).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                    start_x = ColBegin;
                    start_y = RowBegin;
                    end_x = ColEnd;
                    end_y = RowEnd;
                    start_z = 0;
                    end_z = 0;
                    break;
                case "XZ":
                    // 拟合XY直线
                    if ((coord_x.TupleMax() - coord_x.TupleMin()) > (coord_z.TupleMax() - coord_z.TupleMin()))
                        ha.sortPairs(coord_x, coord_z, 1, out sort_x, out sort_z);
                    else
                        ha.sortPairs(coord_x, coord_z, 2, out sort_x, out sort_z);
                    new HXLDCont(sort_z, sort_x).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                    start_x = ColBegin;
                    start_y = 0;
                    end_x = ColEnd;
                    end_y = 0;
                    start_z = RowBegin;
                    end_z = RowEnd;
                    break;
                case "YZ":
                    // 拟合XY直线
                    if ((coord_y.TupleMax() - coord_y.TupleMin()) > (coord_z.TupleMax() - coord_z.TupleMin()))
                        ha.sortPairs(coord_y, coord_z, 1, out sort_y, out sort_z);
                    else
                        ha.sortPairs(coord_y, coord_z, 2, out sort_y, out sort_z);
                    new HXLDCont(sort_z, sort_y).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                    start_x = 0;
                    start_y = ColBegin;
                    end_x = 0;
                    end_y = ColEnd;
                    start_z = RowBegin;
                    end_z = RowEnd;
                    break;
            }
        }
        public void FitLine3D(double[] X, double[] Y, double[] Z, string FitCoord, out double start_x, out double start_y, out double start_z, out double end_x, out double end_y, out double end_z)
        {
            if (X == null)
            {
                throw new ArgumentNullException("X"); //num_points
            }
            if (Y == null)
            {
                throw new ArgumentNullException("Y"); //num_points
            }
            if (Z == null)
            {
                throw new ArgumentNullException("Z"); //num_points
            }
            if (X.Length != Y.Length || X.Length != Z.Length)
            {
                throw new ArgumentNullException("长度不相等!"); //num_points
            }
            int count = X.Length;
            if (count == 0)
            {
                throw new ArgumentException("对象中不包含数据点", "lineObjcetModel");
            }
            HalconLibrary ha = new HalconLibrary();
            HTuple coord_x, coord_y, coord_z, sort_x, sort_y, sort_z;
            double[] x, y;
            coord_x = X;
            coord_y = Y;
            coord_z = Z;
            double Nr, Nc, Dist, RowBegin, ColBegin, RowEnd, ColEnd, Pro_x, Pro_y;
            switch (FitCoord)
            {
                default:
                case "XYZ":
                    x = new double[coord_x.Length];
                    y = new double[coord_x.Length];
                    // 拟合XY直线
                    if ((coord_x.TupleMax() - coord_x.TupleMin()) > (coord_y.TupleMax() - coord_y.TupleMin()))
                        ha.sortPairs(coord_x, coord_y, 1, out sort_x, out sort_y);
                    else
                        ha.sortPairs(coord_x, coord_y, 2, out sort_x, out sort_y);
                    new HXLDCont(sort_y, sort_x).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                    start_x = ColBegin;
                    start_y = RowBegin;
                    end_x = ColEnd;
                    end_y = RowEnd;
                    // 将离散的XY点转换到拟合的直线上，通过点到直线的投影
                    for (int i = 0; i < coord_x.Length; i++)
                    {
                        HMisc.ProjectionPl(coord_y[i].D, coord_x[i].D, RowBegin, ColBegin, RowEnd, ColEnd, out Pro_y, out Pro_x);
                        x[i] = Pro_x;
                        y[i] = Pro_y;
                    }
                    // 将三维轮廓转换为二维轮廓
                    double[] single_x = new double[x.Length];
                    double[] single_z = new double[x.Length];
                    double min_x = x.Min();
                    double min_y = y.Min();
                    for (int j = 0; j < x.Length; j++)
                    {
                        single_x[j] = HMisc.DistancePp(y[j], x[j], y[0], x[0]);//将二维的XY坐标转换为一维的X坐标
                        single_z[j] = coord_z[j].D;
                    }
                    // 拟合直线
                    new HXLDCont(single_z, single_x).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                    start_z = RowBegin;
                    end_z = RowEnd;
                    break;
                case "XY":
                    // 拟合XY直线
                    if ((coord_x.TupleMax() - coord_x.TupleMin()) > (coord_y.TupleMax() - coord_y.TupleMin()))
                        ha.sortPairs(coord_x, coord_y, 1, out sort_x, out sort_y);
                    else
                        ha.sortPairs(coord_x, coord_y, 2, out sort_x, out sort_y);
                    new HXLDCont(sort_y, sort_x).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                    start_x = ColBegin;
                    start_y = RowBegin;
                    end_x = ColEnd;
                    end_y = RowEnd;
                    start_z = 0;
                    end_z = 0;
                    break;
                case "XZ":
                    // 拟合XY直线
                    if ((coord_x.TupleMax() - coord_x.TupleMin()) > (coord_z.TupleMax() - coord_z.TupleMin()))
                        ha.sortPairs(coord_x, coord_z, 1, out sort_x, out sort_z);
                    else
                        ha.sortPairs(coord_x, coord_z, 2, out sort_x, out sort_z);
                    new HXLDCont(sort_z, sort_x).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                    start_x = ColBegin;
                    start_y = 0;
                    end_x = ColEnd;
                    end_y = 0;
                    start_z = RowBegin;
                    end_z = RowEnd;
                    break;
                case "YZ":
                    // 拟合XY直线
                    if ((coord_y.TupleMax() - coord_y.TupleMin()) > (coord_z.TupleMax() - coord_z.TupleMin()))
                        ha.sortPairs(coord_y, coord_z, 1, out sort_y, out sort_z);
                    else
                        ha.sortPairs(coord_y, coord_z, 2, out sort_y, out sort_z);
                    new HXLDCont(sort_z, sort_y).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                    start_x = 0;
                    start_y = ColBegin;
                    end_x = 0;
                    end_y = ColEnd;
                    start_z = RowBegin;
                    end_z = RowEnd;
                    break;
            }
        }
        public void FitProfile3D(HObjectModel3D lineObjcetModel, out HObjectModel3D fitLineObjcetModel)
        {
            fitLineObjcetModel = new HObjectModel3D();
            if (lineObjcetModel == null)
            {
                throw new ArgumentNullException("lineObjcetModel"); //num_points
            }
            int count = lineObjcetModel.GetObjectModel3dParams("num_points");
            if (count == 0)
            {
                throw new ArgumentException("对象中不包含数据点", "lineObjcetModel");
            }
            HTuple line_x, line_y, line_z;
            double[] x, y;
            line_x = lineObjcetModel.GetObjectModel3dParams("point_coord_x"); //要把三维的图形转换成二维图形
            line_y = lineObjcetModel.GetObjectModel3dParams("point_coord_y");
            line_z = lineObjcetModel.GetObjectModel3dParams("point_coord_z");
            x = new double[line_x.Length];
            y = new double[line_x.Length];
            // 拟合直线
            double Nr, Nc, Dist, RowBegin, ColBegin, RowEnd, ColEnd, Pro_x, Pro_y;
            new HXLDCont(line_y, line_x).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
            for (int i = 0; i < line_y.Length; i++)
            {
                HMisc.ProjectionPl(line_y[i].D, line_x[i].D, RowBegin, ColBegin, RowEnd, ColEnd, out Pro_y, out Pro_x);
                x[i] = Pro_x;
                y[i] = Pro_y;
            }
            fitLineObjcetModel.GenObjectModel3dFromPoints(x, y, line_z);
        }
        public void SmoothProfileXY(double[] X, double[] Y, enProfileSmoothMethod smoothMethod, double smoothSigma, out double[] smooth_x, out double[] smooth_y)
        {
            if (X == null)
            {
                throw new ArgumentNullException("X", "输入参数X为空");
            }
            if (Y == null)
            {
                throw new ArgumentNullException("Y", "输入参数Y为空");
            }
            if (Y.Length != X.Length)
            {
                throw new ArgumentException("X/Y数组长度不相等");
            }
            /////////////
            HTuple sort_x, m_x, m_y;
            switch (smoothMethod)
            {
                default:
                case enProfileSmoothMethod.均值平滑:
                    new HXLDCont(Y, X).SmoothContoursXld((int)smoothSigma).GetContourXld(out m_y, out m_x);
                    smooth_x = m_x.ToDArr();
                    smooth_y = m_y.ToDArr();
                    break;
                case enProfileSmoothMethod.高斯平滑:
                    HFunction1D hFunction1D = new HFunction1D(Y.ToArray());
                    HFunction1D hFunction1D1 = hFunction1D.SmoothFunct1dGauss(smoothSigma);
                    hFunction1D1.Funct1dToPairs(out sort_x, out m_y);
                    smooth_x = X;
                    smooth_y = m_y.ToDArr();
                    break;
            }
        }

        /// <summary>
        /// 采样后的坐标点将按升序排列
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="sampleDist"></param>
        /// <param name=""></param>
        /// <param name="sample_x"></param>
        /// <param name="sample_y"></param>
        public void SampleProfileXY(double[] X, double[] Y, double sampleDist, out double[] sample_x, out double[] sample_y)
        {
            if (X == null)
            {
                throw new ArgumentNullException("X", "输入参数X为空");
            }
            if (Y == null)
            {
                throw new ArgumentNullException("Y", "输入参数Y为空");
            }
            if (Y.Length != X.Length)
            {
                throw new ArgumentException("X/Y数组长度不相等");
            }
            if (sampleDist <= 0)
            {
                throw new ArgumentException("输入的采样距离值必需大于0", "sampleDist");
            }
            HTuple s_x, s_y, sort_x, sort_y;
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            this.sortPairs(X, Y, 1, out sort_x, out sort_y); // 必需要强制排一次序，数据从小往大排列
            list_x.Add(sort_x[0].D); // 先添加第一个点
            list_y.Add(sort_y[0].D);
            for (int i = 0; i < sort_x.Length - 1; i++)
            {
                if (sort_x[i + 1] > sort_x[i])
                {
                    list_x.Add(sort_x[i + 1].D);
                    list_y.Add(sort_y[i + 1].D);
                }
            }
            HFunction1D hFunction1D = new HFunction1D(list_x.ToArray(), list_y.ToArray());
            HFunction1D hFunction1DSample = hFunction1D.SampleFunct1d(list_x[0], list_x[list_x.Count - 1], sampleDist, "constant");
            hFunction1DSample.Funct1dToPairs(out s_x, out s_y);
            sample_x = s_x.ToDArr();
            sample_y = s_y.ToDArr();
        }

        /// <summary>
        /// 拟合3D点云到平面 
        /// </summary>
        /// <param name="ObjectModel"></param>
        /// <param name="pose"></param>
        public void GetPlaneObjectModel3DPose(HObjectModel3D ObjectModel, out HPose pose)
        {
            HTuple planePoseNormal, ref_pose;
            if (ObjectModel == null)
            {
                throw new ArgumentNullException("targetObjectModel");
            }
            // 如果参考对象为基本体对象
            if (ObjectModel.GetObjectModel3dParams("has_primitive_data").S == "false")
            {
                if (ObjectModel.GetObjectModel3dParams("num_points").I == 0)
                {
                    throw new ArgumentException("对象中不包含数据点", "lineObjcetModel");
                }
                HObjectModel3D refPlaneObjectModel3D = ObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_huber"));
                ref_pose = refPlaneObjectModel3D.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = refPlaneObjectModel3D.GetObjectModel3dParams("primitive_parameter");
                if (planePoseNormal[2].D < 0)  // 这一步是为了保证平面的法向向Z轴正方向
                {
                    HTuple homMat3D, homMat3DRotate;
                    HOperatorSet.PoseToHomMat3d(ref_pose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out ref_pose);
                    pose = new HPose(ref_pose);
                }
                else
                    pose = new HPose(ref_pose);
            }
            else
            {
                ref_pose = ObjectModel.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = ObjectModel.GetObjectModel3dParams("primitive_parameter");
                if (planePoseNormal[2].D < 0)
                {
                    HTuple homMat3D, homMat3DRotate;
                    HOperatorSet.PoseToHomMat3d(ref_pose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out ref_pose);
                    pose = new HPose(ref_pose);
                }
                else
                    pose = new HPose(ref_pose);
            }
        }
        public void GetPlaneObjectModel3DPose(double[] X, double[] Y, double[] Z, out HPose pose)
        {
            HTuple planePoseNormal, ref_pose;
            if (X == null)
            {
                throw new ArgumentNullException("X");
            }
            if (Y == null)
            {
                throw new ArgumentNullException("Y");
            }
            if (Z == null)
            {
                throw new ArgumentNullException("Z");
            }
            HObjectModel3D ObjectModel = new HObjectModel3D(X, Y, Z);
            // 如果参考对象为基本体对象
            if (ObjectModel.GetObjectModel3dParams("has_primitive_data").S == "false")
            {
                if (ObjectModel.GetObjectModel3dParams("num_points").I == 0)
                {
                    throw new ArgumentException("对象中不包含数据点", "lineObjcetModel");
                }
                HObjectModel3D refPlaneObjectModel3D = ObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_huber"));
                ref_pose = refPlaneObjectModel3D.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = refPlaneObjectModel3D.GetObjectModel3dParams("primitive_parameter");
                if (planePoseNormal[2].D < 0)  // 这一步是为了保证平面的法向向Z轴正方向
                {
                    HTuple homMat3D, homMat3DRotate;
                    HOperatorSet.PoseToHomMat3d(ref_pose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out ref_pose);
                    pose = new HPose(ref_pose);
                }
                else
                    pose = new HPose(ref_pose);
            }
            else
            {
                ref_pose = ObjectModel.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = ObjectModel.GetObjectModel3dParams("primitive_parameter");
                if (planePoseNormal[2].D < 0)
                {
                    HTuple homMat3D, homMat3DRotate;
                    HOperatorSet.PoseToHomMat3d(ref_pose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out ref_pose);
                    pose = new HPose(ref_pose);
                }
                else
                    pose = new HPose(ref_pose);
            }
            ObjectModel?.ClearObjectModel3d();
        }
        public void GetPlaneObjectModel3DPose(HObjectModel3D ObjectModel, out userWcsPlane planePose)
        {
            HTuple planePoseNormal, ref_pose;
            planePose = new userWcsPlane();
            if (ObjectModel == null)
            {
                throw new ArgumentNullException("targetObjectModel");
            }
            // 如果参考对象为基本体对象
            if (ObjectModel.GetObjectModel3dParams("has_primitive_data").S == "false")
            {
                if (ObjectModel.GetObjectModel3dParams("num_points").I == 0)
                {
                    throw new ArgumentException("对象中不包含数据点", "lineObjcetModel");
                }
                HObjectModel3D refPlaneObjectModel3D = ObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_huber"));
                ref_pose = refPlaneObjectModel3D.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = refPlaneObjectModel3D.GetObjectModel3dParams("primitive_parameter");
                if (planePoseNormal[2].D < 0)  // 这一步是为了保证平面的法向向Z轴正方向
                {
                    HTuple homMat3D, homMat3DRotate;
                    HOperatorSet.PoseToHomMat3d(ref_pose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out ref_pose);
                    //pose = new HPose(ref_pose);
                }
                //else
                //    pose = new HPose(ref_pose);
            }
            else
            {
                ref_pose = ObjectModel.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = ObjectModel.GetObjectModel3dParams("primitive_parameter");
                if (planePoseNormal[2].D < 0)
                {
                    HTuple homMat3D, homMat3DRotate;
                    HOperatorSet.PoseToHomMat3d(ref_pose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out ref_pose);
                    //pose = new HPose(ref_pose);
                }
                //else
                //    pose = new HPose(ref_pose);
            }
            /////////////////
            switch (ref_pose.Type)
            {
                case HTupleType.DOUBLE:
                    planePose.X = ref_pose[0].D;
                    planePose.Y = ref_pose[1].D;
                    planePose.Z = ref_pose[2].D;
                    planePose.WcsPose = new userWcsPose(ref_pose);
                    break;

                case HTupleType.INTEGER:
                    planePose.X = (double)ref_pose[0].I;
                    planePose.Y = (double)ref_pose[1].I;
                    planePose.Z = (double)ref_pose[2].I;
                    planePose.WcsPose = new userWcsPose(ref_pose);
                    break;

                case HTupleType.LONG:
                    planePose.X = (double)ref_pose[0].L;
                    planePose.Y = (double)ref_pose[1].L;
                    planePose.Z = (double)ref_pose[2].L;
                    planePose.WcsPose = new userWcsPose(ref_pose);
                    break;

                case HTupleType.MIXED:
                    planePose.X = (double)ref_pose[0].O;
                    planePose.Y = (double)ref_pose[1].O;
                    planePose.Z = (double)ref_pose[2].O;
                    planePose.WcsPose = new userWcsPose(ref_pose);
                    break;

                case HTupleType.EMPTY:
                case HTupleType.STRING:
                default:
                    planePose.X = 0;
                    planePose.Y = 0;
                    planePose.Z = 0;
                    planePose.WcsPose = new userWcsPose();
                    break;
            }
        }

        public void GetPlaneObjectModel3DPose(double[] X, double[] Y, double[] Z, out userWcsPlane planePose)
        {
            HTuple planePoseNormal, ref_pose;
            planePose = new userWcsPlane();
            if (X == null)
            {
                throw new ArgumentNullException("X");
            }
            if (Y == null)
            {
                throw new ArgumentNullException("Y");
            }
            if (Z == null)
            {
                throw new ArgumentNullException("Z");
            }
            HObjectModel3D ObjectModel = new HObjectModel3D(X, Y, Z);
            // 如果参考对象为基本体对象
            if (ObjectModel.GetObjectModel3dParams("has_primitive_data").S == "false")
            {
                if (ObjectModel.GetObjectModel3dParams("num_points").I == 0)
                {
                    throw new ArgumentException("对象中不包含数据点", "lineObjcetModel");
                }
                HObjectModel3D refPlaneObjectModel3D = ObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_huber"));
                ref_pose = refPlaneObjectModel3D.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = refPlaneObjectModel3D.GetObjectModel3dParams("primitive_parameter");
                if (planePoseNormal[2].D < 0)  // 这一步是为了保证平面的法向向Z轴正方向
                {
                    HTuple homMat3D, homMat3DRotate;
                    HOperatorSet.PoseToHomMat3d(ref_pose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out ref_pose);
                };
            }
            else
            {
                ref_pose = ObjectModel.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = ObjectModel.GetObjectModel3dParams("primitive_parameter");
                if (planePoseNormal[2].D < 0)
                {
                    HTuple homMat3D, homMat3DRotate;
                    HOperatorSet.PoseToHomMat3d(ref_pose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out ref_pose);
                }
            }
            ObjectModel?.ClearObjectModel3d();
            /////////////////
            switch (ref_pose.Type)
            {
                case HTupleType.DOUBLE:
                    planePose.X = ref_pose[0].D;
                    planePose.Y = ref_pose[1].D;
                    planePose.Z = ref_pose[2].D;
                    planePose.WcsPose = new userWcsPose(ref_pose);
                    break;

                case HTupleType.INTEGER:
                    planePose.X = (double)ref_pose[0].I;
                    planePose.Y = (double)ref_pose[1].I;
                    planePose.Z = (double)ref_pose[2].I;
                    planePose.WcsPose = new userWcsPose(ref_pose);
                    break;

                case HTupleType.LONG:
                    planePose.X = (double)ref_pose[0].L;
                    planePose.Y = (double)ref_pose[1].L;
                    planePose.Z = (double)ref_pose[2].L;
                    planePose.WcsPose = new userWcsPose(ref_pose);
                    break;

                case HTupleType.MIXED:
                    planePose.X = (double)ref_pose[0].O;
                    planePose.Y = (double)ref_pose[1].O;
                    planePose.Z = (double)ref_pose[2].O;
                    planePose.WcsPose = new userWcsPose(ref_pose);
                    break;

                case HTupleType.EMPTY:
                case HTupleType.STRING:
                default:
                    planePose.X = 0;
                    planePose.Y = 0;
                    planePose.Z = 0;
                    planePose.WcsPose = new userWcsPose();
                    break;
            }
        }
        public void UpdataObjectModel(HTuple X, HTuple Y, HTuple Z, out HTuple objectModel3D)
        {
            objectModel3D = null;
            if (X == null)
            {
                throw new ArgumentNullException("X");
            }
            if (Y == null)
            {
                throw new ArgumentNullException("Y");
            }
            if (Z == null)
            {
                throw new ArgumentNullException("Z");
            }
            if (X.Length != Y.Length)
            {
                throw new ArgumentException("X/Y长度不相等");
            }
            if (X.Length != Z.Length)
            {
                throw new ArgumentException("X/Z长度不相等");
            }
            if (Y.Length != Z.Length)
            {
                throw new ArgumentException("Y/Z长度不相等");
            }
            HOperatorSet.GenObjectModel3dFromPoints(X, Y, Z, out objectModel3D);
        }
        public void ClearObjectModel3D(HTuple objectModel3D)
        {
            if (objectModel3D != null)
            {
                for (int i = 0; i < objectModel3D.Length; i++)
                {
                    Clear(objectModel3D[i]);
                }
            }
        }









        public bool WriteImageToFile(object objectModel, string path)
        {
            bool result = false;
            FileOperate fo = new FileOperate();
            string[] extenName = path.Split('.');
            //////////////////////
            if (objectModel == null) return result;
            ////////////////////////
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            switch (objectModel.GetType().Name)
            {
                case "ImageDataClass": //
                    if (((ImageDataClass)objectModel).Image == null) return result;
                    if (extenName[1] == "jpeg")
                        HOperatorSet.WriteImage(((ImageDataClass)objectModel).Image, "jpeg", 0, path);
                    if (extenName[1] == "bmp")
                        HOperatorSet.WriteImage(((ImageDataClass)objectModel).Image, "bmp", 0, path);
                    if (extenName[1] == "tiff")
                        HOperatorSet.WriteImage(((ImageDataClass)objectModel).Image, "tiff", 0, path);
                    if (extenName[1] == "png")
                        HOperatorSet.WriteImage(((ImageDataClass)objectModel).Image, "png", 0, path);
                    break;
                case "HImage": //
                    if (extenName[1] == "jpeg")
                        HOperatorSet.WriteImage(((HImage)objectModel), "jpeg", 0, path);
                    if (extenName[1] == "bmp")
                        HOperatorSet.WriteImage(((HImage)objectModel), "bmp", 0, path);
                    if (extenName[1] == "tiff")
                        HOperatorSet.WriteImage(((HImage)objectModel), "tiff", 0, path);
                    if (extenName[1] == "png")
                        HOperatorSet.WriteImage(((HImage)objectModel), "png", 0, path);
                    break;
                case "HRegion":
                    if (extenName[1] == "hobj")
                        HOperatorSet.WriteRegion((HRegion)objectModel, path);
                    break;
            }
            result = true;
            return result;

        }
        public bool WriteXLDToFile(object objectModel, string path)
        {
            bool result = false;
            FileOperate fo = new FileOperate();
            string[] extenName = path.Split('.');
            //////////////////////
            if (objectModel == null) return result;
            ////////////////////////
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            switch (objectModel.GetType().Name)
            {
                case "HXLDCont":
                    if (((XldDataClass)objectModel).HXldCont == null) return result;
                    if (extenName[1] == "dxf")
                        HOperatorSet.WriteContourXldDxf(((XldDataClass)objectModel).HXldCont, path);
                    break;
                case "HXLDPoly":
                    if (((XldDataClass)objectModel).HXldCont == null) return result;
                    if (extenName[1] == "dxf")
                        HOperatorSet.WritePolygonXldDxf(((XldDataClass)objectModel).HXldCont, path);
                    break;
            }
            result = true;
            return result;
        }


        public void GenParallelLine(userWcsLine lineIn, enOffsetType type, double dist, int count, out userWcsLine[] lineOut)
        {
            lineOut = null;
            if (lineIn.X1 == 0 && lineIn.Y1 == 0 && lineIn.Z1 == 0 && lineIn.X2 == 0 && lineIn.Y2 == 0 && lineIn.Z2 == 0)
            {
                lineOut = null;
                return;
            }
            try
            {
                HObject line, ParallelLine;
                HTuple row, col;
                switch (type)
                {
                    case enOffsetType.对称偏置:
                        lineOut = new userWcsLine[count * 2];
                        for (int i = 0; i < lineOut.Length; i++)
                        {
                            HOperatorSet.GenContourPolygonXld(out line, new HTuple(lineIn.Y1, lineIn.Y2), new HTuple(lineIn.X1, lineIn.X2));
                            if (i < count)
                                HOperatorSet.GenParallelContourXld(line, out ParallelLine, "regression_normal", dist * (i + 1));
                            else
                                HOperatorSet.GenParallelContourXld(line, out ParallelLine, "regression_normal", dist * (i - count + 1) * -1);
                            HOperatorSet.GetContourXld(ParallelLine, out row, out col);
                            if (row != null && row.Length > 0)
                                lineOut[i] = new userWcsLine(col[0].D, row[0].D, lineIn.Z1, col[1].D, row[1].D, lineIn.Z2, null, null);
                        }
                        break;
                    case enOffsetType.正向偏置:
                        lineOut = new userWcsLine[count];
                        for (int i = 0; i < lineOut.Length; i++)
                        {
                            HOperatorSet.GenContourPolygonXld(out line, new HTuple(lineIn.Y1, lineIn.Y2), new HTuple(lineIn.X1, lineIn.X2));
                            HOperatorSet.GenParallelContourXld(line, out ParallelLine, "regression_normal", dist * (i + 1));
                            HOperatorSet.GetContourXld(ParallelLine, out row, out col);
                            if (row != null && row.Length > 0)
                                lineOut[i] = new userWcsLine(col[0].D, row[0].D, lineIn.Z1, col[1].D, row[1].D, lineIn.Z2, null, null);
                        }
                        break;
                    case enOffsetType.负向偏置:
                        lineOut = new userWcsLine[count];
                        for (int i = 0; i < lineOut.Length; i++)
                        {
                            HOperatorSet.GenContourPolygonXld(out line, new HTuple(lineIn.Y1, lineIn.Y2), new HTuple(lineIn.X1, lineIn.X2));
                            HOperatorSet.GenParallelContourXld(line, out ParallelLine, "regression_normal", dist * (i + 1) * -1);
                            HOperatorSet.GetContourXld(ParallelLine, out row, out col);
                            if (row != null && row.Length > 0)
                                lineOut[i] = new userWcsLine(col[0].D, row[0].D, lineIn.Z1, col[1].D, row[1].D, lineIn.Z2, null, null);
                        }
                        break;
                }
            }
            catch
            {

            }
        }

        public void Transform3DProfileTo2DProfile(HTuple objectModel3D, out HTuple data_x, out HTuple data_y)
        {
            HTuple x_value, y_value, z_value;
            HTuple unionObjectModel = null;
            HTuple x = new HTuple();
            HTuple y = new HTuple();
            HTuple z = new HTuple();
            HTuple X_minValeu;
            HTuple Y_minValue;
            data_x = null;
            data_y = null;
            if (objectModel3D.Length > 0) // 都执行一次合并
                HOperatorSet.UnionObjectModel3d(objectModel3D, "points_surface", out unionObjectModel);
            else
                throw new FormatException();
            ///////////////////////////
            try
            {
                if (IsContainPoint(unionObjectModel)) // 如果模型中包含有点
                {
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_x", out x_value);
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_y", out y_value);
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_z", out z_value);
                    ///////////////////////////////////////
                    HTuple dist;
                    // HTuple X_minValeu = x_value.TupleMin();
                    // HTuple Y_minValue = y_value.TupleMin();
                    X_minValeu = 0; // 以原点做为相对参考点，这样便于统一坐标
                    Y_minValue = 0;
                    for (int i = 0; i < x_value.Length; i++)
                    {
                        dist = Math.Sqrt((x_value[i].D - X_minValeu.D) * (x_value[i].D - X_minValeu.D) + (y_value[i].D - Y_minValue.D) * (y_value[i].D - Y_minValue.D));
                        x = x.TupleConcat(X_minValeu + dist);
                        y = y.TupleConcat(z_value[i]);
                    }
                    sortPairs(x, y, 1, out data_x, out data_y);
                }
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                Clear(unionObjectModel);
            }
        }



        public void TriangulateObjectModel3d(HTuple objectModel, HTuple method, HTuple paramName, HTuple paramValue, out HTuple triangulatedObjectModel3D)
        {
            // "greedy", new HTuple("greedy_kNN", "greedy_hole_filling"), new HTuple(40, 200)
            triangulatedObjectModel3D = null;
            HTuple unionObjectModel = null;
            HTuple ParamValue = null;
            try
            {
                // 获取元素中包含的包含的数
                if (objectModel != null && objectModel.Length > 0)
                    HOperatorSet.UnionObjectModel3d(objectModel, "points_surface", out unionObjectModel);
                else
                    throw new FormatException();
                //////////////////
                HOperatorSet.TriangulateObjectModel3d(unionObjectModel, method, paramName, paramValue, out triangulatedObjectModel3D, out ParamValue);
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                Clear(unionObjectModel);
            }


        }
        public void FindSurfaceModel(HTuple surfaceModelID, HTuple objectModel3D, HTuple rel_samp_Dist, HTuple key_point_fraction, HTuple minScore, HTuple returnResultHandle, HTuple paramName, HTuple paramValue, out HTuple matchPose, out HTuple score, out HTuple surfaceMatchResultID)
        {
            HTuple unionObjectModel = null;
            score = 0;
            surfaceMatchResultID = null;
            matchPose = null;
            try
            {
                if (objectModel3D != null && objectModel3D.Length > 0) // 都执行一次合并
                    HOperatorSet.UnionObjectModel3d(objectModel3D, "points_surface", out unionObjectModel);
                else
                    throw new FormatException();
                //////////////////////////
                if (surfaceModelID == null) return;
                if (surfaceModelID.Length == 0) return;
                HOperatorSet.FindSurfaceModel(surfaceModelID, unionObjectModel, rel_samp_Dist, key_point_fraction, minScore, returnResultHandle, paramName, paramValue, out matchPose, out score, out surfaceMatchResultID);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                Clear(unionObjectModel);
            }
        }
        public void RegisterMatch(HTuple Match_ObjectModel, HTuple ref_ObjectModel, HTuple paramName, HTuple paramValue, out HTuple matchPose, out HTuple score)
        {
            HTuple unionMatchObjectModel = null;
            HTuple unionRefObjectModel = null;
            matchPose = null;
            score = 0.0;
            try
            {
                if (Match_ObjectModel != null && Match_ObjectModel.Length > 0) // 都执行一次合并
                    HOperatorSet.UnionObjectModel3d(Match_ObjectModel, "points_surface", out unionMatchObjectModel);
                else
                    throw new FormatException();
                if (ref_ObjectModel != null && ref_ObjectModel.Length > 0) // 都执行一次合并
                    HOperatorSet.UnionObjectModel3d(ref_ObjectModel, "points_surface", out unionRefObjectModel);
                else
                    throw new FormatException();
                /////////////////////////匹配/////////////////////////////    new HTuple("default_parameters", "rel_sampling_distance"), new HTuple(this.matchMethod, samp_Dist)
                HOperatorSet.RegisterObjectModel3dPair(unionMatchObjectModel, unionRefObjectModel, "matching", paramName, paramValue, out matchPose, out score);// ICP 算法用于位姿细化                                                                                                                                                              // HOperatorSet.PoseInvert(pose, out matchPose);// 该位姿描述的是参考对象相对于匹配对象的位姿；；；；注意图像补正与位置补正的区别
            }
            finally
            {
                Clear(unionMatchObjectModel);
                Clear(unionRefObjectModel);
            }
            // 注册匹配与表面匹配的区别，注册匹配是寻找对象到参考对象的最佳变换，而表面匹配是寻找表面模型到对象的最佳变换
            // 即，注册匹配是获得，参考对象相对于匹配对象的位姿，而表面匹配是获得，对象相对于表面模型的位姿 ，所以这里需要取           
        }


        public void GaussSmooth2DPoint(float[] value, out float smoothValue)
        {
            smoothValue = 0;
            HTuple filterValue = new HTuple();
            HTuple function, smoothFunction, smooth_x, smooth_y;
            if (value == null) return;
            // 去悼0值点
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != 0)
                    filterValue.Append(value[i]);
            }
            if (filterValue.Length < 5)
            {
                if (filterValue.Length == 0)
                    smoothValue = 0;
                else
                    smoothValue = (float)(filterValue.TupleSum() / filterValue.Length).D;
            }
            else
            {

                HTuple sigma = Math.Abs((filterValue.Length - 2) / 8.0);
                if (sigma.D < 0.1)
                    sigma = 0.1;
                HOperatorSet.CreateFunct1dArray(filterValue, out function);
                HOperatorSet.SmoothFunct1dGauss(function, sigma, out smoothFunction);
                HOperatorSet.Funct1dToPairs(smoothFunction, out smooth_x, out smooth_y);
                smoothValue = (float)(smooth_y.TupleSum() / smooth_y.Length).D;
            }
        }
        public void GaussSmooth2DPoint(double[] value, out double smoothValue)
        {
            smoothValue = 0;
            HTuple filterValue = new HTuple();
            HTuple function, smoothFunction, smooth_x, smooth_y;
            if (value == null) return;
            // 去悼0值点
            for (int i = 0; i < value.Length; i++) // (int)(value.Length * 0.4)
            {
                if (value[i] >= 0)
                    filterValue.Append(value[i]);
            }
            if (filterValue.Length < 5)
            {
                if (filterValue.Length == 0)
                    smoothValue = 0;
                else
                    smoothValue = (filterValue.TupleSum() / filterValue.Length).D;
            }
            else
            {
                HTuple sigma = Math.Abs((filterValue.Length - 2) / 8.0);
                if (sigma.D < 0.01)
                    sigma = 0.01;
                HOperatorSet.CreateFunct1dArray(filterValue, out function);
                HOperatorSet.SmoothFunct1dGauss(function, sigma, out smoothFunction);
                HOperatorSet.Funct1dToPairs(smoothFunction, out smooth_x, out smooth_y);
                smoothValue = (smooth_y.TupleSum() / smooth_y.Length).D;
            }
        }

        public void GetHeightValueOnWindow(HTuple HalconWindow, HTuple camPara, HTuple camPose, HTuple row, HTuple col, out HTuple grayValue)
        {
            try
            {
                HTuple button = 0;
                grayValue = 0;
                HTuple homMat3D;
                HTuple x, y, z;
                HOperatorSet.GetDispObjectModel3dInfo(HalconWindow, row, col, "depth", out z);
                HOperatorSet.PoseToHomMat3d(camPose, out homMat3D);
                if (z.D == -1)
                    grayValue = -1.0;
                else
                {
                    HOperatorSet.ImagePointsToWorldPlane(camPara, camPose, row, col, 1, out x, out y);
                    grayValue = new HTuple(x[0], y[0], z[0], -1, -1);
                }
                //HOperatorSet.AffineTransPoint3d(homMat3D, 0, 0, z, out x, out y, out grayValue);
            }
            catch (Exception e)
            {
                grayValue = -1.0;
            }
        }
        public void GetGrayValueOnWindow(HTuple HalconWindow, HObject image, HTuple row, HTuple col, out HTuple grayValue)
        {
            //grayValue = -1.0;
            //if (!(image is HImage)) return;
            //HTuple Value = new HTuple(-1, -1, -1);
            //HTuple r1, c1, r2, c2, scaleR, scaleC, width, height;
            //HOperatorSet.GetWindowExtents(HalconWindow, out r1, out c1, out width, out height);
            //HOperatorSet.GetPart(HalconWindow, out r1, out c1, out r2, out c2);
            //try
            //{
            //    scaleR = (r2 - r1) / (height * 1.0); // 图像部分尺寸除以窗口尺寸,得到图像与窗口的比例，将窗口中的行列坐标 * 比例即可得到图像中的像素坐标
            //    scaleC = (c2 - c1) / (width * 1.0);
            //    // 图像的左上角相对于图像部分偏移一个r1、c1,所以这里要加上一个偏移量
            //    HOperatorSet.GetGrayval(image, (row * scaleR).TupleInt() + r1, (col * scaleC).TupleInt() + c1, out Value);
            //    grayValue = new HTuple((row * scaleR).TupleInt() + r1, (col * scaleC).TupleInt() + c1, Value);
            //}
            //catch (Exception e)
            //{
            //    scaleR = 1.0;
            //    scaleC = 1.0;
            //    grayValue = new HTuple((row * scaleR).TupleInt() + r1, (col * scaleC).TupleInt() + c1, Value);
            //}
            grayValue = -1.0;
            if (!(image is HImage)) return;
            HTuple Value = new HTuple(-1, -1, -1);
            HTuple button, row1, col1;
            try
            {
                HOperatorSet.GetMpositionSubPix(HalconWindow, out row1, out col1, out button);
                HOperatorSet.GetGrayval(image, row1, col1, out Value);
                grayValue = new HTuple(Math.Round(row1.D, 2), Math.Round(col1.D, 2), Math.Round(Value.D, 2));
            }
            catch (Exception ex)
            {
                grayValue = new HTuple("-");
            }

        }

        #endregion

        #region 控件随窗体尺寸缩放代码

        private void Form1_Resize(object sender, EventArgs e)
        {
            string[] mytag = ((Form)sender).Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
            float newx = (((Form)sender).Width) / Convert.ToSingle(mytag[0]); //窗体宽度缩放比例
            float newy = ((Form)sender).Height / Convert.ToSingle(mytag[1]);//窗体高度缩放比例
            ResetControlsSize((Form)sender, newx, newy);//随窗体改变控件大小
        }

        /// <summary>
        /// 放在窗体加载函数里，用于记录初始的控件信息
        /// </summary>
        /// <param name="cons"></param>
        private void SetTag(Control cons)
        {
            // 先记录容器的相关数据
            cons.Tag = cons.Width + ":" + cons.Height;
            //遍历窗体中的控件
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    SetTag(con);
            }
        }

        /// <summary>
        /// 放在窗体变化的事件方法里，当窗体尺寸发生变化时，重置控件的尺寸
        /// </summary>
        /// <param name="cons"></param>
        /// <param name="newx"></param>
        /// <param name="newy"></param>
        private void ResetControlsSize(Control cons, float newx, float newy)
        {
            string[] mytag;
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                float value = Convert.ToSingle(mytag[0]) * newx;//宽度,根据窗体缩放比例确定控件的值
                con.Width = (int)value;//宽度
                value = Convert.ToSingle(mytag[1]) * newy;//高度
                con.Height = (int)(value);
                value = Convert.ToSingle(mytag[2]) * newx;//左边距离
                con.Left = (int)(value);
                value = Convert.ToSingle(mytag[3]) * newy;//上边缘距离
                con.Top = (int)(value);
                Single currentSize = Convert.ToSingle(mytag[4]) * newy;//字体大小
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    ResetControlsSize(con, newx, newy);
                }
            }
        }

        #endregion

        /// <summary>
        /// 生成高斯滤波掩膜
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public HMatrix GenGaussMask(double sigma, int width, int height)
        {
            if (width < 0) throw new ArgumentException("width 值不能小于 0 ");
            if (height < 0) throw new ArgumentException("height 值不能小于 0 ");
            int result1 = 0, result2 = 0, w, h;
            Math.DivRem(width, 2, out result1);
            Math.DivRem(height, 2, out result2);
            if (result1 == 0) w = width + 1;
            else w = width;
            if (result2 == 0) h = height + 1;
            else h = height;
            HMatrix hMatrix = new HMatrix(w, h, 0.0);
            int center_h = (h - 1) / 2;
            int center_w = (w - 1) / 2;
            double sum = 0.0;
            double x, y;
            for (int i = 0; i < h; ++i) // i，j 以中心为原点的坐标位置
            {
                y = Math.Pow(i - center_h, 2);
                for (int j = 0; j < w; ++j)
                {
                    x = Math.Pow(j - center_w, 2);
                    //因为最后都要归一化的，常数部分可以不计算，也减少了运算量(常数部分即为：2* Math.Pi * sigma * sigma
                    double g = Math.Exp(-(x + y) / (2 * sigma * sigma));
                    hMatrix[i, j] = g;
                    sum += g;
                }
            }
            for (int i = 0; i < h; ++i)
            {
                for (int j = 0; j < w; ++j)
                {
                    hMatrix[i, j] = hMatrix[i, j] / sum;
                }
            }
            return hMatrix;
        }

        /// <summary>
        /// 生成均值掩膜
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public HMatrix GenMeanMask(int width, int height)
        {
            if (width < 0) throw new ArgumentException("width 值不能小于 0 ");
            if (height < 0) throw new ArgumentException("height 值不能小于 0 ");
            int result1 = 0, result2 = 0, w, h;
            Math.DivRem(width, 2, out result1);
            Math.DivRem(height, 2, out result2);
            if (result1 == 0) w = width + 1;
            else w = width;
            if (result2 == 0) h = height + 1;
            else h = height;
            HMatrix hMatrix = new HMatrix(w, h, 0.0);
            for (int i = 0; i < h; ++i) // i，j 以中心为原点的坐标位置
            {
                for (int j = 0; j < w; ++j)
                {
                    hMatrix[i, j] = 1 / (width * height);
                }
            }
            return hMatrix;
        }

        /// <summary>
        /// 线性插值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="sampleDist"></param>
        /// <param name="inter_x"></param>
        /// <param name="inter_y"></param>
        public void LinearInterpolation(double[] x, double[] y, double sampleDist, out double[] inter_x, out double[] inter_y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            if (x.Length != y.Length) throw new ArgumentNullException("xy元素个数不相等");
            inter_x = new double[x.Length];
            inter_y = new double[x.Length];
            inter_x[x.Length - 1] = x[x.Length - 1]; // 将最后一个值填充
            inter_y[x.Length - 1] = y[x.Length - 1];
            double step = 0, k, b; ;
            if (x[x.Length - 1] > x[0])
                step = sampleDist;
            else
                step = sampleDist * -1;
            //////////////////////////
            for (int i = 0; i < x.Length - 1; i++)
            {
                k = (y[i + 1] - y[i]) / (x[i + 1] - x[i]);
                b = y[i] - k * x[i];
                inter_y[i] = k * (x[i] + step * i) + b;
                inter_x[i] = x[i] + step * i;
            }

        }

        /// <summary>
        ///获取指定位置处的Y值，要求数据都为升序或都为降序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x"></param>
        public double GetYValueFunct1d(double[] X, double[] Y, double x)
        {
            if (X == null) throw new ArgumentNullException("X");
            if (Y == null) throw new ArgumentNullException("Y");
            if (X.Length != Y.Length) throw new ArgumentException("X/Y元素个数不相等");
            if (Y.Length < 2) throw new ArgumentException("长度不能小于2");
            double k = 0, b = 0, y = 0;
            //////////////////////////
            for (int i = 0; i < X.Length - 1; i++)
            {
                // X在两点之间
                if (x > X[i] && x < X[i + 1])
                {
                    k = (Y[i + 1] - Y[i]) / (X[i + 1] - X[i]);
                    b = Y[i] - k * X[i];
                    y = k * x + b;
                }
                // x 在第一个点之外
                if (x < X[0])
                {
                    k = (Y[1] - Y[0]) / (X[1] - X[0]);
                    b = Y[0] - k * X[0];
                    y = k * x + b;
                }
                if (x > X[X.Length - 1])
                {
                    k = (Y[Y.Length - 2] - Y[Y.Length - 1]) / (X[X.Length - 2] - X[X.Length - 1]);
                    b = Y[X.Length - 1] - k * X[X.Length - 1];
                    y = k * x + b;
                }
            }
            return y;
        }

        /// <summary>
        /// 线性插值3D点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="dataWidth"></param>
        /// <param name="dataHeight"></param>
        /// <param name="sampleDist"></param>
        /// <param name="InterpolationAxis"></param>
        /// <param name="sample_x"></param>
        /// <param name="sample_y"></param>
        /// <param name="sample_z"></param>
        public void LinearInterpolationPoint3D(double[] x, double[] y, double[] z, int dataWidth, int dataHeight, double sampleDist, string InterpolationAxis, double[] sample_x, double[] sample_y, double[] sample_z)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            if (x.Length != y.Length) throw new ArgumentNullException("xy元素个数不相等");
            if (x.Length != z.Length) throw new ArgumentNullException("xz元素个数不相等");
            ////////////////////////////////
            double[] hTuple_x, hTuple_y, hTuple_z, x_value, y_value, z_value;
            sample_x = new double[dataWidth * dataHeight];
            sample_y = new double[dataWidth * dataHeight];
            sample_z = new double[dataWidth * dataHeight];
            /////////////////////////////////////////
            switch (InterpolationAxis)
            {
                case "x":
                case "X":
                case "x轴":
                case "X轴":
                    /// 插值X方向
                    hTuple_x = new double[dataWidth];
                    hTuple_z = new double[dataWidth];
                    for (int i = 0; i < dataHeight; i++)
                    {
                        for (int j = 0; j < dataWidth; j++)
                        {
                            hTuple_x[j] = x[i * dataWidth + j];
                            hTuple_z[j] = z[i * dataWidth + j];
                        }
                        //////////////////////////////////// 线性插值
                        LinearInterpolation(hTuple_x, hTuple_z, sampleDist, out x_value, out z_value);
                        for (int j = 0; j < dataWidth; j++)
                        {
                            sample_x[i * dataWidth + j] = x_value[j];
                            sample_z[i * dataWidth + j] = z_value[j];
                            // 当仅插值X方向时，Y复制为原来的值,保存不变
                            sample_y[i * dataWidth + j] = y[i * dataWidth + j];
                        }
                    }
                    break;

                case "y":
                case "Y":
                case "y轴":
                case "Y轴":
                default:
                    ///////////////////// 插值Y方向
                    hTuple_y = new double[dataHeight];
                    hTuple_z = new double[dataHeight];
                    for (int i = 0; i < dataWidth; i++)
                    {
                        for (int j = 0; j < dataHeight; j++)
                        {
                            hTuple_y[j] = y[j * dataWidth + i];
                            hTuple_z[j] = z[j * dataWidth + i];
                        }
                        /////////////////////////////////////
                        LinearInterpolation(hTuple_y, hTuple_z, sampleDist, out y_value, out z_value);
                        for (int j = 0; j < dataHeight; j++)
                        {
                            sample_y[j * dataWidth + i] = y_value[j];
                            sample_z[j * dataWidth + i] = z_value[j];
                            // 当仅插值Y方向时，X复制为原来的值,保存不变
                            sample_x[j * dataWidth + i] = x[j * dataWidth + i];
                        }
                    }
                    break;
                case "xy":
                case "XY":
                case "xy轴":
                case "XY轴":
                    /// 插值X方向
                    hTuple_x = new double[dataWidth];
                    hTuple_z = new double[dataWidth];
                    for (int i = 0; i < dataHeight; i++)
                    {
                        for (int j = 0; j < dataWidth; j++)
                        {
                            hTuple_x[j] = x[i * dataWidth + j];
                            hTuple_z[j] = z[i * dataWidth + j];
                        }
                        //////////////////////////////////// 线性插值
                        LinearInterpolation(hTuple_x, hTuple_z, sampleDist, out x_value, out z_value);
                        for (int j = 0; j < dataWidth; j++)
                        {
                            sample_x[i * dataWidth + j] = x_value[j];
                            sample_z[i * dataWidth + j] = z_value[j];
                        }
                    }
                    ///////////////////// 插值Y方向
                    hTuple_y = new double[dataHeight];
                    hTuple_z = new double[dataHeight];
                    for (int i = 0; i < dataWidth; i++)
                    {
                        for (int j = 0; j < dataHeight; j++)
                        {
                            hTuple_y[j] = y[j * dataWidth + i];
                            hTuple_z[j] = z[j * dataWidth + i];
                        }
                        /////////////////////////////////////
                        LinearInterpolation(hTuple_y, hTuple_z, sampleDist, out y_value, out z_value);
                        for (int j = 0; j < dataHeight; j++)
                        {
                            sample_y[j * dataWidth + i] = y_value[j];
                            sample_z[j * dataWidth + i] = z_value[j];
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 用于生成3D点云的X坐标
        /// </summary>
        /// <param name="dataWidth"></param>
        /// <param name="dataHeight"></param>
        /// <param name="gapDist"></param>
        /// <param name="value"></param>
        public void GenPointCloudMatrix_X(int dataWidth, int dataHeight, double gapDist, out double[] value)
        {
            value = new double[dataWidth * dataHeight];
            for (int i = 0; i < dataHeight; i++)
            {
                for (int j = 0; j < dataWidth; j++)
                {
                    value[i * dataWidth + j] = j * gapDist;
                }
            }
        }

        /// <summary>
        /// 用于生成3D点云的Y坐标
        /// </summary>
        /// <param name="dataWidth"></param>
        /// <param name="dataHeight"></param>
        /// <param name="gapDist"></param>
        /// <param name="value"></param>
        public void GenPointCloudMatrix_Y(int dataWidth, int dataHeight, double gapDist, out double[] value)
        {
            value = new double[dataWidth * dataHeight];
            for (int i = 0; i < dataHeight; i++)
            {
                for (int j = 0; j < dataWidth; j++)
                {
                    value[i * dataWidth + j] = i * gapDist;
                }
            }
        }

        /// <summary>
        /// 用于生成3D点去的Z坐标
        /// </summary>
        /// <param name="lenght"></param>
        /// <param name="refValue"></param>
        /// <param name="value"></param>
        public void GenPointCloudMatrix_Z(int lenght, double refValue, out double[] value)
        {
            value = new double[lenght];
            for (int i = 0; i < lenght; i++)
            {
                value[i] = refValue;
            }
        }
        public HImage GenRealImage(int imageWidht, int imageHeight, double[] value)
        {
            HImage hImage = new HImage();
            float[] temData = new float[value.Length];
            for (int i = 0; i < value.Length; i++)
                temData[i] = (float)value[i];
            IntPtr intPtr = IntPtr.Zero;
            try
            {
                intPtr = Marshal.AllocHGlobal(imageWidht * imageHeight * sizeof(float));
                Marshal.Copy(temData, 0, intPtr, imageWidht * imageHeight);
                hImage = new HImage("real", imageWidht, imageHeight, intPtr);   // real :图像类型为每像素 4 字节
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }
            return hImage;
        }
        public HImage GenRealImage(int imageWidht, int imageHeight, float[] value)
        {
            HImage hImage = new HImage();
            IntPtr intPtr = IntPtr.Zero;
            try
            {
                intPtr = Marshal.AllocHGlobal(imageWidht * imageHeight * sizeof(float));
                Marshal.Copy(value, 0, intPtr, imageWidht * imageHeight);
                hImage = new HImage("real", imageWidht, imageHeight, intPtr);   // real :图像类型为每像素 4 字节
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }
            return hImage;
        }


        public HImage Mean_n(HImage Image, int MaskWidth, int MaskHeight, int noiseValue)
        {
            HImage ImageMean = null;
            IntPtr intPtr = IntPtr.Zero;
            IntPtr intPtrDest = IntPtr.Zero;
            try
            {
                int width, height, verticalPitch, horizontalBitPitch, bitsPerPixel, result1, result2;
                HTuple type = Image.GetImageType();
                if (type.S != "byte")
                {
                    throw new ArgumentException("Image 类型异常，只接受字节类型图像");
                }
                intPtr = Image.GetImagePointer1Rect(out width, out height, out verticalPitch, out horizontalBitPitch, out bitsPerPixel);
                byte[] sourceValue = new byte[width * height];
                byte[] destValue = new byte[width * height];
                Marshal.Copy(intPtr, sourceValue, 0, width * height);
                List<int> list = new List<int>();
                int indexRow = 0, indexCol = 0; // 获取掩膜内的值时使用的行列索引
                Math.DivRem(MaskWidth, 2, out result1);
                Math.DivRem(MaskHeight, 2, out result2);
                if (result1 == 0) MaskWidth += 1;
                if (result2 == 0) MaskHeight += 1;  // 掩膜只能为奇数
                // 遍历像素求和
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++) //  表示遍历第 i 行，j 列的位置
                    {
                        list.Clear();
                        #region 遍历掩膜内的值
                        for (int k = (int)((MaskHeight - 1) * -0.5); k < (int)((MaskHeight + 1) * 0.5); k++)
                        {
                            //////  行镜像处理  ///////////
                            if (i + k < 0 || i + k > height - 1)
                                indexRow = i - k;
                            else
                                indexRow = i + k;
                            //////////////////////////
                            for (int m = (int)((MaskWidth - 1) * -0.5); m < (int)((MaskWidth + 1) * 0.5); m++)
                            {
                                //////  列镜像处理  ///////////
                                if (j + m < 0 || j + m > width - 1)
                                    indexCol = j - m;
                                else
                                    indexCol = j + m;
                                ////////////////////////// 
                                list.Add(sourceValue[indexRow * width + indexCol]);
                            }
                        }
                        #endregion

                        ///// 获取掩膜内的值求平均值，在这里可对获取的数据作滤波处理
                        destValue[i * width + j] = (byte)(list.Average() * 1.5);
                    }
                }
                ////////////////////////////////  将目标值复制到新图像中
                intPtrDest = Marshal.AllocHGlobal(width * height * sizeof(byte));
                Marshal.Copy(destValue, 0, intPtrDest, width * height);
                ImageMean = new HImage("byte", width, height, intPtrDest);   // real :图像类型为每像素 4 字节
            }
            catch (Exception ex)
            {
            }
            finally
            {
                //Marshal.FreeHGlobal(intPtr);
                Marshal.FreeHGlobal(intPtrDest);
            }
            return ImageMean;
        }


        /// <summary>
        /// 使用不共线的至少三点计算拟合圆的圆心,有两个圆心，手动选择
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="Cols"></param>
        /// <param name="centerRow"></param>
        /// <param name="centerCol"></param>
        /// <summary>
        /// 有两个圆心，手动选择
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="Cols"></param>
        /// <param name="stepAngle"></param>
        /// <param name="centerRow"></param>
        /// <param name="centerCol"></param>
        /// <param name="centerRow2"></param>
        /// <param name="centerCol2"></param>
        public void CalculateCenter(double[] Rows, double[] Cols, double stepAngle, out double centerRow, out double centerCol, out double centerRow2, out double centerCol2)
        {
            if (Rows.Length < 2)
                throw new ArgumentException("元素个数不能小于2");
            if (Rows.Length != Cols.Length)
                throw new ArgumentException("输入元素个数不相等");
            double line1Row1, line1Row2, line1Col1, line1Col2;
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> listRow2 = new List<double>();
            List<double> listCol2 = new List<double>();
            List<double> radius = new List<double>();
            HTuple row, col;
            for (int i = 0; i < Rows.Length - 1; i++)
            {
                NormalLine(Rows[i], Cols[i], Rows[i + 1], Cols[i + 1], out line1Row1, out line1Col1, out line1Row2, out line1Col2);
                double dist = HMisc.DistancePp(Rows[i], Cols[i], Rows[i + 1], Cols[i + 1]);
                double R = dist * 0.5 / Math.Sin(stepAngle * 0.5 * Math.PI / 180.0);
                radius.Add(R);
                HOperatorSet.IntersectionLineCircle(line1Row1, line1Col1, line1Row2, line1Col2, (Rows[i] + Rows[i + 1]) * 0.5, (Cols[i] + Cols[i + 1]) * 0.5,
                R, 0, 6.28318, "positive", out row, out col);
                if (row.Length > 0)
                {
                    if (row.Length > 0)
                    {
                        listRow.Add(row[0]);
                        listCol.Add(col[0]);
                    }
                    if (row.Length > 1)
                    {
                        listRow2.Add(row[1]);
                        listCol2.Add(col[1]);
                    }
                }
            }
            centerRow = listRow.Average();
            centerCol = listCol.Average();
            centerRow2 = listRow2.Average();
            centerCol2 = listCol2.Average();
        }
        private void NormalLine(double Row1, double Column1, double Row2, double Column2, out double lineRow1, out double lineCol1, out double lineRow2, out double lineCol2)
        {
            double mid_row = (Row1 + Row2) * 0.5;
            double mid_col = (Column1 + Column2) * 0.5;
            double ATan = Math.Atan2(Row1 - Row2, Column1 - Column2);
            double k = 1 / Math.Tan(ATan) * -1;
            double b = mid_row - mid_col * k;
            ///////////////////
            double y = (mid_col + 1) * k + b;
            lineRow1 = mid_row;
            lineCol1 = mid_col;
            lineRow2 = y;
            lineCol2 = mid_col + 1;
        }


        private void Clear(HTuple objectModel3D)
        {
            HTuple num = 0;
            HTuple value = 0;
            if (objectModel3D == null) return;
            try
            {
                HOperatorSet.GetObjectModel3dParams(objectModel3D, "num_points", out num);
                HOperatorSet.GetObjectModel3dParams(objectModel3D, "has_primitive_data", out value); //基本体中不包含点
                if (num > 0 || value == "true")
                    HOperatorSet.ClearObjectModel3d(objectModel3D);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 排序数组对
        /// </summary>
        /// <param name="T1"></param>
        /// <param name="T2"></param>
        /// <param name="SortMode"></param>
        /// <param name="Sorted1"></param>
        /// <param name="Sorted2"></param>
        public void sortPairs(HTuple T1, HTuple T2, HTuple SortMode, out HTuple Sorted1, out HTuple Sorted2)
        {
            HTuple hv_Indices1 = new HTuple(), hv_Indices2 = new HTuple();
            // Initialize local and output iconic variables 
            Sorted1 = new HTuple();
            Sorted2 = new HTuple();
            //
            if ((int)((new HTuple(SortMode.TupleEqual("1"))).TupleOr(new HTuple(SortMode.TupleEqual(
                1)))) != 0)
            {
                HOperatorSet.TupleSortIndex(T1, out hv_Indices1);
                Sorted1 = T1.TupleSelect(hv_Indices1);
                Sorted2 = T2.TupleSelect(hv_Indices1);
            }
            else if ((int)((new HTuple((new HTuple(SortMode.TupleEqual("column"))).TupleOr(
                new HTuple(SortMode.TupleEqual("2"))))).TupleOr(new HTuple(SortMode.TupleEqual(
                2)))) != 0)
            {
                HOperatorSet.TupleSortIndex(T2, out hv_Indices2);
                Sorted1 = T1.TupleSelect(hv_Indices2);
                Sorted2 = T2.TupleSelect(hv_Indices2);
            }

            return;
        }

        public bool IsContainPoint(HTuple objectModel3D)
        {
            HTuple num = 0;
            HTuple value = 0;
            try
            {
                if (objectModel3D != null)
                {
                    HOperatorSet.GetObjectModel3dParams(objectModel3D, "num_points", out num);
                    HOperatorSet.GetObjectModel3dParams(objectModel3D, "has_primitive_data", out value);
                    if (num > 0 || value == "true") return true;
                }
            }
            catch (Exception e)
            {
            }
            return false;
        }

        #region 变换结构体

        public void AffinePoint3D(userWcsRectangle2 rect2, HTuple transPose3D, out userWcsRectangle2 rect)
        {
            HTuple Qx = new HTuple(0);
            HTuple Qy = new HTuple(0);
            HTuple Qz = new HTuple(0);
            HTuple X = new HTuple(rect2.X);
            HTuple Y = new HTuple(rect2.Y);
            HTuple Z = new HTuple(rect2.Z);
            HTuple homMat3D = null;
            if (transPose3D != null && transPose3D.Length == 7)
                HOperatorSet.PoseToHomMat3d(transPose3D, out homMat3D);
            else
                HOperatorSet.HomMat3dIdentity(out homMat3D);
            if (X != null && X.Length > 0)
            {
                //if (Z == null || Z.Length != X.Length)
                HOperatorSet.TupleGenConst(X.Length, 0, out Z);
                HOperatorSet.AffineTransPoint3d(homMat3D, X, Y, Z, out Qx, out Qy, out Qz);
            }
            rect = new userWcsRectangle2(Qx[0].D, Qy[0].D, rect2.Z, rect2.Deg + transPose3D[5].D, rect2.Length1, rect2.Length2);
        }
        public void AffinePoint3D(userWcsCircle circle1, HTuple transPose3D, out userWcsCircle circle)
        {
            HTuple Qx = new HTuple(0);
            HTuple Qy = new HTuple(0);
            HTuple Qz = new HTuple(0);
            HTuple X = new HTuple(circle1.X);
            HTuple Y = new HTuple(circle1.Y);
            HTuple Z = new HTuple(circle1.Z);
            HTuple homMat3D = null;
            if (transPose3D != null && transPose3D.Length == 7)
                HOperatorSet.PoseToHomMat3d(transPose3D, out homMat3D);
            else
                HOperatorSet.HomMat3dIdentity(out homMat3D);
            if (X != null && X.Length > 0)
            {
                //if (Z == null || Z.Length != X.Length)
                HOperatorSet.TupleGenConst(X.Length, 0, out Z);
                HOperatorSet.AffineTransPoint3d(homMat3D, X, Y, Z, out Qx, out Qy, out Qz);
            }
            circle = new userWcsCircle(Qx[0].D, Qy[0].D, circle1.Z, circle1.Radius, null, null); // 变换半径不会变
        }
        public void AffinePoint3D(userWcsLine line1, HTuple transPose3D, out userWcsLine line)
        {
            HTuple Qx = new HTuple(0, 0);
            HTuple Qy = new HTuple(0, 0);
            HTuple Qz = new HTuple(0, 0);
            HTuple X = new HTuple(line1.X1, line1.X2);
            HTuple Y = new HTuple(line1.Y1, line1.Y2);
            HTuple Z = new HTuple(line1.Z1, line1.Z2);
            HTuple homMat3D = null;
            if (transPose3D != null && transPose3D.Length == 7)
                HOperatorSet.PoseToHomMat3d(transPose3D, out homMat3D);
            else
                HOperatorSet.HomMat3dIdentity(out homMat3D);
            if (X != null && X.Length > 0)
            {
                //if (Z == null || Z.Length != X.Length)
                HOperatorSet.TupleGenConst(X.Length, 0, out Z);
                HOperatorSet.AffineTransPoint3d(homMat3D, X, Y, Z, out Qx, out Qy, out Qz);
            }
            line = new userWcsLine(Qx[0].D, Qy[0].D, line1.Z1, Qx[1].D, Qy[1].D, line1.Z2); // 变换半径不会变
        }

        #endregion

        /// <summary>
        /// 根据向量角度求平台的平移量和旋转量
        /// </summary>
        /// <param name="currectVector">金手指坐标角度</param>
        /// <param name="referenceVector">胶带的坐标角度</param>
        /// <param name="addVector">贴胶的坐标角度</param>
        public userWcsVector VectorAngleToMotionXYTh(userWcsVector currectVector, userWcsVector referenceVector)
        {
            userWcsVector addVector = new userWcsVector();
            HTuple HomMat = new HTuple();
            HOperatorSet.HomMat2dIdentity(out HomMat);
            HOperatorSet.VectorAngleToRigid(currectVector.X, currectVector.Y, currectVector.Angle, referenceVector.X, referenceVector.Y, referenceVector.Angle, out HomMat);  //根据坐标角度计算仿射变换矩阵
            HTuple Sx = new HTuple(), Sy = new HTuple(), Phi = new HTuple(), Theta = new HTuple(), Tx = new HTuple(), Ty = new HTuple();
            HOperatorSet.HomMat2dToAffinePar(HomMat, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);//根据仿射变换矩阵计算平台的XYU轴的平移量
            addVector.X = Tx.D;
            addVector.Y = Ty.D;
            addVector.Angle = Phi.TupleDeg().D;
            return addVector;
        }

        public userWcsVector VectorAngleToMotionXYTheta(userWcsVector currectVector, userWcsVector referenceVector)
        {
            double Sx, Sy, Phi, Theta, Tx, Ty;
            userWcsVector addVector = new userWcsVector();
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(currectVector.X, currectVector.Y, currectVector.Angle, referenceVector.X, referenceVector.Y, referenceVector.Angle);
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            addVector.X = Tx;
            addVector.Y = Ty;
            addVector.Angle = Phi * 180 / Math.PI;
            return addVector;
        }

        public userWcsVector VectorPointToMotionXYTheta(userWcsVector[] currectVector, userWcsVector[] referenceVector)
        {
            double Sx, Sy, Phi, Theta, Tx, Ty;
            userWcsVector addVector = new userWcsVector();
            HHomMat2D hHomMat2D = new HHomMat2D();
            if (currectVector.Length < 2 || referenceVector.Length < 2)
                throw new ArgumentNullException("最小点数不能小于2");
            if (currectVector.Length != referenceVector.Length)
                throw new ArgumentNullException("输入点长度不相等！");
            List<double> list_curx = new List<double>();
            List<double> list_cury = new List<double>();
            List<double> list_refx = new List<double>();
            List<double> list_refy = new List<double>();
            foreach (var item in currectVector)
            {
                list_curx.Add(item.X);
                list_cury.Add(item.Y);
            }
            foreach (var item in referenceVector)
            {
                list_refx.Add(item.X);
                list_refy.Add(item.Y);
            }
            switch (currectVector.Length)
            {
                case 2:
                    hHomMat2D.VectorToSimilarity(list_curx.ToArray(), list_cury.ToArray(), list_refx.ToArray(), list_refy.ToArray());
                    Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
                    addVector.X = Tx;
                    addVector.Y = Ty;
                    addVector.Angle = Phi * 180 / Math.PI;
                    break;
                default:
                    hHomMat2D.VectorToHomMat2d(list_curx.ToArray(), list_cury.ToArray(), list_refx.ToArray(), list_refy.ToArray());
                    Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
                    addVector.X = Tx;
                    addVector.Y = Ty;
                    addVector.Angle = Phi * 180 / Math.PI;
                    break;
            }
            return addVector;
        }

        public userWcsVector PointToMotionXYTheta(userWcsPoint[] currectVector, userWcsPoint[] referenceVector)
        {
            double Sx, Sy, Phi, Theta, Tx, Ty;
            userWcsVector addVector = new userWcsVector();
            HHomMat2D hHomMat2D = new HHomMat2D();
            if (currectVector.Length < 2 || referenceVector.Length < 2)
                throw new ArgumentNullException("最小点数不能小于2");
            if (currectVector.Length != referenceVector.Length)
                throw new ArgumentNullException("输入点长度不相等！");
            List<double> list_curx = new List<double>();
            List<double> list_cury = new List<double>();
            List<double> list_refx = new List<double>();
            List<double> list_refy = new List<double>();
            foreach (var item in currectVector)
            {
                list_curx.Add(item.X);
                list_cury.Add(item.Y);
            }
            foreach (var item in referenceVector)
            {
                list_refx.Add(item.X);
                list_refy.Add(item.Y);
            }
            switch (currectVector.Length)
            {
                case 2:
                    hHomMat2D.VectorToSimilarity(list_curx.ToArray(), list_cury.ToArray(), list_refx.ToArray(), list_refy.ToArray());
                    Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
                    addVector.X = Tx;
                    addVector.Y = Ty;
                    addVector.Angle = Phi * 180 / Math.PI;
                    break;
                default:
                    hHomMat2D.VectorToHomMat2d(list_curx.ToArray(), list_cury.ToArray(), list_refx.ToArray(), list_refy.ToArray());
                    Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
                    addVector.X = Tx;
                    addVector.Y = Ty;
                    addVector.Angle = Phi * 180 / Math.PI;
                    break;
            }
            return addVector;
        }

        public HHomMat2D GetHomMat2D(userWcsPoint[] sourceWcsPoint, userWcsPoint[] targetWcsPoint)
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            if (sourceWcsPoint.Length < 2 || targetWcsPoint.Length < 2)
                throw new ArgumentNullException("最小点数不能小于2");
            if (sourceWcsPoint.Length != targetWcsPoint.Length)
                throw new ArgumentNullException("输入点长度不相等！");
            List<double> list_curx = new List<double>();
            List<double> list_cury = new List<double>();
            List<double> list_refx = new List<double>();
            List<double> list_refy = new List<double>();
            foreach (var item in sourceWcsPoint)
            {
                list_curx.Add(item.X);
                list_cury.Add(item.Y);
            }
            foreach (var item in targetWcsPoint)
            {
                list_refx.Add(item.X);
                list_refy.Add(item.Y);
            }
            switch (sourceWcsPoint.Length)
            {
                case 2:
                    hHomMat2D.VectorToSimilarity(list_curx.ToArray(), list_cury.ToArray(), list_refx.ToArray(), list_refy.ToArray());
                    break;
                default:
                    hHomMat2D.VectorToHomMat2d(list_curx.ToArray(), list_cury.ToArray(), list_refx.ToArray(), list_refy.ToArray());
                    break;
            }
            return hHomMat2D;
        }

        public HHomMat2D GetHomMat2D(userWcsVector[] sourceWcsPoint, userWcsVector[] targetWcsPoint)
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            if (sourceWcsPoint.Length < 2 || targetWcsPoint.Length < 2)
                throw new ArgumentNullException("最小点数不能小于2");
            if (sourceWcsPoint.Length != targetWcsPoint.Length)
                throw new ArgumentNullException("输入点长度不相等！");
            List<double> list_curx = new List<double>();
            List<double> list_cury = new List<double>();
            List<double> list_refx = new List<double>();
            List<double> list_refy = new List<double>();
            foreach (var item in sourceWcsPoint)
            {
                list_curx.Add(item.X);
                list_cury.Add(item.Y);
            }
            foreach (var item in targetWcsPoint)
            {
                list_refx.Add(item.X);
                list_refy.Add(item.Y);
            }
            switch (sourceWcsPoint.Length)
            {
                case 2:
                    hHomMat2D.VectorToSimilarity(list_curx.ToArray(), list_cury.ToArray(), list_refx.ToArray(), list_refy.ToArray());
                    break;
                default:
                    hHomMat2D.VectorToHomMat2d(list_curx.ToArray(), list_cury.ToArray(), list_refx.ToArray(), list_refy.ToArray());
                    break;
            }
            return hHomMat2D;
        }
        public HHomMat2D GetHomMat2D(userWcsVector sourceVector, userWcsVector targetVector)
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            if (sourceVector == null)
                throw new ArgumentNullException("currectVector");
            if (targetVector == null)
                throw new ArgumentNullException("referenceVector");
            hHomMat2D.VectorAngleToRigid(sourceVector.X, sourceVector.Y, sourceVector.Angle, targetVector.X, targetVector.Y, targetVector.Angle);
            return hHomMat2D;
        }

        public HHomMat2D HomMat2dCompose(HHomMat2D leftHomMat2D, HHomMat2D rightHomMat2D)
        {
             HHomMat2D  homMat2DCompose = leftHomMat2D.HomMat2dCompose(rightHomMat2D);
            return homMat2DCompose;
        }
        public userWcsVector GetHomMat2DXYTheta(HHomMat2D hHomMat2D)
        {
            double Sx, Sy, Phi, Theta, Tx, Ty;
            userWcsVector addVector = new userWcsVector();
            if (hHomMat2D == null)
                throw new ArgumentNullException("hHomMat2D");
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            addVector.X = Tx;
            addVector.Y = Ty;
            addVector.Angle = Phi * 180 / Math.PI;
            return addVector;
        }

        public userWcsPoint[] AffineTransPoint2d(userWcsPoint[] wcsPoint, userWcsVector addVector)
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            if (wcsPoint == null)
                throw new ArgumentNullException("wcsPoint");
            List<double> list_curx = new List<double>();
            List<double> list_cury = new List<double>();
            List<double> list_refx = new List<double>();
            List<double> list_refy = new List<double>();
            foreach (var item in wcsPoint)
            {
                list_curx.Add(item.X);
                list_cury.Add(item.Y);
            }
            hHomMat2D.VectorAngleToRigid(0, 0, 0, addVector.X, addVector.Y, addVector.Angle * 180 / Math.PI);
            /////////////////////////////////////////////////////////////////////////////////
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(list_curx.ToArray(), list_cury.ToArray(), out Qy);
            //////////////////////////////
            userWcsPoint[] wcsPoints = new userWcsPoint[Qx.Length];
            for (int i = 0; i < Qx.Length; i++)
            {
                wcsPoints[i] = new userWcsPoint(Qx[i].D, Qy[i].D, 0);
            }
            return wcsPoints;
        }

        public userWcsVector[] AffineTransPoint2d(userWcsVector[] wcsPoint, userWcsVector addVector)
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            if (wcsPoint == null)
                throw new ArgumentNullException("wcsPoint");
            List<double> list_curx = new List<double>();
            List<double> list_cury = new List<double>();
            List<double> list_refx = new List<double>();
            List<double> list_refy = new List<double>();
            foreach (var item in wcsPoint)
            {
                list_curx.Add(item.X);
                list_cury.Add(item.Y);
            }
            hHomMat2D.VectorAngleToRigid(0, 0, 0, addVector.X, addVector.Y, addVector.Angle * 180 / Math.PI);
            /////////////////////////////////////////////////////////////////////////////////
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(list_curx.ToArray(), list_cury.ToArray(), out Qy);
            //////////////////////////////
            userWcsVector[] wcsPoints = new userWcsVector[Qx.Length];
            for (int i = 0; i < Qx.Length; i++)
            {
                wcsPoints[i] = new userWcsVector(Qx[i].D, Qy[i].D, 0,0);
            }
            return wcsPoints;
        }

        public userWcsVector AffineTransPoint2d(HHomMat2D hHomMat2D, userWcsVector wcsPoint)
        {
            if (wcsPoint == null)
                throw new ArgumentNullException("wcsPoint");
            /////////////////////////////////////////////////////////////////////////////////
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(wcsPoint.X, wcsPoint.Y, out Qy);
            //////////////////////////////
            userWcsVector wcsPoints = new userWcsVector(Qx.D, Qy.D, 0.0, 0);
            return wcsPoints;
        }
        public userWcsPoint[] AffineTransPoint2d(HHomMat2D hHomMat2D, userWcsPoint[] wcsPoint)
        {
            if (wcsPoint == null)
                throw new ArgumentNullException("wcsPoint");
            List<double> list_curx = new List<double>();
            List<double> list_cury = new List<double>();
            List<double> list_refx = new List<double>();
            List<double> list_refy = new List<double>();
            foreach (var item in wcsPoint)
            {
                list_curx.Add(item.X);
                list_cury.Add(item.Y);
            }
            /////////////////////////////////////////////////////////////////////////////////
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(list_curx.ToArray(), list_cury.ToArray(), out Qy);
            //////////////////////////////
            userWcsPoint[] wcsPoints = new userWcsPoint[Qx.Length];
            for (int i = 0; i < Qx.Length; i++)
            {
                wcsPoints[i] = new userWcsPoint(Qx[i].D, Qy[i].D, 0);
            }
            return wcsPoints;
        }

        public userWcsVector[] AffineTransPoint2d(HHomMat2D hHomMat2D, userWcsVector[] wcsPoint)
        {
            if (wcsPoint == null)
                throw new ArgumentNullException("wcsPoint");
            List<double> list_curx = new List<double>();
            List<double> list_cury = new List<double>();
            List<double> list_refx = new List<double>();
            List<double> list_refy = new List<double>();
            foreach (var item in wcsPoint)
            {
                list_curx.Add(item.X);
                list_cury.Add(item.Y);
            }
            /////////////////////////////////////////////////////////////////////////////////
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(list_curx.ToArray(), list_cury.ToArray(), out Qy);
            //////////////////////////////
            userWcsVector[] wcsPoints = new userWcsVector[Qx.Length];
            for (int i = 0; i < Qx.Length; i++)
            {
                wcsPoints[i] = new userWcsVector(Qx[i].D, Qy[i].D, 0);
            }
            return wcsPoints;
        }


        public void FindCircleMarkCoord(HImage image, HRegion region, int rowCount, int colCount, double threshold, out HTuple Rows, out HTuple Columns)
        {
            Rows = new HTuple();
            Columns = new HTuple();
            HTuple circleRow, circleColumn, circleRadius, StartPhi, EndPhi, PointOrder, sortRow, sortCol, sortRow2, sortCol2;
            if (image == null)
                throw new ArgumentNullException("image", "输入图像参数为NULL");
            if (threshold < 0)
                throw new ArgumentNullException("Threshold", "输入阈值参数小于0");
            // 计算像素位置点
            HXLDCont circle = image.ReduceDomain(region).ThresholdSubPix(threshold).UnionCocircularContoursXld(0.5, 0.1, 0.2, 30, 10, 10, "true", 3); //              
            HTuple length = circle.LengthXld();
            circle = circle.SelectShapeXld("contlength", "and", length.TupleMax().D * 0.66, length.TupleMax().D * 1.05);
            circle.FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out circleRow, out circleColumn, out circleRadius, out StartPhi, out EndPhi, out PointOrder);
            circle.GenCircleContourXld(circleRow, circleColumn, circleRadius, StartPhi, EndPhi, PointOrder, 0.001);
            this.sortPairs(circleRow, circleColumn, 1, out sortRow, out sortCol);// 行排序
            if (rowCount * colCount != sortRow.Length)
                throw new ArgumentException("提取的圆数量与指定的圆数量不相等，请调整参数");
            for (int i = 0; i < rowCount; i++)
            {
                // 列排序
                this.sortPairs(sortRow.TupleSelectRange(i * colCount, (i + 1) * colCount - 1), sortCol.TupleSelectRange(i * colCount, (i + 1) * colCount - 1), 2, out sortRow2, out sortCol2);
                Rows.Append(sortRow2);
                Columns.Append(sortCol2);
            }

        }

        #region  方法



        public static bool CalculateProjectionPoint(userWcsPoint Point, userWcsLine Line, out userWcsPoint proPoint)
        {
            bool result = false;
            if (Point == null)
            {
                throw new ArgumentNullException("Point");
            }
            if (Line == null)
            {
                throw new ArgumentNullException("Line");
            }
            double Proj_y, Proj_x;
            /////////////////////////////////
            HMisc.ProjectionPl(Point.Y, Point.X, Line.Y1, Line.X1, Line.Y2, Line.X2, out Proj_y, out Proj_x);
            proPoint = new userWcsPoint(Proj_x, Proj_y, Point.Z, Point.CamParams);
            proPoint.Grab_x = Point.Grab_x;
            proPoint.Grab_y = Point.Grab_y;
            proPoint.Grab_theta = Point.Grab_theta;
            proPoint.CamName = Point.CamName;
            proPoint.ViewWindow = Point.ViewWindow;
            proPoint.Tag = Point.Tag;
            result = true;
            return result;
        }

        public static bool CalculateLineToLineDist2D(userWcsLine Line1, userWcsLine Line2, out double _maxDist, out double _minDist, out double _meanDist, out userWcsLine distLine)
        {
            bool result = false;
            if (Line1 == null)
            {
                throw new ArgumentNullException("Line1");
            }
            if (Line2 == null)
            {
                throw new ArgumentNullException("Line2");
            }
            double Proj_y, Proj_x;
            ///////////////////////////////////////////////////
            HMisc.ProjectionPl((Line1.Y1 + Line1.Y2) * 0.5, (Line1.X1 + Line1.X2) * 0.5, Line2.Y1, Line2.X1, Line2.Y2, Line2.X2, out Proj_y, out Proj_x);
            distLine = new userWcsLine((Line1.X1 + Line1.X2) * 0.5, (Line1.Y1 + Line1.Y2) * 0.5, (Line1.Z1 + Line1.Z2) * 0.5, Proj_x, Proj_y, (Line1.Z1 + Line1.Z2) * 0.5, Line1.CamParams);
            if (Line1.Grab_x == Line2.Grab_x && Line1.Grab_y == Line2.Grab_y)
            {
                distLine.Grab_x = Line1.Grab_x;
                distLine.Grab_y = Line1.Grab_y;
            }
            else
            {
                distLine.Grab_x = 0;
                distLine.Grab_y = 0;
            }
            distLine.Grab_x = Line1.Grab_x;
            distLine.Grab_y = Line1.Grab_y;
            distLine.Grab_theta = Line1.Grab_theta;
            distLine.CamName = Line1.CamName;
            distLine.ViewWindow = Line1.ViewWindow;
            distLine.Tag = Line1.Tag;
            HTuple dist = HMisc.DistancePl(new HTuple(Line1.Y1, Line1.Y2), new HTuple(Line1.X1, Line1.X2), new HTuple(Line2.Y1), new HTuple(Line2.X1), new HTuple(Line2.Y2), new HTuple(Line2.X2));
            _maxDist = dist.TupleMax().D;
            _minDist = dist.TupleMin().D;
            _meanDist = dist.TupleMean().D;

            ///
            result = true;
            return result;
        }
        public static bool CalculatePointToPointDist(userWcsPoint point1, userWcsPoint point2, out double maxDist, out double levelDist, out double verticalDist, out userWcsLine distLine)
        {
            bool result = false;
            if (point1 == null)
            {
                throw new ArgumentNullException("point1");
            }
            if (point2 == null)
            {
                throw new ArgumentNullException("point2");
            }
            ///////////////////////////////////////////////////
            maxDist = Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y) + (point1.Z - point2.Z) * (point1.Z - point2.Z));
            levelDist = Math.Abs(point1.X - point2.X);
            verticalDist = Math.Abs(point1.Y - point2.Y);
            distLine = new userWcsLine(point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, point1.CamParams);
            distLine.CamName = point1.CamName;
            distLine.Grab_x = point1.Grab_x;
            distLine.Grab_y = point1.Grab_y;
            distLine.Grab_theta = point1.Grab_theta;
            distLine.ViewWindow = point1.ViewWindow;
            distLine.Tag = point1.Tag;
            result = true;
            return result;
        }
        public static bool CalculatePointToLineDist2D(userWcsPoint Point, userWcsLine Line, out double distValue, out userWcsLine distLine)
        {
            bool result = false;
            if (Point == null)
            {
                throw new ArgumentNullException("Point");
            }
            if (Line == null)
            {
                throw new ArgumentNullException("Line");
            }
            double Proj_y, Proj_x;
            /////////////////////////////////
            HMisc.ProjectionPl(Point.Y, Point.X, Line.Y1, Line.X1, Line.Y2, Line.X2, out Proj_y, out Proj_x);
            distLine = new userWcsLine(Point.X, Point.Y, Point.Z, Proj_x, Proj_y, Point.Z, Point.CamParams);
            distValue = HMisc.DistancePl(Point.Y, Point.X, Line.Y1, Line.X1, Line.Y2, Line.X2);
            distLine.CamName = Point.CamName;
            distLine.Grab_x = Point.Grab_x;
            distLine.Grab_y = Point.Grab_y;
            distLine.Grab_theta = Point.Grab_theta;
            distLine.ViewWindow = Point.ViewWindow;
            distLine.Tag = Point.Tag;
            result = true;
            return result;
        }
        public static bool CalculateVectorPoint(userWcsPoint Point, userWcsLine Line, out userWcsVector wcsVector)
        {
            bool result = false;
            if (Point == null)
            {
                throw new ArgumentNullException("Point");
            }
            if (Line == null)
            {
                throw new ArgumentNullException("Line");
            }
            /////////////////////////////////
            wcsVector = new userWcsVector(Point.X, Point.Y, Point.Z,0, Point.CamParams);
            wcsVector.Angle = Math.Atan2(Line.Y2 - Line.Y1, Line.X2 - Line.X1) * 180 / Math.PI;
            wcsVector.CamName = Point.CamName;
            wcsVector.Grab_x = Point.Grab_x;
            wcsVector.Grab_y = Point.Grab_y;
            wcsVector.Grab_theta = Point.Grab_theta;
            wcsVector.ViewWindow = Point.ViewWindow;
            wcsVector.Tag = Point.Tag;
            result = true;
            return result;
        }
        public static bool AngleBisector(userWcsLine line1, userWcsLine line2, out userWcsLine line)
        {
            bool result = false;
            HTuple row, col, isOverLap, angle, angle1, angle2, Y2, X2, row1Proj, col1Proj, row2Proj, col2Proj, HomMat2DIdentity, HomMat2DRotate;
            HObject affineLine, ContoursAffinTrans;
            if (line1 == null)
            {
                throw new ArgumentNullException("line1");
            }
            if (line1 == null)
            {
                throw new ArgumentNullException("line2");
            }
            line = new userWcsLine();
            ///////////////////////////////////////////
            HOperatorSet.IntersectionLines(line1.Y1, line1.X1, line1.Y2, line1.X2, line2.Y1, line2.X1, line2.Y2, line2.X2, out row, out col, out isOverLap);
            if (row != null && row.Length > 0) // 表示两直线不完全平行，存在交点
            {
                // 以交点作为角度计算的起点，这样才能保证一致性
                HOperatorSet.AngleLl(row, col, (line1.Y1 + line1.Y2) / 2.0, (line1.X1 + line1.X2) / 2.0, row, col, (line2.Y1 + line2.Y2) / 2.0, (line2.X1 + line2.X2) / 2.0, out angle);
                HOperatorSet.GenContourPolygonXld(out affineLine, new HTuple(line1.Y1, line1.Y2), new HTuple(line1.X1, line1.X2));
                HOperatorSet.HomMat2dIdentity(out HomMat2DIdentity);
                HOperatorSet.HomMat2dRotate(HomMat2DIdentity, angle / 2.0, row, col, out HomMat2DRotate);
                HOperatorSet.AffineTransContourXld(affineLine, out ContoursAffinTrans, HomMat2DRotate);
                HOperatorSet.GetContourXld(ContoursAffinTrans, out Y2, out X2);
                if (Y2 != null && Y2.Length > 0)
                    line = new userWcsLine(X2[0].D, Y2[0].D * 1, 0, X2[1].D, Y2[1].D * 1, 0, line1.CamParams);
                HOperatorSet.AngleLl(Y2[0].D, X2[0].D, Y2[1].D, X2[1].D, row, col, line1.Y1, line1.X1, out angle1);
                HOperatorSet.AngleLl(Y2[0].D, X2[0].D, Y2[1].D, X2[1].D, row, col, line2.Y1, line2.X1, out angle2);
            }
            else // 表示两直线完全平行，不存在交点
            {
                HOperatorSet.ProjectionPl(line1.Y1, line1.X1, line2.Y1, line2.X1, line2.Y2, line2.X2, out row1Proj, out col1Proj);
                HOperatorSet.ProjectionPl(line1.Y2, line1.X2, line2.Y1, line2.X1, line2.Y2, line2.X2, out row2Proj, out col2Proj);
                line = new userWcsLine((col1Proj.D + line1.X1) / 2.0, (row1Proj.D + line1.Y1) / 2.0 * 1, 0, (col2Proj.D + line1.X2) / 2.0, (row2Proj.D + line1.Y2) / 2.0 * 1, 0, line1.CamParams);
            }
            userWcsLine affineline = line.Clone();
            double phi = Math.Atan2(affineline.Y2 - affineline.Y1, affineline.X2 - affineline.X1);
            if (phi >= 0)
                line = new userWcsLine(affineline.X1, affineline.Y1, 0, affineline.X2, affineline.Y2, 0);
            else
                line = new userWcsLine(affineline.X2, affineline.Y2, 0, affineline.X1, affineline.Y1, 0);
            line.CamName = line1.CamName;
            line.Grab_x = line1.Grab_x;
            line.Grab_y = line1.Grab_y;
            line.Grab_theta = line1.Grab_theta;
            line.ViewWindow = line1.ViewWindow;
            line.CamParams = line1.CamParams;
            line.Tag = line1.Tag;
            result = true;
            return result;
        }

        public static bool AngleBisector(userWcsRectangle2 rect2, out userWcsLine line)
        {
            bool result = false;
            if (rect2 == null)
            {
                throw new ArgumentNullException("rect2");
            }
            ///////////////////////////////////////////
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(rect2.X, rect2.Y, rect2.Deg * Math.PI / 180, 0, 0, 0);
            userWcsRectangle2 wcsRect2 = rect2.AffineWcsRectangle2D(hHomMat2D.RawData);
            userWcsLine lineRef = new userWcsLine(-wcsRect2.Length1, 0, 0, wcsRect2.Length1, 0, 0);
            userWcsLine affineline = lineRef.AffineWcsLine2D(hHomMat2D.HomMat2dInvert().RawData);
            double phi = Math.Atan2(affineline.Y2 - affineline.Y1, affineline.X2 - affineline.X1);
            if (phi >= 0)
                line = new userWcsLine(affineline.X1, affineline.Y1, 0, affineline.X2, affineline.Y2, 0);
            else
                line = new userWcsLine(affineline.X2, affineline.Y2, 0, affineline.X1, affineline.Y1, 0);
            line.CamName = rect2.CamName;
            line.Grab_x = rect2.Grab_x;
            line.Grab_y = rect2.Grab_y;
            line.Grab_theta = rect2.Grab_theta;
            line.ViewWindow = rect2.ViewWindow;
            line.CamParams = rect2.CamParams;
            line.Tag = rect2.Tag;
            result = true;
            return result;
        }
        public static bool LineLineAngle(userWcsLine line1, userWcsLine line2, out double deg)
        {
            bool result = false;
            ///////////////////////////
            HTuple angle = 0;
            deg = 0;///
            if (line1 == null)
            {
                throw new ArgumentNullException("line1");
            }
            if (line2 == null)
            {
                throw new ArgumentNullException("line2");
            }
            ///
            HOperatorSet.AngleLl(line1.Y1, line1.X1, line1.Y2, line1.X2, line2.Y1, line2.X1, line2.Y2, line2.X2, out angle);
            if (Math.Abs(angle.TupleDeg().D) > 90)
                deg = 180 - Math.Abs(angle.TupleDeg().D);
            else
                deg = Math.Abs(angle.TupleDeg().D);

            result = true;
            return result;
        }

        public static bool IntersectionPoint(userWcsLine line1, userWcsLine line2, out userWcsPoint points)
        {
            bool result = false;
            HTuple row, column, isOverLap;
            if (line1 == null)
            {
                throw new ArgumentNullException("line1");
            }
            if (line2 == null)
            {
                throw new ArgumentNullException("line2");
            }
            points = new userWcsPoint();
            ///////////////////////////////
            HOperatorSet.IntersectionLines(line1.Y1, line1.X1, line1.Y2, line1.X2, line2.Y1, line2.X1, line2.Y2, line2.X2, out row, out column, out isOverLap);
            if (row != null && row.Length > 0)
            {
                points = new userWcsPoint(column.D, row.D, 0, line1.CamParams);
                if (line1.Grab_x == line2.Grab_x && line1.Grab_y == line2.Grab_y) // 如果两个相等，表示两条边是在同一个视野下拍的图获取的
                {
                    points.Grab_x = line1.Grab_x;
                    points.Grab_y = line1.Grab_y;
                    points.Grab_z = line1.Grab_z;
                    points.Grab_theta = line1.Grab_theta;
                    points.Grab_u = line1.Grab_u;
                    points.Grab_v = line1.Grab_v;
                    points.CamName = line1.CamName;
                    points.ViewWindow = line1.ViewWindow;
                    points.Tag = line1.Tag;
                }
                else
                {
                    points.Grab_x = 0;
                    points.Grab_y = 0;
                    points.Grab_z = line1.Grab_z;
                    points.Grab_theta = line1.Grab_theta;
                    points.Grab_u = line1.Grab_u;
                    points.Grab_v = line1.Grab_v;
                    points.CamName = "NONE";
                    points.ViewWindow = line1.ViewWindow;
                    points.Tag = line1.Tag;
                }
            }
            result = true;
            return result;
        }
        public static bool IntersectionPoint(userWcsLine line, userWcsCircle circle, out userWcsPoint[] wcsPoint)
        {
            bool result = false;
            HTuple row, column;
            if (line == null)
            {
                throw new ArgumentNullException("line");
            }
            if (circle == null)
            {
                throw new ArgumentNullException("circle");
            }
            ////////////////////////// 直线与圆相交
            wcsPoint = new userWcsPoint[2];
            HOperatorSet.IntersectionLineCircle((line).Y1, (line).X1, (line).Y2, (line).X2, (circle).Y, (circle).X, (circle).Radius, 0, Math.PI * 2, "true", out row, out column);
            if (row != null)
            {
                switch (row.Length)
                {
                    case 0:
                        wcsPoint[0] = new userWcsPoint(0, 0, 0, (line).CamParams);
                        wcsPoint[1] = new userWcsPoint(0, 0, 0, (line).CamParams);
                        wcsPoint[0].CamName = line.CamName;
                        wcsPoint[1].CamName = line.CamName;
                        wcsPoint[0].Grab_x = line.Grab_x;
                        wcsPoint[0].Grab_y = line.Grab_y;
                        wcsPoint[0].Grab_z = line.Grab_z;
                        wcsPoint[1].Grab_x = line.Grab_x;
                        wcsPoint[1].Grab_y = line.Grab_y;
                        wcsPoint[1].Grab_z = line.Grab_z;
                        wcsPoint[0].ViewWindow = line.ViewWindow;
                        wcsPoint[1].ViewWindow = line.ViewWindow;
                        wcsPoint[0].Tag = line.Tag;
                        wcsPoint[1].Tag = line.Tag;
                        break;
                    case 1:
                        wcsPoint[0] = new userWcsPoint(column[0].D, row[0].D, 0, (line).CamParams);
                        wcsPoint[1] = new userWcsPoint(column[0].D, row[0].D, 0, (line).CamParams);
                        wcsPoint[0].CamName = line.CamName;
                        wcsPoint[1].CamName = line.CamName;
                        ////////
                        wcsPoint[0].Grab_x = line.Grab_x;
                        wcsPoint[0].Grab_y = line.Grab_y;
                        wcsPoint[0].Grab_z = line.Grab_z;
                        wcsPoint[1].Grab_x = line.Grab_x;
                        wcsPoint[1].Grab_y = line.Grab_y;
                        wcsPoint[1].Grab_z = line.Grab_z;
                        wcsPoint[0].ViewWindow = line.ViewWindow;
                        wcsPoint[1].ViewWindow = line.ViewWindow;
                        wcsPoint[0].Tag = line.Tag;
                        wcsPoint[1].Tag = line.Tag;
                        break;
                    case 2:
                        wcsPoint[0] = new userWcsPoint(column[0].D, row[0].D, 0, (line).CamParams);
                        wcsPoint[1] = new userWcsPoint(column[1].D, row[1].D, 0, (line).CamParams);
                        wcsPoint[0].CamName = line.CamName;
                        wcsPoint[1].CamName = line.CamName;
                        //////////////////////////////
                        wcsPoint[0].Grab_x = line.Grab_x;
                        wcsPoint[0].Grab_y = line.Grab_y;
                        wcsPoint[0].Grab_z = line.Grab_z;
                        wcsPoint[1].Grab_x = line.Grab_x;
                        wcsPoint[1].Grab_y = line.Grab_y;
                        wcsPoint[1].Grab_z = line.Grab_z;
                        wcsPoint[0].ViewWindow = line.ViewWindow;
                        wcsPoint[1].ViewWindow = line.ViewWindow;
                        wcsPoint[0].Tag = line.Tag;
                        wcsPoint[1].Tag = line.Tag;
                        break;
                }
                result = true;
            }
            else
                result = false;
            return result;
        }

        public static bool CalculateCircleToLineDist2D(userWcsCircle Circle, userWcsLine Line, out double circleDist, out double maxDist, out double minDist, out userWcsLine distLine)
        {
            bool result = false;
            if (Circle == null)
            {
                throw new ArgumentNullException("Circle");
            }
            if (Line == null)
            {
                throw new ArgumentNullException("Line");
            }
            double Proj_y, Proj_x;
            /////////////////////////////////
            HMisc.ProjectionPl(Circle.Y, Circle.X, Line.Y1, Line.X1, Line.Y2, Line.X2, out Proj_y, out Proj_x);
            distLine = new userWcsLine(Circle.X, Circle.Y, Circle.Z, Proj_x, Proj_y, Circle.Z, Circle.CamParams);
            distLine.Grab_x = Line.Grab_x;
            distLine.Grab_y = Line.Grab_y;
            circleDist = HMisc.DistancePl(Circle.Y, Circle.X, Line.Y1, Line.X1, Line.Y2, Line.X2);
            maxDist = circleDist + Circle.Radius + Circle.Radius;
            minDist = circleDist - Circle.Radius - Circle.Radius;
            result = true;
            distLine.CamName = Circle.CamName;
            distLine.Grab_x = Circle.Grab_x;
            distLine.Grab_y = Circle.Grab_y;
            distLine.Grab_z = Circle.Grab_z;
            distLine.Grab_theta = Circle.Grab_theta;
            distLine.ViewWindow = Circle.ViewWindow;
            distLine.CamName = Circle.CamName;
            distLine.Tag = Circle.Tag;
            //////////////////////////////////////
            return result;
        }

        public static bool CalculateNPointToLineDist2D(userWcsPoint[] wcsPoint, userWcsLine Line, out double meanDist, out double maxDist, out double minDist, out userWcsPoint minWcsPoint, out userWcsPoint maxWcsPoint)
        {
            bool result = false;
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (Line == null)
            {
                throw new ArgumentNullException("Line");
            }
            if (wcsPoint.Length > 0)
            {
                double[] x = new double[wcsPoint.Length];
                double[] y = new double[wcsPoint.Length];
                for (int i = 0; i < wcsPoint.Length; i++)
                {
                    x[i] = wcsPoint[i].X;
                    y[i] = wcsPoint[i].Y;
                }
                HTuple dist = HMisc.DistancePl(y, x, Line.Y1, Line.X1, Line.Y2, Line.X2);
                HTuple sortIndex = dist.TupleSortIndex();
                List<double> list_x = new List<double>();
                List<double> list_y = new List<double>();
                for (int i = 0; i < 5; i++)
                {
                    if (i < x.Length)
                    {
                        list_x.Add(x[sortIndex[i].I]);
                        list_y.Add(y[sortIndex[i].I]);
                    }
                }
                minWcsPoint = new userWcsPoint(list_x.Average(), list_y.Average(), 0, wcsPoint[0].CamParams);
                minWcsPoint.ViewWindow = wcsPoint[0].ViewWindow;
                minWcsPoint.Tag = wcsPoint[0].Tag;
                minWcsPoint.CamName = wcsPoint[0].CamName;
                minWcsPoint.Grab_x = wcsPoint[0].Grab_x;
                minWcsPoint.Grab_y = wcsPoint[0].Grab_y;
                minWcsPoint.Grab_z = wcsPoint[0].Grab_z;
                minWcsPoint.Grab_theta = wcsPoint[0].Grab_theta;
                minWcsPoint.Grab_u = wcsPoint[0].Grab_u;
                minWcsPoint.Grab_v = wcsPoint[0].Grab_v;
                ////////////////////  计算最大值点 ///////////////////////////////////
                list_x.Clear();
                list_y.Clear();
                for (int i = x.Length - 1; i >= x.Length - 5; i--)
                {
                    if (i >= 0)
                    {
                        list_x.Add(x[sortIndex[i].I]);
                        list_y.Add(y[sortIndex[i].I]);
                    }
                }
                maxWcsPoint = new userWcsPoint(list_x.Average(), list_y.Average(), 0, wcsPoint[0].CamParams);
                maxWcsPoint.ViewWindow = wcsPoint[0].ViewWindow;
                maxWcsPoint.Tag = wcsPoint[0].Tag;
                maxWcsPoint.CamName = wcsPoint[0].CamName;
                maxWcsPoint.Grab_x = wcsPoint[0].Grab_x;
                maxWcsPoint.Grab_y = wcsPoint[0].Grab_y;
                maxWcsPoint.Grab_z = wcsPoint[0].Grab_z;
                maxWcsPoint.Grab_theta = wcsPoint[0].Grab_theta;
                maxWcsPoint.Grab_u = wcsPoint[0].Grab_u;
                maxWcsPoint.Grab_v = wcsPoint[0].Grab_v;
                /////////////////////////////////////////
                maxDist = dist.TupleMax().D;
                minDist = dist.TupleMin().D;
                meanDist = dist.TupleMean().D;
            }
            else
            {
                maxWcsPoint = new userWcsPoint();
                minWcsPoint = new userWcsPoint();
                maxDist = 0;
                minDist = 0;
                meanDist = 0;
            }
            result = true;
            //////////////////////////////////////
            return result;
        }


        public static bool IntersectionPoint(userWcsCircle circle1, userWcsCircle circle2, out userWcsPoint[] wcsPoint)
        {
            bool result = false;
            HTuple row, column, isOverLap;
            if (circle1 == null)
            {
                throw new ArgumentNullException("circle1");
            }
            if (circle2 == null)
            {
                throw new ArgumentNullException("circle2");
            }
            wcsPoint = new userWcsPoint[2];
            //////////////////////////
            HOperatorSet.IntersectionCircles(circle1.Y, circle1.X, circle1.Radius, 0, Math.PI * 2, "true", circle2.Y, circle2.X, circle2.Radius, 0, Math.PI * 2, "true", out row, out column, out isOverLap);
            if (row != null)
            {
                switch (row.Length)
                {
                    case 0:
                        wcsPoint[0] = new userWcsPoint(0, 0, 0, (circle1).CamParams);
                        wcsPoint[1] = new userWcsPoint(0, 0, 0, (circle1).CamParams);
                        wcsPoint[0].CamName = circle1.CamName;
                        wcsPoint[1].CamName = circle1.CamName;
                        wcsPoint[0].Grab_x = circle1.Grab_x;
                        wcsPoint[1].Grab_x = circle1.Grab_x;
                        wcsPoint[0].Grab_y = circle1.Grab_y;
                        wcsPoint[1].Grab_y = circle1.Grab_y;
                        wcsPoint[0].Grab_z = circle1.Grab_z;
                        wcsPoint[1].Grab_z = circle1.Grab_z;
                        wcsPoint[0].Grab_theta = circle1.Grab_theta;
                        wcsPoint[1].Grab_theta = circle1.Grab_theta;
                        wcsPoint[0].ViewWindow = circle1.ViewWindow;
                        wcsPoint[1].ViewWindow = circle1.ViewWindow;
                        wcsPoint[0].Tag = circle1.Tag;
                        wcsPoint[1].Tag = circle1.Tag;
                        break;
                    case 1:
                        wcsPoint[0] = new userWcsPoint(column[0].D, row[0].D, 0, (circle1).CamParams);
                        wcsPoint[1] = new userWcsPoint(column[0].D, row[0].D, 0, (circle1).CamParams);
                        wcsPoint[0].CamName = circle1.CamName;
                        wcsPoint[1].CamName = circle1.CamName;
                        wcsPoint[0].Grab_x = circle1.Grab_x;
                        wcsPoint[1].Grab_x = circle1.Grab_x;
                        wcsPoint[0].Grab_y = circle1.Grab_y;
                        wcsPoint[1].Grab_y = circle1.Grab_y;
                        wcsPoint[0].Grab_z = circle1.Grab_z;
                        wcsPoint[1].Grab_z = circle1.Grab_z;
                        wcsPoint[0].Grab_theta = circle1.Grab_theta;
                        wcsPoint[1].Grab_theta = circle1.Grab_theta;
                        wcsPoint[0].ViewWindow = circle1.ViewWindow;
                        wcsPoint[1].ViewWindow = circle1.ViewWindow;
                        wcsPoint[0].Tag = circle1.Tag;
                        wcsPoint[1].Tag = circle1.Tag;
                        break;
                    case 2:
                        wcsPoint[0] = new userWcsPoint(column[0].D, row[0].D, 0, (circle1).CamParams);
                        wcsPoint[1] = new userWcsPoint(column[1].D, row[1].D, 0, (circle1).CamParams);
                        wcsPoint[0].CamName = circle1.CamName;
                        wcsPoint[1].CamName = circle1.CamName;
                        wcsPoint[0].Grab_x = circle1.Grab_x;
                        wcsPoint[1].Grab_x = circle1.Grab_x;
                        wcsPoint[0].Grab_y = circle1.Grab_y;
                        wcsPoint[1].Grab_y = circle1.Grab_y;
                        wcsPoint[0].Grab_z = circle1.Grab_z;
                        wcsPoint[1].Grab_z = circle1.Grab_z;
                        wcsPoint[0].Grab_theta = circle1.Grab_theta;
                        wcsPoint[1].Grab_theta = circle1.Grab_theta;
                        wcsPoint[0].ViewWindow = circle1.ViewWindow;
                        wcsPoint[1].ViewWindow = circle1.ViewWindow;
                        wcsPoint[0].Tag = circle1.Tag;
                        wcsPoint[1].Tag = circle1.Tag;
                        break;
                }
                result = true;
            }
            else
                result = false;
            return result;
        }

        public static bool CalculateCircleToCircleDist2D(userWcsCircle circle1, userWcsCircle circle2, out double circleToCircle, out double maxDist, out double minDist, out userWcsLine distLine)
        {
            bool result = false;
            if (circle1 == null)
            {
                throw new ArgumentNullException("circle1");
            }
            if (circle2 == null)
            {
                throw new ArgumentNullException("circle2");
            }
            ////////////////////////////////////////////////
            circleToCircle = HMisc.DistancePp(circle1.Y, circle1.X, circle2.Y, circle2.X);
            maxDist = circleToCircle + circle1.Radius + circle2.Radius;
            minDist = circleToCircle - circle1.Radius - circle2.Radius;
            distLine = new userWcsLine(circle1.X, circle1.Y, circle1.Z, circle2.X, circle2.Y, circle2.Z, circle1.CamParams);
            result = true;
            distLine.CamName = circle1.CamName;
            distLine.Grab_x = circle1.Grab_x;
            distLine.Grab_y = circle1.Grab_y;
            distLine.Grab_z = circle1.Grab_z;
            distLine.Grab_theta = circle1.Grab_theta;
            distLine.ViewWindow = circle1.ViewWindow;
            distLine.Tag = circle1.Tag;
            return result;
        }

        public static bool LineAngle(userWcsLine line1, string refAxis, out double obtuseAngle, out double acuteAngle)
        {
            bool result = false;
            ///////////////////////////
            HTuple angle = 0;
            obtuseAngle = 0;///
            acuteAngle = 0;
            if (line1 == null)
            {
                throw new ArgumentNullException("line1");
            }
            switch (refAxis)
            {
                default:
                case "x":
                case "X":
                    HOperatorSet.AngleLl(line1.X1, line1.Y1, line1.X2, line1.Y2, 0, 0, 1, 0, out angle); // 直线方向定义为与X轴正轴的夹角
                    obtuseAngle = Math.Abs(angle.TupleDeg().D);
                    acuteAngle = 180 - Math.Abs(angle.TupleDeg().D);
                    break;
                case "Y":
                case "y":
                    HOperatorSet.AngleLl(line1.X1, line1.Y1, line1.X2, line1.Y2, 0, 0, 0, 1, out angle); // 直线方向定义为与X轴正轴的夹角
                    obtuseAngle = Math.Abs(angle.TupleDeg().D);
                    acuteAngle = 180 - Math.Abs(angle.TupleDeg().D);
                    break;
            }

            result = true;
            return result;
        }

        public static bool LineMiddlePoint(userWcsLine Line, out userWcsPoint points)
        {
            if (Line == null)
                throw new ArgumentNullException("Line");
            points = new userWcsPoint();
            points = new userWcsPoint((Line.X1 + Line.X2) * 0.5, (Line.Y1 + Line.Y2) * 0.5, (Line.Z1 + Line.Z2) * 0.5, Line.CamParams);
            points.CamName = Line.CamName;
            points.Grab_x = Line.Grab_x;
            points.Grab_y = Line.Grab_y;
            points.Grab_z = Line.Grab_z;
            points.Grab_theta = Line.Grab_theta;
            points.ViewWindow = Line.ViewWindow;
            points.Tag = Line.Tag;
            return true;
        }
        public static bool LineMiddlePoint(userWcsLine Line, out userWcsVector wcsVector)
        {
            if (Line == null)
                throw new ArgumentNullException("Line");
            wcsVector = new userWcsVector();
            wcsVector = new userWcsVector((Line.X1 + Line.X2) * 0.5, (Line.Y1 + Line.Y2) * 0.5, (Line.Z1 + Line.Z2) * 0.5,0, Line.CamParams);
            wcsVector.Angle = Math.Atan2(Line.Y2 - Line.Y1, Line.X2 - Line.X1) * 180 / Math.PI;
            wcsVector.CamName = Line.CamName;
            wcsVector.Grab_x = Line.Grab_x;
            wcsVector.Grab_y = Line.Grab_y;
            wcsVector.Grab_z = Line.Grab_z;
            wcsVector.Grab_theta = Line.Grab_theta;
            wcsVector.ViewWindow = Line.ViewWindow;
            wcsVector.Tag = Line.Tag;
            return true;
        }

        public static bool GetLineLineCoordSystem(userWcsLine wcsLine1, userWcsLine wcsLine2, bool ReSet, out userWcsCoordSystem wcsCoordSystem)
        {
            bool result = false;
            double x_pose = 0, y_pose = 0, z_pose = 0;
            double x, y, overLap, phi, deg;
            int isParallel;
            /////////////////////////////
            wcsCoordSystem = new userWcsCoordSystem();
            if (wcsLine1.Y1 == 0 && wcsLine1.Y2 == 0) return result;
            if (wcsLine2.Y1 == 0 && wcsLine2.Y2 == 0) return result;
            //this.wcsCoordSystem = new userWcsCoordSystem[1];
            //////////////////
            HMisc.IntersectionLl(wcsLine1.X1, wcsLine1.Y1, wcsLine1.X2, wcsLine1.Y2,
                                  wcsLine2.X1, wcsLine2.Y1, wcsLine2.X2, wcsLine2.Y2, out x, out y, out isParallel);
            double line1Mid_y = (wcsLine1.Y1 + wcsLine1.Y2) * 0.5;
            double line1Mid_x = (wcsLine1.X1 + wcsLine1.X2) * 0.5;
            if (isParallel == 0) // 表示有交点
            {
                deg = Math.Atan2(line1Mid_y - y, line1Mid_x - x) * 180 / Math.PI;
                if (wcsLine1.Grab_x == wcsLine2.Grab_x)
                {
                    x_pose = wcsLine2.Grab_x;
                    y_pose = wcsLine1.Grab_y;
                    z_pose = wcsLine1.Grab_z;
                    wcsCoordSystem.CurrentPoint = new userWcsVector(x, y, 0, deg, wcsLine1.CamParams); // AffineUserWcsVector(this.t_x, this.t_y, this.t_z);
                    wcsCoordSystem.CurrentPoint.Grab_x = x_pose;
                    wcsCoordSystem.CurrentPoint.Grab_y = y_pose;
                    wcsCoordSystem.CurrentPoint.Grab_z = z_pose;
                    if (ReSet)
                    {
                        wcsCoordSystem.ReferencePoint = new userWcsVector(x, y, 0, deg, wcsLine1.CamParams); // AffineUserWcsVector(this.t_x, this.t_y, this.t_z);
                        wcsCoordSystem.ReferencePoint.Grab_x = x_pose;
                        wcsCoordSystem.ReferencePoint.Grab_y = y_pose;
                        //ReSet = true;
                    }
                }
                else
                {
                    x_pose = 0;
                    y_pose = 0;
                    z_pose = 0;
                    wcsCoordSystem.CurrentPoint = new userWcsVector(x, y, 0, deg);//.AffineUserWcsVector(this.t_x, this.t_y, this.t_z);
                    wcsCoordSystem.CurrentPoint.Grab_x = 0;
                    wcsCoordSystem.CurrentPoint.Grab_y = 0;
                    wcsCoordSystem.CurrentPoint.Grab_z = 0;
                    if (ReSet)
                    {
                        wcsCoordSystem.ReferencePoint = new userWcsVector(x, y, 0, deg);//.AffineUserWcsVector(this.t_x, this.t_y, this.t_z);
                        wcsCoordSystem.ReferencePoint.Grab_x = 0;
                        wcsCoordSystem.ReferencePoint.Grab_y = 0;
                        wcsCoordSystem.ReferencePoint.Grab_z = 0;
                        //isInit = true;
                    }
                }
            }
            else
            {
                deg = Math.Atan2(wcsLine1.Y2 - wcsLine1.Y1, wcsLine1.X2 - wcsLine1.X1) * 180 / Math.PI;
                wcsCoordSystem.CurrentPoint = new userWcsVector(wcsLine1.X1, wcsLine1.Y1, 0, deg, wcsLine1.CamParams); // AffineUserWcsVector(this.t_x, this.t_y, this.t_z);
                wcsCoordSystem.CurrentPoint.Grab_x = x_pose;
                wcsCoordSystem.CurrentPoint.Grab_y = y_pose;
                wcsCoordSystem.CurrentPoint.Grab_z = z_pose;
                if (ReSet)
                {
                    wcsCoordSystem.ReferencePoint = new userWcsVector(wcsLine1.X1, wcsLine1.Y1, 0, deg, wcsLine1.CamParams); // AffineUserWcsVector(this.t_x, this.t_y, this.t_z);
                    wcsCoordSystem.ReferencePoint.Grab_x = x_pose;
                    wcsCoordSystem.ReferencePoint.Grab_y = y_pose;
                    wcsCoordSystem.CurrentPoint.Grab_z = z_pose;
                    //isInit = true;
                }
            }
            result = true;
            return result;
        }
        #endregion


    }


}
