using System;

namespace FunctionBlock
{

    public class FtpConfigParam
    {
        // 设备类型
        public string FtpServerIp { set; get; }
        public int FTP_Port { set; get; }
        public string FtpLocalPath { set; get; }
        public string FtpURI { set; get; }
        public string ShopCode { set; get; } // 工站号
        public string Step_ID { set; get; } // 工站号
        public string EQP_ID { set; get; } // 工站号

        public FtpConfigParam()
        {
            this.FtpLocalPath = "E:\\FtpData\\上传文件";
            this.FTP_Port = 5000;
            this.FtpServerIp = "10.102.160.92";
            this.FtpURI = "ftp://" + this.FtpServerIp;

            this.ShopCode = "MOUDLE-A";
            this.Step_ID = "M1A2MOLB07";
            this.EQP_ID = "PAI100"; //M1A2MOLB07PAI100
        }


        public string GetRemotePanelFilePath(FtpConfigParam ftpConfig, string Panel_ID)
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

            filePath = string.Join("/", ftpConfig.ShopCode, ftpConfig.Step_ID, ftpConfig.EQP_ID, FAB_ID + Model_ID, date, Panel_ID, "DATA");

            return ftpConfig.FtpURI + "/" + filePath + "/" + Panel_ID + "_" + ftpConfig.Step_ID + "_" + ftpConfig.EQP_ID + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv";
        }
        public string GetRemoteImageFilePath(FtpConfigParam ftpConfig, string Panel_ID)
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


            folderPath = string.Join("/", ftpConfig.ShopCode, ftpConfig.Step_ID, ftpConfig.EQP_ID, FAB_ID + Model_ID, date, Panel_ID, "IMAGE", "EQP", "DFT");
            return ftpConfig.FtpURI + "/" + folderPath + "/" + Panel_ID + "_" + ftpConfig.Step_ID + "_" + ftpConfig.EQP_ID + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".jpg";
        }
        public string GetRemoteDmyFilePath(FtpConfigParam ftpConfig, string Panel_ID)
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

            filePath = string.Join("/", ftpConfig.ShopCode, ftpConfig.Step_ID, ftpConfig.EQP_ID, FAB_ID + Model_ID, date, Panel_ID, "LOG");

            return ftpConfig.FtpURI + "/" + filePath + "/" + Panel_ID + "_" + ftpConfig.Step_ID + "_" + ftpConfig.EQP_ID + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv.dmy";
        }





    }


}


