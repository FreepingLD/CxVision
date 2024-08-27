using Common;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CxVision
{
    public partial class SensorConfig : Form
    {
        private FileOperate fo = new FileOperate();
        private Dictionary<string, List<string>> content;
        public SensorConfig()
        {
            InitializeComponent();
            ////////
            if ((Dictionary<string, List<string>>)fo.ReadConfigParam(Application.StartupPath + "\\" + "sensorConfig.txt") != null)
                content = (Dictionary<string, List<string>>)fo.ReadConfigParam(Application.StartupPath + "\\" + "sensorConfig.txt");
            else
            {
                content = new Dictionary<string, List<string>>();
                for (int i = 0; i < this.showCategory_comboBox.Items.Count; i++)
                {
                    if (!content.ContainsKey(this.showCategory_comboBox.Items[i].ToString()))
                        content.Add(this.showCategory_comboBox.Items[i].ToString(), new List<string>());
                }
            }
        }

        private void add_button1_Click(object sender, EventArgs e)
        {
            if (this.项目内容textBox.Text.Trim().Length > 0 && !this.listBox1.Items.Contains(this.项目内容textBox.Text))
            {
                this.listBox1.Items.Add(this.项目内容textBox.Text);
                content[this.showCategory_comboBox.SelectedItem.ToString()].Add(this.项目内容textBox.Text);
            }
            //  对象不是唯一的，但文件是唯一的，所以可以以文件作为操作依据，这里给配置文件提供一个路径
            if(this.showCategory_comboBox.SelectedItem.ToString()== enSensorBrand.Stil线激光.ToString())
            {
                //ParamConfig paramConfig;   // 这里改了启什么作用?
                //if ((ParamConfig)fo.ReadConfigParam("stilParamConfig.txt") != null)
                //    paramConfig = (ParamConfig)fo.ReadConfigParam("stilParamConfig.txt");
                //else
                //    paramConfig = new ParamConfig();
                //paramConfig.StilLineSensorConfig = this.configNameComboBox.SelectedItem.ToString();
                //////修改后马上保存
                //fo.SaveConfigParam("stilParamConfig.txt", paramConfig);
            }
        }

        private void delete_tbutton2_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex != -1)
            {
                try
                {
                    content[this.showCategory_comboBox.SelectedItem.ToString()].Remove(this.listBox1.SelectedItem.ToString());
                    this.listBox1.Items.Remove(this.listBox1.SelectedItem);
                    this.项目内容textBox.Text = "";
                }
                catch
                {
                    MessageBox . Show(new Exception().ToString());
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem != null)
                this.项目内容textBox.Text = this.listBox1.SelectedItem.ToString();
        }

        private void SensorConfig_Load(object sender, EventArgs e)
        {
            string[] item = new string[content.Keys.Count];
            content.Keys.CopyTo(item, 0);
            this.showCategory_comboBox.Items.AddRange(item);
            if (this.showCategory_comboBox.Items.Count > 0) this.showCategory_comboBox.SelectedIndex = 0;
            this.Add_category_comboBox.DataSource = Enum.GetNames(typeof(enSensorBrand));
            this.sub_category_comboBox.DataSource = Enum.GetNames(typeof(enSensorBrand));
            ///添加描述内容
            this.describe_textBox.Text="如果需要传入多个参数，则每个参数间使用分号隔开";
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Items.Count > 0)
                this.listBox1.Items.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.showCategory_comboBox.SelectedItem.ToString()) // 点激光可以自动添加
            {
                // 凡是通过USB连接的都可以自动获取序列号.只有一个参数的才可以自动添加
                case "Stil点激光":
                    ////////////////
                    if (this.showCategory_comboBox.SelectedItem.ToString() == enSensorBrand.Stil点激光.ToString())//
                    {
                        //StilPointLaser point = new StilPointLaser();
                        //if (point.InitDLL()) // DLL 只能初始化一次
                        //{
                        //   // string[] name = point.GetSensorName();
                        //   // content[this.showCategory_comboBox.SelectedItem.ToString()].Clear(); // 先清空再添加
                        //   // if (name != null) content[this.showCategory_comboBox.SelectedItem.ToString()].AddRange(name);
                        //}
                        //else
                        //{
                        //  //  MessageBox.Show("DLL 初始化失败");
                        //}
                    }
                    break;
                //case "LiYi点激光":
                //    ////////////////
                //    if (this.showCategory_comboBox.SelectedItem.ToString() == enSensorType.LiYi点激光.ToString())//
                //    {
                //        LiYiAdapter liyi = new LiYiAdapter();
                //        if (liyi.InitDLL())
                //        {
                //            string[] name = liyi.GetSensorName();
                //            if (content[this.showCategory_comboBox.SelectedItem.ToString()].Count==0)
                //           // content[this.showCategory_comboBox.SelectedItem.ToString()].Clear(); // 先清空再添加
                //            if (name != null) content[this.showCategory_comboBox.SelectedItem.ToString()].AddRange(name);
                //        }
                //    }
                //    break;
                default:
                    break;
            }
            //// 在列表中显示
            this.listBox1.Items.Clear();
            this.项目内容textBox.Clear();
            this.项目内容textBox.Focus();
            if (content.ContainsKey(this.showCategory_comboBox.SelectedItem.ToString()))
            {
                for (int i = 0; i < content[this.showCategory_comboBox.SelectedItem.ToString()].Count; i++)
                {
                    this.listBox1.Items.Add(content[this.showCategory_comboBox.SelectedItem.ToString()][i]);
                }           
            }
        }

        private void SensorConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                fo.SaveConfigParam("sensorConfig.txt", content);
               // MessageBox.Show("保存成功");
            }   
            catch
            {
               // MessageBox.Show("配置保存失败");
            }
        }

        // 添加类别
        private void AddCategory_button1_Click(object sender, EventArgs e)
        {
            this.showCategory_comboBox.Items.Add(this.Add_category_comboBox.SelectedItem);
            if (this.Add_category_comboBox.SelectedItem.ToString() == "NONE") MessageBox.Show("添加的类别不能为NONE");
            if (!content.ContainsKey(this.Add_category_comboBox.SelectedItem.ToString()))
                content.Add(this.Add_category_comboBox.SelectedItem.ToString(), new List<string>());
            // 这样排序后，可实现先创建对象，再改变索引
            this.showCategory_comboBox.Text = this.Add_category_comboBox.SelectedItem.ToString();
        }
        // 删除类别
        private void ClearCategory_button_Click(object sender, EventArgs e) // Category:类别，分类
        {
            if (this.showCategory_comboBox.SelectedIndex != -1)
            {
                content.Remove(this.sub_category_comboBox.SelectedItem.ToString());
                this.showCategory_comboBox.Items.Remove(this.sub_category_comboBox.SelectedItem);
            }
            this.showCategory_comboBox.SelectedIndex = 0;
        }

        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                fo.SaveConfigParam("sensorConfig.txt", content);
                MessageBox.Show("保存成功");
            }
            catch
            {
                MessageBox.Show("配置保存失败");
            }
        }
    }
}
