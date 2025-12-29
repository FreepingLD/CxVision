using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace VisionBase
{
    public partial class UsrInspParaTeach : UserControl
    {

        /* 这个用户控件类的作用是：示教检测检测检测工程中的检测参数集；
         * 检测参数集（TeachInspParaGroups）：包含多组检测参数 （TeachInspParaGroup）
         * 检测参数集（TeachInspParaGroups）按组显示在数据表（InspDgv）中
         * 点击数据表（InspDgv），选中要示教的检测参数组 （TeachInspParaGroup）
         * 
         * 
         * 
         */

        public HObject CurImg = new HObject();
        ///Roi内的原始图像
        private HObject roiOriImg = new HObject();
        /// <summary> Roi内的滤波图像/ </summary>
        private HObject filterOriImg = new HObject();
        public UsrInspParaTeach()
        {
            InitializeComponent();
        }
        /// <summary> 当前ROI下的检测参数组  ，检测工程的检测参数集</summary>
        public List<InspParaBase> TeachInspParaGroups = new List<InspParaBase>();

        public bool IsSave = false;
        /// <summary>  组数 </summary>
        public int GroupCount = 0;
        /// <summary> 每组的长度集合 </summary>
        public List<int> ListGrLenth = new List<int>();
        /// <summary> 每组的起始索引值集合 </summary>
        public List<int> StartIndexList = new List<int>();
        /// <summary> 示教的组号索引 </summary>
        public int TeachGroupNo = 0;
        /// <summary> 选出检测类型 </summary>
        public InspEnum SelectModel = InspEnum.InspBase;
        /// <summary> 检测参数示教界面 </summary>
        public UsrInspSubBase myInspUsr = new UsrInspSubBase();
        /// <summary> 当前示教的组参数</summary>
        public List<InspParaBase> TeachInspParaGroup = new List<InspParaBase>();
        public UsrInspParaTeach(List<InspParaBase> ListInspParaIn  )
        {
            InitializeComponent();
            TeachInspParaGroups = ListInspParaIn;
            IsSave = false;
        }
        ///传入原始图像
        public void UpDateCurImg(HObject ImgIn)
        {
            if (CurImg != null) CurImg.Dispose();
            CurImg = ImgIn;
            //FilterBase MyFilter =  FilterCtrl.CreateFun()
        }

        public void SetImg(HObject RoiOriImg, HObject RoiFilterImg)
        {
            if (this.filterOriImg != null) this.filterOriImg.Dispose();
            if (this.roiOriImg != null) this.roiOriImg.Dispose();
            this.filterOriImg = RoiFilterImg;
            this.roiOriImg = RoiOriImg;
            myInspUsr.SetImg(RoiOriImg, RoiFilterImg);
        }

        private void UsrInspParaTeach_Load(object sender, EventArgs e)
        {
            this.InspModelDgvCbx.DataSource = new InspEnum[] {InspEnum.InspBase,  InspEnum.AcfInsp, InspEnum.RectInsp, InspEnum.LineInsp,
                InspEnum.CircleInsp, InspEnum.Circle2Insp  ,InspEnum.LineConvolInsp};
            this.InspEnumCbx.DataSource = new InspEnum[] {InspEnum.InspBase, InspEnum.AcfInsp, InspEnum.RectInsp, InspEnum.LineInsp,
                InspEnum.CircleInsp, InspEnum.Circle2Insp ,InspEnum.LineConvolInsp};
            if (InspParaCtrl.GetGroupCount(TeachInspParaGroups, out GroupCount, out ListGrLenth, out StartIndexList))
            {
                UpdateDgv();
            }
            InspParaCtrl.GetGroupInspPara(TeachInspParaGroups, 0, out TeachInspParaGroup);
            UpDatePanel();

        }

        private void StartTeachBtn_Click(object sender, EventArgs e)
        {
            AddBtn.Enabled = true;
            DeleteBtn.Enabled = true;
            ParaResetBtn.Enabled = true;
            SaveBtn.Enabled = true;
            UsrInspPanel.Enabled = true;
        }
        /// <summary>
        /// 删除指定的检测组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DialogResult rlt = MessageBox.Show("即将删除参数 ，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rlt == DialogResult.No) return; 

            if (TeachInspParaGroups.Count == 0) return;
            if (!InspParaCtrl.GetGroupCount(TeachInspParaGroups, out GroupCount, out ListGrLenth, out StartIndexList))
            {
                return;           
            }
            InspParaCtrl.DeleListInspPara(TeachInspParaGroups, StartIndexList[TeachGroupNo], ListGrLenth[TeachGroupNo], out TeachInspParaGroups); //删除指定组
            if (InspParaCtrl.GetGroupCount(TeachInspParaGroups, out GroupCount, out ListGrLenth, out StartIndexList))
            {
                UpdateDgv();
            }
            TeachGroupNo = 0;
        }

        /// <summary>
        /// 添加新的检组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBtn_Click(object sender, EventArgs e)
        {
            DialogResult rlt = MessageBox.Show("是否添加新的检测参数组 ，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rlt == DialogResult.No) return;
            InspParaCtrl.AddListInspPara(TeachInspParaGroups, out TeachInspParaGroups);
            if (InspParaCtrl.GetGroupCount(TeachInspParaGroups, out GroupCount, out ListGrLenth, out StartIndexList))
            {
                UpdateDgv();
            }
        }
        /// <summary>
        /// 重置检测参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParaResetBtn_Click(object sender, EventArgs e)
        {
            if (this.roiOriImg == null && this.roiOriImg.IsInitialized())
            {
                MessageBox.Show("请先加载图片", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return;
            }
            DialogResult rlt = MessageBox.Show("即将重置当前组的检测参数 ，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rlt == DialogResult.No) return;
            InspParaCtrl.GetGroupInspPara(TeachInspParaGroups, TeachGroupNo, out TeachInspParaGroup);
            if (TeachInspParaGroup.Count > 0) 
            if (SelectModel != TeachInspParaGroup[0].InspModel)
            {
                for (int i = 0; i < TeachInspParaGroup.Count(); i++)
                {
                    TeachInspParaGroup[i] = InspParaCtrl.CreateInspPara(SelectModel);
                }           
            }
            this.UpDatePanel();
        }


        /// <summary>
        /// 将myInspUsr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            DialogResult rlt = MessageBox.Show("即将保存参数 ，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rlt == DialogResult.Yes)
            {
                IsSave = true;
                TeachInspParaGroup = myInspUsr.ListInspPara;
                //TeachInspParaGroups = myInspUsr.ListInspPara;
                //InspParaCtrl.InsertGroup(TeachInspParaGroups, TeachGroupNo, TeachInspParaGroup, out TeachInspParaGroups);//保存示教的参数组
                InspParaCtrl.ReplaceGreoupInspPara(TeachInspParaGroups, TeachGroupNo, TeachInspParaGroup, out TeachInspParaGroups);//保存示教的参数
            }
            if (InspParaCtrl.GetGroupCount(TeachInspParaGroups, out GroupCount, out ListGrLenth, out StartIndexList))
            {
                UpdateDgv();
            }
            AddBtn.Enabled = false;
            DeleteBtn.Enabled = false;
            ParaResetBtn.Enabled = false;
            SaveBtn.Enabled = false;
            UsrInspPanel.Enabled = false;

        }


        /// <summary>
        /// 检测参数信息更新到dgv
        /// </summary>
        private  void UpdateDgv()
        {
            DataGridView myDgv = InspDgv;
            myDgv.Rows.Clear();
            int Index = 0;
            if (GroupCount == ListGrLenth.Count && GroupCount == StartIndexList.Count)
            {
                for (int i = 0; i < GroupCount; i++)
                {
                    myDgv.Rows.Add(
                        ++Index,
                        TeachInspParaGroups[StartIndexList[i]].InspModel,
                        StartIndexList[i].ToString(),
                        ListGrLenth[i].ToString()
                        );    
                }           
            }     
        }

        /// <summary>
        /// 选取想要示教的检测参数组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InspDgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TeachGroupNo = e.RowIndex;
            for (int i = 0; i < InspDgv.Rows.Count; i++)
            {
                InspDgv.Rows[i].DefaultCellStyle.BackColor = Color.White;
            }
            InspDgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
            if (e.ColumnIndex == 4)
            {
                InspParaCtrl.GetGroupInspPara(TeachInspParaGroups, e.RowIndex, out TeachInspParaGroup);
                UpDatePanel();
            }
            

        }

        private void InspEnumCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectModel = (InspEnum)InspEnumCbx.SelectedItem;
        }

        /// <summary>
        /// 显示示教界面，将检测组的检测参数（TeachInspParaGroup），传给具体的检测参数示教界面
        /// </summary>
        private void UpDatePanel()
        {            
            UsrInspPanel.Controls.Clear();
            if(myInspUsr!=null)  myInspUsr.Dispose();
            if (TeachInspParaGroup.Count > 0)
            {
                myInspUsr = UsrInspSubBase.GetInspUsr(TeachInspParaGroup[0].InspModel); //根据检测类型，
                UsrInspPanel.Controls.Add(myInspUsr);
                myInspUsr.SetImg(this.roiOriImg, this.filterOriImg);
                myInspUsr.SetListInspPara(TeachInspParaGroup);
                myInspUsr.Show();

            }
           
        }


        #region 注释
        private void DeleteBtn_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.DeleteBtn, "删除检测算法。先在左侧算法列表中选择要删除的算法，点击“删除”。");
            FileLib.FormToolTip.SetToolTipMaxLength(toolTip1, 200);
        }

        private void AddBtn_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.AddBtn, "新增检测算法。先在上方算法列表下拉框中选择要添加的检测算法，点击“新增”。");
            FileLib.FormToolTip.SetToolTipMaxLength(toolTip1, 200);
        }

        private void ParaResetBtn_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.ParaResetBtn, "重置当前算法的检测参数。先在左侧算法列表中选择要重置参数的算法，点击“重置参数”。");
            FileLib.FormToolTip.SetToolTipMaxLength(toolTip1, 200);
        }

        private void SaveBtn_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.SaveBtn, "保存算法参数。修改的算法和对应的参数将会保存。");
            FileLib.FormToolTip.SetToolTipMaxLength(toolTip1, 200);
        }
        #endregion

        private void InspEnumCbx_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.InspEnumCbx, "选择要添加的算法。RectInsp用于表面普通缺陷检测，耗时短、LineConvolInsp线卷积算法，用于检表面细微划痕，耗时长。");
            FileLib.FormToolTip.SetToolTipMaxLength(toolTip1, 200);
        }
    }
}
