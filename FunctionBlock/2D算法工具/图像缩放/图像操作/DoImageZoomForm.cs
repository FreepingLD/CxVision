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
    public partial class DoImageZoomForm : Form
    {
        private bool IsLoad = false;
        private IFunction _function;
        public DoImageZoomForm(IFunction param)
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
                this.方法comboBox.Items.Clear();
                foreach (var item in Enum.GetValues(typeof(enImageZoomMethod)))
                    this.方法comboBox.Items.Add(item);
                //this.方法comboBox.DataSource = Enum.GetNames(typeof(enImageEnhancementMethod));
                ////////////////////////////////////////////////
                this.方法comboBox.DataBindings.Add(nameof(this.方法comboBox.Text), ((ImageZoom)this._function).ZoomOperator.ZoomParam, "Method", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 方法comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

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

        private void 方法comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.方法comboBox.SelectedIndex == -1) return;
                switch (this.方法comboBox.SelectedItem.ToString())
                {
                    case nameof(enImageZoomMethod.zoom_image_factor):
                        if (this.IsLoad)
                            ((ImageZoom)this._function).ZoomOperator.ZoomParam = new ZoomImageFactorParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new ZoomImageFactorForm(((ImageZoom)this._function).ZoomOperator.ZoomParam));
                        break;
                    case nameof(enImageZoomMethod.zoom_image_size):
                        if (this.IsLoad)
                            ((ImageZoom)this._function).ZoomOperator.ZoomParam = new ZoomImageSizeParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new ZoomImageSizeForm(((ImageZoom)this._function).ZoomOperator.ZoomParam));
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DoImageMorphologyForm_Load(object sender, EventArgs e)
        {
            this.IsLoad = true;
        }
    }
}
