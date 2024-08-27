using Common;
using HalconDotNet;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    public class ImageSaveParam
    {
        private string _childDolder = "";
        private string folderPath;
        private string extendName = ".bmp";
        private bool addDataTime = false;
        private string fileName;
        private string folderPathNg;

        public string FolderPath
        {
            get
            {
                return folderPath;
            }

            set
            {
                folderPath = value;
            }
        }
        public string ExtendName
        {
            get
            {
                return extendName;
            }

            set
            {
                extendName = value;
            }
        }
        public bool AddDataTime
        {
            get
            {
                return addDataTime;
            }

            set
            {
                addDataTime = value;
            }
        }
        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
            }
        }
        public enSaveMethod SaveMethod { get; set; }
        public bool IsSaveWindowImage { get; set; }
        public bool IsSaveSourceImage { get; set; }
        public bool IsSaveRegionImage { get; set; }
        public bool EnableProductID { get; set; }
        public string WindowName { get; set; }
        public string SavePath { get; set; }
        public string FolderPathNg { get => folderPathNg; set => folderPathNg = value; }

        public bool IsReducedomain { get; set; }
        public ImageSaveParam()
        {
            this.SaveMethod = enSaveMethod.按天保存;
            this.addDataTime = true;
            this.IsSaveWindowImage = false;
            this.IsSaveRegionImage = false;
            this.WindowName = "图像窗口";
            this.IsSaveSourceImage = true;
            this.EnableProductID = false;
            this.folderPathNg = "";
            this.folderPath = "";
            this.IsReducedomain = false;
        }

        public bool SaveImage(ImageDataClass hImage, RegionDataClass regionData, string index)
        {
            bool result = false;
            try
            {
                if (this.FolderPath == null)
                    this.FolderPath = "D:\\图像保存";
                switch (this.SaveMethod)
                {
                    default:
                    case enSaveMethod.按天保存:
                        this._childDolder = DateTime.Now.ToString("yyyyMMdd");
                        if (!Directory.Exists(this.FolderPath + "\\" + this._childDolder))
                            Directory.CreateDirectory(this.FolderPath + "\\" + this._childDolder);
                        break;
                    case enSaveMethod.按周保存:
                        this._childDolder = DateTime.Now.ToString("yyyyMMww");
                        if (!Directory.Exists(this.FolderPath + "\\" + this._childDolder))
                            Directory.CreateDirectory(this.FolderPath + "\\" + this._childDolder);
                        break;
                    case enSaveMethod.按小时保存:
                        this._childDolder = DateTime.Now.ToString("yyyyMMddHH");
                        if (!Directory.Exists(this.FolderPath + "\\" + this._childDolder))
                            Directory.CreateDirectory(this.FolderPath + "\\" + this._childDolder);
                        break;
                }
                ////// 是否按产品ID来存放  ///////
                if (this.EnableProductID)
                {
                    string productID = CommunicationConfigParamManger.Instance.ReadValue(enCoordSysName.CoordSys_0, enCommunicationCommand.ProductID).ToString();
                    string edgePos = CommunicationConfigParamManger.Instance.ReadValue(enCoordSysName.CoordSys_0, enCommunicationCommand.StationNum).ToString();
                    if (productID == null || productID.Length == 0)
                        productID = "123456";
                    if (edgePos == null || edgePos.Length == 0)
                        edgePos = "";
                    if (productID != null && productID.Length > 0)
                        this._childDolder += "\\" + productID;
                    if (edgePos != null && edgePos.Length > 0)
                        this._childDolder += "\\" + "边缘" + edgePos;
                    else
                        this._childDolder += "\\" + "边缘1";
                }
                /////////////////////////////////////////////////////////////////////////////////////////
                string path = "";
                string date = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                if (IsSaveSourceImage)
                {
                    if (addDataTime)
                    {
                        path = this.FolderPath + "\\" + this._childDolder + "\\" + this.fileName + index + "_Source_" + date + this.ExtendName;
                        if (!Directory.Exists(this.FolderPath + "\\" + this._childDolder))
                            Directory.CreateDirectory(this.FolderPath + "\\" + this._childDolder);
                    }
                    else
                    {
                        path = this.FolderPath + "\\" + this._childDolder + "\\" + this.fileName + index + "_Source_" + this.ExtendName;
                        if (!Directory.Exists(this.FolderPath + "\\" + this._childDolder))
                            Directory.CreateDirectory(this.FolderPath + "\\" + this._childDolder);
                    }
                    if (this.IsReducedomain)
                        result = this.WriteImageToFile(hImage.Image.CropDomain(), path);
                    else
                        result = this.WriteImageToFile(hImage, path);
                }
                else
                    result = true;
                this.SavePath = path;
                ///////////////////////////////// 保存缺陷图 /////////////////////
                if (regionData != null && regionData.Region != null && regionData.Region.IsInitialized())
                {
                    if (IsSaveRegionImage)
                    {
                        if (addDataTime)
                        {
                            path = this.FolderPath + "\\" + this._childDolder + "\\" + "FlawImage" + "\\" + this.fileName + index + "_Flaw_" + date + this.ExtendName;
                            if (!Directory.Exists(this.FolderPath + "\\" + this._childDolder))
                                Directory.CreateDirectory(this.FolderPath + "\\" + this._childDolder);
                        }
                        else
                        {
                            path = this.FolderPath + "\\" + this._childDolder + "\\" + "FlawImage" + "\\" + this.fileName + index + "_Flaw_" + this.ExtendName;
                            if (!Directory.Exists(this.FolderPath + "\\" + this._childDolder))
                                Directory.CreateDirectory(this.FolderPath + "\\" + this._childDolder);
                        }
                        result = this.WriteImageToFile(GenFlawImage(regionData?.Region, hImage?.Image), path);
                    }
                    else
                        result = true;
                }
                ///////////////////////////////////////////////////
                if (IsSaveWindowImage) // 保存窗口图像
                {
                    if (this.WindowName != null && HWindowManage.HWindowList.ContainsKey(this.WindowName))
                    {
                        path = this.FolderPath + "\\" + this._childDolder + "\\" + "Window" + "\\" + this.fileName + index + "_" + this.WindowName + "_" + date + this.ExtendName;
                        HImage windowImage = HWindowManage.HWindowList[this.WindowName].DumpWindowImage();
                        HTuple hTuple = windowImage.GetImageType();
                        result = this.WriteImageToFile(windowImage, path);
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool SaveImageNg(ImageDataClass hImage, RegionDataClass regionData)
        {
            bool result = false;
            try
            {
                if (this.FolderPathNg == null)
                    this.FolderPathNg = "D:\\图像保存";
                switch (this.SaveMethod)
                {
                    default:
                    case enSaveMethod.按天保存:
                        this._childDolder = DateTime.Now.ToString("yyyyMMdd");
                        if (!Directory.Exists(this.FolderPathNg + "\\" + this._childDolder))
                            Directory.CreateDirectory(this.FolderPathNg + "\\" + this._childDolder);
                        break;
                    case enSaveMethod.按周保存:
                        this._childDolder = DateTime.Now.ToString("yyyyMMww");
                        if (!Directory.Exists(this.FolderPathNg + "\\" + this._childDolder))
                            Directory.CreateDirectory(this.FolderPathNg + "\\" + this._childDolder);
                        break;
                    case enSaveMethod.按小时保存:
                        this._childDolder = DateTime.Now.ToString("yyyyMMddHH");
                        if (!Directory.Exists(this.FolderPathNg + "\\" + this._childDolder))
                            Directory.CreateDirectory(this.FolderPathNg + "\\" + this._childDolder);
                        break;
                }
                //////////////////  用区域来判断 是否有缺陷  ///////////////////////////////////////////
                if (regionData == null || regionData.Region == null || !regionData.Region.IsInitialized())
                    this._childDolder += "\\" + "SourceImage";
                else
                    this._childDolder += "\\" + "FlawImage";
                ////// 是否按产品ID来存放  ///////
                if (this.EnableProductID)
                {
                    string productID = CommunicationConfigParamManger.Instance.ReadValue(enCoordSysName.CoordSys_0, enCommunicationCommand.ProductID).ToString();
                    string edgePos = CommunicationConfigParamManger.Instance.ReadValue(enCoordSysName.CoordSys_0, enCommunicationCommand.StationNum).ToString();
                    if (productID == null || productID.Length == 0)
                        productID = "123456";
                    if (edgePos == null || edgePos.Length == 0)
                        edgePos = "";
                    if (productID != null && productID.Length > 0)
                        this._childDolder += "\\" + productID;
                    if (edgePos != null && edgePos.Length > 0)
                        this._childDolder += "\\" + "边缘" + edgePos;
                }
                /////////////////////////////////////////////////////////////////////////////////////////
                string path = "";
                string date = DateTime.Now.ToString("yyyyMMddHHmmssffff");

                if (addDataTime)
                {
                    path = this.FolderPathNg + "\\" + this._childDolder + "\\" + this.fileName + date + this.ExtendName;
                    if (!Directory.Exists(this.FolderPathNg + "\\" + this._childDolder))
                        Directory.CreateDirectory(this.FolderPathNg + "\\" + this._childDolder);
                }
                else
                {
                    path = this.FolderPathNg + "\\" + this._childDolder + "\\" + this.fileName + this.ExtendName;
                    if (!Directory.Exists(this.FolderPathNg + "\\" + this._childDolder))
                        Directory.CreateDirectory(this.FolderPathNg + "\\" + this._childDolder);
                }
                if (IsSaveSourceImage)
                    result = this.WriteImageToFile(hImage, path);
                else
                    result = true;
                this.SavePath = path;
                ///////////////////////////////// 保存缺陷图 /////////////////////
                if (IsSaveRegionImage)
                {
                    result = this.WriteImageToFile(GenFlawImage(regionData?.Region, hImage?.Image), path);
                }
                else
                    result = true;
                ///////////////////////////////////////////////////
                if (IsSaveWindowImage) // 保存窗口图像
                {
                    if (this.WindowName != null && HWindowManage.HWindowList.ContainsKey(this.WindowName))
                    {
                        path = this.FolderPathNg + "\\" + this._childDolder + "\\" + "Window-" + this.WindowName + "-" + date + this.ExtendName;
                        HImage windowImage = HWindowManage.HWindowList[this.WindowName].DumpWindowImage();
                        HTuple hTuple = windowImage.GetImageType();
                        result = this.WriteImageToFile(windowImage, path);
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        private HImage GenFlawImage(HObject hObject, HImage sourceImage)
        {
            //HImage flawImge = new HImage();
            //if (hObject == null || !hObject.IsInitialized()) return sourceImage;
            //if (sourceImage == null || !sourceImage.IsInitialized()) return sourceImage;
            //switch (hObject.GetType().Name)
            //{
            //    case nameof(HXLDCont):
            //        HXLDCont hXLDCont = new HXLDCont(hObject);
            //        HImage redImage = hXLDCont.PaintXld(sourceImage, 255.0);
            //        HImage darkImage = hXLDCont.PaintXld(sourceImage, 0.0);
            //        flawImge = redImage.Compose3(darkImage, darkImage);
            //        break;
            //    case nameof(HRegion):
            //        HRegion hRegion = new HRegion(hObject);
            //        redImage = hRegion.PaintRegion(sourceImage, 255.0, "margin");
            //        darkImage = hRegion.PaintRegion(sourceImage, 0.0, "margin");
            //        flawImge = redImage.Compose3(darkImage, darkImage);
            //        break;
            //    default:
            //        throw new ArgumentException("hObject 参数类型错误，请指定为 HRegion 或 HXLDCont!");
            //}
            //return flawImge;

            HImage flawImge = new HImage();
            if (hObject == null || !hObject.IsInitialized()) return sourceImage;
            if (sourceImage == null || !sourceImage.IsInitialized()) return sourceImage;
            HImage hImageSourceCopy = sourceImage.Clone();
            switch (hObject.GetType().Name)
            {
                case nameof(HXLDCont):
                    HXLDCont hXLDCont = new HXLDCont(hObject);
                    HImage redImage = hXLDCont.PaintXld(hImageSourceCopy, 255.0);
                    HImage darkImage = hXLDCont.PaintXld(hImageSourceCopy, 0.0);
                    flawImge = redImage.Compose3(darkImage, darkImage);
                    break;
                case nameof(HRegion):
                    HRegion hRegion = new HRegion(hObject);
                    redImage = hRegion.PaintRegion(hImageSourceCopy, 255.0, "margin");
                    darkImage = hRegion.PaintRegion(hImageSourceCopy, 0.0, "margin");
                    flawImge = redImage.Compose3(darkImage, darkImage);
                    break;
                default:
                    throw new ArgumentException("hObject 参数类型错误，请指定为 HRegion 或 HXLDCont!");
            }
            return flawImge;
        }
        private bool WriteImageToFile(object objectModel, string path)
        {
            bool result = false;
            FileOperate fo = new FileOperate();
            string[] extenName = path.Split('.');
            //////////////////////
            if (objectModel == null) return result;
            ////////////////////////
            switch (objectModel.GetType().Name)
            {
                case "ImageDataClass": //
                    if (!Directory.Exists(new FileInfo(path).DirectoryName))
                        Directory.CreateDirectory(new FileInfo(path).DirectoryName);
                    if (((ImageDataClass)objectModel).Image == null || !((ImageDataClass)objectModel).Image.IsInitialized()) return result;
                    if (extenName[1] == "jpg")
                        HOperatorSet.WriteImage(((ImageDataClass)objectModel).Image, "jpg", 0, path);
                    if (extenName[1] == "jpeg")
                        HOperatorSet.WriteImage(((ImageDataClass)objectModel).Image, "jpeg", 0, path);
                    if (extenName[1] == "bmp")
                        HOperatorSet.WriteImage(((ImageDataClass)objectModel).Image, "bmp", 0, path);
                    if (extenName[1] == "tiff")
                        HOperatorSet.WriteImage(((ImageDataClass)objectModel).Image, "tiff", 0, path);
                    if (extenName[1] == "png")
                        HOperatorSet.WriteImage(((ImageDataClass)objectModel).Image, "png", 0, path);
                    break;
                case "HImage": //
                    if (!Directory.Exists(new FileInfo(path).DirectoryName))
                        Directory.CreateDirectory(new FileInfo(path).DirectoryName);
                    if (((HImage)objectModel) == null || !((HImage)objectModel).IsInitialized()) return result;
                    if (extenName[1] == "jpg")
                        HOperatorSet.WriteImage(((HImage)objectModel), "jpg", 0, path);
                    if (extenName[1] == "jpeg")
                        HOperatorSet.WriteImage(((HImage)objectModel), "jpeg", 0, path);
                    if (extenName[1] == "bmp")
                        HOperatorSet.WriteImage(((HImage)objectModel), "bmp", 0, path);
                    if (extenName[1] == "tiff")
                        HOperatorSet.WriteImage(((HImage)objectModel), "tiff", 0, path);
                    if (extenName[1] == "png")
                        HOperatorSet.WriteImage(((HImage)objectModel), "png", 0, path);
                    break;
                case "HRegion":
                    if (!Directory.Exists(new FileInfo(path).DirectoryName))
                        Directory.CreateDirectory(new FileInfo(path).DirectoryName);
                    if (!((HRegion)objectModel).IsInitialized()) return result;
                    if (extenName[1] == "hobj")
                        HOperatorSet.WriteRegion((HRegion)objectModel, path);
                    break;
            }
            result = true;
            return result;

        }


    }
}
