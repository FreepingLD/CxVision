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
    public partial class InParamForm : Form
    {
        public BindingList<InParam> _InParam { get; set; }

        public InParamForm()
        {
            InitializeComponent();
        }
        public InParamForm(BindingList<InParam> inParam)
        {
            InitializeComponent();
            this._InParam = inParam;
        }
        private void 输入参数dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (输入参数dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "DeletCol":
                            if (this._InParam.Count > e.RowIndex)
                                this._InParam.RemoveAt(e.RowIndex);
                            ///////////////////////////////////////////////////////////
                            for (int i = 0; i < this.输入参数dataGridView.Rows.Count; i++)
                            {
                                if (this.输入参数dataGridView.Rows.Count > i)
                                    this.输入参数dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        default:

                            break;
                    }
                    this.输入参数dataGridView.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



    }
}
