using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MotionControlCard
{


    [Serializable]
    public class ReadSocketData
    {
        public enCoordSysName CoordSysName { get; set; }
        public enDataTypes DataType { get; set; }
        public string ReadValue { get; set; }
        public string TargetValue { get; set; }
        public bool IsCompare { get; set; }
        public bool IsOutput { get; set; }
        public string Describe { get; set; }  //

        public ReadSocketData()
        {
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.DataType = enDataTypes.Int;
            this.Describe = "";
            this.IsCompare = false;
            this.IsOutput = false;
            this.ReadValue = "";
            this.TargetValue = "";
        }

    }

    [Serializable]
    public class CommunicationDataItemPlc
    {
        public string Adress { get; set; }
        public enDataTypes DataType { get; set; }
        public enAxisReadWriteState ReadWriteState { get; set; }
        public object WriteValue { get; set; }
        private bool IsAbsoluteValue { get; set; }
        public bool IsReset { get; set; }
        public object ReceiverValue { get; set; }
        public int Length { get; set; }
        public string Describe { get; set; }
        public int OkState { get; set; }
        public int NgState { get; set; }
        public int OtherState { get; set; }


        public CommunicationDataItemPlc()
        {
            this.DataType = enDataTypes.Int;
            this.Describe = "";
            this.IsReset = false;
            this.IsAbsoluteValue = false;
            this.ReadWriteState = enAxisReadWriteState.ReadOnly;
            this.Length = 1;
            this.OkState = 0;
            this.NgState = 0;
            this.OtherState = 0;
        }

    }





}
