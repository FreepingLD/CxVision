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
using Common;

namespace FunctionBlock
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    public class ShowMultipleObjects : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D[] dataHandle3D = null;
        private HObjectModel3D[] extractRefSource1Data()
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
            return listObjectModel3D.ToArray();
        }




        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            HalconLibrary ha = new HalconLibrary();
            try
            {
                this.dataHandle3D = extractRefSource1Data();
                if (this.dataHandle3D == null)
                    return Result;
                else
                    Result.Succss = true;
                OnExcuteCompleted(this.name, this.dataHandle3D);
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
                case "3D对象":
                case "源3D对象":
                case "输入3D对象":
                    return this.dataHandle3D; //
                default:
                    if (this.name == propertyName)
                        return this.dataHandle3D;
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
                //////////////////////            
                default:
                    return true;
            }
        }
        public void ReleaseHandle()
        {
            try
            {
                if (this.dataHandle3D != null)
                {
                    for (int i = 0; i < this.dataHandle3D.Length; i++)
                    {
                        this.dataHandle3D[i].Dispose();
                    }
                }
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
            finally
            {
                OnItemDeleteEvent(this, this.name);
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
        }
    }
}
