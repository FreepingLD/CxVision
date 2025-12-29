using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    ///  Socket 通信指令
    /// </summary>
    [Serializable]
    [ProtoContract]
    public class SocketCommandRfidOld
    {
        [ProtoMember(1)]
        public string CamStation { get; set; }// 相机工位，即相机名称
        [ProtoMember(2)]
        public string Command { get; set; }
        [ProtoMember(3)]
        public int Parity { get; set; } // 奇偶行
        [ProtoMember(4)]
        public int GrabNo { get; set; } // 拍照位置
        [ProtoMember(5)]
        public double X { get; set; }
        [ProtoMember(6)]
        public double Y { get; set; }
        [ProtoMember(7)]
        public double Z { get; set; }
        [ProtoMember(8)]
        public double Theta { get; set; }
        [ProtoMember(9)]
        public string Result { get; set; } // 结果3种状态，Wait：表示等待结果，OK: 表示测量结果OK，NG:表示测量结果 NG
        [ProtoMember(10)]
        public int TriggerFromSocket { get; set; } // 触发标志位
        [ProtoMember(11)]
        public int ColCount { get; set; } // 每一行列的数量
        [ProtoMember(12)]
        public int Polarity { get; set; } // 用来区分拍照方向 

        /// <summary>
        /// 用于存储每一个拍照位的结果信息
        /// </summary>
        [ProtoMember(13)]
        public Dictionary<int, RfidCommand> RFIDInfo { get; set; }
        [ProtoMember(14)]
        public string ErrorMessage { get; set; }
        [ProtoMember(15)]
        public double RowAngle { get; set; }
        [ProtoMember(16)]
        public ulong  RowIndex { get; set; }
        [ProtoMember(17)]
        public double CommandSendTime { get; set; } // 运控发送指令使用的时间
        [ProtoMember(18)]
        public double ResponseTime { get; set; }  // 视觉处理完后的回怎么时间

        [ProtoMember(19)]
        public double ColDist { get; set; }  // 相邻两列间的距离

        public SocketCommandRfidOld()
        {
            this.CamStation = "UpAOICam";
            this.Command = "Grab";
            this.GrabNo = 0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Theta = 0;
            this.Result = "Wait";
            this.TriggerFromSocket = 0;
            this.Polarity = 1;
            this.RFIDInfo = new Dictionary<int, RfidCommand>();
            this.ErrorMessage = "";
            this.RowAngle = 0;
            this.RowIndex = 0;
            this.CommandSendTime = 0;
            this.ResponseTime = 0;
            this.ColDist = 0;   
        }

        public SocketCommandRfidOld(string camStation)
        {
            this.Command = "Grab";
            this.CamStation = camStation;
            this.GrabNo = 0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Theta = 0;
            this.Result = "Wait";
            this.TriggerFromSocket = 0;
            this.Polarity = 1;
            this.RFIDInfo = new Dictionary<int, RfidCommand>();
            this.ErrorMessage = "";
            this.RowAngle = 0;
            this.RowIndex = 0;
            this.CommandSendTime = 0;
            this.ResponseTime = 0;
            this.ColDist = 0;
        }

        public void Reset()
        {
            this.GrabNo = 0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Theta = 0;
            this.Parity = 0;
            this.Result = "Wait";
            this.ColCount = 0;  
            this.TriggerFromSocket = 0;
            this.Polarity = 1;
            this.RFIDInfo = new Dictionary<int, RfidCommand>();
            this.ErrorMessage = "";
            this.RowAngle = 0;
            this.RowIndex = 0;
            this.CommandSendTime = 0;
            this.ResponseTime = 0;
            this.ColDist = 0;
        }

        public SocketCommandRfid Clone()
        {
            SocketCommandRfid command = new SocketCommandRfid();
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, this);//序列化
                ms.Seek(0, SeekOrigin.Begin); // 将当前流的位置移动到开始处
                command = (SocketCommandRfid)bf.Deserialize(ms);//反序列化
            }
            return command;
        }

        public static string[] GetPropertyName()
        {
            SocketCommandRfid command = new SocketCommandRfid();
            List<string> list = new List<string>();
            Type type = command.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (var item in propertyInfos)
            {
                list.Add(item.Name);
            }
            return list.ToArray();
        }


    }




}
