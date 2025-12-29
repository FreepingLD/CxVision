using Common;
using HalconDotNet;
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
    public partial class ScaleParamForm : Form
    {
        private static object lockState = new object();
        private static ScaleParamForm _Instance;
        private bool isBreak = false;
        public static HXLDCont _crossIcon;
        private CameraParam cameraParam;
        private int width;
        private int height;
        private static object _lockState = new object();

        public DialogResult Result { get; set; }
        public static ScaleParamForm Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        _Instance = new ScaleParamForm();
                    }
                }
                return _Instance;
            }
        }

        public ScaleParamForm()
        {
            InitializeComponent();
            ScaleParamManager.Instance.Read();
            this.ShowInTaskbar = true;
            this.TopMost = true;
        }


        private void ScaleParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.isBreak = false;
                ScaleParamManager.Instance.Save();
            }
            catch
            {

            }
        }

        private void ScaleParamForm_Load(object sender, EventArgs e)
        {
            try
            {
                //ScaleParamManager.Instance.Read();
                this.颜色comboBox.DataSource = Enum.GetNames(typeof(enColor));
                this.半径textBox.DataBindings.Add("Text", ScaleParamManager.Instance.Param, "Radius", true, DataSourceUpdateMode.OnPropertyChanged);
                this.步距textBox.DataBindings.Add("Text", ScaleParamManager.Instance.Param, "StepDist", true, DataSourceUpdateMode.OnPropertyChanged);
                this.颜色comboBox.DataBindings.Add(nameof(this.颜色comboBox.Text), ScaleParamManager.Instance.Param, "Color", true, DataSourceUpdateMode.OnPropertyChanged);
                this.十字线checkBox.DataBindings.Add(nameof(this.十字线checkBox.Checked), ScaleParamManager.Instance.Param, "IsShowCrossMark", true, DataSourceUpdateMode.OnPropertyChanged);
                this.刻度线checkBox.DataBindings.Add(nameof(this.刻度线checkBox.Checked), ScaleParamManager.Instance.Param, "IsShowScaleMark", true, DataSourceUpdateMode.OnPropertyChanged);
                this.中心圆checkBox.DataBindings.Add(nameof(this.中心圆checkBox.Checked), ScaleParamManager.Instance.Param, "IsShowCircleMark", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {

            }
        }

        private void 确定Btn_Click(object sender, EventArgs e)
        {
            this.isBreak = false;
            this.Result = DialogResult.OK;
            lock (_lockState)
            {
                _crossIcon?.Dispose();
            }
            this.Close();
        }

        private void 取消Btn_Click(object sender, EventArgs e)
        {
            this.isBreak = false;
            this.Result = DialogResult.Cancel;
            this.Close();
        }

        public static HXLDCont GetCrossIcon(CameraParam cameraParam, int width=2048, int height=2048)
        {
            lock (_lockState)
            {
                if (_crossIcon == null || !_crossIcon.IsInitialized())
                {
                    _crossIcon = new HXLDCont();
                    _crossIcon.GenEmptyObj();
                    _crossIcon = _crossIcon.ConcatObj(new HXLDCont(new HTuple(height * 0.5, height * 0.5), new HTuple(0, width)));
                    _crossIcon = _crossIcon.ConcatObj(new HXLDCont(new HTuple(0, height), new HTuple(width * 0.5, width * 0.5)));
                    //////////////////////////////////////////////////////////////////////////////////
                    double step, radius, pixStep, pixRadius, center_row, center_col;
                    step = ScaleParamManager.Instance.Param.StepDist;
                    radius = ScaleParamManager.Instance.Param.Radius;
                    if (cameraParam != null)
                    {
                        pixStep = cameraParam.TransWcsLengthToPixLength(step);
                        pixRadius = cameraParam.TransWcsLengthToPixLength(radius);
                    }
                    else
                    {
                        pixStep = step;
                        pixRadius = radius;
                    }
                    if (height == 0)
                        height = 2048;
                    if (width == 0)
                        width = 2048;
                    /////////////////////////////////////////////////
                    center_row = height * 0.5;
                    center_col = width * 0.5;
                    if (pixStep < 1) pixStep = 1;
                    if (pixRadius < 2) pixRadius = 2;
                    int index = 1;
                    if (ScaleParamManager.Instance.Param.IsShowCircleMark)
                    {
                        HXLDCont hXLDCont1 = new HXLDCont();
                        hXLDCont1.GenCircleContourXld(height * 0.5, width * 0.5, pixRadius, 0.0, Math.PI * 2, "positive", 0.01);
                        _crossIcon = _crossIcon.ConcatObj(hXLDCont1);
                    }
                    /////////////////////////////////////////
                    if (ScaleParamManager.Instance.Param.IsShowScaleMark)
                    {
                        // 绘制行方向
                        index = 1;
                        while (true)
                        {
                            _crossIcon = _crossIcon.ConcatObj(new HXLDCont(new HTuple(center_row - pixStep * index, center_row - pixStep * index), new HTuple(center_col, center_col + pixStep)));
                            if (center_row - pixStep * index < 0) break;
                            index++;
                        }
                        index = 1;
                        while (true)
                        {
                            _crossIcon = _crossIcon.ConcatObj(new HXLDCont(new HTuple(center_row + pixStep * index, center_row + pixStep * index), new HTuple(center_col, center_col + pixStep)));
                            if (center_row + pixStep * index > height) break;
                            index++;
                        }
                        // 绘制列方向
                        index = 1;
                        while (true)
                        {
                            _crossIcon = _crossIcon.ConcatObj(new HXLDCont(new HTuple(center_row, center_row + pixStep), new HTuple(center_col - pixStep * index, center_col - pixStep * index)));
                            if (center_col - pixStep * index < 0) break;
                            index++;
                        }
                        index = 1;
                        while (true)
                        {
                            _crossIcon = _crossIcon.ConcatObj(new HXLDCont(new HTuple(center_row, center_row + pixStep), new HTuple(center_col + pixStep * index, center_col + pixStep * index)));
                            if (center_col + pixStep * index > width) break;
                            index++;
                        }
                    }
                    /////////////////////////////////////////
                    return _crossIcon; //.UnionAdjacentContoursXld(100, 10, "attr_keep");
                }
                else
                    return _crossIcon;
            }
        }

        public DialogResult ShowDialog(CameraParam cameraParam, int width, int height)
        {
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this.isBreak = true;
            this.Show();
            while (isBreak)
            {
                Application.DoEvents();
            }
            return this.DialogResult;
        }




    }
}
