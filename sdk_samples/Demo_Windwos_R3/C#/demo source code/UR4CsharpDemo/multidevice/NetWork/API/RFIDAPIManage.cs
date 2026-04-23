using BLEDeviceAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UHFAPP.USB.multidevice;
using static UHFAPP.UHFAPI;
using static UHFAPP.USB.multidevice.RFIDAPIManage;

namespace UHFAPP.MultiDevice.NetWork.API
{
    public class RFIDAPIManage
    {
        /// <summary>
        /// 盘点标签委托 (inventory tag delegation)
        /// </summary>
        /// <param name="device">设备对象(device)</param>
        /// <param name="eventArgs">标签上报事件(tag report event)</param>
        public delegate void InventoryTagEventHandler(string device, InventoryTagEventArgs eventArgs);
        /// <summary>
        /// 盘点标签事件(inventory tag event)
        /// </summary>
        public event InventoryTagEventHandler InventoryTagEvent = null;


        //第二步：定义断开回调委托
        internal static UHFAPP.UHFAPI.OnDisconnectCallback DisconnectCallback = null;

        //第二步：定义停止盘点回调委托
        internal static UHFAPP.UHFAPI.OnEventStopInventory OnEventStopInventory = null;
        private object _lock = new object();
        private bool isRuning = false;
        private Dictionary<string, RFIDAPI> _ht = new Dictionary<string, RFIDAPI>();

        private static RFIDAPIManage rfidAPIManage = new RFIDAPIManage();
        private RFIDAPIManage() {
            //第三步：给变量赋值
            DisconnectCallback = OnDisconnectCallback;
            //第三步：给变量赋值
            OnEventStopInventory = OnStopInventoryCallback;
        }
       
        public static RFIDAPIManage GetInstance  {
            get { return rfidAPIManage; }
        }
        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public RFIDAPI GetDeviceByIP(string ip)
        {
            lock (_lock)
            {
                if(ip==null || ip == "")
                {
                    return null;
                }
                if (!_ht.ContainsKey(ip))
                {
                    _ht.Add(ip, new RFIDAPI(ip));
                }
                RFIDAPI obj = _ht[ip];
                return obj;

            }
        }
        /// <summary>
        /// 获取所有的设备
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, RFIDAPI> GetAllDevice()
        {
            lock (_lock)
            {
                return _ht;
            }
        }
    

        #region
        //第一步：定义断开回调函数
        private void OnDisconnectCallback(int id)
        {
            string ip = GetIpById(id, UHFAPI.DEVICE_DISCONNECT);
            RFIDAPI rfid=GetDeviceByIP(ip);
            if (rfid != null)
            {
                rfid.RFIDDisconnectCallbank();
            }
            System.Console.WriteLine("OnDisconnectCallback id=" + id + "  ip="+ ip);

        }
        //第一步：定义停止盘点回调函数
        private void OnStopInventoryCallback(int id)
        {
            string ip = GetIpById(id, UHFAPI.DEVICE_ALL);
            RFIDAPI rfid = GetDeviceByIP(ip);
            if (rfid != null)
            {
                rfid.OnStopInventoryCallback();
            }
            System.Console.WriteLine("OnStopInventoryCallback id=" + id + "  ip=" + ip);
        }
        private string GetIpById(int id,int connectionState)
        {
            List<DeviceInfo> deviceList = UHFAPI.getInstance().LinkGetDeviceInfo(connectionState);
            if (deviceList != null && deviceList.Count > 0)
            {
                for (int k = 0; k < deviceList.Count; k++)
                {
                    DeviceInfo deviceInfo = deviceList[k];
                    if (id == deviceInfo.Id)
                    {
                        return deviceInfo.Ip;
                    }
                }
            }
            return "";
        }
   
        #endregion


