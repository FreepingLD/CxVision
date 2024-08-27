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
    public partial class RegionMorphologyForm : Form
    {
        private bool IsLoad = false;
        private IFunction _function;
        public RegionMorphologyForm(IFunction param)
        {
            this._function = param;
            InitializeComponent();
            this.BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                foreach (var item in Enum.GetValues(typeof(enRegionMorphology)))
                    this.方法comboBox.Items.Add(item);
                ////////////////////////////////////////////////
                this.方法comboBox.DataBindings.Add(nameof(this.方法comboBox.Text), ((Blob)this._function).BlobParam.MorphologyParam, "Method", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 阈值方法comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.方法comboBox.SelectedItem == null) return;
            //    switch (this.方法comboBox.SelectedItem.ToString())
            //    {
            //        case nameof(enMorphology.dilation_circle):
            //        case nameof(enMorphology.erosion_circle):
            //        case nameof(enMorphology.closing_circle):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.MorphologyParam = new CircleParam();
            //            AddForm(this.splitContainer1.Panel2, new CircleMorphologyForm(((DoBlob)this._function).BlobParam.MorphologyParam));
            //            break;
            //        case nameof(enMorphology.closing_rectangle1):
            //        case nameof(enMorphology.dilation_rectangle1):
            //        case nameof(enMorphology.erosion_rectangle1):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.MorphologyParam = new RectParam();
            //            AddForm(this.splitContainer1.Panel2, new Rectangle1MorphologyForm(((DoBlob)this._function).BlobParam.MorphologyParam));
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

        private void 方法comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.方法comboBox.SelectedIndex == -1) return;
                switch (this.方法comboBox.SelectedItem.ToString())
                {
                    case nameof(enRegionMorphology.dilation_circle):
                    case nameof(enRegionMorphology.erosion_circle):
                    case nameof(enRegionMorphology.closing_circle):
                    case nameof(enRegionMorphology.opening_circle):
                        //if (this.IsLoad)
                        //    ((Blob)this._function).BlobParam.MorphologyParam = new RegionCircleParam(this.方法comboBox.SelectedItem.ToString());
                        //AddForm(this.splitContainer1.Panel2, new CircleMorphologyForm(((Blob)this._function).BlobParam.MorphologyParam)); //
                        break;
                    case nameof(enRegionMorphology.closing_rectangle1):
                    case nameof(enRegionMorphology.dilation_rectangle1):
                    case nameof(enRegionMorphology.erosion_rectangle1):
                    case nameof(enRegionMorphology.opening_rectangle1):
                        //if (this.IsLoad)
                        //    ((Blob)this._function).BlobParam.MorphologyParam = new RegionRectParam(this.方法comboBox.SelectedItem.ToString());
                        //AddForm(this.splitContainer1.Panel2, new Rectangle1MorphologyForm(((Blob)this._function).BlobParam.MorphologyParam));
                        break;
                    default:
                        AddForm(this.splitContainer1.Panel2, new Form());
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RegionMorphologyForm_Load(object sender, EventArgs e)
        {
            this.IsLoad = true;
        }
    }
}
