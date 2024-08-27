using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class AddViewForm : Form
    {

        private List<Form> listForm = null;

        public string FormName { get; set; }
        public string ViewName { get; set; }

        public bool IsCancel { get; set; }
        public AddViewForm(List<Form> listForm)
        {
            InitializeComponent();
            this.listForm = listForm;
        }
        public AddViewForm(string viewType = "NONE")
        {
            InitializeComponent();
            this.窗体类型comboBox.DataSource = Enum.GetValues(typeof(enViewForm));
            this.FormName = viewType;
            this.ViewName = "NONE";
            this.视图名称comboBox.Items.Clear();
            this.视图名称comboBox.Items.Add("NONE");
            foreach (var item in Sensor.SensorManage.GetCamSensorName())
            {
                this.视图名称comboBox.Items.Add(item);
            }
            this.窗体类型comboBox.Text = viewType;
        }

        private void 确定button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.窗体类型comboBox.Text != null)
                    this.FormName = this.窗体类型comboBox.Text.ToString().Split('_')[0];
                else
                    return;
                if (this.视图名称comboBox.Text != null)
                {
                    switch (this.窗体类型comboBox.Text.Trim())
                    {
                        case nameof(enViewForm.TabPage_视图页面):
                            this.ViewName = this.视图名称comboBox.Text.ToString();
                            break;
                        case nameof(enViewForm.ViewForm_视图窗体):
                            this.ViewName = "图像视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.ImageViewForm_图像视图窗体):
                            this.ViewName = "图像视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.GraphicViewForm_图形视图窗体):
                            this.ViewName = "图形视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.DisplayPositionForm_轴位置视图窗体):
                            this.ViewName = "位置视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.JogMotionForm_Jog视图窗体):
                            this.ViewName = "Jog视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.LightControlForm_光源视图窗体):
                            this.ViewName = "光源视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.ElementViewForm_元素视图窗体):
                            this.ViewName = "元素视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.ProgramForm_程序视图窗体):
                            this.ViewName = "程序视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.DataDisplayForm_数据显示视图窗体):
                            this.ViewName = "数据视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.ReportForm_报表窗体):
                            this.ViewName = "报表视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.RobotJawParaManagerForm_夹抓窗体):
                            this.ViewName = "夹抓视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.FlawDetecteViewForm_瑕疵检测窗体):
                            this.ViewName = "瑕疵视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.ThicknessViewForm_对射测厚):
                            this.ViewName = "对射测厚:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;  //
                        case nameof(enViewForm.ProgramViewForm_程序配方):
                            this.ViewName = "程序配方:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;  //
                        case nameof(enViewForm.MeasureViewForm_测量配置):
                            this.ViewName = "测量配置:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;  //
                        case nameof(enViewForm.OkNgViewForm_结果显示):
                            this.ViewName = "结果显示:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;  //
                        case nameof(enViewForm.OperateViewForm_操作员):
                            this.ViewName = "操作员:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;  //
                        case nameof(enViewForm.ShowDataForm_数据展示):
                            this.ViewName = "数据展示:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.AlignDataForm_对位数据):
                            this.ViewName = "对位数据:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.LoadViewForm_上料视图):
                            this.ViewName = "上料视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                        case nameof(enViewForm.TryPlateParamViewForm_参数视图):
                            this.ViewName = "Try盘参数视图:" + "[" + this.视图名称comboBox.Text.ToString() + "]";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.IsCancel = false;
            this.Close();
        }

        private void AddViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.FormName = "";
        }

        private void 取消Btn_Click(object sender, EventArgs e)
        {
            this.IsCancel = true;
            this.Close();
        }
    }


    public enum enViewForm
    {
        NONE,
        TabPage_视图页面,
        ViewForm_视图窗体,
        ImageViewForm_图像视图窗体,
        GraphicViewForm_图形视图窗体,
        DisplayPositionForm_轴位置视图窗体,
        JogMotionForm_Jog视图窗体,
        LightControlForm_光源视图窗体,
        ElementViewForm_元素视图窗体,
        ProgramForm_程序视图窗体,
        DataDisplayForm_数据显示视图窗体,
        ReportForm_报表窗体,
        RobotJawParaManagerForm_夹抓窗体,
        FlawDetecteViewForm_瑕疵检测窗体,
        ThicknessViewForm_对射测厚,
        ProgramViewForm_程序配方,
        MeasureViewForm_测量配置,
        OkNgViewForm_结果显示,
        OperateViewForm_操作员,
        ShowDataForm_数据展示,
        AlignDataForm_对位数据,
        LoadViewForm_上料视图,
        TryPlateParamViewForm_参数视图,
    }


}
