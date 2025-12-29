using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{

   public class ScreenConfigParam
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public ScreenConfigParam()
        {
            this.WidthScale = 1;
            this.HeightScale = 1;
            this.Width = 1920;
            this.Height = 1152;
        }

        /// <summary>
        /// 重置参数
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void InitParam(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
        public void SetScaleParam(int width, int height)
        {
            this.WidthScale = width * 1.0 / this.Width * 1.0;
            this.HeightScale = height * 1.0 / this.Height * 1.0;
        }

        public int Width
        {
            set;
            get;
        }
        public int Height
        {
            set;
            get;
        }
        public double WidthScale
        {
            set;
            get;
        }
        public double HeightScale
        {
            set;
            get;
        }







    }



}
