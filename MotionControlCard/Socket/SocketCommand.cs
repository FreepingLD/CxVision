using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MotionControlCard
{
    /// <summary>
    ///  Socket 通信指令
    /// </summary>
    [Serializable]
    public class SocketCommand
    {
        protected string PlcName { get; set; }// 相机工位
        public string CamStation { get; set; }// 相机工位
        public string Command { get; set; } // 指令名称  Grab == 拍照 /Calib == 标定 /GetData == 获取数据
        public int AlignCount { get; set; }// 对齐次数
        public int GrabNo { get; set; } // 拍照位置
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Theta { get; set; }
        public double U { get; set; }
        public double V { get; set; }
        public double Add_x { get; set; }
        public double Add_y { get; set; }
        public double Add_z { get; set; }
        public double Add_theta { get; set; }
        public double Add_u { get; set; }
        public double Add_v { get; set; }
        public string GrabResult { get; set; } // 结果


        /// <summary>
        /// 这个属性不需要发送与接收
        /// </summary>
        public int TriggerFromSocket { get; set; }


        /// <summary>
        /// 后面不再使用
        /// </summary>
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public List<double> Path_X { get; set; }
        public List<double> Path_Y { get; set; }
        public List<double> Path_Z { get; set; }
        public List<double> Path_Theta { get; set; }
        public List<double> Path_U { get; set; }
        public List<double> Path_V { get; set; }

        //public static List<string> ListCommand { get; set; }
        public SocketCommand()
        {
            this.Command = "Grab";
            this.CamStation = "UpAOICam";
            this.GrabNo = 1;
            this.AlignCount = 1;
            this.X = 0;
            this.Y = 0;
            this.Theta = 0;
            this.AlignCount = 1;
            this.GrabResult = "OK";
            this.TriggerFromSocket = 0;
            this.Path_X = new List<double>();
            this.Path_Y = new List<double>();
            this.Path_Z = new List<double>();
            this.Path_Theta = new List<double>();
            this.Path_U = new List<double>();
            this.Path_V = new List<double>();
        }

        public SocketCommand(bool isInit = true)
        {
            this.Command = "Grab";
            this.CamStation = "UpAOICam";
            this.GrabNo = 1;
            this.AlignCount = 1;
            this.X = 0;
            this.Y = 0;
            this.Theta = 0;
            this.AlignCount = 1;
            this.GrabResult = "OK";
            this.TriggerFromSocket = 0;
            this.Path_X = new List<double>();
            this.Path_Y = new List<double>();
            this.Path_Z = new List<double>();
            this.Path_Theta = new List<double>();
            this.Path_U = new List<double>();
            this.Path_V = new List<double>();
        }

        public SocketCommand(string camStation)
        {
            this.Command = "Grab";
            this.CamStation = camStation;
            this.GrabNo = 1;
            this.AlignCount = 1;
            this.X = 0;
            this.Y = 0;
            this.Theta = 0;
            this.AlignCount = 1;
            this.GrabResult = "OK";
            this.TriggerFromSocket = 0;
            this.Path_X = new List<double>();
            this.Path_Y = new List<double>();
            this.Path_Z = new List<double>();
            this.Path_Theta = new List<double>();
            this.Path_U = new List<double>();
            this.Path_V = new List<double>();
        }
        public static SocketCommand GetSocketCommand(string content, string motionName)
        {
            /////////////////////////////////////
            SocketCommand command = new SocketCommand();
            string[] semicolon = content.Split(';');
            int index = 0;
            Type type = command.GetType();
            command.PlcName = motionName;
            foreach (var item in CommandConfigManger.Instance.GetCommandParam(motionName).ParamList)
            {
                PropertyInfo propertyInfo = type.GetProperty(item);
                if (propertyInfo != null)
                {
                    switch (propertyInfo.Name)
                    {
                        case "Path_X":
                            if (index < semicolon.Length)
                            {
                                string[] value = semicolon[index].Split(',');
                                foreach (var item1 in value)
                                {
                                    command.Path_X.Add(Convert.ToDouble(item1));
                                }
                            }
                            break;
                        case "Path_Y":
                            if (index < semicolon.Length)
                            {
                                string[] value = semicolon[index].Split(',');
                                foreach (var item1 in value)
                                {
                                    command.Path_Y.Add(Convert.ToDouble(item1));
                                }
                            }
                            break;
                        case "Path_Z":
                            if (index < semicolon.Length)
                            {
                                string[] value = semicolon[index].Split(',');
                                foreach (var item1 in value)
                                {
                                    command.Path_Z.Add(Convert.ToDouble(item1));
                                }
                            }
                            break;
                        case "Path_Theta":
                            if (index < semicolon.Length)
                            {
                                string[] value = semicolon[index].Split(',');
                                foreach (var item1 in value)
                                {
                                    command.Path_Theta.Add(Convert.ToDouble(item1));
                                }
                            }
                            break;
                        case "Path_U":
                            if (index < semicolon.Length)
                            {
                                string[] value = semicolon[index].Split(',');
                                foreach (var item1 in value)
                                {
                                    command.Path_U.Add(Convert.ToDouble(item1));
                                }
                            }
                            break;
                        case "Path_V":
                            if (index < semicolon.Length)
                            {
                                string[] value = semicolon[index].Split(',');
                                foreach (var item1 in value)
                                {
                                    command.Path_V.Add(Convert.ToDouble(item1));
                                }
                            }
                            break;
                        case "\\n\\r":
                            break;
                        case "GrabNo":
                            int GrabNo = 0;
                            if (index < semicolon.Length)
                            {
                                int.TryParse(semicolon[index], out GrabNo);
                                propertyInfo.SetValue(command, GrabNo);
                            }
                            break;
                        case "AlignCount":
                            int AlignCount = 0;
                            if (index < semicolon.Length)
                            {
                                int.TryParse(semicolon[index], out AlignCount);
                                propertyInfo.SetValue(command, AlignCount);
                            }
                            break;
                        case "X":
                        case "Y":
                        case "Z":
                        case "Theta":
                        case "U":
                        case "V":
                        case "Add_x":
                        case "Add_y":
                        case "Add_z":
                        case "Add_theta":
                        case "Add_u":
                        case "Add_v":
                            double curValue = 0;
                            if (index < semicolon.Length)
                            {
                                double.TryParse(semicolon[index], out curValue);
                                propertyInfo.SetValue(command, curValue);
                            }
                            break;
                        default:
                        case "CamStation":
                        case "Command":
                        case "GrabResult":
                            if (index < semicolon.Length)
                            {
                                propertyInfo.SetValue(command, semicolon[index]);
                            }
                            break;
                    }
                }
                index++;
            }
            return command;
        }

        public void Reset()
        {
            this.GrabNo = 0;
            this.AlignCount = 0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Theta = 0;
            this.U = 0;
            this.V = 0;
            this.GrabResult = "OK";
            this.TriggerFromSocket = 0;
            this.X1 = 0;
            this.Y1 = 0;
            this.X2 = 0;
            this.Y2 = 0;
            this.Path_X = new List<double>();
            this.Path_Y = new List<double>();
            this.Path_Z = new List<double>();
            this.Path_Theta = new List<double>();
            this.Path_U = new List<double>();
            this.Path_V = new List<double>();
            this.Add_x = 0;
            this.Add_y = 0;
            this.Add_z = 0;
            this.Add_theta = 0;
            this.Add_u = 0;
            this.Add_v = 0;
        }
        public SocketCommand Clone()
        {
            SocketCommand command = new SocketCommand();
            command.PlcName = this.PlcName;
            command.Command = this.Command;
            command.CamStation = this.CamStation;
            command.GrabNo = this.GrabNo;
            command.AlignCount = this.AlignCount;
            command.X = this.X;
            command.Y = this.Y;
            command.Z = this.Z;
            command.Theta = this.Theta;
            command.U = this.U;
            command.V = this.V;
            command.GrabResult = this.GrabResult;
            command.TriggerFromSocket = this.TriggerFromSocket;
            command.X1 = this.X1;
            command.Y1 = this.Y1;
            command.X2 = this.X2;
            command.Y2 = this.Y2;
            command.Path_X = this.Path_X;
            command.Path_Y = this.Path_Y;
            command.Path_Z = this.Path_Z;
            command.Path_Theta = this.Path_Theta;
            command.Path_U = this.Path_U;
            command.Path_V = this.Path_V;
            command.Add_x = this.Add_x;
            command.Add_y = this.Add_y;
            command.Add_z = this.Add_z;
            command.Add_theta = this.Add_theta;
            command.Add_u = this.Add_u;
            command.Add_v = this.Add_v;
            return command;
        }


        public static string[] GetPropertyName()
        {
            SocketCommand command = new SocketCommand();
            List<string> list = new List<string>();
            Type type = command.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (var item in propertyInfos)
            {
                list.Add(item.Name);
            }
            return list.ToArray();
        }

        public string ToString2()
        {
            string str1 = "", str2 = "", str3 = "";
            str1 = string.Join(",", this.Command, this.CamStation, this.GrabNo, this.AlignCount, this.X, this.Y, this.Theta, this.GrabResult, this.X1, this.Y1, this.X2, this.Y2, this.Add_x, this.Add_theta);
            /// 数组发送时，需要对数据做去重处理
            if (this.Path_X.Count > 0)
            {
                List<double> New_PathX = new List<double>();
                List<double> New_PathY = new List<double>();
                double start_x = this.Path_X[0];
                double start_y = this.Path_Y[0];
                New_PathX.Add(start_x);
                New_PathY.Add(start_y);
                for (int i = 1; i < this.Path_X.Count; i++)
                {
                    if (Math.Abs(this.Path_X[i] - this.Path_X[i - 1]) > 0.02)
                    {
                        New_PathX.Add(this.Path_X[i]);
                        New_PathY.Add(this.Path_Y[i]);
                    }
                }
                //////////////////////////
                str2 = string.Join(",", New_PathX.ToArray());
                str3 = string.Join(",", New_PathY.ToArray());
            }
            else
            {
                if (this.Path_X != null)
                    str2 = string.Join(",", this.Path_X.ToArray());
                else
                    str2 = "NULL";
                if (this.Path_Y != null)
                    str3 = string.Join(",", this.Path_Y.ToArray());
                else
                    str3 = "NULL";
            }
            return string.Join(";", str1, str2, str3);
        }

        public override string ToString()
        {
            List<object> list = new List<object>();
            Type type = this.GetType();
            if(CommandConfigManger.Instance.IsContainCommandParam(this.PlcName))
            {
                foreach (var item in CommandConfigManger.Instance.GetCommandParam(this.PlcName).ParamList) // 命令间以分号分开，命令内部以逗号分开
                {
                    PropertyInfo propertyInfo = type.GetProperty(item);
                    if (propertyInfo != null)
                    {
                        object value = propertyInfo.GetValue(this);
                        list.Add(value);
                    }
                }
            }
            list.Add("\r\n");
            return string.Join(";", list.ToArray());
        }




    }
}
