using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using AlgorithmsLibrary;
using Common;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FunctionBlock
{
    [Serializable]
    public class GeometryMeasure  //: INotifyPropertyChanged // 这个接口不能序列化，所以不能让实现的这个接口的类型作为某个类型的字段来使用
    {
        public event MeasureChangeEventHandler MeasureChange;// 
        public event PropertyChangedEventHandler PropertyChanged;

        // 这里需要再加两个事件
        [NonSerialized]
        private HMeasure[] hMeasure;
        [NonSerialized]
        private List<double> listEdgesRow = new List<double>();
        [NonSerialized]
        private List<double> listEdgesCol = new List<double>();
        [NonSerialized]
        private List<double> listEdgesRow2 = new List<double>();
        [NonSerialized]
        private List<double> listEdgesCol2 = new List<double>();
        // gen_measure_rectangle2 参数 , 
        private double measure_length1 = 20;
        private double measure_length2 = 5;
        private enInterpolation interpolation = enInterpolation.nearest_neighbor;
        private int imageWidth;
        private int imageHeight;
        // measure_pos 参数
        private double measure_sigma = 1;
        private double measure_threshold = 30;
        private enEdgeSelect select = enEdgeSelect.first_第一个边缘;
        private enEdgeTransition transition = enEdgeTransition.all_所有极性;
        private int num_measures = 10;
        private enMeasureDirection measure_direction = enMeasureDirection.从左到右;
        private string _fillUpInvalidData = "false";
        private double _dataPercent = 0.7;
        private string type;
        // 模糊测量
        private double fuzzyThresh = 0.5;
        // 摄像机参数
        [NonSerialized]
        private userCamParam camParam;
        [NonSerialized]
        private userCamPose camPose;
        // 内部用数据
        [NonSerialized]
        private double[] rect2RowPoints;
        [NonSerialized]
        private double[] rect2ColPoints;
        [NonSerialized]
        private double[] rect2PhiPoints;
        [NonSerialized]
        private object[] sourcePoints; // 计量对象的输入数据


        public double Measure_length1
        {
            get
            {
                return measure_length1;
            }

            set
            {
                measure_length1 = value;
                CreateMeasure();
                OnPropertyChanged("Measure_length1");
            }
        }
        public double Measure_length2
        {
            get
            {
                return measure_length2;
            }

            set
            {
                measure_length2 = value;
                CreateMeasure();
                OnPropertyChanged("Measure_length2");
            }
        }
        public double Measure_sigma
        {
            get
            {
                return measure_sigma;
            }

            set
            {
                measure_sigma = value;
                OnPropertyChanged("Measure_sigma");
            }
        }
        public double Measure_threshold
        {
            get
            {
                return measure_threshold;
            }

            set
            {
                measure_threshold = value;
                OnPropertyChanged("Measure_threshold");
            }
        }
        public enEdgeSelect Measure_select
        {
            get
            {
                return select;
            }

            set
            {
                select = value;
                OnPropertyChanged("Measure_select");
            }
        }
        public enEdgeTransition Measure_transition
        {
            get
            {
                return transition;
            }

            set
            {
                transition = value;
                OnPropertyChanged("Measure_transition");
            }
        }

        public bool IsOutFitPoint { get; set; }

        public int Num_measures
        {
            get
            {
                return num_measures;
            }

            set
            {
                num_measures = value;
                if (this.sourcePoints == null) return;
                switch (this.type)
                {
                    case "point":
                        break;
                    case "line": // 数量变了，要重新计算点位
                        CreateLineMeasure(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]), Convert.ToDouble(this.sourcePoints[4]), Convert.ToDouble(this.sourcePoints[5]), this.imageWidth, this.imageHeight);
                        CreateMeasure();
                        break;
                    case "circle":
                        CreateCircleMeasure(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]), Convert.ToDouble(this.sourcePoints[4]), Convert.ToDouble(this.sourcePoints[5]), this.sourcePoints[5].ToString(), this.imageWidth, this.imageHeight);
                        CreateMeasure();
                        break;
                    case "ellipse":
                        CreateEllipseMeasure(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]), Convert.ToDouble(this.sourcePoints[4]), Convert.ToDouble(this.sourcePoints[5]), Convert.ToDouble(this.sourcePoints[6]), Convert.ToDouble(this.sourcePoints[7]), this.imageWidth, this.imageHeight);
                        CreateMeasure();
                        break;
                    case "rect2":
                        CreateRect2Measure(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(sourcePoints[3]), Convert.ToDouble(this.sourcePoints[4]), Convert.ToDouble(this.sourcePoints[5]), this.imageWidth, this.imageHeight);
                        CreateMeasure();
                        break;
                }
                OnPropertyChanged("Num_measures");
            }
        }
        public enMeasureDirection Measure_direction
        {
            get
            {
                return measure_direction;
            }

            set
            {
                measure_direction = value;
                CreateMeasure();
                OnPropertyChanged("Measure_direction");
            }
        }
        public double FuzzyThresh
        {
            get
            {
                return fuzzyThresh;
            }

            set
            {
                fuzzyThresh = value;
                OnPropertyChanged("FuzzyThresh");
            }
        }

        public string FillUpInvalidData { get => _fillUpInvalidData; set => _fillUpInvalidData = value; }
        public double DataPercent { get => _dataPercent; set => _dataPercent = value; }

        public GeometryMeasure()
        {

        }

        protected void OnPropertyChanged(string propertyName = null) //[CallerMemberName] 
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
        public GeometryMeasure(enMeasureType measureType)
        {
            this.type = measureType.ToString();
            switch (this.type)
            {
                case "point":
                    this.measure_direction = enMeasureDirection.从左到右;
                    break;
                case "line":
                    this.measure_direction = enMeasureDirection.从左到右;
                    break;
                case "cross":
                    this.measure_direction = enMeasureDirection.从左到右;
                    break;
                case "circle":
                    this.measure_direction = enMeasureDirection.从内到外;
                    break;
                case "ellipse":
                    this.measure_direction = enMeasureDirection.从内到外;
                    break;
                case "rect2":
                    this.measure_direction = enMeasureDirection.从内到外;
                    break;
                case "width":
                    this.measure_direction = enMeasureDirection.从内到外;
                    break;
            }
        }
        public GeometryMeasure(int imageWidth, int imageHeight)
        {
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
        }
        private void GetPointLine2(double RowBegin, double ColumnBegin, double RowEnd, double ColumnEnd, double length, out double Row, out double Col)
        {
            Row = 0;
            Col = 0;
            //////////////////////////////////
            double k = (RowEnd - RowBegin) / (ColumnEnd - ColumnBegin);
            double RowDist = RowEnd - RowBegin;
            double ColDist = ColumnEnd - ColumnBegin;
            double startRow, startCol, endRow, endCol;
            if (HMisc.DistancePp(RowBegin, ColumnBegin, 0, 0) <= HMisc.DistancePp(0, 0, RowEnd, ColumnEnd))
            {
                startRow = RowBegin;
                startCol = ColumnBegin;
                endRow = RowEnd;
                endCol = ColumnEnd;
            }
            else
            {
                startRow = RowEnd;
                startCol = ColumnEnd;
                endRow = RowBegin;
                endCol = ColumnBegin;
            }
            /////////////////////////////
            if (RowDist == 0 && ColDist != 0)
            {
                Row = startRow;
                Col = length + startCol;
            }
            ///////
            if (RowDist != 0 && ColDist == 0)
            {
                Row = length + startRow;
                Col = startCol;
            }
            //////////////////
            HTuple tempRow, tempCol;
            if (RowDist != 0 && ColDist != 0)
            {
                HOperatorSet.IntersectionSegmentCircle(startRow, startCol, endRow, endCol, startRow, startCol, length, 0, Math.PI * 2, "positive", out tempRow, out tempCol);
                if (tempRow.Length > 0)
                {
                    Row = tempRow.D;
                    Col = tempCol.D;
                }
                else
                {
                    Row = endRow; // 最后一个点有可能不会相交
                    Col = endCol;
                }
            }
            ////////////////
            if (RowDist == 0 && ColDist == 0)
            {
                Row = startRow;
                Col = startCol;
            }
        }
        private void GetPointLine(double RowBegin, double ColumnBegin, double RowEnd, double ColumnEnd, double length, out double Row, out double Col)
        {
            HTuple row, col;
            //////////////////
            if (HMisc.DistancePp(RowBegin, ColumnBegin, RowEnd, ColumnEnd) == 0)
            {
                Row = RowBegin;
                Col = ColumnBegin;
            }
            else
            {
                HOperatorSet.IntersectionSegmentCircle(RowBegin, ColumnBegin, RowEnd, ColumnEnd, RowBegin, ColumnBegin, length, 0, Math.PI * 2, "positive", out row, out col);
                if (row.Length > 0)
                {
                    Row = row.D;
                    Col = col.D;
                }
                else
                {
                    Row = RowEnd;
                    Col = ColumnEnd;
                }
            }

        }

        private void GetPointCircle(double Row_Center, double Column_Center, double Radius, double rad, out double Row, out double Col)
        {
            Row = Row_Center - Radius * Math.Sin(rad);
            Col = Column_Center + Radius * Math.Cos(rad);
            //Angle = Math.Atan2(Row - Row_Center, Col - Column_Center);
        }
        private void GetPointEllipse(double Row_Center, double Column_Center, double Phi, double Radius1, double Radius2, double rad, out double Row, out double Col)
        {
            HTuple rowPoint, colPoint;
            HOperatorSet.GetPointsEllipse(rad, Row_Center, Column_Center, Phi, Radius1, Radius2, out rowPoint, out colPoint);
            if (rowPoint != null && rowPoint.Length > 0)
            {
                Row = rowPoint.D;
                Col = colPoint.D;
            }
            else
            {
                Row = Row_Center + Radius1;
                Col = Column_Center + Radius1;
            }
        }

        private void GetPointRect2(double Row, double Column, double phi, double length1, double length2, out double[] rows, out double[] cols)
        {
            rows = new double[this.num_measures * 4];
            cols = new double[this.num_measures * 4];
            double leftUpRow, leftUpCol, leftDownRow, leftDownCol, rightUpRow, rightUpCol, rightDownRow, rightDownCol;
            ///////////////
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D homMat2dRotate = hHomMat2D.HomMat2dRotate(phi, Row, Column);
            leftUpRow = homMat2dRotate.AffineTransPoint2d(Row - length2, Column - length1, out leftUpCol);
            /////////////////////////
            leftDownRow = homMat2dRotate.AffineTransPoint2d(Row + length2, Column - length1, out leftDownCol); ;
            /////////////////////
            rightUpRow = homMat2dRotate.AffineTransPoint2d(Row - length2, Column + length1, out rightUpCol);
            ////////////////////////////
            rightDownRow = homMat2dRotate.AffineTransPoint2d(Row + length2, Column + length1, out rightDownCol);
            ////////////////////
            double row, col;
            double dist1Mea = HMisc.DistancePp(leftUpRow, leftUpCol, rightUpRow, rightUpCol) / (this.num_measures - 1);
            double dist2Mea = HMisc.DistancePp(rightUpRow, rightUpCol, rightDownRow, rightDownCol) / (this.num_measures - 1);
            double dist3Mea = HMisc.DistancePp(rightDownRow, rightDownCol, leftDownRow, leftDownCol) / (this.num_measures - 1);
            double dist4Mea = HMisc.DistancePp(leftDownRow, leftDownCol, leftUpRow, leftUpCol) / (this.num_measures - 1);
            //////////////////////////////////////////////////////
            for (int i = 0; i < this.num_measures; i++)
            {
                GetPointLine(leftUpRow, leftUpCol, rightUpRow, rightUpCol, dist1Mea * i, out row, out col);
                rows[i] = row;
                cols[i] = col;
            }
            ////////////////////////
            for (int i = 0; i < this.num_measures; i++)
            {
                GetPointLine(rightUpRow, rightUpCol, rightDownRow, rightDownCol, dist2Mea * i, out row, out col);
                rows[i + this.num_measures] = row;
                cols[i + this.num_measures] = col;
            }
            ////////////////////////
            for (int i = 0; i < this.num_measures; i++)
            {
                GetPointLine(rightDownRow, rightDownCol, leftDownRow, leftDownCol, dist3Mea * i, out row, out col);
                rows[i + this.num_measures * 2] = row;
                cols[i + this.num_measures * 2] = col;
            }
            ////////////////////////
            for (int i = 0; i < this.num_measures; i++)
            {
                GetPointLine(leftDownRow, leftDownCol, leftUpRow, leftUpCol, dist4Mea * i, out row, out col);
                rows[i + this.num_measures * 3] = row;
                cols[i + this.num_measures * 3] = col;
            }
        }

        private void GetPointWidth(double Row, double Column, double phi, double length1, double length2, out double[] rows, out double[] cols)
        {
            rows = new double[this.num_measures * 2];
            cols = new double[this.num_measures * 2];
            double leftUpRow, leftUpCol, leftDownRow, leftDownCol, rightUpRow, rightUpCol, rightDownRow, rightDownCol;
            ///////////////
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D homMat2dRotate = hHomMat2D.HomMat2dRotate(phi, Row, Column);
            leftUpRow = homMat2dRotate.AffineTransPoint2d(Row - length2, Column - length1, out leftUpCol);
            /////////////////////////
            leftDownRow = homMat2dRotate.AffineTransPoint2d(Row + length2, Column - length1, out leftDownCol); ;
            /////////////////////
            rightUpRow = homMat2dRotate.AffineTransPoint2d(Row - length2, Column + length1, out rightUpCol);
            ////////////////////////////
            rightDownRow = homMat2dRotate.AffineTransPoint2d(Row + length2, Column + length1, out rightDownCol);
            ////////////////////
            double row, col;
            double dist1Mea = HMisc.DistancePp(leftUpRow, leftUpCol, rightUpRow, rightUpCol) / (this.num_measures - 1);
            double dist2Mea = HMisc.DistancePp(rightUpRow, rightUpCol, rightDownRow, rightDownCol) / (this.num_measures - 1);
            double dist3Mea = HMisc.DistancePp(rightDownRow, rightDownCol, leftDownRow, leftDownCol) / (this.num_measures - 1);
            double dist4Mea = HMisc.DistancePp(leftDownRow, leftDownCol, leftUpRow, leftUpCol) / (this.num_measures - 1);
            //////////////////////////////////////////////////////
            for (int i = 0; i < this.num_measures; i++)
            {
                GetPointLine(leftUpRow, leftUpCol, rightUpRow, rightUpCol, dist1Mea * i, out row, out col);
                rows[i] = row;
                cols[i] = col;
            }
            ////////////////////////
            for (int i = 0; i < this.num_measures; i++)
            {
                GetPointLine(leftDownRow, leftDownCol, rightDownRow, rightDownCol, dist3Mea * i, out row, out col);
                rows[i + this.num_measures] = row;
                cols[i + this.num_measures] = col;
            }
        }
        private void CreateMeasure()
        {
            // 清空测量对象
            if (this.hMeasure != null)
            {
                for (int i = 0; i < this.hMeasure.Length; i++)
                {
                    if (this.hMeasure[i] != null)
                        this.hMeasure[i].Dispose();
                }
                this.hMeasure = null;
            }
            ///////计算位置点的角度////////
            GetNormalAngle(this.rect2RowPoints, this.rect2ColPoints, out this.rect2PhiPoints);
            /////////////////////////////
            this.hMeasure = new HMeasure[this.rect2RowPoints.Length];
            for (int i = 0; i < this.rect2RowPoints.Length; i++)
            {
                this.hMeasure[i] = new HMeasure(this.rect2RowPoints[i], this.rect2ColPoints[i], this.rect2PhiPoints[i], this.measure_length1, this.measure_length2, this.imageWidth, this.imageHeight, this.interpolation.ToString());
            }
        }
        private void GetNormalAngle(double[] rows, double[] cols, out double[] phi)
        {
            phi = new double[rows.Length];
            HXLDCont xld = new HXLDCont(this.rect2RowPoints, this.rect2ColPoints);
            HXLDCont paralleXld = null;
            HTuple normalRow = new HTuple(phi);
            HTuple normalCol = new HTuple(phi);
            ////////////////////////////
            double row, col;
            double[] RowPoints;
            double[] ColPoints;
            double[] Row2Points;
            double[] Col2Points;
            double distMea;
            ////////////////////////////////
            switch (this.type)
            {
                case "point":
                    this.rect2PhiPoints[0] = Math.Atan2((Convert.ToDouble(this.sourcePoints[2]) - Convert.ToDouble(this.sourcePoints[0])) * -1, Convert.ToDouble(this.sourcePoints[3]) - Convert.ToDouble(this.sourcePoints[1])); // 从起点到终点的方向作为测量方向
                    break;

                case "line":
                    for (int i = 0; i < rows.Length; i++)
                    {
                        //phi[i] = Math.Atan2((this.sourcePoints[0] - this.sourcePoints[2]) * -1, this.sourcePoints[1] - this.sourcePoints[3]) - Math.PI * 0.5; // 从起点到终点方向的顺时针旋转为法向
                        phi[i] = Convert.ToDouble(this.sourcePoints[5]);
                    }
                    break;

                case "cross":
                    for (int i = 0; i < rows.Length; i++)
                    {
                        if (i < (int)(rows.Length * 0.5))
                            phi[i] = Convert.ToDouble(this.sourcePoints[5]);
                        else
                            phi[i] = Convert.ToDouble(this.sourcePoints[5]) + Math.PI * 0.5;
                    }
                    break;

                case "circle":
                    switch (this.measure_direction)
                    {
                        case enMeasureDirection.从内到外:
                            RowPoints = new double[this.num_measures];
                            ColPoints = new double[this.num_measures];
                            distMea = Convert.ToDouble(this.sourcePoints[6]);
                            //if (Convert.ToDouble(this.sourcePoints[3]) <= Convert.ToDouble(this.sourcePoints[4]))
                            //distMea = (Convert.ToDouble(this.sourcePoints[4]) - Convert.ToDouble(this.sourcePoints[3])) / this.num_measures;// Math.Abs(this.sourcePoints[4] - this.sourcePoints[3]) / this.num_measures;
                            //else
                            //distMea = (Convert.ToDouble(this.sourcePoints[4]) - Convert.ToDouble(this.sourcePoints[3])) / this.num_measures; //(Math.PI * 2 - Math.Abs(this.sourcePoints[4] - this.sourcePoints[3])) / this.num_measures;
                            ///////////////////////////////////////////////////
                            for (int i = 0; i < this.num_measures; i++)
                            {
                                GetPointCircle(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]) + 0.1, distMea * i + Convert.ToDouble(this.sourcePoints[3]), out row, out col);
                                RowPoints[i] = row;
                                ColPoints[i] = col;
                            }
                            normalRow = new HTuple(RowPoints);
                            normalCol = new HTuple(ColPoints);
                            break;
                        case enMeasureDirection.从外到内:
                            RowPoints = new double[this.num_measures];
                            ColPoints = new double[this.num_measures];
                            distMea = Convert.ToDouble(this.sourcePoints[6]);
                            //if (Convert.ToDouble(this.sourcePoints[3]) <= Convert.ToDouble(this.sourcePoints[4]))
                            //distMea = (Convert.ToDouble(this.sourcePoints[4]) - Convert.ToDouble(this.sourcePoints[3])) / this.num_measures;// Math.Abs(this.sourcePoints[4] - this.sourcePoints[3]) / this.num_measures;
                            //else
                            //distMea = (Convert.ToDouble(this.sourcePoints[4]) - Convert.ToDouble(this.sourcePoints[3])) / this.num_measures; // (Math.PI * 2 - Math.Abs(this.sourcePoints[4] - this.sourcePoints[3])) / this.num_measures;
                            ///////////////////////////////////////////////////
                            for (int i = 0; i < this.num_measures; i++)
                            {
                                GetPointCircle(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]) - 0.1, distMea * i + Convert.ToDouble(this.sourcePoints[3]), out row, out col);
                                RowPoints[i] = row;
                                ColPoints[i] = col;
                            }
                            normalRow = new HTuple(RowPoints);
                            normalCol = new HTuple(ColPoints);
                            break;
                    }
                    ////////////////////////////
                    for (int i = 0; i < rows.Length; i++)
                    {
                        phi[i] = Math.Atan2((normalRow[i].D - rows[i]) * -1, normalCol[i].D - cols[i]);
                    }
                    break;
                //////////////////////////////
                case "ellipse":
                    switch (this.measure_direction)
                    {
                        case enMeasureDirection.从内到外:
                            RowPoints = new double[this.num_measures];
                            ColPoints = new double[this.num_measures];
                            distMea = Convert.ToDouble(this.sourcePoints[8]);
                            //if (Convert.ToDouble(this.sourcePoints[5]) <= Convert.ToDouble(this.sourcePoints[6]))
                            //distMea = Math.Abs(Convert.ToDouble(this.sourcePoints[5]) - Convert.ToDouble(this.sourcePoints[6])) / this.num_measures;
                            //else
                            //distMea = (Math.PI * 2 - Math.Abs(Convert.ToDouble(this.sourcePoints[5]) - Convert.ToDouble(this.sourcePoints[6]))) / this.num_measures;
                            ///////////////////////////////////////////////////
                            for (int i = 0; i < this.num_measures; i++)
                            {
                                GetPointEllipse(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]) + 0.1, Convert.ToDouble(this.sourcePoints[4]) + 0.1, distMea * i + Convert.ToDouble(this.sourcePoints[5]), out row, out col);
                                RowPoints[i] = row;
                                ColPoints[i] = col;
                            }
                            normalRow = new HTuple(RowPoints);
                            normalCol = new HTuple(ColPoints);
                            break;
                        case enMeasureDirection.从外到内:
                            RowPoints = new double[this.num_measures];
                            ColPoints = new double[this.num_measures];
                            distMea = Convert.ToDouble(this.sourcePoints[8]);
                            //if (Convert.ToDouble(this.sourcePoints[5]) <= Convert.ToDouble(this.sourcePoints[6]))
                            //distMea = Math.Abs(Convert.ToDouble(this.sourcePoints[5]) - Convert.ToDouble(this.sourcePoints[6])) / this.num_measures;
                            //else
                            //distMea = (Math.PI * 2 - Math.Abs(Convert.ToDouble(this.sourcePoints[5]) - Convert.ToDouble(this.sourcePoints[6]))) / this.num_measures;
                            ///////////////////////////////////////////////////
                            for (int i = 0; i < this.num_measures; i++)
                            {
                                GetPointEllipse(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]) - 0.1, Convert.ToDouble(this.sourcePoints[4]) - 0.1, distMea * i + Convert.ToDouble(this.sourcePoints[5]), out row, out col);
                                RowPoints[i] = row;
                                ColPoints[i] = col;
                            }
                            normalRow = new HTuple(RowPoints);
                            normalCol = new HTuple(ColPoints);
                            break;
                    }
                    ////////////////////////////
                    for (int i = 0; i < rows.Length; i++)
                    {
                        phi[i] = Math.Atan2((normalRow[i].D - rows[i]) * -1, normalCol[i].D - cols[i]);
                    }
                    break;

                case "rect2":
                    switch (this.measure_direction)
                    {
                        case enMeasureDirection.从内到外:
                            RowPoints = new double[this.num_measures * 4];
                            ColPoints = new double[this.num_measures * 4];
                            Row2Points = new double[this.num_measures * 4];
                            Col2Points = new double[this.num_measures * 4];
                            // 每次只能计算两条边的偏置
                            GetPointRect2(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]) + 0.1, Convert.ToDouble(this.sourcePoints[4]), out RowPoints, out ColPoints);
                            GetPointRect2(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]), Convert.ToDouble(this.sourcePoints[4]) + 0.1, out Row2Points, out Col2Points);
                            ////////////////////////////
                            Array.Copy(Row2Points, 0, RowPoints, 0, this.num_measures);
                            Array.Copy(Row2Points, this.num_measures * 2, RowPoints, this.num_measures * 2, this.num_measures);
                            Array.Copy(Col2Points, 0, ColPoints, 0, this.num_measures);
                            Array.Copy(Col2Points, this.num_measures * 2, ColPoints, this.num_measures * 2, this.num_measures);
                            normalRow = new HTuple(RowPoints);
                            normalCol = new HTuple(ColPoints);
                            break;
                        case enMeasureDirection.从外到内:
                            RowPoints = new double[this.num_measures * 4];
                            ColPoints = new double[this.num_measures * 4];
                            Row2Points = new double[this.num_measures * 4];
                            Col2Points = new double[this.num_measures * 4];
                            GetPointRect2(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]) - 0.1, Convert.ToDouble(this.sourcePoints[4]), out RowPoints, out ColPoints);
                            GetPointRect2(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]), Convert.ToDouble(this.sourcePoints[4]) - 0.1, out Row2Points, out Col2Points);
                            ////////////////////////////
                            Array.Copy(Row2Points, 0, RowPoints, 0, this.num_measures);
                            Array.Copy(Row2Points, this.num_measures * 2, RowPoints, this.num_measures * 2, this.num_measures);
                            Array.Copy(Col2Points, 0, ColPoints, 0, this.num_measures);
                            Array.Copy(Col2Points, this.num_measures * 2, ColPoints, this.num_measures * 2, this.num_measures);
                            normalRow = new HTuple(RowPoints);
                            normalCol = new HTuple(ColPoints);
                            break;
                    }
                    for (int i = 0; i < rows.Length; i++)
                    {
                        phi[i] = Math.Atan2((normalRow[i].D - rows[i]) * -1, normalCol[i].D - cols[i]);
                    }
                    break;
                case "width":
                    switch (this.measure_direction)
                    {
                        case enMeasureDirection.从内到外:
                            RowPoints = new double[this.num_measures * 2];
                            ColPoints = new double[this.num_measures * 2];
                            Row2Points = new double[this.num_measures * 2];
                            Col2Points = new double[this.num_measures * 2];
                            // 每次只能计算两条边的偏置
                            GetPointWidth(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]) + 0.1, Convert.ToDouble(this.sourcePoints[4]), out RowPoints, out ColPoints);
                            GetPointWidth(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]), Convert.ToDouble(this.sourcePoints[4]) + 0.1, out Row2Points, out Col2Points);
                            ////////////////////////////
                            Array.Copy(Row2Points, 0, RowPoints, 0, this.num_measures);
                            Array.Copy(Row2Points, this.num_measures * 1, RowPoints, this.num_measures * 1, this.num_measures);
                            Array.Copy(Col2Points, 0, ColPoints, 0, this.num_measures);
                            Array.Copy(Col2Points, this.num_measures * 1, ColPoints, this.num_measures * 1, this.num_measures);
                            normalRow = new HTuple(RowPoints);
                            normalCol = new HTuple(ColPoints);
                            break;
                        case enMeasureDirection.从外到内:
                            RowPoints = new double[this.num_measures * 2];
                            ColPoints = new double[this.num_measures * 2];
                            Row2Points = new double[this.num_measures * 2];
                            Col2Points = new double[this.num_measures * 2];
                            GetPointWidth(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]) - 0.1, Convert.ToDouble(this.sourcePoints[4]), out RowPoints, out ColPoints);
                            GetPointWidth(Convert.ToDouble(this.sourcePoints[0]), Convert.ToDouble(this.sourcePoints[1]), Convert.ToDouble(this.sourcePoints[2]), Convert.ToDouble(this.sourcePoints[3]), Convert.ToDouble(this.sourcePoints[4]) - 0.1, out Row2Points, out Col2Points);
                            ////////////////////////////
                            Array.Copy(Row2Points, 0, RowPoints, 0, this.num_measures);
                            Array.Copy(Row2Points, this.num_measures * 1, RowPoints, this.num_measures * 1, this.num_measures);
                            Array.Copy(Col2Points, 0, ColPoints, 0, this.num_measures);
                            Array.Copy(Col2Points, this.num_measures * 1, ColPoints, this.num_measures * 1, this.num_measures);
                            normalRow = new HTuple(RowPoints);
                            normalCol = new HTuple(ColPoints);
                            break;
                    }
                    for (int i = 0; i < rows.Length; i++)
                    {
                        phi[i] = Math.Atan2((normalRow[i].D - rows[i]) * -1, normalCol[i].D - cols[i]);
                    }
                    break;
                    /////////
            }
            ////
            if (xld != null)
                xld.Dispose();
            if (paralleXld != null)
                paralleXld.Dispose();
        }



        public void CreatePointMeasure(double RowBegin, double ColumnBegin, double RowEnd, double ColumnEnd, int imageWidth, int imageHeight)
        {
            this.type = "point";
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.rect2RowPoints = null;
            this.rect2ColPoints = null;
            this.rect2PhiPoints = null;
            this.rect2RowPoints = new double[1];
            this.rect2ColPoints = new double[1];
            this.rect2PhiPoints = new double[1];
            this.rect2RowPoints[0] = (RowBegin + RowEnd) * 0.5;
            this.rect2ColPoints[0] = (ColumnBegin + ColumnEnd) * 0.5;
            this.sourcePoints = new object[4] { RowBegin, ColumnBegin, RowEnd, ColumnEnd };
            this.measure_length1 = HMisc.DistancePp(RowBegin, ColumnBegin, RowEnd, ColumnEnd) * 0.5;
            CreateMeasure();
        }
        public void CreateCrossLineMeasure(double RowBegin, double ColumnBegin, double RowEnd, double ColumnEnd, double diffRadius, double normalPhi, int imageWidth, int imageHeight)
        {
            this.type = "cross";
            this.measure_length1 = Math.Abs(diffRadius); // 测量区域的长度等于箭头的长度
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.rect2RowPoints = null;
            this.rect2ColPoints = null;
            this.rect2PhiPoints = null;
            double row, col;
            if (this.num_measures < (int)(HMisc.DistancePp(RowBegin, ColumnBegin, RowEnd, ColumnEnd) / (this.measure_length2 * 2)))
                this.num_measures = (int)(HMisc.DistancePp(RowBegin, ColumnBegin, RowEnd, ColumnEnd) / (this.measure_length2 * 2)); // 自动计算测量区域的数量
            this.rect2RowPoints = new double[this.num_measures * 2];
            this.rect2ColPoints = new double[this.num_measures * 2];
            double distMea = HMisc.DistancePp(RowBegin, ColumnBegin, RowEnd, ColumnEnd) / (this.num_measures - 1);
            for (int i = 0; i < this.num_measures; i++)
            {
                GetPointLine(RowBegin, ColumnBegin, RowEnd, ColumnEnd, distMea * i, out row, out col);
                this.rect2RowPoints[i] = row;
                this.rect2ColPoints[i] = col;
            }
            //////////////////////////////// 第二条直线 ////////////////////
            HTuple Qx, Qy;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid((RowBegin + RowEnd) * 0.5, (ColumnBegin + ColumnEnd) * 0.5, Math.Atan2(RowEnd - RowBegin, ColumnEnd - ColumnBegin),
                                         (RowBegin + RowEnd) * 0.5, (ColumnBegin + ColumnEnd) * 0.5, Math.Atan2(RowEnd - RowBegin, ColumnEnd - ColumnBegin) + Math.PI * 0.5);
            Qx = hHomMat2D.AffineTransPoint2d(new HTuple(RowBegin, RowEnd), new HTuple(ColumnBegin, ColumnEnd), out Qy);
            double startPointRow2 = Qx[0].D;
            double startPointCol2 = Qy[0].D;
            double endPointRow2 = Qx[1].D;
            double endPointCol2 = Qy[1].D;
            for (int i = num_measures; i < this.num_measures * 2; i++)
            {
                GetPointLine(startPointRow2, startPointCol2, endPointRow2, endPointCol2, distMea * (i - num_measures), out row, out col);
                this.rect2RowPoints[i] = row;
                this.rect2ColPoints[i] = col;
            }
            /////////////////////////
            this.sourcePoints = new object[6] { RowBegin, ColumnBegin, RowEnd, ColumnEnd, diffRadius, normalPhi };
            CreateMeasure();
        }
        public void CreateLineMeasure(double RowBegin, double ColumnBegin, double RowEnd, double ColumnEnd, double diffRadius, double normalPhi, int imageWidth, int imageHeight)
        {
            this.type = "line";
            this.measure_length1 = Math.Abs(diffRadius); // 测量区域的长度等于箭头的长度
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.rect2RowPoints = null;
            this.rect2ColPoints = null;
            this.rect2PhiPoints = null;
            double row, col;
            if (this.num_measures < (int)(HMisc.DistancePp(RowBegin, ColumnBegin, RowEnd, ColumnEnd) / (this.measure_length2 * 2)))
                this.num_measures = (int)(HMisc.DistancePp(RowBegin, ColumnBegin, RowEnd, ColumnEnd) / (this.measure_length2 * 2)); // 自动计算测量区域的数量
            this.rect2RowPoints = new double[this.num_measures];
            this.rect2ColPoints = new double[this.num_measures];
            double distMea = HMisc.DistancePp(RowBegin, ColumnBegin, RowEnd, ColumnEnd) / (this.num_measures - 1);
            for (int i = 0; i < this.num_measures; i++)
            {
                GetPointLine(RowBegin, ColumnBegin, RowEnd, ColumnEnd, distMea * i, out row, out col);
                this.rect2RowPoints[i] = row;
                this.rect2ColPoints[i] = col;
            }
            /////////////////////////
            this.sourcePoints = new object[6] { RowBegin, ColumnBegin, RowEnd, ColumnEnd, diffRadius, normalPhi };
            CreateMeasure();
        }
        public void CreateCircleMeasure(double Row, double Column, double radius, double startPhi, double endPhi, double diffRadius, string pointOrder, int imageWidth, int imageHeight)
        {
            this.type = "circle";
            if (diffRadius >= 0)
                this.measure_direction = enMeasureDirection.从内到外;
            else
                this.measure_direction = enMeasureDirection.从外到内;
            this.measure_length1 = Math.Abs(diffRadius);
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.rect2RowPoints = null;
            this.rect2ColPoints = null;
            this.rect2PhiPoints = null;
            /// 统一终止角的方向
            switch (pointOrder)
            {
                default:
                case "positive":
                    if (startPhi < 0)
                        startPhi += Math.PI * 2; // 逆时针方向用正角度表示
                    if (endPhi < 0)
                        endPhi += Math.PI * 2; // 逆时针方向用正角度表示
                    break;
                case "negative": // 顺时针方向的用负角度表示，
                    if (startPhi > 0)
                        startPhi -= Math.PI * 2;
                    if (endPhi > 0)
                        endPhi -= Math.PI * 2;
                    break;
            }
            if (this.num_measures < (int)(Math.PI * radius * 2 * Math.Abs(endPhi - startPhi) / (this.measure_length2 * 2 * Math.PI * 2)))
                this.num_measures = (int)(Math.PI * radius * 2 * Math.Abs(endPhi - startPhi) / (this.measure_length2 * 2 * Math.PI * 2));
            this.rect2RowPoints = new double[this.num_measures];
            this.rect2ColPoints = new double[this.num_measures];
            double distMea;
            ////////////////////////////// : 积极的，以起点作为初始值 positive，表示迭代值为正，negative：表示迭代值为负
            double Sx, Sy, Phi, Theta, Tx, Ty;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(Row, Column, startPhi, Row, Column, endPhi);
            hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty); // 一个角度变换到另一个角度，有两个方向可达，一个为锐角，一个为钝角
            switch (pointOrder)
            {
                default:
                case "positive":
                    if (Phi < 0)
                        Phi += Math.PI * 2;
                    break;
                case "negative": // 顺时针方向的用负角度表示，
                    if (Phi > 0)
                        Phi -= Math.PI * 2;
                    break;
            }
            distMea = Phi / this.num_measures; // 要保证等分的是需要的弧段   
            //distMea = (endPhi - startPhi) / this.num_measures; // 要保证等分的是需要的弧段   
            double row, col;
            for (int i = 0; i < this.num_measures; i++)
            {
                GetPointCircle(Row, Column, radius, distMea * i + startPhi, out row, out col);
                this.rect2RowPoints[i] = row;
                this.rect2ColPoints[i] = col;
            }
            /////////////////////////
            this.sourcePoints = new object[7] { Row, Column, radius, startPhi, endPhi, diffRadius, distMea };
            CreateMeasure();
        }
        public void CreateEllipseMeasure(double Row, double Column, double Phi, double radius1, double radius2, double startPhi, double endPhi, double diffRadius, int imageWidth, int imageHeight)
        {
            this.type = "ellipse";
            if (diffRadius >= 0)
                this.measure_direction = enMeasureDirection.从内到外;
            else
                this.measure_direction = enMeasureDirection.从外到内;
            this.measure_length1 = Math.Abs(diffRadius);
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.rect2RowPoints = null;
            this.rect2ColPoints = null;
            this.rect2PhiPoints = null;
            double row, col;
            if (this.num_measures < (int)((Math.PI * 2 * radius2 + 4 * (radius1 - radius2)) * Math.Abs(endPhi - startPhi) / (this.measure_length2 * 2 * Math.PI * 2)))
                this.num_measures = (int)((Math.PI * 2 * radius2 + 4 * (radius1 - radius2)) * Math.Abs(endPhi - startPhi) / (this.measure_length2 * 2 * Math.PI * 2));
            this.rect2RowPoints = new double[this.num_measures];
            this.rect2ColPoints = new double[this.num_measures];
            double distMea;
            if (startPhi <= endPhi)
                distMea = Math.Abs(endPhi - startPhi) / this.num_measures;
            else
                distMea = (Math.PI * 2 - Math.Abs(endPhi - startPhi)) / this.num_measures;
            //////////////////////////////////////////////////////
            for (int i = 0; i < this.num_measures; i++)
            {
                GetPointEllipse(Row, Column, Phi, radius1, radius2, distMea * i + startPhi, out row, out col);
                this.rect2RowPoints[i] = row;
                this.rect2ColPoints[i] = col;
            }
            /////////////////////////
            this.sourcePoints = new object[9] { Row, Column, Phi, radius1, radius2, startPhi, endPhi, diffRadius, distMea };
            CreateMeasure();
        }
        public void CreateRect2Measure(double Row, double Column, double phi, double length1, double length2, double diffRadius, int imageWidth, int imageHeight)
        {
            this.type = "rect2";
            if (diffRadius >= 0) // 这个值只影响测量方向
                this.measure_direction = enMeasureDirection.从内到外;
            else
                this.measure_direction = enMeasureDirection.从外到内;
            this.measure_length1 = Math.Abs(diffRadius);
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.rect2RowPoints = null;
            this.rect2ColPoints = null;
            this.rect2PhiPoints = null;
            /////////////////////////////
            if (this.num_measures < (int)((length1 + length2) * 2 / (this.measure_length2 * 2)))
                this.num_measures = (int)((length1 + length2) * 2 / (this.measure_length2 * 2)); // 自动计算测量区域的数量
            GetPointRect2(Row, Column, phi, length1, length2, out this.rect2RowPoints, out this.rect2ColPoints);
            /////////////////////////
            this.sourcePoints = new object[6] { Row, Column, phi, length1, length2, diffRadius };
            CreateMeasure();
        }
        public void CreateWidthMeasure(double Row, double Column, double phi, double length1, double length2, double diffRadius, int imageWidth, int imageHeight)
        {
            this.type = "width";
            this.measure_length1 = Math.Abs(diffRadius);
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.rect2RowPoints = null;
            this.rect2ColPoints = null;
            this.rect2PhiPoints = null;
            /////////////////////////////
            if (this.num_measures < (int)((length2) * 2 / (this.measure_length2 * 2)))
                this.num_measures = (int)((length2) * 2 / (this.measure_length2 * 2)); // 自动计算测量区域的数量
            GetPointWidth(Row, Column, phi, length1, length2, out this.rect2RowPoints, out this.rect2ColPoints);  // 测量矩形的中心点
            /////////////////////////
            this.sourcePoints = new object[6] { Row, Column, phi, length1, length2, diffRadius };
            CreateMeasure();
        }
        public void ApplyMeasurePose(HImage dataImage)
        {
            HTuple RowEdge, ColumnEdge, Amplitude, Distance, RowEdge2, ColumnEdge2, Amplitude2, Distance2;
            int width, height;
            dataImage.GetImageSize(out width, out height);
            if (this.hMeasure == null || width != this.imageWidth || height != imageHeight) return;
            // 自动调整阈值检测 
            double tempThreshold = this.measure_threshold;
            ////////////////////
            while (true)
            {
                if (listEdgesRow == null || listEdgesCol == null)
                {
                    listEdgesRow = new List<double>();
                    listEdgesCol = new List<double>();
                }
                else
                {
                    listEdgesRow.Clear();
                    listEdgesCol.Clear();
                    listEdgesRow2.Clear();
                    listEdgesCol2.Clear();
                }
                if (listEdgesRow2 == null || listEdgesCol2 == null)
                {
                    listEdgesRow2 = new List<double>();
                    listEdgesCol2 = new List<double>();
                }
                else
                {
                    listEdgesRow2.Clear();
                    listEdgesCol2.Clear();
                }
                string _transition = this.transition.ToString().Substring(0, this.transition.ToString().LastIndexOf('_'));
                for (int i = 0; i < this.hMeasure.Length; i++)
                {
                    if (this.rect2ColPoints[i] < 0 || this.rect2ColPoints[i] > width) continue;
                    if (this.rect2RowPoints[i] < 0 || this.rect2RowPoints[i] > height) continue;
                    switch (this.transition)
                    {
                        default:
                        case enEdgeTransition.all_所有极性:
                        case enEdgeTransition.negative_从白到黑:
                        case enEdgeTransition.positive_从黑到白:
                            this.hMeasure[i].MeasurePos(dataImage, this.measure_sigma, Math.Abs(tempThreshold), _transition, "all", out RowEdge, out ColumnEdge, out Amplitude, out Distance);
                            if (RowEdge == null || RowEdge.Length == 0)
                            {
                                switch (this.FillUpInvalidData)
                                {
                                    case "fastFill_最远填充":
                                    case "FastFill_最远填充":
                                        RowEdge = this.rect2RowPoints[i] - this.measure_length1 * Math.Sin(this.rect2PhiPoints[i]);
                                        ColumnEdge = this.rect2ColPoints[i] + this.measure_length1 * Math.Cos(this.rect2PhiPoints[i]);
                                        Amplitude = 1;
                                        break;
                                    case "NearFill_最近填充":
                                    case "nearFill_最近填充":
                                        RowEdge = this.rect2RowPoints[i] + this.measure_length1 * Math.Sin(this.rect2PhiPoints[i]);
                                        ColumnEdge = this.rect2ColPoints[i] + this.measure_length1 * Math.Cos(this.rect2PhiPoints[i]);
                                        Amplitude = 1;
                                        break;
                                    case "true":
                                    case "true_填充":
                                    case "True_填充":
                                    case "centerFill_中心填充":
                                        RowEdge = this.rect2RowPoints[i];
                                        ColumnEdge = this.rect2ColPoints[i];
                                        Amplitude = 1;
                                        break;
                                }
                            }
                            if (RowEdge != null && RowEdge.Length > 0)
                            {
                                switch (this.select) //.ToString()
                                {
                                    case enEdgeSelect.all_所有边缘:
                                        if (this.type == "cross" || this.type == "width")
                                        {
                                            if (i < (int)this.hMeasure.Length * 0.5)
                                            {
                                                listEdgesRow.AddRange(RowEdge.DArr);
                                                listEdgesCol.AddRange(ColumnEdge.DArr);
                                            }
                                            else
                                            {
                                                listEdgesRow2.AddRange(RowEdge.DArr);
                                                listEdgesCol2.AddRange(ColumnEdge.DArr);
                                            }
                                        }
                                        else
                                        {
                                            listEdgesRow.AddRange(RowEdge.DArr);
                                            listEdgesCol.AddRange(ColumnEdge.DArr);
                                        }
                                        break;
                                    case enEdgeSelect.strongest_最强边缘:
                                        HTuple sortHtuple = Amplitude.TupleAbs().TupleSortIndex();
                                        if (this.type == "cross" || this.type == "width")
                                        {
                                            if (i < (int)this.hMeasure.Length * 0.5)
                                            {
                                                listEdgesRow.AddRange(RowEdge.DArr);
                                                listEdgesCol.AddRange(ColumnEdge.DArr);
                                            }
                                            else
                                            {
                                                listEdgesRow2.AddRange(RowEdge.DArr);
                                                listEdgesCol2.AddRange(ColumnEdge.DArr);
                                            }
                                        }
                                        else
                                        {
                                            listEdgesRow.Add(RowEdge[sortHtuple[sortHtuple.Length - 1].I].D);
                                            listEdgesCol.Add(ColumnEdge[sortHtuple[sortHtuple.Length - 1].I].D);
                                        }
                                        break;
                                    case enEdgeSelect.first_第一个边缘:
                                        if (this.type == "cross" || this.type == "width")
                                        {
                                            if (i < (int)this.hMeasure.Length * 0.5)
                                            {
                                                listEdgesRow.AddRange(RowEdge.DArr);
                                                listEdgesCol.AddRange(ColumnEdge.DArr);
                                            }
                                            else
                                            {
                                                listEdgesRow2.AddRange(RowEdge.DArr);
                                                listEdgesCol2.AddRange(ColumnEdge.DArr);
                                            }
                                        }
                                        else
                                        {
                                            listEdgesRow.Add(RowEdge[0].D);
                                            listEdgesCol.Add(ColumnEdge[0].D);
                                        }
                                        break;
                                    case enEdgeSelect.second_第二个边缘:
                                        if (this.type == "cross" || this.type == "width")
                                        {
                                            if (i < (int)this.hMeasure.Length * 0.5)
                                            {
                                                if (RowEdge.Length > 1)
                                                {
                                                    listEdgesRow.Add(RowEdge[1].D);
                                                    listEdgesCol.Add(ColumnEdge[1].D);
                                                }
                                                else
                                                {
                                                    listEdgesRow.Add(RowEdge[0].D);
                                                    listEdgesCol.Add(ColumnEdge[0].D);
                                                }
                                            }
                                            else
                                            {
                                                if (RowEdge.Length > 1)
                                                {
                                                    listEdgesRow2.Add(RowEdge[1].D);
                                                    listEdgesCol2.Add(ColumnEdge[1].D);
                                                }
                                                else
                                                {
                                                    listEdgesRow2.Add(RowEdge[0].D);
                                                    listEdgesCol2.Add(ColumnEdge[0].D);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (RowEdge.Length > 1)
                                            {
                                                listEdgesRow.Add(RowEdge[1].D);
                                                listEdgesCol.Add(ColumnEdge[1].D);
                                            }
                                            else
                                            {
                                                listEdgesRow.Add(RowEdge[0].D);
                                                listEdgesCol.Add(ColumnEdge[0].D);
                                            }
                                        }
                                        break;
                                    case enEdgeSelect.third_第三个边缘:
                                        if (this.type == "cross" || this.type == "width")
                                        {
                                            if (i < (int)this.hMeasure.Length * 0.5)
                                            {
                                                if (RowEdge.Length > 2)
                                                {
                                                    listEdgesRow.Add(RowEdge[2].D);
                                                    listEdgesCol.Add(ColumnEdge[2].D);
                                                }
                                                else
                                                {
                                                    listEdgesRow.Add(RowEdge[0].D);
                                                    listEdgesCol.Add(ColumnEdge[0].D);
                                                }
                                            }
                                            else
                                            {
                                                if (RowEdge.Length > 2)
                                                {
                                                    listEdgesRow2.Add(RowEdge[2].D);
                                                    listEdgesCol2.Add(ColumnEdge[2].D);
                                                }
                                                else
                                                {
                                                    listEdgesRow2.Add(RowEdge[0].D);
                                                    listEdgesCol2.Add(ColumnEdge[0].D);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (RowEdge.Length > 2)
                                            {
                                                listEdgesRow.Add(RowEdge[2].D);
                                                listEdgesCol.Add(ColumnEdge[2].D);
                                            }
                                            else
                                            {
                                                listEdgesRow.Add(RowEdge[0].D);
                                                listEdgesCol.Add(ColumnEdge[0].D);
                                            }
                                        }
                                        break;
                                    case enEdgeSelect.last_最后一个边缘:
                                        listEdgesRow.Add(RowEdge[RowEdge.Length - 1].D);
                                        listEdgesCol.Add(ColumnEdge[ColumnEdge.Length - 1].D);
                                        break;
                                    case enEdgeSelect.near_最近边缘:
                                        double dist = 100000;
                                        int index = 0;
                                        double temp_dist = 0;
                                        for (int ii = 0; ii < RowEdge.Length; ii++)
                                        {
                                            temp_dist = HMisc.DistancePp(this.rect2RowPoints[i], this.rect2ColPoints[i], RowEdge[ii].D, ColumnEdge[ii].D);
                                            if (temp_dist < dist)
                                            {
                                                dist = temp_dist;
                                                index = ii;
                                            }
                                        }
                                        listEdgesRow.Add(RowEdge[index].D);
                                        listEdgesCol.Add(ColumnEdge[index].D);
                                        break;
                                    case enEdgeSelect.fast_最远边缘:
                                        dist = 0;
                                        index = 0;
                                        temp_dist = 0;
                                        for (int ii = 0; ii < RowEdge.Length; ii++)
                                        {
                                            temp_dist = HMisc.DistancePp(this.rect2RowPoints[i], this.rect2ColPoints[i], RowEdge[ii].D, ColumnEdge[ii].D);
                                            if (temp_dist > dist)
                                            {
                                                dist = temp_dist;
                                                index = ii;
                                            }
                                        }
                                        listEdgesRow.Add(RowEdge[index].D);
                                        listEdgesCol.Add(ColumnEdge[index].D);
                                        break;
                                }
                            }
                            break;
                        case enEdgeTransition.all_strongest_最强边缘:     // 这些边缘变换表示使用的是: 测量边缘对
                        case enEdgeTransition.negative_strongest_从白到黑:
                        case enEdgeTransition.positive_strongest_从黑到白:
                            this.hMeasure[i].MeasurePairs(dataImage, this.measure_sigma, Math.Abs(tempThreshold), _transition, "all", out RowEdge, out ColumnEdge, out Amplitude,
                                                          out RowEdge2, out ColumnEdge2, out Amplitude2, out Distance, out Distance2);
                            if (RowEdge != null && RowEdge.Length > 0 && RowEdge2 != null && RowEdge2.Length > 0)
                            {
                                switch (this.select)
                                {
                                    case enEdgeSelect.all_所有边缘:
                                        listEdgesRow.AddRange(RowEdge.DArr);
                                        listEdgesCol.AddRange(ColumnEdge.DArr);
                                        listEdgesRow2.AddRange(RowEdge2.DArr);
                                        listEdgesCol2.AddRange(RowEdge2.DArr);
                                        break;
                                    default:
                                    case enEdgeSelect.strongest_最强边缘:
                                        HTuple sortHtuple = Amplitude.TupleAbs().TupleSortIndex();
                                        HTuple sortHtuple2 = Amplitude2.TupleAbs().TupleSortIndex();
                                        listEdgesRow.Add(RowEdge[sortHtuple[sortHtuple.Length - 1].I].D);
                                        listEdgesCol.Add(ColumnEdge[sortHtuple[sortHtuple.Length - 1].I].D);
                                        listEdgesRow2.Add(RowEdge2[sortHtuple2[sortHtuple2.Length - 1].I].D);
                                        listEdgesCol2.Add(ColumnEdge2[sortHtuple2[sortHtuple2.Length - 1].I].D);
                                        break;
                                    case enEdgeSelect.first_第一个边缘:
                                        listEdgesRow.Add(RowEdge[0].D);
                                        listEdgesCol.Add(ColumnEdge[0].D);
                                        listEdgesRow2.Add(RowEdge2[0].D);
                                        listEdgesCol2.Add(ColumnEdge2[0].D);
                                        break;
                                    case enEdgeSelect.second_第二个边缘:
                                        if (RowEdge.Length > 1)
                                        {
                                            listEdgesRow.Add(RowEdge[1].D);
                                            listEdgesCol.Add(ColumnEdge[1].D);
                                        }
                                        else
                                        {
                                            listEdgesRow.Add(RowEdge[0].D);
                                            listEdgesCol.Add(ColumnEdge[0].D);
                                        }
                                        if (RowEdge2.Length > 1)
                                        {
                                            listEdgesRow2.Add(RowEdge2[1].D);
                                            listEdgesCol2.Add(ColumnEdge2[1].D);
                                        }
                                        else
                                        {
                                            listEdgesRow2.Add(RowEdge2[0].D);
                                            listEdgesCol2.Add(ColumnEdge2[0].D);
                                        }
                                        break;
                                    case enEdgeSelect.third_第三个边缘:
                                        if (RowEdge.Length > 2)
                                        {
                                            listEdgesRow.Add(RowEdge[2].D);
                                            listEdgesCol.Add(ColumnEdge[2].D);
                                        }
                                        else
                                        {
                                            listEdgesRow.Add(RowEdge[0].D);
                                            listEdgesCol.Add(ColumnEdge[0].D);
                                        }
                                        if (RowEdge2.Length > 2)
                                        {
                                            listEdgesRow2.Add(RowEdge2[2].D);
                                            listEdgesCol2.Add(ColumnEdge2[2].D);
                                        }
                                        else
                                        {
                                            listEdgesRow2.Add(RowEdge2[0].D);
                                            listEdgesCol2.Add(ColumnEdge2[0].D);
                                        }
                                        break;
                                    case enEdgeSelect.last_最后一个边缘:
                                        listEdgesRow.Add(RowEdge[RowEdge.Length - 1].D);
                                        listEdgesCol.Add(ColumnEdge[ColumnEdge.Length - 1].D);
                                        listEdgesRow2.Add(RowEdge[RowEdge2.Length - 1].D);
                                        listEdgesCol2.Add(ColumnEdge[ColumnEdge2.Length - 1].D);
                                        break;

                                    case enEdgeSelect.near_最近边缘:
                                        double dist = 100000;
                                        int index = 0;
                                        double temp_dist = 0;
                                        for (int ii = 0; ii < RowEdge.Length; ii++)
                                        {
                                            temp_dist = HMisc.DistancePp(this.rect2RowPoints[i], this.rect2ColPoints[i], RowEdge[ii].D, ColumnEdge[ii].D);
                                            if (temp_dist < dist)
                                            {
                                                dist = temp_dist;
                                                index = ii;
                                            }
                                        }
                                        listEdgesRow.Add(RowEdge[index].D);
                                        listEdgesCol.Add(ColumnEdge[index].D);
                                        break;
                                }
                            }
                            break;
                    }
                }
                //////////////////////////////////////
                if (listEdgesRow.Count < this.hMeasure.Length * this._dataPercent)
                {
                    if (tempThreshold < 0) // 如果阈值 < 0 ,表示使用固定阈值
                        break;
                    else
                    {
                        tempThreshold -= 5;
                        if (tempThreshold < 2)
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
            /////////////////////////////////
            foreach (var item in this.hMeasure)
            {
                item.Dispose();
            }
        }
        public void ApplyMeasurePairs(HImage dataImage)
        {
            int width, height;
            dataImage.GetImageSize(out width, out height);
            if (this.hMeasure == null || width != this.imageWidth || height != imageHeight) return;
            HTuple RowEdge, ColumnEdge, Amplitude, Distance, RowEdge2, ColumnEdge2, Amplitude2, Distance2;
            if (listEdgesRow == null || listEdgesCol == null)
            {
                listEdgesRow = new List<double>();
                listEdgesCol = new List<double>();
            }
            else
            {
                listEdgesRow.Clear();
                listEdgesCol.Clear();
            }
            if (listEdgesRow2 == null || listEdgesCol2 == null)
            {
                listEdgesRow2 = new List<double>();
                listEdgesCol2 = new List<double>();
            }
            else
            {
                listEdgesRow2.Clear();
                listEdgesCol2.Clear();
            }
            double tempThreshold = this.measure_threshold;
            for (int i = 0; i < this.hMeasure.Length; i++)
            {
                // 测量边缘对，边缘极性得选ALl
                this.hMeasure[i].MeasurePairs(dataImage, this.measure_sigma, Math.Abs(tempThreshold), "all", "all", out RowEdge, out ColumnEdge, out Amplitude, out RowEdge2, out ColumnEdge2, out Amplitude2, out Distance, out Distance2);
                if (RowEdge != null && RowEdge.Length > 0)
                {
                    listEdgesRow.AddRange(RowEdge.DArr);
                    listEdgesCol.AddRange(ColumnEdge.DArr);
                }
                if (RowEdge2 != null && RowEdge2.Length > 0)
                {
                    listEdgesRow2.AddRange(RowEdge2.DArr);
                    listEdgesCol2.AddRange(ColumnEdge2.DArr);
                }
            }
            ////////////////////////////
            foreach (var item in this.hMeasure)
            {
                item.Dispose();
            }
        }
        public void ApplyFuzzyMeasurePos(HImage dataImage)
        {
            int width, height;
            dataImage.GetImageSize(out width, out height);
            if (this.hMeasure == null || width != this.imageWidth || height != imageHeight) return;
            HTuple RowEdge, ColumnEdge, Amplitude, Distance, FuzzyScore;
            if (listEdgesRow == null || listEdgesCol == null)
            {
                listEdgesRow = new List<double>();
                listEdgesCol = new List<double>();
            }
            else
            {
                listEdgesRow.Clear();
                listEdgesCol.Clear();
            }
            double tempThreshold = this.measure_threshold;
            for (int i = 0; i < this.hMeasure.Length; i++)
            {
                this.hMeasure[i].FuzzyMeasurePos(dataImage, this.measure_sigma, Math.Abs(tempThreshold), this.fuzzyThresh, this.transition.ToString().Split('_')[0], out RowEdge, out ColumnEdge, out Amplitude, out FuzzyScore, out Distance);
                if (RowEdge != null && RowEdge.Length > 0)
                {
                    listEdgesRow.AddRange(RowEdge.DArr);
                    listEdgesCol.AddRange(ColumnEdge.DArr);
                }
            }
            foreach (var item in this.hMeasure)
            {
                item.Dispose();
            }
        }
        public void ApplyFuzzyMeasurePairs(HImage dataImage)
        {
            int width, height;
            dataImage.GetImageSize(out width, out height);
            if (this.hMeasure == null || width != this.imageWidth || height != imageHeight) return;
            HTuple RowEdge, ColumnEdge, Amplitude, Distance, RowEdge2, ColumnEdge2, Amplitude2, Distance2, RowEdgeCenter, ColumnEdgeCenter, FuzzyScore;
            if (listEdgesRow == null || listEdgesCol == null)
            {
                listEdgesRow = new List<double>();
                listEdgesCol = new List<double>();
            }
            else
            {
                listEdgesRow.Clear();
                listEdgesCol.Clear();
            }
            if (listEdgesRow2 == null || listEdgesCol2 == null)
            {
                listEdgesRow2 = new List<double>();
                listEdgesCol2 = new List<double>();
            }
            else
            {
                listEdgesRow2.Clear();
                listEdgesCol2.Clear();
            }
            double tempThreshold = this.measure_threshold;
            for (int i = 0; i < this.hMeasure.Length; i++)
            {
                this.hMeasure[i].FuzzyMeasurePairs(dataImage, this.measure_sigma, Math.Abs(tempThreshold), this.fuzzyThresh, this.transition.ToString().Split('_')[0], out RowEdge, out ColumnEdge, out Amplitude, out RowEdge2, out ColumnEdge2, out Amplitude2, out RowEdgeCenter, out ColumnEdgeCenter, out FuzzyScore, out Distance, out Distance2);
                if (RowEdge != null && RowEdge.Length > 0)
                {
                    listEdgesRow.AddRange(RowEdge.DArr);
                    listEdgesCol.AddRange(ColumnEdge.DArr);
                }
                if (RowEdge2 != null && RowEdge2.Length > 0)
                {
                    listEdgesRow2.AddRange(RowEdge2.DArr);
                    listEdgesCol2.AddRange(ColumnEdge2.DArr);
                }
            }
            ////////////////////////////
            foreach (var item in this.hMeasure)
            {
                item.Dispose();
            }
        }
        public void SetFuzzyMeasure(string SetType, HFunction1D Function)
        {
            for (int i = 0; i < this.hMeasure.Length; i++)
            {
                this.hMeasure[i].SetFuzzyMeasure(SetType, Function);
            }
        }
        public void SetCameraParam(HTuple camParam, HTuple camPose)
        {
            this.camParam = new userCamParam(camParam);
            this.camPose = new userCamPose(camPose);
        }
        public void SetCameraParam(userCamParam camParam, userCamPose camPose)
        {
            this.camParam = camParam;
            this.camPose = camPose;
        }
        public void GetMeasureObjectParam(enParamType paramType, out HTuple Parameter)
        {
            Parameter = new HTuple();
            if (this.listEdgesRow.Count == 0) return;
            switch (paramType)
            {
                case enParamType.row_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesRow.ToArray());
                            break;
                    }
                    break;
                case enParamType.column_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesCol.ToArray());
                            break;
                    }
                    break;
                case enParamType.row_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesRow2.ToArray());
                            break;
                    }
                    break;
                case enParamType.column_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesCol2.ToArray());
                            break;
                    }
                    break;
                case enParamType.X_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y;
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X);
                            }
                            else
                                Parameter = new HTuple(listEdgesCol.ToArray());
                            break;
                    }
                    break;
                case enParamType.X_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y;
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X);
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesCol.ToArray());
                            }
                            break;
                    }
                    break;
                case enParamType.Y_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y;
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(Y);
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesRow.ToArray());
                            }
                            break;
                    }
                    break;
                case enParamType.Y_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y;
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(Y);
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesRow.ToArray());
                            }
                            break;
                    }
                    break;
                case enParamType.fitResult_Edges1:
                    switch (this.type)
                    {
                        case "point":
                            HTuple X, Y;
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X[0].D, Y[0].D);
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesRow[0], listEdgesCol[0]);
                            }
                            break;
                        case "line":
                            HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist;
                            new HXLDCont(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray())).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(RowBegin, RowEnd), new HTuple(ColBegin, ColEnd), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X[0].D, Y[0].D, X[1].D, Y[1].D);
                            }
                            else
                            {
                                Parameter = new HTuple(RowBegin, ColBegin, RowEnd, ColEnd);
                            }
                            break;
                        case "circle":
                            HTuple Row = null, Column = null, Radius1 = null, Radius2 = null, Phi = null, StartPhi = null, EndPhi = null, PointOrder = null;
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), 1, out X, out Y);
                            new HXLDCont(Y * -1, X).FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out Row, out Column, out Radius1, out StartPhi, out EndPhi, out PointOrder);
                            if (Row != null && Row.Length > 0)
                            {
                                // 由于measure_length2的存在，导致在测量圆弧类尺寸，会导致测量值比实际值偏小，测量该类型对象时就使测量区域宽度尽量小，所以这里加了一个补偿值compensationRadius
                                Parameter = new HTuple(Column[0].D, Row[0].D * -1, Radius1[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D);
                            }
                            else
                                Parameter = new HTuple();
                            break;
                        case "ellipse":
                            Row = null; Column = null; Radius1 = null; Radius2 = null; Phi = null; StartPhi = null; EndPhi = null; PointOrder = null;
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), 1, out X, out Y);
                            new HXLDCont(Y * -1, X).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                            if (Row != null && Row.Length > 0)
                            {
                                // 由于measure_length2的存在，导致在测量圆弧类尺寸，会导致测量值比实际值偏小，测量该类型对象时就使测量区域宽度尽量小，所以这里加了一个补偿值compensationRadius
                                Parameter = new HTuple(Column[0].D, Row[0].D * -1, Phi.TupleDeg()[0].D, Radius1[0].D, Radius2[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D);
                            }
                            else
                                Parameter = new HTuple();
                            break;
                        case "rect2":
                            Row = null; Column = null; Radius1 = null; Radius2 = null; Phi = null; StartPhi = null; EndPhi = null; PointOrder = null;
                            HTuple Length1 = null, Length2 = null;

                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), 1, out X, out Y);
                            new HXLDCont(Y * -1, X).FitRectangle2ContourXld("regression", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                            if (Row != null && Row.Length > 0)
                            {
                                Parameter = new HTuple(Column[0].D, Row[0].D * -1, Phi.TupleDeg()[0].D, Length1[0], Length2[0]);
                            }
                            else
                                Parameter = new HTuple();
                            break;
                    }
                    break;
                case enParamType.fitResult_Edges2:
                    switch (this.type)
                    {
                        case "point":
                            HTuple X, Y;
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X[0].D, Y[0].D, X[1].D, Y[1].D);
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesRow[0], listEdgesCol[0]);
                            }
                            break;
                        case "line":
                            HTuple RowBegin = null, ColBegin = null, RowEnd = null, ColEnd = null, Nr = null, Nc = null, Dist = null;
                            new HXLDCont(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray())).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(RowBegin, RowEnd), new HTuple(ColBegin, ColEnd), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X[0].D, Y[0].D, X[1].D, Y[1].D);
                            }
                            else
                            {
                                Parameter = new HTuple(RowBegin, ColBegin, RowEnd, ColEnd);
                            }
                            break;
                        case "circle":
                            HTuple Row, Column, Radius1, Radius2, Phi, StartPhi, EndPhi, PointOrder;
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), 1, out X, out Y);
                            new HXLDCont(Y * -1, X).FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out Row, out Column, out Radius1, out StartPhi, out EndPhi, out PointOrder);
                            if (Row != null && Row.Length > 0)
                            {
                                // 由于measure_length2的存在，导致在测量圆弧类尺寸，会导致测量值比实际值偏小，测量该类型对象时就使测量区域宽度尽量小，所以这里加了一个补偿值compensationRadius
                                Parameter = new HTuple(Column[0].D, Row[0].D * -1, Radius1[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D);
                            }
                            else
                                Parameter = new HTuple();
                            break;
                        case "ellipse":
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), 1, out X, out Y);
                            new HXLDCont(Y * -1, X).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                            if (Row != null && Row.Length > 0)
                            {
                                // 由于measure_length2的存在，导致在测量圆弧类尺寸，会导致测量值比实际值偏小，测量该类型对象时就使测量区域宽度尽量小，所以这里加了一个补偿值compensationRadius
                                Parameter = new HTuple(Column[0].D, Row[0].D * -1, Phi.TupleDeg()[0].D, Radius1[0].D, Radius2[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D);
                            }
                            else
                                Parameter = new HTuple();
                            break;
                        case "rect2":
                            HTuple Length1, Length2;
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), 1, out X, out Y);
                            new HXLDCont(Y * -1, X).FitRectangle2ContourXld("regression", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                            if (Row != null && Row.Length > 0)
                            {
                                Parameter = new HTuple(Column[0].D, Row[0].D * -1, Phi.TupleDeg()[0].D, Length1[0].D, Length2[0].D);
                            }
                            else
                                Parameter = new HTuple();
                            break;
                    }
                    break;
            }

        }
        public void GetMeasureObjectParam(CameraParam camParams, double Grab_X, double Grab_Y, double Grab_Z, enParamType paramType, out HTuple Parameter)
        {
            Parameter = new HTuple();
            //if (this.listEdgesRow.Count == 0) return;
            switch (paramType)
            {
                case enParamType.row_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesRow.ToArray());
                            break;
                    }
                    break;
                case enParamType.column_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesCol.ToArray());
                            break;
                    }
                    break;
                case enParamType.row_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesRow2.ToArray());
                            break;
                    }
                    break;
                case enParamType.column_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesCol2.ToArray());
                            break;
                    }
                    break;
                case enParamType.X_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y, Z;
                            if (this.listEdgesRow.Count > 0)
                            {
                                camParams.ImagePointsToWorldPlane(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                Parameter = new HTuple(X);
                            }
                            else
                            {
                                Parameter = new HTuple(0);
                            }
                            break;
                        case "cross":
                        case "width":
                            if (this.listEdgesRow.Count > 0)
                            {
                                camParams.ImagePointsToWorldPlane(new HTuple(this.listEdgesRow.ToArray(), this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol.ToArray(), this.listEdgesCol2.ToArray()), Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                Parameter = new HTuple(X);
                            }
                            else
                            {
                                Parameter = new HTuple(0);
                            }
                            break;
                    }
                    break;
                case enParamType.X_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y, Z;
                            if (this.listEdgesRow2.Count > 0)
                            {
                                camParams.ImagePointsToWorldPlane(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                Parameter = new HTuple(X);
                            }
                            else
                            {
                                Parameter = new HTuple(0);
                            }
                            break;
                    }
                    break;
                case enParamType.Y_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y, Z;
                            if (this.listEdgesRow.Count > 0)
                            {
                                camParams.ImagePointsToWorldPlane(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                Parameter = new HTuple(Y);
                            }
                            else
                            {
                                Parameter = new HTuple(0);
                            }
                            break;
                        case "cross":
                        case "width":
                            if (this.listEdgesRow.Count > 0)
                            {
                                camParams.ImagePointsToWorldPlane(new HTuple(this.listEdgesRow.ToArray(), this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol.ToArray(), this.listEdgesCol2.ToArray()), Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                Parameter = new HTuple(Y);
                            }
                            else
                            {
                                Parameter = new HTuple(0);
                            }
                            break;
                    }
                    break;
                case enParamType.Y_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y, Z;
                            if (this.listEdgesRow2.Count > 0)
                            {
                                camParams.ImagePointsToWorldPlane(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                Parameter = new HTuple(Y);
                            }
                            else
                            {
                                Parameter = new HTuple(0);
                            }
                            break;
                    }
                    break;
                case enParamType.fitResult_Edges1:
                    switch (this.type)
                    {
                        case "point":
                            HTuple X, Y, X2, Y2, Z;
                            if (this.listEdgesRow.Count == 0)
                            {
                                Parameter = new HTuple(-1, -1);
                            }
                            else
                            {
                                camParams.ImagePointsToWorldPlane(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D);
                                }
                                else
                                {
                                    Parameter = new HTuple();
                                }
                            }
                            break;
                        case "cross":
                            HTuple RowBegin1, ColBegin1, RowEnd1, ColEnd1, RowBegin2, ColBegin2, RowEnd2, ColEnd2, Nr12, Nc12, Dist12, row, col, ispra;
                            if (this.listEdgesRow.Count == 0 || this.listEdgesRow2.Count == 0)
                            {
                                Parameter = new HTuple();
                            }
                            else
                            {
                                new HXLDCont(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray())).FitLineContourXld("tukey", -1, 0, 10, 2, out RowBegin1, out ColBegin1, out RowEnd1, out ColEnd1, out Nr12, out Nc12, out Dist12);
                                new HXLDCont(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray())).FitLineContourXld("tukey", -1, 0, 10, 2, out RowBegin2, out ColBegin2, out RowEnd2, out ColEnd2, out Nr12, out Nc12, out Dist12);
                                HMisc.IntersectionLl(RowBegin1, ColBegin1, RowEnd1, ColEnd1, RowBegin2, ColBegin2, RowEnd2, ColEnd2, out row, out col, out ispra);
                                camParams.ImagePointsToWorldPlane(row, col, Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D);
                                }
                                else
                                {
                                    Parameter = new HTuple();
                                }
                            }
                            break;
                        case "line":
                            HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist;
                            if (this.listEdgesRow.Count >= 2)
                            {
                                new HXLDCont(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray())).FitLineContourXld("tukey", -1, 0, 10, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                                camParams.ImagePointsToWorldPlane(new HTuple(RowBegin, RowEnd), new HTuple(ColBegin, ColEnd), Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D, X[1].D, Y[1].D, Z[0].D);
                                }
                                else
                                {
                                    Parameter = new HTuple(RowBegin.D, ColBegin.D, 0.0, RowEnd.D, ColEnd.D, 0.0);
                                }
                            }
                            else
                            {
                                RowBegin = Convert.ToDouble(this.sourcePoints[0]);
                                ColBegin = Convert.ToDouble(this.sourcePoints[1]);
                                RowEnd = Convert.ToDouble(this.sourcePoints[2]);
                                ColEnd = Convert.ToDouble(this.sourcePoints[3]);
                                camParams.ImagePointsToWorldPlane(new HTuple(RowBegin, RowEnd), new HTuple(ColBegin, ColEnd), Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D, X[1].D, Y[1].D, Z[0].D);
                            }
                            break;
                        case "circle":
                            HTuple Row = null, Column = null, Radius1 = null, Radius2 = null, Phi = null, StartPhi = null, EndPhi = null, PointOrder = null;
                            if (this.listEdgesRow.Count >= 3)
                            {
                                new HXLDCont(this.listEdgesRow.ToArray(), this.listEdgesCol.ToArray()).FitCircleContourXld("algebraic", -1, 0, 0, 10, 1, out Row, out Column, out Radius1, out StartPhi, out EndPhi, out PointOrder);
                                camParams.ImagePointsToWorldPlane(Row, Column, Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                double radius = camParams.TransPixLengthToWcsLength(Radius1[0].D);
                                // }
                                if (Row != null && Row.Length > 0)
                                {
                                    // 由于measure_length2的存在，导致在测量圆弧类尺寸，会导致测量值比实际值偏小，测量该类型对象时就使测量区域宽度尽量小，所以这里加了一个补偿值compensationRadius
                                    Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D, radius, StartPhi.TupleDeg().D, EndPhi.TupleDeg().D, PointOrder.S);
                                }
                                else
                                {
                                    Parameter = new HTuple();
                                }
                            }
                            else
                            {
                                Row = Convert.ToDouble(this.sourcePoints[0]);
                                Column = Convert.ToDouble(this.sourcePoints[1]);
                                Radius1 = Convert.ToDouble(this.sourcePoints[2]);
                                camParams.ImagePointsToWorldPlane(Row, Column, Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                double radius = camParams.TransPixLengthToWcsLength(Radius1[0].D);
                                Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D, radius, 0, 360, "positive");
                            }
                            break;
                        case "ellipse":
                            Row = null; Column = null; Radius1 = null; Radius2 = null; Phi = null; StartPhi = null; EndPhi = null; PointOrder = null;
                            new HXLDCont(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray())).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 10, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                            camParams.ImagePointsToWorldPlane(Row, Column, Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                            Radius1 = camParams.TransPixLengthToWcsLength(Radius1[0].D);
                            Radius2 = camParams.TransPixLengthToWcsLength(Radius2[0].D);
                            //}
                            if (Row != null && Row.Length > 0)
                            {
                                // 由于measure_length2的存在，导致在测量圆弧类尺寸，会导致测量值比实际值偏小，测量该类型对象时就使测量区域宽度尽量小，所以这里加了一个补偿值compensationRadius
                                Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D, Phi.TupleDeg()[0].D, Radius1[0].D, Radius2[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D, PointOrder.S);
                            }
                            else
                            {
                                Parameter = new HTuple();
                            }
                            break;
                        case "rect2":
                            HTuple Length1, Length2;
                            new HXLDCont(this.listEdgesRow.ToArray(), this.listEdgesCol.ToArray()).FitRectangle2ContourXld("regression", -1, 0, 0, 10, 5, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                            camParams.ImagePointsToWorldPlane(Row, Column, Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                            double len1 = camParams.TransPixLengthToWcsLength(Length1.D);
                            double len2 = camParams.TransPixLengthToWcsLength(Length2.D);
                            double deg = Phi.TupleDeg()[0].D * 1;
                            if (Row != null && Row.Length > 0)
                            {
                                Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D, deg, len1, len2);
                            }
                            else
                            {
                                Parameter = new HTuple();
                            }
                            break;
                        case "width":
                            double rowBegin1, colBegin1, rowEnd1, colEnd1, rowBegin2, colBegin2, rowEnd2, colEnd2, nr12, nc12, dist12, rowProj1, colProj1, rowProj2, colProj2;
                            if (this.listEdgesRow.Count == 0 || this.listEdgesRow2.Count == 0)
                            {
                                Parameter = new HTuple(-1, -1, 0, 0, 0);
                            }
                            else
                            {
                                new HXLDCont(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray())).FitLineContourXld("tukey", -1, 0, 10, 2, out rowBegin1, out colBegin1, out rowEnd1, out colEnd1, out nr12, out nc12, out dist12);
                                new HXLDCont(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray())).FitLineContourXld("tukey", -1, 0, 10, 2, out rowBegin2, out colBegin2, out rowEnd2, out colEnd2, out nr12, out nc12, out dist12);
                                HMisc.ProjectionPl((rowBegin1), (colBegin1), rowBegin2, colBegin2, rowEnd2, colEnd2, out rowProj1, out colProj1);
                                HMisc.ProjectionPl((rowEnd1), (colEnd1), rowBegin2, colBegin2, rowEnd2, colEnd2, out rowProj2, out colProj2);
                                new HXLDCont(new HTuple(rowBegin1, rowEnd1, rowProj1, rowProj2, rowBegin1),
                                             new HTuple(colBegin1, colEnd1, colProj1, colProj2, colBegin1)).SmallestRectangle2Xld(out Row, out Column, out Phi, out Length1, out Length2);  // .FitRectangle2ContourXld("tukey", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                                camParams.ImagePointsToWorldPlane(Row, Column, Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                len1 = camParams.TransPixLengthToWcsLength(Length1);
                                len2 = camParams.TransPixLengthToWcsLength(Length2);
                                deg = Phi.TupleDeg().D * 1;
                                /////////////////////
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D, deg, len1, len2);
                                }
                                else
                                {
                                    Parameter = new HTuple();
                                }
                            }
                            break;
                    }
                    break;
                case enParamType.fitResult_Edges2:
                    switch (this.type)
                    {
                        case "point":
                            HTuple X, Y, Z;
                            if (this.listEdgesRow2.Count == 0)
                            {
                                Parameter = new HTuple();
                            }
                            else
                            {
                                camParams.ImagePointsToWorldPlane(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D, X[1].D, Y[1].D);
                                }
                                else
                                {
                                    Parameter = new HTuple(listEdgesRow[0], listEdgesCol[0], 0);
                                }
                            }
                            break;
                        case "line":
                            HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist;
                            new HXLDCont(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray())).FitLineContourXld("tukey", -1, 0, 10, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                            camParams.ImagePointsToWorldPlane(new HTuple(RowBegin, RowEnd), new HTuple(ColBegin, ColEnd), Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D, X[1].D, Y[1].D, Z[0].D);
                            }
                            else
                            {
                                Parameter = new HTuple();
                            }
                            break;
                        case "circle":
                            HTuple Row = null, Column = null, Radius1 = null, Radius2 = null, Phi = null, StartPhi = null, EndPhi = null, PointOrder = null;
                            new HXLDCont(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray())).FitCircleContourXld("algebraic", -1, 0, 0, 10, 2, out Row, out Column, out Radius1, out StartPhi, out EndPhi, out PointOrder);
                            camParams.ImagePointsToWorldPlane(Row, Column, Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                            if (Row != null && Row.Length > 0)
                            {
                                // 由于measure_length2的存在，导致在测量圆弧类尺寸，会导致测量值比实际值偏小，测量该类型对象时就使测量区域宽度尽量小，所以这里加了一个补偿值compensationRadius
                                Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D, Radius1[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D, PointOrder.S);
                            }
                            else
                            {
                                Parameter = new HTuple();
                            }
                            break;
                        case "ellipse":
                            new HXLDCont(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray())).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 10, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                            camParams.ImagePointsToWorldPlane(Row, Column, Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                            if (Row != null && Row.Length > 0)
                            {
                                // 由于measure_length2的存在，导致在测量圆弧类尺寸，会导致测量值比实际值偏小，测量该类型对象时就使测量区域宽度尽量小，所以这里加了一个补偿值compensationRadius
                                Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D, Phi.TupleDeg()[0].D, Radius1[0].D, Radius2[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D, PointOrder.S);
                            }
                            else
                                Parameter = new HTuple();
                            break;
                        case "rect2":
                            HTuple Length1, Length2;
                            new HXLDCont(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray())).FitRectangle2ContourXld("regression", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                            camParams.ImagePointsToWorldPlane(Row, Column, Grab_X, Grab_Y, Grab_Z, out X, out Y, out Z);
                            if (Row != null && Row.Length > 0)
                            {
                                Parameter = new HTuple(X[0].D, Y[0].D, Z[0].D, Phi.TupleDeg()[0].D, Length1[0].D, Length2[0].D);
                            }
                            else
                                Parameter = new HTuple();
                            //}
                            break;
                    }
                    break;
            }

        }
        public void GetMeasureObjectParam(AxisCalibration calibrateFile, enParamType paramType, out HTuple Parameter)
        {
            Parameter = new HTuple();
            if (this.listEdgesRow.Count == 0) return;
            switch (paramType)
            {
                case enParamType.row_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesRow.ToArray());
                            break;
                    }
                    break;
                case enParamType.column_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesCol.ToArray());
                            break;
                    }
                    break;
                case enParamType.row_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesRow2.ToArray());
                            break;
                    }
                    break;
                case enParamType.column_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesCol2.ToArray());
                            break;
                    }
                    break;
                case enParamType.X_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y;
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), camParam, camPose, out X, out Y);
                            //else
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X);
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesCol.ToArray());
                            }
                            break;
                    }
                    break;
                case enParamType.X_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y;
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), camParam, camPose, out X, out Y);
                            //else
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X);
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesCol.ToArray());
                            }
                            break;
                    }
                    break;
                case enParamType.Y_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y;
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), camParam, camPose, out X, out Y);
                            //else
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(Y);
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesRow.ToArray());
                            }
                            break;
                    }
                    break;
                case enParamType.Y_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y;
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), camParam, camPose, out X, out Y);
                            //else
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(Y);
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesRow.ToArray());
                            }
                            break;
                    }
                    break;
                case enParamType.fitResult_Edges1:
                    switch (this.type)
                    {
                        case "point":
                            HTuple X, Y;
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), camParam, camPose, out X, out Y);
                            //else
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X[0].D, Y[0].D);
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesRow[0], listEdgesCol[0]);
                            }
                            break;
                        case "line":
                            HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist;
                            new HXLDCont(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray())).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);

                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(RowBegin, RowEnd), new HTuple(ColBegin, ColEnd), camParam, camPose, out X, out Y);
                            //else
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(RowBegin, RowEnd), new HTuple(ColBegin, ColEnd), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X[0].D, Y[0].D, X[1].D, Y[1].D);
                            }
                            else
                            {
                                Parameter = new HTuple(RowBegin, ColBegin, RowEnd, ColEnd);
                            }
                            break;
                        case "circle":
                            HTuple Row = null, Column = null, Radius1 = null, Radius2 = null, Phi = null, StartPhi = null, EndPhi = null, PointOrder = null;
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //{
                            //    // 因为在测量圆弧时，圆心和半径有可能会很大，所以这里只能先将边缘点转换到世界坐标系中再拟合圆
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), camParam, camPose, out X, out Y);
                            //    new HXLDCont(Y * -1, X).FitCircleContourXld("algebraic", -1, 0, 0, 3, 1, out Row, out Column, out Radius1, out StartPhi, out EndPhi, out PointOrder);
                            //}
                            //else
                            //{
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), 1, out X, out Y);
                            new HXLDCont(Y * -1, X).FitCircleContourXld("algebraic", -1, 0, 0, 3, 1, out Row, out Column, out Radius1, out StartPhi, out EndPhi, out PointOrder);
                            // }
                            if (Row != null && Row.Length > 0)
                            {
                                // 由于measure_length2的存在，导致在测量圆弧类尺寸，会导致测量值比实际值偏小，测量该类型对象时就使测量区域宽度尽量小，所以这里加了一个补偿值compensationRadius
                                Parameter = new HTuple(Column[0].D, Row[0].D * -1, Radius1[0].D, StartPhi.TupleDeg().D, EndPhi.TupleDeg().D, PointOrder.S);
                            }
                            else
                            {
                                Parameter = new HTuple();
                            }
                            break;
                        case "ellipse":
                            Row = null; Column = null; Radius1 = null; Radius2 = null; Phi = null; StartPhi = null; EndPhi = null; PointOrder = null;
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //{
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), camParam, camPose, out X, out Y);
                            //    new HXLDCont(Y * -1, X).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                            //}
                            //else
                            //{
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), 1, out X, out Y);
                            new HXLDCont(Y * -1, X).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                            //}
                            if (Row != null && Row.Length > 0)
                            {
                                // 由于measure_length2的存在，导致在测量圆弧类尺寸，会导致测量值比实际值偏小，测量该类型对象时就使测量区域宽度尽量小，所以这里加了一个补偿值compensationRadius
                                Parameter = new HTuple(Column[0].D, Row[0].D * -1, Phi.TupleDeg()[0].D, Radius1[0].D, Radius2[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D, PointOrder.S);
                            }
                            else
                            {
                                Parameter = new HTuple();
                            }
                            break;
                        case "rect2":
                            HTuple Length1, Length2;
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //{
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), camParam, camPose, out X, out Y);
                            //    new HXLDCont(Y * -1, X).FitRectangle2ContourXld("regression", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                            //}
                            //else
                            //{
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray()), 1, out X, out Y);
                            new HXLDCont(Y * -1, X).FitRectangle2ContourXld("regression", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                            //}
                            if (Row != null && Row.Length > 0)
                            {
                                Parameter = new HTuple(Column[0].D, Row[0].D * -1, Phi.TupleDeg()[0].D, Length1[0].D, Length2[0].D);
                            }
                            else
                            {
                                Parameter = new HTuple();
                            }
                            break;
                    }
                    break;
                case enParamType.fitResult_Edges2:
                    switch (this.type)
                    {
                        case "point":
                            HTuple X, Y;
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), camParam, camPose, out X, out Y);
                            //else
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X[0].D, Y[0].D, X[1].D, Y[1].D);
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesRow[0], listEdgesCol[0]);
                            }
                            break;
                        case "line":
                            HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist;
                            new HXLDCont(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray())).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(RowBegin, RowEnd), new HTuple(ColBegin, ColEnd), camParam, camPose, out X, out Y);
                            //else
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(RowBegin, RowEnd), new HTuple(ColBegin, ColEnd), 1, out X, out Y);
                            if (X.Length > 0)
                            {
                                Parameter = new HTuple(X[0].D, Y[0].D, X[1].D, Y[1].D);
                            }
                            else
                            {
                                Parameter = new HTuple(RowBegin, ColBegin, RowEnd, ColEnd);
                            }
                            break;
                        case "circle":
                            HTuple Row = null, Column = null, Radius1 = null, Radius2 = null, Phi = null, StartPhi = null, EndPhi = null, PointOrder = null;
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //{
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), camParam, camPose, out X, out Y);
                            //    new HXLDCont(Y * -1, X).FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out Row, out Column, out Radius1, out StartPhi, out EndPhi, out PointOrder);
                            //}
                            //else
                            //{
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), 1, out X, out Y);
                            new HXLDCont(Y * -1, X).FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out Row, out Column, out Radius1, out StartPhi, out EndPhi, out PointOrder);
                            //}
                            if (Row != null && Row.Length > 0)
                            {
                                // 由于measure_length2的存在，导致在测量圆弧类尺寸，会导致测量值比实际值偏小，测量该类型对象时就使测量区域宽度尽量小，所以这里加了一个补偿值compensationRadius
                                Parameter = new HTuple(Column[0].D, Row[0].D * -1, Radius1[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D, PointOrder.S);
                            }
                            else
                            {
                                Parameter = new HTuple();
                            }
                            break;
                        case "ellipse":
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //{
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), out X, out Y);
                            //    new HXLDCont(Y * -1, X).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                            //}
                            //else
                            //{
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), 1, out X, out Y);
                            new HXLDCont(Y * -1, X).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                            // }
                            if (Row != null && Row.Length > 0)
                            {
                                // 由于measure_length2的存在，导致在测量圆弧类尺寸，会导致测量值比实际值偏小，测量该类型对象时就使测量区域宽度尽量小，所以这里加了一个补偿值compensationRadius
                                Parameter = new HTuple(Column[0].D, Row[0].D * -1, Phi.TupleDeg()[0].D, Radius1[0].D, Radius2[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D, PointOrder.S);
                            }
                            else
                                Parameter = new HTuple();

                            break;
                        case "rect2":
                            HTuple Length1, Length2;
                            //if (calibrateFile != null && GlobalVariable.pConfig.EnableCameraCalibrate)
                            //{
                            //    calibrateFile.GetCalibratePointsFromWorldPlaneToCalibratePlane(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), camParam, camPose, out X, out Y);
                            //    new HXLDCont(Y * -1, X).FitRectangle2ContourXld("regression", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                            //}
                            //else
                            //{
                            HOperatorSet.ImagePointsToWorldPlane(new HTuple(camParam), new HTuple(camPose), new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray()), 1, out X, out Y);
                            new HXLDCont(Y * -1, X).FitRectangle2ContourXld("regression", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                            //}
                            if (Row != null && Row.Length > 0)
                            {
                                Parameter = new HTuple(Column[0].D, Row[0].D * -1, Phi.TupleDeg()[0].D, Length1[0].D, Length2[0].D);
                            }
                            else
                                Parameter = new HTuple();
                            //}
                            break;
                    }
                    break;
            }
        }
        public void GetMeasureObjectParam(HHomMat2D hHomMat2D, enParamType paramType, out HTuple Parameter)
        {
            Parameter = new HTuple();
            if (this.listEdgesRow.Count == 0) return;
            switch (paramType)
            {
                case enParamType.row_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesRow.ToArray());
                            break;
                    }
                    break;
                case enParamType.column_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesCol.ToArray());
                            break;
                    }
                    break;
                case enParamType.row_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesRow2.ToArray());
                            break;
                    }
                    break;
                case enParamType.column_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            Parameter = new HTuple(listEdgesCol2.ToArray());
                            break;
                    }
                    break;
                case enParamType.X_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y;
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(this.listEdgesCol.ToArray()), new HTuple(this.listEdgesRow.ToArray()), out Y);
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(X);
                                }
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesCol.ToArray());
                            }
                            break;
                    }
                    break;
                case enParamType.X_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y;
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(this.listEdgesCol2.ToArray()), new HTuple(this.listEdgesRow2.ToArray()), out Y);
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(X);
                                }
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesCol.ToArray());
                            }
                            break;
                    }
                    break;
                case enParamType.Y_Edges1:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y;
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(this.listEdgesCol.ToArray()), new HTuple(this.listEdgesRow.ToArray()), out Y);
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(Y);
                                }
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesRow.ToArray());
                            }
                            break;
                    }
                    break;
                case enParamType.Y_Edges2:
                    switch (this.type)
                    {
                        case "point":
                        case "line":
                        case "circle":
                        case "ellipse":
                        case "rect2":
                            HTuple X, Y;
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(this.listEdgesCol2.ToArray()), new HTuple(this.listEdgesRow2.ToArray()), out Y);
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(Y);
                                }
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesRow.ToArray());
                            }
                            break;
                    }
                    break;
                case enParamType.fitResult_Edges1:
                    switch (this.type)
                    {
                        case "point":
                            HTuple X, Y;
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(this.listEdgesCol.ToArray()), new HTuple(this.listEdgesRow.ToArray()), out Y);
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(X[0].D, Y[0].D);
                                }
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesRow[0], listEdgesCol[0]);
                            }
                            break;
                        case "line":
                            HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist;
                            new HXLDCont(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray())).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(ColBegin, ColEnd), new HTuple(RowBegin, RowEnd), out Y);
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(X[0].D, Y[0].D, X[1].D, Y[1].D);
                                }
                            }
                            else
                            {
                                Parameter = new HTuple(RowBegin, ColBegin, RowEnd, ColEnd);
                            }
                            break;
                        case "circle":
                            HTuple Row = null, Column = null, Radius1 = null, Radius2 = null, Phi = null, StartPhi = null, EndPhi = null, PointOrder = null;
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(this.listEdgesCol.ToArray()), new HTuple(this.listEdgesRow.ToArray()), out Y);
                                new HXLDCont(Y * -1, X).FitCircleContourXld("algebraic", -1, 0, 0, 3, 1, out Row, out Column, out Radius1, out StartPhi, out EndPhi, out PointOrder);
                                if (Row != null && Row.Length > 0)
                                {
                                    Parameter = new HTuple(Column[0].D, Row[0].D * -1, Radius1[0].D, StartPhi.TupleDeg().D, EndPhi.TupleDeg().D, PointOrder.S);
                                }
                            }
                            else
                            {
                                new HXLDCont(this.listEdgesRow.ToArray(), this.listEdgesCol.ToArray()).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 1, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                                if (Row != null && Row.Length > 0)
                                    Parameter = new HTuple(Row[0].D, Column[0].D, Radius1[0].D, StartPhi[0].D, EndPhi[0].D, PointOrder.S);
                                else
                                    Parameter = new HTuple();
                            }
                            break;
                        case "ellipse":
                            Row = null; Column = null; Radius1 = null; Radius2 = null; Phi = null; StartPhi = null; EndPhi = null; PointOrder = null;
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(this.listEdgesCol.ToArray()), new HTuple(this.listEdgesRow.ToArray()), out Y);
                                new HXLDCont(Y * -1, X).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                                if (Row != null && Row.Length > 0)
                                {
                                    Parameter = new HTuple(Column[0].D, Row[0].D * -1, Phi.TupleDeg()[0].D, Radius1[0].D, Radius2[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D, PointOrder.S);
                                }
                            }
                            else
                            {
                                new HXLDCont(this.listEdgesRow.ToArray(), this.listEdgesCol.ToArray()).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 1, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                                if (Row != null && Row.Length > 0)
                                    Parameter = new HTuple(Row[0].D, Column[0].D, Phi[0].D, Radius1[0].D, Radius2[0].D, StartPhi[0].D, EndPhi[0].D, PointOrder.S);
                                else
                                    Parameter = new HTuple();
                            }
                            break;
                        case "rect2":
                            HTuple Length1, Length2;
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(this.listEdgesCol.ToArray()), new HTuple(this.listEdgesRow.ToArray()), out Y);
                                new HXLDCont(Y * -1, X).FitRectangle2ContourXld("regression", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                                if (Row != null && Row.Length > 0)
                                {
                                    Parameter = new HTuple(Column[0].D, Row[0].D * -1, Phi.TupleDeg()[0].D, Length1[0].D, Length2[0].D);
                                }
                            }
                            else
                            {
                                new HXLDCont(new HTuple(this.listEdgesRow.ToArray()), new HTuple(this.listEdgesCol.ToArray())).FitRectangle2ContourXld("regression", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                                if (Row != null && Row.Length > 0)
                                    Parameter = new HTuple(Row[0].D, Column[0].D, Phi[0].D, Length1[0].D, Length2[0].D);
                                else
                                    Parameter = new HTuple();
                            }
                            break;
                    }
                    break;
                case enParamType.fitResult_Edges2:
                    switch (this.type)
                    {
                        case "point":
                            HTuple X, Y;
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(this.listEdgesCol2.ToArray()), new HTuple(this.listEdgesRow2.ToArray()), out Y);
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(X[0].D, Y[0].D);
                                }
                            }
                            else
                            {
                                Parameter = new HTuple(listEdgesRow2[0], listEdgesCol2[0]);
                            }
                            break;
                        case "line":
                            HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist;
                            new HXLDCont(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray())).FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(ColBegin, ColEnd), new HTuple(RowBegin, RowEnd), out Y);
                                if (X.Length > 0)
                                {
                                    Parameter = new HTuple(X[0].D, Y[0].D, X[1].D, Y[1].D);
                                }
                            }
                            else
                            {
                                Parameter = new HTuple(RowBegin, ColBegin, RowEnd, ColEnd);
                            }
                            break;
                        case "circle":
                            HTuple Row = null, Column = null, Radius1 = null, Radius2 = null, Phi = null, StartPhi = null, EndPhi = null, PointOrder = null;
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(this.listEdgesCol2.ToArray()), new HTuple(this.listEdgesRow2.ToArray()), out Y);
                                new HXLDCont(Y * -1, X).FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out Row, out Column, out Radius1, out StartPhi, out EndPhi, out PointOrder);
                                if (Row != null && Row.Length > 0)
                                {
                                    Parameter = new HTuple(Column[0].D, Row[0].D * -1, Radius1[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D, PointOrder.S);
                                }
                                else
                                    Parameter = new HTuple();
                            }
                            else
                            {
                                new HXLDCont(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray())).FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out Row, out Column, out Radius1, out StartPhi, out EndPhi, out PointOrder);
                                if (Row != null && Row.Length > 0)
                                    Parameter = new HTuple(Row[0].D, Column[0].D, Radius1[0].D, StartPhi[0].D, EndPhi[0].D, PointOrder.S);
                                else
                                    Parameter = new HTuple();
                            }
                            break;
                        case "ellipse":
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(this.listEdgesCol2.ToArray()), new HTuple(this.listEdgesRow2.ToArray()), out Y);
                                new HXLDCont(Y * -1, X).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                                if (Row != null && Row.Length > 0)
                                {
                                    Parameter = new HTuple(Column[0].D, Row[0].D * -1, Phi.TupleDeg()[0].D, Radius1[0].D, Radius2[0].D, StartPhi.TupleDeg()[0].D, EndPhi.TupleDeg()[0].D, PointOrder.S);
                                }
                                else
                                    Parameter = new HTuple();
                            }
                            else
                            {
                                new HXLDCont(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray())).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                                if (Row != null && Row.Length > 0)
                                    Parameter = new HTuple(Row[0].D, Column[0].D, Phi.D, Radius1[0].D, Radius2[0].D, StartPhi.D, EndPhi.D, PointOrder.S);
                                else
                                    Parameter = new HTuple();
                            }
                            break;
                        case "rect2":
                            HTuple Length1, Length2;
                            if (hHomMat2D != null)
                            {
                                X = hHomMat2D.AffineTransPoint2d(new HTuple(this.listEdgesCol2.ToArray()), new HTuple(this.listEdgesRow2.ToArray()), out Y);
                                new HXLDCont(Y * -1, X).FitRectangle2ContourXld("regression", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                                if (Row != null && Row.Length > 0)
                                {
                                    Parameter = new HTuple(Column[0].D, Row[0].D * -1, Phi.TupleDeg()[0].D, Length1[0].D, Length2[0].D);
                                }
                                else
                                    Parameter = new HTuple();
                            }
                            else
                            {
                                new HXLDCont(new HTuple(this.listEdgesRow2.ToArray()), new HTuple(this.listEdgesCol2.ToArray())).FitRectangle2ContourXld("regression", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                                if (Row != null && Row.Length > 0)
                                    Parameter = new HTuple(Row[0].D, Column[0].D, Phi[0].D, Length1[0].D, Length2[0].D);
                                else
                                    Parameter = new HTuple();
                            }
                            break;
                    }
                    break;
            }
        }

        public HXLDCont GetRect2MeasureRegion()
        {
            HXLDCont region = new HXLDCont();
            region.GenEmptyObj();
            if (this.type == "point") return region;
            for (int i = 0; i < this.rect2RowPoints.Length; i++)
            {
                HXLDCont rect2 = new HXLDCont();
                rect2.GenRectangle2ContourXld(this.rect2RowPoints[i], this.rect2ColPoints[i], this.rect2PhiPoints[i], this.measure_length1, this.measure_length2);
                region = region.ConcatObj(rect2);
            }
            return region.UnionAdjacentContoursXld(10000, 10000, "attr_keep");
        }

        public HXLDCont GetArrowMeasureRegion()
        {
            HXLDCont region = new HXLDCont();
            region.GenEmptyObj();
            if (this.type == "point") return region;
            HTuple rows, cols, Qx, Qy;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.HomMat2dIdentity();
            for (int i = 0; i < this.rect2RowPoints.Length; i++)
            {
                HHomMat2D hHomMat2DRotate = hHomMat2D.HomMat2dRotate(this.rect2PhiPoints[i], this.rect2RowPoints[i], this.rect2ColPoints[i]);
                rows = new HTuple(this.rect2RowPoints[i], this.rect2RowPoints[i], this.rect2RowPoints[i], this.rect2RowPoints[i] - this.measure_length2, this.rect2RowPoints[i], this.rect2RowPoints[i] + this.measure_length2, this.rect2RowPoints[i]);
                cols = new HTuple(this.rect2ColPoints[i] - this.measure_length1, this.rect2ColPoints[i], this.rect2ColPoints[i] + this.measure_length1 - this.measure_length2, this.rect2ColPoints[i] + this.measure_length1 - this.measure_length2,
                    this.rect2ColPoints[i] + this.measure_length1, this.rect2ColPoints[i] + this.measure_length1 - this.measure_length2, this.rect2ColPoints[i] + this.measure_length1 - this.measure_length2);
                Qy = hHomMat2DRotate.AffineTransPoint2d(rows, cols, out Qx); // 变换的是行列，那么输出的也应是行列
                region = region.ConcatObj(new HXLDCont(Qy, Qx));
            }
            return region;
        }

        private HXLDCont GetCrossPoint(double[] rows, double[] cols)
        {
            HXLDCont region = new HXLDCont();
            if (rows != null && rows.Length > 0)
                region.GenCrossContourXld(rows, cols, this.measure_length2, 0);
            return region;
        }



    }

    [Serializable]
    public enum enEdgeSelect
    {
        all_所有边缘,
        strongest_最强边缘,
        first_第一个边缘,
        second_第二个边缘,
        third_第三个边缘,
        last_最后一个边缘,
        near_最近边缘,
        fast_最远边缘,
    }
    [Serializable]
    public enum enEdgeTransition
    {
        all_所有极性,
        negative_从白到黑,
        positive_从黑到白,
        all_strongest_最强边缘,
        negative_strongest_从白到黑,
        positive_strongest_从黑到白,
    }
    [Serializable]
    public enum enMeasureDirection
    {
        从左到右,
        从右到左,
        从上到下,
        从下到上,
        从内到外,
        从外到内,
    }
    [Serializable]
    public enum enInterpolation
    {
        nearest_neighbor,
        bicubic,
        bilinear,
    }
    [Serializable]
    public enum enParamType
    {
        row_Edges1,
        column_Edges1,
        row_Edges2,
        column_Edges2,
        // 只提供边缘像素点的获取，不提供世界坐标的获取
        X_Edges1,
        Y_Edges1,
        X_Edges2,
        Y_Edges2,
        fitResult_Edges1,
        fitResult_Edges2,
    }

    public enum enMeasureType
    {
        point,
        line,
        circle,
        ellipse,
        rect2,
        width,
    }
    public enum enDataFillUp
    {
        True,
        False,
        fastFill,
        nearFill,
        centerFill,
    }


}
