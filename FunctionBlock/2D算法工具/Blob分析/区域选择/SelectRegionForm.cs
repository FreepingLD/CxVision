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
    public partial class SelectRegionForm : Form
    {
        private bool IsLoad = false;
        private IFunction _function;
        public SelectRegionForm(IFunction param)
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
                foreach (var item in Enum.GetValues(typeof(enSelectRegionMethod)))
                    this.阈值方法comboBox.Items.Add(item);
                //this.阈值方法comboBox.DataBindings.Add(nameof(this.阈值方法comboBox.Text), ((Blob)this._function).BlobParam.SelectParam, "Method", true, DataSourceUpdateMode.OnPropertyChanged);
                //switch (((Blob)this._function).BlobParam.SelectParam.GetType().GenericTypeArguments[0].Name)
                //{
                //    case nameof(SelectRegionPointParam):
                //        this.阈值方法comboBox.Text = "select_region_point";
                //        AddForm(this.splitContainer1.Panel2, new SelectRegionPointForm(((Blob)this._function).BlobParam.SelectParam as BindingList<SelectRegionPointParam>));
                //        break;
                //    case nameof(SelectShapeParam):
                //        this.阈值方法comboBox.Text = "select_shape";
                //        AddForm(this.splitContainer1.Panel2, new SelectShapeForm(((Blob)this._function).BlobParam.SelectParam as BindingList<SelectShapeParam>)); //
                //        break;
                //    case nameof(SelectShapeStdParam):;
                //        this.阈值方法comboBox.Text = "select_shape_std";
                //        AddForm(this.splitContainer1.Panel2, new SelectShapeStdForm(((Blob)this._function).BlobParam.SelectParam as BindingList<SelectShapeStdParam>));
                //        break;
                //    default:
                //        this.阈值方法comboBox.Text = "NONE";
                //        break;
                //}
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

        private void 阈值方法comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.阈值方法comboBox.SelectedIndex == -1) return;
                //switch (this.阈值方法comboBox.SelectedItem.ToString())
                //{
                //    case nameof(enSelectRegionMethod.select_region_point):
                //        if (((Blob)this._function).BlobParam.SelectParam.GetType().GenericTypeArguments[0].Name != nameof(SelectRegionPointParam))
                //        {
                //            if (this.IsLoad)
                //                ((Blob)this._function).BlobParam.SelectParam = new BindingList<SelectRegionPointParam>();// new SelectRegionPointParam(this.阈值方法comboBox.SelectedItem.ToString());
                //        }
                //        AddForm(this.splitContainer1.Panel2, new SelectRegionPointForm(((Blob)this._function).BlobParam.SelectParam as BindingList<SelectRegionPointParam>));
                //        break;
                //    case nameof(enSelectRegionMethod.select_shape):
                //        if (((Blob)this._function).BlobParam.SelectParam.GetType().GenericTypeArguments[0].Name != nameof(SelectShapeParam))
                //        {
                //            if (this.IsLoad)
                //                ((Blob)this._function).BlobParam.SelectParam = new BindingList<SelectShapeParam>(); //new SelectShapeParam(this.阈值方法comboBox.SelectedItem.ToString());
                //        }
                //        AddForm(this.splitContainer1.Panel2, new SelectShapeForm(((Blob)this._function).BlobParam.SelectParam as BindingList<SelectShapeParam>)); //
                //        break;
                //    case nameof(enSelectRegionMethod.select_shape_std):
                //        if (((Blob)this._function).BlobParam.SelectParam.GetType().GenericTypeArguments[0].Name != nameof(SelectShapeStdParam))
                //        {
                //            if (this.IsLoad)
                //                ((Blob)this._function).BlobParam.SelectParam = new BindingList<SelectShapeStdParam>(); // new SelectShapeStdParam(this.阈值方法comboBox.SelectedItem.ToString());
                //        }
                //        AddForm(this.splitContainer1.Panel2, new SelectShapeStdForm(((Blob)this._function).BlobParam.SelectParam as BindingList<SelectShapeStdParam>));
                //        break;
                //    default:
                //        AddForm(this.splitContainer1.Panel2, new Form());
                //        break;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SelectRegionForm_Load(object sender, EventArgs e)
        {
            this.IsLoad = true;
        }
    }
}
