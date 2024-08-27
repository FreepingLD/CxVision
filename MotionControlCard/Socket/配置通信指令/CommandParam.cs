using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MotionControlCard
{
    [Serializable]
   public class CommandParam
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommandParam()
        {
            this.ParamList = new List<string>();
            this.IsActive = false;
        }

        /// <summary>
        /// 当前文化语言
        /// </summary>
        public List<string> ParamList
        {
            set;
            get;
        }

        public bool IsActive { get; set; }

        public string Describe { get; set; }

        public  CommandParam(string name)
        {
            this.ParamList = new List<string>();
            this.IsActive = false;
            this.Describe = name;
        }


    }





}
