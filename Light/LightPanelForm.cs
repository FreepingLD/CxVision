using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Light
{
    public partial class LightPanelForm : Form
    {
        private ILightControl _light;
        public LightPanelForm()
        {
            InitializeComponent();
        }

        public LightPanelForm(ILightControl light)
        {
            InitializeComponent();
            this.光源名称comboBox.Items.Clear();
            foreach (var item in LightConnectManage.GetLightName())
            {
                this.光源名称comboBox.Items.Add(item);
            }
            this.光源名称comboBox.Text = light?.Name;
            this.InitForm(light);
        }

        private void InitForm(ILightControl light)
        {
            this._light = light;
            if (this._light != null)
            {
                /////////////////////////////////////////////
                this.通道1trackBar.Enabled = false;
                this.通道1numericUpDown.Enabled = false;
                this.通道1Btn.Enabled = false;
                this.通道2trackBar.Enabled = false;
                this.通道2numericUpDown.Enabled = false;
                this.通道2Btn.Enabled = false;
                this.通道3trackBar.Enabled = false;
                this.通道3numericUpDown.Enabled = false;
                this.通道3Btn.Enabled = false;
                this.通道4trackBar.Enabled = false;
                this.通道4numericUpDown.Enabled = false;
                this.通道4Btn.Enabled = false;
                this.通道5trackBar.Enabled = false;
                this.通道5numericUpDown.Enabled = false;
                this.通道5Btn.Enabled = false;
                this.通道6trackBar.Enabled = false;
                this.通道6numericUpDown.Enabled = false;
                this.通道6Btn.Enabled = false;
                this.通道7trackBar.Enabled = false;
                this.通道7numericUpDown.Enabled = false;
                this.通道7Btn.Enabled = false;
                this.通道8trackBar.Enabled = false;
                this.通道8numericUpDown.Enabled = false;
                this.通道8Btn.Enabled = false;
                this.通道9trackBar.Enabled = false;
                this.通道9numericUpDown.Enabled = false;
                this.通道9Btn.Enabled = false;
                this.通道10trackBar.Enabled = false;
                this.通道10numericUpDown.Enabled = false;
                this.通道10Btn.Enabled = false;
                this.通道11trackBar.Enabled = false;
                this.通道11numericUpDown.Enabled = false;
                this.通道11Btn.Enabled = false;
                this.通道12trackBar.Enabled = false;
                this.通道12numericUpDown.Enabled = false;
                this.通道12Btn.Enabled = false;
                this.通道13trackBar.Enabled = false;
                this.通道13numericUpDown.Enabled = false;
                this.通道13Btn.Enabled = false;
                this.通道14trackBar.Enabled = false;
                this.通道14numericUpDown.Enabled = false;
                this.通道14Btn.Enabled = false;
                this.通道15trackBar.Enabled = false;
                this.通道15numericUpDown.Enabled = false;
                this.通道15Btn.Enabled = false;
                this.通道16trackBar.Enabled = false;
                this.通道16numericUpDown.Enabled = false;
                this.通道16Btn.Enabled = false;
                this.通道1textBox.Enabled = false;
                this.通道2textBox.Enabled = false;
                this.通道3textBox.Enabled = false;
                this.通道4textBox.Enabled = false;
                this.通道5textBox.Enabled = false;
                this.通道6textBox.Enabled = false;
                this.通道7textBox.Enabled = false;
                this.通道8textBox.Enabled = false;
                this.通道9textBox.Enabled = false;
                this.通道10textBox.Enabled = false;
                this.通道11textBox.Enabled = false;
                this.通道12textBox.Enabled = false;
                this.通道13textBox.Enabled = false;
                this.通道14textBox.Enabled = false;
                this.通道15textBox.Enabled = false;
                this.通道16textBox.Enabled = false;
                this.实时1Btn.Enabled = false;
                this.实时2Btn.Enabled = false;
                this.实时3Btn.Enabled = false;
                this.实时4Btn.Enabled = false;
                this.实时5Btn.Enabled = false;
                this.实时6Btn.Enabled = false;
                this.实时7Btn.Enabled = false;
                this.实时8Btn.Enabled = false;
                this.实时9Btn.Enabled = false;
                this.实时10Btn.Enabled = false;
                this.实时11Btn.Enabled = false;
                this.实时12Btn.Enabled = false;
                this.实时13Btn.Enabled = false;
                this.实时14Btn.Enabled = false;
                this.实时15Btn.Enabled = false;
                this.实时16Btn.Enabled = false;
                //////////////////////////////
                for (int i = 0; i < this._light.ConfigParam.ChannelCount; i++)
                {
                    int value = this._light.GetLight((enLightChannel)(i + 1));
                    //if (value < 0) break;
                    foreach (Control item in this.Controls)
                    {
                        if (item.Text == $"通道{i + 1}")
                        {
                            foreach (Control item2 in item.Controls)
                            {
                                if (item2.Name == $"通道{i + 1}trackBar")
                                {
                                    switch (this._light.ConfigParam.LightModel)
                                    {
                                        case enLightModel.常规控制器:
                                            ((TrackBar)item2).Value = value;
                                            item2.Enabled = true;
                                            break;
                                        case enLightModel.频闪控制器:
                                            ((TrackBar)item2).Value = value;
                                            item2.Enabled = true;
                                            break;
                                    }
                                }
                                //////////////////////////////////////////
                                if (item2.Name == $"通道{i + 1}numericUpDown")
                                {
                                    switch (this._light.ConfigParam.LightModel)
                                    {
                                        case enLightModel.常规控制器:
                                            ((NumericUpDown)item2).Value = value;
                                            item2.Enabled = true;
                                            break;
                                        case enLightModel.频闪控制器:
                                            ((NumericUpDown)item2).Value = value;
                                            item2.Enabled = true;
                                            break;
                                    }
                                }
                                //////////////////////////////////////////
                                if (item2.Name == $"通道{i + 1}Btn")
                                {
                                    item2.Enabled = true;
                                    if (this._light.LightParamList.Count > i)
                                    {
                                        if (this._light.LightParamList[i].ChannelState)
                                            item2.Text = "关闭";
                                        else
                                            item2.Text = "打开";
                                    }
                                }
                                //////////////////////////////////////////
                                if (item2.Name == $"通道{i + 1}textBox")
                                {
                                    if (this._light.LightParamList.Count > i)
                                    {
                                        item2.Text = this._light.LightParamList[i].Describe;
                                        item2.Enabled = true;
                                        //if (this._light.LightParamList[i].ChannelState) item2.Enabled = true;
                                    }
                                }
                                //////////////////////////////////////////
                                if (item2.Name == $"实时{i + 1}Btn")
                                {
                                    item2.Enabled = true;
                                    if (this._light.LightParamList.Count > i)
                                    {
                                        if (this._light.LightParamList[i].TriggerMode)
                                            item2.Text = "实时模式";
                                        else
                                            item2.Text = "触发模式";
                                    }
                                }
                            }
                            ///////////////////////////////////////////////////////
                            break;
                        }
                    }
                }
            }
        }

        private void 通道1trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_1, this.通道1trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道1numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_1);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道1numericUpDown.Value = this.通道1trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道2trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_2, this.通道2trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道2numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_2);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道2numericUpDown.Value = this.通道2trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道3trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_3, this.通道3trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道3numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_3);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道3numericUpDown.Value = this.通道3trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道4trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_4, this.通道4trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道4numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_4);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道4numericUpDown.Value = this.通道4trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道5trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_5, this.通道5trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道5numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_5);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道5numericUpDown.Value = this.通道5trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道6trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_6, this.通道6trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道6numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_6);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道6numericUpDown.Value = this.通道6trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道7trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_7, this.通道7trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道7numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_7);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道7numericUpDown.Value = this.通道7trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道8trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_8, this.通道8trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道8numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_8);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道8numericUpDown.Value = this.通道8trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道9trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_9, this.通道9trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道9numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_9);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道9numericUpDown.Value = this.通道9trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道10trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_10, this.通道10trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道10numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_10);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道10numericUpDown.Value = this.通道10trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道11trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_11, this.通道11trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道11numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_11);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道11numericUpDown.Value = this.通道11trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道12trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_12, this.通道12trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道12numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_12);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道12numericUpDown.Value = this.通道12trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道13trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_13, this.通道13trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道13numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_13);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道13numericUpDown.Value = this.通道13trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道14trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_14, this.通道14trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道14numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_14);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道14numericUpDown.Value = this.通道14trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道15trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetLight(enLightChannel.Channel_15, this.通道15trackBar.Value);
                    switch (this._light.ConfigParam.LightModel)
                    {
                        default:
                        case enLightModel.常规控制器:
                            this.通道15numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_15);
                            break;
                        case enLightModel.频闪控制器:
                            this.通道15numericUpDown.Value = this.通道15trackBar.Value;
                            break;
                    }
                    this.Cursor = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        private void 通道16trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this._light.SetLight(enLightChannel.Channel_16, this.通道16trackBar.Value);
                    this.通道16numericUpDown.Value = this._light.GetLight(enLightChannel.Channel_16);
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道1Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                ///////////////////////////////////////////
                if (this.通道1Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_1))
                        this.通道1Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_1))
                        this.通道1Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道2Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道2Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_2))
                        this.通道2Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_2))
                        this.通道2Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道3Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道3Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_3))
                        this.通道3Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_3))
                        this.通道3Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道4Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道4Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_4))
                        this.通道4Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_4))
                        this.通道4Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道5Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道5Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_5))
                        this.通道5Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_5))
                        this.通道5Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道6Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道6Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_6))
                        this.通道6Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_6))
                        this.通道6Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道7Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道7Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_7))
                        this.通道7Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_7))
                        this.通道7Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道8Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道8Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_8))
                        this.通道8Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_8))
                        this.通道8Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道9Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道9Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_9))
                        this.通道9Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_9))
                        this.通道9Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道10Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道10Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_10))
                        this.通道10Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_10))
                        this.通道10Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道11Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道11Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_11))
                        this.通道11Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_11))
                        this.通道11Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道12Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道12Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_12))
                        this.通道12Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_12))
                        this.通道12Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道13Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道13Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_13))
                        this.通道13Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_13))
                        this.通道13Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道14Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道14Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_14))
                        this.通道14Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_14))
                        this.通道14Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道15Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道15Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_15))
                        this.通道15Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_15))
                        this.通道15Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道16Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                {
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                    return;
                }
                //////////////////////////////////////////////////
                if (this.通道16Btn.Text == "打开")
                {
                    if (this._light.Open(enLightChannel.Channel_16))
                        this.通道16Btn.Text = "关闭";
                }
                else
                {
                    if (this._light.Close(enLightChannel.Channel_16))
                        this.通道16Btn.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道1numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_1, (int)this.通道1numericUpDown.Value);
                                this.通道1trackBar.Value = this._light.GetLight(enLightChannel.Channel_1);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_1, (int)this.通道1numericUpDown.Value);
                                this.通道1trackBar.Value = (int)this.通道1numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道2numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_2, (int)this.通道2numericUpDown.Value);
                                this.通道2trackBar.Value = this._light.GetLight(enLightChannel.Channel_2);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_2, (int)this.通道2numericUpDown.Value);
                                this.通道2trackBar.Value = (int)this.通道2numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道3numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_3, (int)this.通道3numericUpDown.Value);
                                this.通道3trackBar.Value = this._light.GetLight(enLightChannel.Channel_3);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_3, (int)this.通道3numericUpDown.Value);
                                this.通道3trackBar.Value = (int)this.通道3numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道4numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_4, (int)this.通道4numericUpDown.Value);
                                this.通道4trackBar.Value = this._light.GetLight(enLightChannel.Channel_4);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_4, (int)this.通道4numericUpDown.Value);
                                this.通道4trackBar.Value = (int)this.通道4numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道5numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_5, (int)this.通道5numericUpDown.Value);
                                this.通道5trackBar.Value = this._light.GetLight(enLightChannel.Channel_5);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_5, (int)this.通道5numericUpDown.Value);
                                this.通道5trackBar.Value = (int)this.通道5numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道6numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_6, (int)this.通道6numericUpDown.Value);
                                this.通道6trackBar.Value = this._light.GetLight(enLightChannel.Channel_6);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_6, (int)this.通道6numericUpDown.Value);
                                this.通道6trackBar.Value = (int)this.通道6numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道7numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_7, (int)this.通道7numericUpDown.Value);
                                this.通道7trackBar.Value = this._light.GetLight(enLightChannel.Channel_7);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_7, (int)this.通道7numericUpDown.Value);
                                this.通道7trackBar.Value = (int)this.通道7numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道8numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_8, (int)this.通道8numericUpDown.Value);
                                this.通道8trackBar.Value = this._light.GetLight(enLightChannel.Channel_8);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_8, (int)this.通道8numericUpDown.Value);
                                this.通道8trackBar.Value = (int)this.通道8numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道9numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_9, (int)this.通道9numericUpDown.Value);
                                this.通道9trackBar.Value = this._light.GetLight(enLightChannel.Channel_9);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_9, (int)this.通道9numericUpDown.Value);
                                this.通道9trackBar.Value = (int)this.通道9numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道10numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_10, (int)this.通道10numericUpDown.Value);
                                this.通道10trackBar.Value = this._light.GetLight(enLightChannel.Channel_10);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_10, (int)this.通道10numericUpDown.Value);
                                this.通道10trackBar.Value = (int)this.通道10numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道11numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_11, (int)this.通道11numericUpDown.Value);
                                this.通道11trackBar.Value = this._light.GetLight(enLightChannel.Channel_11);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_11, (int)this.通道11numericUpDown.Value);
                                this.通道11trackBar.Value = (int)this.通道11numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道12numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_12, (int)this.通道12numericUpDown.Value);
                                this.通道12trackBar.Value = this._light.GetLight(enLightChannel.Channel_12);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_12, (int)this.通道12numericUpDown.Value);
                                this.通道12trackBar.Value = (int)this.通道12numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道13numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_13, (int)this.通道13numericUpDown.Value);
                                this.通道13trackBar.Value = this._light.GetLight(enLightChannel.Channel_13);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_13, (int)this.通道13numericUpDown.Value);
                                this.通道13trackBar.Value = (int)this.通道13numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道14numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_14, (int)this.通道14numericUpDown.Value);
                                this.通道14trackBar.Value = this._light.GetLight(enLightChannel.Channel_14);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_14, (int)this.通道14numericUpDown.Value);
                                this.通道14trackBar.Value = (int)this.通道14numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }

                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道15numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_15, (int)this.通道15numericUpDown.Value);
                                this.通道15trackBar.Value = this._light.GetLight(enLightChannel.Channel_15);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_15, (int)this.通道15numericUpDown.Value);
                                this.通道15trackBar.Value = (int)this.通道15numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道16numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        this.Cursor = Cursors.WaitCursor;
                        switch (this._light.ConfigParam.LightModel)
                        {
                            default:
                            case enLightModel.常规控制器:
                                this._light.SetLight(enLightChannel.Channel_16, (int)this.通道16numericUpDown.Value);
                                this.通道16trackBar.Value = this._light.GetLight(enLightChannel.Channel_16);
                                break;
                            case enLightModel.频闪控制器:
                                this._light.SetLight(enLightChannel.Channel_16, (int)this.通道16numericUpDown.Value);
                                this.通道15trackBar.Value = (int)this.通道15numericUpDown.Value;
                                break;
                        }
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发模式Btn_Click(object sender, EventArgs e)
        {
            bool result = true;
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    for (int i = 1; i <= this._light.ConfigParam.ChannelCount; i++)
                    {
                        result = result && this._light.SetParam("触发模式", i);
                    }
                    this.Cursor = Cursors.Default;
                    if (result)
                        new UserMessageForm().ShowDialog("触发模式设置成功", "设置触发模式");
                    else
                        new UserMessageForm().ShowDialog("触发模式设置失败", "设置触发模式");
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 常亮模式Btn_Click(object sender, EventArgs e)
        {
            bool result = true;
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    for (int i = 1; i <= this._light.ConfigParam.ChannelCount; i++)
                    {
                        result = result && this._light.SetParam("常亮模式", i);
                    }
                    this.Cursor = Cursors.Default;
                    if (result)
                        new UserMessageForm().ShowDialog("常亮模式设置成功", "设置常亮模式");
                    else
                        new UserMessageForm().ShowDialog("常亮模式设置失败", "设置常亮模式");
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 频闪模式1Btn_Click(object sender, EventArgs e)
        {
            bool result = true;
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    for (int i = 1; i <= this._light.ConfigParam.ChannelCount; i++)
                    {
                        result = result && this._light.SetParam("频闪模式1", $"{i},{this.频闪模式1textBox.Text}");
                    }
                    this.Cursor = Cursors.Default;
                    if (result)
                        new UserMessageForm().ShowDialog("频闪模式1设置成功", "设置频闪模式1");
                    else
                        new UserMessageForm().ShowDialog("频闪模式1设置失败", "设置频闪模式1");
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 频闪模式2Btn_Click(object sender, EventArgs e)
        {
            bool result = true;
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    for (int i = 1; i <= this._light.ConfigParam.ChannelCount; i++)
                    {
                        result = result && this._light.SetParam("频闪模式2", $"{i},{this.频闪模式2textBox.Text}");
                        //if (result)
                        //    new UserMessageForm().ShowDialog($"通道{i}频闪设置成功", "设置频闪模式2");
                        //else
                        //    new UserMessageForm().ShowDialog($"通道{i}频闪设置失败", "设置频闪模式2");
                    }
                    this.Cursor = Cursors.Default;
                    if (result)
                        new UserMessageForm().ShowDialog("频闪模式2设置成功", "设置频闪模式2");
                    else
                        new UserMessageForm().ShowDialog("频闪模式2设置失败", "设置频闪模式2");
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发1Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 1);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发2Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 2);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发3Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 3);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发4Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 4);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发5Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 5);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发6Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 6);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发7Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 7);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发8Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 8);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发9Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 9);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发10Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 10);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发11Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 11);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发12Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 12);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发13Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 13);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发14Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 14);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发15Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 15);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 触发16Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._light.SetParam("触发", 16);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 光源名称comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.光源名称comboBox.SelectedItem == null) return;
                this.光源名称comboBox.Text = this.光源名称comboBox.SelectedItem.ToString();
                this._light = LightConnectManage.GetLight(this.光源名称comboBox.Text);
                this.InitForm(this._light);
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void LightControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this._light?.SetParam("保存参数", null);
            }
            catch
            {

            }
        }

        private void 通道1textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 0)
                            this._light.LightParamList[0].Describe = this.通道1textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道2textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 1)
                            this._light.LightParamList[1].Describe = this.通道2textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道3textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 2)
                            this._light.LightParamList[2].Describe = this.通道3textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道4textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 3)
                            this._light.LightParamList[3].Describe = this.通道4textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道5textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 4)
                            this._light.LightParamList[4].Describe = this.通道5textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道6textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 5)
                            this._light.LightParamList[5].Describe = this.通道6textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道7textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 6)
                            this._light.LightParamList[6].Describe = this.通道7textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道8textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 7)
                            this._light.LightParamList[7].Describe = this.通道8textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道9textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 8)
                            this._light.LightParamList[8].Describe = this.通道9textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道10textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 9)
                            this._light.LightParamList[9].Describe = this.通道10textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道11textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 10)
                            this._light.LightParamList[10].Describe = this.通道11textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道12textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 11)
                            this._light.LightParamList[11].Describe = this.通道12textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道13textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 12)
                            this._light.LightParamList[12].Describe = this.通道13textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道14textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 13)
                            this._light.LightParamList[13].Describe = this.通道14textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道15textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 14)
                            this._light.LightParamList[14].Describe = this.通道15textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道16textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this._light == null)
                        new UserMessageForm().ShowDialog("光源对象为NULL");
                    else
                    {
                        if (this._light.LightParamList != null && this._light.LightParamList.Count > 15)
                            this._light.LightParamList[15].Describe = this.通道16textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道1textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 0)
                        this._light.LightParamList[0].Describe = this.通道1textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道2textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 1)
                        this._light.LightParamList[1].Describe = this.通道2textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道3textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 2)
                        this._light.LightParamList[2].Describe = this.通道3textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道4textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 3)
                        this._light.LightParamList[3].Describe = this.通道4textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道5textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 4)
                        this._light.LightParamList[4].Describe = this.通道5textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道6textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 5)
                        this._light.LightParamList[5].Describe = this.通道6textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道7textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 6)
                        this._light.LightParamList[6].Describe = this.通道7textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道8textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 7)
                        this._light.LightParamList[7].Describe = this.通道8textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道9textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 8)
                        this._light.LightParamList[8].Describe = this.通道9textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道10textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 9)
                        this._light.LightParamList[9].Describe = this.通道10textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道11textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 10)
                        this._light.LightParamList[10].Describe = this.通道11textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道12textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 11)
                        this._light.LightParamList[11].Describe = this.通道12textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道13textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 12)
                        this._light.LightParamList[12].Describe = this.通道13textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道14textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 13)
                        this._light.LightParamList[13].Describe = this.通道14textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道15textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 14)
                        this._light.LightParamList[14].Describe = this.通道15textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 通道16textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    if (this._light.LightParamList != null && this._light.LightParamList.Count > 15)
                        this._light.LightParamList[15].Describe = this.通道16textBox.Text;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时1Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时1Btn.Text)
                    {
                        case "实时模式":
                            this.实时1Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 1);
                            break;
                        default:
                        case "触发模式":
                            this.实时1Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 1);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时2Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时2Btn.Text)
                    {
                        case "实时模式":
                            this.实时2Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 2);
                            break;
                        default:
                        case "触发模式":
                            this.实时2Btn.Text = "实时模式";
                            this._light.SetParam("触发模式",2);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时3Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时3Btn.Text)
                    {
                        case "实时模式":
                            this.实时3Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 3);
                            break;
                        default:
                        case "触发模式":
                            this.实时3Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 3);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时4Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时4Btn.Text)
                    {
                        case "实时模式":
                            this.实时4Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 4);
                            break;
                        default:
                        case "触发模式":
                            this.实时4Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 4);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时5Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时5Btn.Text)
                    {
                        case "实时模式":
                            this.实时5Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 5);
                            break;
                        default:
                        case "触发模式":
                            this.实时5Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 5);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时6Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时6Btn.Text)
                    {
                        case "实时模式":
                            this.实时6Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 6);
                            break;
                        default:
                        case "触发模式":
                            this.实时6Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 6);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时7Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时7Btn.Text)
                    {
                        case "实时模式":
                            this.实时7Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 7);
                            break;
                        default:
                        case "触发模式":
                            this.实时7Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 7);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时8Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时8Btn.Text)
                    {
                        case "实时模式":
                            this.实时8Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 8);
                            break;
                        default:
                        case "触发模式":
                            this.实时8Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 8);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时9Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时9Btn.Text)
                    {
                        case "实时模式":
                            this.实时9Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 9);
                            break;
                        default:
                        case "触发模式":
                            this.实时9Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 9);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时10Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时10Btn.Text)
                    {
                        case "实时模式":
                            this.实时10Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 10);
                            break;
                        default:
                        case "触发模式":
                            this.实时10Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 10);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时11Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时11Btn.Text)
                    {
                        case "实时模式":
                            this.实时11Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 11);
                            break;
                        default:
                        case "触发模式":
                            this.实时11Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 11);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时12Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时12Btn.Text)
                    {
                        case "实时模式":
                            this.实时12Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 12);
                            break;
                        default:
                        case "触发模式":
                            this.实时12Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 12);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时13Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时13Btn.Text)
                    {
                        case "实时模式":
                            this.实时13Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 13);
                            break;
                        default:
                        case "触发模式":
                            this.实时13Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 13);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时14Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时14Btn.Text)
                    {
                        case "实时模式":
                            this.实时14Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 14);
                            break;
                        default:
                        case "触发模式":
                            this.实时14Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 14);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时15Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时15Btn.Text)
                    {
                        case "实时模式":
                            this.实时15Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 15);
                            break;
                        default:
                        case "触发模式":
                            this.实时15Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 15);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 实时16Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    switch (this.实时16Btn.Text)
                    {
                        case "实时模式":
                            this.实时16Btn.Text = "触发模式";
                            this._light.SetParam("常亮模式", 16);
                            break;
                        default:
                        case "触发模式":
                            this.实时16Btn.Text = "实时模式";
                            this._light.SetParam("触发模式", 16);
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 频闪间隔Btn_Click(object sender, EventArgs e)
        {
            bool result = true;
            try
            {
                if (this._light == null)
                    new UserMessageForm().ShowDialog("光源对象为NULL");
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    for (int i = 1; i <= this._light.ConfigParam.ChannelCount; i++)
                    {
                        result = result && this._light.SetParam("频闪间隔", string.Join(",", i, this.内部触发间隔textBox.Text));
                    }
                    this.Cursor = Cursors.Default;
                    if (result)
                        new UserMessageForm().ShowDialog("常亮模式设置成功", "设置常亮模式");
                    else
                        new UserMessageForm().ShowDialog("常亮模式设置失败", "设置常亮模式");
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }


    }
}


