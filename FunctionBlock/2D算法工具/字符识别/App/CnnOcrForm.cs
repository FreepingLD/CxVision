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
    public partial class CnnOcrForm : Form
    {
       private OcrCnnParam _param;
        public CnnOcrForm(OcrCnnParam param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
        }


        private void BindProperty()
        {
            try
            {
                this.识别方法comboBox.DataSource = Enum.GetNames(typeof(enOcrMethod));
                this.字体名称comboBox.DataBindings.Add("Text", this._param, "OcrFontName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.识别方法comboBox.DataBindings.Add("Text", this._param, "OcrMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                this.类数量comboBox.DataBindings.Add("Text", this._param, "NumClasses", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.期望类别comboBox.DataBindings.Add("Text", this._param, "Expression", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.替代数量comboBox.DataBindings.Add("Text", this._param, "NumAlternatives", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.校正数量comboBox.DataBindings.Add("Text", this._param, "NumCorrections", true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反图像checkBox.DataBindings.Add(nameof(this.取反图像checkBox.Checked), this._param, "InvertImage", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void CnnOcrForm_Load(object sender, EventArgs e)
        {

        }

        private void 字符提取button_Click(object sender, EventArgs e)
        {
            try
            {
                new ExtractChartForm(this._param?.ExtractParam).Show();
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }



    }
}
