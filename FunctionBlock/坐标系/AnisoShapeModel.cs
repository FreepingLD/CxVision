using AlgorithmsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Common;
using HalconDotNet;
using System.Windows.Forms;
using System.IO;
using Sensor;

namespace FunctionBlock
{
    public class AnisoShapeModel : IFunction
    {
        private string name = "";
        private Dictionary<string, IFunction> refSource1 = new Dictionary<string, IFunction>();
        private Dictionary<string, IFunction> refSource2 = new Dictionary<string, IFunction>();
        private userPixVector ref_PixPoint;
        //private userWcsVector ref_WcsPoint;
        //private userPixVector current_PixPoint;
        //private userWcsVector current_WcsPoint;
        private bool initCoordPoint = false;
        // private HXLDCont coordSystem = null;
        private HObject coordSystem = null;
        private userPixVector CoordPoint;

        private userPixRectangle2 rect2 = new userPixRectangle2();
        HXLD shapeModelContours = null;
        HImage templateImage = null;
        // 创建形状模型用参数 "auto", -1, 2, "auto", 0.9, 1.1, "auto", 0.9, 1.1, "auto", "auto", "use_polarity", "auto", "auto"
        // 创建模型参数
        private object _createNumLevels = "auto";
        private double _createAngleStart = 0;
        private double _createAngleExtent = 6.28;
        private object _createAngleStep = "auto";
        private double _createScaleRMin = 0.9;
        private double _createScaleRMax = 1.1;
        private object _createScaleRStep = "auto";
        private double _createScaleCMin = 0.9;
        private double _createScaleCMax = 1.1;
        private object _createScaleCStep = "auto";
        private string _createOptimization = "auto";
        private string _createMetric = "use_polarity";
        private object _createContrast = "auto";
        private object _createMinContrast = "auto";
        //
        // 模型匹配参数
        private double _shapeAngleStart = -0.4;
        private double _shapeAngleExtent = 0.8;
        private double _shapeScaleRMin = 0.9;
        private double _shapeScaleRMax = 1.1;
        private double _shapeScaleCMin = 0.9;
        private double _shapeScaleCMax = 1.1;
        private double _shapeMinScore = 0.5;
        private int _shapeNumMatches = 1;
        private double _shapeMaxOverlap = 0.5;
        private string _shapeSubPixel = "least_squares";
        private int _shapeNumLevels = 0;
        private double _shapeGreediness = 0.9;


        public object CreateNumLevels
        {
            get
            {
                return _createNumLevels;
            }

            set
            {
                _createNumLevels = value;
            }
        }
        public double CreateAngleStart
        {
            get
            {
                return _createAngleStart;
            }

            set
            {
                _createAngleStart = value;
            }
        }
        public double CreateAngleExtent
        {
            get
            {
                return _createAngleExtent;
            }

            set
            {
                _createAngleExtent = value;
            }
        }
        public object CreateAngleStep
        {
            get
            {
                return _createAngleStep;
            }

            set
            {
                _createAngleStep = value;
            }
        }
        public double CreateScaleRMin
        {
            get
            {
                return _createScaleRMin;
            }

            set
            {
                _createScaleRMin = value;
            }
        }
        public double CreateScaleRMax
        {
            get
            {
                return _createScaleRMax;
            }

            set
            {
                _createScaleRMax = value;
            }
        }
        public object CreateScaleRStep
        {
            get
            {
                return _createScaleRStep;
            }

            set
            {
                _createScaleRStep = value;
            }
        }
        public double CreateScaleCMin
        {
            get
            {
                return _createScaleCMin;
            }

            set
            {
                _createScaleCMin = value;
            }
        }
        public double CreateScaleCMax
        {
            get
            {
                return _createScaleCMax;
            }

            set
            {
                _createScaleCMax = value;
            }
        }
        public object CreateScaleCStep
        {
            get
            {
                return _createScaleCStep;
            }

            set
            {
                _createScaleCStep = value;
            }
        }
        public string CreateOptimization
        {
            get
            {
                return _createOptimization;
            }

            set
            {
                _createOptimization = value;
            }
        }
        public string CreateMetric
        {
            get
            {
                return _createMetric;
            }

            set
            {
                _createMetric = value;
            }
        }
        public object CreateContrast
        {
            get
            {
                return _createContrast;
            }

