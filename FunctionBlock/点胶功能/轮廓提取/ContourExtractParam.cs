using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class ContourExtractParam
    {

        public double InterStep { get; set; }
        public double UnionStep { get; set; }
        public bool RemoveRepetitivePoint { get; set; }


        public bool EnableSelfCheck { get; set; }
        public double StartPercent { get; set; }
        public double EndPercent { get; set; }
        public enTransformationType TransformationType { get; set; }

        public double AnomalyThreshold { get; set; }
        public int AnomalyCount { get; set; }

        public ContourExtractParam()
        {
            this.InterStep = 0.05;
            this.UnionStep = 0.02;
            this.RemoveRepetitivePoint = true;

            this.EnableSelfCheck = false;
            this.StartPercent = 0.1;
            this.EndPercent = 0.7;
            this.TransformationType = enTransformationType.rigid;
            this.AnomalyThreshold = 0.05;
            this.AnomalyCount = 5;
        }




    }




}
