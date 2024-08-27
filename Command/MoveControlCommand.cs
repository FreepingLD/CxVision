using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{

    /// <summary>
    /// 运动命令类,发送命令来控制机台运动 
    /// </summary>
    public class MoveControlCommand : CommandBase
    {
        private MoveCommandParam moveParam;
        private IMotionControl i_Card;
        public MoveCommandParam MoveParam { get => moveParam; set => moveParam = value; }

        public MoveControlCommand(IMotionControl card)
        {
            this.i_Card = card;
        }
        public override void execute()
        {
            if (i_Card != null)
                i_Card.MoveMultyAxis(this.moveParam.CoordSysName,this.moveParam.MoveAxis, this.moveParam.MoveSpeed, this.moveParam.AxisParam);
        }



    }




}
