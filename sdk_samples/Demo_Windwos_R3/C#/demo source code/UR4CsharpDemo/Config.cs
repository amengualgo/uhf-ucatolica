using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace UHFAPP
{
    public class Config
    {
        /// <summary>
        /// 主窗体相关配置
        /// </summary>
        public MainForm mainForm { get; set; }

        public ConfigForm configForm { get; set; }

        public UpgradeForm upgradeForm { get; set; }

        /// <summary>
        /// 主窗体属性集合
        /// </summary>
        public class MainForm
        {
            public bool VisibleConnectMultipleDevicesMenuItem { get; set; }
            public bool VisibleUsbExportDataMenuItem { get; set; }
            public bool VisibleBarcodeScanMenuItem { get; set; }
            public bool VisibleBaudrate { get; set; }
            public bool VisibleHidInputMenuItem { get; set; }
        } 

        public class  ConfigForm
        { 
            public bool VisibleEthernetDHCP { get; set; }
            public bool VisibleCalibration { get; set; }
            public bool VisibleEthernetMAC { get; set; }
            public bool VisibleEpcReserved { get; set; }
            public bool VisibleLBT { get; set; }
        }
        public class UpgradeForm
        {
            public bool VisibleEx10SDK { get; set; }
            public bool VisibleReaderBootloader { get; set; }
        }

 
 


    public static Config JsonToConfig()
        {
            string path = System.Environment.CurrentDirectory + "\\APP.config";
            if (!File.Exists(path))
            {
                return null ;
            }
            try
            {
                string jsonStr = FileManage.ReadFile(path);
                if (jsonStr == null || jsonStr.Length == 0)
                {
                    return null;
                }
                Config configObj = JsonConvert.DeserializeObject<Config>(jsonStr);
                return configObj;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