        #region   ReadEPC
        /// <summary>
        /// 开始盘点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void StartThread()
        {
            lock (_lock)
            {
                if (!isRuning)
                {
                    isRuning = true;
                    new Thread(new ThreadStart(delegate { ReadEPC(); })).Start();
                }
            }

        }
        /// <summary>
        /// 结束盘点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void StopThread()
        {
            lock (_lock)
            {
                Dictionary<string, RFIDAPI> dictionary = RFIDAPIManage.GetInstance.GetAllDevice();
                foreach (KeyValuePair<string, RFIDAPI> kv in dictionary)
                {
                    RFIDAPI rFIDAPI = kv.Value;
                    if (rFIDAPI.IsConnected())
                    {
                        return;
                    }
                }

                isRuning = false;
                Thread.Sleep(50);
            }
       

        }
        //获取epc
        private void ReadEPC()
        {
            Console.WriteLine("ReadEPC begin...");
            while (isRuning)
            {
                TagInfo tagInfo = GetTagData();
                if (tagInfo != null && tagInfo.UhfTagInfo != null)
                {
                    Dictionary<string, RFIDAPI> dictionary = RFIDAPIManage.GetInstance.GetAllDevice();
                    foreach (KeyValuePair<string, RFIDAPI> kv in dictionary)
                    {
                        RFIDAPI rFIDAPI = kv.Value;
                        if (tagInfo.Id == rFIDAPI.Id)
                        {
                            if (InventoryTagEvent != null)
                            {
                                InventoryTagEvent(rFIDAPI.GetIP(), new InventoryTagEventArgs(tagInfo.UhfTagInfo));
                            }
                        }
                  
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
            Console.WriteLine("ReadEPC end...");

        }
        //return 0,no data, > 0 tag length, < 0 error code
        //tdata tag data, type+length+content+...+type+length+content
        //type:1-epc,2-tid,3-user,4-rssi,5-antenna,6-id
        //06 02 00 00     01 0E 30 00 E2 00 51 57 88 18 01 61 22 20 2F 60 04 02 FD B1 05 01 01
        private static TagInfo GetTagData()
        {
            
            TagInfo info = new TagInfo();
            byte[] tagTempData = new byte[150]; //Array.Clear(tagTempData, 0, tagTempData.Length);
            int result = UHFAPI.UHFGetTagData(tagTempData, tagTempData.Length);
            info.ErrCode = result;
            if (result > 0)
            {
                try
                {
                    // if (tagTempData[0] == 0)
                    {
                        string hex = BitConverter.ToString(tagTempData, 0, result);
                        // Console.WriteLine("hex=" + hex + " result=" + result);
                    }
                    int index = 0;
                    UHFTAGInfo uhfinfo = new UHFTAGInfo();
                    while (true)
                    {
                        if (index > result)
                        {
                            break;
                        }
                        int type = tagTempData[index++];
                        if (index > result)
                        {
                            break;
                        }
                        int len = tagTempData[index++];
                        if (index + len > result)
                        {
                            break;
                        }
                        byte[] data = Utils.CopyArray<byte>(tagTempData, index, len);
                        index = index + len;

                        if (type == UHFAPI.CELL_UHF_EPC)
                        {
                            //epc
                            uhfinfo.Epc = BitConverter.ToString(data, 0, data.Length).Replace("-", "");
                        }
                        else if (type == UHFAPI.CELL_UHF_PC)
                        {
                            //pc
                            uhfinfo.Pc = BitConverter.ToString(data, 0, data.Length).Replace("-", "");
                        }
                        else if (type == UHFAPI.CELL_UHF_TID)
                        {
                            //tid
                            uhfinfo.Tid = BitConverter.ToString(data, 0, data.Length).Replace("-", "");
                            if (data.Length > 12)
                            {
                                //user
                                uhfinfo.User = BitConverter.ToString(data, 12, data.Length - 12).Replace("-", "");
                            }
                        }
                        else if (type == UHFAPI.CELL_UHF_USER)
                        {
                            //user
                            uhfinfo.User = BitConverter.ToString(data, 0, data.Length).Replace("-", "");
                        }
                        else if (type == UHFAPI.CELL_UHF_RSSI)
                        {
                            //rssi
                            int rssiTemp = (data[1] | (data[0] << 8)) - 65535;
                            float rssi_data = (float)((float)rssiTemp / 10.0);// RSSI  =  (0xFED6   -65535)/10
                                                                              //if (!rssi_data.Contains("."))
                                                                              //{
                                                                              //    rssi_data = rssi_data + ".0";
                                                                              //}

                            uhfinfo.Rssi = rssi_data;


                        }
                        else if (type == UHFAPI.CELL_UHF_ANTENNA)
                        {
                            //ant
                            uhfinfo.Ant = data[0];
                        }
                        else if (type == UHFAPI.CELL_CONNECT_ID)
                        {
                            //id
                            info.Id = data[1];
                        }
                        else if (type == 8)
                        {
                            //Sensor

                        }
                        else if (type == UHFAPI.CELL_UHF_PHASE)
                        {
                            //PHASE
                            uhfinfo.Phase = (data[0] << 8) | data[1];
                           // Console.WriteLine("info.Phase =" + uhfinfo.Phase);

                        }
                        else if (type == UHFAPI.CELL_UHF_FREQUENCY_POINT)
                        {
                            if (data.Length == 3)
                            {
                                int ifrequency = ((data[0] & 0xFF) << 16) | ((data[1] & 0xFF) << 8) | ((data[2] & 0xFF));
                                uhfinfo.FrequencyPoint = ifrequency;
                            }

                        }
                    }
                    info.UhfTagInfo = uhfinfo;

                }
                catch (Exception ex)
                { 
                
                }
            }
            return info;
        }
        #endregion
    }
}
