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
    public class DoDataCodeDetection
    {
        [NonSerialized]
        [XmlIgnore]
        private HDataCode2D hDataCode;
        private DataCodeParam _codeParam;
        private enSymbolType _symbolType;
        public FindDataCodeParam FindParam { get; set; }
        public DataCodeParam CodeParam
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

        public enSymbolType SymbolType
        {
            get => _symbolType;
            set
            {
                _symbolType = value;
                switch (_symbolType)
                {
                    default:
                    case enSymbolType.GS1_DataMatrix:
                    case enSymbolType.Data_Matrix_ECC_200:
                        this._codeParam = new ECC200DataCodeParam();
                        break;
                    case enSymbolType.GS1_Aztec_Code:
                    case enSymbolType.Aztec_Code:
                        this._codeParam = new AztecDataCodeParam();
                        break;
                    case enSymbolType.Micro_QR_Code:
                        this._codeParam = new MicroQRDataCodeParam();
                        break;
                    case enSymbolType.PDF417:
                        this._codeParam = new PDF417DataCodeParam();
                        break;
                    case enSymbolType.GS1_QR_Code:
                    case enSymbolType.QR_Code:
                        this._codeParam = new QRDataCodeParam();
                        break;
                    case enSymbolType.DotCode:
                    case enSymbolType.GS1_DotCode:
                        this._codeParam = new DotDataCodeParam();
                        break;
                }
            }
        }

        public HDataCode2D HDataCode { get => hDataCode; set => hDataCode = value; }

        public DoDataCodeDetection()
        {
            this._symbolType = enSymbolType.Data_Matrix_ECC_200;
            this.FindParam = new FindDataCodeParam();
            this.CodeParam = new ECC200DataCodeParam();
            this.CreateDataCode2D(); // 默认创建数据码模型
        }
        public bool CreateDataCode2D()
        {
            if (hDataCode == null || !hDataCode.IsInitialized())
            {
                this.hDataCode = DataCodeMethod.Instance.create_data_code_2d_model(this.SymbolType);
            }
            else
            {
                this.hDataCode.ClearDataCode2dModel();
                this.hDataCode = DataCodeMethod.Instance.create_data_code_2d_model(this.SymbolType);
            }
            //////////////////////////////////////////////////////
            return true;
        }

        public bool FindDataCode2D(HImage hImage, out DataCodeResult codeResult)
        {
            codeResult = new DataCodeResult();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            //////////////////////////////////////////////////////
            DataCodeMethod.Instance.find_data_code_2d(this.hDataCode, hImage, this.FindParam, out codeResult);
            ////////////////////////////////////////////////
            return true;
        }
        public bool TrainDataCode2D(HImage hImage, out DataCodeResult codeResult)
        {
            codeResult = new DataCodeResult();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            //////////////////////////////////////////////////////
            DataCodeMethod.Instance.train_data_code_2d(this.hDataCode, hImage, this.FindParam, out codeResult);
            ////////////////////////////////////////////////
            return true;
        }
        public bool Reset_data_code_2d_param()
        {
            if (this.hDataCode == null || !this.hDataCode.IsInitialized())
                throw new ArgumentNullException(" this.hDataCode 为空或未被初始化");
            if (this._codeParam == null)
                throw new ArgumentNullException(" this._codeParam 为空或未被初始化");
            ////////////////////////////////////////////////////////////////////
            try
            {
                this.FindParam.default_parameters = this._codeParam.default_parameters; // 寻找的相应参数也要更改
                this.hDataCode.SetDataCode2dParam("default_parameters", this._codeParam.default_parameters);
                HTuple paramName = this.hDataCode.QueryDataCode2dParams("get_model_params");
                HTuple paramValue = this.hDataCode.GetDataCode2dParam(paramName);
                Type type = this._codeParam.GetType();
                for (int i = 0; i < paramName.Length; i++)
                {
                    PropertyInfo propertyInfo = type.GetProperty(paramName[i].S);
                    if (propertyInfo != null)
                    {
                        switch (propertyInfo.GetValue(this._codeParam).GetType().Name)
                        {
                            case nameof(Double):
                                double result2 = 0;
                                if (double.TryParse(paramValue[i].O.ToString(), out result2))
                                {
                                    propertyInfo.SetValue(this._codeParam, result2);
                                }
                                break;
                            case nameof(String):
                                propertyInfo.SetValue(this._codeParam, paramValue[i].O.ToString());
                                break;
                            case nameof(Int32):
                                int result = 0;
                                if (int.TryParse(paramValue[i].O.ToString(), out result))
                                {
                                    propertyInfo.SetValue(this._codeParam, result);
                                }
                                else
                                {
                                    if(propertyInfo.Name == "timeout")
                                    {
                                        propertyInfo.SetValue(this._codeParam, 5000);
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            ///////////////////////////////
            return true;
        }

        public bool updata_data_code_2d_param()
        {
            if (this.hDataCode == null || !this.hDataCode.IsInitialized())
                throw new ArgumentNullException(" this.hDataCode 为空或未被初始化");
            if (this._codeParam == null)
                throw new ArgumentNullException(" this._codeParam 为空或未被初始化");
            ////////////////////////////////////////////////////////////////////
            try
            {
                HTuple paramName = this.hDataCode.QueryDataCode2dParams("get_model_params");
                HTuple paramValue = this.hDataCode.GetDataCode2dParam(paramName);
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
                ////////////////
                this.hDataCode.SetDataCode2dParam(paramName, paramValue);
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
            if (this.hDataCode != null && this.hDataCode.IsInitialized())
                this.hDataCode.ClearDataCode2dModel();
        }

        public void SaveDataCodeModel(string filePath)
        {
            if (this.hDataCode != null && this.hDataCode.IsInitialized())
                this.hDataCode.WriteDataCode2dModel(filePath);
            //////////////////////////////////////////////////
        }

        public void ReadDataCodeModel(string filePath)
        {
            if (this.hDataCode == null)
                this.hDataCode = new HDataCode2D();
            else
            {
                if (this.hDataCode.IsInitialized())
                    this.hDataCode.ClearDataCode2dModel();
                this.hDataCode = new HDataCode2D();
            }
            if (File.Exists(filePath))
                this.hDataCode.ReadDataCode2dModel(filePath);

        }


    }
}
