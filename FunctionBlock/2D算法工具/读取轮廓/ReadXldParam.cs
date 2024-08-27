using System;
using System.Collections.Generic;
using HalconDotNet;
using System.Windows.Forms;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;
using System.Reflection;

namespace AlgorithmsLibrary
{
    [Serializable]
    public class ReadXldParam
    {
        public List<string> FilePath { get; set; }
        public int FileCount
        {
            get;
            set;
        }
        public string SingleFilePath
        {
            get;
            set;
        }
        public string FolderPath
        {
            get;
            set;
        }

        public ReadXldParam()
        {
            this.FilePath = new List<string>();
            this.FileCount = 0;
            this.SingleFilePath = "";
            this.FolderPath = "";
        }

        public bool ReadHXLDCon(string path,out HXLDCont contXLD)
        {
            string state;
            contXLD = new HXLDCont();
            if (path == null) { throw new ArgumentNullException(); }  
            if (path.Trim().Length == 0) return false;
            HXLDCont hXLDCont = new HXLDCont();
            state = contXLD.ReadContourXldDxf(path, "max_approx_error", 0.001);
            return true; // new XldDataClass(hXLDCont);
        }
    }
}
