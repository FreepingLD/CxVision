using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using LD.Config;
using LD.Ui;
using VisionBase;

namespace MainVision
{
    public partial class MainForm1 : Form
    {
        public static MainForm1 mainfrm = null;

        public MainForm1()
        {
            InitializeComponent();
        }
        private void MainForm1_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            this.Size = new Size(1260, 950);
            dockPanel1.Dock = DockStyle.Fill;
            ConfigSystem cfg = ConfigManager.Instance.ConfigSystem;
            label_ShowUserName.Text = cfg.Admin[cfg.State - 1];
            mainForm.Show(this.dockPanel1, DockState.Document);
            this.WindowState = FormWindowState.Normal;
            //this.FormClosing += MyFormClosing;
            this.Programlabel.Text = ProjectParaManager.Instance.ProductName;
        }
        
        /// <summary>
        /// 创建窗体实例
        /// </summary>
        public MainForm mainForm = new MainForm();

        private void button3_Click(object sender, EventArgs e)
        {
            UserLogin frmuser = new UserLogin();
            frmuser.ShowDialog();
            ConfigSystem cfg = ConfigManager.Instance.ConfigSystem;
            label_ShowUserName.Text = cfg.Admin[cfg.State - 1];
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox5.Text = DateTime.Now.ToString("yyyy/MM/dd");
            textBox4.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void MyFormClosing(object sender, FormClosingEventArgs e)
        {
            //Dialog MyDlg = 
            DialogResult DlgReslult = MessageBox.Show("是否关闭程序", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DlgReslult == DialogResult.No) e.Cancel = true;
        }

        private void label_ShowEqName_Click(object sender, EventArgs e)
        {

        }
    }
}
