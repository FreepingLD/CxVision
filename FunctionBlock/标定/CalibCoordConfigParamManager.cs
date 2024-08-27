using Common;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class CalibCoordConfigParamManager
    {
       // private  string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内

        public  CalibCoordConfigParamManager ()
        {

        }
        public BindingList<WriteCommunicateCommand > WriteAdress { get; set; }

        private BindingList<CoordSysAxisParam> _CalibCoordParamList = new BindingList<CoordSysAxisParam>();
        public BindingList<CoordSysAxisParam> CalibCoordParamList { get => _CalibCoordParamList; set => _CalibCoordParamList = value; }


        public CoordSysAxisParam GetParam(double X,double Y,double Z,double theta)
        {
            foreach (var item in _CalibCoordParamList)
            {
                if (item.X == X && item.Y == Y && item.Z == Z && item.Theta == theta) return item; 
            }
            return null;
        }


        public  bool Save(string path)
        {
            bool IsOk = true;
            string dic = new FileInfo(path).DirectoryName;
            if (!DirectoryEx.Exist(new FileInfo(path).DirectoryName)) DirectoryEx.Create(new FileInfo(path).DirectoryName);
            IsOk = IsOk && XML<BindingList<CoordSysAxisParam>>.Save(_CalibCoordParamList, path); // 以类名作为文件名
            return IsOk;
        }
        public  void Read(string path)
        {
            if (File.Exists(path))
                this._CalibCoordParamList = XML<BindingList<CoordSysAxisParam>>.Read(path);
            else
                this._CalibCoordParamList = new BindingList<CoordSysAxisParam>();
        }


    }
}
