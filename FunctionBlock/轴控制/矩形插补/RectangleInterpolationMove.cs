using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using Common;
using System.IO;
using Sensor;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class RectangleInterpolationMove : BaseFunction, IFunction
    {
        // 从机台读取的坐标值
        private DataTable coord1Table = new DataTable();
        private IMotionControl _card;
        private enCoordType coordType = enCoordType.机台坐标;
        private double startVel=0;//起始速度
        private double maxVel=50;//运行速度
        private double tacc=0.5;//加速时间
        private double tdec=0.5;//减速时间
        private double stopVel;//停止速度
        private double s_para=0.1;//S段时间
        private enAxisName moveAxis = enAxisName.XY轴矩形插补;
        private ushort ioOutPort = 0;
        private bool asynchronous = false;
        private double vector_x = 0.5;
        private double vector_y = 0.5; 
        private int lineCount = 1;
        private ushort interpolateMode = 0;

        private MoveCommandParam CommandParam = new MoveCommandParam(enAxisName.XY轴矩形插补, 10);
        public DataTable Coord1Table
        {
            get
            {
                return coord1Table;
            }

            set
            {
                coord1Table = value;
            }
        }

        public enAxisName MoveAxis { get => moveAxis; set => moveAxis = value; }
        public IMotionControl Card { get => _card; set => _card = value; }
        public enCoordType CoordType { get => coordType; set => coordType = value; }
        public double StartVel { get => startVel; set => startVel = value; }
        public double MaxVel { get => maxVel; set => maxVel = value; }
        public double Tacc { get => tacc; set => tacc = value; }
        public double Tdec { get => tdec; set => tdec = value; }
        public double StopVel { get => stopVel; set => stopVel = value; }
        public double S_para { get => s_para; set => s_para = value; }
        public ushort IoOutPort { get => ioOutPort; set => ioOutPort = value; }
        public bool Asynchronous { get => asynchronous; set => asynchronous = value; }
        public double Vector_x { get => vector_x; set => vector_x = value; }
        public double Vector_y { get => vector_y; set => vector_y = value; }
        public int LineCount { get => lineCount; set => lineCount = value; }
        public ushort InterpolateMode { get => interpolateMode; set => interpolateMode = value; }

        public RectangleInterpolationMove(IMotionControl _card)
        {
            if (_card != null)
                this._card = MotionCardManage.GetCard(_card.Name);//  new AcqSource(_camSensor);
            this.Coord1Table.Columns.AddRange(new DataColumn[] { new DataColumn("X坐标"), new DataColumn("Y坐标") });
        }
        public RectangleInterpolationMove(IMotionControl _card, IFunction coordSystem)
        {
            if (_card != null)
                this._card = MotionCardManage.GetCard(_card.Name);//  new AcqSource(_camSensor);
            if (coordSystem != null)
                this.RefSource2.Add(coordSystem.Name, coordSystem);
            this.Coord1Table.Columns.AddRange(new DataColumn[] { new DataColumn("X坐标"), new DataColumn("Y坐标") });
        }

        public userWcsCoordSystem extractRefSource2Data()
        {
            userWcsCoordSystem coordSystem = new userWcsCoordSystem();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource2.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource2[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource2[item].GetPropertyValues(item.Split('.')[1]);
                if (object3D != null) // 这样做是为了动态获取名称
                {
                    switch (object3D.GetType().Name)
                    {
                        case "userWcsCoordSystem":
                            coordSystem = (((userWcsCoordSystem)object3D));
                            break;
                        case "userWcsCoordSystem[]":
                            coordSystem = (((userWcsCoordSystem[])object3D))[0];
                            break;
                        case "userWcsPose3D":
                            coordSystem = (new userWcsCoordSystem(((userWcsPose)object3D)));
                            break;
                    }
                }
            }
            return coordSystem;
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            DataRow[] dataCoord1Row = this.coord1Table.Select();
            try
            {
                for (int i = 0; i < dataCoord1Row.Length; i++) //
                {
                    this.CommandParam.MoveAxis = this.moveAxis;
                    this.CommandParam.MoveSpeed = this.maxVel;
                    this.CommandParam.StartVel = this.startVel;
                    this.CommandParam.StopVel = this.stopVel;
                    this.CommandParam.Tacc = this.tacc;
                    this.CommandParam.Tdec = this.tdec;
                    this.CommandParam.S_para = this.s_para;
                    this.CommandParam.Rect2Param.vector_x = this.vector_x;
                    this.CommandParam.Rect2Param.vector_y = this.vector_y;
                    this.CommandParam.Rect2Param.lineCount = this.lineCount;
                    this.CommandParam.Rect2Param.InterpolateMode = this.interpolateMode;
                    switch (this.moveAxis)
                    {
                        case enAxisName.XY轴矩形插补:
                            CommandParam.AxisParam = new CoordSysAxisParam(  Convert.ToDouble(dataCoord1Row[i]["X坐标"]),Convert.ToDouble(dataCoord1Row[i]["Y坐标"]) ,0,0,0,0);
                            this._card.MoveMultyAxis(CommandParam);
                            break;
                        default:
                            throw new Exception("点位运动不支持该模式");
                    }  
                }
                // 使用发命令的方式来更新视图  
                Result.Succss = true;
                if (Result.Succss)
                    LoggerHelper.Info(this.name + "运动完成");
                else
                    LoggerHelper.Error(this.name + "运动失败");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "运动报错" + e);
                return Result;
            }
            return Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "坐标系":
                    return this.extractRefSource2Data();
                case "名称":
                default:
                    return this.name;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            string name = "";
            if (value != null)
                name = value[0].ToString();
            switch (propertyName)
            {
                default:
                case "名称":
                    this.name = value[0].ToString();
                    return true;         
            }
        }

        public void ReleaseHandle()
        {

        }
        public void Read(string path)
        {
           // throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }

        #endregion



    }

}
