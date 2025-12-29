using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace FunctionBlock
{
    public class DataPrefixConfigManager
    {
        private static int headCount = 0;
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static DataPrefixConfigManager _Instance;
        public static DataPrefixConfigManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new DataPrefixConfigManager();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<DataPrefixConfig> _DataItemParamList = new BindingList<DataPrefixConfig>();

        public BindingList<DataPrefixConfig> DataItemParamList { get => _DataItemParamList; set => _DataItemParamList = value; }
        public static int HeadCount { get => headCount; set => headCount = value; }

        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<DataPrefixConfig>>.Save(_DataItemParamList, ParaPath + "\\" + "DataPrefixConfig.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "DataPrefixConfig.xml"))
                this._DataItemParamList = XML<BindingList<DataPrefixConfig>>.Read(ParaPath + "\\" + "DataPrefixConfig.xml");
            else
            {
                this._DataItemParamList = new BindingList<DataPrefixConfig>();
                this._DataItemParamList.Add(new DataPrefixConfig());
            }
        }




    }
}