            set
            {
                _createContrast = value;
            }
        }
        public object CreateMinContrast
        {
            get
            {
                return _createMinContrast;
            }

            set
            {
                _createMinContrast = value;
            }
        }
        public double ShapeAngleStart
        {
            get
            {
                return _shapeAngleStart;
            }

            set
            {
                _shapeAngleStart = value;
            }
        }
        public double ShapeAngleExtent
        {
            get
            {
                return _shapeAngleExtent;
            }

            set
            {
                _shapeAngleExtent = value;
            }
        }
        public double ShapeScaleRMin
        {
            get
            {
                return _shapeScaleRMin;
            }

            set
            {
                _shapeScaleRMin = value;
            }
        }
        public double ShapeScaleRMax
        {
            get
            {
                return _shapeScaleRMax;
            }

            set
            {
                _shapeScaleRMax = value;
            }
        }
        public double ShapeScaleCMin
        {
            get
            {
                return _shapeScaleCMin;
            }

            set
            {
                _shapeScaleCMin = value;
            }
        }
        public double ShapeScaleCMax
        {
            get
            {
                return _shapeScaleCMax;
            }

            set
            {
                _shapeScaleCMax = value;
            }
        }
        public double ShapeMinScore
        {
            get
            {
                return _shapeMinScore;
            }

            set
            {
                _shapeMinScore = value;
            }
        }
        public int ShapeNumMatches
        {
            get
            {
                return _shapeNumMatches;
            }

            set
            {
                _shapeNumMatches = value;
            }
        }
        public double ShapeMaxOverlap
        {
            get
            {
                return _shapeMaxOverlap;
            }

            set
            {
                _shapeMaxOverlap = value;
            }
        }
        public string ShapeSubPixel
        {
            get
            {
                return _shapeSubPixel;
            }

            set
            {
                _shapeSubPixel = value;
            }
        }
        public int ShapeNumLevels
        {
            get
            {
                return _shapeNumLevels;
            }

            set
            {
                _shapeNumLevels = value;
            }
        }
        public double ShapeGreediness
        {
            get
            {
                return _shapeGreediness;
            }

            set
            {
                _shapeGreediness = value;
            }
        }

        //


