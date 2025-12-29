using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class CenterCalibParam
    {
        public enCenterType CenterType { get; set; } = enCenterType.世界坐标;

    }

    public enum enCenterType
    {
        世界坐标,
        像素坐标,
    }


}
