using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class SaveDataFtpParam
    {
        private string _saveFolderPath;
        public string SaveFolderPath
        {
            get
            {
                return _saveFolderPath;
            }

            set
            {
                _saveFolderPath = value;
            }
        }
        public string FilePath
        {
            get;
            set;
        }
        public string ImagePath
        {
            get;
            set;
        }
        public string PanelPath
        {
            get;
            set;
        }
        public string DmyPath
        {
            get;
            set;
        }

        public string FtpServerIp { set; get; }
        public string FtpURI { set; get; }
        public string Code { set; get; } // 工站号
        public string ShopCode { set; get; } // 工站号
        public string Step_ID { set; get; } // 工站号
        public string EQP_ID { set; get; } // 工站号
        public string UnitID { set; get; } // 工站号
        public string SubUnitID { set; get; } // 工站号
        public string DEFECTCODE { get; set; }
        public SaveDataFtpParam()
        {
            this.FtpServerIp = "10.102.160.92";
            this.FtpURI = "ftp://" + this.FtpServerIp;
            this.SaveFolderPath = "E:\\FtpData";
            this.ShopCode = "MOUDLE-A";
            this.Code = "MOUDLEA";
            this.Step_ID = "M1A2MOLB07";
            this.EQP_ID = "M1A2MOLB07PAI100"; //
            this.UnitID = "M1A2MOLB07ULD101";
            this.SubUnitID = "M1A2MOLB07ULD10101";
            this.DEFECTCODE = "F39";
        }

        public bool WriteData(string panel_ID, string[] imageFilePath, object[] data)
        {
            bool result = false;
            this.FtpURI = "ftp://" + this.FtpServerIp;
            try
            {
                if (this.SaveFolderPath == null || !Directory.Exists("E:"))
                    this.SaveFolderPath = "D:\\FtpData";
                if (!Directory.Exists(this.SaveFolderPath))
                    Directory.CreateDirectory(this.SaveFolderPath);
                /////////////////////////////////////////////////////
                List<string> remoteImageList = new List<string>();
                List<string> localImageList = new List<string>();
                string path = "", remotePath = "", first = "";
                int i = 0;
                foreach (var item in imageFilePath)
                {
                  
                    if (first != item)
                    {
                        i++;
                        first = item;
                        path = SaveImageFileToLocal(item, panel_ID,i);
                        remotePath = path.Replace(this.SaveFolderPath, this.FtpURI).Replace("\\", "/");
                    }
                    remoteImageList.Add(remotePath);
                    localImageList.Add(path);
                }
                if (remoteImageList.Count > 0)
                    this.ImagePath = remoteImageList.Last();
                this.PanelPath = SavePanelFileToLocal(panel_ID, remoteImageList.ToArray(), data);
                this.DmyPath = SaveDmyFileToLocal(panel_ID);
                string DmyPath2 = SaveDmyFileToLocal2(panel_ID);
                ///////////////////////////////////////////////
                if (!Directory.Exists(this.SaveFolderPath + "\\" + "上传文件"))
                    Directory.CreateDirectory(this.SaveFolderPath + "\\" + "上传文件");
                using (StreamWriter sw = new StreamWriter(this.SaveFolderPath + "\\" + "上传文件" + "\\" + panel_ID + ".txt"))
                {
                    sw.WriteLine(this.DmyPath);
                    sw.WriteLine(DmyPath2);
                    sw.WriteLine(this.PanelPath);
                    first = "";
                    foreach (var item in localImageList)
                    {
                        if (first != item)
                        {
                            first = item;
                            sw.WriteLine(item);
                        }
                    }
                }
                remoteImageList.Clear();
                localImageList.Clear();
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error("保存文件出错" + ex.ToString());
            }
            result = true;
            return result;
        }
        public string SavePanelFileToLocal(string panel_ID, string[] imagePath, params object[] data)
        {
            string path = "";

            #region  // 第一行
            object PANEL_ID = panel_ID;
            object READ_PANELID_TYPE = "Panel";
            object PRODUCT_SPEC = "***";
            object STEP_ID = this.Step_ID;
            object EQP_ID = this.EQP_ID;
            object UNIT_ID = this.UnitID;
            object SUB_UNIT_ID = this.SubUnitID;
            object RECIPE_ID = "***";
            object PG_ID = "***";
            object CARRIER_ID = "***";
            object JUDGE = "OK";
            object START_TIME = DateTime.Now.ToString("yyyyMMdd_hhmmss");
            object END_TIME = DateTime.Now.ToString("yyyyMMdd_hhmmss");
            object REPAIRCNT = "***";
            object OPERATOR_ID = "***";
            object WORKORDER_ID = "***";
            object WORKORDER_TYPE = "***";
            object TP_RESULT = "***";
            object TP_ERROR = "***";
            object PANEL_GRADE = "***";
            #endregion
            #region  // 第二行
            object DEF_TYPE = "P/C";
            object DEFECT_NO = "***";
            object DEFECT_CODE = this.DEFECTCODE;
            object DEF_NAME = "YaHeng"; ;
            object DEF_COORDINATES_X = "***";
            object DEF_COORDINATES_Y = "***";
            object DEF_ZONE = "***";
            object DEF_PATTERN = "***";
            object DEF_AREA = "***";
            object IMAGEURL = "***";
            object IMAGENAME = "***";
            object DISTANCE = "***";
            object RESERVED_1 = "***";
            object RESERVED_2 = "***";
            object RESERVED_3 = "***";
            object RESERVED_4 = "***";
            object RESERVED_5 = "***";

            #endregion
            #region // 第三行
            //object DEFECT_NO = "";
            object OPERATION_ID = "***";
            object YCJUDGE = "***";
            //object DEFECT_CODE = "";
            //object DEF_NAME = "";
            object DEF_COORDINATES_X1 = "***";
            object DEF_COORDINATES_Y1 = "***";
            object DEF_COORDINATES_X2 = "***";
            object DEF_COORDINATES_Y2 = "***";
            //object DEF_ZONE = "";
            // object DEF_PATTERN = "";
            //object DEF_AREA = "";
            object MEASURE_NAME = "***";
            object MEA_COORDINATES_X1 = "***";
            object MEA_COORDINATES_Y1 = "***";
            object DATA_1 = "***";
            //object IMAGEURL = "";
            //object IMAGENAME = "";
            //object RESERVED_1 = "";
            //object RESERVED_2 = "";
            #endregion
            #region // 第四行
            //object OPERATION_ID = "";
            object OPJUDGE = "***";
            //object DEFECT_CODE = "";
            //object DEF_NAME = "";
            //object DEF_COORDINATES_X = "";
            //object DEF_COORDINATES_Y = "";
            //object DEF_ZONE = "";
            //object DEF_PATTERN = "";
            //object DEF_AREA = "";
            //object RESERVED_1 = "";
            // object RESERVED_2 = "";
            //object RESERVED_3 = "";
            //object RESERVED_4 = "";
            //object RESERVED_5 = "";
            object RESERVED_6 = "***";

            #endregion

            /// 判断数据结果
            foreach (var item in data)
            {
                if (item.ToString().Contains("NG"))
                {
                    JUDGE = "NG";
                    break;
                }
            }
            ////////////////////////////////////
            string[] headContent = new string[]
            {              
                /////表头信息
                // string.Join(",","GLASSINFO","FIELD_COUNT","GLASS_ID","LOT_ID","PRODUCTSPECNAME","PROCESSOPERATIONNAME","CARRIERNAME","OWNER_ID","RECIPE_ID","REJUDGE","GLASS_JUDGE","GLASS_GRADE","GLASS_MAP_NAME1","MACHINETYPE","MACHINENAME","UNITID","SUBUNITID","EVENTUSER","RECIPENAME","PRODUCTIONTYPE","SLOTID","TKINTIME","TKOUTTIME","TOTALPANELCOUNT","TOTALDEFECTCOUNT","GLASSMAPNAME","GLASS_FLAG1","GLASS_FLAG2","GLASS_FLAG3","GLASS_FLAG4","GLASS_FLAG5","GLASS_FLAG6","GLASS_FLAG7","GLASS_FLAG8","GLASS_FLAG9","GLASS_FLAG10"),
                // string.Join(",","GLASSDATA",FIELD_COUNT, GLASS_ID, LOT_ID,  PRODUCTSPECNAME, PROCESSOPERATIONNAME, CARRIERNAME, OWNER_ID, RECIPE_ID,REJUDGE, GLASS_JUDGE, GLASS_GRADE, GLASS_MAP_NAME1, MACHINETYPE, MACHINENAME, UNITID, SUBUNITID , EVENTUSER,   RECIPENAME, PRODUCTIONTYPE, SLOTID, TKINTIME ,TKOUTTIME , TOTALPANELCOUNT, TOTALDEFECTCOUNT,GLASSMAPNAME, GLASS_FLAG1 ,GLASS_FLAG2 ,GLASS_FLAG3, GLASS_FLAG4, GLASS_FLAG5, GLASS_FLAG6 ,GLASS_FLAG7 ,GLASS_FLAG8 ,GLASS_FLAG9 ,GLASS_FLAG10),
                // string.Join(",","PANELINFO","FIELD_COUNT","PROCESSOPERATIONNAME","MACHINETYPE","WHITE_BRIGHTNESS","BLACK_BRIGHTNESS","WO_ID","FILE_CREATED_TIME","PANELID","CF_PANEL_ID","TFT_PANEL_ID","EQ_ID","SUB_EQ_ID","START_TIME","END_TIME","MACHINE_TACT_TIME","PRODUCT_ID","CST_ID","SLOT_ID","PP_ID","OP_ID","MODEL_NAME","P_SPECIAL_CODE","P_ABNORMAL_FLAG","PANELJUDGE","PANELGRADE","MAIN_DEFECT_CODE","MAIN_DEF_NAME","COORDINATES_X1","COORDINATES_Y1","COORDINATES_X2","COORDINATES_Y2","DEFECT_COUNT","GATELINE","DATALINE","PANEL_SIZE_X","PANEL_SIZE_Y","REPAIR_DEFECT_CODE","REPAIR_JUDGE_CODE","REPAIR_RESULT","REPAIR_COUNT","PRODUCT_REQUEST_TYPE","PANEL_FLAG1","PANEL_FLAG2","PANEL_FLAG3","PANEL_FLAG4","PANEL_FLAG5","PANEL_FLAG6","PANEL_FLAG7","PANEL_FLAG8","PANEL_FLAG9","PANEL_FLAG10"),
                // string.Join(",","PANELDATA",FIELD_COUNT, PROCESSOPERATIONNAME, MACHINETYPE, WHITE_BRIGHTNESS, BLACK_BRIGHTNESS,WO_ID,   FILE_CREATED_TIME ,  PANELID, CF_PANEL_ID, TFT_PANEL_ID, EQ_ID,SUB_EQ_ID,START_TIME, END_TIME, MACHINE_TACT_TIME,   PRODUCT_ID,  CST_ID,  SLOT_ID, PP_ID,   OP_ID,   MODEL_NAME,  P_SPECIAL_CODE,  P_ABNORMAL_FLAG, PANELJUDGE,  PANELGRADE,  MAIN_DEFECT_CODE, MAIN_DEF_NAME, COORDINATES_X1,  COORDINATES_Y1,  COORDINATES_X2,  COORDINATES_Y2, DEFECT_COUNT, GATELINE, DATALINE,PANEL_SIZE_X,PANEL_SIZE_Y, REPAIR_DEFECT_CODE,  REPAIR_JUDGE_CODE,   REPAIR_RESULT,   REPAIR_COUNT,    PRODUCT_REQUEST_TYPE,    PANEL_FLAG1, PANEL_FLAG2, PANEL_FLAG3, PANEL_FLAG4, PANEL_FLAG5, PANEL_FLAG6, PANEL_FLAG7, PANEL_FLAG8, PANEL_FLAG9, PANEL_FLAG10),
                // string.Join(",","DEFECTINFO","FIELD_COUNT","PANELID","OP_ID","CT_SITE","DEFECT_NO","DEFECTCODE","DEFECTJUDGE","DEFECT_NAME","DEF_COORDINATES_X1","DEF_COORDINATES_Y1","DEF_COORDINATES_X2","DEF_COORDINATES_Y2","DEF_PATTERN","DEFECT_AREA","DEFECT_GRAY","REPAIR_DEFECT_CODE","REPAIR_JUDGE_CODE","REPAIR_RESULT","IMAGE_PATH","IMAGE_NAME_A","IMAGE_NAME_B","IMAGE_NAME_C","IMAGE_NAME_D","IMAGE_NAME_E","PROCESSOPERATIONNAME","MACHINETYPE","EQ_ID","SUB_EQ_ID","START_TIME","END_TIME","REPAIR_TIME","DEFECT_FLAG1","DEFECT_FLAG2","DEFECT_FLAG3","DEFECT_FLAG4","DEFECT_FLAG5","DEFECT_FLAG6","DEFECT_FLAG7","DEFECT_FLAG8","DEFECT_FLAG9","DEFECT_FLAG10"),
                // string.Join(",","DEFECTDATA",FIELD_COUNT,PANELID, OP_ID,CT_SITE, DEFECT_NO,DEFECTCODE,  DEFECTJUDGE, DEFECT_NAME ,DEF_COORDINATES_X1,  DEF_COORDINATES_Y1,  DEF_COORDINATES_X2 , DEF_COORDINATES_Y2 , DEF_PATTERN, DEFECT_AREA, DEFECT_GRAY, REPAIR_DEFECT_CODE,  REPAIR_JUDGE_CODE,   REPAIR_RESULT,   IMAGE_PATH,  IMAGE_NAME_A,    IMAGE_NAME_B,    IMAGE_NAME_C,    IMAGE_NAME_D,    IMAGE_NAME_E ,   PROCESSOPERATIONNAME,    MACHINETYPE, EQ_ID,   SUB_EQ_ID,   START_TIME,  END_TIME,    REPAIR_TIME, DEFECT_FLAG1,    DEFECT_FLAG2 ,   DEFECT_FLAG3,    DEFECT_FLAG4,    DEFECT_FLAG5,    DEFECT_FLAG6,    DEFECT_FLAG7 ,   DEFECT_FLAG8,    DEFECT_FLAG9,    DEFECT_FLAG10),
                // ///添加测量内容
                
                 ///新表头信息
                 string.Join(",","PANELINFO","PANEL_ID","READ_PANELID_TYPE","PRODUCT_SPEC ","STEP_ID","EQP_ID ","UNIT_ID","SUB_UNIT_ID","RECIPE_ID","    PG_ID","CARRIER_ID","JUDGE","START_TIME ","END_TIME ","REPAIRCNT","OPERATOR_ID ","WORKORDER_ID ","WORKORDER_TYPE ","TP_RESULT","TP_ERROR","PANEL_GRADE"),
                 string.Join(",","PANELDATA",PANEL_ID,READ_PANELID_TYPE,PRODUCT_SPEC,STEP_ID,EQP_ID,  UNIT_ID, SUB_UNIT_ID ,RECIPE_ID  , PG_ID ,  CARRIER_ID , JUDGE  , START_TIME , END_TIME ,   REPAIRCNT,   OPERATOR_ID, WORKORDER_ID ,   WORKORDER_TYPE,  TP_RESULT ,  TP_ERROR,    PANEL_GRADE),
                 string.Join(",","AOI_DEFECTINFO","DEFECT_NO","DEF_TYPE","DEF_NAME","DEFECT_CODE","DEF_COORDINATES_X","DEF_COORDINATES_Y","DEF_ZONE","DEF_PATTERN","DEF_AREA","IMAGEURL","IMAGENAME","DISTANCE","RESERVED_1","RESERVED_2","RESERVED_3","RESERVED_4","RESERVED_5"),
                 //string.Join(",","AOI_DEFECTDATA",DEF_TYPE,  DEFECT_NO ,  DEFECT_CODE ,DEF_NAME  ,  DEF_COORDINATES_X ,  DEF_COORDINATES_Y ,  DEF_ZONE,    DEF_PATTERN ,DEF_AREA  ,  IMAGEURL,    IMAGENAME ,  DISTANCE  ,  RESERVED_1,  RESERVED_2,  RESERVED_3 , RESERVED_4 , RESERVED_5),
                 
                 //string.Join(",","RDI_DEFECTINFO","DEFECT_NO","OPERATION_ID","YCJUDGE","DEFECT_CODE DEF_NAME","DEF_COORDINATES_X1","DEF_COORDINATES_Y1","DEF_COORDINATES_X2","DEF_COORDINATES_Y2","DEF_ZONE","DEF_PATTERN DEF_AREA","MEASURE_NAME","MEA_COORDINATES_X1","MEA_COORDINATES_Y1","DATA_1","IMAGEURL","IMAGENAME","RESERVED_1","RESERVED_2"),
                 //string.Join(",","RDI_DEFECTDATA",DEFECT_NO, OPERATION_ID,    YCJUDGE, DEFECT_CODE, DEF_NAME,    DEF_COORDINATES_X1,  DEF_COORDINATES_Y1 , DEF_COORDINATES_X2,  DEF_COORDINATES_Y2,  DEF_ZONE ,DEF_PATTERN, DEF_AREA,MEASURE_NAME,MEA_COORDINATES_X1,MEA_COORDINATES_Y1,DATA_1,IMAGEURL,IMAGENAME,RESERVED_1,RESERVED_2),
                 //string.Join(",","PCIM_DEFECTINFO","OPERATION_ID","OPJUDGE","DEFECT_CODE","DEF_NAME","DEF_COORDINATES_X","DEF_COORDINATES_Y","DEF_ZONE","DEF_PATTERN","DEF_AREA"," RESERVED_1","RESERVED_2","RESERVED_3","RESERVED_4","RESERVED_5","RESERVED_6"),
                 //string.Join(",","PCIM_DEFECTDATA",OPERATION_ID, OPJUDGE, DEFECT_CODE ,DEF_NAME,DEF_COORDINATES_X,DEF_COORDINATES_Y,DEF_ZONE ,DEF_PATTERN ,DEF_AREA,RESERVED_1,RESERVED_2,RESERVED_3,RESERVED_4,RESERVED_5,RESERVED_6),
                 ///添加测量内容  
            };
            string[] headContent2 = new string[]
            {              
                /////表头信息
                // string.Join(",","GLASSINFO","FIELD_COUNT","GLASS_ID","LOT_ID","PRODUCTSPECNAME","PROCESSOPERATIONNAME","CARRIERNAME","OWNER_ID","RECIPE_ID","REJUDGE","GLASS_JUDGE","GLASS_GRADE","GLASS_MAP_NAME1","MACHINETYPE","MACHINENAME","UNITID","SUBUNITID","EVENTUSER","RECIPENAME","PRODUCTIONTYPE","SLOTID","TKINTIME","TKOUTTIME","TOTALPANELCOUNT","TOTALDEFECTCOUNT","GLASSMAPNAME","GLASS_FLAG1","GLASS_FLAG2","GLASS_FLAG3","GLASS_FLAG4","GLASS_FLAG5","GLASS_FLAG6","GLASS_FLAG7","GLASS_FLAG8","GLASS_FLAG9","GLASS_FLAG10"),
                // string.Join(",","GLASSDATA",FIELD_COUNT, GLASS_ID, LOT_ID,  PRODUCTSPECNAME, PROCESSOPERATIONNAME, CARRIERNAME, OWNER_ID, RECIPE_ID,REJUDGE, GLASS_JUDGE, GLASS_GRADE, GLASS_MAP_NAME1, MACHINETYPE, MACHINENAME, UNITID, SUBUNITID , EVENTUSER,   RECIPENAME, PRODUCTIONTYPE, SLOTID, TKINTIME ,TKOUTTIME , TOTALPANELCOUNT, TOTALDEFECTCOUNT,GLASSMAPNAME, GLASS_FLAG1 ,GLASS_FLAG2 ,GLASS_FLAG3, GLASS_FLAG4, GLASS_FLAG5, GLASS_FLAG6 ,GLASS_FLAG7 ,GLASS_FLAG8 ,GLASS_FLAG9 ,GLASS_FLAG10),
                // string.Join(",","PANELINFO","FIELD_COUNT","PROCESSOPERATIONNAME","MACHINETYPE","WHITE_BRIGHTNESS","BLACK_BRIGHTNESS","WO_ID","FILE_CREATED_TIME","PANELID","CF_PANEL_ID","TFT_PANEL_ID","EQ_ID","SUB_EQ_ID","START_TIME","END_TIME","MACHINE_TACT_TIME","PRODUCT_ID","CST_ID","SLOT_ID","PP_ID","OP_ID","MODEL_NAME","P_SPECIAL_CODE","P_ABNORMAL_FLAG","PANELJUDGE","PANELGRADE","MAIN_DEFECT_CODE","MAIN_DEF_NAME","COORDINATES_X1","COORDINATES_Y1","COORDINATES_X2","COORDINATES_Y2","DEFECT_COUNT","GATELINE","DATALINE","PANEL_SIZE_X","PANEL_SIZE_Y","REPAIR_DEFECT_CODE","REPAIR_JUDGE_CODE","REPAIR_RESULT","REPAIR_COUNT","PRODUCT_REQUEST_TYPE","PANEL_FLAG1","PANEL_FLAG2","PANEL_FLAG3","PANEL_FLAG4","PANEL_FLAG5","PANEL_FLAG6","PANEL_FLAG7","PANEL_FLAG8","PANEL_FLAG9","PANEL_FLAG10"),
                // string.Join(",","PANELDATA",FIELD_COUNT, PROCESSOPERATIONNAME, MACHINETYPE, WHITE_BRIGHTNESS, BLACK_BRIGHTNESS,WO_ID,   FILE_CREATED_TIME ,  PANELID, CF_PANEL_ID, TFT_PANEL_ID, EQ_ID,SUB_EQ_ID,START_TIME, END_TIME, MACHINE_TACT_TIME,   PRODUCT_ID,  CST_ID,  SLOT_ID, PP_ID,   OP_ID,   MODEL_NAME,  P_SPECIAL_CODE,  P_ABNORMAL_FLAG, PANELJUDGE,  PANELGRADE,  MAIN_DEFECT_CODE, MAIN_DEF_NAME, COORDINATES_X1,  COORDINATES_Y1,  COORDINATES_X2,  COORDINATES_Y2, DEFECT_COUNT, GATELINE, DATALINE,PANEL_SIZE_X,PANEL_SIZE_Y, REPAIR_DEFECT_CODE,  REPAIR_JUDGE_CODE,   REPAIR_RESULT,   REPAIR_COUNT,    PRODUCT_REQUEST_TYPE,    PANEL_FLAG1, PANEL_FLAG2, PANEL_FLAG3, PANEL_FLAG4, PANEL_FLAG5, PANEL_FLAG6, PANEL_FLAG7, PANEL_FLAG8, PANEL_FLAG9, PANEL_FLAG10),
                // string.Join(",","DEFECTINFO","FIELD_COUNT","PANELID","OP_ID","CT_SITE","DEFECT_NO","DEFECTCODE","DEFECTJUDGE","DEFECT_NAME","DEF_COORDINATES_X1","DEF_COORDINATES_Y1","DEF_COORDINATES_X2","DEF_COORDINATES_Y2","DEF_PATTERN","DEFECT_AREA","DEFECT_GRAY","REPAIR_DEFECT_CODE","REPAIR_JUDGE_CODE","REPAIR_RESULT","IMAGE_PATH","IMAGE_NAME_A","IMAGE_NAME_B","IMAGE_NAME_C","IMAGE_NAME_D","IMAGE_NAME_E","PROCESSOPERATIONNAME","MACHINETYPE","EQ_ID","SUB_EQ_ID","START_TIME","END_TIME","REPAIR_TIME","DEFECT_FLAG1","DEFECT_FLAG2","DEFECT_FLAG3","DEFECT_FLAG4","DEFECT_FLAG5","DEFECT_FLAG6","DEFECT_FLAG7","DEFECT_FLAG8","DEFECT_FLAG9","DEFECT_FLAG10"),
                // string.Join(",","DEFECTDATA",FIELD_COUNT,PANELID, OP_ID,CT_SITE, DEFECT_NO,DEFECTCODE,  DEFECTJUDGE, DEFECT_NAME ,DEF_COORDINATES_X1,  DEF_COORDINATES_Y1,  DEF_COORDINATES_X2 , DEF_COORDINATES_Y2 , DEF_PATTERN, DEFECT_AREA, DEFECT_GRAY, REPAIR_DEFECT_CODE,  REPAIR_JUDGE_CODE,   REPAIR_RESULT,   IMAGE_PATH,  IMAGE_NAME_A,    IMAGE_NAME_B,    IMAGE_NAME_C,    IMAGE_NAME_D,    IMAGE_NAME_E ,   PROCESSOPERATIONNAME,    MACHINETYPE, EQ_ID,   SUB_EQ_ID,   START_TIME,  END_TIME,    REPAIR_TIME, DEFECT_FLAG1,    DEFECT_FLAG2 ,   DEFECT_FLAG3,    DEFECT_FLAG4,    DEFECT_FLAG5,    DEFECT_FLAG6,    DEFECT_FLAG7 ,   DEFECT_FLAG8,    DEFECT_FLAG9,    DEFECT_FLAG10),
                // ///添加测量内容
                
                 ///新表头信息
                 //string.Join(",","PANELINFO","PANEL_ID","READ_PANELID_TYPE","PRODUCT_SPEC ","STEP_ID","EQP_ID ","UNIT_ID","SUB_UNIT_ID","RECIPE_ID","    PG_ID","CARRIER_ID","JUDGE","START_TIME ","END_TIME ","REPAIRCNT","OPERATOR_ID ","WORKORDER_ID ","WORKORDER_TYPE ","TP_RESULT","TP_ERROR","PANEL_GRADE"),
                 //string.Join(",","PANELDATA",PANEL_ID,READ_PANELID_TYPE,PRODUCT_SPEC,STEP_ID,EQP_ID,  UNIT_ID, SUB_UNIT_ID ,RECIPE_ID  , PG_ID ,  CARRIER_ID , JUDGE  , START_TIME , END_TIME ,   REPAIRCNT,   OPERATOR_ID, WORKORDER_ID ,   WORKORDER_TYPE,  TP_RESULT ,  TP_ERROR,    PANEL_GRADE),
                 //string.Join(",","AOI_DEFECTINFO","DEF_TYPE","DEFECT_NO","DEFECT_CODE","DEF_NAME","DEF_COORDINATES_X","DEF_COORDINATES_Y","DEF_ZONE","DEF_PATTERN DEF_AREA","IMAGEURL","IMAGENAME","DISTANCE","RESERVED_1","RESERVED_2","RESERVED_3","RESERVED_4","RESERVED_5"),
                 // string.Join(",","AOI_DEFECTDATA",DEF_TYPE,  DEFECT_NO ,  DEFECT_CODE ,DEF_NAME  ,  DEF_COORDINATES_X ,  DEF_COORDINATES_Y ,  DEF_ZONE,    DEF_PATTERN ,DEF_AREA  ,  IMAGEURL,    IMAGENAME ,  DISTANCE  ,  RESERVED_1,  RESERVED_2,  RESERVED_3 , RESERVED_4 , RESERVED_5),

                 //string.Join(",","RDI_DEFECTINFO","DEFECT_NO","OPERATION_ID","YCJUDGE","DEFECT_CODE DEF_NAME","DEF_COORDINATES_X1","DEF_COORDINATES_Y1","DEF_COORDINATES_X2","DEF_COORDINATES_Y2","DEF_ZONE","DEF_PATTERN DEF_AREA","MEASURE_NAME","MEA_COORDINATES_X1","MEA_COORDINATES_Y1","DATA_1","IMAGEURL","IMAGENAME","RESERVED_1","RESERVED_2"),
                 //string.Join(",","RDI_DEFECTDATA",DEFECT_NO, OPERATION_ID,    YCJUDGE, DEFECT_CODE, DEF_NAME,    DEF_COORDINATES_X1,  DEF_COORDINATES_Y1 , DEF_COORDINATES_X2,  DEF_COORDINATES_Y2,  DEF_ZONE ,DEF_PATTERN, DEF_AREA,MEASURE_NAME,MEA_COORDINATES_X1,MEA_COORDINATES_Y1,DATA_1,IMAGEURL,IMAGENAME,RESERVED_1,RESERVED_2),
                 //string.Join(",","PCIM_DEFECTINFO","OPERATION_ID","OPJUDGE","DEF_NAME","DEFECT_CODE","DEF_COORDINATES_X","DEF_COORDINATES_Y","DEF_ZONE","DEF_PATTERN","DEF_AREA"," RESERVED_1","RESERVED_2","RESERVED_3","RESERVED_4","RESERVED_5","RESERVED_6"),
                 //string.Join(",","PCIM_DEFECTDATA",OPERATION_ID, OPJUDGE,DEF_NAME, DEFECT_CODE ,DEF_COORDINATES_X,DEF_COORDINATES_Y,DEF_ZONE ,DEF_PATTERN ,DEF_AREA,RESERVED_1,RESERVED_2,RESERVED_3,RESERVED_4,RESERVED_5,RESERVED_6),
                 ///添加测量内容  
            };
            ///////////////////////////////////////
            path = GetLocalPanelFilePath(panel_ID);
            if (!Directory.Exists(new FileInfo(path).DirectoryName))
                Directory.CreateDirectory(new FileInfo(path).DirectoryName);
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                ///// 添加行头 ///////
                for (int i = 0; i < headContent.Length; i++)
                {
                    sw.WriteLine(headContent[i]);
                }
                ///////////////////// 写入数据 ///////
                int index = 0;
                //string X = "",Y ="",ImagePath ="";
                foreach (var item in data)
                {
                    string[] con = item.ToString().Split(',', '^');
                    if (con == null || con.Length == 0) continue;
                    switch (con[0])
                    {
                        case "X1":
                        case "X2":
                        case "X3":
                        case "X4":
                        case "X5":
                        case "X6":
                        case "X7":
                        case "X8":
                            //X = con[1];
                            DEF_COORDINATES_X = con[1];
                            break;
                        case "Y1":
                        case "Y2":
                        case "Y3":
                        case "Y4":
                        case "Y5":
                        case "Y6":
                        case "Y7":
                        case "Y8":
                            //Y = con[1];
                            DEF_COORDINATES_Y = con[1];
                            if (imagePath.Length > index)
                            {
                                string[] name = imagePath[index].Split(new string[1] { "IMAGE/" }, StringSplitOptions.RemoveEmptyEntries);
                                IMAGEURL = imagePath[index];
                                IMAGENAME = name[1];
                            }
                            else
                            {
                                IMAGEURL = "";
                            }
                            DEFECT_NO = (index + 1) / 2;
                            sw.WriteLine(string.Join(",", "AOI_DEFECTDATA", DEFECT_NO, DEF_TYPE, DEF_NAME, DEFECT_CODE, DEF_COORDINATES_X, DEF_COORDINATES_Y, DEF_ZONE, DEF_PATTERN, DEF_AREA, IMAGEURL, IMAGENAME, DISTANCE, RESERVED_1, RESERVED_2, RESERVED_3, RESERVED_4, RESERVED_5));
                            break;
                    }
                    index++;
                }
                ////////////////////////////////////////////  添加行头尾 ///////
                //for (int i = 0; i < headContent2.Length; i++)
                //{
                //    sw.WriteLine(headContent2[i]);
                //}
            }
            return path;
        }
        public string SaveImageFileToLocal(string localFile, string Panel_ID,int i)
        {
            string path = "", grabNo = "";
            if (File.Exists(localFile))
            {
                grabNo = new FileInfo(localFile).Name.Split('_')[0];
                path = GetLocalImageFilePath(Panel_ID, grabNo,i);
                if (!Directory.Exists(new FileInfo(path).DirectoryName))
                    Directory.CreateDirectory(new FileInfo(path).DirectoryName);
                //HImage hImage = new HImage(localFile);
                //hImage.WriteImage("jpeg", 0, path);
                //hImage?.Dispose();

                Image image = Image.FromFile(localFile);
                image.Save(path, ImageFormat.Jpeg);
                image?.Dispose();
            }
            return path;
        }
        public string SaveDmyFileToLocal(string Panel_ID)
        {
            string path = GetLocalDmyFilePath(Panel_ID);
            if (!Directory.Exists(new FileInfo(path).DirectoryName))
                Directory.CreateDirectory(new FileInfo(path).DirectoryName);
            using (StreamWriter sw = new StreamWriter(path))
            {
            }
            return path;
        }
        public string SaveDmyFileToLocal2(string Panel_ID)
        {

            string path2 = GetLocalDmyFilePath2(Panel_ID);
            if (!Directory.Exists(new FileInfo(path2).DirectoryName))
                Directory.CreateDirectory(new FileInfo(path2).DirectoryName);
            using (StreamWriter sw = new StreamWriter(path2))
            {
            }
            return path2;
        }


        public string GetLocalPanelFilePath(string Panel_ID)
        {
            string filePath = "";
            if (Panel_ID == null || Panel_ID.Length == 0)
                Panel_ID = "W21VP1005A0101";
            string Model_ID;
            string FAB_ID;
            if (Panel_ID.Length > 1)
                FAB_ID = Panel_ID.Substring(0, 1);
            else
                FAB_ID = " ";
            if (Panel_ID.Length > 8)
                Model_ID = Panel_ID.Substring(5, 4);
            else
                Model_ID = Panel_ID;
            string date = DateTime.Now.ToString("yyyyMMdd");

            filePath = string.Join("\\", "AutoMotive", "OLB", this.Step_ID, "M1A2MOLB07PAI100", date, Panel_ID, "DATA");

            return this.SaveFolderPath + "\\" + filePath + "\\" + Panel_ID + "_" + this.Step_ID + "_M1A2MOLB07PAI100_EQP" + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv";
        }

        public string GetRemotePanelFilePath(string Panel_ID)
        {
            string filePath = "";
            if (Panel_ID == null || Panel_ID.Length == 0)
                Panel_ID = "W21VP1005A0101";
            string Model_ID;
            string FAB_ID;
            if (Panel_ID.Length > 1)
                FAB_ID = Panel_ID.Substring(0, 1);
            else
                FAB_ID = " ";
            if (Panel_ID.Length > 8)
                Model_ID = Panel_ID.Substring(5, 4);
            else
                Model_ID = Panel_ID;
            string date = DateTime.Now.ToString("yyyyMMdd");

            filePath = string.Join("/", this.ShopCode, this.Step_ID, this.EQP_ID, FAB_ID + Model_ID, date, Panel_ID, "DATA");

            return this.FtpURI + "/" + filePath + "/" + Panel_ID + "_" + this.Step_ID + "_" + this.EQP_ID + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv";
        }
        public string GetRemoteImageFilePath(string Panel_ID)
        {
            string folderPath = "";
            if (Panel_ID == null || Panel_ID.Length == 0)
                Panel_ID = "W21VP1005A0101";
            string Model_ID;
            string FAB_ID;
            if (Panel_ID.Length > 1)
                FAB_ID = Panel_ID.Substring(0, 1);
            else
                FAB_ID = " ";
            if (Panel_ID.Length > 8)
                Model_ID = Panel_ID.Substring(5, 4);
            else
                Model_ID = Panel_ID;
            string date = DateTime.Now.ToString("yyyyMMdd");

            folderPath = string.Join("/", this.ShopCode, this.ShopCode.Replace("-", ""), this.Step_ID, this.EQP_ID, FAB_ID + Model_ID, date, Panel_ID, "IMAGE", "EQP", "DFT");
            return this.FtpURI + "/" + folderPath + "/" + Panel_ID + "_" + this.Step_ID + "_" + this.EQP_ID + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".jpeg";
        }

        public string GetLocalImageFilePath(string Panel_ID, string grabNo,int i)
        {
            string folderPath = "";
            if (Panel_ID == null || Panel_ID.Length == 0)
                Panel_ID = "W21VP1005A0101";
            string Model_ID;
            string FAB_ID;
            if (Panel_ID.Length > 1)
                FAB_ID = Panel_ID.Substring(0, 1);
            else
                FAB_ID = " ";
            if (Panel_ID.Length > 8)
                Model_ID = Panel_ID.Substring(5, 4);
            else
                Model_ID = Panel_ID;
            string date = DateTime.Now.ToString("yyyyMMdd");

            folderPath = string.Join("\\", "AutoMotive", "OLB", this.Step_ID, "M1A2MOLB07PAI100", date, Panel_ID, "IMAGE"); //, "FULL"
            return this.SaveFolderPath + "\\" + folderPath + "\\" + Panel_ID + "_" + this.Step_ID + "_"+i +"_DFT_" +"@_@_@_@_@_@_@_"  + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".jpg";
        }
        public string GetLocalDmyFilePath(string Panel_ID)
        {
            string filePath = "";
            if (Panel_ID == null || Panel_ID.Length == 0)
                Panel_ID = "W21VP1005A0101";
            string Model_ID;
            string FAB_ID;
            if (Panel_ID.Length > 1)
                FAB_ID = Panel_ID.Substring(0, 1);
            else
                FAB_ID = " ";
            if (Panel_ID.Length > 8)
                Model_ID = Panel_ID.Substring(5, 4);
            else
                Model_ID = Panel_ID;
            string date = DateTime.Now.ToString("yyyyMMdd");

            filePath = string.Join("\\", "AutoMotive", "OLB", this.Step_ID, "DUMMY");

            return this.SaveFolderPath + "\\" + filePath + "\\" + Panel_ID + "_" + this.Step_ID + "_" + "M1A2MOLB07PAI100" + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv.dmy";
        }
        public string GetLocalDmyFilePath2(string Panel_ID)
        {
            string filePath = "";
            if (Panel_ID == null || Panel_ID.Length == 0)
                Panel_ID = "W21VP1005A0101";
            string Model_ID;
            string FAB_ID;
            if (Panel_ID.Length > 1)
                FAB_ID = Panel_ID.Substring(0, 1);
            else
                FAB_ID = " ";
            if (Panel_ID.Length > 8)
                Model_ID = Panel_ID.Substring(5, 4);
            else
                Model_ID = Panel_ID;
            string date = DateTime.Now.ToString("yyyyMMdd");

            filePath = string.Join("\\", "AutoMotive", "OLB", this.Step_ID, "INDEX");

            return this.SaveFolderPath + "\\" + filePath + "\\" + Panel_ID + "_" + this.Step_ID + "_" + "M1A2MOLB07PAI100" + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv.dmy";
        }
        public string GetRemoteDmyFilePath(string Panel_ID)
        {
            string filePath = "";
            if (Panel_ID == null || Panel_ID.Length == 0)
                Panel_ID = "W21VP1005A0101";
            string Model_ID;
            string FAB_ID;
            if (Panel_ID.Length > 1)
                FAB_ID = Panel_ID.Substring(0, 1);
            else
                FAB_ID = " ";
            if (Panel_ID.Length > 8)
                Model_ID = Panel_ID.Substring(5, 4);
            else
                Model_ID = Panel_ID;
            string date = DateTime.Now.ToString("yyyyMMdd");

            filePath = string.Join("/", this.ShopCode, this.Step_ID, this.EQP_ID, FAB_ID + Model_ID, date, Panel_ID, "LOG");

            return this.FtpURI + "/" + filePath + "/" + Panel_ID + "_" + this.Step_ID + "_" + "M1A2MOLB07PAI100" + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv.dmy";
        }


    }


}
