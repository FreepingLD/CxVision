using Common;
using Sensor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    /// <summary>
    /// 管理程序类
    /// </summary>
    [Serializable]
    public class ProgramItemsSource
    {
        //private TreeNode node;
        //public event EditeNodeEventHandler Edite;
        //private CancellationTokenSource cts;
        //private TreeView treeView;
        private static ProgramItemsSource instance = new ProgramItemsSource();
        private Dictionary<string, IFunction> programItems;
        //// 在这里面可以加一个点击事件 
        //public event ClickNodeEventHandler NodeClick;
        public Dictionary<string, IFunction> ProgramItems
        {
            get
            {
                return programItems;
            }

            set
            {
                programItems = value;
            }
        }


        private ProgramItemsSource()
        {
            programItems = new Dictionary<string, IFunction>();
        }
        public static ProgramItemsSource getInstance()
        {
            if (instance == null)
                return new ProgramItemsSource();
            else
                return instance;
        }




    }
}
