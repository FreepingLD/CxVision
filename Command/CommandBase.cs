using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{
    /// <summary>
    /// 命令基类，不同的命令需要不同的命令参数，具体命令类只执行单一的命令参数，命令参数由命令发送者提供
    /// </summary>
    public class CommandBase : ICommand
    {
        public virtual void execute()
        {
            throw new NotImplementedException();
        }


    }
}
