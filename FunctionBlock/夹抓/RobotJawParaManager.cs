using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
   public class RobotJawParaManager
    {
        /// <summary> 机器人夹爪参数列表 </summary>
        public BindingList<RobotJawParam> RobotJawParaItems { get; set; }

        private static RobotJawParaManager _instance;

        private static object lockObj = new object();

        private static string Path = @"机器人夹爪参数";

        public static RobotJawParaManager Instance
        {
            get
            {
                lock (lockObj)
                {
                    return _instance = _instance ?? new RobotJawParaManager();
                }
            }
        }

        private RobotJawParaManager()
        {
            this.RobotJawParaItems = new BindingList<RobotJawParam>();
        }

        public bool Read()
        {
            bool IsOk = true;
            RobotJawParaManager Obj = XML<RobotJawParaManager>.Read(Path + @"\" + "RobotJawParaManager.xml");
            if (Obj == null) Obj = RobotJawParaManager.Instance;
            if (Obj.RobotJawParaItems.Count() == 0)
            {
                RobotJawParam item = new RobotJawParam();
                Obj.RobotJawParaItems = new BindingList<RobotJawParam>();
                Obj.RobotJawParaItems.Add(item);
            }
            _instance = Obj;
            return IsOk;
        }



        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(Path)) DirectoryEx.Create(Path);
            IsOk = IsOk && XML<RobotJawParaManager>.Save(this, Path + @"\" + "RobotJawParaManager.xml");
            return IsOk;
        }
        public userWcsVector GetJawValue (enRobotJawEnum JawName)
        {
            userWcsVector JawVector = new userWcsVector();
            foreach (var item in RobotJawParaItems)
            {
                if (item.JawName == JawName)
                    JawVector = new userWcsVector(item.X, item.Y, 0, item.Angle);
            }
            return JawVector;
        }
        public RobotJawParam GetJawParam(string JawName)
        {
            RobotJawParam JawParam = new RobotJawParam();
            foreach (var item in RobotJawParaItems)
            {
                if (item.JawName.ToString() == JawName.Trim())
                    JawParam = item;
            }
            return JawParam;
        }
        public RobotJawParam GetJawParam(enRobotJawEnum JawName)
        {
            RobotJawParam JawParam = new RobotJawParam();
            foreach (var item in RobotJawParaItems)
            {
                if (item.JawName == JawName)
                    JawParam = item;
            }
            return JawParam;
        }



    }
}
