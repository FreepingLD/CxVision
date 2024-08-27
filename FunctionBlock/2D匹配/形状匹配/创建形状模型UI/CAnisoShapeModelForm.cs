﻿using FunctionBlock;
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
    public partial class CAnisoShapeModelForm : Form
    {
        private C_AnisoShapeModelParam _shapeModel;
        public CAnisoShapeModelForm(C_AnisoShapeModelParam shapeModel)
        {
            this._shapeModel = shapeModel;
            InitializeComponent();
            BindProperty();
        }
        public CAnisoShapeModelForm(C_ShapeModelParamBase shapeModel)
        {
            this._shapeModel = (C_AnisoShapeModelParam)shapeModel;
            InitializeComponent();
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                this.金字塔层级comboBox.DataBindings.Add("Text", this._shapeModel, "NumLevels", true, DataSourceUpdateMode.OnPropertyChanged);
                this.起始角度textBox.DataBindings.Add("Text", this._shapeModel, "AngleStart", true, DataSourceUpdateMode.OnPropertyChanged);
                this.终止角度textBox.DataBindings.Add("Text", this._shapeModel, "AngleExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.角度步长comboBox.DataBindings.Add("Text", this._shapeModel, "AngleStep", true, DataSourceUpdateMode.OnPropertyChanged);
                this.行最小缩放textBox.DataBindings.Add("Text", this._shapeModel, "ScaleRMin", true, DataSourceUpdateMode.OnPropertyChanged);
                this.行最大缩放textBox.DataBindings.Add("Text", this._shapeModel, "ScaleRMax", true, DataSourceUpdateMode.OnPropertyChanged);
                this.行缩放步长textBox.DataBindings.Add("Text", this._shapeModel, "ScaleRStep", true, DataSourceUpdateMode.OnPropertyChanged);
                ///////////////////////
                this.列最小缩放textBox.DataBindings.Add("Text", this._shapeModel, "ScaleCMin", true, DataSourceUpdateMode.OnPropertyChanged);
                this.列最大缩放textBox.DataBindings.Add("Text", this._shapeModel, "ScaleCMax", true, DataSourceUpdateMode.OnPropertyChanged);
                this.列缩放步长textBox.DataBindings.Add("Text", this._shapeModel, "ScaleCStep", true, DataSourceUpdateMode.OnPropertyChanged);
                this.优化comboBox.DataBindings.Add("Text", this._shapeModel, "Optimization", true, DataSourceUpdateMode.OnPropertyChanged);
                this.极性comboBox.DataBindings.Add("Text", this._shapeModel, "Metric", true, DataSourceUpdateMode.OnPropertyChanged);
                this.对比度comboBox.DataBindings.Add("Text", this._shapeModel, "Contrast", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小对比度comboBox.DataBindings.Add("Text", this._shapeModel, "MinContrast", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}
