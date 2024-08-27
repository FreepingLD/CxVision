using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class ReportData
    {
        [DisplayName("图像")]
        public Bitmap Image { get; set; }

        [DisplayName("时间")]
        public string DateTime { get; set; }

        [DisplayName("产品编号")]
        public string ProductID { get; set; }

        [DisplayName("X1偏移")]
        public double X1 { get; set; }
        [DisplayName("Y1偏移")]
        public double Y1 { get; set; }
        [DisplayName("X2偏移")]
        public double X2 { get; set; }
        [DisplayName("Y2偏移")]
        public double Y2 { get; set; }

        [DisplayName("结果")]
        public string Result { get; set; }

        public ReportData()
        {
        }

        public ReportData(HImage image,string productID, int x1, int y1, double x2, double y2)
        {
            //this.Image = ConvertToBitmap(image);
            this.ProductID = productID;
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
            this.Result = "NG";
        }

        public Bitmap ConvertToBitmap(HalconDotNet.HImage image)
        {
            if (image != null && image.IsInitialized())
            {
                int width, height, verticalPitch, horizontalBitPitch, bitsPerPixel;
                IntPtr intPtrSource = image.GetImagePointer1Rect(out width, out height, out verticalPitch, out horizontalBitPitch, out bitsPerPixel);
                byte[] sourceData = new byte[width * height];
                Marshal.Copy(intPtrSource, sourceData, 0, width * height);
                Bitmap bitmap = ByteToBmp(sourceData, width, height, CreateColorPalette());
                return bitmap;
            }
            else
            {
                return null;
            }
        }
        private ColorPalette CreateColorPalette()
        {
            Bitmap bmp = new Bitmap(10, 10, PixelFormat.Format8bppIndexed);
            ColorPalette cp = bmp.Palette;
            Color[] cm_entries = cp.Entries;
            for (int i = 0; i < 256; i++)
            {
                Color b = new Color();
                b = Color.FromArgb((byte)i, (byte)i, (byte)i);
                cm_entries[i] = b;
            }
            return cp;
        }
        private ColorPalette load_palette(string filename)
        {
            Bitmap bmp = new Bitmap(10, 10, PixelFormat.Format8bppIndexed);
            ColorPalette cp = bmp.Palette;
            Color[] cm_entries = cp.Entries;

            if (!File.Exists(filename))
            {
                for (int i = 0; i < 256; i++)
                {
                    Color b = new Color();
                    b = Color.FromArgb(i, i, i);
                    cm_entries[i] = b;
                }
            }
            else
            {
                string[] color_table = File.ReadAllLines(filename);
                for (int i = 3; i < 258; i++)
                {
                    string[] col_str = color_table[i].Split(' ');
                    Color b = new Color();
                    b = Color.FromArgb(Convert.ToByte(col_str[0]), Convert.ToByte(col_str[1]), Convert.ToByte(col_str[2]));
                    cm_entries[i - 3] = b;
                }
            }
            return cp;
        }
        private Bitmap ByteToBmp(byte[] bdata, int width, int height, ColorPalette palette)
        {
            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            bmp.Palette = palette;
            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(
                                 new Rectangle(0, 0, bmp.Width, bmp.Height),
                                 ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(bdata, 0, bmpData.Scan0, bdata.Length);
            //Unlock the pixels
            bmp.UnlockBits(bmpData);
            //Return the bitmap 
            return bmp;
        }

        public override string ToString()
        {
            return string.Join(",", this.ProductID,this.X1, this.Y1, this.X2, this.Y2);
        }


    }


}
