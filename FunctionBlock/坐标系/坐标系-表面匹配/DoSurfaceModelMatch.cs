using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using HalconDotNet;

namespace FunctionBlock
{
    public class DoSurfaceModelMatch : BaseFunction, IFunction
    {
        private SurfaceMatch _modelMatch;
        public SurfaceMatch ModelMatch { get => _modelMatch; set => _modelMatch = value; }
        public DoSurfaceModelMatch()
        {
            //FunctionManage.CoordSystemList.Add(this);
            this._modelMatch = new SurfaceMatch();
            this.resultDataTable.Columns.Clear();
            resultDataTable.Columns.AddRange(new DataColumn[7] { new DataColumn("元素名称"), new DataColumn("属性名称"), new DataColumn("行"),
                new DataColumn("列"), new DataColumn("角度"), new DataColumn("得分"),new DataColumn("匹配时间")});
        }
        public HObjectModel3D [] extractRefSource1Data()
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
                        case "userWcsPose3D": // 如果是位姿，将其转换成3D对象
                            HObjectModel3D plane3D = new HObjectModel3D();
                            plane3D.GenPlaneObjectModel3d(((userWcsPose)object3D).GetHPose(), new HTuple(), new HTuple());
                            listObjectModel3D.Add(plane3D);
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();// HObjectModel3D.UnionObjectModel3d(listObjectModel3D.ToArray(), "points_surface");
        }



        public OperateResult Execute(params object[] param)
        {
           this.Result.Succss = false;
            try
            {
                Result.Succss = this._modelMatch.FindSurfaceModel(extractRefSource1Data(), _modelMatch.F_SurfaceModelParam);
                OnExcuteCompleted(this.name, this._modelMatch.MatchingResult.WcsPose3D);
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
                case "2D坐标系":
                case "3D坐标系":
                case "坐标系":
                        return this._modelMatch.MatchingResult.WcsPose3D; //

                case "输入对象":
                    return this.extractRefSource1Data();
                default:
                    return this._modelMatch.MatchingResult.WcsPose3D; //
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            throw new NotImplementedException();
        }
        public void ReleaseHandle()
        {
           this._modelMatch.ClearHandle();
        }

        public void Read(string path)
        {
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
           // throw new NotImplementedException();
        }


    }
}
