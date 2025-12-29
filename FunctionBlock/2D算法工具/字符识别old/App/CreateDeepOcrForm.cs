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
    public partial class CreateDeepOcrForm : Form
    {
       private DeepOcrCreateParam _deepParam;
        private DeepOcrRecognitionParam _recParam;
        public CreateDeepOcrForm(DeepOcrCreateParam param, DeepOcrRecognitionParam recParam)
        {
            this._deepParam = param;
            this._recParam = recParam;
            InitializeComponent();
            BindProperty();
        }


        private void BindProperty()
        {
            try
            {
                this.模型comboBox.DataSource = Enum.GetNames(typeof(enDeepOcrMode));
                this.模型comboBox.DataBindings.Add("Text", this._deepParam, "DeepOcrMode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.字符区域宽度comboBox.DataBindings.Add("Text", this._recParam, "RecognitionChartWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.字符区域高度comboBox.DataBindings.Add("Text", this._recParam, "RecognitionChartHeight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反图像checkBox.DataBindings.Add(nameof(this.取反图像checkBox.Checked), this._recParam, "InvertImage", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void CreateDeepOcrForm_Load(object sender, EventArgs e)
        {

        }

    }
}
