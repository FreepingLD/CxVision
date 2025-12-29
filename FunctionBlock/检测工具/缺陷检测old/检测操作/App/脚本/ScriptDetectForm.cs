using Common;
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
    public partial class ScriptDetectForm : Form
    {
        private ThresholParam _param;
        public ScriptDetectForm(ThresholParam param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.脚本类型comboBox.DataSource = Enum.GetValues(typeof(enScriptType));
                this.脚本类型comboBox.DataBindings.Add(nameof(this.脚本类型comboBox.SelectedItem), ((ScriptInspParam)this._param), "ScriptType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.脚本路径textBox.DataBindings.Add("Text", ((ScriptInspParam)this._param), "ScriptPath", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), (ScriptInspParam)this._param, "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 读取Btn_Click(object sender, EventArgs e)
        {
            FileOperate fo = new FileOperate();
            string path = fo.OpenImage();
            try
            {
                if (path != null && path.Trim().Length > 0)
                {
                    ((ScriptInspParam)this._param).ScriptPath = path;
                    this.脚本路径textBox.Text = path;
                }
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 输入参数设置Btn_Click(object sender, EventArgs e)
        {
            try
            {
                new InParamForm(((ScriptInspParam)this._param).InParam).Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 输出参数设置Btn_Click(object sender, EventArgs e)
        {
            try
            {
                new OutParamForm(((ScriptInspParam)this._param).OutParam).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
