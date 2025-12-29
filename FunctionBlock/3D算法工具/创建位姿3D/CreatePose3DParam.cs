using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using Common;
using System.IO;
using System.ComponentModel;
using AlgorithmsLibrary;

namespace FunctionBlock
{
    [Serializable]
    public class CreatePose3DParam
    {
        private userWcsPose coord_System;
        private double trans_x = 0;
        private double trans__y = 0;
        private double trans__z = 0;
        private double rot_x = 0;
        private double rot_y = 0;
        private double rot_z = 0;
        private string orderOfTransform = "Rp+T";
        private string orderOfRotation = "gba";
        private string viewOfTransform = "point";
        private bool invertPose = false;

        public double Tx
        {
            get
            {
                return trans_x;
            }

            set
            {
                trans_x = value;
            }
        }
        public double Ty
        {
            get
            {
                return trans__y;
            }

            set
            {
                trans__y = value;
            }
        }
        public double Tz
        {
            get
            {
                return trans__z;
            }

            set
            {
                trans__z = value;
            }
        }
        public double RotX
        {
            get
            {
                return rot_x;
            }

            set
            {
                rot_x = value;
            }
        }
        public double RotY
        {
            get
            {
                return rot_y;
            }

            set
            {
                rot_y = value;
            }
        }
        public double RotZ
        {
            get
            {
                return rot_z;
            }

            set
            {
                rot_z = value;
            }
        }
        public string OrderOfTransform
        {
            get
            {
                return orderOfTransform;
            }

            set
            {
                orderOfTransform = value;
            }
        }
        public string OrderOfRotation
        {
            get
            {
                return orderOfRotation;
            }

            set
            {
                orderOfRotation = value;
            }
        }
        public string ViewOfTransform
        {
            get
            {
                return viewOfTransform;
            }

            set
            {
                viewOfTransform = value;
            }
        }
        public bool InvertPose
        {
            get
            {
                return invertPose;
            }

            set
            {
                invertPose = value;
            }
        }


        public bool Create3DPose(out userWcsPose wcsPose)
        {
            HTuple tempPose = null;
            HTuple invertPose = null;
            wcsPose = new userWcsPose();
            bool result = false;
            /////////////////////
            HOperatorSet.CreatePose(this.Tx, this.Ty, this.Tz, this.RotX, this.RotY, this.RotZ, this.OrderOfTransform, this.OrderOfRotation, this.ViewOfTransform, out tempPose);
            HOperatorSet.PoseInvert(tempPose, out invertPose);
            if (this.InvertPose)
                wcsPose = new userWcsPose(invertPose);
            else
                wcsPose = new userWcsPose(tempPose);
            result = true;
            /////////////////
            return result;
        }

    }


}
