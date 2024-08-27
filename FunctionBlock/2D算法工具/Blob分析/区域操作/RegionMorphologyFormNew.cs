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
    public partial class RegionMorphologyFormNew : Form
    {
        private bool IsLoad = false;
        private IFunction _function;
        BindingList<RegionOperateParam> _paramList;
        public RegionMorphologyFormNew(IFunction param)
        {
            this._function = param;
            this._paramList = ((Blob)this._function).BlobParam.MorphologyParam;
            InitializeComponent();
            this.BindProperty(); // BindingList<RegionOperateParam>
        }
        public RegionMorphologyFormNew(BindingList<RegionOperateParam> param)
        {
            this._paramList = param;
            InitializeComponent();
            this.BindProperty(); // 
        }
        private void BindProperty()
        {
            try
            {
                this.MethodCol.Items.Clear();
                this.MethodCol.ValueType = typeof(enRegionMorphology);
                foreach (enRegionMorphology temp in Enum.GetValues(typeof(enRegionMorphology)))
                    this.MethodCol.Items.Add(temp);
                this.dataGridView1.DataSource = this._paramList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
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
        private void AddForm(Splitter MastPanel, Form form)
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

        private void RegionMorphologyFormNew_Load(object sender, EventArgs e)
        {
            this.IsLoad = true;
            //AddForm(this.splitContainer1.Panel2, new CircleMorphologyForm(new RegionCircleParam()));
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    BindingList<RegionOperateParam> listParam = ((Blob)this._function).BlobParam.MorphologyParam;
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
                                case enRegionMorphology.closing_circle:
                                case enRegionMorphology.dilation_circle:
                                case enRegionMorphology.erosion_circle:
                                case enRegionMorphology.opening_circle:
                                    if (listParam[e.RowIndex].RegionParam == null)
                                        listParam[e.RowIndex].RegionParam = new RegionCircleParam();
                                    else
                                    {
                                        //DialogResult dialogResult = MessageBox.Show("确定要覆盖之前的参数吗?", "创建参数", MessageBoxButtons.OKCancel);
                                        //if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
                                        //    listParam[e.RowIndex].RegionParam = new RegionCircleParam();
                                    }
                                    AddForm(this.splitContainer1.Panel2, new CircleMorphologyForm((RegionCircleParam)listParam[e.RowIndex].RegionParam)); //
                                    break;
                                case enRegionMorphology.closing_rectangle1:
                                case enRegionMorphology.dilation_rectangle1:
                                case enRegionMorphology.erosion_rectangle1:
                                case enRegionMorphology.opening_rectangle1:
                                    if (listParam[e.RowIndex].RegionParam == null)
                                        listParam[e.RowIndex].RegionParam = new RegionRectParam();
                                    else
                                    {
                                        //DialogResult dialogResult = MessageBox.Show("确定要覆盖之前的参数吗?", "创建参数", MessageBoxButtons.OKCancel);
                                        //if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
                                        //    listParam[e.RowIndex].RegionParam = new RegionRectParam();
                                    }
                                    AddForm(this.splitContainer1.Panel2, new Rectangle1MorphologyForm(listParam[e.RowIndex].RegionParam));
                                    break;
                                case enRegionMorphology.shape_trans:
                                    if (listParam[e.RowIndex].RegionParam == null)
                                        listParam[e.RowIndex].RegionParam = new ShapeTransParam();
                                    else
                                    {
                                        //DialogResult dialogResult = MessageBox.Show("确定要覆盖之前的参数吗?", "创建参数", MessageBoxButtons.OKCancel);
                                        //if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
                                        //    listParam[e.RowIndex].RegionParam = new ShapeTransParam();
                                    }
                                    AddForm(this.splitContainer1.Panel2, new ShapeTransForm(listParam[e.RowIndex].RegionParam));
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
                            if (listParam[e.RowIndex].RegionParam == null) return;
                            switch (listParam[e.RowIndex].Method)
                            {
                                case enRegionMorphology.closing_circle:
                                case enRegionMorphology.dilation_circle:
                                case enRegionMorphology.erosion_circle:
                                case enRegionMorphology.opening_circle:
                                    AddForm(this.splitContainer1.Panel2, new CircleMorphologyForm((RegionCircleParam)listParam[e.RowIndex].RegionParam)); //
                                    break;
                                case enRegionMorphology.closing_rectangle1:
                                case enRegionMorphology.dilation_rectangle1:
                                case enRegionMorphology.erosion_rectangle1:
                                case enRegionMorphology.opening_rectangle1:
                                    AddForm(this.splitContainer1.Panel2, new Rectangle1MorphologyForm((RegionRectParam)listParam[e.RowIndex].RegionParam));
                                    break;
                                case enRegionMorphology.shape_trans:
                                    AddForm(this.splitContainer1.Panel2, new ShapeTransForm((ShapeTransParam)listParam[e.RowIndex].RegionParam));
                                    break;
                                default:
                                    throw new NotImplementedException(listParam[e.RowIndex].Method.ToString() + "未实现!");
                            }
                            break;
                        case "UpMoveCol":
                            if (e.RowIndex > 0)
                            {
                                RegionOperateParam up = listParam[e.RowIndex - 1];
                                RegionOperateParam cur = listParam[e.RowIndex];
                                listParam[e.RowIndex - 1] = cur;
                                listParam[e.RowIndex] = up;
                            }
                            break;
                        case "DownMoveCol":
                            if (e.RowIndex < listParam.Count - 1)
                            {
                                RegionOperateParam down = listParam[e.RowIndex + 1];
                                RegionOperateParam cur = listParam[e.RowIndex];
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
                        //if (((Blob)this._function).BlobParam.MorphologyParam[e.RowIndex] == null) return;
                        //switch (((Blob)this._function).BlobParam.MorphologyParam[e.RowIndex].Method)
                        //{
                        //    case enRegionMorphology.closing_circle:
                        //    case enRegionMorphology.dilation_circle:
                        //    case enRegionMorphology.erosion_circle:
                        //    case enRegionMorphology.opening_circle:
                        //        ((Blob)this._function).BlobParam.MorphologyParam[e.RowIndex].RegionParam = new RegionCircleParam();
                        //        break;
                        //    case enRegionMorphology.closing_rectangle1:
                        //    case enRegionMorphology.dilation_rectangle1:
                        //    case enRegionMorphology.erosion_rectangle1:
                        //    case enRegionMorphology.opening_rectangle1:
                        //        ((Blob)this._function).BlobParam.MorphologyParam[e.RowIndex].RegionParam = new RegionRectParam();
                        //        break;
                        //    default:
                        //        throw new NotImplementedException(((Blob)this._function).BlobParam.MorphologyParam[e.RowIndex].Method.ToString() + "未实现!");
                        //}
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
