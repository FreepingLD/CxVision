using AlgorithmsLibrary;
using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public static class FitMethod3D
    {
        public static bool FitSphere(HObjectModel3D[] objectModel, FitSphereParam3D Param, out userWcsSphere wcsSphere, out userWcsPose wcsPose)
        {
            bool result = false;
            HObjectModel3D fitObjectModel = null;
            HTuple pose, isPrimitive, primitiveParam;
            wcsSphere = new userWcsSphere();
            wcsPose = new userWcsPose();
            /////////////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel.Length == 0)
            {
                throw new ArgumentNullException("objectModel:对象中不包含数据");
            }
            HObjectModel3D unionObjectModel = HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface");
            fitObjectModel = unionObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm", "output_point_coord", "min_radius", "max_radius"), new HTuple(Param.PrimitiveType, Param.FittingAlgorithm, Param.OutputPointCoord, 0.01, double.MaxValue));
            unionObjectModel?.ClearObjectModel3d();
            //fitObjectModel = HObjectModel3D.FitPrimitivesObjectModel3d(unionObjectModel, new HTuple("primitive_type", "fitting_algorithm", "output_point_coord", "min_radius", "max_radius"), new HTuple(Param.PrimitiveType, Param.FittingAlgorithm, Param.OutputPointCoord, 0.01, double.MaxValue));
            isPrimitive = fitObjectModel.GetObjectModel3dParams("has_primitive_data");
            if (isPrimitive.S == "true")
            {
                pose = fitObjectModel.GetObjectModel3dParams("primitive_parameter_pose");
                primitiveParam = fitObjectModel.GetObjectModel3dParams("primitive_parameter");
                /////////////////////////////////////
                wcsSphere.WcsPose = new userWcsPose(pose);
                wcsPose = new userWcsPose(pose);
                wcsSphere.X = primitiveParam[0].D;
                wcsSphere.Y = primitiveParam[1].D;
                wcsSphere.Z = primitiveParam[2].D;
                wcsSphere.Radius = primitiveParam[3].D;
            }
            else
            {
                wcsSphere.WcsPose = new userWcsPose();
                wcsSphere.X = 0;
                wcsSphere.Y = 0;
                wcsSphere.Z = 0;
                wcsSphere.Radius = 0;
            }
            result = true;
            return result;
        }
        public static bool FitBox(HObjectModel3D[] objectModel, FitBoxParam3D Param, out userWcsBox wcsBox, out userWcsPose wcsPose)
        {
            bool result = false;
            HalconLibrary ha = new HalconLibrary();
            HObjectModel3D fitObjectModel = null;
            HTuple isPrimitive, primitiveParam;
            wcsBox = new userWcsBox();
            wcsPose = new userWcsPose();
            ///////////////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel.Length == 0)
            {
                throw new ArgumentNullException("objectModel:对象中不包含数据");
            }
            ///////////
            HObjectModel3D unionObjectModel = HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface");
            //fitObjectModel = HObjectModel3D.FitPrimitivesObjectModel3d(objectModel, new HTuple("primitive_type", "fitting_algorithm", "output_point_coord", "min_radius", "max_radius"), new HTuple(this.primitiveType, this.fittingAlgorithm, this.output_point_coord, 0.01, 10000000));
            fitObjectModel = unionObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm", "output_point_coord", "min_radius", "max_radius"), new HTuple(Param.PrimitiveType, Param.FittingAlgorithm, Param.OutputPointCoord, 0.01, double.MaxValue));
            unionObjectModel?.ClearObjectModel3d();
            isPrimitive = fitObjectModel.GetObjectModel3dParams("has_primitive_data");
            if (isPrimitive.S == "true")
            {
                primitiveParam = fitObjectModel.GetObjectModel3dParams("primitive_parameter");
                /////////////////////////////////////
                wcsBox.WcsPose = new userWcsPose(primitiveParam.TupleSelectRange(0, 6));
                wcsPose = new userWcsPose(primitiveParam.TupleSelectRange(0, 6));
                wcsBox.X = primitiveParam[0].D;
                wcsBox.Y = primitiveParam[1].D;
                wcsBox.Z = primitiveParam[2].D;
                wcsBox.AxisLength_x = primitiveParam[7].D;
                wcsBox.AxisLength_y = primitiveParam[8].D;
                wcsBox.AxisLength_z = primitiveParam[9].D;
            }
            else
            {
                wcsBox.WcsPose = new userWcsPose();
                wcsBox.X = 0;
                wcsBox.Y = 0;
                wcsBox.Z = 0;
                wcsBox.AxisLength_x = 0;
                wcsBox.AxisLength_y = 0;
                wcsBox.AxisLength_z = 0;
            }
            result = true;
            return result;
        }
        public static bool FitPlane(HObjectModel3D[] objectModel, FitPlaneParam3D Param, out userWcsPlane wcsPlane, out userWcsPose wcsPose)
        {
            bool result = false;
            HObjectModel3D fitObjectModel = null;
            HTuple pose, isPrimitive, planeCenter, planePoseNormal, homMat3D, homMat3DRotate;
            wcsPlane = new userWcsPlane();
            wcsPose = new userWcsPose();
            ////////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel.Length == 0)
            {
                throw new ArgumentNullException("objectModel:对象中不包含数据");
            }
            //////////////////////////////
            HObjectModel3D unionObjectModel = HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface");
            //fitObjectModel = HObjectModel3D.FitPrimitivesObjectModel3d(objectModel, new HTuple("primitive_type", "fitting_algorithm", "output_point_coord", "min_radius", "max_radius"), new HTuple(this.primitiveType, this.fittingAlgorithm, this.output_point_coord, 0.01, 10000000));
            fitObjectModel = unionObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm", "output_point_coord", "min_radius", "max_radius"), new HTuple(Param.PrimitiveType, Param.FittingAlgorithm, Param.OutputPointCoord, 0.01, double.MaxValue));
            unionObjectModel?.ClearObjectModel3d();
            ///////////////////////
            isPrimitive = fitObjectModel.GetObjectModel3dParams("has_primitive_data");
            if (isPrimitive.S == "true")
            {
                result = true;
                pose = fitObjectModel.GetObjectModel3dParams("primitive_pose");
                planeCenter = fitObjectModel.GetObjectModel3dParams("center");
                planePoseNormal = fitObjectModel.GetObjectModel3dParams("primitive_parameter");
                if (planePoseNormal[2] < 0)  // 这一步是为了保证平面的法向向Z轴正方向, 一般来说角度的旋转不会大于180度
                {
                    HOperatorSet.PoseToHomMat3d(pose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out pose);
                }
                /////////////////////////////////////
                wcsPlane.WcsPose = new userWcsPose(pose);
                wcsPose = new userWcsPose(pose);
                wcsPlane.X = planeCenter[0].D;
                wcsPlane.Y = planeCenter[1].D;
                wcsPlane.Z = planeCenter[2].D;
            }
            else
            {
                wcsPlane.WcsPose = new userWcsPose();
                wcsPose = new userWcsPose();
                wcsPlane.X = 0;
                wcsPlane.Y = 0;
                wcsPlane.Z = 0;
            }

            result = true;
            return result;
        }
        public static bool FitProfile(HObjectModel3D[] objectModel, FitProfileParam3D Param, out userWcsCircle fitCircle)
        {
            bool result = false;
            HTuple coord_x, coord_y, coord_z;
            double[] fit_x, fit_y, fit_z;
            fitCircle = new userWcsCircle();
            ////////////////////////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel.Length == 0)
            {
                throw new ArgumentException("objectModel:对象中不包含元素");
            }
            //////////////////////////////
            HObjectModel3D unionObjectModel = HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface");
            if (unionObjectModel.GetObjectModel3dParams("num_points").I == 0)
            {
                throw new ArgumentException("objectModel" + "个对象中不包含数据点");
            }
            coord_x = unionObjectModel.GetObjectModel3dParams("point_coord_x");
            coord_y = unionObjectModel.GetObjectModel3dParams("point_coord_y");
            coord_z = unionObjectModel.GetObjectModel3dParams("point_coord_z");
            unionObjectModel?.ClearObjectModel3d();
            /////////////////////////////
            if (coord_x.Length == 0)
            {
                throw new ArgumentException("objectModel:对象中不包含数据点");
            }
            HTuple Row, Column, Radius, StartPhi, EndPhi, PointOrder;
            switch (Param.FitCoordPoint)
            {
                case enFitCoordPoint.XY:
                    new HXLDCont(coord_y.DArr, coord_x.DArr).FitCircleContourXld(Param.FitMethod, -1, 0, 0, 5, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
                    fitCircle = new userWcsCircle(Column.D, Row.D, coord_z.TupleMean().D, Radius.D);
                    break;

                case enFitCoordPoint.XZ:
                    fit_x = coord_x.DArr;
                    fit_z = coord_z.DArr;
                    new HXLDCont(fit_z, fit_x).FitCircleContourXld(Param.FitMethod, -1, 0, 0, 5, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
                    fitCircle = new userWcsCircle(Column.D, coord_y.TupleMean().D, Row.D, Radius.D);
                    break;
                case enFitCoordPoint.YZ:
                    fit_y = coord_y.DArr;
                    fit_z = coord_y.DArr;
                    new HXLDCont(fit_z, fit_y).FitCircleContourXld(Param.FitMethod, -1, 0, 0, 5, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
                    fitCircle = new userWcsCircle(coord_x.TupleMean().D, Column.D, Row.D, Radius.D);
                    break;
                default:
                case enFitCoordPoint.XYZ:
                    double min_x = coord_x.TupleMin().D;
                    double min_y = coord_y.TupleMin().D;
                    fit_x = new double[coord_x.Length];
                    fit_z = new double[coord_x.Length];
                    for (int j = 0; j < coord_x.Length; j++)
                    {
                        fit_x[j] = Math.Sqrt((coord_x[j].D - min_x) * (coord_x[j].D - min_x) + (coord_y[j].D - min_y) * (coord_y[j].D - min_y));
                        fit_z[j] = coord_z[j].D;
                    }
                    new HXLDCont(fit_z, fit_x).FitCircleContourXld(Param.FitMethod, -1, 0, 0, 5, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
                    fitCircle = new userWcsCircle(Column.D, coord_y.TupleMean().D, Row.D, Radius.D);
                    break;
            }
            result = true;

            return result;

        }
        public static bool FitCylinder(HObjectModel3D[] objectModel, FitCylinderParam3D Param, out userWcsCylinder wcsCylinder, out userWcsPose wcsPose)
        {
            bool result = false;
            HObjectModel3D fitObjectModel = null;
            HTuple pose, isPrimitive, primitiveParam;
            wcsCylinder = new userWcsCylinder();
            wcsPose = new userWcsPose();
            /////////////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel.Length == 0)
            {
                throw new ArgumentNullException("objectModel:对象中不包含数据");
            }
            //////////////////////////////
            HObjectModel3D unionObjectModel = HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface");
            //fitObjectModel = HObjectModel3D.FitPrimitivesObjectModel3d(objectModel, new HTuple("primitive_type", "fitting_algorithm", "output_point_coord", "min_radius", "max_radius"), new HTuple(this.primitiveType, this.fittingAlgorithm, this.output_point_coord, 0.01, 10000000));
            fitObjectModel = unionObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm", "output_point_coord", "min_radius", "max_radius"), new HTuple(Param.PrimitiveType, Param.FittingAlgorithm, Param.OutputPointCoord, 0.01, double.MaxValue));
            unionObjectModel?.ClearObjectModel3d();
            //////////////////////////////
            isPrimitive = fitObjectModel.GetObjectModel3dParams("has_primitive_data");
            if (isPrimitive.S == "true")
            {
                pose = fitObjectModel.GetObjectModel3dParams("primitive_pose");
                primitiveParam = fitObjectModel.GetObjectModel3dParams("primitive_parameter");
                /////////////////////////////////////
                wcsPose = new userWcsPose(pose);
                wcsCylinder.WcsPose = new userWcsPose(pose);
                wcsCylinder.X = primitiveParam[0].D;
                wcsCylinder.Y = primitiveParam[1].D;
                wcsCylinder.Z = primitiveParam[2].D;
            }
            else
            {
                wcsPose = new userWcsPose();
                wcsCylinder.X = 0;
                wcsCylinder.Y = 0;
                wcsCylinder.Z = 0;
            }
            result = true;
            return result;
        }


    }
}
