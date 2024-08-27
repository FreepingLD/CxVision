using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock.WrapUIClass
{
    public class ProgramManager
    {
        private List<TreeViewWrapClass> _ProgramList = new List<TreeViewWrapClass>();
        public List<TreeViewWrapClass> ProgramList { get => _ProgramList; set => _ProgramList = value; }

        private object lockState = new object();
        private ProgramManager _Instance = null;
        private ProgramManager()
        {

        }
        public ProgramManager Instance
        {
            get
            {
                if (this._Instance == null)
                {
                    lock (this.lockState)
                    {
                        _Instance = new ProgramManager();
                    }
                }
                return _Instance;
            }
        }


        public void LoadProgram(string programPath)
        {
            foreach (var item in _ProgramList)
            {
                item.OpenProgram(programPath);
            }
        }
        public void SaveProgram(string programPath)
        {
            foreach (var item in _ProgramList)
            {
                item.SaveProgram(programPath);
            }
        }



    }
}
