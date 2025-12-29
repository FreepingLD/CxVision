using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
   public class AlignGuidedParamManager
    {
        /// <summary> 机器人夹爪参数列表 </summary>
        public BindingList<AlignGuidedParam> RobotJawParaItems { get; set; }

        private static AlignGuidedParamManager _instance;

        private static object lockObj = new object();

        private static string Path = @"机器人夹爪参数";

        public static AlignGuidedParamManager Instance
        {
            get
            {
                lock (lockObj)
                {
                    return _instance = _instance ?? new AlignGuidedParamManager();
                }
            }
        }

        private AlignGuidedParamManager()
        {
            this.RobotJawParaItems = new BindingList<AlignGuidedParam>();
        }

        public bool Read()
        {
            bool IsOk = true;
            AlignGuidedParamManager Obj = XML<AlignGuidedParamManager>.Read(Path + @"\" + "AlignGuidedParamManager.xml");
            if (Obj == null) Obj = AlignGuidedParamManager.Instance;
            if (Obj.RobotJawParaItems.Count() == 0)
            {
                AlignGuidedParam item = new AlignGuidedParam();
                Obj.RobotJawParaItems = new BindingList<AlignGuidedParam>();
                Obj.RobotJawParaItems.Add(item);
            }
            _instance = Obj;
            return IsOk;
        }
        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(Path)) DirectoryEx.Create(Path);
            IsOk = IsOk && XML<AlignGuidedParamManager>.Save(this, Path + @"\" + "AlignGuidedParamManager.xml");
            return IsOk;
        }





    }
}
