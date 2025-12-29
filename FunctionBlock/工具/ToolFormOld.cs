using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FunctionBlock;
using Sensor;

namespace FunctionBlock
{
    public partial class ToolFormOld : Form
    {
        //private AddAcqSourceForm form;
        private TreeViewWrapClass _treeViewTarget;
        public TreeViewWrapClass TreeViewTarget { get => _treeViewTarget; set => _treeViewTarget = value; }

        public ToolFormOld()
        {
            InitializeComponent();
        }
        public ToolFormOld(TreeViewWrapClass treeViewTarget)
        {
            InitializeComponent();
            this._treeViewTarget = treeViewTarget;
            this._treeViewTarget.ToolName = "";
        }

        public ToolFormOld(TreeViewWrapClass treeViewTarget, string toolName = "")
        {
            InitializeComponent();
            this._treeViewTarget = treeViewTarget;
            this._treeViewTarget.ToolName = toolName;
        }

        public void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode rootNode = null;
            string name = e.Node.Text;
            if (this._treeViewTarget == null)
            {
                if (ProgramForm.Instance.ProgramDic.ContainsKey(this.treeView1.Parent.Text))
                    this._treeViewTarget = ProgramForm.Instance.ProgramDic[this.treeView1.Parent.Text];
                else
                    new Common.UserMessageForm().ShowDialog($"未包含名称为:{this.treeView1.Parent.Text} 程序面板");
                //this._treeViewTarget = ((ProgramForm)Application.OpenForms?["ProgramForm"]).TreeViewWrapClass;
            }
            string nam22e = this._treeViewTarget.TreeView.Parent.Text;
            //ProgramForm.Instance.ProgramD[this.treeView1.Parent.Text]
            ////////////////////////////////////////////////////////////////
            switch (name.Trim())
            {
                case "相机采集源节点":
                case "相机采集源":
                case "采集源":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                this._treeViewTarget.AddItems(new AcqSource(SensorManage.CurrentCamSensor), "相机采集源");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "激光采集源节点":
                case "激光采集源":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                this._treeViewTarget.AddItems(new AcqSource(SensorManage.CurrentLaserSensor), "激光采集源");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "激光点":
                case "激光点节点":
                    if (this._treeViewTarget != null)
                    {
                        LaserPointAcq laserPointAcq;
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                if (AcqSourceManage.Instance.LaserAcqSourceList().Count > 0 && AcqSourceManage.Instance.GetCamAcqSourceList().Count > 0)
                                    laserPointAcq = new LaserPointAcq(AcqSourceManage.Instance.LaserAcqSourceList().Last(), AcqSourceManage.Instance.GetCamAcqSourceList().Last());
                                else
                                    laserPointAcq = new LaserPointAcq(new AcqSource("激光采集源"), new AcqSource("相机采集源"));
                                ///////////////////////////
                                rootNode = this._treeViewTarget.AddItems(laserPointAcq, laserPointAcq.Name, nameof(laserPointAcq.LaserAcqSource), nameof(laserPointAcq.CamAcqSource), nameof(laserPointAcq.WcsCoordSystem),
                                    nameof(laserPointAcq.Laser1Dist1DataHandle), nameof(laserPointAcq.Laser1Dist2DataHandle), nameof(laserPointAcq.Laser1ThickDataHandle));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "激光线":
                case "激光线节点":
                    if (this._treeViewTarget != null)
                    {
                        LaserLineScanAcq laserLineAcq;
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                if (AcqSourceManage.Instance.LaserAcqSourceList().Count > 0 && AcqSourceManage.Instance.GetCamAcqSourceList().Count > 0)
                                    laserLineAcq = new LaserLineScanAcq(AcqSourceManage.Instance.LaserAcqSourceList().Last(), AcqSourceManage.Instance.GetCamAcqSourceList().Last());
                                else
                                    laserLineAcq = new LaserLineScanAcq(new AcqSource("激光采集源"), new AcqSource("相机采集源"));
                                ///////////////////////////
                                rootNode = this._treeViewTarget.AddItems(laserLineAcq, laserLineAcq.Name, nameof(laserLineAcq.LaserAcqSource), nameof(laserLineAcq.CamAcqSource), nameof(laserLineAcq.WcsCoordSystem),
                                    nameof(laserLineAcq.Laser1Dist1DataHandle), nameof(laserLineAcq.Laser1Dist2DataHandle), nameof(laserLineAcq.Laser1ThickDataHandle));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "激光点2":
                case "激光点2节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new LaserPointAcqStandard(SensorManage.CurrentLaserSensor, GetCurrentCoordSystem()), "激光点2", "RefSource2-<=参考坐标系", "距离1-=>距离1", "距离2-=>距离2", "厚度-=>厚度");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;
                case "激光线2":
                case "激光线2节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new LaserScanAcqStandard(SensorManage.CurrentLaserSensor, GetCurrentCoordSystem()), "激光线2", "RefSource2-<=参考坐标系", "距离1-=>距离1", "距离2-=>距离2", "厚度-=>厚度");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "点云采集":  // 
                case "点云采集节点":  // 
                    if (this._treeViewTarget != null)
                    {
                        PointCloudAcq pointCloud = new PointCloudAcq();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(pointCloud, "点云采集", nameof(pointCloud.TrackPoint), nameof(pointCloud.WcsCoordSystem), nameof(pointCloud.Dist1DataHandle), nameof(pointCloud.Dist2DataHandle), nameof(pointCloud.ThickDataHandle));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "图像采集":
                case "图像采集节点":
                    if (this._treeViewTarget != null)
                    {
                        ImageAcq imag;
                        //if (AcqSourceManage.Instance.GetCamAcqSourceList().Count > 0)
                        //    imag = new ImageAcq(AcqSourceManage.Instance.GetCamAcqSourceList()[0].Name);
                        //else
                        //    imag = new ImageAcq(null);
                        // 
                        imag = new ImageAcq(ProgramForm.Instance.AcqSourceName);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(imag, "图像采集", nameof(imag.ImageData), nameof(imag.DarkImageData), nameof(imag.GrabPoint));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(imag, "ImageGrab", nameof(imag.ImageData), nameof(imag.DarkImageData), nameof(imag.GrabPoint));
                                break;
                        }
                    }
                    break;
                case "开始采集":
                case "开始采集节点":
                    if (this._treeViewTarget != null)
                    {
                        StartAcq startAcq = new StartAcq(ProgramForm.Instance.AcqSourceName);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(startAcq, "开始采集"); // 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "停止采集":
                case "停止采集节点":
                    if (this._treeViewTarget != null)
                    {
                        StopAcq stotAcq = new StopAcq(ProgramForm.Instance.AcqSourceName);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(stotAcq, "停止采集"); // 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "相机配置":
                case "相机配置节点":
                case "传感器配置":
                case "传感器配置节点":
                    if (this._treeViewTarget != null)
                    {
                        SensorConfig sensorConfig = new SensorConfig();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(sensorConfig, "相机配置"); // 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "图像采集2":
                case "图像采集2节点":
                    if (this._treeViewTarget != null)
                    {
                        //ImagesAcq2 imagesAcq2 = new ImagesAcq2();
                        //this._treeViewTarget.AddItems(imagesAcq2, "图像采集2", nameof(imagesAcq2.AcqSourceName), nameof(imagesAcq2.AcqSourceName)); // , "图像对象-=>图像对象"
                    }
                    break;
                case "矩形扫描":
                case "矩形扫描节点":
                    switch (SensorManage.LaserList.Count)
                    {
                        case 0:
                            if (this._treeViewTarget != null)
                                rootNode = this._treeViewTarget.AddItems(new RectangleScanAcq(null, null, GetCurrentCoordSystem()), "矩形扫描", "RefSource2-<=参考坐标系", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");   // 减少3D对象定义域  双激光扫描线
                            break;
                        case 1:
                            if (this._treeViewTarget != null)
                                rootNode = this._treeViewTarget.AddItems(new RectangleScanAcq(SensorManage.LaserList[0].Name, null, GetCurrentCoordSystem()), "矩形扫描", "RefSource2-<=参考坐标系", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");
                            break;
                        default:
                        case 2:
                            if (this._treeViewTarget != null)
                                rootNode = this._treeViewTarget.AddItems(new RectangleScanAcq(SensorManage.LaserList[0].Name, SensorManage.LaserList[1].Name, GetCurrentCoordSystem()), "矩形扫描", "RefSource2-<=参考坐标系", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");
                            break;
                    }
                    break;

                case "圆弧扫描":
                case "圆弧扫描节点":
                    switch (SensorManage.LaserList.Count)
                    {
                        case 0:
                            if (this._treeViewTarget != null)
                                rootNode = this._treeViewTarget.AddItems(new SpiralScanAcq(null, null, GetCurrentCoordSystem()), "圆弧扫描", "RefSource2-<=参考坐标系", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");   // 减少3D对象定义域  双激光扫描线
                            break;
                        case 1:
                            if (this._treeViewTarget != null)
                                rootNode = this._treeViewTarget.AddItems(new SpiralScanAcq(SensorManage.LaserList[0].Name, null, GetCurrentCoordSystem()), "圆弧扫描", "RefSource2-<=参考坐标系", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");
                            break;
                        default:
                        case 2:
                            if (this._treeViewTarget != null)
                                rootNode = this._treeViewTarget.AddItems(new SpiralScanAcq(SensorManage.LaserList[0].Name, SensorManage.LaserList[1].Name, GetCurrentCoordSystem()), "圆弧扫描", "RefSource2-<=参考坐标系", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");
                            break;
                    }
                    break;
                case "直线扫描":
                case "直线扫描节点":
                    switch (SensorManage.LaserList.Count)
                    {
                        case 0:
                            if (this._treeViewTarget != null)
                                rootNode = this._treeViewTarget.AddItems(new LaserScanAcqPath(null), "直线扫描", "RefSource1-<=扫描路径", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");   // 减少3D对象定义域  双激光扫描线
                            break;
                        default:
                        case 1:
                            if (this._treeViewTarget != null)
                                rootNode = this._treeViewTarget.AddItems(new LaserScanAcqPath(SensorManage.LaserList[0]), "直线扫描", "RefSource1-<=扫描路径", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");
                            break;
                    }
                    break;

                case "对射校准":
                case "对射校准节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new TroughCalibrate(), "对射校准");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                //case "对射测厚":
                //case "对射测厚节点":
                //    if (this._treeViewTarget != null)
                //    {
                //        ThicknessMeasure measure = new ThicknessMeasure();
                //        switch (SystemParamManager.Instance.SysConfigParam.Language)
                //        {
                //            default:
                //            case "zh-CN":
                //                rootNode = this._treeViewTarget.AddItems(measure, "对射测厚");
                //                break;
                //            case "en-US":

                //                break;
                //        }

                //    }
                //    break;

                case "创建位姿3D":
                case "创建位姿3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new CreatePose3D(), "创建位姿3D");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "翻转对象3D":
                case "翻转对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new OverturnObjectModel3D(), "翻转对象3D", "RefSource1-<=输入对象3D");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "读取对象3D":
                case "读取对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new ReadObjectModel3D(), "读取对象3D");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "保存对象3D":
                case "保存对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        SaveObjectModel3D saveObjectModel3D = new SaveObjectModel3D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(saveObjectModel3D, "保存对象3D", nameof(saveObjectModel3D.DataHandle3D));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "平面度":
                case "平面度节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new Planeness(), "平面度", "RefSource1-<=输入对象3D");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "体积":
                case "体积节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new Volume(), "体积", "RefSource1-<=输入对象3D");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "厚度":
                case "厚度节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new Thickness(), "厚度", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "提取截面3D":
                case "提取截面3D节点":
                    if (this._treeViewTarget != null)
                    {
                        Section section = new Section();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(section, "提取截面3D", nameof(section.DataHandle3D), nameof(section.OutObjectModel3D));
                                break;
                            case "en-US":

                                break;
                        }

                    }

                    break;
                case "点到面距离3D":
                case "点到面距离3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new PointToFaceDist(), "点到面距离3D", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "点到线距离3D":
                case "点到线距离3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new PointToLineDist3D(), "点到线距离3D", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "点到点距离3D":
                case "点到点距离3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new PointToPointDist3D(), "点到点距离3D", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "线到线距离3D":
                case "线到线距离3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new LineToLineDist3D(), "线到线距离3D", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "面到面距离3D":
                case "面到面距离3D节点":
                    if (this._treeViewTarget != null)
                    {
                        FaceToFaceDist3D faceDist3D = new FaceToFaceDist3D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new FaceToFaceDist3D(), "面到面距离3D", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "采样对象3D":
                case "采样对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new SampleObjectModel3D(), "采样对象3D", "RefSource1-<=输入对象1");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "选择对象3D":
                case "选择对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        SelectObjectModelRange3D selectObjectModel = new SelectObjectModelRange3D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(selectObjectModel, "选择对象3D", nameof(selectObjectModel.DataObjectModel), nameof(selectObjectModel.SelectObjectModel));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "滤波对象3D":
                case "滤波对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new FilterObjectModel3D(), "滤波3D对象", "RefSource1-<=输入对象");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "分割对象3D":
                case "分割对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new SegmentObjectModle3D(), "分割3D对象", "RefSource1-<=输入对象");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "合并对象3D":
                case "合并对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new Union3DobjectModel(), "合并3D对象", "RefSource1-<=输入对象");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "平滑对象3D":
                case "平滑对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new SmoothObjectModel3D(), "平滑3D对象", "RefSource1-<=输入对象");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "平滑3D轮廓对象":
                case "平滑3D轮廓对象节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new SmoothProfileModel3D(), "平滑3D轮廓对象", "RefSource1-<=输入对象");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "坐标系-表面匹配":
                case "坐标系-表面匹配节点":
                case "表面匹配":
                case "表面匹配节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new DoSurfaceModelMatch(), "表面匹配", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2", "坐标系-=>坐标系", "场景采样点对象-=>场景采样点对象", "关键点3D对象-=>关键点3D对象", "变形采样点3D对象-=>变形采样点3D对象");
                                break;
                            case "en-US":

                                break;
                        }
                    }//选择3D对象范围
                    break;


                case "变换对象3D":
                case "变换对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new AffineObjectModel3D(), "变换对象3D", "RefSource1-<=输入对象", "RefSource2-<=坐标系");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "缩放对象3D":
                case "缩放对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new ScaleObjectModel3D(), "缩放对象", "RefSource1-<=输入对象");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "转换对象到图像3D":
                case "转换对象到图像3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new TransformObject3DToXYZImage(), "转换对象到图像3D", "RefSource1-<=输入对象", "实值图像X-=>实值图像X", "实值图像Y-=>实值图像Y", "实值图像Z-=>实值图像Z", "字节图像X-=>字节图像X", "字节图像Y-=>字节图像Y", "字节图像Z-=>字节图像Z");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "转换图像到对象3D":
                case "转换图像到对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new TransformXYZImageToObject3D(), "转换图像到对象3D", "RefSource1-<=输入图像X", "RefSource2-<=输入图像Y", "RefSource3-<=输入图像Z");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "角平分线":
                case "角平分线节点":
                    if (this._treeViewTarget != null)
                    {
                        CalculateAngleBisector angleBisector = new CalculateAngleBisector();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(angleBisector, "角平分线", nameof(angleBisector.WcsLine1), nameof(angleBisector.WcsLine2), nameof(angleBisector.WcsLine)); //, "直线对象-=>直线对象"
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(angleBisector, "AngularBisector", nameof(angleBisector.WcsLine1), nameof(angleBisector.WcsLine2), nameof(angleBisector.WcsLine)); //, "直线对象-=>直线对象"
                                break;
                        }
                    }
                    break;


                case "中分线":
                case "中分线节点":
                    if (this._treeViewTarget != null)
                    {
                        CalculateMiddleBisector middleBisector = new CalculateMiddleBisector();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(middleBisector, "中分线", nameof(middleBisector.WcsRect2), nameof(middleBisector.WcsLine)); break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "线线距离":
                case "线线距离节点":
                    if (this._treeViewTarget != null)
                    {
                        LineToLineDist2D doLineToLineDist2D = new LineToLineDist2D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(doLineToLineDist2D, "线线距离", nameof(doLineToLineDist2D.WcsLine1), nameof(doLineToLineDist2D.WcsLine2), nameof(doLineToLineDist2D.MeanDist),
                                                        nameof(doLineToLineDist2D.MaxDist), nameof(doLineToLineDist2D.MinDist));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(doLineToLineDist2D, "LineToLineDist", nameof(doLineToLineDist2D.WcsLine1), nameof(doLineToLineDist2D.WcsLine2), nameof(doLineToLineDist2D.MeanDist),
                                                               nameof(doLineToLineDist2D.MaxDist), nameof(doLineToLineDist2D.MinDist));
                                break;
                        }
                    }
                    break;

                case "点点距离":
                case "点点距离节点":
                    if (this._treeViewTarget != null)
                    {
                        PointToPointDist2D pointToPointDist2D = new PointToPointDist2D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(pointToPointDist2D, "点点距离", nameof(pointToPointDist2D.WcsPoint1), nameof(pointToPointDist2D.WcsPoint2),
                                                              nameof(pointToPointDist2D.MaxDist), nameof(pointToPointDist2D.VerticalDist), nameof(pointToPointDist2D.LevelDist));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(pointToPointDist2D, "PointToPointDist", nameof(pointToPointDist2D.WcsPoint1), nameof(pointToPointDist2D.WcsPoint2),
                                                              nameof(pointToPointDist2D.MaxDist), nameof(pointToPointDist2D.VerticalDist), nameof(pointToPointDist2D.LevelDist));
                                break;
                        }
                    }
                    break;

                case "点线距离":
                case "点线距离节点":
                    if (this._treeViewTarget != null)
                    {
                        PointToLineDist2D doPointToLineDist2D = new PointToLineDist2D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(doPointToLineDist2D, "点线距离", nameof(doPointToLineDist2D.WcsPoint), nameof(doPointToLineDist2D.WcsLine),
                                     nameof(doPointToLineDist2D.Dist));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(doPointToLineDist2D, "PointToLineDist", nameof(doPointToLineDist2D.WcsPoint), nameof(doPointToLineDist2D.WcsLine),
                                     nameof(doPointToLineDist2D.Dist));
                                break;
                        }

                    }
                    break;

                case "圆线距离":
                case "圆线距离节点":
                    if (this._treeViewTarget != null)
                    {
                        CircleToLineDist2D circleToLineDist2D = new CircleToLineDist2D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(circleToLineDist2D, "圆线距离", nameof(circleToLineDist2D.WcsCircle), nameof(circleToLineDist2D.WcsLine),
                                                              nameof(circleToLineDist2D.CircleDist), nameof(circleToLineDist2D.MaxDist), nameof(circleToLineDist2D.MinDist));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(circleToLineDist2D, "CircleToLineDist", nameof(circleToLineDist2D.WcsCircle), nameof(circleToLineDist2D.WcsLine),
                                                              nameof(circleToLineDist2D.CircleDist), nameof(circleToLineDist2D.MaxDist), nameof(circleToLineDist2D.MinDist));
                                break;
                        }
                    }
                    break;

                case "圆圆距离":
                case "圆圆距离节点":
                    if (this._treeViewTarget != null)
                    {
                        CircleToCircleDist2D circleToCircleDist2D = new CircleToCircleDist2D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(circleToCircleDist2D, "圆圆距离", nameof(circleToCircleDist2D.WcsCircle1), nameof(circleToCircleDist2D.WcsCircle2),
                                                              nameof(circleToCircleDist2D.CircleDist), nameof(circleToCircleDist2D.MaxDist), nameof(circleToCircleDist2D.MinDist));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(circleToCircleDist2D, "CircleToCircleDist", nameof(circleToCircleDist2D.WcsCircle1), nameof(circleToCircleDist2D.WcsCircle2),
                                                              nameof(circleToCircleDist2D.CircleDist), nameof(circleToCircleDist2D.MaxDist), nameof(circleToCircleDist2D.MinDist));
                                break;
                        }
                    }
                    break;

                case "点到坐标系距离":
                case "点到坐标系距离节点":
                    if (this._treeViewTarget != null)
                    {
                        PointToCoordSysDist2D pointToCoordSysDist2D = new PointToCoordSysDist2D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(pointToCoordSysDist2D, "点到坐标系距离", nameof(pointToCoordSysDist2D.WcsPoint), nameof(pointToCoordSysDist2D.WcsCoordSystem), nameof(pointToCoordSysDist2D.LevelDist), nameof(pointToCoordSysDist2D.VerticalDist));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(pointToCoordSysDist2D, "PointToCoordSysDist2D", nameof(pointToCoordSysDist2D.WcsPoint), nameof(pointToCoordSysDist2D.WcsCoordSystem), nameof(pointToCoordSysDist2D.LevelDist), nameof(pointToCoordSysDist2D.VerticalDist));
                                break;
                        }
                    }
                    break; //点到坐标系距离

                case "直线中点":
                case "直线中点节点":
                    if (this._treeViewTarget != null)
                    {
                        LineMiddlePoint lineMiddlePoint = new LineMiddlePoint();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(lineMiddlePoint, "直线中点", nameof(lineMiddlePoint.WcsLine), nameof(lineMiddlePoint.WcsVector));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(lineMiddlePoint, "LineMidPoint", nameof(lineMiddlePoint.WcsLine), nameof(lineMiddlePoint.WcsVector));
                                break;
                        }
                    }
                    break;

                case "直线角度":
                case "直线方向":
                case "直线角度节点":
                case "直线方向节点":
                    if (this._treeViewTarget != null)
                    {
                        LineAngle2D lineAngle2D = new LineAngle2D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(lineAngle2D, "LineAngle", nameof(lineAngle2D.WcsLine), nameof(lineAngle2D.ObtuseAngle));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(lineAngle2D, "直线角度", nameof(lineAngle2D.WcsLine), nameof(lineAngle2D.ObtuseAngle));
                                break;
                        }

                    }
                    break;

                case "N点到直线":
                case "N点到直线节点":
                    if (this._treeViewTarget != null)
                    {
                        NPointToLineDist2D pointToLineDist2D = new NPointToLineDist2D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(pointToLineDist2D, "N点到直线", nameof(pointToLineDist2D.WcsPoint), nameof(pointToLineDist2D.WcsLine), nameof(pointToLineDist2D.MinWcsPoint), nameof(pointToLineDist2D.MaxWcsPoint));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(pointToLineDist2D, "NPointToLineDist", nameof(pointToLineDist2D.WcsPoint), nameof(pointToLineDist2D.WcsLine), nameof(pointToLineDist2D.MinWcsPoint), nameof(pointToLineDist2D.MaxWcsPoint));
                                break;
                        }

                    }
                    break;
                case "轮廓到线距离":
                case "轮廓到线距离节点":
                    if (this._treeViewTarget != null)
                    {
                        ContToLineDist2D contToLineDist2D = new ContToLineDist2D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(contToLineDist2D, "轮廓到线距离", nameof(contToLineDist2D.WcsPoint), nameof(contToLineDist2D.WcsLine), nameof(contToLineDist2D.MinWcsPoint), nameof(contToLineDist2D.MaxWcsPoint));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(contToLineDist2D, "CountorToLineDist", nameof(contToLineDist2D.WcsPoint), nameof(contToLineDist2D.WcsLine), nameof(contToLineDist2D.MinWcsPoint), nameof(contToLineDist2D.MaxWcsPoint));
                                break;
                        }
                    }
                    break;

                case "投影点":
                case "投影点节点":
                    if (this._treeViewTarget != null)
                    {
                        ProjectionPoint projection = new ProjectionPoint();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(projection, "投影点", nameof(projection.WcsPoint1), nameof(projection.WcsLine2), nameof(projection.WcsPoint));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(projection, "ProjectionPoint", nameof(projection.WcsPoint1), nameof(projection.WcsLine2), nameof(projection.WcsPoint));
                                break;
                        }

                    }
                    break;

