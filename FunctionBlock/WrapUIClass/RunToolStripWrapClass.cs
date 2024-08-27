using FunctionBlock;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
  public  class RunToolStripWrapClass
    {
        private IFunction _function;
        private  ToolStripStatusLabel 运行结果toolStripStatusLabel;
        private ToolStrip toolStrip1;
        public RunToolStripWrapClass(IFunction function,ToolStrip toolStrip,ToolStripStatusLabel lable)
        {
            this._function = function;
            this.运行结果toolStripStatusLabel = lable;
            this.toolStrip1 = toolStrip;
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
        }
        protected void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "执行":
                        if (this.运行结果toolStripStatusLabel.Text == "等待……") break;
                        this.运行结果toolStripStatusLabel.Text = "等待……";
                        this.运行结果toolStripStatusLabel.ForeColor = Color.Yellow;
                        Task.Run(() =>
                        {
                            if (this._function.Execute(null).Succss)
                            {
                                this.toolStrip1.Invoke(new Action(() =>
                                {
                                    //this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.运行结果toolStripStatusLabel.Text = "成功";
                                    this.运行结果toolStripStatusLabel.ForeColor = Color.Green;
                                }));
                            }
                            else
                            {
                                this.toolStrip1.Invoke(new Action(() =>
                                {
                                    //this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.运行结果toolStripStatusLabel.Text = "失败";
                                    this.运行结果toolStripStatusLabel.ForeColor = Color.Red;
                                }));
                            }
                        }
                        );
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }





    }
}
