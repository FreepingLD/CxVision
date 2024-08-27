using Common;
using MotionControlCard;
using System;
using System.Windows.Forms;

namespace MotionControlCard
{

    public partial class MachineParamForm : Form
    {
        private IMotionControl _card = MotionCardManage.CurrentCard;
        public MachineParamForm(IMotionControl card)
        {
            InitializeComponent();
            _card = card;
        }

        public MachineParamForm()
        {
            InitializeComponent();
        }

        private void BindData()
        {
            try
            {
                /// 绑定机台坐标
                if (this._card == null)
                {
                    throw new ArgumentNullException("控制卡对象为空值");
                }
                //this.xAxisResolution.DataBindings.Add("Text", this._card.ParamSetting, "X_encoderResolution", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.yAxisResolution.DataBindings.Add("Text", this._card.ParamSetting, "Y_encoderResolution", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.zAxisResolution.DataBindings.Add("Text", this._card.ParamSetting, "Z_encoderResolution", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.uAxisResolution.DataBindings.Add("Text", this._card.ParamSetting, "U_encoderResolution", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.vAxisResolution.DataBindings.Add("Text", this._card.ParamSetting, "V_encoderResolution", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.wAxisResolution.DataBindings.Add("Text", this._card.ParamSetting, "W_encoderResolution", true, DataSourceUpdateMode.OnPropertyChanged);
                //////////
                //this.启用禁用X轴编码器checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "EnabeDisable_X", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.启用禁用Y轴编码器checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "EnabeDisable_Y", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.启用禁用Z轴编码器checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "EnabeDisable_Z", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.启用禁用U轴编码器checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "EnabeDisable_U", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.启用禁用V轴编码器checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "EnabeDisable_V", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.启用禁用W轴编码器checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "EnabeDisable_W", true, DataSourceUpdateMode.OnPropertyChanged);
                ////  EnableCameraCalibrate EnableMachineCalibrate
                //this.取反X轴checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertAxisFeedBack_x", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反Y轴checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertAxisFeedBack_y", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反Z轴checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertAxisFeedBack_z", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反U轴checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertAxisFeedBack_u", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反V轴checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertAxisFeedBack_v", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反W轴checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertAxisFeedBack_w", true, DataSourceUpdateMode.OnPropertyChanged);
                ///////
                //this.取反X轴指令checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertAxisCommandPos_x", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反Y轴指令checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertAxisCommandPos_y", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反Z轴指令checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertAxisCommandPos_z", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反U轴指令checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertAxisCommandPos_u", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反V轴指令checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertAxisCommandPos_v", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反W轴指令checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertAxisCommandPos_w", true, DataSourceUpdateMode.OnPropertyChanged);
                ///////////////////
                //this.取反X轴Jog方向checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertJogAxisX", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反Y轴JOG方向checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertJogAxisY", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反Z轴Jog方向checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertJogAxisZ", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反U轴Jog方向checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertJogAxisU", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反V轴Jog方向checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertJogAxisV", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反W轴JOG方向checkBox.DataBindings.Add("Checked", this._card.ParamSetting, "InvertJogAxisW", true, DataSourceUpdateMode.OnPropertyChanged);
                ///// 绑定机台坐标
                //this.X_轴传动比textBox.DataBindings.Add("Text", this._card.ParamSetting, "TransmissionRatio_x", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.Y_轴传动比textBox.DataBindings.Add("Text", this._card.ParamSetting, "TransmissionRatio_y", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.Z_轴传动比textBox.DataBindings.Add("Text", this._card.ParamSetting, "TransmissionRatio_z", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.U_轴传动比textBox.DataBindings.Add("Text", this._card.ParamSetting, "TransmissionRatio_u", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.V_轴传动比textBox.DataBindings.Add("Text", this._card.ParamSetting, "TransmissionRatio_v", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.W_轴传动比textBox.DataBindings.Add("Text", this._card.ParamSetting, "TransmissionRatio_w", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void paramConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //if (this._card != null)
                   // this._card.ParamSetting.SaveParamConfig(Application.StartupPath + "\\" + "机台轴参数" + "\\" + this._card.Name + ".txt");
            }
            catch
            {

            }
        }

        private void 控制卡comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.控制卡comboBox.SelectedIndex == -1) return;
                this._card = (IMotionControl)this.控制卡comboBox.SelectedItem;
                MotionCardManage.CurrentCard = this._card;
                BindData();
            }
            catch
            {

            }

        }

        private void MachineParamForm_Load(object sender, EventArgs e)
        {
            this.控制卡comboBox.DataSource = MotionCardManage.CardList;
            this.控制卡comboBox.DisplayMember = "Name";
            this.控制卡comboBox.SelectedItem = MotionCardManage.CurrentCard;
            BindData();
        }


    }
}
