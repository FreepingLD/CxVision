using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common
{

    /// <summary>
    /// 处理激光采集完成的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AcqCompleteEventHandler(PointCloudAcqCompleteEventArgs e);//AcqCompleteEventArgs

    public delegate void ImageAcqCompleteEventHandler(object sender, ImageAcqCompleteEventArgs e);//AcqCompleteEventArgs

    public delegate void PointCloudAcqCompleteEventHandler(object sender, PointCloudAcqCompleteEventArgs e);//AcqCompleteEventArgs

    public delegate void AcqNumberEventHandler(AcqNumberEventArgs e);
    // 节点编辑委托
    public delegate void EditeNodeEventHandler(object sender, TreeViewEventArgs e);

    public delegate void ClickNodeEventHandler(object sender, TreeNodeMouseClickEventArgs e);

    public delegate void DeleteNodeEventHandler(object sender, TreeViewEventArgs e);

    public delegate void AddNodeEventHandler(object sender, TreeViewEventArgs e);

    public delegate void ClearNodeEventHandler(object sender, TreeViewEventArgs e);

    public delegate void ExcuteCompletedEventHandler(object sender, ExcuteCompletedEventArgs e);//

    public delegate void ItemDeleteEventHandler(object sender, ItemDeleteEventArgs e);//

    public delegate void RunResultEventHandler(GraphicUpdateEventArgs e);//

    public delegate void GraphicUpdataEventHandler(object sender, ExcuteCompletedEventArgs e);

    // 运动到位委托类
    public delegate void PoseInfoEventHandler(object send, PoseInfoEventArgs e);

    public delegate void AxisMoveEventHandler(object send, EventArgs e);

    public delegate void MeasureChangeEventHandler();//
    [Serializable]

    public delegate void MetrolegyCompletedEventHandler(MetrolegyCompletedEventArgs e);//

    public delegate void DisplayMeasureResultEventHandler(DisplayMetrolegyObjectEventArgs e);//

    public delegate void ItemsChangeEventHandler(object send, ItemsChangeEventArgs e);
    //public delegate void DeleteItemsEventHandler(object send, ItemsEventArgs e);

    public delegate void DataSendEventHandler(object send, DataSendEventArgs e);

    public delegate void JudgeResultSendEventHandler(object send, DataSendEventArgs e);

    public delegate void GrayValueInfoEventHandler(object send, GrayValueInfoEventArgs e);
    /// <summary>
    /// 处理激光采集完成的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //public delegate void MeasureCompleteEventHandler(MeasureCompleteEventArgs e);//AcqCompleteEventArgs ;object sender,


    public delegate void ButonClickEventHandler(object send, ButonClickEventArgs e);

}
