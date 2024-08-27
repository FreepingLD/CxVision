using Common;
using FunctionBlock;
using Light;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class LightSetForm : Form
    {
        private AcqSource acqSource;
        public LightSetForm()
        {
            InitializeComponent();
        }
        public LightSetForm(AcqSource acqSource)
        {
            InitializeComponent();
            this.Text = acqSource.Name;
            this.acqSource = acqSource;
        }
        private void LightConfigForm_Load(object sender, EventArgs e)
        {
            this.LightControlColumn.Items.Clear();
            this.LightControlColumn.ValueType = typeof(string);
            foreach (string temp in LightConnectManage.GetLightName())
                this.LightControlColumn.Items.Add(temp.ToString());
            this.LightChennelColumn.Items.Clear();
            this.LightChennelColumn.ValueType = typeof(enLightChannel);
            foreach (enLightChannel temp in Enum.GetValues(typeof(enLightChannel)))
                this.LightChennelColumn.Items.Add(temp);
            this.光源dataGridView1.DataSource = this.acqSource?.LightParamList;
            ///////////////////////////////////////
        }

        private void LightConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

            }
            catch
            {

            }
        }

        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                AcqSourceManage.Instance.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 光源dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 光源dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (光源dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "SetLightBtn":
                            if (this.acqSource == null) return;
                            if (acqSource.LightParamList.Count > e.RowIndex)
                            {
                                acqSource.LightParamList[e.RowIndex].SetLight();
                            }
                            break;
                        case "DeleteBtn":
                            if (this.acqSource == null) return;
                            if (acqSource.LightParamList.Count > e.RowIndex)
                            {
                                this.acqSource.LightParamList.RemoveAt(e.RowIndex);
                            }
                            break;
                        case "OpenBtn":
                            if (this.acqSource == null) return;
                            if (acqSource.LightParamList.Count > e.RowIndex)
                            {
                                acqSource.LightParamList[e.RowIndex].Open();
                            }
                            break;
                    }
                    this.光源dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置光源参数时报错：" + ex.ToString());
            }
        }


    }
}
