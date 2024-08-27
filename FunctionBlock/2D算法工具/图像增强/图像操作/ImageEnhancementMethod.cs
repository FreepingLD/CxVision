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
    public class ImageEnhancementMethod
    {
        //private static object lockState = new object();
        //private static ImageEnhancementMethod _Instance = null;
        public ImageEnhancementMethod()
        {

        }

        //public static ImageEnhancementMethod Instance
        //{
        //    get
        //    {
        //        if (_Instance == null)
        //        {
        //            lock (lockState)
        //            {
        //                if (_Instance == null)
        //                    _Instance = new ImageEnhancementMethod();
        //            }
        //        }
        //        return _Instance;
        //    }
        //}

        public HImage ImageEnhancement(HImage hImage, ImageEnhancementParam param)
        {
            HImage tarHImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            ////////////////////////////////////////////////
            switch (param.Method.Trim())
            {
                case nameof(enImageEnhancementMethod.coherence_enhancing_diff):
                    tarHImage = coherence_enhancing_diff(hImage, (CoherenceEnhancingDiffParam)param);
                    break;
                case nameof(enImageEnhancementMethod.emphasize):
                    tarHImage = emphasize(hImage, (EmphasizeParam)param);
                    break;
                case nameof(enImageEnhancementMethod.equ_histo_image):
                    tarHImage = equ_histo_image(hImage, (EquHistoImageParam)param);
                    break;
                case nameof(enImageEnhancementMethod.illuminate):
                    tarHImage = illuminate(hImage, (IlluminateParam)param);
                    break;
                case nameof(enImageEnhancementMethod.mean_curvature_flow):
                    tarHImage = mean_curvature_flow(hImage, (MeanCurvatureFlowParam)param);
                    break;
                case nameof(enImageEnhancementMethod.scale_image_max):
                    tarHImage = scale_image_max(hImage, (ScaleImageMaxParam)param);
                    break;
                case nameof(enImageEnhancementMethod.scale_image):
                    tarHImage = scale_image(hImage, (ScaleImageMaxParam)param);
                    break;
                case nameof(enImageEnhancementMethod.shock_filter):
                    tarHImage = shock_filter(hImage, (ShockFilterParam)param);
                    break;
            }
            return tarHImage;
        }


        private HImage emphasize(HImage hImage, EmphasizeParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.Emphasize((int)param.MaskWidth, (int)param.MaskHeight, param.Factor);
            return tarImage;
        }
        private HImage coherence_enhancing_diff(HImage hImage, CoherenceEnhancingDiffParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.CoherenceEnhancingDiff(param.Sigma, param.Rho, param.Theta, param.Iterations);
            return tarImage;
        }
        private HImage equ_histo_image(HImage hImage, EquHistoImageParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.EquHistoImage();
            return tarImage;
        }
        private HImage illuminate(HImage hImage, IlluminateParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.Illuminate((int)param.MaskWidth, (int)param.MaskHeight, param.Factor);
            return tarImage;
        }
        private HImage mean_curvature_flow(HImage hImage, MeanCurvatureFlowParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.MeanCurvatureFlow(param.Sigma, param.Theta, param.Iterations);
            return tarImage;
        }
        private HImage scale_image_max(HImage hImage, ScaleImageMaxParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.ScaleImageMax();

            return tarImage;
        }
        private HImage scale_image(HImage hImage, ScaleImageMaxParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            double Mult = 255.0 / (param.Max - param.Min);
            double Add = -Mult * param.Min;
            tarImage = hImage.ScaleImage(Mult, Add);
            return tarImage;
        }
        private HImage shock_filter(HImage hImage, ShockFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.ShockFilter(param.Theta, param.Iterations, param.Mode, param.Sigma);
            return tarImage;
        }

    }

    public enum enImageEnhancementMethod
    {
        emphasize,
        coherence_enhancing_diff,
        equ_histo_image,
        illuminate,
        mean_curvature_flow,
        scale_image_max,
        scale_image,
        shock_filter,
    }





}
