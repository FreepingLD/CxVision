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
    public partial class OutParamForm : Form
    {
        public BindingList<OutParam> _OutParam { get; set; }
        public OutParamForm()
        {
            InitializeComponent();
        }
        public OutParamForm( BindingList<OutParam> outParam)
        {
            InitializeComponent();
            this._OutParam = outParam;
        }
        private void 输出参数dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (输出参数dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "OutDeletCol":
                            if (this._OutParam.Count > e.RowIndex)
                                this._OutParam.RemoveAt(e.RowIndex);
                            //////////////////////////////////////////////////////////////////////
                            for (int i = 0; i < this.输出参数dataGridView.Rows.Count; i++)
                            {
                                if (this.输出参数dataGridView.Rows.Count > i)
                                    this.输出参数dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        default:
                            break;
                    }
                    this.输出参数dataGridView.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
