using AlgorithmsLibrary;
using Common;
using FunctionBlock;
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
using View;

namespace FunctionBlock
{
    public partial class SelectParamForm : Form
    {
        private bool isLoad = false;
        private List<FlawClassParam> _classParamList;
        private FlawClassParam _MyClassPara;

        public SelectParamForm(List<FlawClassParam> classParamList)
        {
            InitializeComponent();
            this._classParamList = classParamList;
        }

        private void SelectParamForm_Load(object sender, EventArgs e)
        {
            this.isLoad = true;
        }

        private void 添加Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.瑕疵描述textBox.Text != null)
                {
                    if (this.IsContainClassName(this.瑕疵描述textBox.Text))
                        MessageBox.Show("已包含有相同的类型，请重新指定类名");
                    else
                    {
                        this._classParamList.Add(new FlawClassParam(this.瑕疵描述textBox.Text));
                        this.瑕疵类型comboBox.Items.Add(this.瑕疵描述textBox.Text);
                        this.瑕疵类型comboBox.SelectedItem = this.瑕疵描述textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 删除Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.瑕疵描述textBox.Text != null)
                {
                    this.瑕疵类型comboBox.Items.Remove(this.瑕疵描述textBox.Text);
                    if (this.IsContainClassName(this.瑕疵描述textBox.Text))
                        this._classParamList.Remove(this.GetClassParam(this.瑕疵描述textBox.Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 保存Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this.UpdateClassParam(this._MyClassPara);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void UpdateClassParam(FlawClassParam ClassParaIn)
        {
            ClassParaIn.FlawClassName = this.瑕疵描述textBox.Text;
            ClassParaIn.IsAcitve = this.激活checkBox.Checked;
            ClassParaIn.FlawArea = this.面积usrCtrlCondition.Value;
            ClassParaIn.FlawLen1 = this.长度usrCtrlCondition.Value;
            ClassParaIn.FlawLen2 = this.宽度usrCtrlCondition.Value;
            ClassParaIn.FlawGrayMean = this.平均灰度usrCtrlCondition.Value;
            ClassParaIn.FlawGrayDiff = this.灰度差异usrCtrlCondition.Value;
            ClassParaIn.FlawCircularity = this.圆度usrCtrlCondition.Value;
            ClassParaIn.FlawRectangularity = this.矩形度usrCtrlCondition.Value;
            ClassParaIn.FlawCompactness = this.紧密度usrCtrlCondition.Value;
            // 瑕疵数量usrCtrlCondition.Value = ClassParaIn.FlawCountThd;
            ClassParaIn.FlawLwRate = this.长宽比usrCtrlCondition.Value;
            ClassParaIn.FlawDiameter = this.直径usrCtrlCondition.Value;
        }
        private void UpdateFrm(FlawClassParam ClassParaIn)
        {
            if (ClassParaIn != null)
            {
                this.瑕疵描述textBox.Text = ClassParaIn.FlawClassName;
                this.激活checkBox.Checked = ClassParaIn.IsAcitve;
                this.面积usrCtrlCondition.Value = ClassParaIn.FlawArea;
                this.长度usrCtrlCondition.Value = ClassParaIn.FlawLen1;
                this.宽度usrCtrlCondition.Value = ClassParaIn.FlawLen2;
                this.平均灰度usrCtrlCondition.Value = ClassParaIn.FlawGrayMean;
                this.灰度差异usrCtrlCondition.Value = ClassParaIn.FlawGrayDiff;
                this.圆度usrCtrlCondition.Value = ClassParaIn.FlawCircularity;
                this.矩形度usrCtrlCondition.Value = ClassParaIn.FlawRectangularity;
                this.紧密度usrCtrlCondition.Value = ClassParaIn.FlawCompactness;
                // 瑕疵数量usrCtrlCondition.Value = ClassParaIn.FlawCountThd;
                this.长宽比usrCtrlCondition.Value = ClassParaIn.FlawLwRate;
                this.直径usrCtrlCondition.Value = ClassParaIn.FlawDiameter;
            }
        }

        private void 瑕疵类型comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.瑕疵类型comboBox.SelectedItem == null) return;
                this._MyClassPara = this.GetClassParam(this.瑕疵类型comboBox.SelectedItem.ToString());
                this.瑕疵描述textBox.Text = this.瑕疵类型comboBox.SelectedItem.ToString();
                this.UpdateFrm(this._MyClassPara);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool IsContainClassName(string className)
        {
            if (this._classParamList == null) return false;
            foreach (var item in _classParamList)
            {
                if (item.FlawClassName == className) return true;
            }
            return false;
        }

        private FlawClassParam GetClassParam(string className)
        {
            if (this._classParamList == null) return null;
            foreach (var item in _classParamList)
            {
                if (item.FlawClassName == className) return item;
            }
            return null;
        }

        private void 面积usrCtrlCondition_ValueChangeEvent(double Min, double Max, bool isActive)
        {
            try
            {
                if (this._MyClassPara != null && this.isLoad)
                    this._MyClassPara.FlawArea = new userControl.St_Condition(Min, Max, isActive);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 长度usrCtrlCondition_ValueChangeEvent(double Min, double Max, bool isActive)
        {
            try
            {
                if (this._MyClassPara != null && this.isLoad)
                    this._MyClassPara.FlawLen1 = new userControl.St_Condition(Min, Max, isActive);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 宽度usrCtrlCondition_ValueChangeEvent(double Min, double Max, bool isActive)
        {
            try
            {
                if (this._MyClassPara != null && this.isLoad)
                    this._MyClassPara.FlawLen2 = new userControl.St_Condition(Min, Max, isActive);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 长宽比usrCtrlCondition_ValueChangeEvent(double Min, double Max, bool isActive)
        {
            try
            {
                if (this._MyClassPara != null && this.isLoad)
                    this._MyClassPara.FlawLwRate = new userControl.St_Condition(Min, Max, isActive);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 平均灰度usrCtrlCondition_ValueChangeEvent(double Min, double Max, bool isActive)
        {
            try
            {
                if (this._MyClassPara != null && this.isLoad)
                    this._MyClassPara.FlawGrayMean = new userControl.St_Condition(Min, Max, isActive);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 灰度差异usrCtrlCondition_ValueChangeEvent(double Min, double Max, bool isActive)
        {
            try
            {
                if (this._MyClassPara != null && this.isLoad)
                    this._MyClassPara.FlawGrayDiff = new userControl.St_Condition(Min, Max, isActive);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 圆度usrCtrlCondition_ValueChangeEvent(double Min, double Max, bool isActive)
        {
            try
            {
                if (this._MyClassPara != null && this.isLoad)
                    this._MyClassPara.FlawCircularity = new userControl.St_Condition(Min, Max, isActive);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 矩形度usrCtrlCondition_ValueChangeEvent(double Min, double Max, bool isActive)
        {
            try
            {
                if (this._MyClassPara != null && this.isLoad)
                    this._MyClassPara.FlawRectangularity = new userControl.St_Condition(Min, Max, isActive);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 紧密度usrCtrlCondition_ValueChangeEvent(double Min, double Max, bool isActive)
        {
            try
            {
                if (this._MyClassPara != null && this.isLoad)
                    this._MyClassPara.FlawCompactness = new userControl.St_Condition(Min, Max, isActive);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 直径usrCtrlCondition_ValueChangeEvent(double Min, double Max, bool isActive)
        {
            try
            {
                if (this._MyClassPara != null && this.isLoad)
                    this._MyClassPara.FlawDiameter = new userControl.St_Condition(Min, Max, isActive);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 单位长度缺陷数量usrCtrlCondition_ValueChangeEvent(double Min, double Max, bool isActive)
        {
            try
            {
                if (this._MyClassPara != null && this.isLoad)
                    this._MyClassPara.FlawCountPerUnit = new userControl.St_Condition(Min, Max, isActive);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }




    }
}
