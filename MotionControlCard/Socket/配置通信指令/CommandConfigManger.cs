using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionControlCard
{
    public class CommandConfigManger
    {
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static CommandConfigManger _Instance;
        public static CommandConfigManger Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new CommandConfigManger();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<CommandParam> _commandParam = new BindingList<CommandParam>();
        public BindingList<CommandParam> CommandParam { get => _commandParam; set => _commandParam = value; }


        public CommandParam GetCommandParam(string name)
        {
            CommandParam param = null;
            if (this._commandParam != null)
            {
                foreach (var item in this._commandParam)
                {
                    if (name == item.Describe) return item;
                }
            }
            return param;
        }

        public bool IsContainCommandParam(string name)
        {
            if (this._commandParam != null)
            {
                foreach (var item in this._commandParam)
                {
                    if (name == item.Describe) return true;
                }
            }
            return false;
        }
        public void AddCommandParam(string name)
        {
            if (!this.IsContainCommandParam(name))
            {
                if (this._commandParam == null) this._commandParam = new BindingList<CommandParam>();
                this._commandParam.Add(new CommandParam(name));
            }
        }
        public void DeleteCommandParam(string name)
        {
            if (this.IsContainCommandParam(name))
            {
                if (this._commandParam != null)
                {
                    int index = 0;
                    foreach (var item in this._commandParam)
                    {
                        if (name == item.Describe) break;
                        index++;
                    }
                    if (index < this._commandParam.Count)
                        this._commandParam.RemoveAt(index);
                }
            }
        }

        public void ClearCommandParam()
        {
            if (this._commandParam != null)
            {
                this._commandParam.Clear();
            }
        }



        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<CommandParam>>.Save(_commandParam, ParaPath + "\\" + "CommandParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "CommandParam.xml"))
                this._commandParam = XML<BindingList<CommandParam>>.Read(ParaPath + "\\" + "CommandParam.xml");
            else
                this._commandParam = new BindingList<CommandParam>();
            if (this._commandParam == null)
                this._commandParam = new BindingList<CommandParam>();
        }


    }
}
