
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sensor
{
    [Serializable]
    public  class BoMingStructuredLightParams
    {
        private AlgoParas[] bmPcdProcessParams = new AlgoParas[2];      // 点云处理参数
        private FilterParas bmHighQualityParams=new FilterParas() ; // 高精度参数
        private BmsIoOutPara bmIoOutPara=new BmsIoOutPara();
        private ImageROI bmRoiParams=new ImageROI();
        private enBmsTriggerMode bmTriggerMode=enBmsTriggerMode.SoftwareTrigger;
        private float gain = 1;
        private byte ledLight = 200;
        private short expose = 20;
        private bool enbaledROI=false;
        private bool bmHighSpeedEnabled = false;  // 高速率使能(下采样开启)
        private bool bmHighQualityEnabled = true; // 高精度使能(高精度开启)
        private bool bmSyncScan = true;  // 是否同步方式
        public AlgoParas[] BmPcdProcessParams
        {
            get
            {
                return bmPcdProcessParams;
            }

            set
            {
                bmPcdProcessParams = value;
            }
        }
        public FilterParas BmHighQualityParams
        {
            get
            {
                return bmHighQualityParams;
            }

            set
            {
                bmHighQualityParams = value;
            }
        }
        public BmsIoOutPara BmIoOutPara
        {
            get
            {
                return bmIoOutPara;
            }

            set
            {
                bmIoOutPara = value;
            }
        }
        public ImageROI BmRoiParams
        {
            get
            {
                return bmRoiParams;
            }

            set
            {
                bmRoiParams = value;
            }
        }
        public enBmsTriggerMode BmTriggerMode
        {
            get
            {
                return bmTriggerMode;
            }

            set
            {
                bmTriggerMode = value;
            }
        }
        public float Gain
        {
            get
            {
                return gain;
            }

            set
            {
                gain = value;
            }
        }
        public byte LedLight
        {
            get
            {
                return ledLight;
            }

            set
            {
                ledLight = value;
            }
        }
        public short Expose
        {
            get
            {
                return expose;
            }

            set
            {
                expose = value;
            }
        }
        public bool EnbaledROI
        {
            get
            {
                return enbaledROI;
            }

            set
            {
                enbaledROI = value;
            }
        }
        public bool BmHighSpeedEnabled
        {
            get
            {
                return bmHighSpeedEnabled;
            }

            set
            {
                bmHighSpeedEnabled = value;
            }
        }
        public bool BmHighQualityEnabled
        {
            get
            {
                return bmHighQualityEnabled;
            }

            set
            {
                bmHighQualityEnabled = value;
            }
        }
        public bool BmSyncScan
        {
            get
            {
                return bmSyncScan;
            }

            set
            {
                bmSyncScan = value;
            }
        }

        public  BoMingStructuredLightParams Read(string configFileName)
        {
            string path = configFileName;
            if (!File.Exists(path)) return null;
            BinaryFormatter binFormat = new BinaryFormatter();
            try
            {
                using (Stream fStream = File.OpenRead(path))
                {
                    return (BoMingStructuredLightParams)binFormat.Deserialize(fStream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }
        public void Save(string configFileName)
        {
            string path = configFileName;
            BinaryFormatter binFormat = new BinaryFormatter();
            try
            {
                using (FileStream fStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    binFormat.Serialize(fStream, this);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


    }




}
