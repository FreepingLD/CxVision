using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class MapCalibParam
    {
        public enMapCalibMethod MapCalibMethod { get; set; } = enMapCalibMethod.PixToWcs;

    }

    public enum enMapCalibMethod
    {
        PixToWcs,
        WcsToWcs,
        PixToPix,
    }


}
