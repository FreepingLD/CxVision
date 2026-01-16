using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;


namespace MotionControlCard
{
    public partial class CoordSysConfigParamManageForm : Form
    {

        public bool _userEnable;
        public bool UserEnable
        {
            get
            {
                return _userEnable;
            }
            set
            {
                this._userEnable = value;
                if (this._userEnable)
                {
                    this.dataGridView1.ReadOnly = false;
                    this.SaveButton.Enabled = true;
                    this.坐标系配置label.Enabled = true;
                }
                else
                {
                    this.dataGridView1.ReadOnly = true;
                    this.SaveButton.Enabled = false;
                    this.坐标系配置label.Enabled = false;
                }
            }
        }

        private BindingList<CoordAxisConfigParam> listConfigPara;

        public CoordSysConfigParamManageForm()
        {
            InitializeComponent();
        }

        private void DeviceConfigParamManageForm_Load(object sender, EventArgs e)
        {
            // 读取传感器配置文件 
            this.listConfigPara = CoordSysConfigParamManger.Instance.CoordSysConfigParamList; // 使用一个全局的参数对象, 配置文件统一在某个地方读取
            if (listConfigPara == null)
                listConfigPara = new BindingList<CoordAxisConfigParam>();
            ///////////////////////////////////////////////////// 添加项目一定要放到 数据源绑定源前面
            this.CoordSysName.Items.Clear();
            this.CoordSysName.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.CoordSysName.Items.Add(item);
            /// 添加轴名称
            this.AxisName.Items.Clear();
            this.AxisName.ValueType = typeof(enAxisName);
            foreach (enAxisName item in Enum.GetValues(typeof(enAxisName)))
                this.AxisName.Items.Add(item);
            /// 添加数据类型
            this.DataTypeColumn.Items.Clear();
            this.DataTypeColumn.ValueType = typeof(enDataTypes);
            foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                this.DataTypeColumn.Items.Add(item);
            /// 添加轴读写状态
            this.AxisReadWrite.Items.Clear();
            this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
            foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                this.AxisReadWrite.Items.Add(item);
            /// 添加运动设备名称
            this.CardNameColumn.Items.Clear();
            this.CardNameColumn.ValueType = typeof(string);
            this.CardNameColumn.Items.Add("NONE");
            foreach (IMotionControl item in MotionCardManage.CardList)
                this.CardNameColumn.Items.Add(item.Name);
            //if(this.CardNameColumn.Items.Count==0)
            //    this.CardNameColumn.Items.Add("NONE");
            //////////////////////
            this.dataGridView1.DataSource = listConfigPara;
            this.dataGridView1.DoubleBuffere(true);
            //////////////////////////////////////////////
            //this.服务器comboBox.DataSource = SocketConnectManager.Instance.GetSocketName();
        }

        private void CoordSysConfigParamManageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //this.save();
                //XML<BindingList<SensorConfigParam>>.Save(this.listConfigPara, Application.StartupPath + "\\" + "sensorConfig.txt");
                if (CoordSysConfigParamManger.Instance.Save())
                    LoggerHelper.Error("坐标系配置文件保存成功");
                else
                    LoggerHelper.Error("坐标系配置文件保存失败");
            }
            catch
            {
                LoggerHelper.Error("坐标系配置文件保存报错");
            }
        }



        private void ClearButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.listConfigPara.Clear();
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn":
                            if (e.RowIndex < 0) return;
                            if (this.listConfigPara.Count > e.RowIndex)
                                this.listConfigPara.RemoveAt(e.RowIndex);
                            break;

                        case "InsertBtn":
                            this.listConfigPara.Insert(e.RowIndex, new CoordAxisConfigParam());
                            break;

                        case "AxisInfo":
                            //AxisInfo axisInfo = this.listConfigPara[e.RowIndex].AxisInfo ;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (CoordSysConfigParamManger.Instance.Save())
                    new UserMessageForm().ShowDialog("配置文件保存成功");
                else
                    new UserMessageForm().ShowDialog("配置文件保存失败");
            }
            catch (Exception ex)
            {
                new UserMessageForm().ShowDialog("配置文件保存报错:" + ex.ToString());
            }
        }

        private void 下载到服务器Btn_Click(object sender, EventArgs e)
        {
            try
            {
                SocketBase socket = SocketConnectManager.Instance.GetSocket(this.服务器comboBox.Text);
                if (socket != null)
                {
                    SocketMessage message = new SocketMessage();
                    message.Lable = enSocketInfo.下载坐标系配置;
                    message.MesContent = this.listConfigPara;
                    object recive = socket.GetDataAsync(message, true, 30000);
                    message = recive as SocketMessage;
                    if (socket.WaitReceive())
                        new UserMessageForm().ShowDialog(message.MesContent.ToString());
                    else
                        new UserMessageForm().ShowDialog("下载到服务器失败!");
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 加载服务器配置Btn_Click(object sender, EventArgs e)
        {
            try
            {
                SocketBase socket = SocketConnectManager.Instance.GetSocket(this.服务器comboBox.Text);
                if (socket != null)
                {
                    SocketMessage message = new SocketMessage();
                    message.Lable = enSocketInfo.加载坐标系配置;
                    message.MesContent = "";
                    //socket.SendDataAsync(message, true);
                    //socket.WaitReceive();
                    object oo = socket.GetDataAsync(message, true, 30000);
                    if (oo != null)
                    {
                        SocketMessage mesg = oo as SocketMessage;
                        if (mesg.MesContent != null)
                        {
                            switch (mesg.MesContent.GetType().Name)
                            {
                                case "BindingList`1":
                                    Type[] type = mesg.MesContent.GetType().GenericTypeArguments;
                                    if (type.Length > 0)
                                    {
                                        switch (type[0].Name)
                                        {
                                            case nameof(CoordAxisConfigParam):
                                                BindingList<CoordAxisConfigParam> listCam = mesg.MesContent as BindingList<CoordAxisConfigParam>;
                                                this.listConfigPara?.Clear();
                                                foreach (var item in listCam)
                                                {
                                                    this.listConfigPara.Add(item);
                                                }
                                                //////////////////////////
                                                new UserMessageForm().ShowDialog("加载成功");
                                                break;
                                            default:
                                                new UserMessageForm().ShowDialog("加载失败,加载的参数类型错误!");
                                                break;
                                        }
                                    }
                                    break;
                                case "List`1":
                                    type = mesg.MesContent.GetType().GenericTypeArguments;
                                    if (type.Length > 0)
                                    {
                                        switch (type[0].Name)
                                        {
                                            case nameof(CoordAxisConfigParam):
                                                List<CoordAxisConfigParam> listCam = mesg.MesContent as List<CoordAxisConfigParam>;
                                                this.listConfigPara?.Clear();
                                                foreach (var item in listCam)
                                                {
                                                    this.listConfigPara.Add(item);
                                                }
                                                //////////////////////////
                                                new UserMessageForm().ShowDialog("加载成功");
                                                break;
                                            default:
                                                new UserMessageForm().ShowDialog("加载失败,加载的参数类型错误!");
                                                break;
                                        }
                                    }

                                    break;
                                default:
                                    new UserMessageForm().ShowDialog("加载失败,加载对象的数据内容为非集合类型!");
                                    break;
                            }
                        }
                        else
                            new UserMessageForm().ShowDialog("加载失败,加载对象的数据内容为空!");
                    }
                    else
                        new UserMessageForm().ShowDialog("加载失败,加载对象为空!");
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }
    }
}
