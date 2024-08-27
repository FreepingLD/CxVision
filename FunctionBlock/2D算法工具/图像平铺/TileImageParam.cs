using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class TileImageParam
    {
        public int OffsetRow { get; set; }
        public int OffsetCol { get; set; }
        public int Row1 { get; set; }
        public int Col1 { get; set; }
        public int Row2 { get; set; }
        public int Col2 { get; set; }
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public TileImageParam()
        {
            this.OffsetRow = 0;
            this.OffsetCol = 0;
            this.Row1 = -1;
            this.Col1 = -1;
            this.Row2 = -1;
            this.Col2 = -1;
            this.Width = 1000;
            this.Height = 1000;
        }








    }
}