        public event ExcuteCompletedEventHandler ExcuteCompleted;
        private void OnExcuteCompleted(object propertyName)
        {
            if (this.ExcuteCompleted != null)
            {
                this.ExcuteCompleted(this, new ExcuteCompletedEventArgs(propertyName));
            }
        }
        private void extractRefSource1Data(out HObject image)
        {
            HalconLibrary ha = new HalconLibrary();
            object object3D = null;
            image = null;
            string[] refSource1Keys = new string[this.refSource1.Count];
            if (this.refSource1.Count == 0) return;
            this.refSource1.Keys.CopyTo(refSource1Keys, 0);
            // 获取所有3D对象模型
            for (int i = 0; i < refSource1Keys.Length; i++)
            {
                if (refSource1Keys[i].Split('.').Length == 1)
                    object3D = this.refSource1[refSource1Keys[i]].GetPropertyValues(refSource1Keys[i]);
                else
                    object3D = this.refSource1[refSource1Keys[i]].GetPropertyValues(refSource1Keys[i].Split('.')[1]);
                if (object3D != null && object3D is HObject) // 这样做是为了动态获取名称
                    image = (HObject)object3D;
            }
        }
        private void extractRefSource2Data(out HObject image)
        {
            HalconLibrary ha = new HalconLibrary();
            image = null;
            object object3D = null;
            string[] refSource2Keys = new string[this.refSource2.Count];
            if (this.refSource2.Count == 0) return;
            this.refSource2.Keys.CopyTo(refSource2Keys, 0);
            // 获取所有3D对象模型
            for (int i = 0; i < refSource2Keys.Length; i++)
            {
                if (refSource2Keys[i].Split('.').Length == 1)
                    object3D = this.refSource2[refSource2Keys[i]].GetPropertyValues(refSource2Keys[i]);
                else
                    object3D = this.refSource2[refSource2Keys[i]].GetPropertyValues(refSource2Keys[i].Split('.')[1]); 
                if (object3D != null && object3D is HObject) // 这样做是为了动态获取名称
                    image = (HObject)object3D;
            }
        }
        private void CreateShapeModel(HObject image, HObject templatRegion, out HXLD ShapeModelContours)
        {
            string fileHandle = Application.StartupPath + "\\" + "TemplateFile" + "\\" + this.name + ".shm";
            HTuple shapeModelID, phi, area, row, col;
            HObject imageReduce = null;
            HObject shapeModelContours = null;
            ShapeModelContours = null;
            /////////////////////////////
            HOperatorSet.ReduceDomain(image, templatRegion, out imageReduce); // 加这句指令，templatRegion这参数可为区域也可为图像 
            HOperatorSet.CreateAnisoShapeModel(imageReduce, new HTuple(this.CreateNumLevels), this.CreateAngleStart, this.CreateAngleExtent, new HTuple(this.CreateAngleStep)
                , this.CreateScaleRMin, this.CreateScaleRMax, new HTuple(this.CreateScaleRStep), this.CreateScaleCMin, this.CreateScaleCMax, new HTuple(this.CreateScaleCStep),
                this.CreateOptimization, this.CreateMetric, new HTuple(this.CreateContrast), new HTuple(this.CreateMinContrast), out shapeModelID);
            HOperatorSet.GetShapeModelContours(out shapeModelContours, shapeModelID, 1);
            HOperatorSet.WriteShapeModel(shapeModelID, fileHandle);
            // 创建参考点
            this.initCoordPoint = false;
            if (!initCoordPoint)
            {
                HObject regionUnion;
                HOperatorSet.Union1(templatRegion, out regionUnion);
                HOperatorSet.OrientationRegion(regionUnion, out phi);
                HOperatorSet.AreaCenter(templatRegion, out area, out row, out col);
                //HOperatorSet.ImagePointsToWorldPlane(new HTuple(GlobalVariable.cameraParam), new HTuple(GlobalVariable.worldPose), row, col, "m", out X, out Y);
                //HOperatorSet.TupleDeg(phi, out deg);
                this.ref_PixPoint = new userPixVector(row.D, col.D, phi.D);
                // this.ref_WcsPoint = new userWcsVector(X.D, Y.D, 0.0, deg.D);
                this.initCoordPoint = true; // 参考点只能初始化一次
            }
            if (shapeModelContours != null)
                ShapeModelContours = new HXLD(shapeModelContours);
            if (imageReduce != null)
                templateImage = new HImage(imageReduce);
        }
        private void FindShape(HObject image, HTuple modelID, out userPixVector vectorPixPoint, out userWcsVector vectorWcsPoint)
        {
            // 输出匹配坐标
            HTuple _shapeRow;
            HTuple _shapeColumn;
            HTuple _shapeAngle;
            HTuple _shapeScaleR;
            HTuple _shapeScaleC;
            HTuple _shapeScore;
            // HTuple modelID;
            HTuple X, Y, deg;
            // HalconLibrary ha = new HalconLibrary();
            // string fileHandle = Application.StartupPath + "\\" + "TemplateFile" + "\\" + this.name + ".shm";
            //if (!File.Exists(fileHandle))
            //{
            //    MessageBox.Show("模型文件不存在");
            //    return;
            //}
            // HOperatorSet.ReadShapeModel(fileHandle, out modelID);
            HOperatorSet.FindAnisoShapeModel(image, modelID, this.ShapeAngleStart, this.ShapeAngleExtent, this.ShapeScaleRMin, this.ShapeScaleRMax
                , this.ShapeScaleCMin, this.ShapeScaleCMax, this.ShapeMinScore, this.ShapeNumMatches, this.ShapeMaxOverlap, this.ShapeSubPixel, this.ShapeNumLevels,
                this.ShapeGreediness, out _shapeRow, out _shapeColumn, out _shapeAngle, out _shapeScaleR, out _shapeScaleC, out _shapeScore);
            if (_shapeRow != null && _shapeRow.Length > 0)
            {
                HOperatorSet.ImagePointsToWorldPlane(new HTuple(GlobalVariable.cameraParam), new HTuple(GlobalVariable.worldPose), _shapeRow, _shapeColumn, "m", out X, out Y);
                HOperatorSet.TupleDeg(_shapeAngle, out deg);
                vectorPixPoint = new userPixVector(_shapeRow.D, _shapeColumn.D, _shapeAngle.D);
                vectorWcsPoint = new userWcsVector(X.D, Y.D, 0.0, deg.D);
            }
            else
            {
                vectorPixPoint = new userPixVector();
                vectorWcsPoint = new userWcsVector();
            }
        }
        private void FindShape(HObject image, HTuple modelID, out userPixVector vectorPixPoint)
        {
            // 输出匹配坐标
            HTuple _shapeRow;
            HTuple _shapeColumn;
            HTuple _shapeAngle;
            HTuple _shapeScaleR;
            HTuple _shapeScaleC;
            HTuple _shapeScore;
            HOperatorSet.FindAnisoShapeModel(image, modelID, this.ShapeAngleStart, this.ShapeAngleExtent, this.ShapeScaleRMin, this.ShapeScaleRMax
                , this.ShapeScaleCMin, this.ShapeScaleCMax, this.ShapeMinScore, this.ShapeNumMatches, this.ShapeMaxOverlap, this.ShapeSubPixel, this.ShapeNumLevels,
                this.ShapeGreediness, out _shapeRow, out _shapeColumn, out _shapeAngle, out _shapeScaleR, out _shapeScaleC, out _shapeScore);
            if (_shapeRow != null && _shapeRow.Length > 0)
            {
                vectorPixPoint = new userPixVector(_shapeRow.D, _shapeColumn.D, _shapeAngle.D);
            }
            else
            {
                vectorPixPoint = new userPixVector();
            }
        }


