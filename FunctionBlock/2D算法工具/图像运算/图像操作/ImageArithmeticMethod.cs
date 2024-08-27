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
    public class ImageArithmeticMethod
    {
        //private static object lockState = new object();
        //private static ImageArithmeticMethod _Instance = null;
        public ImageArithmeticMethod()
        {

        }

        //public static ImageArithmeticMethod Instance
        //{
        //    get
        //    {
        //        if (_Instance == null)
        //        {
        //            lock (lockState)
        //            {
        //                if (_Instance == null)
        //                    _Instance = new ImageArithmeticMethod();
        //            }
        //        }
        //        return _Instance;
        //    }
        //}
        public HImage ArithmeticImage(HImage hImage1, HImage hImage2, ImageArithmeticParam param)
        {
            HImage tarRegion = new HImage();
            if (hImage1 == null)
                throw new ArgumentNullException("hImage1");
            if (hImage2 == null)
                throw new ArgumentNullException("hImage2");
            if (param == null)
                throw new ArgumentNullException("param");
            /////////////////////////////////
            switch (param.Method)
            {
                case nameof(enImageArithmeticMethod.add_image):
                    tarRegion = add_image(hImage1, hImage2,(MultAddParam)param);
                    break;
                case nameof(enImageArithmeticMethod.sub_image):
                    tarRegion = sub_image(hImage1, hImage2, (MultAddParam)param);
                    break;
                case nameof(enImageArithmeticMethod.mult_image):
                    tarRegion = mult_image(hImage1, hImage2, (MultAddParam)param);
                    break;
                case nameof(enImageArithmeticMethod.div_image):
                    tarRegion = div_image(hImage1, hImage2, (MultAddParam)param);
                    break;
            }
            return tarRegion;
        }


        private HImage add_image(HImage hImage1, HImage hImage2, MultAddParam param)
        {
            HImage tarImage = new HImage();
            if (hImage1 == null)
                throw new ArgumentNullException("hImage1");
            if (hImage2 == null)
                throw new ArgumentNullException("hImage2");
            tarImage = hImage1.AddImage(hImage2, param.Mult, param.Add);
            return tarImage;
        }
        private HImage sub_image(HImage hImage1, HImage hImage2, MultAddParam param)
        {
            HImage tarImage = new HImage();
            if (hImage1 == null)
                throw new ArgumentNullException("hImage1");
            if (hImage2 == null)
                throw new ArgumentNullException("hImage2");
            ///////////////////////////////////////////////////
            tarImage = hImage1.SubImage(hImage2, param.Mult, param.Add);
            return tarImage;
        }
        private HImage mult_image(HImage hImage1, HImage hImage2, MultAddParam param)
        {
            HImage tarImage = new HImage();
            if (hImage1 == null)
                throw new ArgumentNullException("hImage1");
            if (hImage2 == null)
                throw new ArgumentNullException("hImage2");
            ///////////////////////////////////////////////////
            tarImage = hImage1.MultImage(hImage2, param.Mult, param.Add);
            return tarImage;
        }
        private HImage div_image(HImage hImage1, HImage hImage2, MultAddParam param)
        {
            HImage tarImage = new HImage();
            if (hImage1 == null)
                throw new ArgumentNullException("hImage1");
            if (hImage2 == null)
                throw new ArgumentNullException("hImage2");
            ///////////////////////////////////////////////////
            tarImage = hImage1.DivImage(hImage2, param.Mult, param.Add);
            return tarImage;
        }



    }

    public enum enImageArithmeticMethod
    {
        add_image,
        div_image,
        mult_image,
        sub_image,
    }





}
