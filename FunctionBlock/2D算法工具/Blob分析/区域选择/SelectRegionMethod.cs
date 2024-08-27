using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class SelectRegionMethod
    {
        private static object lockState = new object();
        private static SelectRegionMethod _Instance = null;
        private SelectRegionMethod()
        {

        }

        public static SelectRegionMethod Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        if (_Instance == null)
                            _Instance = new SelectRegionMethod();
                    }
                }
                return _Instance;
            }
        }

        //
        public HRegion SelectRegion(HRegion hRegion, object param)
        {
            HRegion selectRegion = new HRegion();
            if (hRegion == null || !hRegion.IsInitialized())
            {
                throw new ArgumentNullException("hRegion");
            }
            if (param == null)
            {
                throw new ArgumentNullException("info");
            }
            switch (param.GetType().GenericTypeArguments[0].Name)
            {
                case nameof(SelectShapeParam):
                    BindingList<SelectShapeParam> ShapeParam = param as BindingList<SelectShapeParam>;
                    selectRegion = select_shape(hRegion, ShapeParam);
                    break;
                case nameof(SelectShapeStdParam):
                    BindingList<SelectShapeStdParam> StdParam = param as BindingList<SelectShapeStdParam>;
                    selectRegion = select_shape_std(hRegion, StdParam);
                    break;
                case nameof(SelectRegionPointParam):
                    BindingList<SelectRegionPointParam> RegionPointParam = param as BindingList<SelectRegionPointParam>;
                    selectRegion = select_region_point(hRegion, RegionPointParam);
                    break;
            };
            return selectRegion;
        }


        public HRegion SelectRegion(HImage hImage, HRegion region, BindingList<SelectOperateParam> param)
        {
            HRegion selectRegion = new HRegion();
            HRegion selectConnectRegion = new HRegion();
            //selectRegion.GenEmptyObj();
            if (!region.IsInitialized()) return selectRegion;
            selectRegion = region.Clone();
            HRegion tempRegion = region.Clone();
            foreach (var item in param)
            {
                switch (item.Method.ToString())
                {
                    case nameof(enSelectRegionMethod.select_region_point):
                        if (item.IsActive)
                        {
                            selectRegion?.Dispose();
                            selectRegion = select_region_point(tempRegion, item);
                            tempRegion?.Dispose();
                            if (selectRegion.IsInitialized())
                                tempRegion = selectRegion.Clone();
                            if (item.IsUnion)
                            {
                                if (selectRegion.IsInitialized())
                                    selectRegion = selectRegion.Union1();
                            }
                        }
                        break;
                    case nameof(enSelectRegionMethod.select_shape):
                        if (item.IsActive)
                        {
                            selectRegion?.Dispose();
                            selectRegion = select_shape(tempRegion, item);
                            tempRegion?.Dispose();
                            if (selectRegion.IsInitialized())
                                tempRegion = selectRegion.Clone();
                            if (item.IsUnion)
                            {
                                if (selectRegion.IsInitialized())
                                    selectRegion = selectRegion.Union1();
                            }
                        }
                        break;
                    case nameof(enSelectRegionMethod.select_shape_std):
                        if (item.IsActive)
                        {
                            selectRegion?.Dispose();
                            selectRegion = select_shape_std(tempRegion, item);
                            tempRegion?.Dispose();
                            if (selectRegion.IsInitialized())
                                tempRegion = selectRegion.Clone();
                            if (item.IsUnion)
                            {
                                if (selectRegion.IsInitialized())
                                    selectRegion = selectRegion.Union1();
                            }
                        }
                        break;
                    case nameof(enSelectRegionMethod.intensity_deviation):
                        if (item.IsActive)
                        {
                            selectRegion?.Dispose();
                            selectRegion = select_intensity(hImage, tempRegion, item);
                            tempRegion?.Dispose();
                            if (selectRegion.IsInitialized())
                                tempRegion = selectRegion.Clone();
                            if (item.IsUnion)
                            {
                                if (selectRegion.IsInitialized())
                                    selectRegion = selectRegion.Union1();
                            }
                        }
                        break;
                    case nameof(enSelectRegionMethod.select_connect_region):
                        if (item.IsActive)
                        {
                            selectConnectRegion?.Dispose();
                            selectConnectRegion = select_connect(region, item);
                        }
                        break;
                    default:
                        selectRegion = region;
                        break;
                }
            }
            tempRegion?.Dispose();
            if (selectConnectRegion.IsInitialized())
            {
                if (selectRegion.IsInitialized())
                    selectRegion = selectRegion.ConcatObj(selectConnectRegion);
                else
                    selectRegion = selectConnectRegion.Clone();
            }
            selectConnectRegion?.Dispose();
            return selectRegion;
        }

        public HRegion select_region_point(HRegion region, SelectOperateParam param)
        {
            HRegion selectRegion = new HRegion();
            if (region == null)
                throw new ArgumentNullException("region");
            SelectRegionPointParam regionPointParam = param.SelectParam as SelectRegionPointParam;
            if (regionPointParam != null && region.IsInitialized())
                selectRegion = region.SelectRegionPoint(regionPointParam.Row, regionPointParam.Col);
            return selectRegion;
        }

        public HRegion select_shape(HRegion region, SelectOperateParam Param)
        {
            HRegion selectRegion = new HRegion();
            SelectShapeParam ShapeParam = Param.SelectParam as SelectShapeParam;
            if (ShapeParam != null && region.IsInitialized())
            {
                selectRegion = region?.SelectShape(ShapeParam.Features.ToString(), ShapeParam.Operation.ToString(), Convert.ToDouble(ShapeParam.Min), Convert.ToDouble(ShapeParam.Max));
            }
            return selectRegion;
        }

        public HRegion select_shape_std(HRegion region, SelectOperateParam param)
        {
            HRegion selectRegion = new HRegion();
            if (region == null)
                throw new ArgumentNullException("region");
            SelectShapeStdParam regionPointParam = param.SelectParam as SelectShapeStdParam;
            if (regionPointParam != null && region.IsInitialized())
                selectRegion = region.SelectShapeStd(regionPointParam.StdFeatures.ToString(), regionPointParam.Percent);
            return selectRegion;
        }
        public HRegion select_intensity(HImage hImage, HRegion region, SelectOperateParam param)
        {
            HRegion selectRegion = new HRegion();
            if (region == null)
                throw new ArgumentNullException("region");
            IntensityParam Param = param.SelectParam as IntensityParam;
            if (Param != null)
            {
                double Deviation = 0, Mean = 0;
                HRegion tempRegion = null;
                if (!region.IsInitialized()) return selectRegion;
                int count = region.CountObj();
                for (int i = 1; i <= count; i++)
                {
                    tempRegion = region.SelectObj(i);
                    Mean = region.Intensity(hImage, out Deviation);
                    switch (Param.IntensityFeatures)
                    {
                        default:
                        case enSelectIntensityFeatures.NONE:
                            if (selectRegion.IsInitialized())
                                selectRegion = selectRegion.ConcatObj(tempRegion);
                            else
                                selectRegion = tempRegion.Clone();
                            tempRegion?.Dispose();
                            break;
                        case enSelectIntensityFeatures.Mean:
                            if (Mean >= Param.Mean)
                            {
                                if (selectRegion.IsInitialized())
                                    selectRegion = selectRegion.ConcatObj(tempRegion);
                                else
                                    selectRegion = tempRegion.Clone();
                                tempRegion?.Dispose();
                            }
                            break;
                        case enSelectIntensityFeatures.Deviation:
                            if (Deviation >= Param.Deviation)
                            {
                                if (selectRegion.IsInitialized())
                                    selectRegion = selectRegion.ConcatObj(tempRegion);
                                else
                                    selectRegion = tempRegion.Clone();
                                tempRegion?.Dispose();
                            }
                            break;
                        case enSelectIntensityFeatures.Mean_Deviation:
                            switch (Param.Operation)
                            {
                                default:
                                case enOperation.and:
                                    if (Deviation >= Param.Deviation && Mean >= Param.Mean)
                                    {
                                        if (selectRegion.IsInitialized())
                                            selectRegion = selectRegion.ConcatObj(tempRegion);
                                        else
                                            selectRegion = tempRegion.Clone();
                                        tempRegion?.Dispose();
                                    }
                                    break;
                                case enOperation.or:
                                    if (Deviation < Param.Deviation || Mean < Param.Mean)
                                    {
                                        if (selectRegion.IsInitialized())
                                            selectRegion = selectRegion.ConcatObj(tempRegion);
                                        else
                                            selectRegion = tempRegion.Clone();
                                        tempRegion?.Dispose();
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
            return selectRegion;
        }
        public HRegion select_connect(HRegion region, SelectOperateParam Param)
        {
            HRegion selectRegion = new HRegion();
            SelectConnectParam ShapeParam = Param.SelectParam as SelectConnectParam;
            if (ShapeParam != null && region.IsInitialized())
            {
                HRegion hRegion1 = region?.SelectShape("area", "and", Convert.ToDouble(ShapeParam.MinArea), Convert.ToDouble(ShapeParam.MaxArea));
                HRegion hRegion2 = hRegion1.ShapeTrans("convex");
                HTuple row, col;
                hRegion2.AreaCenter(out row, out col);
                HTuple hTuple = HTuple.TupleGenConst(row.Length, 0);
                HObjectModel3D hObjectModel3D = new HObjectModel3D(row, col, hTuple);
                HObjectModel3D[] hObjectModels = hObjectModel3D.ConnectionObjectModel3d("distance_3d", ShapeParam.ConnectDist);
                HObjectModel3D[] selectObject = HObjectModel3D.SelectObjectModel3d(hObjectModels, "num_points", "and", ShapeParam.ConnectCount, double.MaxValue);
                ///////////////////////////////////////
                foreach (var item in selectObject)
                {
                    row = item.GetObjectModel3dParams("point_coord_x");
                    col = item.GetObjectModel3dParams("point_coord_y");
                    HRegion hRegion = new HRegion();
                    hRegion.GenRegionPoints(row, col);
                    if (selectRegion == null || !selectRegion.IsInitialized())
                        selectRegion = hRegion.ShapeTrans("convex");
                    else
                        selectRegion = selectRegion.ConcatObj(hRegion.ShapeTrans("convex"));
                }
                /////////////////////////////////////
                hObjectModel3D?.ClearObjectModel3d();
                HObjectModel3D.ClearObjectModel3d(hObjectModels);
                HObjectModel3D.ClearObjectModel3d(selectObject);
            }
            return selectRegion;
        }


        public HRegion select_region_point(HRegion region, BindingList<SelectRegionPointParam> param)
        {
            HRegion selectRegion = new HRegion();
            HRegion tempRegion = new HRegion();
            selectRegion.GenEmptyRegion();
            ////////////////////////////////////////
            //foreach (var item in param)
            //{
            //    if (item.Active)
            //    {
            //        tempRegion = region.SelectRegionPoint(Convert.ToInt32(item.Row), Convert.ToInt32(item.Col));
            //        selectRegion = selectRegion.ConcatObj(tempRegion);
            //    }
            //}
            selectRegion = tempRegion.Clone();
            return selectRegion;
        }

        public HRegion select_shape(HRegion region, BindingList<SelectShapeParam> param)
        {
            HRegion selectRegion = new HRegion();
            selectRegion = region.Clone();
            //foreach (var item in param)
            //{
            //    switch (item.Features)
            //    {
            //        case enSelectShapeFeatures.area:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("area", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.row:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("row", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.column:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("column", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.width:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("width", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.height:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("height", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.ratio:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("ratio", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.circularity:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("circularity", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.compactness:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("compactness", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.contlength:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("contlength", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.convexity:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("convexity", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.rectangularity:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("rectangularity", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.ra:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("ra", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.rb:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("rb", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.phi:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("phi", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.anisometry:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("anisometry", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.bulkiness:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("bulkiness", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.struct_factor:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("struct_factor", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.inner_width:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("inner_width", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.inner_height:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("inner_height", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.dist_mean:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("dist_mean", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.roundness:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("roundness", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.num_sides:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("num_sides", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.outer_radius:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                //HTuple row, column, radius;
            //                //selectRegion.SmallestCircle(out  row, out  column, out  radius);
            //                selectRegion = selectRegion?.SelectShape("outer_radius", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.inner_radius:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("inner_radius", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.connect_num:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("connect_num", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.holes_num:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("holes_num", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.rect2_phi:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("rect2_phi", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.rect2_len1:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("rect2_len1", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.rect2_len2:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("rect2_len2", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.area_holes:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("area_holes", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.max_diameter:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("max_diameter", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.orientation:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("orientation", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        case enSelectShapeFeatures.euler_number:
            //            if (item.Active)
            //            {
            //                if (!selectRegion.IsInitialized())
            //                    selectRegion = region.Clone();
            //                selectRegion = selectRegion?.SelectShape("euler_number", item.Operation.ToString(), Convert.ToDouble(item.Min), Convert.ToDouble(item.Max));
            //                if (item.IsUnion)
            //                    selectRegion = selectRegion?.Union1();
            //            }
            //            break;
            //        default:
            //            if (item.Active)
            //            {
            //                throw new NotImplementedException(item.Features + "未实现功能!");
            //            }
            //            break;
            //    }
            //}
            return selectRegion;
        }

        public HRegion select_shape_std(HRegion region, BindingList<SelectShapeStdParam> param)
        {
            HRegion selectRegion = new HRegion();
            HRegion tempRegion = region.Clone();
            foreach (var item in param)
            {
                //switch (item.Features)
                //{
                //    default:
                //    case enSelectStdFeatures.max_area:
                //        if (item.Active)
                //        {
                //            tempRegion = tempRegion?.SelectShapeStd("max_area", Convert.ToDouble(item.Percent));
                //        }
                //        break;
                //    case enSelectStdFeatures.rectangle1:
                //        if (item.Active)
                //        {
                //            tempRegion = tempRegion?.SelectShapeStd("rectangle1", Convert.ToDouble(item.Percent));
                //        }
                //        break;
                //    case enSelectStdFeatures.rectangle2:
                //        if (item.Active)
                //        {
                //            tempRegion = tempRegion?.SelectShapeStd("rectangle2", Convert.ToDouble(item.Percent));
                //        }
                //        break;
                //}
            }
            return selectRegion;
        }


    }


}
