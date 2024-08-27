using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common
{
    [Serializable]
    public class AutoReSizeForm
    {
        private int Y = 840;
        private int X = 525;
        public double SH
        {
            get
            {
                return (double)Screen.PrimaryScreen.WorkingArea.Height / (double)Y;
            }
        }
        public double SW
        {
            get
            {
                return (double)Screen.PrimaryScreen.WorkingArea.Width / (double)X;
            }
        }


        public void SetFormSize(int width, int height)
        {
            X = width;
            Y = height;
        }
        public void SetFormSize(Control fm)
        {
            fm.Location = new Point(Convert.ToInt32((double)fm.Location.X * SW), Convert.ToInt32((double)fm.Location.Y * SH));
            fm.Size = new Size(Convert.ToInt32((double)fm.Size.Width * SW), Convert.ToInt32((double)fm.Size.Height * SH));
            fm.Font = new Font(fm.Font.Name, Convert.ToSingle(fm.Font.Size * SH), fm.Font.Style, fm.Font.Unit, fm.Font.GdiCharSet, fm.Font.GdiVerticalFont);
            if (fm.Controls.Count != 0)
            {
                SetControlSize(fm);
            }
        }
        public void SetFormSize(Control fm, int newWidth, int newHeight, int oldWidth, int oldHeight)
        {
            double sw = (double)newWidth / (double)oldWidth;
            double sh = (double)newHeight / (double)oldHeight;
            fm.Location = new Point(Convert.ToInt32((double)fm.Location.X * sw), Convert.ToInt32((double)fm.Location.Y * sh));
            fm.Size = new Size(newWidth, newHeight);
            fm.Font = new Font(fm.Font.Name, Convert.ToSingle(fm.Font.Size * (sh - 0.3)), fm.Font.Style, fm.Font.Unit, fm.Font.GdiCharSet, fm.Font.GdiVerticalFont);
            if (fm.Controls.Count != 0)
            {
                SetControlSize(fm, newWidth, newHeight, oldWidth, oldHeight);
            }
        }
        private void SetControlSize(Control InitC)
        {
            foreach (Control c in InitC.Controls)
            {
                c.Location = new Point(Convert.ToInt32((double)c.Location.X * SW), Convert.ToInt32((double)c.Location.Y * SH));
                c.Size = new Size(Convert.ToInt32((double)c.Size.Width * SW), Convert.ToInt32((double)c.Size.Height * SH));
                //c.Font = new Font(c.Font.Name, Convert.ToSingle(c.Font.Size * SH * (100 / 100)), c.Font.Style, c.Font.Unit, c.Font.GdiCharSet, c.Font.GdiVerticalFont);
                if (c.Controls.Count != 0)
                {
                    SetControlSize(c);
                }
            }
        }
        private void SetControlSize(Control InitC, int newWidth, int newHeight, int oldWidth, int oldHeight)
        {
            double sw = (double)newWidth / (double)oldWidth;
            double sh = (double)newHeight / (double)oldHeight;
            foreach (Control c in InitC.Controls)
            {
                c.Location = new Point(Convert.ToInt32((double)c.Location.X * sw), Convert.ToInt32((double)c.Location.Y * sh));
                c.Size = new Size(Convert.ToInt32((double)c.Size.Width * sw), Convert.ToInt32((double)c.Size.Height * sh));
                //c.Font = new Font(c.Font.Name, Convert.ToSingle(c.Font.Size * sh * (100 / 100)), c.Font.Style, c.Font.Unit, c.Font.GdiCharSet, c.Font.GdiVerticalFont);
                if (c.Controls.Count != 0)
                {
                    SetControlSize(c, newWidth, newHeight, oldWidth, oldHeight);
                }
            }
        }

    }


    [Serializable]

    /// <summary>
    /// 对窗体上的控件进行了自适应的方式,同时将控件中的控件进行了自适应调整：控件包含窗体上的控件各控件中的控件
    /// </summary>
    public class AutoReSizeFormClass
    {
        // 声明结构,只记录窗体和其控件的初始位置和大小
        public struct ControlRect
        {
            public int Left;
            public int Top;
            public int Width;
            public int Height;
        }
        public List<ControlRect> oldCtrl = new List<ControlRect>();
        int ctrlNo = 0;
        // 记录窗体和其控件的初始位置和大小,  
        public void controllInitializeSize(Control mForm)
        {
            ControlRect cR;
            cR.Left = mForm.Left; cR.Top = mForm.Top; cR.Width = mForm.Width; cR.Height = mForm.Height;
            oldCtrl.Add(cR);//第一个为"窗体本身",只加入一次即可  
            AddControl(mForm);
        }

        private void AddControl(Control ctl)
        {
            foreach (Control c in ctl.Controls)
            {
                ControlRect objCtrl;
                objCtrl.Left = c.Left; objCtrl.Top = c.Top; objCtrl.Width = c.Width; objCtrl.Height = c.Height;
                oldCtrl.Add(objCtrl);
                //**放在这里，是先记录控件本身，后记录控件的子控件  
                if (c.Controls.Count > 0)
                    AddControl(c);//窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用  （在方法的内部调用方法自身）
            }
        }

        //(3.2)控件自适应大小,  
        public void controlAutoSize(Control mForm)
        {
            if (ctrlNo == 0)
            { //*如果在窗体的Form1_Load中，记录控件原始的大小和位置，正常没有问题，但要加入皮肤就会出现问题，因为有些控件如dataGridView的的子控件还没有完成，个数少  
              //*要在窗体的Form1_SizeChanged中，第一次改变大小时，记录控件原始的大小和位置,这里所有控件的子控件都已经形成  
                ControlRect cR;
                //  cR.Left = mForm.Left; cR.Top = mForm.Top; cR.Width = mForm.Width; cR.Height = mForm.Height;  为什么不用这个语句？
                cR.Left = 0; cR.Top = 0; cR.Width = mForm.PreferredSize.Width; cR.Height = mForm.PreferredSize.Height;

                oldCtrl.Add(cR);//第一个为"窗体本身",只加入一次即可  
                AddControl(mForm);//窗体内其余控件可能嵌套其它控件(比如panel),故单独抽出以便递归调用  
            }
            float wScale = (float)mForm.Width / (float)oldCtrl[0].Width;//新旧窗体之间的比例，与最早的旧窗体  
            float hScale = (float)mForm.Height / (float)oldCtrl[0].Height;//.Height;  
            ctrlNo = 1;//进入=1，第0个为窗体本身,窗体内的控件,从序号1开始  
            AutoScaleControl(mForm, wScale, hScale);//窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用  
        }

        private void AutoScaleControl(Control ctl, float wScale, float hScale)
        {
            int ctrLeft0, ctrTop0, ctrWidth0, ctrHeight0;
            //int ctrlNo = 1;//第1个是窗体自身的 Left,Top,Width,Height，所以窗体控件从ctrlNo=1开始  
            foreach (Control c in ctl.Controls)
            {
                ctrLeft0 = oldCtrl[ctrlNo].Left;
                ctrTop0 = oldCtrl[ctrlNo].Top;
                ctrWidth0 = oldCtrl[ctrlNo].Width;
                ctrHeight0 = oldCtrl[ctrlNo].Height;
                //调整窗体尺寸
                c.Left = (int)((ctrLeft0) * wScale);//新旧控件之间的线性比例。控件位置只相对于窗体，所以不能加 + wLeft1  
                c.Top = (int)((ctrTop0) * hScale);//  
                c.Width = (int)(ctrWidth0 * wScale);//只与最初的大小相关，所以不能与现在的宽度相乘 (int)(c.Width * w);  
                c.Height = (int)(ctrHeight0 * hScale);//  
                ctrlNo++;//累加序号  
                //**放在这里，是先缩放控件本身，后缩放控件的子控件  
                if (c.Controls.Count > 0)
                    AutoScaleControl(c, wScale, hScale);//窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用  

                if (ctl is DataGridView)
                {
                    DataGridView dgv = ctl as DataGridView;
                    Cursor.Current = Cursors.WaitCursor;

                    int widths = 0;
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        dgv.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);  // 自动调整列宽    
                        widths += dgv.Columns[i].Width;   // 计算调整列后单元列的宽度和                         
                    }
                    if (widths >= ctl.Size.Width)  // 如果调整列的宽度大于设定列宽    
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;  // 调整列的模式 自动    
                    else
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;  // 如果小于 则填充    

                    Cursor.Current = Cursors.Default;
                }
            }


        }


    }

    [Serializable]
    public class AutoReSizeFormControls
    {
        private float originalWidth;
        private float originalHeight;
        private float scaleWidth;
        private float scaleHeight;

        public void GetFormSize(Control cons, int width, int height)
        {
            this.originalWidth =(float) width;
            this.originalHeight = (float)height;
            setTag(cons);
        }

        public void GetScale(int width, int height)
        {
            if (originalWidth != 0) scaleWidth = width / originalWidth;
            else
                scaleWidth = 1;

            if (originalHeight != 0) scaleHeight = height / originalHeight;
            else
                scaleHeight = 1;
        }

        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0) setTag(con);   //如果此控件存在子控件，则此控件的子控件执行此函数一次
            }
        }

        public void ResetControlSize(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                if (con.Tag != null)
                {
                    string[] myTag = con.Tag.ToString().Split(':');     //将con的宽、高、左边距、顶边距离及字体大小通过字符“:”分割成数组
                    float a = Convert.ToSingle(myTag[0]) * scaleWidth;    //根据窗口的缩放比例确定控件相应的值，宽度
                    con.Width = (int)a;
                    a = Convert.ToSingle(myTag[1]) * scaleHeight;    //高度
                    con.Height = (int)a;
                    a = Convert.ToSingle(myTag[2]) * scaleWidth;    //左边距
                    con.Left = (int)a;
                    a = Convert.ToSingle(myTag[3]) * scaleHeight;    //顶边距离
                    con.Top = (int)a;
                    Single currentSize = Convert.ToSingle(myTag[4]) * scaleHeight;     //字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0) ResetControlSize(con);     //如果此控件存在子控件，则将相应子控件执行一次setControls函数
                }
            }
        }






    }






}