        #region 实现接口
        public void Execute(object param)
        {
            HTuple modelID = null;
            HObject image, templatRegion;
            HalconLibrary ha = new HalconLibrary();
            userPixVector vectorPixPoint;
            string fileHandle = Application.StartupPath + "\\" + "TemplateFile" + "\\" + this.name + ".shm";
            try
            {
                extractRefSource1Data(out image);
                extractRefSource2Data(out templatRegion);
                if (param != null && param.ToString() == "创建模型")
                {
                    CreateShapeModel(image, templatRegion, out this.shapeModelContours);
                    OnExcuteCompleted(this.shapeModelContours);
                }
                else
                {
                    if (!File.Exists(fileHandle))
                    {
                        MessageBox.Show("模型文件不存在");
                        return;
                    }
                    HOperatorSet.ReadShapeModel(fileHandle, out modelID);
                    FindShape(image, modelID, out vectorPixPoint);
                    // ha.GenCoordSystem(vectorPixPoint, 50, 15, 15, out this.coordSystem);
                    OnExcuteCompleted(vectorPixPoint);
                }
            }
            catch (HalconException ee)
            {
                MessageBox.Show(ee.ToString());
            }
            finally
            {
                ha.ClearShapeModel(modelID);
            }
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                    return this.name;
                case "数据源":
                case "数据源1":
                    return this.refSource1;
                case "数据源2":
                    return this.refSource2;
                case "事件状态":
                    return ExcuteCompleted != null ? true : false; //
                case "坐标系":
                    return this.coordSystem; //
                case "模型轮廓":
                    return this.shapeModelContours; //
                case "模型图像":
                    return this.templateImage; //
                default:
                    break;
            }
            return null;
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                case "数据源":
                case "数据源1": // 每一个类中必需实现这个标签
                    if (value.Length > 1) //两个参数表示是添加数据源，一个表示删除数据源
                    {
                        if (value[1] is IFunction && !refSource1.ContainsKey(value[0].ToString())) // 只接受这两个类作为数据源
                            refSource1.Add(value[0].ToString(), (IFunction)value[1]);
                        else
                            return false;
                    }
                    else
                    {
                        if (value[0] is String)
                            refSource1.Remove(value[0].ToString());
                        else
                            return false;
                    }
                    return true;
                //////////////////////////////////////////////
                case "数据源2": // 每一个类中必需实现这个标签;这是添加坐标点的
                    if (value.Length > 1) //两个参数表示是添加数据源，一个表示删除数据源
                    {
                        if (value[1] is IFunction && !refSource2.ContainsKey(value[0].ToString())) // 只接受这两个类作为数据源
                        {
                            refSource2.Add(value[0].ToString(), (IFunction)value[1]);
                        }
                        else
                            return false;
                    }
                    else
                    {
                        if (value[0] is String)
                        {
                            refSource2.Remove(value[0].ToString());
                        }
                        else
                            return false;
                    }
                    return true;
                default:
                    return true;
            }
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        #endregion
    }
}
