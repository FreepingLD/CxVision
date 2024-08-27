using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    public class ViewConfigParam
    {
        public string CamName { get; set; }
        public Point Location { get; set; }
        public Size FormSize { get; set; }
        public string ContainerName { get; set; }
        public string FormName { get; set; }
        public string Tag { get; set; }
        public string ViewName { get; set; }
        public string ProgramNode { get; set; }
        public bool IsShowCross { get; set; }

        /////////////////////////
        public int BtnWid { get; set; }
        public int BtnHei { get; set; }
        public int ImageNum { get; set; }
        public string Path { get; set; }

        /// <summary>
        /// 视图的显示名称
        /// </summary>
        public string Text { get; set; }

        public PageConfigParam PageParam { get; set; }


        public ViewConfigParam()
        {
            this.CamName = "NONE";
            this.Location = new Point(50, 50);
            this.FormSize = new Size(300, 300);
            this.ContainerName = "NONE";
            this.FormName = "ViewForm";
            this.Tag = "NONE";
            this.ViewName = "NONE";
            this.ProgramNode = "NONE";
            this.IsShowCross = true;
            ////////////////////////
            this.BtnWid = 80;
            this.BtnHei = 50;
            this.ImageNum = 10;
            this.Path = "";
            this.Text = "";
            //////////////////////////////////////
            this.PageParam = new PageConfigParam();
        }


    }

    [Serializable]
    public class PageConfigParam
    {
        /////////////////////////
        public int BtnWid { get; set; }
        public int BtnHei { get; set; }
        public int ImageNumEdge1 { get; set; }
        public int ImageNumEdge2 { get; set; }
        public int ImageNumEdge3 { get; set; }
        public int ImageNumEdge4 { get; set; }
        public string Path { get; set; }
        public int DetectEdgeNum { get; set; }


        public PageConfigParam()
        {
            ////////////////////////
            this.BtnWid = 80;
            this.BtnHei = 50;
            this.ImageNumEdge1 = 10;
            this.ImageNumEdge2 = 10;
            this.ImageNumEdge3 = 10;
            this.ImageNumEdge4 = 10;
            this.DetectEdgeNum = 1;
            this.Path = "";
        }



    }
}
