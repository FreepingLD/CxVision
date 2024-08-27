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
    public partial class SortRegionForm : Form
    {
        private IFunction _function;
        public SortRegionForm(IFunction param)
        {
            this._function = param;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.排序模式ComboBox.DataSource = Enum.GetValues(typeof(enSortMode));
                this.排序模式ComboBox.DataBindings.Add(nameof(this.排序模式ComboBox.SelectedItem), ((Blob)this._function).BlobParam.SortParam, "SortMode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.排序顺序comboBox.DataBindings.Add(nameof(this.排序顺序comboBox.SelectedItem), ((Blob)this._function).BlobParam.SortParam, "Order", true, DataSourceUpdateMode.OnPropertyChanged);
                this.排序对象comboBox.DataBindings.Add(nameof(this.排序对象comboBox.SelectedItem), ((Blob)this._function).BlobParam.SortParam, "RowOrCol", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 排序模式ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.排序模式ComboBox.SelectedItem == null) return;
                if(this.排序模式ComboBox.SelectedItem.ToString() == "none")
                {
                    this.排序顺序comboBox.Enabled = false;
                    this.排序对象comboBox.Enabled = false;
                }
                else
                {
                    this.排序顺序comboBox.Enabled = true;
                    this.排序对象comboBox.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
