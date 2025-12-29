using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;

namespace FunctionBlock
{
    /// <summary>
    /// 体积的计算是需要基准面的
    /// </summary>
    [Serializable]
    public class TransformObject3DToXYZImage : BaseFunction, IFunction
    {

        [NonSerialized]
        private HImage image_x = null;
        [NonSerialized]
        private HImage image_y = null;
        [NonSerialized]
        private HImage image_z = null;
        [NonSerialized]
        private HImage byte_image_x = null;
        [NonSerialized]
        private HImage byte_image_y = null;
        [NonSerialized]
        private HImage byte_image_z = null;
        private int imageWidth = 180;

        public int ImageWidth
        {
            get
            {
                return imageWidth;
            }

            set
            {
                imageWidth = value;
            }
        }

        private HObjectModel3D extractRefSource1Data()
        {
            List<HObjectModel3D> listObjectModel3D = new List<HObjectModel3D>();
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
                        case "HTuple":
                            listObjectModel3D.Add(new HObjectModel3D(((HTuple)object3D).IP));
                            break;
                        case "HObjectModel3D":
                            listObjectModel3D.Add((HObjectModel3D)object3D);
                            break;
                        case "HObjectModel3D[]":
                            listObjectModel3D.AddRange((HObjectModel3D[])object3D);
                            break;
                    }
                }
            }
            return HObjectModel3D.UnionObjectModel3d(listObjectModel3D.ToArray(), "points_surface");
        }

        /// <summary>
        /// 转换3D对象模型到图像
        /// </summary>
        /// <param name="objectModel"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageX"></param>
        /// <param name="imageY"></param>
        /// <param name="imageZ"></param>
        public bool TransformObject3DToXyzRealImage(HTuple objectModel, HTuple imageWidth, out HImage imageX, out HImage imageY, out HImage imageZ)
        {
            bool result = false;
            HTuple paramX, paramY, paramZ;
            HTuple imageHeigth = 0;
            HTuple unionObjectModel = null;
            HalconLibrary ha = new HalconLibrary();
            imageX = null;
            imageY = null;
            imageZ = null;
            try
            {
                if (objectModel != null && objectModel.Length > 0) // 都执行一次合并
                    HOperatorSet.UnionObjectModel3d(objectModel, "points_surface", out unionObjectModel);
                else
                    return result;
                //////////////////////
                if (ha.IsContainPoint(unionObjectModel))
                {
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_x", out paramX);
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_y", out paramY);
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_z", out paramZ);
                    ///
                    imageHeigth = (int)(paramX.Length / imageWidth.D);
                    //////////////
                    imageX = new HImage("real", imageWidth, imageHeigth);
                    imageY = new HImage("real", imageWidth, imageHeigth);
                    imageZ = new HImage("real", imageWidth, imageHeigth);
                    HTuple rows, cols;
                    HOperatorSet.TupleGenSequence(0, imageWidth - 1, 1.0, out cols);
                    HOperatorSet.TupleGenSequence(0, imageHeigth - 1, 1.0, out rows);
                    // 矩阵处理
                    HTuple MatrixID_Col, MatrixID_Row, MatrixRepeatedID_Col, MatrixRepeatedID_Row;
                    HTuple Value_Rows, Value_Cols;
                    HOperatorSet.CreateMatrix(1, (int)imageWidth, cols, out MatrixID_Col);
                    HOperatorSet.CreateMatrix(imageHeigth, 1, rows, out MatrixID_Row);
                    HOperatorSet.RepeatMatrix(MatrixID_Col, imageHeigth, 1, out MatrixRepeatedID_Col);
                    HOperatorSet.RepeatMatrix(MatrixID_Row, 1, imageWidth, out MatrixRepeatedID_Row);
                    HOperatorSet.GetFullMatrix(MatrixRepeatedID_Col, out Value_Cols);
                    HOperatorSet.GetFullMatrix(MatrixRepeatedID_Row, out Value_Rows);
                    ////////生成图像
                    int a;
                    if (paramX.Length != Value_Cols.Length)
                    {
                        a = 10;
                    }
                    imageX.SetGrayval(Value_Rows, Value_Cols, paramX.TupleSelectRange(0, imageHeigth * imageWidth - 1));
                    imageY.SetGrayval(Value_Rows, Value_Cols, paramY.TupleSelectRange(0, imageHeigth * imageWidth - 1));
                    imageZ.SetGrayval(Value_Rows, Value_Cols, paramZ.TupleSelectRange(0, imageHeigth * imageWidth - 1));
                    // 释放矩阵
                    HOperatorSet.ClearMatrix(MatrixID_Col);
                    HOperatorSet.ClearMatrix(MatrixID_Row);
                    HOperatorSet.ClearMatrix(MatrixRepeatedID_Col);
                    HOperatorSet.ClearMatrix(MatrixRepeatedID_Row);
                    ///
                    result = true;
                }
                else
                {
                    imageX = new HImage("real", 512, 512);
                    imageY = new HImage("real", 512, 512);
                    imageZ = new HImage("real", 512, 512);
                }
            }
            catch
            {
                throw new HalconException();
            }
            finally
            {
                ha.ClearObjectModel3D(unionObjectModel);
            }
            return result;
        }

        public bool TransformObject3DToXyzRealImage(HObjectModel3D objectModel, HTuple imageWidth, out HImage imageX, out HImage imageY, out HImage imageZ)
        {
            bool result = false;
            HTuple paramX, paramY, paramZ;
            HTuple imageHeigth = 0;
            HalconLibrary ha = new HalconLibrary();
            imageX = null;
            imageY = null;
            imageZ = null;
            /////////////////////////////////////////
            if (objectModel == null || objectModel.GetObjectModel3dParams("num_points").I == 0) return result;
            //////////////////////
            paramX = objectModel.GetObjectModel3dParams("point_coord_x");
            paramY = objectModel.GetObjectModel3dParams("point_coord_y");
            paramZ = objectModel.GetObjectModel3dParams("point_coord_z");
            ///
            imageHeigth = (int)(paramX.Length / imageWidth.D);
            //////////////
            imageX = new HImage("real", imageWidth, imageHeigth);
            imageY = new HImage("real", imageWidth, imageHeigth);
            imageZ = new HImage("real", imageWidth, imageHeigth);
            HTuple rows, cols;
            HOperatorSet.TupleGenSequence(0, imageWidth - 1, 1.0, out cols);
            HOperatorSet.TupleGenSequence(0, imageHeigth - 1, 1.0, out rows);
            // 矩阵处理
            HTuple MatrixID_Col, MatrixID_Row, MatrixRepeatedID_Col, MatrixRepeatedID_Row;
            HTuple Value_Rows, Value_Cols;
            HOperatorSet.CreateMatrix(1, (int)imageWidth, cols, out MatrixID_Col);
            HOperatorSet.CreateMatrix(imageHeigth, 1, rows, out MatrixID_Row);
            HOperatorSet.RepeatMatrix(MatrixID_Col, imageHeigth, 1, out MatrixRepeatedID_Col);
            HOperatorSet.RepeatMatrix(MatrixID_Row, 1, imageWidth, out MatrixRepeatedID_Row);
            HOperatorSet.GetFullMatrix(MatrixRepeatedID_Col, out Value_Cols);
            HOperatorSet.GetFullMatrix(MatrixRepeatedID_Row, out Value_Rows);
            ////////生成图像
            int a;
            if (paramX.Length != Value_Cols.Length)
            {
                a = 10;
            }
            imageX.SetGrayval(Value_Rows, Value_Cols, paramX.TupleSelectRange(0, imageHeigth * imageWidth - 1));
            imageY.SetGrayval(Value_Rows, Value_Cols, paramY.TupleSelectRange(0, imageHeigth * imageWidth - 1));
            imageZ.SetGrayval(Value_Rows, Value_Cols, paramZ.TupleSelectRange(0, imageHeigth * imageWidth - 1));
            // 释放矩阵
            HOperatorSet.ClearMatrix(MatrixID_Col);
            HOperatorSet.ClearMatrix(MatrixID_Row);
            HOperatorSet.ClearMatrix(MatrixRepeatedID_Col);
            HOperatorSet.ClearMatrix(MatrixRepeatedID_Row);
            ///
            result = true;

            return result;
        }

        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            HalconLibrary ha = new HalconLibrary();
            try
            {
                Result.Succss = TransformObject3DToXyzRealImage(extractRefSource1Data(), this.imageWidth, out this.image_x, out this.image_y, out this.image_z);
                this.byte_image_x = this.image_x.ScaleImageMax();
                this.byte_image_y = this.image_y.ScaleImageMax();
                this.byte_image_z = this.image_z.ScaleImageMax();
                OnExcuteCompleted(this.name, this.image_z);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                    return this.name;
                case "实值图像X":
                    return this.image_x; //
                case "实值图像Y":
                    return this.image_y; //
                case "实值图像Z":
                    return this.image_z; //
                case "字节图像X":
                    return this.byte_image_x; //
                case "字节图像Y":
                    return this.byte_image_y; //
                case "字节图像Z":
                    return this.byte_image_z; //
                case "源3D对象":
                case "输入3D对象":
                    return extractRefSource1Data(); //
                default:
                    if (this.name == propertyName)
                        return this.image_z;
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
            try
            {
                if (this.image_x != null) this.image_x.Dispose();
                if (this.image_y != null) this.image_y.Dispose();
                if (this.image_z != null) this.image_z.Dispose();
                if (this.byte_image_x != null) this.byte_image_x.Dispose();
                if (this.byte_image_y != null) this.byte_image_y.Dispose();
                if (this.byte_image_z != null) this.byte_image_z.Dispose();
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
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

        public enum enShowItems
        {
            输入3D对象,
            输出实值图像X,
            输出实值图像Y,
            输出实值图像Z,
            输出字节图像X,
            输出字节图像Y,
            输出字节图像Z,
        }
    }
}
