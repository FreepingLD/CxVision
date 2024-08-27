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
    public partial class RegionSementForm : Form
    {
        private bool IsLoad = false;
        private IFunction _function;
        public RegionSementForm(IFunction param)
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
                foreach (var item in Enum.GetValues(typeof(enRegionSegmentMethod)))
                    this.阈值方法comboBox.Items.Add(item);
                this.模式comboBox.Items.Clear();
                this.模式comboBox.Items.Add("单个分割");
                this.模式comboBox.Items.Add("合并分割");
                ////////////////////////////////////////////////
                this.阈值方法comboBox.DataBindings.Add(nameof(this.阈值方法comboBox.Text), ((Blob)this._function).BlobParam.SegParam, "SegmentMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                this.模式comboBox.DataBindings.Add(nameof(this.模式comboBox.Text), ((Blob)this._function).BlobParam.SegParam, "SegmentMode", true, DataSourceUpdateMode.OnPropertyChanged);
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
            //    if (this.阈值方法comboBox.SelectedItem == null) return;
            //    switch (this.阈值方法comboBox.SelectedItem.ToString())
            //    {
            //        case nameof(enRegionSegmentMethod.Threshold):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SegParam = new ThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new ThresholdForm(((DoBlob)this._function).BlobParam.SegParam));
            //            break;
            //        case nameof(enRegionSegmentMethod.AutoThreshold):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SegParam = new AutoThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new AutoThresholdForm(((DoBlob)this._function).BlobParam.SegParam));
            //            break;
            //        case nameof(enRegionSegmentMethod.BinaryThreshold):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SegParam = new BinaryThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new BinaryThresholdForm(((DoBlob)this._function).BlobParam.SegParam));
            //            break;
            //        case nameof(enRegionSegmentMethod.CharThreshold):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SegParam = new CharThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new CharThresholdForm(((DoBlob)this._function).BlobParam.SegParam));
            //            break;
            //        case nameof(enRegionSegmentMethod.DualThreshold):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SegParam = new DualThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new DualThresholdForm(((DoBlob)this._function).BlobParam.SegParam));
            //            break;
            //        case nameof(enRegionSegmentMethod.DynThreshold):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SegParam = new DynThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new DynThresholdForm(((DoBlob)this._function).BlobParam.SegParam));
            //            break;
            //        case nameof(enRegionSegmentMethod.FastThreshold):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SegParam = new FastThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new FastThresholdForm(((DoBlob)this._function).BlobParam.SegParam));
            //            break;
            //        case nameof(enRegionSegmentMethod.HysteresisThreshold):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SegParam = new HysteresisThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new HysteresisThresholdForm(((DoBlob)this._function).BlobParam.SegParam));
            //            break;
            //        case nameof(enRegionSegmentMethod.LocalThreshold):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SegParam = new LocalThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new LocalThresholdForm(((DoBlob)this._function).BlobParam.SegParam));
            //            break;
            //        case nameof(enRegionSegmentMethod.VarThreshold):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SegParam = new VarThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new VarThresholdForm(((DoBlob)this._function).BlobParam.SegParam));
            //            break;
            //        case nameof(enRegionSegmentMethod.WatershedsThreshold):
            //            if (this.IsLoad)
            //                ((DoBlob)this._function).BlobParam.SegParam = new WatershedsThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
            //            AddForm(this.splitContainer1.Panel2, new WatershedsThresholdForm(((DoBlob)this._function).BlobParam.SegParam));
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

        private void 阈值方法comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.阈值方法comboBox.SelectedIndex == -1) return;
                switch (this.阈值方法comboBox.SelectedItem.ToString())
                {
                    case nameof(enRegionSegmentMethod.Threshold):
                        if (this.IsLoad)
                            ((Blob)this._function).BlobParam.SegParam = new ThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new ThresholdForm(((Blob)this._function).BlobParam.SegParam));
                        break;
                    case nameof(enRegionSegmentMethod.AutoThreshold):
                        if (this.IsLoad)
                            ((Blob)this._function).BlobParam.SegParam = new AutoThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new AutoThresholdForm(((Blob)this._function).BlobParam.SegParam));
                        break;
                    case nameof(enRegionSegmentMethod.BinaryThreshold):
                        if (this.IsLoad)
                            ((Blob)this._function).BlobParam.SegParam = new BinaryThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new BinaryThresholdForm(((Blob)this._function).BlobParam.SegParam));
                        break;
                    case nameof(enRegionSegmentMethod.CharThreshold):
                        if (this.IsLoad)
                            ((Blob)this._function).BlobParam.SegParam = new CharThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new CharThresholdForm(((Blob)this._function).BlobParam.SegParam));
                        break;
                    case nameof(enRegionSegmentMethod.DualThreshold):
                        if (this.IsLoad)
                            ((Blob)this._function).BlobParam.SegParam = new DualThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new DualThresholdForm(((Blob)this._function).BlobParam.SegParam));
                        break;
                    case nameof(enRegionSegmentMethod.DynThreshold):
                        if (this.IsLoad)
                            ((Blob)this._function).BlobParam.SegParam = new DynThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new DynThresholdForm(((Blob)this._function).BlobParam.SegParam));
                        break;
                    case nameof(enRegionSegmentMethod.FastThreshold):
                        if (this.IsLoad)
                            ((Blob)this._function).BlobParam.SegParam = new FastThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new FastThresholdForm(((Blob)this._function).BlobParam.SegParam));
                        break;
                    case nameof(enRegionSegmentMethod.HysteresisThreshold):
                        if (this.IsLoad)
                            ((Blob)this._function).BlobParam.SegParam = new HysteresisThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new HysteresisThresholdForm(((Blob)this._function).BlobParam.SegParam));
                        break;
                    case nameof(enRegionSegmentMethod.LocalThreshold):
                        if (this.IsLoad)
                            ((Blob)this._function).BlobParam.SegParam = new LocalThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new LocalThresholdForm(((Blob)this._function).BlobParam.SegParam));
                        break;
                    case nameof(enRegionSegmentMethod.VarThreshold):
                        if (this.IsLoad)
                            ((Blob)this._function).BlobParam.SegParam = new VarThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new VarThresholdForm(((Blob)this._function).BlobParam.SegParam));
                        break;
                    case nameof(enRegionSegmentMethod.WatershedsThreshold):
                        if (this.IsLoad)
                            ((Blob)this._function).BlobParam.SegParam = new WatershedsThresholdBlob(this.阈值方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new WatershedsThresholdForm(((Blob)this._function).BlobParam.SegParam));
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RegionSementForm_Load(object sender, EventArgs e)
        {
            this.IsLoad = true;
        }
    }
}
