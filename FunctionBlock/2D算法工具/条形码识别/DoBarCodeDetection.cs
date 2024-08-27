using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FunctionBlock
{
    /// <summary>
    /// 相当于一个控制器
    /// </summary>
    [Serializable]
    public class DoBarCodeDetection
    {
        [NonSerialized]
        [XmlIgnore]
        private HBarCode hBarCode;
        private BarCodeParam _codeParam;
        public FindBarCodeParam FindParam { get; set; }
        public BarCodeParam CodeParam
        {
            get
            {
                return _codeParam;
            }
            set
            {
                _codeParam = value;
            }
        }

        public HBarCode HBarCode { get => hBarCode; set => hBarCode = value; }

        public DoBarCodeDetection()
        {
            this.FindParam = new FindBarCodeParam();
            this.CodeParam = new BarCodeParam();
            this.hBarCode = new HBarCode(new HTuple(),new HTuple());
        }


        public bool FindBarCode2D(HImage hImage, out BarCodeResult codeResult)
        {
            codeResult = new BarCodeResult();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            //////////////////////////////////////////////////////
            BarCodeMethod.Instance.find_bar_code_2d(this.hBarCode, hImage, this.FindParam, out codeResult);
            ////////////////////////////////////////////////
            return true;
        }

        public bool TrainBarCode2D(HImage hImage, out BarCodeResult codeResult)
        {
            codeResult = new BarCodeResult();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            //////////////////////////////////////////////////////
            BarCodeMethod.Instance.train_bar_code_2d(this.hBarCode, hImage, this.FindParam, out codeResult);
            ////////////////////////////////////////////////
            return true;
        }
 
        public bool updata_data_code_2d_param()
        {
            if (this.hBarCode == null || !this.hBarCode.IsInitialized())
                throw new ArgumentNullException(" this.hDataCode 为空或未被初始化");
            if (this._codeParam == null)
                throw new ArgumentNullException(" this._codeParam 为空或未被初始化");
            ////////////////////////////////////////////////////////////////////
            try
            {
                HTuple paramName = this.hBarCode.QueryBarCodeParams("get_model_params");
                HTuple paramValue = this.hBarCode.GetBarCodeParam(paramName);
                Type type = this._codeParam.GetType();
                for (int i = 0; i < paramName.Length; i++)
                {
                    PropertyInfo propertyInfo = type.GetProperty(paramName[i].S);
                    if (propertyInfo != null)
                    {
                        object value = propertyInfo.GetValue(this._codeParam);
                        switch (value.GetType().Name)
                        {
                            case nameof(Double):
                                double result2 = 0;
                                if (double.TryParse(value.ToString(), out result2))
                                {
                                    paramValue[i] = result2;
                                }
                                break;
                            case nameof(String):
                                paramValue[i] = value.ToString();
                                break;
                            case nameof(Int32):
                                int result = 0;
                                if (int.TryParse(value.ToString(), out result))
                                {
                                    paramValue[i] = result;
                                }
                                break;
                        }
                    }
                }
                /////////////////////////////////////////////
                this.hBarCode.SetBarCodeParam(paramName, paramValue);
            }
            catch (Exception ex)
            {
                return false;
            }
            ///////////////////////////////
            return true;
        }

        public void ClearDataCode2dModel()
        {
            if (this.hBarCode != null && this.hBarCode.IsInitialized())
                this.hBarCode.ClearBarCodeModel();
        }

        public void SaveDataCodeModel(string filePath)
        {
            if (this.hBarCode != null && this.hBarCode.IsInitialized())
                this.hBarCode.WriteBarCodeModel(filePath);
            //////////////////////////////////////////////////
        }

        public void ReadDataCodeModel(string filePath)
        {
            if (this.hBarCode == null)
                this.hBarCode = new HBarCode();
            else
            {
                if (this.hBarCode.IsInitialized())
                    this.hBarCode.ClearBarCodeModel();
                this.hBarCode = new HBarCode();
            }
            if (File.Exists(filePath))
                this.hBarCode.ReadBarCodeModel(filePath);
        }



    }
}
