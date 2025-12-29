using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;

namespace FunctionBlock
{
    [Serializable]
    public class SelectMethod
    {

        public SelectMethod()
        {

        }

        public HRegion FlawSelect(HRegion hRegion, SelectFlawParam param)
        {
            HRegion tarHImage = new HRegion();
            switch (param.Method)
            {
                case nameof(enFlawSelectMethod.select_shape):
                    tarHImage = select_shape(hRegion, (SelectShapeFlawParam)param);
                    break;
                case nameof(enFlawSelectMethod.select_shape_proto):
                    tarHImage = select_shape_proto(hRegion, (SelectShapeProtoFlawParam)param);
                    break;
                case nameof(enFlawSelectMethod.select_shape_std):
                    tarHImage = select_shape_std(hRegion, (SelectShapeStdFlawParam)param);
                    break;
                default:
                case nameof(enFlawSelectMethod.none):
                    tarHImage = hRegion.Clone();
                    break;
            }
            ////////////////////////////////
            return tarHImage;
        }


        private HRegion select_shape(HRegion hRegion, SelectShapeFlawParam param)
        {
            HRegion tarRegion = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            if (param == null)
                throw new ArgumentNullException("param");
            string[] featuresValue = param.Features.Split(',', ';', ':');
            string[] minValue = param.Min.Split(',', ';', ':');
            string[] maxValue = param.Max.Split(',', ';', ':');
            if (featuresValue.Length != minValue.Length || featuresValue.Length != maxValue.Length)
                throw new ArgumentException("参数名称与参数值长度不相等!");
            ///////////////////////////////////////////////////////////////
            HTuple features = new HTuple();
            HTuple min = new HTuple();
            HTuple max = new HTuple();
            for (int i = 0; i < featuresValue.Length; i++)
            {
                double result = 0;
                features[i] = featuresValue[i].Trim();
                if (double.TryParse(minValue[i], out result))
                    min[i] = result;
                else
                    min[i] = minValue[i].Trim();
                ////////////////////////////////////
                if (double.TryParse(maxValue[i], out result))
                    max[i] = result;
                else
                    max[i] = maxValue[i].Trim();
            }
            tarRegion = hRegion.Connection().SelectShape(new HTuple(features), param.Operation, new HTuple(min), new HTuple(max));
            ////////////////////////////////
            return tarRegion;
        }

        private HRegion select_shape_proto(HRegion hRegion, SelectShapeProtoFlawParam param)
        {
            HRegion tarRegion = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            if (param == null)
                throw new ArgumentNullException("param");
            string[] featuresValue = param.Features.Split(',', ';', ':');
            string[] minValue = param.Min.Split(',', ';', ':');
            string[] maxValue = param.Max.Split(',', ';', ':');
            if (featuresValue.Length != minValue.Length || featuresValue.Length != maxValue.Length)
                throw new ArgumentException("参数名称与参数值长度不相等!");
            ///////////////////////////////////////////////////////////////
            HTuple features = new HTuple();
            HTuple min = new HTuple();
            HTuple max = new HTuple();
            for (int i = 0; i < featuresValue.Length; i++)
            {
                double result = 0;
                features[i] = featuresValue[i].Trim();
                if (double.TryParse(minValue[i], out result))
                    min[i] = result;
                else
                    min[i] = minValue[i].Trim();
                ////////////////////////////////////
                if (double.TryParse(maxValue[i], out result))
                    max[i] = result;
                else
                    max[i] = maxValue[i].Trim();
            }
            tarRegion = hRegion.Connection().SelectShapeProto(new HRegion(), new HTuple(features), new HTuple(min), new HTuple(max));
            ////////////////////////////////
            return tarRegion;
        }

        private HRegion select_shape_std(HRegion hRegion, SelectShapeStdFlawParam param)
        {
            HRegion tarRegion = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            if (param == null)
                throw new ArgumentNullException("param");
            string[] featuresValue = param.Features.Split(',', ';', ':');
            string[] percentValue = param.Percent.Split(',', ';', ':');
            if (featuresValue.Length != percentValue.Length )
                throw new ArgumentException("参数名称与参数值长度不相等!");
            ///////////////////////////////////////////////////////////////
            HTuple features = new HTuple();
            HTuple percent = new HTuple();
            for (int i = 0; i < featuresValue.Length; i++)
            {
                double result = 0;
                features[i] = featuresValue[i].Trim();
                if (double.TryParse(percentValue[i], out result))
                    percent[i] = result;
                else
                    percent[i] = percentValue[i].Trim();
            }
            tarRegion = hRegion.Connection().SelectShapeStd(new HTuple(features),new HTuple(percent));
            ////////////////////////////////
            return tarRegion;
        }



    }

    public enum enFlawSelectMethod
    {
        none,
        select_shape,
        select_shape_proto,
        select_shape_std,
    }



}
