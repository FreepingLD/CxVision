using Common;
using System;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class ParamConfigForm : Form
    {
        FileOperate fo = new FileOperate();
        public ParamConfigForm()
        {
            InitializeComponent();
            //if (fo.ReadConfigParam(Application.StartupPath + "\\" + "ParamConfig.txt") != null)
            //    //GlobalVariable.pConfig = (Common.ParamConfig)fo.ReadConfigParam(Application.StartupPath + "\\" + "ParamConfig.txt");
            //    GlobalVariable.pConfig = Common.ParamConfig.ReadParamConfig(Application.StartupPath + "\\" + "ParamConfig.txt");
            //else
            //    GlobalVariable.pConfig = new ParamConfig();
            //GlobalVariable.pConfig = new Common.ParamConfig().ReadParamConfig(Application.StartupPath + "\\" + "ParamConfig.txt"); // 在打开软件的时候加载了，这里不需要了
            BindData();
        }



        private void BindData()
        {
            try
            {
                this.视图类型comboBox.DataSource= Enum.GetNames(typeof(Common.enViewType));
                this.强度属性comboBox.DataSource = Enum.GetNames(typeof(Common.enViewIntensity));
                this.点云质量comboBox.DataSource = Enum.GetNames(typeof(Common.enViewQuality));
                this.测量环境配置comboBox.DataSource = Enum.GetNames(typeof(Common.enMeasureEnvironmentConfig));
                /// 绑定机台坐标
                this.xAxisResolution.DataBindings.Add("Text", GlobalVariable.pConfig, "X_encoderResolution", true, DataSourceUpdateMode.OnPropertyChanged);
                this.yAxisResolution.DataBindings.Add("Text", GlobalVariable.pConfig, "Y_encoderResolution", true, DataSourceUpdateMode.OnPropertyChanged);
                this.zAxisResolution.DataBindings.Add("Text", GlobalVariable.pConfig, "Z_encoderResolution", true, DataSourceUpdateMode.OnPropertyChanged);
                this.uAxisResolution.DataBindings.Add("Text", GlobalVariable.pConfig, "U_encoderResolution", true, DataSourceUpdateMode.OnPropertyChanged);
                this.vAxisResolution.DataBindings.Add("Text", GlobalVariable.pConfig, "V_encoderResolution", true, DataSourceUpdateMode.OnPropertyChanged);
                this.wAxisResolution.DataBindings.Add("Text", GlobalVariable.pConfig, "W_encoderResolution", true, DataSourceUpdateMode.OnPropertyChanged);
                this.ScanSpeed.DataBindings.Add("Text", GlobalVariable.pConfig, "ScanSpeed", true, DataSourceUpdateMode.OnPropertyChanged);
                this.移动textBox.DataBindings.Add("Text", GlobalVariable.pConfig, "MoveSpeed", true, DataSourceUpdateMode.OnPropertyChanged);
                this.坐标显示checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, "IsShowCoordSys", true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像列宽缩放textBox.DataBindings.Add("Text", GlobalVariable.pConfig, "ImageWidthScale", true, DataSourceUpdateMode.OnPropertyChanged);
                this.视图类型comboBox.DataBindings.Add("Text", GlobalVariable.pConfig, "ViewType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.保留点云深度checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, "Depth_persistence", true, DataSourceUpdateMode.OnPropertyChanged);
                this.强度属性comboBox.DataBindings.Add("Text", GlobalVariable.pConfig, "Intensity", true, DataSourceUpdateMode.OnPropertyChanged);
                this.点云质量comboBox.DataBindings.Add("Text", GlobalVariable.pConfig, "PointQuality", true, DataSourceUpdateMode.OnPropertyChanged);
                /////////////////////////////
                this.测量环境配置comboBox.DataBindings.Add("Text", GlobalVariable.pConfig, "MeasureEnvironment", true, DataSourceUpdateMode.OnPropertyChanged);
                this.点间隔textBox.DataBindings.Add("Text", GlobalVariable.pConfig, "Stil_PointPitch", true, DataSourceUpdateMode.OnPropertyChanged);
                ///////////////////////////////////////////////////////////////
                this.启用禁用X轴编码器checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, "EnabeDisable_X", true, DataSourceUpdateMode.OnPropertyChanged); //Stil_PointPitch
                this.启用禁用Y轴编码器checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, "EnabeDisable_Y", true, DataSourceUpdateMode.OnPropertyChanged); //Stil_PointPitch
                this.启用禁用Z轴编码器checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, "EnabeDisable_Z", true, DataSourceUpdateMode.OnPropertyChanged); //Stil_PointPitch
                this.启用禁用U轴编码器checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, "EnabeDisable_U", true, DataSourceUpdateMode.OnPropertyChanged); //Stil_PointPitch
                this.启用禁用V轴编码器checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, "EnabeDisable_V", true, DataSourceUpdateMode.OnPropertyChanged); //Stil_PointPitch
                this.启用禁用W轴编码器checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, "EnabeDisable_W", true, DataSourceUpdateMode.OnPropertyChanged); //Stil_PointPitch
                //
                this.箭头长度numericUpDown.DataBindings.Add("Text", GlobalVariable.pConfig, "ArrowLength", true, DataSourceUpdateMode.OnPropertyChanged); //
                this.节点尺寸numericUpDown.DataBindings.Add("Text", GlobalVariable.pConfig, "NodeSize", true, DataSourceUpdateMode.OnPropertyChanged); //arrowLength
                this.点尺寸numericUpDown.DataBindings.Add("Text", GlobalVariable.pConfig, "PointSize", true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像刷新时间numericUpDown.DataBindings.Add("Text", GlobalVariable.pConfig, "ImageUpdataTime", true, DataSourceUpdateMode.OnPropertyChanged);
                this.OKNG字体大小textBox.DataBindings.Add("Text", GlobalVariable.pConfig, "OKNgSize", true, DataSourceUpdateMode.OnPropertyChanged);
                this.OKNG字体位置comboBox.DataBindings.Add("Text", GlobalVariable.pConfig, "OKNgPosition", true, DataSourceUpdateMode.OnPropertyChanged);
                this.OKNG行偏移textBox.DataBindings.Add("Text", GlobalVariable.pConfig, "OKNgRowOffset", true, DataSourceUpdateMode.OnPropertyChanged);
                this.OKNG列偏移textBox.DataBindings.Add("Text", GlobalVariable.pConfig, "OKNgColOffset", true, DataSourceUpdateMode.OnPropertyChanged);
                // 数据保存
                this.数据保存目标comboBox.DataBindings.Add("Text", GlobalVariable.pConfig, "DataSaveTarget", true, DataSourceUpdateMode.OnPropertyChanged);
                this.数据保存间隔numericUpDown.DataBindings.Add("Text", GlobalVariable.pConfig, "DataSaveGap", true, DataSourceUpdateMode.OnPropertyChanged);
                this.数据刷新numericUpDown.DataBindings.Add("Text", GlobalVariable.pConfig, "DataUpdataGap", true, DataSourceUpdateMode.OnPropertyChanged);
                this.数据输出绑定comboBox.DataBindings.Add("Text", GlobalVariable.pConfig, "DataOutputBinding", true, DataSourceUpdateMode.OnPropertyChanged);
                //  EnableCameraCalibrate EnableMachineCalibrate
                this.启用机台校准checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, "EnableMachineCalibrate", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用相机校准checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, "EnableCameraCalibrate", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch
            {
               // throw new Exception();
            }
        }

        private void paramConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //fo.SaveConfigParam("ParamConfig.txt", GlobalVariable.pConfig);
            GlobalVariable.pConfig.SaveParamConfig("ParamConfig.txt");
        }


    }
}
