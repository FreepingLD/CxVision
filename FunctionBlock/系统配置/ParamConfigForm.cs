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
                this.颜色属性comboBox.DataSource = Enum.GetNames(typeof(Common.enViewIntensity));
                this.点云质量comboBox.DataSource = Enum.GetNames(typeof(Common.enViewQuality));
                this.测量环境配置comboBox.DataSource = Enum.GetNames(typeof(Common.enMeasureEnvironmentConfig));
                this.进程Socket_comboBox.Items.Clear();
                this.进程Socket_comboBox.Items.Add("NONE");
                //foreach (var item in Common.SocketConnectManager.Instance.GetSocketName())
                //{
                //    this.进程Socket_comboBox.Items.Add(item);
                //}
                //this.进程Socket_comboBox.DataSource = Common.SocketConnectManager.Instance.GetSocketName() ;
                /// 绑定机台坐标
                this.xAxisResolution.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.X_encoderResolution), true, DataSourceUpdateMode.OnPropertyChanged);
                this.yAxisResolution.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.Y_encoderResolution), true, DataSourceUpdateMode.OnPropertyChanged);
                this.zAxisResolution.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.Z_encoderResolution), true, DataSourceUpdateMode.OnPropertyChanged);
                this.uAxisResolution.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.U_encoderResolution), true, DataSourceUpdateMode.OnPropertyChanged);
                this.vAxisResolution.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.V_encoderResolution), true, DataSourceUpdateMode.OnPropertyChanged);
                this.wAxisResolution.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.W_encoderResolution), true, DataSourceUpdateMode.OnPropertyChanged);
                this.ScanSpeed.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.ScanSpeed), true, DataSourceUpdateMode.OnPropertyChanged);
                this.移动textBox.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.MoveSpeed), true, DataSourceUpdateMode.OnPropertyChanged);
                this.坐标显示checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.IsShowCoordSys), true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像列宽缩放textBox.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.ImageWidthScale), true, DataSourceUpdateMode.OnPropertyChanged);
                this.视图类型comboBox.DataBindings.Add(nameof(this.视图类型comboBox.SelectedItem), GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.ViewType), true, DataSourceUpdateMode.OnPropertyChanged);
                this.保留点云深度checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.Depth_persistence), true, DataSourceUpdateMode.OnPropertyChanged);
                this.颜色属性comboBox.DataBindings.Add("Text", GlobalVariable.pConfig,nameof(GlobalVariable.pConfig.ColorAttrib), true, DataSourceUpdateMode.OnPropertyChanged);
                this.点云质量comboBox.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.PointQuality), true, DataSourceUpdateMode.OnPropertyChanged);
                /////////////////////////////
                this.测量环境配置comboBox.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.MeasureEnvironment), true, DataSourceUpdateMode.OnPropertyChanged);
                this.点间隔textBox.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.Stil_PointPitch), true, DataSourceUpdateMode.OnPropertyChanged);
                ///////////////////////////////////////////////////////////////
                this.启用禁用X轴编码器checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.EnabeDisable_X), true, DataSourceUpdateMode.OnPropertyChanged); //Stil_PointPitch
                this.启用禁用Y轴编码器checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.EnabeDisable_Y), true, DataSourceUpdateMode.OnPropertyChanged); //Stil_PointPitch
                this.启用禁用Z轴编码器checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.EnabeDisable_Z), true, DataSourceUpdateMode.OnPropertyChanged); //Stil_PointPitch
                this.启用禁用U轴编码器checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.EnabeDisable_U), true, DataSourceUpdateMode.OnPropertyChanged); //Stil_PointPitch
                this.启用禁用V轴编码器checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.EnabeDisable_V), true, DataSourceUpdateMode.OnPropertyChanged); //Stil_PointPitch
                this.启用禁用W轴编码器checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.EnabeDisable_W), true, DataSourceUpdateMode.OnPropertyChanged); //Stil_PointPitch
                //
                this.箭头长度numericUpDown.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.ArrowLength), true, DataSourceUpdateMode.OnPropertyChanged); //
                this.节点尺寸numericUpDown.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.NodeSize), true, DataSourceUpdateMode.OnPropertyChanged); //arrowLength
                this.点尺寸numericUpDown.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.PointSize), true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像刷新时间numericUpDown.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.ImageUpdataTime), true, DataSourceUpdateMode.OnPropertyChanged);
                this.OKNG字体大小textBox.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.OKNgSize), true, DataSourceUpdateMode.OnPropertyChanged);
                this.OKNG字体位置comboBox.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.OKNgPosition), true, DataSourceUpdateMode.OnPropertyChanged);
                this.OKNG行偏移textBox.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.OKNgRowOffset), true, DataSourceUpdateMode.OnPropertyChanged);
                this.OKNG列偏移textBox.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.OKNgColOffset), true, DataSourceUpdateMode.OnPropertyChanged);
                // 数据保存
                this.数据保存目标comboBox.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.DataSaveTarget), true, DataSourceUpdateMode.OnPropertyChanged);
                this.数据保存间隔numericUpDown.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.DataSaveGap), true, DataSourceUpdateMode.OnPropertyChanged);
                this.数据刷新numericUpDown.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.DataUpdataGap), true, DataSourceUpdateMode.OnPropertyChanged);
                this.数据输出绑定comboBox.DataBindings.Add("Text", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.DataOutputBinding), true, DataSourceUpdateMode.OnPropertyChanged);
                //  EnableCameraCalibrate EnableMachineCalibrate
                this.启用机台校准checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.EnableMachineCalibrate), true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用相机校准checkBox.DataBindings.Add("Checked", GlobalVariable.pConfig, nameof(GlobalVariable.pConfig.EnableCameraCalibrate), true, DataSourceUpdateMode.OnPropertyChanged);

                ///////    系统参数 
                this.启用手边抓边功能checkBox.DataBindings.Add("Checked", SystemParamManager.Instance.SysConfigParam, "EnableManualCalliper", true, DataSourceUpdateMode.OnPropertyChanged);
                this.禁用页面切换checkBox.DataBindings.Add("Checked", SystemParamManager.Instance.SysConfigParam, "DisablePageSwitch", true, DataSourceUpdateMode.OnPropertyChanged);
                this.同步夹抓参数checkBox.DataBindings.Add("Checked", SystemParamManager.Instance.SysConfigParam, "IsSynJawParam", true, DataSourceUpdateMode.OnPropertyChanged);
                this.同步相机参数checkBox.DataBindings.Add("Checked", SystemParamManager.Instance.SysConfigParam, "IsSynCamParam", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.进程Socket_comboBox.DataBindings.Add("Text", SystemParamManager.Instance.SysConfigParam, "GlobalSocketName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.列数量textBox.DataBindings.Add("Text", SystemParamManager.Instance.SysConfigParam, "ColumCount", true, DataSourceUpdateMode.OnPropertyChanged);
                this.窗体顶层显示checkBox.DataBindings.Add("Checked", SystemParamManager.Instance.SysConfigParam, "IsFormTopMost", true, DataSourceUpdateMode.OnPropertyChanged);
                this.相机初始化checkBox.DataBindings.Add("Checked", SystemParamManager.Instance.SysConfigParam, "IsInitSensor", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch
            {
               // throw new Exception();
            }
        }

        private void paramConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //fo.SaveConfigParam("ParamConfig.txt", GlobalVariable.pConfig);
            GlobalVariable.pConfig.SaveParamConfig(); //"ParamConfig.txt"
            SystemParamManager.Instance.Save();
        }


    }
}
