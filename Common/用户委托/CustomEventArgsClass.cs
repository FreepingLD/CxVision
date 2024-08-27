using HalconDotNet;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Common
{

    /// <summary>
    /// 为事件提供数据
    /// </summary>
    public class PointCloudAcqCompleteEventArgs
    {
        public HObjectModel3D PointsCloudData
        {
            get;
            set;
        }
        public string SensorName
        {
            get;
            set;
        }
        public double[] X
        {
            get;
            set;
        }
        public double[] Y
        {
            get;
            set;
        }
        public double[] Dist1
        {
            get;
            set;
        }
        public double[] Dist2
        {
            get;
            set;
        }
        public double[] Thick
        {
            get;
            set;
        }

        // 存储点激光事件数据
        public PointCloudAcqCompleteEventArgs(string sensorName, HObjectModel3D pointDataModel)
        {
            this.PointsCloudData = pointDataModel;
            this.SensorName = sensorName;
        }
        public PointCloudAcqCompleteEventArgs(string sensorName, double []  X = null, double[] Y = null, double[] Dist1 = null, double[] Dist2 = null, double[] Thick = null)
        {
            this.X = X;
            this.Y = Y;
            this.Dist1 = Dist1;
            this.Dist2 = Dist2;
            this.Thick = Thick;
            this.SensorName = sensorName;
        }


    }
    public class ImageAcqCompleteEventArgs
    {
        public ImageDataClass ImageData
        {
            get;
            set;
        }
        public string CamName
        {
            get;
            set;
        }
        public int ImageIndex
        {
            get;
            set;
        }
        public ImageAcqCompleteEventArgs( string camName, ImageDataClass imageData)
        {
            this.ImageData = imageData;
            this.CamName = camName;
            this.ImageIndex = 1;
        }
        public ImageAcqCompleteEventArgs(string camName, ImageDataClass imageData, int index)
        {
            this.ImageData = imageData;
            this.CamName = camName;
            this.ImageIndex = index;
        }


    }
    public class AcqNumberEventArgs
    {
        private int count;
        public int Count
        {
            get
            {
                return count;
            }

            set
            {
                count = value;
            }
        }
        public AcqNumberEventArgs(int count)
        {
            this.count = count;
        }
    }
    public class ExcuteCompletedEventArgs
    {
        private object _dataContent = null;
        private string itemName;
        private string sensorName;
        private string _viewWindow;
        public int ImageIndex
        {
            get;
            set;
        }
        public ExcuteCompletedEventArgs(object dataContent)
        {
            this._dataContent = dataContent;
            this.ImageIndex = 1;
        }
        public ExcuteCompletedEventArgs(string itemName, object dataContent)
        {
            this.itemName = itemName;
            this._dataContent = dataContent;
            this.ImageIndex = 1;
        }
        public ExcuteCompletedEventArgs(string camName, string itemName, object dataContent)
        {
            this._dataContent = dataContent;
            this.sensorName = camName;
            this.itemName = itemName;
            this.ImageIndex = 1;
        }
        public ExcuteCompletedEventArgs(string camName, string viewName, string itemName, object dataContent)
        {
            this._dataContent = dataContent;
            this.sensorName = camName;
            this.itemName = itemName;
            this._viewWindow = viewName;
            this.ImageIndex = 1;
        }
        public object DataContent { get => _dataContent; set => _dataContent = value; }
        public string ItemName { get => itemName; set => itemName = value; }
        public string SensorName { get => sensorName; set => sensorName = value; }
        public string ViewWindow { get => _viewWindow; set => _viewWindow = value; }
    }
    public class ItemDeleteEventArgs
    {
        private object _dataContent = null;
        private string itemName;

        public ItemDeleteEventArgs(object dataContent)
        {
            this._dataContent = dataContent;
        }
        public ItemDeleteEventArgs(object dataContent, string itemName)
        {
            this.itemName = itemName;
            this._dataContent = dataContent;
        }
        public object DataContent
        {
            get
            {
                return _dataContent;
            }

            set
            {
                _dataContent = value;
            }
        }
        public string ItemName
        {
            get
            {
                return itemName;
            }

            set
            {
                itemName = value;
            }
        }


    }
    public class GraphicUpdateEventArgs
    {
        private HObject _dataContent = null;
        public HObject DataContent
        {
            get
            {
                return _dataContent;
            }

            set
            {
                _dataContent = value;
            }
        }

        public GraphicUpdateEventArgs(HObject data)
        {
            this._dataContent = data;
        }

    }
    // 运动到位事件数据类
    public class PoseInfoEventArgs
    {
        private double x;
        private double y;
        private double z;
        private double u;
        private double v;
        private double theta;
        private string poseInfo;
        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }
        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }
        public double Z
        {
            get
            {
                return z;
            }

            set
            {
                z = value;
            }
        }
        public double U
        {
            get
            {
                return u;
            }

            set
            {
                u = value;
            }
        }
        public double V
        {
            get
            {
                return v;
            }

            set
            {
                v = value;
            }
        }
        public double Theta
        {
            get
            {
                return theta;
            }

            set
            {
                theta = value;
            }
        }
        public string PoseInfo
        {
            get
            {
                return poseInfo;
            }

            set
            {
                poseInfo = value;
            }
        }

        public string DeviceName { get; set; }

        public PoseInfoEventArgs(string PoseInfo)
        {
            this.poseInfo = PoseInfo;
        }
        public PoseInfoEventArgs(string deviceName, string PoseInfo)
        {
            this.DeviceName = deviceName;
            this.poseInfo = PoseInfo;
        }
        public PoseInfoEventArgs(double x, double y, double z, double u, double v, double theta)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.u = u;
            this.v = v;
            this.theta = theta;
        }
        public PoseInfoEventArgs(string poseInfo, double x, double y, double z, double u, double v, double theta)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.u = u;
            this.v = v;
            this.theta = theta;
            this.poseInfo = poseInfo;
        }
    }
    public class MeasureChangeEventArgs
    {
        private double[] rowPoint;
        private double[] colPoint;
        private double[] phiPoint;

        public double[] RowPoint
        {
            get
            {
                return rowPoint;
            }

            set
            {
                rowPoint = value;
            }
        }
        public double[] ColPoint
        {
            get
            {
                return colPoint;
            }

            set
            {
                colPoint = value;
            }
        }
        public double[] PhiPoint
        {
            get
            {
                return phiPoint;
            }

            set
            {
                phiPoint = value;
            }
        }


        public MeasureChangeEventArgs(double[] rowPoint, double[] colPoint, double[] phiPoint)
        {
            this.rowPoint = rowPoint;
            this.colPoint = colPoint;
            this.phiPoint = phiPoint;
        }
    }
    public class MetrolegyCompletedEventArgs
    {
        private userWcsPoint[] edgePoint_xyz;
        private double[] edgePoint_x;
        private double[] edgePoint_y;
        private object edgeData; // 这里传入图像、XLD、结构体都可以
        public double[] EdgePoint_x
        {
            get
            {
                return edgePoint_x;
            }

            set
            {
                edgePoint_x = value;
            }
        }
        public double[] EdgePoint_y
        {
            get
            {
                return edgePoint_y;
            }

            set
            {
                edgePoint_y = value;
            }
        }
        public userWcsPoint[] EdgePoint_xyz
        {
            get
            {
                return edgePoint_xyz;
            }

            set
            {
                edgePoint_xyz = value;
            }
        }
        public object EdgeData
        {
            get
            {
                return edgeData;
            }

            set
            {
                edgeData = value;
            }
        }

        public MetrolegyCompletedEventArgs(double[] Point_x, double[] Point_y, object fitObject)
        {
            this.edgePoint_x = Point_x;
            this.edgePoint_y = Point_y;
            this.edgeData = fitObject;
        }
        public MetrolegyCompletedEventArgs(userWcsPoint[] Point_xyz,  object fitObject)
        {
            this.edgePoint_xyz = Point_xyz;
            this.edgeData = fitObject;
        }
    }
    public class DisplayMetrolegyObjectEventArgs
    {
        private object _dataContent = null;
        private string nodeName;
        public object DataContent
        {
            get
            {
                return _dataContent;
            }

            set
            {
                _dataContent = value;
            }
        }

        public string NodeName { get => nodeName; set => nodeName = value; }

        public DisplayMetrolegyObjectEventArgs(object data)
        {
            this._dataContent = data;
        }
        public DisplayMetrolegyObjectEventArgs(object data, string nodename)
        {
            this._dataContent = data;
            this.nodeName = nodename;
        }

    }

    public class ItemsChangeEventArgs
    {
        private object _function; // 条目所添加的对象上
        private string _itemName; // 添加的条目对象
        private string _itemSource; // 添加的条目内容指的是：RefSource1，RefSource2，RefSource3，RefSource4
        public string ItemName { get => _itemName; set => _itemName = value; }
        public object Function { get => _function; set => _function = value; }
        public string ItemSource { get => _itemSource; set => _itemSource = value; }

        //public ItemsChangeEventArgs(object function, string itemName)
        //{
        //    this._itemName = itemName;
        //    this._function = function;
        //}
        public ItemsChangeEventArgs(object function, string itemSource)
        {
            this._itemSource = itemSource;
            this._function = function;
        }
        public ItemsChangeEventArgs(object function, string itemSource, string itemName)
        {
            this._itemName = itemName;
            this._function = function;
            this._itemSource = itemSource;
        }

    }

    public class NodeClickEventArgs
    {
        private TreeNode clickNode;

        public TreeNode ClickNode { get => clickNode; set => clickNode = value; }


        public NodeClickEventArgs(TreeNode clickNode)
        {
            this.clickNode = clickNode;
        }
    }

    public class DataSendEventArgs
    {
        private object _dataContent;
        private object _limitUpContent;
        private object _limitDownContent;
        private object _stdValueContent;


        public object DataContent { get => _dataContent; set => _dataContent = value; }
        public object LimitUpContent { get => _limitUpContent; set => _limitUpContent = value; }
        public object LimitDownContent { get => _limitDownContent; set => _limitDownContent = value; }
        public object StdValueContent { get => _stdValueContent; set => _stdValueContent = value; }

        public DataSendEventArgs(List<object> data)
        {
            this._dataContent = data;
        }
        public DataSendEventArgs(object data)
        {
            this._dataContent = data;
        }

        public DataSendEventArgs(List<string> data, List<string> limitUpData, List<string> limitDownData, List<string> stdData)
        {
            this._dataContent = data;
            this._limitUpContent = limitUpData;
            this._limitDownContent = limitDownData;
            this._stdValueContent = stdData;
        }


    }

    public class GrayValueInfoEventArgs
    {
        private double _Col;
        private double _Row;
        private object[] _grayValue;

        public double Col
        {
            get
            {
                return _Col;
            }

            set
            {
                _Col = value;
            }
        }
        public double Row
        {
            get
            {
                return _Row;
            }

            set
            {
                _Row = value;
            }
        }
        public object[] GaryValue { get => _grayValue; set => _grayValue = value; }

        public GrayValueInfoEventArgs(double row, double col, HTuple garyValue)
        {
            this._Col = col;
            this._Row = row;
            this._grayValue = new object[garyValue.Length]; ;
            switch (garyValue.Type)
            {
                case HTupleType.DOUBLE:
                    for (int i = 0; i < garyValue.Length; i++)
                        this._grayValue[i] = garyValue[i].D;
                    break;
                case HTupleType.INTEGER:
                    for (int i = 0; i < garyValue.Length; i++)
                        this._grayValue[i] = garyValue[i].I;
                    break;
                case HTupleType.LONG:
                    for (int i = 0; i < garyValue.Length; i++)
                        this._grayValue[i] = garyValue[i].L;
                    break;
                case HTupleType.MIXED:
                    for (int i = 0; i < garyValue.Length; i++)
                        this._grayValue[i] = garyValue[i].O;
                    break;
                case HTupleType.STRING:
                    for (int i = 0; i < garyValue.Length; i++)
                        this._grayValue[i] = garyValue[i].S;
                    break;
                case HTupleType.EMPTY:
                default:
                    for (int i = 0; i < garyValue.Length; i++)
                        this._grayValue[i] = "-";
                    break;
            }
        }


    }

    public class ButonClickEventArgs
    {
        public string ClickInfo { get; set; }
        public int BtnIndex { get; set; }
        public ButonClickEventArgs(string info,int index)
        {
            this.ClickInfo = info;
            this.BtnIndex = index;
        }
    }




}
