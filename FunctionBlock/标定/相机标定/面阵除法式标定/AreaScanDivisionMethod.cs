using Sensor;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading;
using Common;
using System.IO;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光扫描采集的数据类
    /// </summary>
    public class AreaScanDivisionMethod
    {
       
        public AreaScanDivisionMethod()
        {

        }


        public void ExtractCircleCenter(HImage image, double Threshold, out double row, out double col, out double radius)
        {
            row = 0;
            col = 0;
            radius = 0;
            if (image == null) return;
            HTuple Row, Column, Radius, StartPhi, EndPhi, PointOrder;
            if (image == null)
                throw new ArgumentNullException("image", "输入图像参数为NULL");
            if (Threshold < 0)
                throw new ArgumentNullException("Threshold", "输入阈值参数小于0");
            HXLDCont circle = image.ThresholdSubPix(Threshold).UnionCocircularContoursXld(0.5, 0.1, 0.2, 30, 10, 10, "true", 3).SelectShapeXld("circularity", "and", 0.7, 99999); // .SelectShapeXld("circularity", "and", 0.7, 99999)            
            if (circle.CountObj() == 0) return;
            HTuple length = circle.LengthXld();
            circle = circle.SelectShapeXld("contlength", "and", length.TupleMax().D * 0.6, length.TupleMax().D * 1.1);
            ////////////////////////////
            circle.SelectShapeXld("contlength", "and", length.TupleMax().D * 0.66, length.TupleMax().D * 1.5).FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
            if (Row != null && Row.Length > 0)
            {
                row = Row[Column.TupleSortIndex()[0].D].D;
                col = Column[Column.TupleSortIndex()[0].D].D;
                radius = Radius[Column.TupleSortIndex()[0].D].D;
            }
        }



        public void CalibrateCamera(HImage image,AreaScanDivisionParam param, double []Rows,double [] Columns, double []X,double []Y, out userCamParam camParam, out userCamPose camPose, out double Error)
        {
            camParam = new userCamParam();
            camPose = new userCamPose();
            Error = 10;
            int width, height;
            try
            {             
                ///////////////////////
                if (Rows.Length != X.Length) MessageBox.Show("理论点与图像点不一致");
                image.GetImageSize(out width,out height);
                HHomMat2D hHomMat2D = new HHomMat2D();
                hHomMat2D.VectorToHomMat2d(Columns, Rows, X, Y);
                NinePointCalibParam.HHomMat2DToCamparaCamPos(hHomMat2D, width, height, param.CamParam.Focus, out camParam, out camPose, out Error);
            }
            catch (Exception ex)
            {
                throw;
            }

        }




    }
}
