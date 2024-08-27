using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;

namespace FunctionBlock
{
    public partial class show_img : Form
    {
        public show_img(int image_size, uint cols, uint row, double[] image_data, double[] quality_data)
        {
            InitializeComponent();

            img_size = image_size;
            img_f_size = img_size * 4;
            columns = cols;
            rows = row;
            this.image_data = image_data;
            this.quality_data = quality_data;
        }
        public show_img(int image_size, uint cols, uint row)
        {
            InitializeComponent();

            img_size = image_size;
            img_f_size = img_size * 4;
            columns = cols;
            rows = row;
        }

        int img_size = 0;
        int img_f_size = 0;
        IntPtr height_pointer;
        IntPtr qualiti_pointer;
        float offset = 0;
        uint columns = 0;
        uint rows = 0;

        ColorPalette grayscal_palette;

        Bitmap height_img;
        Bitmap pseudo_img;
        Bitmap quality_img;
        Bitmap offset_img;

        double[] image_data;
        double[] quality_data;
        private void show_img_Load(object sender, EventArgs e)
        {
            grayscal_palette = creat_grayscall_palette();
            show_data();
        }

        public void show_data()
        {
            //height_pointer = Marshal.AllocHGlobal(img_f_size);
            //qualiti_pointer = Marshal.AllocHGlobal(img_f_size);

            //float[] image_data = new float[img_size];
            //float[] quality_data = new float[img_size];

            //    smartVISwrap.SV3D_getHeightData(height_pointer, qualiti_pointer, ref columns, ref rows, out offset);

            //Marshal.Copy(height_pointer, image_data, 0, img_size);
            //Marshal.Copy(qualiti_pointer, quality_data, 0, img_size);

            //File.WriteAllLines("mydata.txt", image_data.Select(d => d.ToString()));
            byte[] image_b_data = new byte[img_size];
            byte[] quality_b_data = new byte[img_size];
            float max_i = (float)image_data.Max();
            for (int i = 0; i < image_data.Length; i++)
            {
                if (image_data != null && image_data.Length > 0)
                    image_b_data[i] = (byte)((image_data[i] / max_i) * 255);
                if (quality_data != null && quality_data.Length > 0)
                    quality_b_data[i] = (byte)(quality_data[i] * 255);
            }

            height_img = byte_to_bmp(image_b_data, grayscal_palette);
            pictureBox1.Image = height_img;
            ColorPalette rain = load_palette("rainbow.col");
            pseudo_img = byte_to_bmp(image_b_data, rain);
            quality_img = byte_to_bmp(quality_b_data, grayscal_palette);

        }

        private void show_img_FormClosing(object sender, FormClosingEventArgs e)
        {
            Marshal.FreeHGlobal(height_pointer);
            Marshal.FreeHGlobal(qualiti_pointer);
        }

        public ColorPalette creat_grayscall_palette()
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

        public ColorPalette load_palette(string filename)
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

        private Bitmap byte_to_bmp(byte[] bdata, ColorPalette palette)
        {
            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap((int)columns, (int)rows, PixelFormat.Format8bppIndexed);
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

        /// <summary>
        /// Transform a raw high data array into a byte image
        /// </summary>
        /// <param name="raw_data"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public byte[] image_transform(byte[] raw_data, Int32 min, double max)
        {
            byte[] output = new byte[raw_data.Length * 3];
            int[] rgb = new int[3];

            for (int i = 0; i < output.Length; i += 3)
            {
                //Skalieren der Höhendaten auf HSV Farbraum
                double H_Value = ((raw_data[i / 3] + Math.Abs(min)) / ((max) + Math.Abs(min))) * 360.0;
                rgb = hsv_to_rgb(H_Value);

                output[i] = Convert.ToByte(rgb[0]);
                output[i + 1] = Convert.ToByte(rgb[1]);
                output[i + 2] = Convert.ToByte(rgb[2]);
            }

            return output;
        }

        /// <summary>
        /// Farbraumkonvertierung HSV Winkel zu RGB
        /// </summary>
        /// <param name="H"></param>
        /// <returns></returns>
        public int[] hsv_to_rgb(double H)
        {
            int[] output = new int[3];
            double[] temp = new double[3];

            double V = 1;
            double S = 1;

            double C = V * S;
            double X = C * (1 - Math.Abs((H / 60) % 2 - 1));
            double m = V - C;

            if (0 <= H & H < 60)
            {
                temp[0] = C;
                temp[1] = X;
                temp[2] = 0;
            }
            else if (60 <= H & H < 120)
            {
                temp[0] = X;
                temp[1] = C;
                temp[2] = 0;
            }
            else if (120 <= H & H < 180)
            {
                temp[0] = 0;
                temp[1] = C;
                temp[2] = X;
            }
            else if (180 <= H & H < 240)
            {
                temp[0] = 0;
                temp[1] = X;
                temp[2] = C;
            }
            else if (240 <= H & H < 300)
            {
                temp[0] = X;
                temp[1] = 0;
                temp[2] = C;
            }
            else if (300 <= H & H < 360)
            {
                temp[0] = C;
                temp[1] = 0;
                temp[2] = X;
            }

            output[0] = (int)((temp[0] + m) * 255);
            output[1] = (int)((temp[1] + m) * 255);
            output[2] = (int)((temp[2] + m) * 255);

            return output;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = height_img;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pseudo_img;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = quality_img;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.ShowDialog();

            if (sf.FileName != "")
            {
                pictureBox1.Image.Save(sf.FileName + ".bmp", ImageFormat.Bmp);
            }
        }
    }
}
