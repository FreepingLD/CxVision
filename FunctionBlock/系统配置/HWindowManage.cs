using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace FunctionBlock
{
    public class HWindowManage
    {
        private static  Dictionary<string, HWindow> hWindowList =new  Dictionary<string, HWindow>();
        public static Dictionary<string, HWindow> HWindowList { get => hWindowList; set => hWindowList = value; }

        private static Dictionary<string, HWindowControl> hWindowControlList = new Dictionary<string, HWindowControl>();
        public static Dictionary<string, HWindowControl> HWindowControlList { get => hWindowControlList; set => hWindowControlList = value; }

        public static string[] GetKeysList()
        {
            string[] hWindows = new string[hWindowList.Count];
            if (hWindowList == null || hWindowList.Count == 0) return null;
            hWindowList.Keys.CopyTo(hWindows, 0);
            return hWindows;
        }


    }
}
