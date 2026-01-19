using Common;
using FunctionBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;


public class ImageAcqScript : IScript
{

    public bool RunScript(IFunction function, params object[] param)
    {
        bool result = false;
        try
        {
            ImageAcq imageAcq = function as ImageAcq;
            HRegion hRegion = imageAcq.ImageData.Image.VarThreshold(25, 25, 0.2, 10, "light");
            HRegion selectRegion = hRegion.Connection().SelectShape("area", "and", 15000, int.MaxValue);
            imageAcq.ImageData.Image = selectRegion.PaintRegion(imageAcq.ImageData.Image, 0.0, "fill");
            result = true;
        }
        catch (Exception ex)
        {
            result = false;
            new UserMessageForm().ShowDialog("图像采集脚本执行报错" + ex.ToString());
        }
        return result;
    }

    public object GetScriptPropertyValues(string propertyName)
    {
        throw new NotImplementedException();
    }

    public bool SetScriptPropertyValues(string propertyName, object value)
    {
        throw new NotImplementedException();
    }



}

