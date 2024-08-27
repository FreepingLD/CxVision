using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class HalconScript
    {
        private HDevEngine myEngine;
        private HDevProgramCall programCall;
        private HDevProgram program;
        private static HalconScript _instance;
        private static object lockObj = new object();
        public HDevEngine MyEngine { get => myEngine; set => myEngine = value; }
        public HDevProgramCall ProgramCall { get => programCall; set => programCall = value; }
        public HDevProgram Program { get => program; set => program = value; }

        public static HalconScript Instance
        {
            get
            {
                lock (lockObj)
                {
                    return _instance = _instance ?? new HalconScript();
                }
            }
        }

        private HalconScript()
        {
            this.myEngine = new HDevEngine();
            this.program = new HDevProgram();
        }

        public void Dispose()
        {
            this.myEngine?.Dispose();
            this.program?.Dispose();
            this.programCall?.Dispose();
        }



    }
}
