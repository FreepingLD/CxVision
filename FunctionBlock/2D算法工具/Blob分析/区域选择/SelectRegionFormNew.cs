using FunctionBlock;
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
    public partial class SelectRegionFormNew : Form
    {
        private bool IsLoad = false;
        private IFunction _function;
        public SelectRegionFormNew(IFunction param)
        {
            this._function = param;
            InitializeComponent();
            this.BindProperty();
        }

        private void BindProperty()
        {
            this.MethodCol.Items.Clear();
            this.MethodCol.ValueType = typeof(enSelectRegionMethod);
            foreach (enSelectRegionMethod temp in Enum.GetValues(typeof(enSelectRegionMethod)))
                this.MethodCol.Items.Add(temp);
            //this.FeatureCol.Items.Clear();
            //this.FeatureCol.ValueType = typeof(enSelectShapeFeatures);
            //foreach (enSelectShapeFeatures temp in Enum.GetValues(typeof(enSelectShapeFeatures)))
            //    this.FeatureCol.Items.Add(temp);
            //this.OperaterCol.Items.Clear();
            //this.OperaterCol.ValueType = typeof(enOperation);
            //foreach (enOperation temp in Enum.GetValues(typeof(enOperation)))
            //    this.OperaterCol.Items.Add(temp);
            this.dataGridView1.DataSource = ((Blob)this._function).BlobParam.SelectParam;
            //try
            //{
            //    // 创建匹配参数
            //    foreach (var item in Enum.GetValues(typeof(enSelectRegionMethod)))
            //        this.阈值方法comboBox.Items.Add(item);
            //    //this.阈值方法comboBox.DataBindings.Add(nameof(this.阈值方法comboBox.Text), ((Blob)this._function).BlobParam.SelectParam, "Method", true, DataSourceUpdateMode.OnPropertyChanged);
            //    switch (((Blob)this._function).BlobParam.SelectParam.GetType().GenericTypeArguments[0].Name)
            //    {
            //        case nameof(SelectRegionPointParam):
            //            this.阈值方法comboBox.Text = "select_region_point";
            //            AddForm(this.splitContainer1.Panel2, new SelectRegionPointForm(((Blob)this._function).BlobParam.SelectParam as BindingList<SelectRegionPointParam>));
            //            break;
            //        case nameof(SelectShapeParam):
            //            this.阈值方法comboBox.Text = "select_shape";
            //            AddForm(this.splitContainer1.Panel2, new SelectShapeForm(((Blob)this._function).BlobParam.SelectParam as BindingList<SelectShapeParam>)); //
            //            break;
            //        case nameof(SelectShapeStdParam):;
            //            this.阈值方法comboBox.Text = "select_shape_std";
            //            AddForm(this.splitContainer1.Panel2, new SelectShapeStdForm(((Blob)this._function).BlobParam.SelectParam as BindingList<SelectShapeStdParam>));
            //            break;
            //        default:
            //            this.阈值方法comboBox.Text = "NONE";
            //            break;
            //    }
            //}
            //catch (Exception ee)
            //{
            //    MessageBox.Show(ee.ToString());
            //}
        }

        private void 阈值方法comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.阈值方法comboBox.SelectedItem == null) return;
            //    switch (this.阈值方法comboBox.SelectedItem.ToString())
            //    {
            //        case nameof(enSelectRegionMethod.select_region_point):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SelectParam = new SelectRegionPointParam(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new SelectRegionPointForm(((DoBlob)this._function).BlobParam.SelectParam)) ;
            //            break;
            //        case nameof(enSelectRegionMethod.select_shape):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SelectParam = new SelectShapeParam(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new SelectShapeForm(((DoBlob)this._function).BlobParam.SelectParam)) ;
            //            break;
            //        case nameof(enSelectRegionMethod.select_shape_std):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SelectParam = new SelectShapeStdParam(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new SelectShapeStdForm(((DoBlob)this._function).BlobParam.SelectParam));
            //            break;                 
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }


        private void AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel == null) return;
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            MastPanel.Controls.Add(form);
            form.Show();
        }
        private void AddForm(GroupBox groupBox, Form form)
        {
            if (groupBox == null) return;
            if (groupBox.Controls.Count > 0)
                groupBox.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            groupBox.Controls.Add(form);
            form.Show();
        }


        private void SelectRegionForm_Load(object sender, EventArgs e)
        {
            this.IsLoad = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    BindingList<SelectOperateParam> listParam = ((Blob)this._function).BlobParam.SelectParam;
                    switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "TeachCol":
                            if (this.dataGridView1.Rows[e.RowIndex].DataBoundItem == null)
                            {
                                MessageBox.Show("未设置必需的图形参数，请先设置参数!");
                                return;
                            }
                            switch (listParam[e.RowIndex].Method)
                            {
                                case enSelectRegionMethod.select_region_point:
                                    if (listParam[e.RowIndex].SelectParam == null)
                                        listParam[e.RowIndex].SelectParam = new SelectRegionPointParam();
                                    else
                                    {
                                        //DialogResult dialogResult = MessageBox.Show("确定要覆盖之前的参数吗?", "创建参数", MessageBoxButtons.OKCancel);
                                        //if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
                                        //    listParam[e.RowIndex].SelectParam = new SelectRegionPointParam();
                                    }
                                    AddForm(this.splitContainer1.Panel2, new SelectRegionPointParamForm(listParam[e.RowIndex].SelectParam)); //
                                    break;
                                case enSelectRegionMethod.select_shape:   
                                    if (listParam[e.RowIndex].SelectParam == null)
                                        listParam[e.RowIndex].SelectParam = new SelectShapeParam();
                                    else
                                    {
                                        //DialogResult dialogResult = MessageBox.Show("确定要覆盖之前的参数吗?", "创建参数", MessageBoxButtons.OKCancel);
                                        //if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
                                        //    listParam[e.RowIndex].SelectParam = new SelectShapeParam();
                                    }
                                    AddForm(this.splitContainer1.Panel2, new SelectShapeParamForm(listParam[e.RowIndex].SelectParam));
                                    break;
                                case enSelectRegionMethod.select_shape_std:
                                    if (listParam[e.RowIndex].SelectParam == null)
                                        listParam[e.RowIndex].SelectParam = new SelectShapeStdParam();
                                    else
                                    {
                                        //DialogResult dialogResult = MessageBox.Show("确定要覆盖之前的参数吗?", "创建参数", MessageBoxButtons.OKCancel);
                                        //if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
                                        //    listParam[e.RowIndex].SelectParam = new SelectShapeStdParam();
                                    }
                                    AddForm(this.splitContainer1.Panel2, new SelectShapeStdParamForm(listParam[e.RowIndex].SelectParam));
                                    break;
                                case enSelectRegionMethod.intensity_deviation:
                                    if (listParam[e.RowIndex].SelectParam == null)
                                        listParam[e.RowIndex].SelectParam = new IntensityParam();
                                    else
                                    {
                                        //DialogResult dialogResult = MessageBox.Show("确定要覆盖之前的参数吗?", "创建参数", MessageBoxButtons.OKCancel);
                                        //if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
                                        //    listParam[e.RowIndex].SelectParam = new SelectShapeStdParam();
                                    }
                                    AddForm(this.splitContainer1.Panel2, new IntensityParamForm(listParam[e.RowIndex].SelectParam));
                                    break;
                                case enSelectRegionMethod.select_connect_region:
                                    if (listParam[e.RowIndex].SelectParam == null)
                                        listParam[e.RowIndex].SelectParam = new SelectConnectParam();
                                    else
                                    {
                                        //DialogResult dialogResult = MessageBox.Show("确定要覆盖之前的参数吗?", "创建参数", MessageBoxButtons.OKCancel);
                                        //if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
                                        //    listParam[e.RowIndex].SelectParam = new SelectShapeStdParam();
                                    }
                                    AddForm(this.splitContainer1.Panel2, new ConnectParamForm(listParam[e.RowIndex].SelectParam));
                                    break;
                                default:
                                    throw new NotImplementedException(listParam[e.RowIndex].Method.ToString() + "未实现!");
                            }
                            //////////////////////////////////////////////////////////
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "DeletCol":
                            if (listParam.Count > e.RowIndex)
                                listParam.RemoveAt(e.RowIndex);
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                if (this.dataGridView1.Rows.Count > i)
                                    this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "ParamCol":
                            if (listParam[e.RowIndex].SelectParam == null) return;
                            switch (listParam[e.RowIndex].Method)
                            {
                                case enSelectRegionMethod.select_region_point:
                                    AddForm(this.splitContainer1.Panel2, new SelectRegionPointParamForm(listParam[e.RowIndex].SelectParam)); //
                                    break;
                                case enSelectRegionMethod.select_shape:
                                    AddForm(this.splitContainer1.Panel2, new SelectShapeParamForm(listParam[e.RowIndex].SelectParam));
                                    break;
                                case enSelectRegionMethod.select_shape_std:
                                    AddForm(this.splitContainer1.Panel2, new SelectShapeStdParamForm(listParam[e.RowIndex].SelectParam));
                                    break;
                                case enSelectRegionMethod.intensity_deviation:
                                    AddForm(this.splitContainer1.Panel2, new IntensityParamForm(listParam[e.RowIndex].SelectParam));
                                    break;
                                case enSelectRegionMethod.select_connect_region:
                                    AddForm(this.splitContainer1.Panel2, new ConnectParamForm(listParam[e.RowIndex].SelectParam));
                                    break;
                                default:
                                    throw new NotImplementedException(listParam[e.RowIndex].Method.ToString() + "未实现!");
                            }
                            break;
                        case "UpMoveCol":
                            if (e.RowIndex > 0)
                            {
                                SelectOperateParam up = listParam[e.RowIndex - 1];
                                SelectOperateParam cur = listParam[e.RowIndex];
                                listParam[e.RowIndex - 1] = cur;
                                listParam[e.RowIndex] = up;
                            }
                            break;
                        case "DownMoveCol":
                            if (e.RowIndex < listParam.Count - 1)
                            {
                                SelectOperateParam down = listParam[e.RowIndex + 1];
                                SelectOperateParam cur = listParam[e.RowIndex];
                                listParam[e.RowIndex + 1] = cur;
                                listParam[e.RowIndex] = down;
                            }
                            break;
                    }
                    this.dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                switch (this.dataGridView1.Columns[e.ColumnIndex].DataPropertyName)
                {
                    case "Active":
                        break;
                    case "Method":
                        break;
                    case "RegionParam":
                        /////////////////////////
                        e.Value = EvaluateValue(this.dataGridView1.Rows[e.RowIndex].DataBoundItem, this.dataGridView1.Columns[e.ColumnIndex].DataPropertyName);
                        if (e.Value != null)
                            e.FormattingApplied = true;
                        break;
                }
            }
            catch
            {

            }
        }
        public string EvaluateValue(object obj, string property)
        {
            string prop = property;
            string ret = string.Empty;
            if (obj == null) return ret;
            if (property.Contains("."))
            {
                prop = property.Substring(0, property.IndexOf("."));
                System.Reflection.PropertyInfo[] props = obj.GetType().GetProperties();
                foreach (System.Reflection.PropertyInfo propa in props)
                {
                    object obja = propa.GetValue(obj, new object[] { });
                    if (obja.GetType().Name.Contains(prop))
                    {
                        ret = this.EvaluateValue(obja, property.Substring(property.IndexOf(".") + 1)); // 回调
                        break;
                    }
                }
            }
            else
            {
                System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(prop);
                ret = pi?.GetValue(obj, new object[] { })?.ToString();
            }
            return ret;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }



    }
}