                case "Blob分析":
                case "Blob分析节点":
                    if (this._treeViewTarget != null)
                    {
                        Blob doBlob = new Blob();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(doBlob, "Blob分析", nameof(doBlob.ImageData), nameof(doBlob.PixCoordSystem), nameof(doBlob.BlobRegion),
                              nameof(doBlob.BinaryMeanImage), nameof(doBlob.BinaryImage), nameof(doBlob.RegionImage), nameof(doBlob.WcsPoint)); // 
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(doBlob, "Blob", nameof(doBlob.ImageData), nameof(doBlob.PixCoordSystem), nameof(doBlob.BlobRegion),
                              nameof(doBlob.BinaryMeanImage), nameof(doBlob.BinaryImage), nameof(doBlob.RegionImage), nameof(doBlob.WcsPoint)); // 
                                break;
                        }
                    }
                    break;

                case "读取图像":
                case "读取图像节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new ReadImage(), "读取图像"); //, "图像-=>图像"
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(new ReadImage(), "ReadImage"); //, "图像-=>图像"
                                break;
                        }
                    }
                    break;

                case "提取轮廓":
                case "提取轮廓节点":
                    if (this._treeViewTarget != null)
                    {
                        ExtractXLD extract = new ExtractXLD();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(extract, "提取轮廓", "RefSource1-<=输入图像"); //, "XLD轮廓-=>XLD轮廓"
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(extract, "ExtractContour", "RefSource1-<=输入图像"); //, "XLD轮廓-=>XLD轮廓"
                                break;
                        }
                    }
                    break;

                case "选择轮廓":
                case "选择轮廓节点":
                    if (this._treeViewTarget != null)
                    {
                        SelectionXLD selection = new SelectionXLD();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(selection, "选择轮廓", "RefSource1-<=输入XLD轮廓"); //, "XLD轮廓-=>XLD轮廓"
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(selection, "  SelectionContour", "RefSource1-<=输入XLD轮廓"); //, "XLD轮廓-=>XLD轮廓"
                                break;
                        }
                    }
                    break;

                case "数据输出":
                case "数据输出节点":
                    if (this._treeViewTarget != null)
                    {
                        OutputData outputData = new OutputData();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(outputData, "数据输出", nameof(outputData.OutPutContent));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(outputData, "DataOutput", nameof(outputData.OutPutContent));
                                break;
                        }
                    }
                    break;

                case "取反平移对象3D":
                case "取反平移对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new InvertTranslateObjectModel3D(), "取反平移对象3D", "RefSource1-<=输入对象");
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(new InvertTranslateObjectModel3D(), "InvertObject3D", "RefSource1-<=输入对象");
                                break;
                        }
                    }
                    break;


                case "圆形取点3D":
                case "圆形取点3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new CircleCrop(), "圆形取点", "RefSource1-<=输入对象", "RefSource2-<=坐标系");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;


                case "裁剪对象3D":
                case "裁剪对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        CropObjectModel3D cropObject = new CropObjectModel3D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(cropObject, "裁剪对象3D", nameof(cropObject.DataHandle3D), nameof(cropObject.WcsCoordSystem), nameof(cropObject.CropObjectModel));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "创建基本体3D":
                case "创建基本体3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new CreatePrimitive3D(), "创建基本体3D", "RefSource1-<=输入位姿"); //, "3D对象-=>3D对象"
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "轮廓变换":
                case "轮廓变换节点":
                case "轨迹变换":
                case "轨迹变换节点":
                    if (this._treeViewTarget != null)
                    {
                        ContourAffine affine = new ContourAffine();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(affine, "轨迹变换", nameof(affine.WcsPoint), nameof(affine.WcsCoordSystem), nameof(affine.WcsTargetPoint));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "保存图像":
                case "保存图像节点":
                    if (this._treeViewTarget != null)
                    {
                        SaveImage saveImage = new SaveImage();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(saveImage, "保存图像", nameof(saveImage.ImageData), nameof(saveImage.InputRegion), nameof(saveImage.SavePath));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(saveImage, "SaveImage", nameof(saveImage.ImageData), nameof(saveImage.InputRegion), nameof(saveImage.SavePath));
                                break;
                        }

                    }
                    break;

                case "保存轮廓":
                case "保存轮廓节点":
                    if (this._treeViewTarget != null)
                    {
                        SaveContXLD saveXLDCont = new SaveContXLD();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(saveXLDCont, "保存轮廓", nameof(saveXLDCont.XldContData)); //, "XLD轮廓-=>XLD轮廓"
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;


                case "轮廓裁剪":
                case "轮廓裁剪节点":
                    if (this._treeViewTarget != null)
                    {
                        ContourReduce contourReduce = new ContourReduce();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(contourReduce, "轮廓裁剪", nameof(ContourReduce.WcsPolyLine), nameof(ContourReduce.WcsCoordSystem), nameof(ContourReduce.ReduceContour));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(contourReduce, "ContourReduce", nameof(ContourReduce.WcsPolyLine), nameof(ContourReduce.WcsCoordSystem), nameof(ContourReduce.ReduceContour));
                                break;
                        }
                    }
                    break;

                case "读取轮廓":
                case "读取轮廓节点":
                    if (this._treeViewTarget != null)
                    {
                        ReadContourXLD readXLD = new ReadContourXLD();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(readXLD, "读取轮廓", nameof(readXLD.XldContour)); //, "XLD轮廓-=>XLD轮廓"
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "采样轮廓":
                case "采样轮廓节点":
                    if (this._treeViewTarget != null)
                    {
                        SampleXLD sampleXLD = new SampleXLD();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(sampleXLD, "采样轮廓", "RefSource1-<=输入轮廓XLD", "像素采样点-=>像素采样点", "世界采样点-=>世界采样点");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "计算对象位姿3D":
                case "计算对象位姿3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new GetPrimitive3DPose(), "计算对象位姿3D", "RefSource1-<=输入对象");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "校平对象3D":
                case "校平对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        RectifyObjectModel3D rectifyObject = new RectifyObjectModel3D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new RectifyObjectModel3D(), "校平对象3D", nameof(rectifyObject.DataHandle3D), nameof(rectifyObject.WcsCoordSystem), nameof(rectifyObject.RectifyHandle3D),
                                   nameof(rectifyObject.Pose));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "平面相交对象3D":
                case "平面相交对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new IntersectPlaneObjectModel3D(), "平面相交对象3D", "RefSource1-<=平面对象", "RefSource2-<=输入对象");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "变换位姿3D":
                case "变换位姿3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new AffinePose3D(), "变换位姿3D", "RefSource1-<=输入坐标系");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "获取极值轮廓":
                case "获取极值轮廓节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new GetMinMaxValueObjectModel(), "获取极值轮廓", "RefSource1-<=输入3D对象", "RefSource2-<=参考直线", "3D对象-=>3D对象", "直线对象-=>直线对象");
                                break;
                            case "en-US":

                                break;
                        }

                    }// 减少3D对象定义域
                    break;

                case "线线交点":
                case "线线交点节点":
                    if (this._treeViewTarget != null)
                    {
                        IntersectionLineLine intersectionLineLine = new IntersectionLineLine();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(intersectionLineLine, "线线交点", nameof(intersectionLineLine.WcsLine1), nameof(intersectionLineLine.WcsLine2), nameof(intersectionLineLine.WcsVector));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(intersectionLineLine, "LineLineIntersect", nameof(intersectionLineLine.WcsLine1), nameof(intersectionLineLine.WcsLine2), nameof(intersectionLineLine.WcsVector));
                                break;
                        }
                    }
                    break;

                case "线圆交点":
                case "线圆交点节点":
                    if (this._treeViewTarget != null)
                    {
                        IntersectionLineCirlce intersectionLineCirlce = new IntersectionLineCirlce();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(intersectionLineCirlce, "线圆交点", nameof(intersectionLineCirlce.WcsLine), nameof(intersectionLineCirlce.WcsCircle), nameof(intersectionLineCirlce.WcsPoints));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "圆圆交点":
                case "圆圆交点节点":
                    if (this._treeViewTarget != null)
                    {
                        IntersectionCircleCircle intersectionCircleCircle = new IntersectionCircleCircle();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(intersectionCircleCircle, "线圆交点", nameof(intersectionCircleCircle.WcsCircle1), nameof(intersectionCircleCircle.WcsCircle2), nameof(intersectionCircleCircle.WcsPoints));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "直线轮廓交点":
                case "直线轮廓交点节点":
                    if (this._treeViewTarget != null)
                    {
                        IntersectionLineContour intersectionLineContour = new IntersectionLineContour();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(intersectionLineContour, "直线轮廓交点", nameof(intersectionLineContour.WcsLine), nameof(intersectionLineContour.WcsPolyLine), nameof(intersectionLineContour.WcsPoints));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(intersectionLineContour, "IntersectionLineContour", nameof(intersectionLineContour.WcsLine), nameof(intersectionLineContour.WcsPolyLine), nameof(intersectionLineContour.WcsPoints));
                                break;
                        }
                    }
                    break;

                case "线线夹角":
                case "线线夹角节点":
                    if (this._treeViewTarget != null)
                    {
                        AngleLineLine2D angleLineLine2D = new AngleLineLine2D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(angleLineLine2D, "线线夹角", nameof(angleLineLine2D.WcsLine1), nameof(angleLineLine2D.WcsLine2), nameof(angleLineLine2D.Deg));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(angleLineLine2D, "LineLineAngle", nameof(angleLineLine2D.WcsLine1), nameof(angleLineLine2D.WcsLine2), nameof(angleLineLine2D.Deg));
                                break;
                        }
                    }
                    break;

                case "显示多个对象3D":
                case "显示多个对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new ShowMultipleObjects(), "显示多个对象3D", "RefSource1-<=输入3D对象");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "直线夹角3D":
                case "直线夹角3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new AngleLineLine3D(), "直线夹角3D", "RefSource1-<=输入3D对象1", "RefSource2-<=输入3D对象2");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "直线方向3D":
                case "直线方向3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new LineOrientation3D(), "直线方向3D", "RefSource1-<=输入3D对象", "角度X-=>角度X", "角度Y-=>角度Y", "角度Z-=>角度Z");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "直线长度3D": // 这个算子已经没意义了
                case "直线长度3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new LineLength3D(), "直线长度3D", "RefSource1-<=输入3D对象");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "定点采样对象3D": // 这个算子已经没意义了
                case "定点采样对象3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new FixedPointSampleModel(), "定点采样对象3D", "RefSource1-<=输入对象", "RefSource2-<=输入采样点");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;


                case "渲染对象到图像3D":
                case "渲染对象到图像3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new RenderObjectModel3DToImage(), "渲染对象到图像3D", "RefSource1-<=输入对象", "图像1-=>图像1", "图像2-=>图像2", "图像3-=>图像3");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;


                case "坐标系-点线":
                case "坐标系_点线":
                case "坐标系-点线节点":
                case "坐标系_点线节点":
                    if (this._treeViewTarget != null)
                    {
                        CoordSystemPointLine coordSystemPointLine = new CoordSystemPointLine();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(coordSystemPointLine, coordSystemPointLine.Name, nameof(coordSystemPointLine.WcsPoint), nameof(coordSystemPointLine.WcsLine), nameof(coordSystemPointLine.WcsCoordSystem));   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系  坐标系(点-线)
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;
                case "坐标系-线":
                case "坐标系_线":
                case "坐标系-线节点":
                case "坐标系_线节点":
                    if (this._treeViewTarget != null)
                    {
                        CoordSystemLine coordSystemLine = new CoordSystemLine();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(coordSystemLine, coordSystemLine.Name, nameof(coordSystemLine.WcsLine), nameof(coordSystemLine.WcsCoordSystem));   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系  坐标系(点-线)
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;
                case "坐标系-线线":
                case "坐标系_线线":
                case "坐标系-线线节点":
                case "坐标系_线线节点":
                    if (this._treeViewTarget != null)
                    {
                        CoordSystemLineLine coordSystemLineLine = new CoordSystemLineLine();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(coordSystemLineLine, coordSystemLineLine.Name, nameof(coordSystemLineLine.WcsLine1), nameof(coordSystemLineLine.WcsLine2), nameof(coordSystemLineLine.WcsCoordSystem));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;
                case "坐标系-圆线":
                case "坐标系_圆线":
                case "坐标系-圆线节点":
                case "坐标系_圆线节点":
                    if (this._treeViewTarget != null)
                    {
                        CoordSystemCircleLine coordSystemCircleLine = new CoordSystemCircleLine();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(coordSystemCircleLine, coordSystemCircleLine.Name, nameof(coordSystemCircleLine.WcsCircle), nameof(coordSystemCircleLine.WcsLine), nameof(coordSystemCircleLine.WcsCoordSystem));   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系  坐标系(点-线)
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;
                case "坐标系-矩形":
                case "坐标系_矩形":
                case "坐标系-矩形节点":
                case "坐标系_矩形节点":
                    if (this._treeViewTarget != null)
                    {
                        CoordSystemRect2 coordSystemRect2 = new CoordSystemRect2();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(coordSystemRect2, coordSystemRect2.Name, nameof(coordSystemRect2.WcsRect2), nameof(coordSystemRect2.WcsCoordSystem));   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系  坐标系(点-线)
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "拟合平面3D":
                case "拟合平面3D节点":
                    if (this._treeViewTarget != null)
                    {
                        FittingPlane3D fittingPlane = new FittingPlane3D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(fittingPlane, "拟合平面3D", nameof(fittingPlane.DataObjectModel), nameof(fittingPlane.WcsPlane), nameof(fittingPlane.WcsPose));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;
                case "拟合圆柱3D":
                case "拟合圆柱3D节点":
                    if (this._treeViewTarget != null)
                    {
                        FittingCylinder3D fittingCylinder = new FittingCylinder3D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(fittingCylinder, "拟合圆柱3D", nameof(fittingCylinder.DataObjectModel), nameof(fittingCylinder.WcsCylinder), nameof(fittingCylinder.WcsPose));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;
                case "拟合球体3D":
                case "拟合球体3D节点":
                    if (this._treeViewTarget != null)
                    {
                        FittingSphere3D fittingSphere = new FittingSphere3D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(fittingSphere, "拟合球体3D", nameof(fittingSphere.DataObjectModel), nameof(fittingSphere.WcsSphere), nameof(fittingSphere.WcsPose));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;
                case "拟合盒子3D":
                case "拟合盒子3D节点":
                    if (this._treeViewTarget != null)
                    {
                        FittingBox3D fittingBox = new FittingBox3D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(fittingBox, "拟合盒子3D", nameof(fittingBox.DataObjectModel), nameof(fittingBox.WcsBox), nameof(fittingBox.WcsPose));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break; //Y

                case "拟合轮廓圆3D":
                case "拟合轮廓圆3D节点":
                    if (this._treeViewTarget != null)
                    {
                        FitProfileCircle3D fitProfileCircle = new FitProfileCircle3D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(fitProfileCircle, "拟合轮廓圆3D", nameof(fitProfileCircle.DataObjectModel), nameof(fitProfileCircle.WcsCircle));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break; //拟合3D轮廓圆

                case "面面夹角3D":
                case "面面夹角3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new FaceToFaceAngle(), "面面夹角3D", "RefSource1-<=输入平面1", "RefSource2-<=输入平面2");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "直线度":
                case "直线度3D":
                case "直线度节点":
                case "直线度3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new Straightness3D(), "直线度3D", "RefSource1-<=输入3D对象");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "线轮廓度3D":
                case "线轮廓度3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new ProfileTolerance(), "线轮廓度3D", "RefSource1-<=测量3D对象", "RefSource2-<=参考3D对象");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "极值点3D":
                case "极值点3D节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new ExtremePoint3D(), "极值点3D", "RefSource1-<=输入对象3D", "3D对象-=>3D对象");
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;


                #region 2D 拟合

                case "拟合直线":
                case "拟合直线节点":
                    if (this._treeViewTarget != null)
                    {
                        FitLine fitLine = new FitLine();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(fitLine, "拟合直线", nameof(fitLine.WcsPoint), nameof(fitLine.WcsLine));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(fitLine, "FitLine", nameof(fitLine.WcsPoint), nameof(fitLine.WcsLine));
                                break;
                        }
                    }
                    break;

                case "拟合圆":
                case "拟合圆节点":
                    if (this._treeViewTarget != null)
                    {
                        FitCircle fitCircle = new FitCircle();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(fitCircle, "拟合圆", nameof(fitCircle.WcsPoint), nameof(fitCircle.WcsCircle));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break; // 拟合圆弧

                case "拟合圆弧":
                case "拟合圆弧节点":
                    if (this._treeViewTarget != null)
                    {
                        FitCircleSector fitCircleSector = new FitCircleSector();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(fitCircleSector, "拟合圆弧", nameof(fitCircleSector.WcsPoint), nameof(fitCircleSector.WcsCircleSector));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(fitCircleSector, "FitCircleSector", nameof(fitCircleSector.WcsPoint), nameof(fitCircleSector.WcsCircleSector));
                                break;
                        }
                    }
                    break; // 拟合圆弧

                case "拟合椭圆":
                case "拟合椭圆节点":
                    if (this._treeViewTarget != null)
                    {
                        FitEllipse fitEllipse = new FitEllipse();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(fitEllipse, "拟合椭圆", nameof(fitEllipse.WcsPoint), nameof(fitEllipse.WcsEllipse));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break; // N点拟合圆

                case "拟合矩形":
                case "拟合矩形节点":
                    if (this._treeViewTarget != null)
                    {
                        FitRect2 fitRect2 = new FitRect2();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(fitRect2, "拟合矩形", nameof(fitRect2.WcsPoint), nameof(fitRect2.WcsRect2));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break; // N点拟合圆


                //case "N点拟合圆":
                //case "N点拟合圆节点":
                //    if (this._treeViewTarget != null)
                //    {
                //        NPointsFitCircle nPointsFitCircle = new NPointsFitCircle();
                //        switch (SystemParamManager.Instance.SysConfigParam.Language)
                //        {
                //            default:
                //            case "zh-CN":
                //                rootNode = this._treeViewTarget.AddItems(nPointsFitCircle, "N点拟合圆", nameof(nPointsFitCircle.WcsPoint), nameof(nPointsFitCircle.Circle));
                //                break;
                //            case "en-US":

                //                break;
                //        }
                //    }
                //    break; // 

                //case "N点拟合椭圆":
                //case "N点拟合椭圆节点":
                //    if (this._treeViewTarget != null)
                //    {
                //        NPointsFitEllipse nPointsFitEllipse = new NPointsFitEllipse();
                //        switch (SystemParamManager.Instance.SysConfigParam.Language)
                //        {
                //            default:
                //            case "zh-CN":
                //                rootNode = this._treeViewTarget.AddItems(nPointsFitEllipse, "N点拟合椭圆", nameof(nPointsFitEllipse.WcsPoint), nameof(nPointsFitEllipse.Ellipse));
                //                break;
                //            case "en-US":

                //                break;
                //        }
                //    }
                //    break;

                case "NPointFitLine":
                    if (this._treeViewTarget != null)
                    {
                        NPointsFitLine nPointsFitLine = new NPointsFitLine();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(nPointsFitLine, "NPointFitLine", nameof(nPointsFitLine.WcsPoint), nameof(nPointsFitLine.Line));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;
                case "N点拟合直线":
                case "N点拟合直线节点":
                    if (this._treeViewTarget != null)
                    {
                        NPointsFitLine nPointsFitLine = new NPointsFitLine();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(nPointsFitLine, "N点拟合直线", nameof(nPointsFitLine.WcsPoint), nameof(nPointsFitLine.Line));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;
                #endregion

                case "点": //
                case "点节点": //
                    if (this._treeViewTarget != null)
                    {
                        PointMeasure pointMeasure = new PointMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(pointMeasure, "点", nameof(pointMeasure.ImageData), nameof(pointMeasure.PixCoordSystem), nameof(pointMeasure.WcsPoint)); //, "点坐标-=>点坐标", nameof(pointMeasure.Result)
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(pointMeasure, "Point", nameof(pointMeasure.ImageData), nameof(pointMeasure.PixCoordSystem), nameof(pointMeasure.WcsPoint)); //, "点坐标-=>点坐标", nameof(pointMeasure.Result)
                                break;
                        }
                    }
                    break;

                case "十字取点": //
                case "十字取点节点": //
                    if (this._treeViewTarget != null)
                    {
                        CrossPointMeasure pointCrossMeasure = new CrossPointMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(pointCrossMeasure, "十字取点", nameof(pointCrossMeasure.ImageData), nameof(pointCrossMeasure.PixCoordSystem), nameof(pointCrossMeasure.WcsPoint)); //, "点坐标-=>点坐标", nameof(pointMeasure.Result)
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "线":
                case "线节点":
                    if (this._treeViewTarget != null)
                    {
                        LineMeasure lineMeasure = new LineMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(lineMeasure, "线", nameof(lineMeasure.ImageData), nameof(lineMeasure.PixCoordSystem), nameof(lineMeasure.WcsLine)); //, nameof(lineMeasure.Result)
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(lineMeasure, "Line", nameof(lineMeasure.ImageData), nameof(lineMeasure.PixCoordSystem), nameof(lineMeasure.WcsLine)); //, nameof(lineMeasure.Result)
                                break;
                        }
                    }
                    break;

                case "圆":
                case "圆节点":
                    if (this._treeViewTarget != null)
                    {
                        CircleMeasure circleMeasure = new CircleMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(circleMeasure, "圆", nameof(circleMeasure.ImageData), nameof(circleMeasure.PixCoordSystem), nameof(circleMeasure.WcsCircle)); //, nameof(circleMeasure.Result)
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(circleMeasure, "FindCircle", nameof(circleMeasure.ImageData), nameof(circleMeasure.PixCoordSystem), nameof(circleMeasure.WcsCircle)); //, nameof(circleMeasure.Result)
                                break;
                        }
                    }
                    break;

                case "圆弧":
                case "圆弧节点":
                    if (this._treeViewTarget != null)
                    {
                        CircleSectorMeasure circleSectorMeasure = new CircleSectorMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(circleSectorMeasure, "圆弧", nameof(circleSectorMeasure.ImageData), nameof(circleSectorMeasure.PixCoordSystem), nameof(circleSectorMeasure.WcsCircleSector)); //, nameof(circleSectorMeasure.Result)
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(circleSectorMeasure, "FindCircleSector", nameof(circleSectorMeasure.ImageData), nameof(circleSectorMeasure.PixCoordSystem), nameof(circleSectorMeasure.WcsCircleSector)); //, nameof(circleSectorMeasure.Result)
                                break;
                        }
                    }
                    break;

                case "椭圆":
                case "椭圆节点":
                    if (this._treeViewTarget != null)
                    {
                        EllipseMeasure ellipseMeasure = new EllipseMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(ellipseMeasure, "椭圆", nameof(ellipseMeasure.ImageData), nameof(ellipseMeasure.PixCoordSystem), nameof(ellipseMeasure.WcsEllipse)); //, nameof(ellipseMeasure.Result)
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(ellipseMeasure, "FindEllipse", nameof(ellipseMeasure.ImageData), nameof(ellipseMeasure.PixCoordSystem), nameof(ellipseMeasure.WcsEllipse)); //, nameof(ellipseMeasure.Result)
                                break;
                        }
                    }
                    break;

                case "椭圆弧":
                case "椭圆弧节点":
                    if (this._treeViewTarget != null)
                    {
                        EllipseSectorMeasure ellipseSectorMeasure = new EllipseSectorMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(ellipseSectorMeasure, "椭圆弧", nameof(ellipseSectorMeasure.ImageData), nameof(ellipseSectorMeasure.PixCoordSystem), nameof(ellipseSectorMeasure.WcsEllipseSector)); //, nameof(ellipseSectorMeasure.Result)
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(ellipseSectorMeasure, "FindEllipseSector", nameof(ellipseSectorMeasure.ImageData), nameof(ellipseSectorMeasure.PixCoordSystem), nameof(ellipseSectorMeasure.WcsEllipseSector)); //, nameof(ellipseSectorMeasure.Result)
                                break;
                        }
                    }
                    break;

                case "矩形":
                case "矩形节点":
                    if (this._treeViewTarget != null)
                    {
                        Rectangle2Measure rect2Measure = new Rectangle2Measure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(rect2Measure, "矩形", nameof(rect2Measure.ImageData), nameof(rect2Measure.PixCoordSystem));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(rect2Measure, "FindRect2", nameof(rect2Measure.ImageData), nameof(rect2Measure.PixCoordSystem));
                                break;
                        }
                    }
                    break;

                case "宽度":
                case "宽度节点":
                    if (this._treeViewTarget != null)
                    {
                        WidthMeasure width = new WidthMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(width, "宽度", nameof(width.ImageData), nameof(width.PixCoordSystem), nameof(width.EdgeLine1), nameof(width.EdgeLine2));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(width, "WidthMeasure", nameof(width.ImageData), nameof(width.PixCoordSystem), nameof(width.EdgeLine1), nameof(width.EdgeLine2));
                                break;
                        }

                    }
                    break;

                case "多段线":
                case "多段线节点":
                    if (this._treeViewTarget != null)
                    {
                        PolyLineMeasure polyLine = new PolyLineMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(polyLine, "多段线", nameof(polyLine.ImageData), nameof(polyLine.PixCoordSystem));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(polyLine, "PolyLine", nameof(polyLine.ImageData), nameof(polyLine.PixCoordSystem));
                                break;
                        }

                    }
                    break;

                case "多边形":
                case "多边形节点":
                    if (this._treeViewTarget != null)
                    {
                        PolygonMeasure Polygon = new PolygonMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(Polygon, "多边形", nameof(Polygon.ImageData), nameof(Polygon.PixCoordSystem));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(Polygon, "Polygon", nameof(Polygon.ImageData), nameof(Polygon.PixCoordSystem));
                                break;
                        }

                    }
                    break;

                case "手动取点":
                case "手动取点节点":
                    if (this._treeViewTarget != null)
                    {
                        ManualPointMeasure manualPoint = new ManualPointMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(manualPoint, "手动取点", nameof(manualPoint.ImageData), nameof(manualPoint.PixCoordSystem));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(manualPoint, "ManualPointMeasure", nameof(manualPoint.ImageData), nameof(manualPoint.PixCoordSystem));
                                break;
                        }

                    }
                    break;

                case "手动多段线":
                case "手动多段线节点":
                    if (this._treeViewTarget != null)
                    {
                        ManualPolyLineMeasure manualPolyLine = new ManualPolyLineMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(manualPolyLine, "手动多段线", nameof(manualPolyLine.ImageData), nameof(manualPolyLine.PixCoordSystem));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(manualPolyLine, "ManualPolyLineMeasure", nameof(manualPolyLine.ImageData), nameof(manualPolyLine.PixCoordSystem));
                                break;
                        }
                    }
                    break;

                case "手动多边形":
                case "手动多边形节点":
                    if (this._treeViewTarget != null)
                    {
                        ManualPolygonMeasure manualPolygon = new ManualPolygonMeasure(ProgramForm.Instance.AcqSourceName,null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(manualPolygon, "手动多边形", nameof(manualPolygon.ImageData), nameof(manualPolygon.PixCoordSystem));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(manualPolygon, "ManualPolygonMeasure", nameof(manualPolygon.ImageData), nameof(manualPolygon.PixCoordSystem));
                                break;
                        }
                    }
                    break;

                case "手动圆弧":
                case "手动圆弧节点":
                    if (this._treeViewTarget != null)
                    {
                        ManualCircleSectorMeasure manualCircleSector = new ManualCircleSectorMeasure(ProgramForm.Instance.AcqSourceName, null);
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(manualCircleSector, "手动圆弧", nameof(manualCircleSector.ImageData), nameof(manualCircleSector.PixCoordSystem));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(manualCircleSector, "ManualCircleSectorMeasure", nameof(manualCircleSector.ImageData), nameof(manualCircleSector.PixCoordSystem));
                                break;
                        }
                    }
                    break;

                case "数值计算":
                case "数值计算节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new NumericalCalculation(), "数值计算", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "窗口图像":
                case "窗口图像节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new GetHWindowImage(), "窗口图像");   // 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "点位运动":
                case "点位运动节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new PointMove(), "点位运动");   // 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "直线插补":
                case "直线插补节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new LineInterpolationMove(MotionControlCard.MotionCardManage.CurrentCard), "直线插补");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "螺旋线插补":
                case "螺旋线插补节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new SpiralLineInterpolationMove(MotionControlCard.MotionCardManage.CurrentCard), "螺旋线插补");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "矩形插补":
                case "矩形插补节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new RectangleInterpolationMove(MotionControlCard.MotionCardManage.CurrentCard), "矩形插补");
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "轨迹运动":
                case "轨迹运动节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new TrackMove(), "轨迹运动");   // 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "写入IO输出位控制":
                case "写入IO输出位控制节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new WriteIoOutputBit(MotionControlCard.MotionCardManage.CurrentCard), "写入IO输出位控制");   // 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "写入IO输出组控制":
                case "写入IO输出组控制节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new WriteIoOutputGroup(MotionControlCard.MotionCardManage.CurrentCard), "写入IO输出组控制");   // 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "读取IO输入位控制":
                case "读取IO输入位控制节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new ReadIoInputBit(MotionControlCard.MotionCardManage.CurrentCard), "读取IO输入位控制");   // 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "读取IO输出位控制":
                case "读取IO输出位控制节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new ReadIoOutputBit(MotionControlCard.MotionCardManage.CurrentCard), "读取IO输出位控制");   // 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "读取IO输出组控制":
                case "读取IO输出组控制节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new ReadIoOutputGroup(MotionControlCard.MotionCardManage.CurrentCard), "读取IO输出组控制");   // 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "读取IO输入组控制":
                case "读取IO输入组控制节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new ReadIoIntputGroup(MotionControlCard.MotionCardManage.CurrentCard), "读取IO输入组控制");   // 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
              
                case "LineOffset":
                case "直线偏置":
                case "直线偏置节点":
                    if (this._treeViewTarget != null)
                    {
                        LineOffset offset = new LineOffset();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(offset, "直线偏置", nameof(offset.WcsLine), nameof(offset.OffsetWcsLine));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(offset, "LineOffset", nameof(offset.WcsLine), nameof(offset.OffsetWcsLine));   // , "
                                break;
                        }

                    }
                    break;

                case "轮廓偏置":
                case "轮廓偏置节点":
                    if (this._treeViewTarget != null)
                    {
                        ContourOffset offset = new ContourOffset();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(offset, "轮廓偏置", nameof(offset.WcsPoint), nameof(offset.OffsetDist), nameof(offset.WcsPolyLine));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "直线延伸":
                case "直线延伸节点":
                    if (this._treeViewTarget != null)
                    {
                        LineExtend lineExtend = new LineExtend();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(lineExtend, "直线延伸", nameof(lineExtend.WcsLine), nameof(lineExtend.ExtendWcsLine));   // , "
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(lineExtend, "LineExtend", nameof(lineExtend.WcsLine), nameof(lineExtend.ExtendWcsLine));   // , "
                                break;
                        }

                    }
                    break;

                case "直线截取":
                case "直线截取节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new LineCrop(), "直线截取", "RefSource1-<=输入3D对象", "RefSource2-<=输入直线对象");   // , 
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;  // 

                case "获取标定Mark点":
                case "获取标定Mark点节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new GetCalibrateMarkPoint(), "获取标定Mark点", "RefSource1-<=输入对象");    //, 
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;  // 


                case "二维码识别":
                case "二维码识别节点":
                    if (this._treeViewTarget != null)
                    {
                        DataCodeDetection dataCodeDetection = new DataCodeDetection();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(dataCodeDetection, "二维码识别", nameof(dataCodeDetection.ImageData), nameof(dataCodeDetection.DataCodeContent));    //, 
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;  // 

                case "条形码识别":
                case "条形码识别节点":
                    if (this._treeViewTarget != null)
                    {
                        BarCodeDetection barCodeDetection = new BarCodeDetection();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(barCodeDetection, "条形码识别", nameof(barCodeDetection.ImageData), nameof(barCodeDetection.DataCodeContent));    //, 
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;  // 

                case "LightConfig":
                case "光源配置":
                case "光源配置节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new LightConfig(), "光源配置");   // 
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(new LightConfig(), "LightConfig");   //
                                break;
                        }
                    }
                    break;

                case "直线插值":
                case "直线插值节点":
                    if (this._treeViewTarget != null)
                    {
                        LineInterpolation lineInterpolation = new LineInterpolation();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(lineInterpolation, "直线插值", nameof(lineInterpolation.WcsLine), nameof(lineInterpolation.WcsPoints));   // 
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "圆弧插值":
                case "圆弧插值节点":
                    if (this._treeViewTarget != null)
                    {
                        CircleSectorInterpolation circleSectorInterpolation = new CircleSectorInterpolation();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(circleSectorInterpolation, "圆弧插值", nameof(circleSectorInterpolation.WcsCircle), nameof(circleSectorInterpolation.WcsPoints));   //
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "鼠标点击":
                case "鼠标点击节点":
                    if (this._treeViewTarget != null)
                    {
                        MousesClick mouseClick = new MousesClick();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(mouseClick, "鼠标点击", nameof(mouseClick.WcsPoint));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(mouseClick, "MouseClick", nameof(mouseClick.WcsPoint));
                                break;
                        }
                    }
                    break;

                case "设置光标":
                case "设置光标节点":
                    if (this._treeViewTarget != null)
                    {
                        SetCursor setCursor = new SetCursor();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(setCursor, "设置光标");
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(setCursor, "SetCursor");
                                break;
                        }
                    }
                    break;

                case "读取文本":
                case "读取文本节点":
                    if (this._treeViewTarget != null)
                    {
                        ReadText readText = new ReadText();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(readText, "读取文本");
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(readText, "ReadText");
                                break;
                        }
                    }
                    break;

                case "等待文本":
                case "等待文本节点":
                    if (this._treeViewTarget != null)
                    {
                        WaitText waitText = new WaitText();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(waitText, "等待文本");
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(waitText, "WaitText");
                                break;
                        }
                    }
                    break;

                case "写入文本":
                case "写入文本节点":
                    if (this._treeViewTarget != null)
                    {
                        WriteText writeText = new WriteText();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(writeText, "写入文本", nameof(writeText.Content));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(writeText, "WriteText", nameof(writeText.Content));
                                break;
                        }
                    }
                    break;

                case "键盘点击":
                case "键盘点击节点":
                    if (this._treeViewTarget != null)
                    {
                        KeyBoardClick boardClick = new KeyBoardClick();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(boardClick, "键盘点击");
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(boardClick, "KeyBoardClick");
                                break;
                        }
                    }
                    break;

                case "坐标转换":
                case "坐标转换节点":
                    if (this._treeViewTarget != null)
                    {
                        CoordTransform coordTransform = new CoordTransform();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(coordTransform, "坐标转换", nameof(coordTransform.WcsPoint), nameof(coordTransform.WcsPolyLine));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(coordTransform, "CoordTransform", nameof(coordTransform.WcsPoint), nameof(coordTransform.WcsPolyLine));
                                break;
                        }
                    }
                    break;

                case "Wafer寻晶":
                case "Wafer寻晶节点":
                    if (this._treeViewTarget != null)
                    {
                        WaferFindDie waferFindDie = new WaferFindDie();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(waferFindDie, "Wafer寻晶", nameof(waferFindDie.RefWcsPoint), nameof(waferFindDie.SearchWcsPoint), nameof(waferFindDie.WcsOutPoint));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(waferFindDie, "WaferFindDie", nameof(waferFindDie.RefWcsPoint), nameof(waferFindDie.SearchWcsPoint), nameof(waferFindDie.WcsOutPoint));
                                break;
                        }
                    }
                    break;


                case "导圆角":
                case "导圆角节点":
                    if (this._treeViewTarget != null)
                    {
                        RoundCorners roundCorners = new RoundCorners();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(roundCorners, "导圆角", nameof(roundCorners.WcsLine1), nameof(roundCorners.WcsLine2), nameof(roundCorners.WcsPolyLine));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(roundCorners, "RoundCorners", nameof(roundCorners.WcsLine1), nameof(roundCorners.WcsLine2), nameof(roundCorners.WcsPolyLine));
                                break;
                        }
                    }
                    break;

                #region  贴合对位功能
                case "参考位示教":
                case "参考位示教节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new ReferenceTeach(), "参考位示教");
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(new ReferenceTeach(), "ReferenceTeach");
                                break;
                        }

                    }
                    break;  // 


                case "特征定位":
                case "特征定位节点":
                    if (this._treeViewTarget != null)
                    {
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(new FeatureLocalization(), "特征定位");
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(new FeatureLocalization(), "FeatureLocalization");
                                break;
                        }

                    }
                    break;  // 


                //case "胶枪引导":
                //case "胶枪引导节点":
                //    switch (SystemParamManager.Instance.SysConfigParam.Language)
                //    {
                //        default:
                //        case "zh-CN":
                //            rootNode = this._treeViewTarget.AddItems(new GlueAlignGuided(), "胶枪引导"); // 
                //            break;
                //        case "en-US":

                //            break;
                //    }

                //    break;  // 

                //case "对齐计算":
                //case "对齐计算节点":
                //    AlignmentGuidedCalculation alignmentGuided = new AlignmentGuidedCalculation();
                //    switch (SystemParamManager.Instance.SysConfigParam.Language)
                //    {
                //        default:
                //        case "zh-CN":
                //            rootNode = this._treeViewTarget.AddItems(alignmentGuided, "对齐计算", nameof(alignmentGuided.RefPoint), nameof(alignmentGuided.CurPoint), nameof(alignmentGuided.AddXYTheta));
                //            break;
                //        case "en-US":

                //            break;
                //    }

                //    break;  // 

                case "并发执行":
                case "并发执行节点":
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(new ConcurrentExecution(), "并发执行");
                            break;
                        case "en-US":

                            break;
                    }

                    break;  // 

                case "提取点":
                case "提取点节点":
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(new ExtractPoint(), "提取点", "RefSource1-<=输入对象");
                            break;
                        case "en-US":

                            break;
                    }

                    break;  // 

                //case "计算偏移":
                //case "计算偏移节点":
                //    CalculateOffsetValue calculateOffset = new CalculateOffsetValue();
                //    switch (SystemParamManager.Instance.SysConfigParam.Language)
                //    {
                //        default:
                //        case "zh-CN":
                //            rootNode = this._treeViewTarget.AddItems(calculateOffset, "计算偏移", nameof(calculateOffset.RefPoint), nameof(calculateOffset.CurPoint), nameof(calculateOffset.OffsetXYTheta));
                //            break;
                //        case "en-US":

                //            break;
                //    }

                //    break;  //

                case "循环控制":
                case "循环控制节点":
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(new ForLoopControl(), "循环控制");
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //

                //case "数据读取":
                //case "数据读取节点":
                //    DataRead dataRead = new DataRead();
                //    switch (SystemParamManager.Instance.SysConfigParam.Language)
                //    {
                //        default:
                //        case "zh-CN":
                //            rootNode = this._treeViewTarget.AddItems(dataRead, "数据读取", nameof(dataRead.ReadContent));
                //            break;
                //        case "en-US":

                //            break;
                //    }

                //    break;  //

                //case "数据写入":
                //case "数据写入节点":
                //    DataWrite dataWrite = new DataWrite();
                //    switch (SystemParamManager.Instance.SysConfigParam.Language)
                //    {
                //        default:
                //        case "zh-CN":
                //            rootNode = this._treeViewTarget.AddItems(dataWrite, "数据写入", nameof(dataWrite.WriteData));
                //            break;
                //        case "en-US":

                //            break;
                //    }

                //    break;  //

                case "数据等待":
                case "数据等待节点":
                    DataWaitePlc dataWaite = new DataWaitePlc();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(dataWaite, "数据等待", nameof(dataWaite.WaiteContent));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //

                //case "胶路检测":
                //case "胶路检测节点":
                //    GlueDetect glueDetect = new GlueDetect();
                //    switch (SystemParamManager.Instance.SysConfigParam.Language)
                //    {
                //        default:
                //        case "zh-CN":
                //            rootNode = this._treeViewTarget.AddItems(glueDetect, "胶路检测");
                //            break;
                //        case "en-US":

                //            break;
                //    }

                //    break;  //统计结果

                //case "读取文件":
                //case "读取文件节点":
                //    ReadFile readFile = new ReadFile();
                //    switch (SystemParamManager.Instance.SysConfigParam.Language)
                //    {
                //        default:
                //        case "zh-CN":
                //            rootNode = this._treeViewTarget.AddItems(readFile, "读取文件", nameof(readFile.CoordPointData));
                //            break;
                //        case "en-US":

                //            break;
                //    }

                //    break;  //统计结果

                //case "写入文件":
                //case "写入文件节点":
                //    WriteFile writeFile = new WriteFile();
                //    switch (SystemParamManager.Instance.SysConfigParam.Language)
                //    {
                //        default:
                //        case "zh-CN":
                //            rootNode = this._treeViewTarget.AddItems(writeFile, "写入文件", nameof(writeFile.CoordPoint));
                //            break;
                //        case "en-US":

                //            break;
                //    }

                //    break;  //统计结果

                case "坐标映射":
                case "坐标映射节点":
                    CoordMap coordMap = new CoordMap();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(coordMap, "坐标映射", nameof(coordMap.CoordPoint1), nameof(coordMap.CoordPoint2), nameof(coordMap.HomMat2D));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //统计结果

                case "坐标变换":
                case "坐标变换节点":
                    CoordAffine coordAffine = new CoordAffine();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(coordAffine, "坐标变换", nameof(coordAffine.CoordPointData), nameof(coordAffine.HomMat2D), nameof(coordAffine.OutCoordPointData));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  // 

                case "阵列测量":
                case "阵列测量节点":
                    MeasureArray measureArray = new MeasureArray();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(measureArray, "阵列测量");
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //

                case "流程单元":
                case "流程单元节点":
                    if (this._treeViewTarget != null)
                    {
                        JobUnit jobUnit = new JobUnit();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(jobUnit, "流程单元");  // , 
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(jobUnit, "JobUnit");  // , 
                                break;
                        }

                    }
                    break;  // 

                case "偏差计算":
                case "偏差计算节点":
                    OffsetCalculate offsetCalculate = new OffsetCalculate();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(offsetCalculate, "偏差计算", nameof(offsetCalculate.TargetPoint), nameof(offsetCalculate.SourcePoint), nameof(offsetCalculate.AddXYTheta));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  // 

                case "对位计算":
                case "对位计算节点":
                    if (this._treeViewTarget != null)
                    {
                        AlignCalculate alignCalculate = new AlignCalculate();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(alignCalculate, "对位计算", nameof(alignCalculate.TargetPoint), nameof(alignCalculate.SourcePoint), nameof(alignCalculate.AffinePoint), nameof(alignCalculate.AddXYTheta));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(alignCalculate, "AlignCalculate", nameof(alignCalculate.TargetPoint), nameof(alignCalculate.SourcePoint), nameof(alignCalculate.AffinePoint), nameof(alignCalculate.AddXYTheta));
                                break;
                        }
                    }
                    break;  // 

                case "向量对位":
                case "向量对位节点":
                    if (this._treeViewTarget != null)
                    {
                        VectorAlign vectorAlign = new VectorAlign();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(vectorAlign, "向量对位", nameof(vectorAlign.PlateTeachVector), nameof(vectorAlign.PlateCurVector), nameof(vectorAlign.BandTeachVector), nameof(vectorAlign.BandCurVector), nameof(vectorAlign.AddXYTheta));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(vectorAlign, "VectorAlign", nameof(vectorAlign.PlateTeachVector), nameof(vectorAlign.PlateCurVector), nameof(vectorAlign.BandTeachVector), nameof(vectorAlign.BandCurVector), nameof(vectorAlign.AddXYTheta));
                                break;
                        }
                    }
                    break;  // 


                case "纠偏计算":
                case "纠偏计算节点":
                    if (this._treeViewTarget != null)
                    {
                        RectifyCalculate rectify = new RectifyCalculate();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(rectify, "纠偏计算", nameof(rectify.WcsCoordSystem), nameof(rectify.AddXYTheta), nameof(rectify.CurWcsVector), nameof(rectify.TeachWcsVector), nameof(rectify.WcsRectifyCoordSystem));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(rectify, "RectifyCalculate", nameof(rectify.WcsCoordSystem), nameof(rectify.AddXYTheta), nameof(rectify.CurWcsVector), nameof(rectify.TeachWcsVector), nameof(rectify.WcsRectifyCoordSystem));
                                break;
                        }
                    }
                    break;

                case "向量计算":
                case "向量计算节点":
                    if (this._treeViewTarget != null)
                    {
                        VectorCalculate vectorCalculate = new VectorCalculate();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(vectorCalculate, "向量计算", nameof(vectorCalculate.TargetPoint), nameof(vectorCalculate.VectorAngle), nameof(vectorCalculate.WcsVector));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(vectorCalculate, "VectorCalculate", nameof(vectorCalculate.TargetPoint), nameof(vectorCalculate.VectorAngle), nameof(vectorCalculate.WcsVector));
                                break;
                        }
                    }
                    break;  // 

                //case "迁移计算":
                //case "迁移计算节点":
                //    MigrateCalculate migrateCalculate = new MigrateCalculate();
                //    switch (SystemParamManager.Instance.SysConfigParam.Language)
                //    {
                //        default:
                //        case "zh-CN":
                //            rootNode = this._treeViewTarget.AddItems(migrateCalculate, "迁移计算", nameof(migrateCalculate.WcsPoint1), nameof(migrateCalculate.WcsPoint2), nameof(migrateCalculate.JawParam));
                //            break;
                //        case "en-US":

                //            break;
                //    }
                //    break;  // 


                #endregion

                #region 2D 匹配
                case "变形匹配":
                case "变形匹配节点":
                    if (this._treeViewTarget != null)
                    {
                        LocalDeformableModelMatch deformableModelMatch = new LocalDeformableModelMatch();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(deformableModelMatch, "变形匹配",
                                                              nameof(deformableModelMatch.ImageData),
                                                              nameof(deformableModelMatch.RectifyImageData));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;

                case "NCC匹配":
                case "NCC匹配节点":
                    if (this._treeViewTarget != null)
                    {
                        NccModelMatch doNccModelMatch2D = new NccModelMatch();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(doNccModelMatch2D, "NCC匹配",
                                                              nameof(doNccModelMatch2D.ImageData),
                                                              nameof(doNccModelMatch2D.WcsSingleCoordSystem),
                                                              nameof(doNccModelMatch2D.RectifyImageData),
                                                              nameof(doNccModelMatch2D.PixCoordSystem),
                                                              nameof(doNccModelMatch2D.WcsCoordSystem));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(doNccModelMatch2D, "NccMatch",
                                                              nameof(doNccModelMatch2D.ImageData),
                                                              nameof(doNccModelMatch2D.WcsSingleCoordSystem),
                                                              nameof(doNccModelMatch2D.RectifyImageData),
                                                              nameof(doNccModelMatch2D.PixCoordSystem),
                                                              nameof(doNccModelMatch2D.WcsCoordSystem));
                                break;
                        }
                    }
                    break;

                case "形状匹配":
                case "形状匹配2D":
                case "坐标系-形状匹配":
                case "坐标系-2D形状匹配":
                case "形状匹配节点":
                case "形状匹配2D节点":
                case "坐标系-形状匹配节点":
                case "坐标系-2D形状匹配节点":
                    if (this._treeViewTarget != null)
                    {
                        ShapeModelMatch2D doShapeModelMatch2D = new ShapeModelMatch2D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(doShapeModelMatch2D, "形状匹配",
                                                              nameof(doShapeModelMatch2D.ImageData),
                                                              nameof(doShapeModelMatch2D.RemoveWcsVector),
                                                              nameof(doShapeModelMatch2D.WcsSingleCoordSystem),
                                                              nameof(doShapeModelMatch2D.RectifyImageData),
                                                              nameof(doShapeModelMatch2D.PixCoordSystem),
                                                              nameof(doShapeModelMatch2D.WcsCoordSystem)); //  
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(doShapeModelMatch2D, "ShapeMatch",
                                                              nameof(doShapeModelMatch2D.ImageData),
                                                              nameof(doShapeModelMatch2D.RemoveWcsVector),
                                                              nameof(doShapeModelMatch2D.WcsSingleCoordSystem),
                                                              nameof(doShapeModelMatch2D.RectifyImageData),
                                                              nameof(doShapeModelMatch2D.PixCoordSystem),
                                                              nameof(doShapeModelMatch2D.WcsCoordSystem)
                                ); //   
                                break;
                        }
                    }
                    break;

                case "轮廓匹配":
                case "轮廓匹配节点":
                    if (this._treeViewTarget != null)
                    {
                        ContourModelMatch2D contourModelMatch = new ContourModelMatch2D();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(contourModelMatch, "轮廓匹配",
                                                              nameof(contourModelMatch.CurTrackPoint),
                                                              nameof(contourModelMatch.WcsCoordSystem));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(contourModelMatch, "ContourMatch",
                                                              nameof(contourModelMatch.CurTrackPoint),
                                                              nameof(contourModelMatch.WcsCoordSystem));
                                break;
                        }
                    }
                    break;

                case "标签":
                case "标签节点":
                    if (this._treeViewTarget != null)
                    {
                        UserLable lable = new UserLable();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(lable, "标签");
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(lable, "UserLable");
                                break;
                        }
                    }
                    break;
                #endregion

                #region 区域操作

                case "拟合矩形区域":
                case "拟合矩形区域节点":
                    FitRect2Region fitRect2Region = new FitRect2Region();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(fitRect2Region, "拟合矩形区域", nameof(fitRect2Region.RegionData), nameof(fitRect2Region.Rect2));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //

                case "区域运算":
                case "区域运算节点":
                    RegionArithmetic regionArithmetic = new RegionArithmetic();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(regionArithmetic, "区域运算", nameof(regionArithmetic.RegionData), nameof(regionArithmetic.RegionData2), nameof(regionArithmetic.OutRegionData));
                            break;
                        case "en-US":

                            break;
                    }
                    break;  //
                #endregion

                #region 图像操作
                case "字符识别":
                case "字符识别节点":
                    Ocr doOcr = new Ocr();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(doOcr, "字符识别", nameof(doOcr.ImageData), nameof(doOcr.PixCoordSystem), nameof(doOcr.OcrResult));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                case "图像处理":
                case "图像处理节点":
                    ImageMorphology doImageMorphology = new ImageMorphology();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(doImageMorphology, "图像处理", nameof(doImageMorphology.ImageData), nameof(doImageMorphology.OutImageData));
                            break;
                        case "en-US":

                            break;
                    }
                    break;  //
                case "图像滤波":
                case "图像滤波节点":
                    ImageFilter imageFilter = new ImageFilter();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(imageFilter, "图像滤波", nameof(imageFilter.ImageData), nameof(imageFilter.OutImageData));
                            break;
                        case "en-US":

                            break;
                    }
                    break;  //
                case "图像增强":
                case "图像增强节点":
                    ImageEnhancement imageEnhan = new ImageEnhancement();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(imageEnhan, "图像增强", nameof(imageEnhan.ImageData), nameof(imageEnhan.OutImageData));
                            break;
                        case "en-US":

                            break;
                    }
                    break;  //
                case "图像变换":
                case "图像变换节点":
                    ImageAffine imageAffine = new ImageAffine();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(imageAffine, "图像变换", nameof(imageAffine.ImageData), nameof(imageAffine.HomMat2D), nameof(imageAffine.OutImageData));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                case "图像旋转":
                case "图像旋转节点":
                    ImageRotate imageRotate = new ImageRotate();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(imageRotate, "图像旋转", nameof(imageRotate.ImageData), nameof(imageRotate.PixCoordSystem), nameof(imageRotate.RotateImage));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                case "图像运算":
                case "图像运算节点":
                    ImageArithmetic imageArith = new ImageArithmetic();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(imageArith, "图像运算", nameof(imageArith.ImageData), nameof(imageArith.ImageData2), nameof(imageArith.OutImageData));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //


                case "图像裁剪":
                case "图像裁剪节点":
                    ImageReduce reduceImageDomain = new ImageReduce();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(reduceImageDomain, "图像裁剪", nameof(reduceImageDomain.ImageData), nameof(reduceImageDomain.PixCoordSystem), nameof(reduceImageDomain.ReduceImage));
                            break;
                        case "en-US":

                            break;
                    }
                    break; // 
                case "图像裁剪2":
                case "图像裁剪2节点":
                    ImageReduceROI reduceImage2 = new ImageReduceROI();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(reduceImage2, "图像裁剪2", nameof(reduceImage2.ImageData), nameof(reduceImage2.ROI), nameof(reduceImage2.ReduceImage));
                            break;
                        case "en-US":

                            break;
                    }
                    break; // 
                case "图像缩放":
                case "图像缩放节点":
                    ImageZoom imageZoom = new ImageZoom();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(imageZoom, "图像缩放", nameof(imageZoom.ImageData), nameof(imageZoom.OutImageData));
                            break;
                        case "en-US":

                            break;
                    }

                    break; // 

                case "图像分解":
                case "图像分解节点":
                    ImageDecompose imageDecompose = new ImageDecompose();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(imageDecompose, "图像分解", nameof(imageDecompose.ImageData), nameof(imageDecompose.OutImageData1), nameof(imageDecompose.OutImageData2), nameof(imageDecompose.OutImageData3));
                            break;
                        case "en-US":

                            break;
                    }
                    break; //   

                case "图像拼接":
                case "图像拼接节点":
                    TileImage tileImage = new TileImage();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(tileImage, "图像拼接", nameof(tileImage.ImageData), nameof(tileImage.TileImageData));
                            break;
                        case "en-US":

                            break;
                    }
                    break; //   图像拼接

                //// 因为相机安装有角度，所在需要校正相机与轴的平行度
                case "图像校正":
                case "图像校正节点":
                    ImageRectify imageRectify = new ImageRectify();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(imageRectify, "图像校正", nameof(imageRectify.ImageData), nameof(imageRectify.OutImageData));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(imageRectify, "ImageRectify", nameof(imageRectify.ImageData), nameof(imageRectify.OutImageData));
                            break;
                    }
                    break; //   图像拼接

                case "发送图像":
                case "发送图像节点":
                    SendHWindowImage sendWinImage = new SendHWindowImage();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(sendWinImage, "发送图像");
                            break;
                        case "en-US":

                            break;
                    }
                    break; //   图像拼接
                #endregion


                #region PLC操作
                ///   ,过时操作
                case "读取数据PLC":
                case "读取数据PLC节点":
                    ReadPlcData readPlcData = new ReadPlcData();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(readPlcData, "读取数据PLC", nameof(readPlcData.ReadContent)); //, 
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //

                case "写入数据PLC":
                case "写入数据PLC节点":
                    WritePlcData writePlcData = new WritePlcData();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(writePlcData, "写入数据PLC", nameof(writePlcData.WriteContent)); //, 
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //

                case "写入数据Socket":
                case "写入数据Socket节点":
                    WriteDataSocket writeDataSocket = new WriteDataSocket();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(writeDataSocket, "写入数据Socket", nameof(writeDataSocket.WriteContent)); //,  
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //

                case "发送结果Socket":
                case "发送结果Socket节点":
                    SendResultSocket SendResul = new SendResultSocket();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(SendResul, "发送结果Socket", nameof(SendResul.SendContent)); //,  
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //

                case "保存数据PLC":
                case "保存数据PLC节点":
                    SaveDataPlc saveDataPlc = new SaveDataPlc();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(saveDataPlc, "保存数据PLC"); //, 
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                            ////////  ,过时操作

                case "DelayedWait":
                    if (this._treeViewTarget != null)
                    {
                        Waite waite = new Waite();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(waite, "DelayedWait"); //
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "延时等待":
                case "延时等待节点":
                    if (this._treeViewTarget != null)
                    {
                        Waite waite = new Waite();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(waite, "延时等待"); //
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;  //

                case nameof(DataLable):
                    if (this._treeViewTarget != null)
                    {
                        DataLable dataLable = new DataLable();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(dataLable, "DataLable", nameof(dataLable.TextLable));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "数据标签":
                case "数据标签节点":
                    if (this._treeViewTarget != null)
                    {
                        DataLable dataLable = new DataLable();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(dataLable, "数据标签", nameof(dataLable.TextLable));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;  //

                case nameof(SendData):
                    if (this._treeViewTarget != null)
                    {
                        SendData sendData = new SendData();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(sendData, "SendData", nameof(sendData.SendContent));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;  //
                case "发送数据":
                case "发送数据节点":
                    if (this._treeViewTarget != null)
                    {
                        SendData sendData = new SendData();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(sendData, "发送数据", nameof(sendData.SendContent));
                                break;
                            case "en-US":

                                break;
                        }
                    }
                    break;  //

                case "发送数据RFID":
                case "发送数据RFID节点":
                    if (this._treeViewTarget != null)
                    {
                        SendDataRFID sendDataRFID = new SendDataRFID();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(sendDataRFID, "发送数据RFID", nameof(sendDataRFID.SendContent));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(sendDataRFID, "SendDataRFID", nameof(sendDataRFID.SendContent));
                                break;
                        }
                    }
                    break;  //

                case "等待信号":
                case "等待信号节点":
                    if (this._treeViewTarget != null)
                    {
                        WaiteSignal waiteData = new WaiteSignal();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(waiteData, "等待信号", nameof(waiteData.WaiteContent));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;  //

                case nameof(WriteData):
                    if (this._treeViewTarget != null)
                    {
                        WriteData writeData = new WriteData();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(writeData, "WriteData", nameof(writeData.WriteContent));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "写入数据":
                case "写入数据节点":
                    if (this._treeViewTarget != null)
                    {
                        WriteData writeData = new WriteData();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(writeData, "写入数据", nameof(writeData.WriteContent));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;  //

                case nameof(ReadData):
                    if (this._treeViewTarget != null)
                    {
                        ReadData readData = new ReadData();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(readData, "ReadData", nameof(readData.ReadContent));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;

                case "读取数据":
                case "读取数据节点":
                    if (this._treeViewTarget != null)
                    {
                        ReadData readData = new ReadData();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(readData, "读取数据", nameof(readData.ReadContent));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;  // 

                case "读取坐标点":
                case "读取坐标点节点":
                    if (this._treeViewTarget != null)
                    {
                        ReadCoordPoint coordPoint = new ReadCoordPoint();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(coordPoint, "读取坐标点", nameof(coordPoint.WcsPoint));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(coordPoint, "ReadCoordPoint", nameof(coordPoint.WcsPoint));
                                break;
                        }

                    }
                    break;  //  

                case "读取对象":
                case "读取对象节点":
                    if (this._treeViewTarget != null)
                    {
                        ReadObject readObject = new ReadObject();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(readObject, "读取对象", nameof(readObject.ReadContent));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(readObject, "ReadObject", nameof(readObject.ReadContent));
                                break;
                        }

                    }
                    break;  //  读取对象

                //case nameof(SaveData):
                //    if (this._treeViewTarget != null)
                //    {
                //        SaveDataNew saveDataNew = new SaveDataNew();
                //        switch (SystemParamManager.Instance.SysConfigParam.Language)
                //        {
                //            default:
                //            case "zh-CN":
                //                rootNode = this._treeViewTarget.AddItems(saveDataNew, "SaveData", nameof(saveDataNew.SaveContent));
                //                break;
                //            case "en-US":

                //                break;
                //        }

                //    }
                //    break;
                case "保存数据":
                case "保存数据节点":
                    if (this._treeViewTarget != null)
                    {
                        SaveDataNew saveDataNew = new SaveDataNew();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(saveDataNew, "保存数据", nameof(saveDataNew.SaveContent));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;  //

                case "保存数据Ftp":
                case "保存数据Ftp节点":
                    SaveDataFtp saveDataFtp = new SaveDataFtp();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(saveDataFtp, "保存数据Ftp", nameof(saveDataFtp.SaveContent));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //保存数据Ftp

                case "条件判断":
                case "条件判断节点":
                    ConditionalJudge conditionalJudge = new ConditionalJudge();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(conditionalJudge, "条件判断", nameof(conditionalJudge.Content));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //

                case nameof(ResultJudge):
                    if (this._treeViewTarget != null)
                    {
                        ResultJudge resultJudge = new ResultJudge();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(resultJudge, "ResultJudge", nameof(resultJudge.Content));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;
                case "结果判断":
                case "结果判断节点":
                    if (this._treeViewTarget != null)
                    {
                        ResultJudge resultJudge = new ResultJudge();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(resultJudge, "结果判断", nameof(resultJudge.Content));
                                break;
                            case "en-US":

                                break;
                        }

                    }
                    break;  //
                #endregion

                #region 缺陷检测  //
                case "直线检测":
                case "边缘检测":
                case "直线检测节点":
                case "边缘检测节点":
                    LineDetect lineDetect = new LineDetect();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(lineDetect, "直线检测", nameof(lineDetect.WcsLine), nameof(lineDetect.RegionData));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(lineDetect, "LineDetect", nameof(lineDetect.WcsLine), nameof(lineDetect.RegionData));
                            break;
                    }
                    break;  //
                case "破片检测":
                case "破片检测节点":
                    GapDetect gapDetect = new GapDetect();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(gapDetect, "破片检测", nameof(gapDetect.ImageData), nameof(gapDetect.PixCoordSystem), nameof(gapDetect.GapRegion));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(gapDetect, "GapDetect", nameof(gapDetect.ImageData), nameof(gapDetect.PixCoordSystem), nameof(gapDetect.GapRegion));
                            break;
                    }
                    break;  // 
                case "脚本工具":
                case "脚本工具节点":
                    ScriptTool scriptTool = new ScriptTool();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(scriptTool, "脚本工具", nameof(scriptTool.ImageData), nameof(scriptTool.PixCoordSystem), nameof(scriptTool.FlawRegion));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(scriptTool, "ScriptTool", nameof(scriptTool.ImageData), nameof(scriptTool.PixCoordSystem), nameof(scriptTool.FlawRegion));
                            break;
                    }
                    break;  // 

                case "测量检测":
                case "测量检测节点":
                    MeasureDetect measureDetect = new MeasureDetect();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(measureDetect, "测量检测", nameof(measureDetect.ImageData), nameof(measureDetect.FlawRegion));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(measureDetect, "MeasureDetect", nameof(measureDetect.ImageData), nameof(measureDetect.FlawRegion));
                            break;
                    }
                    break;  // 

                case "宽度检测":
                case "宽度检测节点":
                    WidthDetect widthDetect = new WidthDetect();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(widthDetect, "宽度检测", nameof(widthDetect.WcsEdgeCon1), nameof(widthDetect.WcsEdgeCon2), nameof(widthDetect.RegionData));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(widthDetect, "widthDetect", nameof(widthDetect.WcsEdgeCon1), nameof(widthDetect.WcsEdgeCon2), nameof(widthDetect.RegionData));
                            break;
                    }
                    break;  // 

                case "变化模型":
                case "变化模型节点":
                    VariationModeDetect variationModeDetect = new VariationModeDetect();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(variationModeDetect, "变化模型", nameof(variationModeDetect.ImageData), nameof(variationModeDetect.FlawRegion));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(variationModeDetect, "VariationModeDetect", nameof(variationModeDetect.ImageData), nameof(variationModeDetect.FlawRegion));
                            break;
                    }
                    break;  //  

                case "缺陷检测":
                case "瑕疵检测":
                case "瑕疵检测节点":
                    FlawDetect flawDetect = new FlawDetect();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(flawDetect, "缺陷检测", nameof(flawDetect.ImageData), nameof(flawDetect.FlawRegion));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(flawDetect, "FlawDetect", nameof(flawDetect.ImageData), nameof(flawDetect.FlawRegion));
                            break;
                    }
                    break;  //  瑕疵检测

                #endregion


                #region 上下料
                case "下料Try盘":
                case "下料Try盘节点":
                    LayOffTryPlate layOffTryPlate = new LayOffTryPlate();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(layOffTryPlate, "下料Try盘", nameof(layOffTryPlate.TargetPoint), nameof(layOffTryPlate.PlatformParam));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                case "下料穴位":
                case "下料穴位节点":
                case "放料计算":
                case "放料计算节点":
                    RobotLayOff robotLayOff = new RobotLayOff();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(robotLayOff, "放料计算", nameof(robotLayOff.WcsVector), nameof(robotLayOff.WcsVector2), nameof(robotLayOff.AddXYTheta));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                case "上料Try盘":
                case "上料Try盘节点":
                    LoadTryPlate loadTryPlate = new LoadTryPlate();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(loadTryPlate, "上料Try盘", nameof(loadTryPlate.ImageData), nameof(loadTryPlate.PlatformParam));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                case "上料穴位":
                case "上料穴位节点":
                    RobotLoad robotLoad = new RobotLoad();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(robotLoad, "上料穴位", nameof(robotLoad.WcsVector), nameof(robotLoad.TryPlatformParam), nameof(robotLoad.AddXYTheta));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                case "取料计算":
                case "取料计算节点":
                    RobotGrab robotGrab = new RobotGrab();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(robotGrab, "取料计算", nameof(robotGrab.WcsVector), nameof(robotGrab.TryPlatformParam), nameof(robotGrab.AddXYTheta));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(robotGrab, "RobotGrab", nameof(robotGrab.WcsVector), nameof(robotGrab.TryPlatformParam), nameof(robotGrab.AddXYTheta));
                            break;
                    }

                    break;  //
                #endregion

                #region 标定
                case "N点标定":
                case "N点标定节点":
                    MapCalib nPointCalib = new MapCalib();
                    rootNode = this._treeViewTarget.AddItems(nPointCalib, "N点标定", nameof(nPointCalib.SourcePoint), nameof(nPointCalib.TargetPoint), nameof(nPointCalib.HomMat2D));
                    break;  //
                case "旋转标定":
                case "旋转标定节点":
                    CalibRotateCenter rotateCenter = new CalibRotateCenter();
                    rootNode = this._treeViewTarget.AddItems(rotateCenter, "旋转标定", nameof(rotateCenter.SourcePoint), nameof(rotateCenter.TargetPoint), nameof(rotateCenter.WcsCircle), nameof(rotateCenter.PixCircle));
                    break;  //
                #endregion

                #region 流程控制
                case "条件语句":
                case "条件语句节点":
                    Conditional conditional = new Conditional();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(conditional, "条件语句");
                            break;
                        case "en-US":

                            break;
                    }

                    //this._treeViewTarget.AddItems(new ConditionaIf(), "条件语句<If>");
                    //this._treeViewTarget.AddItems(new ConditionaElse(), "条件语句<else>");
                    break;  //
                case "条件语句<If>":
                case "条件语句<If>节点":
                case "If语句节点":
                    ConditionaIf conditionaIf = new ConditionaIf();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(conditionaIf, "条件语句<If>");
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                case "条件语句<else>":
                case "条件语句<else>节点":
                case "Else语句节点":
                    ConditionaElse conditionaElse = new ConditionaElse();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(conditionaElse, "条件语句<else>");
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //

                case "Switch语句":
                case "Switch语句节点":
                    Switch conditionalSwitch = new Switch();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(conditionalSwitch, "Switch语句");
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //

                case "Case标签":
                case "Case标签节点":
                    Case sign = new Case();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(sign, "Case标签");
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //

                #endregion

                #region 点胶功能
                case "轨迹导出":
                case "轨迹导出节点":
                case "轮廓导出":
                case "轮廓导出节点":
                    ContourExport trackExport = new ContourExport();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(trackExport, "轨迹导出", nameof(trackExport.TrackPoint));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(trackExport, "TrackExport", nameof(trackExport.TrackPoint));
                            break;
                    }
                    break;  //
                case "轨迹读取":
                case "轨迹读取节点":
                case "轮廓读取":
                case "轮廓读取节点":
                    ContourRead trackRead = new ContourRead();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(trackRead, "轨迹读取", nameof(trackRead.WcsPolyLine));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(trackRead, "TrackRead", nameof(trackRead.WcsPolyLine));
                            break;
                    }
                    break;  //
                case "轨迹堆叠":
                case "轨迹堆叠节点":
                case "轮廓堆叠":
                case "轮廓堆叠节点":
                    TrackStack trackStack = new TrackStack();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(trackStack, "轨迹堆叠", nameof(trackStack.TrackPoint), nameof(trackStack.WcsPolyLine));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(trackStack, "TrackStack", nameof(trackStack.TrackPoint), nameof(trackStack.WcsPolyLine));
                            break;
                    }
                    break;  //
                case "轨迹合并":
                case "轨迹合并节点":
                case "轮廓合并":
                case "轮廓合并节点":
                    TrackCompose trackCompose = new TrackCompose();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(trackCompose, "轨迹合并", nameof(trackCompose.CamTrackPoint), nameof(trackCompose.LaserTrackPoint), nameof(trackCompose.WcsPolyLine));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(trackCompose, "TrackCompose", nameof(trackCompose.CamTrackPoint), nameof(trackCompose.LaserTrackPoint), nameof(trackCompose.WcsPolyLine));
                            break;
                    }
                    break;  //
                case "轨迹捨取":
                case "轨迹捨取节点":
                case "轮廓捨取":
                case "轮廓捨取节点":
                    ContourPick trackPull = new ContourPick(ProgramForm.Instance.AcqSourceName);
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(trackPull, "轨迹捨取", nameof(trackCompose.WcsPolyLine));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(trackPull, "TrackCompose", nameof(trackCompose.WcsPolyLine));
                            break;
                    }
                    break;  //
                case "轨迹转换":
                case "轨迹转换节点":
                case "轮廓转换":
                case "轮廓转换节点":
                    TrackTrans trackTrans = new TrackTrans();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(trackTrans, "轨迹转换", nameof(trackTrans.TrackPoint), nameof(trackTrans.WcsPolyLine));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(trackTrans, "TrackTrans", nameof(trackTrans.TrackPoint), nameof(trackTrans.WcsPolyLine));
                            break;
                    }
                    break;  //
                case "轨迹自检":
                case "轨迹自检节点":
                case "轮廓自检":
                case "轮廓自检节点":
                    TrackSelfCheck selfCheck = new TrackSelfCheck();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(selfCheck, "轨迹自检", nameof(selfCheck.CurTrackPoint), nameof(selfCheck.StdTrackPoint), nameof(selfCheck.WcsPolyLine));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(selfCheck, "TrackSelfCheck", nameof(selfCheck.CurTrackPoint), nameof(selfCheck.StdTrackPoint), nameof(selfCheck.WcsPolyLine));
                            break;
                    }
                    break;  //

                case "轨迹计算":
                case "轨迹计算节点":
                case "轮廓计算":
                case "轮廓计算节点":
                    TrackCalculate trackCalculate = new TrackCalculate();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(trackCalculate, "轨迹计算", nameof(trackCalculate.TrackPoint), nameof(trackCalculate.WcsVector));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(trackCalculate, "TrackCalculate", nameof(trackCalculate.TrackPoint), nameof(trackCalculate.WcsVector));
                            break;
                    }

                    break;  //

                case "轨迹提取":
                case "轨迹提取节点":
                case "轮廓提取":
                case "轮廓提取节点":
                    ContourExtract trackExtract = new ContourExtract();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(trackExtract, "轮廓提取", nameof(trackExtract.CurTrackPoint), nameof(trackExtract.WcsPolyLine));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(trackExtract, "ContourExtract", nameof(trackExtract.CurTrackPoint), nameof(trackExtract.WcsPolyLine));
                            break;
                    }
                    break;  //
                case "轨迹偏置":
                case "轨迹偏置节点":
                    TrackOffset trackOffset = new TrackOffset();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(trackOffset, "轨迹偏置", nameof(trackOffset.TrackPoint), nameof(trackOffset.WcsPolyLine));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(trackOffset, "TrackOffset", nameof(trackOffset.TrackPoint), nameof(trackOffset.WcsPolyLine));
                            break;
                    }
                    break;  //

                case "轮廓发送":
                case "轮廓发送节点":
                    ContourSend contourSend  = new ContourSend();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(contourSend, "轮廓发送", nameof(contourSend.WcsPoint), nameof(contourSend.WcsPolyLine));
                            break;
                        case "en-US":
                            rootNode = this._treeViewTarget.AddItems(contourSend, "ContourSend", nameof(contourSend.WcsPoint), nameof(contourSend.WcsPolyLine));
                            break;
                    }
                    break;  //
                #endregion

                #region 引导变换
                case "引导标定":
                case "引导标定节点":
                    GuidedCalib guidedCalib = new GuidedCalib();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(guidedCalib, "引导标定", nameof(guidedCalib.WcsPoint), nameof(guidedCalib.HomMat2D));
                            break;
                        case "en-US":

                            break;
                    }

                    break;
                case "点变换":
                case "点引导变换":
                case "点变换节点":
                case "点引导变换节点":
                    PointAffine pointAffine = new PointAffine();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(pointAffine, "点引导变换", nameof(pointAffine.WcsPoint), nameof(pointAffine.AffinePoint));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                case "线变换":
                case "线引导变换":
                case "线变换节点":
                case "线引导变换节点":
                    LineAffine lineAffine = new LineAffine();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(lineAffine, "线引导变换", nameof(lineAffine.WcsLine), nameof(lineAffine.AffineLine));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                case "圆变换":
                case "圆引导变换":
                case "圆变换节点":
                case "圆引导变换节点":
                    CircleAffine circleAffine = new CircleAffine();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(circleAffine, "圆引导变换", nameof(circleAffine.WcsCircle), nameof(circleAffine.AffineCircle));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                case "椭圆变换":
                case "椭圆引导变换":
                case "椭圆变换节点":
                case "椭圆引导变换节点":
                    EllpiseAffine ellpiseAffine = new EllpiseAffine();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(ellpiseAffine, "椭圆引导变换", nameof(ellpiseAffine.WcsEllipse), nameof(ellpiseAffine.AffineEllipse));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                case "矩形变换":
                case "矩形引导变换":
                case "矩形变换节点":
                case "矩形引导变换节点":
                    Rect2Affine rect2Affine = new Rect2Affine();
                    switch (SystemParamManager.Instance.SysConfigParam.Language)
                    {
                        default:
                        case "zh-CN":
                            rootNode = this._treeViewTarget.AddItems(rect2Affine, "矩形引导变换", nameof(rect2Affine.WcsRect2), nameof(rect2Affine.AffineRect2));
                            break;
                        case "en-US":

                            break;
                    }

                    break;  //
                #endregion

                #region 坐标补偿
                case "点补偿":
                case "点补偿节点":
                    if (this._treeViewTarget != null)
                    {
                        PointCompensate compensate = new PointCompensate();
                        switch (SystemParamManager.Instance.SysConfigParam.Language)
                        {
                            default:
                            case "zh-CN":
                                rootNode = this._treeViewTarget.AddItems(compensate, "点补偿", nameof(compensate.WcsPoint1), nameof(compensate.WcsPoint));
                                break;
                            case "en-US":
                                rootNode = this._treeViewTarget.AddItems(compensate, "PointCompensate", nameof(compensate.WcsPoint1), nameof(compensate.WcsPoint));
                                break;
                        }
                    }
                    break;
                #endregion

                default:
                    break;

            }
            //////  添加工具图标  //////////////////////////////////////////
            if (rootNode != null)
            {
                //rootNode.ImageKey = e.Node.ImageKey;
                //rootNode.SelectedImageKey = e.Node.SelectedImageKey;
                //foreach (var item in rootNode.Nodes)
                //{

                //}
            }
        }

        private IFunction GetCurrentCoordSystem()
        {
            //if (FunctionManage.CoordSystemList.Count > 0)
            //    return FunctionManage.CoordSystemList.Last();
            //else
            return null;


        }

        private void ToolForm_Load(object sender, EventArgs e)
        {

        }


    }
}
