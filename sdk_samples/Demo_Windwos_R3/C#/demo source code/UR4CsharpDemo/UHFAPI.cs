using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using BLEDeviceAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using UHFAPP.multidevice;
using System.Xml.Linq;
using System.Threading;
using Microsoft.Office.Interop.Excel;

namespace UHFAPP
{
    public class UHFAPI
    {


        #region UHFAPI.dll
         
        /**********************************************************************************************************
        * 功能：获取天线回波损耗
        * 输出：rLoss -- 天线号(1 byte)+回波损耗(1 byte)+天线号+回波损耗+....。
            回波损耗解释：0表示对应端口没有使能（单端口模块为0则表示没有接天线），单位为dB，值越大说明反射越小。
            模块有N个端口则会返回N组数据，每组两个字节，总长度为：2*N
        * 输出：nBytesReturned -- rLoss数据字节长度 
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int UHFGetReturnLoss(byte[] rLoss, short[] nBytesReturned);


        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFSetTempVal(byte tempVal);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFGetTempVal(byte[] tempVal);
        /*
         * 函数功能：  获取本机 IP 和端口号
         * 输出参数：  ipbuf + postbuf， IP+端口号
			           mask:掩码，4字节
			           gate:网关，4字节
         * 返回值：   0:成功    其它：失败
         */
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFGetIp(byte[] ip, byte[] port, byte[] mask, byte[] gate);
        /*
         * 函数功能：  设置本机 IP 和端口号
         * 输入参数：  ipbuf： IP， 
			           postbuf：端口号
			           mask:掩码，4字节
			           gate：网关，4字节

         * 返回值：   0:成功    其它：失败
         */
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFSetIp(byte[] ipbuf, byte[] postbuf, byte[] mask, byte[] gate);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFSetDestIp(byte[] ip, byte[] port);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFGetDestIp(byte[] ip, byte[] port);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFSetWorkMode(byte mode);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFGetWorkMode(byte[] mode);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFSetBeep(byte mode);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFGetBeep(byte[] mode);

        /**********************************************************************************************************
        * 功能：设置FastID功能
        * 输入：flag -- 1:开启， 0：关闭
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        ///save: 1--保存   0--不保存    flag:1--开启    0:--不开启
        private extern static int UHFSetFastInventory(byte save, byte enable);

        /**********************************************************************************************************
        * 功能：获取FastID功能
        * 输出：flag -- 1:开启， 0：关闭
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFGetFastInventory(ref byte flag);



        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int TCPConnect(StringBuilder ip, uint hostport);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static void TCPDisconnect();

        //打开串口
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int ComOpenWithBaud(int port, int baudrate);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int ComOpen(int comName);
        //关闭串口
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static void ClosePort();

        /**********************************************************************************************************
           * 功能：获取硬件版本号
           * 输出：version[0]--版本号长度 ,  version[1--x]--版本号
           *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetHardwareVersion(byte[] version);
        /**********************************************************************************************************
          * 功能：获取软件版本号
          * 输出：version[0]--版本号长度 ,  version[1--x]--版本号
          *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetSoftwareVersion(byte[] version);
        /**********************************************************************************************************
           * 功能：获取ID号
           * 输出：id--整型ID号
           *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFGetDeviceID(ref int id);


        /**********************************************************************************************************
        * 功能：获取ID号
        * 输出：id--长度(1字节)+编码
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFGetDeviceIdEx(byte[] id);


        /**********************************************************************************************************
        * 功能：设置功率
        * 输入：saveflag  -- 1:保存设置   0：不保存
        * 输入：uPower -- 功率（DB）
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetPower(byte save, byte uPower);
        /**********************************************************************************************************
        * 功能：设置天线功率
        * 输入：saveflag  -- 1:保存设置   0：不保存
        * 输入：num -- 天线编号(1~N)
                read_power -- 接收功率（DB）
                write_power -- 发送功率（DB）
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetAntennaPower(byte save, byte num, byte read_power, byte write_power);

        /**********************************************************************************************************
        * 功能：设置多天线功率
        * 输入：save  -- 1:掉电保存   0：掉电不保存
        * 输入：num  -- 设置的天线个数
        * 输入：param -- 天线编号(1字节)+功率（1字节）+天线编号(1字节)+功率（1字节）+...
        * 天线编号：1~N，功率范围：1DB~30DB
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetMultiAntenna(byte save, byte num, byte[] param);


        /**********************************************************************************************************
        * 功能：获取功率
        * 输出：uPower -- 功率（DB）
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetPower(ref byte uPower);
        /**********************************************************************************************************
        * 功能：获取天线功率
        * 输出：ppower -- 天线功率,格式为（天线编号+读功率+写功率+天线编号+读功率+写功率+.......+天线编号+读功率+写功率）
		        nBytesReturned -- ppower数据长度 
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetAntennaPower(byte[] ppower, int[] nBytesReturned);




        /**********************************************************************************************************
        * 功能：设置跳频
        * 输入：nums -- 跳频个数
        * 输入：Freqbuf--频点数组（整型） ，如920125，921250.....
       *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetJumpFrequency(byte nums, int[] Freqbuf);
        /**********************************************************************************************************
        * 功能：获取跳频
        * 输出：Freqbuf[0]--频点个数， Freqbuf[1]..[x]--频点数组（整型）
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetJumpFrequency(int[] Freqbuf);
        /**********************************************************************************************************
        * 功能：设置Gen2参数
        * 输入：
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetGen2(byte Target, byte Action, byte T, byte Q, byte StartQ, byte MinQ, byte MaxQ, byte D, byte C, byte P, byte Sel, byte Session, byte G, byte LF);
        /**********************************************************************************************************
        * 功能：获取Gen2参数
        * 输入：
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetGen2(ref byte Target, ref byte Action, ref byte T, ref byte Q, ref byte StartQ, ref byte MinQ, ref byte MaxQ, ref byte D, ref byte Coding, ref byte P, ref byte Sel, ref byte Session, ref byte G, ref byte LF);
        /**********************************************************************************************************
        * 功能：设置CW
        * 输入：flag -- 1:开CW，  0：关CW
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetCW(byte flag);
        /**********************************************************************************************************
        * 功能：获取CW
        * 输出：flag -- 1:开CW，  0：关CW
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetCW(ref byte flag);

        /**********************************************************************************************************
        * 功能：天线设置
        * 输入：saveflag -- 1:掉电保存，  0：不保存
        * 输入：buf--2bytes, 共16bits, 每bit 置1选择对应天线
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetANT(byte saveflag, byte[] buf);

        /**********************************************************************************************************
        * 功能：获取天线设置
        * 输出：buf--2bytes, 共16bits,
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetANT(byte[] buf);

        /**********************************************************************************************************
          * 功能：天线设置支持32个天线
          * 输入：saveflag -- 1:掉电保存，  0：不保存
          * 输入：buf--4bytes, 共32bits, 每bit 置1选择对应天线
          *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetAntEx(byte saveflag, byte[] buf, int len);

        /**********************************************************************************************************
        * 功能：获取天线设置支持32个天线
        * 輸入：slen-status數組長度
        * 输出：buf ,
        * 返回：status状态长度，单位字节
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetAntEx(byte[] status, int slen);


        /**********************************************************************************************************
        * 功能：区域设置
        * 输入：saveflag -- 1:掉电保存，  0：不保存
        * 输入：region -- 0x01(China1),0x02(China2),0x04(Europe),0x08(USA),0x16(Korea),0x32(Japan)
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetRegion(byte saveflag, byte region);

        /**********************************************************************************************************
        * 功能：获取区域设置
        * 输出：region -- 0x01(China1),0x02(China2),0x04(Europe),0x08(USA),0x16(Korea),0x32(Japan)
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetRegion(ref byte region);



        /**********************************************************************************************************
        * 功能：设置温度保护
        * 输入：flag -- 1:温度保护， 0：无温度保护
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetTemperatureProtect(byte flag);
        /**********************************************************************************************************
        * 功能：获取温度保护
        * 输出：flag -- 1:温度保护， 0：无温度保护
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetTemperatureProtect(ref byte flag);
        /**********************************************************************************************************
        * 功能：设置天线工作时间
        * 输入：antnum -- 天线号
        * 输入：saveflag -- 1:掉电保存， 0：不保存
        * 输入：WorkTime -- 工作时间 ，单位ms, 范围 10-65535ms
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFSetANTWorkTime(byte antnum, byte saveflag, int WorkTime);
        /**********************************************************************************************************
        * 功能：获取天线工作时间
        * 输入：antnum -- 天线号
        * 输出：WorkTime -- 工作时间 ，单位ms, 范围 10-65535ms
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFGetANTWorkTime(byte antnum, ref int WorkTime);
        /**********************************************************************************************************
        * 功能：设置链路组合
        * 输入：saveflag -- 1:掉电保存， 0：不保存
        * 输入：mode --  0:DSB_ASK/FM0/40KHZ , 1:PR_ASK/Miller4/250KHZ , 2:PR_ASK/Miller4/300KHZ, 3:DSB_ASK/FM0/400KHZ
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetRFLink(byte saveflag, byte mode);

        /**********************************************************************************************************
        * 功能：获取链路组合
        * 输出：mode --  0:DSB_ASK/FM0/40KHZ , 1:PR_ASK/Miller4/250KHZ , 2:PR_ASK/Miller4/300KHZ, 3:DSB_ASK/FM0/400KHZ
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetRFLink(ref byte uMode);
        /**********************************************************************************************************
        * 功能：设置FastID功能
        * 输入：flag -- 1:开启， 0：关闭
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetFastID(byte flag);
        /**********************************************************************************************************
        * 功能：获取FastID功能
        * 输出：flag -- 1:开启， 0：关闭
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetFastID(ref byte flag);
        /**********************************************************************************************************
        * 功能：设置Tagfocus功能
        * 输入：flag -- 1:开启， 0：关闭
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetTagfocus(byte flag);
        /**********************************************************************************************************
        * 功能：获取Tagfocus功能
        * 输出：flag -- 1:开启， 0：关闭
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetTagfocus(ref byte flag);
        /**********************************************************************************************************
        * 功能：设置软件复位
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetSoftReset();
        /**********************************************************************************************************
        * 功能：设置Dual和Singel模式
        * 输入：saveflag -- 1:掉电保存， 0：不保存
        * 输入：mode -- 1:设置Singel模式， 0：设置Dual模式
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetDualSingelMode(byte saveflag, byte mode);
        /**********************************************************************************************************
        * 功能：获取Dual和Singel模式
        * 输出：mode -- 1:设置Singel模式， 0：设置Dual模式
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetDualSingelMode(ref byte mode);
        /**********************************************************************************************************
        * 功能：设置寻标签过滤设置
        * 输入：saveflag -- 1:掉电保存， 0：不保存
        * 输入：bank --  0x01:EPC , 0x02:TID, 0x03:USR
        * 输入：startaddr 起始地址，单位：字节
        * 输入：datalen 数据长度， 单位:字节
        * 输入：databuf 数据
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetFilter(byte saveflag, byte bank, int startaddr, int datalen, byte[] databuf);
        /**********************************************************************************************************
        * 功能：设置EPC和TID模式
        * 输入：saveflag -- 1:掉电保存， 0：不保存
        * 输入：mode -- 1：开启EPC和TID， 0:关闭
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetEPCTIDMode(byte saveflag, byte mode);
        /**********************************************************************************************************
        * 功能：获取EPC和TID模式
        * 输出：mode -- 1：开启EPC和TID， 0:关闭
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetEPCTIDMode(ref byte mode);

        /**********************************************************************************************************
       * 功能：恢复出厂设置
       *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFSetDefaultMode();
        /**********************************************************************************************************
        * 功能：单次盘存标签
        * 输出：uLenUii -- UII长度
        * 输出：uUii -- UII数据
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFInventorySingle(ref byte uLenUii, byte[] uUii);
        /**********************************************************************************************************
        * 功能：连续盘存标签
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFInventory();

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFPerformInventory(short mode, byte[] param, short paramlen);
        /**********************************************************************************************************
        * 功能：停止盘存标签
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFStopGet();
        /**********************************************************************************************************
          * 功能：获取连续盘存标签数据
          * 输出：uLenUii -- UII长度
          * 输出：uUii -- UII数据
          *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHF_GetReceived_EX(ref int uLenUii, byte[] uUii);
        /**********************************************************************************************************
        * 功能：读标签数据区
        * 输入：uAccessPwd -- 4字节密码
        * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
        * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
        * 输入：FilterLen -- 启动过滤的长度， 单位：字节
        * 输入：FilterData -- 启动过滤的数据
        * 输入：uBank -- 读取数据的bank
        * 输入：uPtr --  读取数据的起始地址， 单位：字
        * 输入：uCnt --  读取数据的长度， 单位：字
        * 输出：uReadDatabuf --  读取到的数据
        * 输出：uReadDataLen --  读取到的数据长度
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFReadData(byte[] uAccessPwd,
             byte FilterBank,
             int FilterStartaddr,
             int FilterLen,
             byte[] FilterData,
             byte uBank,
             int uPtr,
             int uCnt,
             byte[] uReadDatabuf,
             ref int uReadDataLen);

        /**********************************************************************************************************
          * 功能：写标签数据区
          * 输入：uAccessPwd -- 4字节密码
          * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
          * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：bit
          * 输入：FilterLen -- 启动过滤的长度， 单位：bit
          * 输入：FilterData -- 启动过滤的数据
          * 输入：uBank -- 写入数据的bank
          * 输入：uPtr --  写入数据的起始地址， 单位：字
          * 输入：uCnt --  写入数据的长度， 单位：字
          * 输入：uWriteDatabuf --  写入的数据
          *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFWriteData(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte uBank, int uPtr, byte uCnt, byte[] uDatabuf);
        /**********************************************************************************************************
        * 功能：LOCK标签
        * 输入：uAccessPwd -- 4字节密码
        * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
        * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
        * 输入：FilterLen -- 启动过滤的长度， 单位：字节
        * 输入：FilterData -- 启动过滤的数据
        * 输入：lockbuf --  3字节，第0-9位为Action位， 第10-19位为Mask位
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFLockTag(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte[] lockbuf);

        /**********************************************************************************************************
        * 功能：KILL标签
        * 输入：uAccessPwd -- 4字节密码
        * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
        * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
        * 输入：FilterLen -- 启动过滤的长度， 单位：字节
        * 输入：FilterData -- 启动过滤的数据
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFKillTag(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData);

        /**********************************************************************************************************
          * 功能：BlockWrite 特定长度的数据到标签的特定地址
          * 输入：uAccessPwd -- 4字节密码
          * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
          * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
          * 输入：FilterLen -- 启动过滤的长度， 单位：字节
          * 输入：FilterData -- 启动过滤的数据
          * 输入：uBank -- 块号  1：EPC, 2:TID, 3:USR
          * 输入：uPtr --  写入数据的起始地址， 单位：字
          * 输入：uCnt --   写入数据的长度， 单位：字
          * 输入：uWriteDatabuf --  写入的数据
          *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFBlockWriteData(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte uBank, int uPtr, int uCnt, byte[] uDatabuf);

        /**********************************************************************************************************
        * 功能：BlockErase 特定长度的数据到标签的特定地址
        * 输入：uAccessPwd -- 4字节密码
        * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
        * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
        * 输入：FilterLen -- 启动过滤的长度， 单位：字节
        * 输入：FilterData -- 启动过滤的数据
        * 输入：uBank -- 块号  1：EPC, 2:TID, 3:USR
        * 输入：uPtr --  读取数据的起始地址， 单位：字
        * 输入：uCnt --  读取数据的长度， 单位：字
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFBlockEraseData(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte uBank, int uPtr, byte uCnt);
        /**********************************************************************************************************
        * 功能：设置QT命令参数
        * 输入：uAccessPwd -- 4字节密码
        * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
        * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
        * 输入：FilterLen -- 启动过滤的长度， 单位：字节
        * 输入：FilterData -- 启动过滤的数据
        * 输入：QTData --  bit0：（0：表示无近距离控制 ， 1：表示启用近距离控制）  bit1：(0:表示启用private Memory map, 1：启用public memory map)
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetQT(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte QTData);
        /**********************************************************************************************************
        * 功能：获取QT命令参数
        * 输入：uAccessPwd -- 4字节密码
        * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
        * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
        * 输入：FilterLen -- 启动过滤的长度， 单位：字节
        * 输入：FilterData -- 启动过滤的数据
        * 输出：QTData --  bit0：（0：表示无近距离控制 ， 1：表示启用近距离控制）  bit1：(0:表示启用private Memory map, 1：启用public memory map)
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetQT(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, ref byte QTData);
        /**********************************************************************************************************
        * 功能：QT标签读操作
        * 输入：uAccessPwd -- 4字节密码
        * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
        * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
        * 输入：FilterLen -- 启动过滤的长度， 单位：字节
        * 输入：FilterData -- 启动过滤的数据
        * 输入：QTData --  bit0：（0：表示无近距离控制 ， 1：表示启用近距离控制）  
        * 输入：uBank -- 块号  1：EPC, 2:TID, 3:USR
        * 输入：uPtr --  读取数据的起始地址， 单位：字
        * 输入：uCnt --  读取数据的长度， 单位：字
        * 输出：uReadDatabuf --  读出的数据
        * 输出：uReadDataLen --  读出的数据长度
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFReadQT(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte QTData, byte uBank, int uPtr, byte uCnt, byte[] uReadDatabuf, ref byte uReadDataLen);
        /**********************************************************************************************************
        * 功能：QT标签写操作
        * 输入：uAccessPwd -- 4字节密码
        * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
        * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
        * 输入：FilterLen -- 启动过滤的长度， 单位：字节
        * 输入：FilterData -- 启动过滤的数据
        * 输入：QTData --  bit0：（0：表示无近距离控制 ， 1：表示启用近距离控制）  
        * 输入：uBank -- 块号  1：EPC, 2:TID, 3:USR
        * 输入：uPtr --  读取数据的起始地址， 单位：字
        * 输入：uCnt --  读取数据的长度， 单位：字
        * 输入：uWriteDatabuf --  写入的数据
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFWriteQT(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte QTData, byte uBank, int uPtr, byte uCnt, byte[] uDatabuf);
        /**********************************************************************************************************
        * 功能：Block Permalock操作
        * 输入：uAccessPwd -- 4字节密码
        * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
        * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
        * 输入：FilterLen -- 启动过滤的长度， 单位：字节
        * 输入：FilterData -- 启动过滤的数据
        * 输入：ReadLock --  bit0：（0：表示Read ， 1：表示Permalock）  
        * 输入：uBank -- 块号  1：EPC, 2:TID, 3:USR
        * 输入：uPtr --  Block起始地址 ，单位为16个块
        * 输入：uRange --  Block范围，单位为16个块
        * 输入：uMaskbuf -- 块掩码数据，2个字节，16bit 对应16个块
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFBlockPermalock(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte ReadLock, byte uBank, int uPtr, byte uRange, byte[] uMaskbuf);

        /**********************************************************************************************************
        * 功能：激活或失效EM4124标签
        * 输入：cmd --整形
        * 输入：uAccessPwd -- 4字节密码
        * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
        * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
        * 输入：FilterData -- 启动过滤的数据
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFDeactivate(int cmd, byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData);

        /**********************************************************************************************************
        * 功能：获取协议类型  
        * 输出：type  0x00 表示 ISO18000-6C 协议,    0x01 表示 GB/T 29768 国标协议,      0x02 表示 GJB 7377.1 国军标协议

        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetProtocolType(byte[] type);


        /**********************************************************************************************************
        * 功能：设置协议类型
        * 输入：type  0x00 表示 ISO18000-6C 协议,0x01 表示 GB/T 29768 国标协议,0x02 表示 GJB 7377.1 国军标协议
        * 
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetProtocolType(byte type);
        /**********************************************************************************************************
        * 功能：国标LOCK标签
        * 输入：uAccessPwd -- 4字节密码
        * 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
        * 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
        * 输入：FilterLen -- 启动过滤的长度， 单位：字节
        * 输入：FilterData -- 启动过滤的数据

        * 输入：memory 存储区：  0x00 表示标签信息区,   0x10 表示编码区,   0x20 表示安全区,   0x3x 表示用户区 0x30-0x3F 表示用户区编号 0 到编号 15
                config 配置：    0x00 表示配置存储区属性,    0x01 表示配置安全模式


		        action:  

                      配置存储区属性:  0x00:可读可写,  0x01:可读不可写,  0x02:不可读可写,  0x03:不可读不可写

			          配置安全模式:    0x00:保留,   0x01:不需要鉴别,   0x02:需要鉴别,不需要安全通信,   0x03:需要鉴别,需要安全通信

        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGBTagLock(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte memory, byte config, byte action);



        /**********************************************************************************************************
         * 功能：获取继电器和 IO 控制输出设置状态
         * 输入：outData[0]:    0:低电平   1：高电平
                 outData[1]:    0:低电平   1：高电平
           返回值：2：数据长度    -1：获取失败
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetIOControl(byte[] inputData);

        /**********************************************************************************************************
        * 功能：继电器和 IO 控制输出设置
        * 输入：output1:    0:低电平   1：高电平
                output2:    0:低电平   1：高电平
		        outStatus： 0：断开    1：闭合
          返回值：0：设置成功     -1：设置失败
        * 
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetIOControl(byte output1, byte output2, byte outStatus);



        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetOutputIO(byte[] output, byte outputLen);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetIOStatus(byte[] statusData, int[] dataLen);



        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFBuildDateTime(byte[] build_date, byte[] build_time);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetVersionCode(byte[] datetime);


        /**********************************************************************************************************
        * 功能：设置连续寻卡工作及等待时间
        * 输入：DByte4 为掉电保存标志，0 表示不保存，1 表示保存；DByte3、DByte2 为工作时间，高字节在前，DByte1、DByte0 为等待时间，高字节在前


          返回值：0：设置成功     -1：设置失败

        * 
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetWorkTime(byte save, byte work1, byte work2, byte wait1, byte wait2);

        /**********************************************************************************************************
        * 功能：获取连续寻卡工作及等待时间
        * 输出：DByte[0]、DByte[1] 表示工作时间；DByte[2]、DByte[3] 表示等待时间，单位为 ms，高位在前，最大 65535ms

          返回值：4：数据长度    -1：获取失败
        * 
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetWorkTime(byte[] data);



        /**********************************************************************************************************
        * 功能：设置EPC TID USER模式

        * 输入：saveflag -- 1:掉电保存， 0：不保存

        * 输入：Memory 0x00，表示关闭； 0x01，表示开启EPC+TID模式（默认 地址为 0x00, 长 度 为 6 个 字 ） ； 0x02, 表 示 开 启EPC+TID+USER模式

        * 输入：address 为USER区的起始地址（单位为字）
        * 输入：为USER区的长度（单位为字）
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetEPCTIDUSERMode(byte saveflag, byte memory, byte address, byte lenth);
        /**********************************************************************************************************
        * 功能：获取EPC TID USER 模式
        * 输入：rev1 :保留数据，传入0
        * 输入：rev2 :保留数据，传入0
        * 输出：mode[0]，Memory 0x00，表示关闭； 0x01，表示开启EPC+TID模式（默认 地址为 0x00, 长 度 为 6 个 字 ） ； 0x02, 表 示 开 启EPC+TID+USER模式
        * 输出：mode[1]address 为USER区的起始地址（单位为字）
        * 输出：mode[2]为USER区的长度（单位为字）
        * 返回值：3：正确，其它：错误
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetEPCTIDUSERMode(byte rev1, byte rev2, byte[] mode);





        /*
        初始化温度标签
        return: 0--success, -1--unknow error, others--error code
        mask_bank：掩码的数据区(0x00 为 Reserve 0x01 为 EPC， 0x02 表示 TID， 0x03 表示USR)。
        mask_addr：为掩码的地址(bit为单位)，高字节在前。
        mask_len：为掩码的长度(bit为单位)，高字节在前。
        mask_data：为掩码数据，mask_len为0时，这里没有数据
        */

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFInitRegFile(byte mask_bank, int mask_addr, int mask_len, byte[] mask_data);

        /*
        读取标签温度
        return: 0--success, -1--unknow error, others--error code
        mask_bank：掩码的数据区(0x00 为 Reserve 0x01 为 EPC， 0x02 表示 TID， 0x03 表示USR)。
        mask_addr：为掩码的地址(bit为单位)，高字节在前。
        mask_len：为掩码的长度(bit为单位)，高字节在前。
        mask_data：为掩码数据，mask_len为0时，这里没有数据
        temp:output,the point of temperature
        */
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFReadTagTemp(byte mask_bank, int mask_addr, int mask_len, byte[] mask_data, float[] outtemp);

        //level:0-close log output, 3-base log,4-detail log
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SetLogLevel(int level);


        /**********************************************************************************************************
        * 功能：设置是否保存传输过程中的日志文件，默认不保存
        * 输入： 
        *save -- 0-不保存、1-保存日志文件
        *返回值：无 
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SaveLogFile(int lsaveevel);



        // zjx 20191127 UHF升级--- start ---
        /*
            flag: 0,upgrade reader application
	              1,upgrade UHF module
	              2,upgrade reader bootloader 
	              3,upgrade Ex10 SDK firmware
            */
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFJump2Boot(byte flag);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFStartUpd();

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFUpdating(byte[] buf);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHF_Upgrade(byte[] buf, int length);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFStopUpdate();



        /**********************************************************************************************************
* 功能：获取读写器软件版本号
* 输出：version[0]--版本号长度 ,  version[1--x]--版本号
*********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetReaderVersion(byte[] version);


        // zjx 20191127 UHF升级--- end ---


        /****************************  zjx 20200416 触发工作模式参数设置获取  -------- start -------- **************************/
        /**********************************************************************************************************
        * 功能：设置触发工作模式参数
        * 输入：
                para[0],	     触发IO：0x00表示触发输入1；0x01表示触发输入2。
                para[1],para[2]  触发工作时间：表示有触发输入时读卡工作时间，单位是10ms，高字节先，低字节后。
                para[3],para[4]	触发时间间隔：表示触发工作时间结束后，间隔多久再判断触发输入IO口进行触发。
                para[5]     	标签输出方式：0x00表示串口输出，0x01表示UDP输出
        * 
        * 返回值：   0:成功    其它：失败
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetWorkModePara(byte[] para);


        /**********************************************************************************************************
        * 功能：获取触发工作模式参数
        * 输出：
                para[0],	     触发IO：0x00表示触发输入1；0x01表示触发输入2。
                para[1],para[2]  触发工作时间：表示有触发输入时读卡工作时间，单位是10ms，高字节先，低字节后。
                para[3],para[4]	 触发时间间隔：表示触发工作时间结束后，间隔多久再判断触发输入IO口进行触发。
                para[5]     	 标签输出方式：0x00表示串口输出，0x01表示UDP输出
        * 
        * 返回值：   0:成功    其它：失败
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetWorkModePara(byte[] para);


        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UsbOpen();
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static void UsbClose();

        /****************************  zjx 20200416 触发工作模式参数设置获取   -------- end -------- **************************/




        /***************************************************************************************/
        //获取当前连接的ip信息
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int LinkGetInfo(byte[] info, int len);

        //选择要操作的id，根据LinkGetInfo获取id信息
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int LinkSelect(int id);

        //获取当前可以操作的id
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int LinkGetSelected();

        //断开所以连接
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void LinkCloseAll();


        /**********************************************************************************************************
        * function:get status of antennas linked
        * out:link_status,status of antenna linked,bit0~bit15 indicate antenna1~antenna16,bit 0/not link 1/linked
        * return：0：success    -1：failure
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int UHFGetAntennaLinkStatus(short[] link_status);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int UHFVerifyVoltage(int[] value);




        /*
           按块写无源电子标签带水墨屏显示
           pwd：4 个字节的块写密码
           sector：掩码的数据区(0x00 为 Reserve，0x01 为 EPC，0x02 表示 TID，0x03 表示 USR)。
           mask_addr：为掩码的地址。
           mask_len：为掩码的长度。
           mask_data：为掩码数据。
           w_addr：为写入数据区的地址（单位是字）。
           w_len：为写入的数据长度（单位是字）。
           w_data：为写入的具体数据（txt 文件中的数据）。
           */
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int UHFWriteScreenBlockData(byte[] pwd, byte sector, short mask_addr, short mask_len, byte[] mask_data, byte type,
            short w_addr, short w_len, byte[] w_data);



        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int UHFUploadUserParam(byte[] param, short paramLen);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int UHFDownloadUserParam(byte[] param, short[] paramLen);


        //return 0,no data, > 0 tag length, < 0 error code
        //tdata tag data, type+length+content+...+type+length+content
        //type:1-epc,2-tid,3-user,4-rssi,5-antenna,6-id
        //

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int UHFGetTagData(byte[] tdata, int recvlen);


        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFInventorySingle(int id);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFStopSingle(int id);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFInventoryById(int id);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UHFStopById(int id);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFPerformInventoryById(int id, short mode, byte[] param, short paramlen);


        //   typedef enum{CELL_INVALID=0, CELL_CONNECT_ID=1, CELL_CONNECT_IP, CELL_UHF_PC, CELL_UHF_RSSI, CELL_UHF_ANTENNA, CELL_UHF_EPC, CELL_UHF_TID, CELL_UHF_USER,CELL_UHF_RESERVE,CELL_BARCODE, CELL_UHF_SENSOR} CELL_DATA_TYPE;



        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate void OnDisconnectCallback(int type);

        [DllImport("UHFAPI.dll", EntryPoint = "SetDisconnectCallback", CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetDisconnectCallback(OnDisconnectCallback disconnectCallback);



        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate void OnEventStopInventory(int id);

        [DllImport("UHFAPI.dll", EntryPoint = "setOnEventStopInventory", CallingConvention = CallingConvention.Cdecl)]
        public extern static void setOnEventStopInventory(OnEventStopInventory onEventStopInventory);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnDataReceived(IntPtr epc, short recvLen);//[MarshalAs(UnmanagedType.LPArray, SizeConst = 4096)]


        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void setOnDataReceived(OnDataReceived onDataRecved);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int PrintTextToCursor(int codeType, byte[] text, short len);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public extern static int BindUDP(int bindport);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public extern static void UnbindUDP();


        /**********************************************************************************************************
        * 功能：获取当前温度
        * 输出：temperature -- 整型
        *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFGetTemperature(ref int temperature);

        /**********************************************************************************************************
        * 功能：过滤重复标签
        * 输出：temperature -- 整型
        *********************************************************************************************************/
      //  public static extern int GetURDTagFilter(unsigned char* enable, unsigned long* timeout);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetURTagFilter(byte[] enable, int[] timeout);

      //public static extern int SetURTagFilter(unsigned char enable, unsigned long timeout);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetURTagFilter(byte enable, int timeout);

        ////type:0-UR4,1-UR1A,2-UR-DEV,3-WeiYuDa
        //extern "C" UHFAPI_API int GetURDeviceType(unsigned char* type);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
         public static extern int GetURDeviceType(byte[] type);

        //extern "C" UHFAPI_API int SetURDeviceType(unsigned char type);
         [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
         public static extern int SetURDeviceType(byte type);

        /*
         * 函数功能：  设置本机 IP 和端口号
         * 输出参数：  ipbuf + postbuf， IP+端口号
                       mask:掩码，4字节
                       gate:网关，4字节
                       dhcp:DHCP_MODE_STATIC(1) or DHCP_MODE_DYNAMIC(2)
         * 返回值：   0:成功    其它：失败
         */
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetEthernetHost(byte[] ipbuf, byte[] postbuf, byte[] mask, byte[] gate, byte dhcp);

        /*
         * 函数功能：  获取本机 IP 和端口号
         * 输出参数：  ipbuf + postbuf， IP+端口号
                       mask:掩码，4字节
                       gate:网关，4字节
                       dhcp:DHCP_MODE_STATIC(1) or DHCP_MODE_DYNAMIC(2)
         * 返回值：   0:成功    其它：失败
         */
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetEthernetHost(byte[] ipbuf, byte[] postbuf, byte[] mask, byte[] gate, byte[] dhcp);
        /**
         * @brief Margin read.
         * @param ap:the point of access password,4 bytes.
         * @param mmb:mask memory bank,this parameter can be one of value:
         *            @arg 0:Reserve 
         *            @arg 1:epc
         *            @arg 2:tid
         *            @arg 3:user
         * @param msa:the starting address of the mask area,in bits
         * @param mdl:the length of mask data,in bits
         * @param mdata:the point of mask data,Occupy N bytes,N=(mdl+7)/8

         * @param mb:memory bank will be read,this parameter can be one of value:
         *            @arg 0:Reserve 
         *            @arg 1:epc
         *            @arg 2:tid
         *            @arg 3:user
         * @param sa:the starting address of the mb area,in bits
         * @param dl:the length of read data,in bits
         * @param wdata:the point of data,Occupy N bytes,N=(mdl+7)/8

         * @retval status,0:execution successful,1:wrote failed, 34:found no tag, others:execution failed,
         */
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFMarginRead(byte[] ap, byte mmb, int msa, int mdl, byte[] mdata, byte mb, int sa, int dl, byte[] wdata);

        /**
         * @brief set tag protection mode and short-range mode.
         * @param ap:the point of access password,4 bytes.
         * @param mmb:mask memory bank,this parameter can be one of value:
         *            @arg 0:Reserve 
         *            @arg 1:epc
         *            @arg 2:tid
         *            @arg 3:user
         * @param msa:the starting address of the mask area,in bits
         * @param mdl:the length of mask data,in bits
         * @param mdata:the point of mask data,Occupy N bytes,N=(mdl+7)/8
         * @param pm:protect mode,this parameter can be one of value:
         *            @arg 0:disable Protected Mode
         *            @arg 1:enable Protected Mode
         * @param sr: 
         *            @arg 0:disable short mode
         *            @arg 1:enable short range
         * @retval status,0:execution successful,1:wrote failed, 34:found no tag, others:execution failed,
         */
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
         public static extern int UHFSetProtectionAndShortRange(byte[] ap, byte mmb, int msa, int mdl, byte[] mdata, byte pm, byte sr);

        //result:0-success,others-failure
        //blocking: 0-none blocking,1-blocking
        //mode: 0-single, 1 start, 2-stop
        //code:the point of return barcode value
        //len:the point of returned length
        //timeou:wait for result, millsecond
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ScannerRead(byte blocking, byte mode, byte[] code,int[] len, int timeout);


        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetURWorkSeconds(int[] work_secs);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetURWorkSeconds(int work_secs);

        /*
        功能：设置UHF模块通讯波特率，设置后必须重启UHF模块，重启后生效，掉电保存
        输入：baud:1--57600, 2--115200, 3--460800
        返回：0正常，其他失败
        */
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFSetBaudrate(byte baud);
        /*
        功能：获取UHF波特率
        输出：baud:1--57600, 2--115200, 3--460800
        返回：0正常，其他失败
        */
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFGetBaudrate(byte[] baud);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetNetMAC(byte[] mac);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetNetMAC(byte[] mac);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFGetInventoryMode(byte rev1, byte rev2, byte[] mode,int[] pmodelen);

        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFSetInventoryMode(byte saveflag, byte memory, byte address, byte lenth, int pwd);


        /**
         * @brief set fast inventory
         * @param saveFlag:specifies the value to be written to the selected bit.
         *         This parameter can be one of the values:
         *            @arg 0: Temporary effectiveness
         *            @arg 1: Permanent Preservation
         * @param cr: CRC mode,this parameter can be one of value:
         *            @arg 0:disable 
         *            @arg 1:CrID16
         *            @arg 2:CrStoredCRC
         *            @arg 3:CrRN16
         *            @arg 4:CrID32
         * @param code: code mode,this parameter can be one of value:
         *            @arg 1:CodeAntipodal
         *            @arg 2:CodeCCOneHalf
         *            @arg 3:CodeCCThreeQuarters
         * @param protection: protection mode,this parameter can be one of value:
         *            @arg 0:ProtectionNoProtection
         *            @arg 1:ProtectionParity
         *            @arg 2:ProtectionCRC5
         *            @arg 3:ProtectionCRC5Plus
         * @param id: protection mode,this parameter can be one of value:
         *            @arg 0:IdNoAckResponse
         *            @arg 1:IdTMNPlusTSN
         *            @arg 2:IdPart
         *            @arg 3:IdFull
         * @retval status,0:execution successful,others:execution failed
         */
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFSetFastInventoryEX(byte saveFlag, byte cr, byte code, byte protection, byte id);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFGetFastInventoryEX(byte[] cr, byte[] code, byte[] protection, byte[] id);
        #endregion



        #region 


        /*
          tdata:
          ---------------------------------------------
          |number|tag len|tag data|tag len|tag data|...
          ---------------------------------------------
          |1 byte|1 byte |N bytes |1 byte |N bytes |...
          ---------------------------------------------
          tlen:the numbre of bytes in tdata
        */
        //extern "C" UHFAPI_API int LocalExportTags(unsigned char *tdata, unsigned int *tlen);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public extern static int LocalExportTags(byte[] data, ref int tlen);

        //mode:0-all, 1-new tags number
        //extern "C" UHFAPI_API int LocalGetTagsNumber(int mode, unsigned int *number);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public extern static int LocalGetTagsNumber(int mode, ref int number);

        //extern "C" UHFAPI_API int LocalDeleteTags();
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public extern static int LocalDeleteTags();

        public static int GetFlashTagsNumber()
        {
            int number = 0;
            if (LocalGetTagsNumber(0, ref number) == 0)
            {
                return number;
            }
            return -1;
        }

        public static List<string> UsbExportTags()
        {
            byte[] tData = new byte[1024*10];
    
            while (true) {
                int tlen = 0;
                bool result = LocalExportTags(tData,ref tlen)==0;
                if (result)
                {
                    Console.WriteLine("UsbExportTags ==>" + DataConvert.ByteArrayToHexString(tData, tlen));

                    List<string> tags = new List<string>();
                    int count = tData[0];
                    int index = 1;
                    for (int k = 0; k < count; k++)
                    {
                        int tagLen = tData[index];
                        string tag = DataConvert.ByteArrayToHexString(Utils.CopyArray(tData, index + 1, tagLen));
                        tags.Add(tag);
                        index = index + tagLen + 1;
                    }
                    return tags;
                }
                return null;
            }
        }
        #endregion

        public const byte CELL_INVALID = 0;
        public const byte CELL_CONNECT_ID = 1;
        public const byte CELL_CONNECT_IP = 2;
        public const byte CELL_UHF_PC = 3;
        public const byte CELL_UHF_RSSI = 4;
        public const byte CELL_UHF_ANTENNA = 5;
        public const byte CELL_UHF_EPC = 6;
        public const byte CELL_UHF_TID = 7;
        public const byte CELL_UHF_USER = 8;
        public const byte CELL_UHF_RESERVE = 9;
        public const byte CELL_BARCODE = 10;
        public const byte CELL_UHF_SENSOR = 11;
        public const byte CELL_INPUT = 12;
        public const byte CELL_KEY_CODE = 13;
        public const byte CELL_UHF_PHASE = 14;
        public const byte CELL_UHF_FREQUENCY_POINT = 15;


        private static UHFAPI uhf = null;
        internal UHFAPI() { }
        public static UHFAPI getInstance()
        {
            if (uhf == null)
                uhf = new UHFAPI();
            return uhf;
        }

        #region  USB Device
        /// <summary>
        /// 打开USB设备(Turn on the USB device.)
        /// </summary>
        /// <returns>true:success false:fail</returns>
        public bool OpenUsb()
        {
            int result = UsbOpen();
            if (result == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 关闭USB设备(Turn off the USB device.)
        /// </summary>
        public void CloseUsb()
        {
            UsbClose();
        }
        #endregion

        #region TCPIP
        /// <summary>
        /// Connect to the reader through the network port.
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Port</param>
        /// <returns>true:success false:fail</returns>
        public bool TcpConnect(string ip, uint port)
        {

            if (ip == null || ip == "")
            {
                return false;
            }
            ip = ip.Trim();

            if (!StringUtils.isIP(ip))
            {
                return false;
            }
            StringBuilder bIp = new StringBuilder();
            bIp.Append(ip);

            Console.WriteLine("TCPConnect(" + ip + ", " + port + ")");
            int result = TCPConnect(bIp, port);
            if (result == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Disconnect
        /// </summary>
        /// <returns></returns>
        public void TcpDisconnect()
        {
            TCPDisconnect();
        }
        #endregion

        #region serial port
        /// <summary>
        /// 打开串口(Open the serial port)
        /// </summary>
        /// <param name="comName">0,1,2....</param>
        /// <returns>true:success false:fail</returns>
        public bool Open(int comName)
        {
            int result = ComOpen(comName);
            if (result == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 打开串口(Open the serial port)
        /// </summary>
        /// <param name="comName">0,1,2....</param>
        /// <param name="Baud">fixed: 115200</param>
        /// <returns>true:success false:fail</returns>
        public bool Open(int comName, int Baud)
        {
            int result = ComOpenWithBaud(comName, Baud);
            if (result == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 关闭串口(Close the serial port)
        /// </summary>
        /// <returns>true:success false:fail</returns>
        public bool Close()
        {
            ClosePort();
            return true;
        }

        #endregion

        #region Protocol
        /// <summary>
        /// 设置协议（Set protocol type）
        /// </summary>
        /// <param name="type">0x00:ISO18000-6C, 0x01:GB/T 29768, 0x02:GJB 7377.1 </param>
        /// <returns></returns>
        public bool SetProtocol(byte type)
        {
            if (UHFSetProtocolType(type) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        ///获取协议( Get protocol type)
        /// </summary>
        /// <returns>0x00:ISO18000-6C, 0x01:GB/T 29768, 0x02:GJB 7377.1</returns>
        public int GetProtocol()
        {
            byte[] type = new byte[1];
            if (UHFGetProtocolType(type) == 0)
            {
                return type[0];
            }
            return -1;
        }
        #endregion

        #region  Buzzer
        /// <summary>
        /// 设置蜂鸣器 (Set buzzer)
        /// </summary>
        /// <param name="mode">0x01:on  0x00:off</param>
        /// <remarks>蜂鸣器如果是打开状态那么盘点过程中读到标签后会有嘟嘟嘟的声音 (if the buzzer is on,it will "beep" during the inventory)</remarks>
        /// <returns>true:success  false:failure</returns>
        public bool UHFSetBuzzer(byte mode)
        {
            if (UHFSetBeep(mode) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 获取蜂鸣器状态 (Get buzzer state)
        /// </summary>
        /// <returns>1:打开(on)  0:关闭(off)</returns>
        public bool UHFGetBuzzer(byte[] mode)
        {
            if (UHFGetBeep(mode) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region Version、ID

        /// <summary>
        /// 获取硬件版本 (Get the hardware version.)
        /// </summary>
        /// <returns>version</returns>
        public string GetHardwareVersion()
        {
            byte[] version = new byte[50];
            if (UHFGetHardwareVersion(version) == 0)
            {
                int len = version[0];
                byte[] versionTemp = new byte[len];
                Array.Copy(version, 1, versionTemp, 0, len);
                return System.Text.Encoding.ASCII.GetString(versionTemp);// DataConvert.ByteArrayToHexString(versionTemp);
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取软件版本(Get the software version.)
        /// </summary>
        /// <returns>version</returns>
        public string GetSoftwareVersion()
        {
            byte[] version = new byte[50];
            if (UHFGetSoftwareVersion(version) == 0)
            {
                int len = version[0];
                byte[] versionTemp = new byte[len];
                Array.Copy(version, 1, versionTemp, 0, len);
                return System.Text.Encoding.ASCII.GetString(versionTemp);//DataConvert.ByteArrayToHexString(versionTemp);
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取主板版本(Gets the mainboard version.) 
        /// </summary>
        /// <returns>version</returns>
        public string GetSTM32Version()
        {
            byte[] version = new byte[50];
            if (UHFGetReaderVersion(version) == 0)
            {
                int len = version[0];
                byte[] versionTemp = new byte[len];
                Array.Copy(version, 1, versionTemp, 0, len);
                return System.Text.Encoding.ASCII.GetString(versionTemp);//DataConvert.ByteArrayToHexString(versionTemp);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取设备ID (Get the device ID.)
        /// </summary>
        /// <returns>hex data</returns>
        public string GetUHFGetDeviceID()
        {
            byte[] id = new byte[100];
            int len = UHFGetDeviceIdEx(id);
            if (len != 0)
            {
                return null;
            }
            byte[] data = Utils.CopyArray(id, 1, id[0]);
            return DataConvert.ByteArrayToHexString(data, data.Length);
        }
        public string GetAPIVersion()
        {
            byte[] time = new byte[40];
            int result = UHFGetVersionCode(time);
            return "Ver" + result + ".0 (" + System.Text.ASCIIEncoding.ASCII.GetString(time, 0, time.Length).Replace("\0", "") + ")";

        }
        #endregion

        #region  Frequency、Power、ANT、temperature
        /// <summary>
        /// 天线设置(Set up the antenna.)
        /// </summary>
        /// <param name="saveflag">1:保存设置(save)   0：不保存(do not save)</param>
        /// <param name="buf">buf--2bytes, 16bits, 16 ANT</param>
        /// 
        ///             buf[0]--> ANT16 - ANT9                                     buf[1]--> ANT8 - ANT1
        /// bit15  bit14  bit13  bit12  bit11  bit10  bit9   bit8       bit7  bit6  bit5  bit4  bit3  bit2  bit1  bit0
        /// ANT16  ANT15  ANT14  ANT13  ANT12  ANT11  ANT10  ANT9       ANT8  ANT7  ANT6  ANT5  ANT4  ANT3  ANT2  ANT1
        /// 
        /// <returns></returns>
        public bool SetANT(byte saveflag, byte[] buf)
        {
            if (UHFSetANT(saveflag, buf) == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取天线设置 (Get antenna Settings)
        /// </summary>
        /// <param name="buf">buf--2bytes, 16bits, 16 ANT</param>
        /// 
        ///             buf[0]--> ANT16 - ANT9                                     buf[1]--> ANT8 - ANT1
        /// bit15  bit14  bit13  bit12  bit11  bit10  bit9   bit8       bit7  bit6  bit5  bit4  bit3  bit2  bit1  bit0
        /// ANT16  ANT15  ANT14  ANT13  ANT12  ANT11  ANT10  ANT9       ANT8  ANT7  ANT6  ANT5  ANT4  ANT3  ANT2  ANT1
        /// 
        /// <returns></returns>
        public bool GetANT(byte[] buf)
        {
            if (UHFGetANT(buf) == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取天线连接状态(Get the antenna connection status.)
        /// </summary>
        /// <param name="buf">buf--2bytes, 16bits, 16 ANT</param>
        /// 
        ///             buf[0]--> ANT16 - ANT9                                     buf[1]--> ANT8 - ANT1
        /// bit15  bit14  bit13  bit12  bit11  bit10  bit9   bit8       bit7  bit6  bit5  bit4  bit3  bit2  bit1  bit0
        /// ANT16  ANT15  ANT14  ANT13  ANT12  ANT11  ANT10  ANT9       ANT8  ANT7  ANT6  ANT5  ANT4  ANT3  ANT2  ANT1
        /// 
        /// <returns></returns>
        public bool GetANTLinkStatus(short[] buf)
        {
            if (UHFGetAntennaLinkStatus(buf) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置区域设置 (Set Region)
        /// </summary>
        /// <param name="saveflag">1:保存设置(save)   0：不保存(do not save)</param>
        /// <param name="region">0x01(China1),0x02(China2),0x04(Europe),0x08(USA),0x16(Korea),0x32(Japan)</param>
        /// <returns></returns>
        public bool SetRegion(byte saveflag, byte region)
        {
            if (UHFSetRegion(saveflag, region) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取区域设置 (Get Region)
        /// </summary>
        /// <param name="region"> 0x01(China1),0x02(China2),0x04(Europe),0x08(USA),0x16(Korea),0x32(Japan)</param>
        /// <returns></returns>
        public bool GetRegion(ref byte region)
        {
            if (UHFGetRegion(ref region) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取模块温度 (Get UHF module temperature)
        /// </summary>
        /// <returns>temperature</returns>
        public string GetTemperature()
        {
            int temperature = 0;
            if (UHFGetTemperature(ref temperature) == 0)
            {
                return temperature.ToString();
            }
            return string.Empty;
        }


        /// <summary>
        /// 设置功率 (Set the power.)
        /// </summary>
        /// <param name="save">1:保存设置(save)   0：不保存(do not save)</param>
        /// <param name="uPower">功率(power)1-30</param>
        /// <returns>true:success false:fail</returns>
        public bool SetPower(byte save, byte uPower)
        {
            if (UHFSetPower(save, uPower) == 0)
                return true;
            return false;
        }
        /// <summary>
        /// 获取功率(Get the power.)
        /// </summary>
        /// <param name="uPower">功率(power)1-30</param>
        /// <returns>true:success false:fail</returns>
        public bool GetPower(ref byte uPower)
        {
            if (UHFGetPower(ref uPower) == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置功率 (Set the power.)
        /// </summary>
        /// <param name="save">1:保存设置(save)   0：不保存(do not save)</param>
        /// <param name="ant">天线(ANT)</param>
        /// <param name="uPower">功率(power)1-30</param>
        /// <returns>true:success false:fail</returns>
        public bool SetAntennaPower(byte save, byte ant, byte uPower)
        {
            if (UHFSetAntennaPower(save, ant, uPower, uPower) == 0)
                return true;
            return false;
        }
        /// <summary>
        /// 设置功率，新固件才支持 (Set the power, Only supported by new firmware)
        /// </summary>
        /// <param name="save">1:保存设置(save)   0：不保存(do not save)</param>
        /// <param name="num">设置的天线个数 (The number of antennas set)</param>
        /// <param name="param">天线编号(1字节,1-16)+ 功率（1字节,1-30 (dBm)）+ 天线编号(1字节)+ 功率（1字节）.... 
        ///                     ANT(1byte,1-16) + Power(1byte,1-30(dBm)) +ANT+Power ......
        /// </param>
        /// <returns></returns>
        public bool SetMultiAntennaPower(byte save, byte num, byte[] param)
        {
            if (UHFSetMultiAntenna(save, num, param) == 0)
                return true;
            return false;
        }
   
        /// <summary>
        /// 获取功率(Get the power.)
        /// </summary>
        /// <param name="uPower">uPower[0]:ANT1 Power ,uPower[1]:ANT2 Power, uPower[2]:ANT3 Power, uPower[3]:ANT4 Power</param>
        /// <returns>true:success false:fail</returns>
        public bool GetAntennaPower(byte[] uPower)
        {
            byte[] data = new byte[100];
            int[] resultLen = new int[1];
            if (UHFGetAntennaPower(data, resultLen) == 0)
            {
                for (int k = 0; k < resultLen[0] / 3; k++)
                {
                    uPower[k] = data[k * 3 + 1];
                }


                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取功率(Get the power.)
        /// </summary>
        /// <param name="uPower">功率(power) </param>
        /// <returns>> [0]:ANT1 Power , [1]:ANT2 Power, [2]:ANT3 Power,  [3]:ANT4 Power</returns>
        public byte[] GetAntennaAllPower()
        {
            byte[] data = new byte[100];
            int[] resultLen = new int[1];
            if (UHFGetAntennaPower(data, resultLen) == 0)
            {
                byte[] uPower = new byte[resultLen[0] / 3];
                for (int k = 0; k < resultLen[0] / 3; k++)
                {
                    uPower[k] = data[k * 3 + 1];
                }
                return uPower;
            }
            return null;
        }

        /// <summary>
        /// 设置频点(Set frequency point)
        /// </summary>
        /// <param name="nums">1</param>
        /// <param name="Freqbuf">920125，921250.....</param>
        /// <returns></returns>
        public bool SetJumpFrequency(byte nums, int[] Freqbuf)
        {
            if (UHFSetJumpFrequency(nums, Freqbuf) == 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region session
        /// <summary>
        /// Set session
        /// </summary>
        /// <param name="Target">Target parameter of the select command,0:s0  1:s1  2:s2  3:s3  4:SL</param>
        /// <param name="Action">Action parameter of the select command
        ///  * 0(Matching: assert SL or inventoried->A    ;  Non-Matching: de-assert SL or inventoried->B)<br>
        /// * 1(Matching: assert SL or inventoried->A    ;  Non-Matching: do nothing)<br>
        /// * 2(Matching: do nothing                     ;  Non-Matching: de-assert SL or inventoried->B)<br>
        /// * 3(Matching: negate SL or(A -> B, B -> A); Non-Matching: do nothing<br>
        /// * 4(Matching: de-assert SL or inventoried->B ;  Non-Matching: de-assert SL or inventoried->A<br>
        /// * 5(Matching: de-assert SL or inventoried->B ;  Non-Matching: do nothing<br>
        /// * 6(Matching: do nothing                     ;  Non-Matching: de-assert SL or inventoried->A<br>
        /// * 7(Matching: do nothing                     ;  Non-Matching: negate SL or(A->B, B->A))<br>
        /// </param>
        /// <param name="T">Truncate parameter of the select command,  0:Disable truncation , 1:Enable truncation</param>
        /// <param name="Q">0:fixed Q algorithm, 1:dynamic Q algorithm</param>
        /// <param name="StartQ"> 0,1,2,3.....15</param>
        /// <param name="MinQ">0,1,2,3.....15</param>
        /// <param name="MaxQ">0,1,2,3.....15</param>
        /// <param name="D">DR parameter of the query command</param>
        /// <param name="Coding">M parameter of the query command 0:FM0, 1:Miller2, 2:Miller4,3:Miller8<br></param>
        /// <param name="P">TRext parameter of the query command 0:No pilot,  1:Use pilot</param>
        /// <param name="Sel">sel parameter of the query command 0:All,  1:All,  2:~SL  ,3:SL</param>
        /// <param name="Session">session parameter of the query command 0:S0, 1:S1, 2:S2, 3:S3</param>
        /// <param name="G">Target parameter of the query command 0:A, 1:B</param>
        /// <param name="LF">Link Frequency setting  0:40KHz, 1:160KHz, 2:200KHz, 3:250KHz, 4:300KHz, 5:320KHz, 6:400KHz, 7:640KHz</param>
        /// <returns></returns>
        public bool SetGen2(byte Target, byte Action, byte T, byte Q,
                              byte StartQ, byte MinQ,
                              byte MaxQ, byte D, byte C, byte P,
                              byte Sel, byte Session, byte G, byte LF)
        {
            if (UHFSetGen2(Target, Action, T, Q, StartQ, MinQ, MaxQ, D, C, P, Sel, Session, G, LF) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get session
        /// </summary>
        /// <param name="Target">Target parameter of the select command,0:s0  1:s1  2:s2  3:s3  4:SL</param>
        /// <param name="Action">Action parameter of the select command
        ///  * 0(Matching: assert SL or inventoried->A    ;  Non-Matching: de-assert SL or inventoried->B)<br>
        /// * 1(Matching: assert SL or inventoried->A    ;  Non-Matching: do nothing)<br>
        /// * 2(Matching: do nothing                     ;  Non-Matching: de-assert SL or inventoried->B)<br>
        /// * 3(Matching: negate SL or(A -> B, B -> A); Non-Matching: do nothing<br>
        /// * 4(Matching: de-assert SL or inventoried->B ;  Non-Matching: de-assert SL or inventoried->A<br>
        /// * 5(Matching: de-assert SL or inventoried->B ;  Non-Matching: do nothing<br>
        /// * 6(Matching: do nothing                     ;  Non-Matching: de-assert SL or inventoried->A<br>
        /// * 7(Matching: do nothing                     ;  Non-Matching: negate SL or(A->B, B->A))<br>
        /// </param>
        /// <param name="T">Truncate parameter of the select command,  0:Disable truncation , 1:Enable truncation</param>
        /// <param name="Q">0:fixed Q algorithm, 1:dynamic Q algorithm</param>
        /// <param name="StartQ"> 0,1,2,3.....15</param>
        /// <param name="MinQ">0,1,2,3.....15</param>
        /// <param name="MaxQ">0,1,2,3.....15</param>
        /// <param name="D">DR parameter of the query command</param>
        /// <param name="Coding">M parameter of the query command 0:FM0, 1:Miller2, 2:Miller4,3:Miller8<br></param>
        /// <param name="P">TRext parameter of the query command 0:No pilot,  1:Use pilot</param>
        /// <param name="Sel">sel parameter of the query command 0:All,  1:All,  2:~SL  ,3:SL</param>
        /// <param name="Session">session parameter of the query command 0:S0, 1:S1, 2:S2, 3:S3</param>
        /// <param name="G">Target parameter of the query command 0:A, 1:B</param>
        /// <param name="LF">Link Frequency setting  0:40KHz, 1:160KHz, 2:200KHz, 3:250KHz, 4:300KHz, 5:320KHz, 6:400KHz, 7:640KHz</param>
        /// <returns></returns>
        public bool GetGen2(ref byte Target, ref byte Action, ref byte T, ref byte Q,
                   ref byte StartQ, ref byte MinQ,
                   ref byte MaxQ, ref byte D, ref byte Coding, ref byte P,
                   ref byte Sel, ref byte Session, ref byte G, ref byte LF)
        {
            if (UHFGetGen2(ref Target, ref Action, ref T, ref Q, ref StartQ, ref MinQ, ref MaxQ, ref D, ref Coding, ref P, ref Sel, ref Session, ref G, ref LF) == 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Continuous wave 
        /// <summary>
        /// 设置CW (Set Continuous wave)
        /// </summary>
        /// <param name="flag">flag -- 1:On，  0：Off</param>
        /// <returns></returns>
        public bool SetCW(byte flag)
        {
            if (UHFSetCW(flag) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取CW  (Get Continuous wave)
        /// </summary>
        /// <param name="flag">flag -- 1:On，  0：Off</param>
        /// <returns></returns>
        public bool GetCW(ref byte flag)
        {
            if (UHFGetCW(ref flag) == 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region RFLink、FastID、Tagfocus 

        /// <summary>
        /// 设置链路组合 (Set RFlink)
        /// </summary>
        /// <param name="saveflag">1:保存设置(save)   0：不保存(do not save)</param>
        /// <param name="mode">0:DSB_ASK/FM0/40KHZ , 1:PR_ASK/Miller4/250KHZ , 2:PR_ASK/Miller4/300KHZ, 3:DSB_ASK/FM0/400KHZ</param>
        /// <returns></returns>
        public bool SetRFLink(byte saveflag, byte mode)
        {

            if (UHFSetRFLink(saveflag, mode) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取链路组合 (Get RFlink )
        /// </summary>
        /// <param name="uMode">0:DSB_ASK/FM0/40KHZ , 1:PR_ASK/Miller4/250KHZ , 2:PR_ASK/Miller4/300KHZ, 3:DSB_ASK/FM0/400KHZ</param>
        /// <returns></returns>
        public bool GetRFLink(ref byte uMode)
        {
            if (UHFGetRFLink(ref uMode) == 0)
                return true;

            return false;
        }
        /// <summary>
        /// 设置FastID功能 (set FastID function)
        /// </summary>
        /// <param name="flag">1:开启(on)， 0：关闭(off)</param>
        /// <returns>true:success  false:failure</returns>
        public bool SetFastID(byte flag)
        {
            if (UHFSetFastID(flag) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取FastID功能 (Get FastID function)
        /// </summary>
        /// <param name="flag">1:开启(on)， 0：关闭(off)</param>
        /// <returns>true:success  false:failure</returns>
        public bool GetFastID(ref byte flag)
        {
            if (UHFGetFastID(ref flag) == 0)
                return true;
            return false;

        }
        /// <summary>
        /// 设置Inventory功能(Set Fast Inventory)
        /// </summary>
        /// <param name="flag">0x00,关闭； 0x01：CrID16， 0x02：CrStoredCRC，0x03：CrRN16，0x04：CrID32</param>
        /// <returns></returns>
        public bool SetFastInventory(int save, byte flag)
        {
            if (UHFSetFastInventory((byte)save, flag) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取FastInventory功能 (Get Fast Inventory)
        /// </summary>
        /// <param name="flag">1:开启(on)， 0：关闭(off)</param>
        /// <returns></returns>
        public bool GetFastInventory(ref byte flag)
        {
            if (UHFGetFastInventory(ref flag) == 0)
                return true;
            return false;

        }
        /// <summary>
        /// SetTagfocus
        /// </summary>
        /// <param name="flag">1:开启(on)， 0：关闭(off)</param>
        /// <returns></returns>
        public bool SetTagfocus(byte flag)
        {
            if (UHFSetTagfocus(flag) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// GetTagfocus
        /// </summary>
        /// <param name="flag">1:开启(on)， 0：关闭(off)</param>
        /// <returns></returns>
        public bool GetTagfocus(ref byte flag)
        {
            if (UHFGetTagfocus(ref flag) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Restart the reader device.
        /// </summary>
        /// <returns></returns>
        public bool SetSoftReset()
        {
            if (UHFSetSoftReset() == 0)
                return true;
            return false;
        }

        #endregion

        #region  Read、Write、LOCK、Kill


        /// <summary>
        /// 读取数据(Read data)
        /// </summary>
        /// <param name="uAccessPwd">访问密码(access password) 4 bites</param>
        /// <param name="FilterBank">启动过滤的bank号(Filtered storage area) 1：EPC, 2:TID, 3:USR</param>
        /// <param name="FilterStartaddr">启动过滤的起始地址(Filter start address)， unit:bit</param>
        /// <param name="FilterLen">启动过滤的长度， 单位：字节  (Filter data length(unit:bit), when filter data length is 0, it means no filter)</param>
        /// <param name="FilterData">启动过滤的数据(Filtered data)</param>
        /// <param name="uBank">读取数据的bank(Read storage area) 0x00:RESERVED   0x01:Bank_EPC    0x02:Bank_TID  0x03:Bank_USER</param>
        /// <param name="uPtr">读取数据的起始地址， 单位：字 (read start address(unit: word))</param>
        /// <param name="uCnt">读取数据的长度， 单位：字 (read data length(unit: word))</param>
        /// <returns>返回十六进制数据，读取失败返回"" (return acquired data, null means read failure)</returns>
        public string ReadData(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte uBank, int uPtr, int uCnt)
        {
            try
            {
                byte[] uReadDatabuf = new byte[2048]; ;
                int uReadDataLen = 0;

                StringBuilder sb = new StringBuilder();
                sb.Append("\r\n密码：" + DataConvert.ByteArrayToHexString(uAccessPwd));
                sb.Append("\r\n过滤数据块（ 1：EPC, 2:TID, 3:USR）：" + FilterBank);
                sb.Append("\r\n过滤起始地址：" + FilterStartaddr);
                sb.Append("\r\n过滤长度：" + FilterLen);
                sb.Append("\r\n过滤数据：" + DataConvert.ByteArrayToHexString(FilterData));
                sb.Append("\r\n");
                sb.Append("\r\n读取的数据块：" + uBank);
                sb.Append("\r\n读取的数据起始地址：" + uPtr);
                sb.Append("\r\n读取的数据长度：" + uCnt);
                sb.Append("\r\n");

               // FileManage.WriterFile("C:\\Users\\Administrator\\Desktop\\UHFLog.txt", sb.ToString(), true);

                int result = UHFReadData(uAccessPwd, FilterBank, FilterStartaddr, FilterLen, FilterData, uBank, uPtr, uCnt, uReadDatabuf, ref uReadDataLen);
                if (result == 0)
                {
                    return DataConvert.ByteArrayToHexString(uReadDatabuf, uReadDataLen);
                }
            }
            catch (Exception ex)
            {

            }

            return string.Empty;
        }


        /// <summary>
        /// 写标签数据区（Write data in tag）
        /// </summary>
        /// <param name="uAccessPwd">访问密码(access password) 4 bites</param>
        /// <param name="FilterBank">启动过滤的bank号(Filtered storage area) 1：EPC, 2:TID, 3:USR</param>
        /// <param name="FilterStartaddr">启动过滤的起始地址(Filter start address)， unit:bit</param>
        /// <param name="FilterLen">启动过滤的长度， 单位：字节  (Filter data length(unit:bit), when filter data length is 0, it means no filter)</param>
        /// <param name="FilterData">启动过滤的数据(Filtered data)</param>
        /// <param name="uBank">标签的存储区(Storage area): 0x00:RESERVED   0x01:Bank_EPC    0x02:Bank_TID  0x03:Bank_USER</param>
        /// <param name="uPtr">起始地址的偏移量(start address(unit: word))</param>
        /// <param name="uCnt">数据的长度（Word为单位，不能为0）(Data length(Word is unit, cannot be 0))</param>
        /// <param name="uDatabuf">要写入的数据（Data）</param>
        /// <returns></returns>
        public bool WriteData(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte uBank, int uPtr, byte uCnt, byte[] uDatabuf)
        {
            if (UHFWriteData(uAccessPwd, FilterBank, FilterStartaddr, FilterLen, FilterData, uBank, uPtr, uCnt, uDatabuf) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 写数据到ECP (Write data to ECP)
        /// </summary>
        /// <param name="uAccessPwd">访问密码(access password) 4 bites</param>
        /// <param name="filterBank">启动过滤的bank号(Filtered storage area) 1：EPC, 2:TID, 3:USR</param>
        /// <param name="filterPtr">启动过滤的起始地址(Filter start address)， unit:bit</param>
        /// <param name="filterCnt">启动过滤的长度， 单位：字节  (Filter data length(unit:bit), when filter data length is 0, it means no filter)</param>
        /// <param name="filterData">启动过滤的数据(Filtered data)</param>
        /// <param name="writeData">要写入的数据（Data）</param>
        /// <returns></returns>
        public bool writeDataToEpc(byte[] accessPwd, byte filterBank, int filterPtr, int filterCnt, byte[] filterData, byte[] writeData)
        {
            if (writeData == null || writeData.Length == 0 || (writeData.Length % 2 != 0))
            {
                throw new Exception("The length of the written data must be a multiple of 2.");
            }
            byte[] newWriteData = new byte[writeData.Length + 2];
            newWriteData[0] = (byte)((writeData.Length / 2) << 3);
            newWriteData[1] = 0;
            Array.Copy(writeData, 0, newWriteData, 2, writeData.Length);
            byte cnt = (byte)(newWriteData.Length / 2);

            return WriteData(accessPwd, filterBank, filterPtr, filterCnt, filterData, 1, 1, cnt, newWriteData);
        }


        /// <summary>
        /// LOCK 
        /// </summary>
        /// <param name="uAccessPwd">访问密码(access password) 4 bites</param>
        /// <param name="FilterBank">启动过滤的bank号(Filtered storage area) 1：EPC, 2:TID, 3:USR</param>
        /// <param name="FilterStartaddr">启动过滤的起始地址(Filter start address)， unit:bit</param>
        /// <param name="FilterLen">启动过滤的长度， 单位：字节  (Filter data length(unit:bit), when filter data length is 0, it means no filter)</param>
        /// <param name="FilterData">启动过滤的数据(Filtered data)</param>
        /// <param name="lockbuf">3bytes，Refer To Demo</param>
        /// <returns></returns>
        public bool LockTag(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte[] lockbuf)
        {
            if (UHFLockTag(uAccessPwd, FilterBank, FilterStartaddr, FilterLen, FilterData, lockbuf) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        ///  KILL 
        /// </summary>
        /// <param name="uAccessPwd">访问密码(access password) 4 bites</param>
        /// <param name="FilterBank">启动过滤的bank号(Filtered storage area) 1：EPC, 2:TID, 3:USR</param>
        /// <param name="FilterStartaddr">启动过滤的起始地址(Filter start address)， unit:bit</param>
        /// <param name="FilterLen">启动过滤的长度， 单位：字节  (Filter data length(unit:bit), when filter data length is 0, it means no filter)</param>
        /// <param name="FilterData">启动过滤的数据(Filtered data)</param>
        /// <returns></returns>
        public bool KillTag(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData)
        {
            if (UHFKillTag(uAccessPwd, FilterBank, FilterStartaddr, FilterLen, FilterData) == 0)
            {

                return true;
            }
            return false;

        }
        /// <summary>
        /// BlockWrite  
        /// </summary>
        /// <param name="uAccessPwd">访问密码(access password) 4 bites</param>
        /// <param name="FilterBank">启动过滤的bank号(Filtered storage area) 1：EPC, 2:TID, 3:USR</param>
        /// <param name="FilterStartaddr">启动过滤的起始地址(Filter start address)， unit:bit</param>
        /// <param name="FilterLen">启动过滤的长度， 单位：字节  (Filter data length(unit:bit), when filter data length is 0, it means no filter)</param>
        /// <param name="FilterData">启动过滤的数据(Filtered data)</param>
        /// <param name="uBank"> 1：EPC, 2:TID, 3:USR</param>
        /// <param name="uPtr">起始地址的偏移量(start address(unit: word))</param>
        /// <param name="uCnt">数据的长度（Word为单位，不能为0）(Data length(Word is unit, cannot be 0))</param>
        /// <param name="uDatabuf">要写入的数据（Data）</param>
        /// <returns></returns>
        public bool BlockWriteData(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte uBank, int uPtr, int uCnt, byte[] uDatabuf)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\r\nUHFBlockWriteData================>");
            sb.Append("\r\n密码：" + DataConvert.ByteArrayToHexString(uAccessPwd));
            sb.Append("\r\n过滤数据块（ 1：EPC, 2:TID, 3:USR）：" + FilterBank);
            sb.Append("\r\n过滤起始地址：" + FilterStartaddr);
            sb.Append("\r\n过滤长度：" + FilterLen);
            sb.Append("\r\n过滤数据：" + DataConvert.ByteArrayToHexString(FilterData));
            sb.Append("\r\n");
            sb.Append("\r\n写入的数据块：" + uBank);
            sb.Append("\r\n写入的数据起始地址：" + uPtr);
            sb.Append("\r\n写入的数据长度：" + uCnt);
            sb.Append("\r\n写入的数据内容：" + DataConvert.ByteArrayToHexString(uDatabuf));
            sb.Append("\r\n");

            //FileManage.WriterFile("C:\\Users\\Administrator\\Desktop\\UHFLog.txt", sb.ToString(), true);
            if (UHFBlockWriteData(uAccessPwd, FilterBank, FilterStartaddr, FilterLen, FilterData, uBank, uPtr, uCnt, uDatabuf) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// BlockErase  
        /// </summary>
        /// <param name="uAccessPwd">访问密码(access password) 4 bites</param>
        /// <param name="FilterBank">启动过滤的bank号(Filtered storage area) 1：EPC, 2:TID, 3:USR</param>
        /// <param name="FilterStartaddr">启动过滤的起始地址(Filter start address)， unit:bit</param>
        /// <param name="FilterLen">启动过滤的长度， 单位：字节  (Filter data length(unit:bit), when filter data length is 0, it means no filter)</param>
        /// <param name="FilterData">启动过滤的数据(Filtered data)</param>
        /// <param name="uBank"> 1：EPC, 2:TID, 3:USR</param>
        /// <param name="uPtr">起始地址的偏移量(start address(unit: word))</param>
        /// <param name="uCnt">数据的长度（Word为单位，不能为0）(Data length(Word is unit, cannot be 0))</param>
        /// <returns></returns>
        public bool BlockEraseData(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte uBank, int uPtr, byte uCnt)
        {
            if (UHFBlockEraseData(uAccessPwd, FilterBank, FilterStartaddr, FilterLen, FilterData, uBank, uPtr, uCnt) == 0)
            {

                return true;
            }
            return false;
        }
        /// <summary>
        /// Block Permalock 
        /// </summary>
        /// <param name="uAccessPwd">访问密码(access password) 4 bites</param>
        /// <param name="FilterBank">启动过滤的bank号(Filtered storage area) 1：EPC, 2:TID, 3:USR</param>
        /// <param name="FilterStartaddr">启动过滤的起始地址(Filter start address)， unit:bit</param>
        /// <param name="FilterLen">启动过滤的长度， 单位：字节  (Filter data length(unit:bit), when filter data length is 0, it means no filter)</param>
        /// <param name="FilterData">启动过滤的数据(Filtered data)</param>
        /// <param name="ReadLock">bit0：（0：Read ， 1：Permalock）  </param>
        /// <param name="uBank">1：EPC, 2:TID, 3:USR</param>
        /// <param name="uPtr">Block start address ，1-16</param>
        /// <param name="uRange">Block end address，1-16</param>
        /// <param name="uMaskbuf">16bit</param>
        /// <returns></returns>
        public bool BlockPermalock(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte ReadLock, byte uBank, int uPtr, byte uRange, byte[] uMaskbuf)
        {
            if (UHFBlockPermalock(uAccessPwd, FilterBank, FilterStartaddr, FilterLen, FilterData, ReadLock, uBank, uPtr, uRange, uMaskbuf) == 0)
            {
                return true;

            }
            return false;
        }
        #endregion

        #region   Inventory
        /// <summary>
        /// 设置寻标签过滤设置 Filter tag in auto scan mode, before scanning tags {@link #Inventory() }, setup data that needs to be filtered.<br>
        /// </summary>
        /// <param name="saveflag">1:保存设置(save)   0：不保存(do not save)</param>
        /// <param name="bank">0x01:EPC , 0x02:TID, 0x03:USR</param>
        /// <param name="startaddr">起始地址的偏移量(start address(unit: bit))</param>
        /// <param name="datalen">数据长度， 单位:字节( Data length(Word is unit, cannot be 0) )</param>
        /// <param name="databuf">数据(Data)</param>
        /// <returns></returns>
        public bool SetFilter(byte saveflag, byte bank, int startaddr, int datalen, byte[] databuf)
        {
            if (UHFSetFilter(saveflag, bank, startaddr, datalen, databuf) == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 开始循环识别标签
        /// Begin looping through the identification labels
        /// 
        /// 备注：开启循环识别标签后模块只能响应{@link #stopInventory()}函数.
        /// Note: The module can only respond to the {@link #stopInventory()} function after the loop identification tag is turned on
        /// </summary>
        /// <returns></returns>
        public bool StartInventory()
        {

            int result = UHFInventory();
            if (result == 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// StopInventory
        /// </summary>
        /// <returns></returns>
        public bool StopInventory()
        {

            if (UHFStopGet() == 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StartInventoryNeedPhase()
        {
            if (UHFPerformInventory(4, null, 0) == 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 获取缓冲区的标签数据
        /// Get tag data from buffer
        /// 
        ///  {@link #StartInventory() }启动识别标签之后，在子线程循环调用此函数不断获取缓冲区的标签信息，每次返回一张标签数据
        ///  {@link #startInventoryTag() } After tag reading has been enabled, call this function in sub threads to get data information continously, return one tag information for each time.<br>
        /// </summary>
        /// <param name="len">data len</param>
        /// <param name="data">data</param>
        /// <returns></returns>
        public bool ReadTagFromBuffer(ref int len, ref byte[] data)
        {
            if (UHF_GetReceived_EX(ref len, data) == 0)
            {
                //  data = 1byte PcAndEPC len + PcAndEPC data + 1byteTID数据+TID+2bytesRSSI
                /**
                 * 
                 * if(epcMode)
                 * {
                 *     data =  1byte PcAndEPC len + PcAndEPC data + 1byte TID len + 2byte Rssi +1byte ANT
                 * }
                 * else if(epcAndTidMode)
                 * {
                 *     data =  1byte PcAndEPC len + PcAndEPC data + 1byte TID len + 12bytes Tid data + 2byte Rssi +1byte ANT
                 * }
                 * else if(epcAndTidUser)
                 * {
                 *     data =  1byte PcAndEPC len + PcAndEPC data + 1byte TIDAndUser len + 12bytes Tid data+ UserData + 2byte Rssi +1byte ANT
                 * }
                 * 
                 * 
                 */
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取缓冲区的标签数据
        /// Get tag data from buffer
        /// 
        ///  {@link #StartInventory() }启动识别标签之后，在子线程循环调用此函数不断获取缓冲区的标签信息，每次返回一张标签数据
        ///  {@link #startInventoryTag() } After tag reading has been enabled, call this function in sub threads to get data information continously, return one tag information for each time.<br>
        /// </summary>
        /// <returns></returns>
        public UHFTAGInfo ReadTagFromBuffer()
        {
            int uLen = 0;
            byte[] bufData = new byte[256];
            if (ReadTagFromBuffer(ref uLen, ref bufData))
            {
                string epc_data = string.Empty;
                string uii_data = string.Empty;//uii数据
                string tid_data = string.Empty; //tid数据
                string rssi_data = string.Empty;
                string ant_data = string.Empty;
                string user_data = string.Empty;

                int uii_len = bufData[0];//uii长度
                int tid_leng = bufData[uii_len + 1];//tid数据长度
                int tid_idex = uii_len + 2;//tid起始位
                int rssi_index = 1 + uii_len + 1 + tid_leng;
                int ant_index = rssi_index + 2;

                string strData = BitConverter.ToString(bufData, 0, uLen).Replace("-", "");
                epc_data = strData.Substring(6, uii_len * 2 - 4);  //Epc

                if (tid_leng > 12)
                {
                    tid_data = strData.Substring(tid_idex * 2, 24); //Tid
                    user_data = strData.Substring(tid_idex * 2 + 24, (tid_leng - 12) * 2); //Tid
                }
                else
                {
                    tid_data = strData.Substring(tid_idex * 2, tid_leng * 2); //Tid
                    if (tid_data.Length < 8)
                    {
                        tid_data = "";
                    }
                }

                string temp = strData.Substring(rssi_index * 2, 4);
                int rssiTemp = Convert.ToInt32(temp, 16) - 65535;
                rssi_data = ((float)rssiTemp / 10.0).ToString();// RSSI  =  (0xFED6   -65535)/10
                if (!rssi_data.Contains("."))
                {
                    rssi_data = rssi_data + ".0";
                }
                ant_data = Convert.ToInt32((strData.Substring(ant_index * 2, 2)), 16).ToString();

                UHFTAGInfo info = new UHFTAGInfo();
                info.Epc = epc_data;
                info.Tid = tid_data;
                info.Rssi = rssi_data;
                info.Ant = ant_data;
                info.User = user_data;

                return info;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取缓冲区的标签数据 (支持相位数据)
        /// Get tag data from buffer (Support phase data)
        /// 
        ///  {@link #StartInventory() }启动识别标签之后，在子线程循环调用此函数不断获取缓冲区的标签信息，每次返回一张标签数据
        ///  {@link #startInventoryTag() } After tag reading has been enabled, call this function in sub threads to get data information continously, return one tag information for each time.<br>
        ///
        ///
        /// </summary>
        /// <returns></returns>
        public TagInfo GetTagData()
        {
            TagInfo info = new TagInfo();
            byte[] tagTempData = new byte[150]; //Array.Clear(tagTempData, 0, tagTempData.Length);
            int result = UHFAPI.UHFGetTagData(tagTempData, tagTempData.Length);
            info.ErrCode = result;
            if (result > 0)
            {
               // 01 02 00 01
               // 03 02 28 00
               // 06 0A 00 00 34 34 34 34 34 34 34 34
               // 07 04 00 00 00 00
               // 04 02 FD D9
               // 05 01 01
               // Console.WriteLine("GetTagData ==>"+DataConvert.ByteArrayToHexString(tagTempData, result));
                int index = 0;
                UHFTAGInfo uhfinfo = new UHFTAGInfo();
                while (true)
                {
                    if (index > result)
                    {
                        break;
                    }
                    int type = tagTempData[index];
                    index = index + 1;
                    if (index > result)
                    {
                        break;
                    }
                    int len = tagTempData[index];
                    index = index + 1;
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
                        if (data.Length > 12)
                        {
                            //tid
                            uhfinfo.Tid = BitConverter.ToString(data, 0, 12).Replace("-", "");
                            //user
                            uhfinfo.User = BitConverter.ToString(data, 12, data.Length - 12).Replace("-", "");
                        }
                        else
                        {
                            uhfinfo.Tid = BitConverter.ToString(data, 0, data.Length).Replace("-", "");
                        }
                    }
                    else if (type == UHFAPI.CELL_UHF_USER)
                    {
                  
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

                        uhfinfo.Rssi = rssi_data+"";


                    }
                    else if (type == UHFAPI.CELL_UHF_ANTENNA)
                    {
                        //ant
                        uhfinfo.Ant = data[0]+"";
                    }
                    else if (type == UHFAPI.CELL_CONNECT_ID)
                    {
                        //id
                        info.Id = data[0];
                    }
                    else if (type == UHFAPI.CELL_UHF_PHASE)
                    {
                        //PHASE
                        uhfinfo.Phase = (data[0] << 8) | data[1];
                        Console.WriteLine("info.Phase =" + uhfinfo.Phase);

                    }
                }
                info.UhfTagInfo = uhfinfo;
                if (uhfinfo.Ant == null)
                {
                    uhfinfo.Ant = "1";
                    // Console.WriteLine("GetTagData ==>" + DataConvert.ByteArrayToHexString(tagTempData, result));
                    // return null;
                }

            }
            return info;
        }
        /// <summary>
        /// 设置循环盘点只获取EPC的数据(Setup auto scan to acquire EPC only)
        /// </summary>
        /// <returns></returns>
        public bool setEPCMode(bool isSave)
        {
            int flag = isSave ? 1 : 0;
            if (UHFSetEPCTIDUSERMode((byte)flag, 0, 0, 0) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置循环盘点同时读取 EPC、TID 模式(Setup auto scan to acquire EPC, TID mode)
        /// </summary>
        /// <returns></returns>
        public bool setEPCAndTIDMode(bool isSave)
        {
            int flag = isSave ? 1 : 0;
            if (UHFSetEPCTIDUSERMode((byte)flag, 0x01, 0, 0) == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置循环盘点同时读取 EPC、TID、USER 模式  ( Setup auto scan to acquire EPC, TID, User mode)
        /// </summary>
        /// <param name="isSave"></param>
        /// <param name="userAddress">Start addressin USER area</param>
        /// <param name="userLenth">Data length in USER area</param>
        /// <returns></returns>
        public bool setEPCAndTIDUSERMode(bool isSave, byte userAddress, byte userLenth)
        {
            int flag = isSave ? 1 : 0;
            if (UHFSetEPCTIDUSERMode((byte)flag, 0x02, userAddress, userLenth) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取当前设置的盘点模式(Get the currently set inventory mode)
        /// </summary>
        /// <param name="userAddress">Start addressin USER area</param>
        /// <param name="userLenth">Data length in USER area</param>
        /// <returns>0:EPC;  1:EPC+TID;  2:EPC+TID:USER</returns>
        public int getEPCTIDUSERMode(ref byte userAddress, ref byte userLenth)
        {
            byte[] mode = new byte[10];
            int result = UHFGetEPCTIDUSERMode(0, 0, mode);
            if (result > 0)
            {
                userAddress = mode[1];
                userLenth = mode[2];
                return mode[0];
            }
            else
            {
                return -1;
            }
        }

        #endregion

        #region  ReaderIP Of UR4
        /// <summary>
        /// 获取读写器IP地址(Get the reader IP address)
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Port</param>
        /// <param name="mask">subnet mask</param>
        /// <param name="gate">gateway</param>
        /// <param name="ipMode">1:static 2:dhcp</param>
        /// <returns>true:success false:fail</returns>
        public bool SetReaderIP(string ip, int port, string mask, string gate,bool isDhcp)
        {

            if (!StringUtils.isIP(ip))
            {
                return false;
            }
            if (!StringUtils.isIP(mask))
            {
                return false;
            }
            if (!StringUtils.isIP(gate))
            {
                return false;
            }
            byte[] bPort = new byte[2];
            byte[] bIP = new byte[4];
            byte[] bmask = new byte[4];
            byte[] bgate = new byte[4];

            string hexPort = DataConvert.DecimalToHexString(port);
            bPort = DataConvert.HexStringToByteArray("0000".Substring(0, 4 - hexPort.Length) + hexPort);

            string[] strIp = ip.Split('.');
            for (int k = 0; k < strIp.Length; k++)
            {
                bIP[k] = byte.Parse(strIp[k]);
            }

            string[] temp = mask.Split('.');
            for (int k = 0; k < temp.Length; k++)
            {
                bmask[k] = byte.Parse(temp[k]);
            }

            temp = gate.Split('.');
            for (int k = 0; k < temp.Length; k++)
            {
                bgate[k] = byte.Parse(temp[k]);
            }
 

            if (SetEthernetHost(bIP, bPort, bmask, bgate, isDhcp?(byte)2:(byte)1) == 0) // if (UHFSetIp(bIP, bPort, bmask, bgate) == 0) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 设置读写器IP地址(Set the reader IP address)
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="mask"></param>
        /// <param name="gate"></param>
        /// <returns>true:success false:fail</returns>
        public bool GetReaderIP(StringBuilder ip, StringBuilder port, StringBuilder mask, StringBuilder gate, bool[] dhcp)
        {
            byte[] sIP = new byte[4];
            byte[] sPort = new byte[2];

            byte[] sMask = new byte[4];
            byte[] sGate = new byte[4];
            int startTime = Environment.TickCount;
            byte[] dhcpTemp = new byte[1];
            if (GetEthernetHost(sIP, sPort, sMask, sGate, dhcpTemp) == 0) //if (UHFGetIp(sIP, sPort, sMask, sGate) == 0) 
            {
                // MessageBoxEx.Show((Environment.TickCount-startTime)+"");
                if(dhcp!=null && dhcp.Length > 0)
                {
                    dhcp[0] = (dhcpTemp[0] == 2);
                }

                if (ip != null)
                {
                    ip.Append(sIP[0]);
                    ip.Append(".");
                    ip.Append(sIP[1]);
                    ip.Append(".");
                    ip.Append(sIP[2]);
                    ip.Append(".");
                    ip.Append(sIP[3]);
                }
                if (port != null)
                {
                    string hexPort = DataConvert.ByteArrayToHexString(sPort).Replace(" ", "");
                    int iPort = Convert.ToInt32(hexPort, 16);
                    port.Append(iPort);
                }

                if (sMask != null)
                {
                    if (sMask[0] == 0 && sMask[1] == 0 && sMask[2] == 0 && sMask[3] == 0)
                    {
                        sMask[0] = 255;
                        sMask[1] = 255;
                        sMask[2] = 255;
                        sMask[3] = 0;
                    }
                    mask.Append(sMask[0]);
                    mask.Append(".");
                    mask.Append(sMask[1]);
                    mask.Append(".");
                    mask.Append(sMask[2]);
                    mask.Append(".");
                    mask.Append(sMask[3]);
                }


                if (sGate != null)
                {
                    if (sGate[0] == 0 && sGate[1] == 0 && sGate[2] == 0 && sGate[3] == 0)
                    {
                        sGate[0] = sIP[0];
                        sGate[1] = sIP[1];
                        sGate[2] = sIP[2];
                        sGate[3] = 1;
                    }
                    gate.Append(sGate[0]);
                    gate.Append(".");
                    gate.Append(sGate[1]);
                    gate.Append(".");
                    gate.Append(sGate[2]);
                    gate.Append(".");
                    gate.Append(sGate[3]);
                }


                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region   UDP DestIP Of UR4
        /// <summary>
        /// 设置自动模式下，接收数据的IP地址。(Set the IP address to receive data in automatic mode)
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns>true:success false:fail</returns>
        public bool SetDestIP(string ip, int port)
        {
            if (ip == null || ip == "")
            {
                return false;
            }
            ip = ip.Trim();

            if (!StringUtils.isIP(ip))
            {
                return false;
            }
            byte[] bPort = new byte[2];
            byte[] bIP = new byte[4];

            string hexPort = DataConvert.DecimalToHexString(port);
            bPort = DataConvert.HexStringToByteArray("0000".Substring(0, 4 - hexPort.Length) + hexPort);

            string[] strIp = ip.Split('.');
            for (int k = 0; k < strIp.Length; k++)
            {
                bIP[k] = byte.Parse(strIp[k]);
            }


            if (UHFSetDestIp(bIP, bPort) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取自动模式下，接收数据的IP地址。(Get the IP address of the received data in automatic mode.)
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns>true:success false:fail</returns>
        public bool GetDestIP(StringBuilder ip, StringBuilder port)
        {
            byte[] sIP = new byte[4];
            byte[] sPort = new byte[2];

            if (UHFGetDestIp(sIP, sPort) == 0)
            {
                if (ip != null)
                {
                    ip.Append(sIP[0]);
                    ip.Append(".");
                    ip.Append(sIP[1]);
                    ip.Append(".");
                    ip.Append(sIP[2]);
                    ip.Append(".");
                    ip.Append(sIP[3]);
                }
                if (port != null)
                {
                    string hexPort = DataConvert.ByteArrayToHexString(sPort).Replace(" ", "");
                    int iPort = Convert.ToInt32(hexPort, 16);
                    port.Append(iPort);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region  WorkMode   Of UR4
        /// <summary>
        ///Set WorkMode
        /// </summary>
        /// <param name="mode">0:command mode   1:auto mode   2:Trigger mode</param>
        /// <returns></returns>
        public bool SetWorkMode(byte mode)
        {
            if (UHFSetWorkMode(mode) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Get WorkMode
        /// </summary>
        /// <param name="mode">0:command mode   1:auto mode   2:Trigger mode</param>
        /// <returns></returns>
        public bool GetWorkMode(byte[] mode)
        {
            if (UHFGetWorkMode(mode) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 设置触发工作模式参数(Set the trigger mode parameters.)
        /// </summary>
        /// <param name="ioControl">GPI：0x00 input1；0x01 input2</param>
        /// <param name="workTime">Trigger working time,  unit:10ms</param>
        /// <param name="intervalTime">Trigger interval time unit:10ms</param>
        /// <param name="mode">0x00:serial port，0x01:UDP</param>
        /// <returns></returns>
        public bool SetWorkModePara(byte ioControl, int workTime, int intervalTime, byte mode)
        {
            byte[] para = new byte[6];
            para[0] = ioControl;
            para[1] = (byte)((workTime >> 8) & 0xFF);
            para[2] = (byte)(workTime & 0xFF);
            para[3] = (byte)((intervalTime >> 8) & 0xFF);
            para[4] = (byte)(intervalTime & 0xFF);
            para[5] = mode;
            int result = UHFSetWorkModePara(para);
            return result == 0;
        }
        public bool GetWorkModePara(ref byte ioControl, ref int workTime, ref int intervalTime, ref byte mode)
        {
            byte[] para = new byte[6];
            if (UHFGetWorkModePara(para) == 0)
            {
                ioControl = para[0];
                workTime = (para[1] << 8) | (para[2] & 0xFF);
                intervalTime = (para[3] << 8) | (para[4] & 0xFF);
                mode = para[5];
                return true;
            }
            return false;
        }


        #endregion

        #region GPIO   Of UR4
        /// <summary>
        /// Get GPI state  On UR4
        /// </summary>
        /// <param name="input1">
        ///       input1[0]:     0:low level   1：high level
        ///       input=2[1]:    0:low level   1：high level
        /// 
        /// </param>
        /// <returns></returns>
        public bool getIOControl(byte[] input)
        {
            byte[] tempVal = new byte[5];
            if (UHFGetIOControl(tempVal) == 0)
            {
                if (input != null && input.Length >= 2)
                {
                    input[0] = tempVal[0];
                    input[1] = tempVal[1];
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Set GPO On UR4
        /// </summary>
        /// <param name="ouput1">0:low level   1：high level</param>
        /// <param name="ouput2">0:low level   1：high level</param>
        /// <param name="outStatus">relay 0：disconnect    1：close</param>
        /// <returns></returns>
        public bool setIOControl(byte ouput1, byte ouput2, byte outStatus)
        {
            if (ouput1 != 0 && ouput1 != 1)
            {
                return false;
            }
            if (ouput2 != 0 && ouput2 != 1)
            {
                return false;
            }
            if (outStatus != 0 && outStatus != 1)
            {
                return false;
            }
            if (UHFSetIOControl(ouput1, ouput2, outStatus) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

  
        /// <summary>
        /// Get GPI state  On UR1A
        /// </summary>
        /// <param name="statusData"></param>
        /// <returns></returns>
        public bool GetInputStatus(byte[] statusData)
        {
            byte[] temp = new byte[10];
            int[] dataLen = new int[1];
            int result = UHFGetIOStatus(temp,  dataLen);
            if (result == 0)
            {
                statusData[0] = temp[0];// temp[1];
                statusData[1] = temp[1];// temp[3];
                return true;
            }
            return false;
        }
        /// <summary>
        /// Set GPO On UR1A
        /// </summary>
        /// <param name="outData">outData[3]: output1  outData[4]:output2</param>
        /// <returns></returns>
        public bool SetOutput(byte[] outData)
        {
            /**
                byte[] outData = new byte[5];
                outData[3] = output1;
                outData[4] = output2;
             */
            int result = UHFSetOutputIO(outData, (byte)outData.Length);
            return result == 0;
        }

        public bool GetInputStatus_DEV_WYD(byte[] statusData)
        {
            byte[] temp = new byte[10];
            int[] dataLen = new int[1];
            int result = UHFGetIOStatus(temp, dataLen);
            if (result == 0)
            {
                statusData[0] = temp[0];// temp[1];
                statusData[1] = temp[1];// temp[3];
                if (statusData.Length > 2 && dataLen[0] > 2)
                    statusData[2] = temp[2];//  
                if (statusData.Length > 3 && dataLen[0] > 3)
                    statusData[3] = temp[3];// 

                return true;
            }
            return false;
        }
        #endregion

        #region 多设备连接

        public const int  DEVICE_ALL = 0;
        public const int  DEVICE_CONNECTED = 1;
        public const int  DEVICE_DISCONNECT = 2;
        //获取当前连接的ip信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0:all  1:connected  2:disconnect</param>
        /// <returns></returns>
        public List<DeviceInfo> LinkGetDeviceInfo(int type)
        {
            try
            {
                byte[] info = new byte[1024*100];
                int resultLen = LinkGetInfo(info, info.Length);
                if (resultLen > 0)
                {
                    string jsonstring = System.Text.ASCIIEncoding.ASCII.GetString(info, 0, resultLen).Replace("\0", "");
                    List<DeviceInfo> list = new List<DeviceInfo>();
                    object obj = JsonConvert.DeserializeObject(jsonstring);
                    JArray arr = JArray.FromObject(obj); //(JArray)JsonConvert.DeserializeObject(jsonstring);


                    for (int k = 0; k < arr.Count; k++)
                    {
                        object _id = arr[k]["id"];
                        object _type = arr[k]["type"];
                        object _ip = arr[k]["ip"];
                        object _port = arr[k]["port"];
                        object _connected = arr[k]["connected"];
                        
                        if (type == 1)
                        {
                            if (_connected.ToString().ToLower() != "true")
                            {
                                continue;
                            }
                        }
                        else if (type == 2)
                        {
                            if (_connected.ToString().ToLower() == "true")
                            {
                                continue;
                            }
                        }
                        else if (type != 0)
                        {
                            continue;
                        }
                         


                        DeviceInfo deviceInfo = new DeviceInfo();
                        if (_id != null)
                        {
                            deviceInfo.Id = int.Parse(_id.ToString());
                        }
                        if (_type != null)
                        {
                            deviceInfo.Type = _type.ToString();
                        }
                        if (_port != null)
                        {
                            deviceInfo.Port = int.Parse(_port.ToString());
                        }
                        if (_ip != null)
                        {
                            deviceInfo.Ip = _ip.ToString();
                        }
                        list.Add(deviceInfo);

                    }
                    return list;

                }
                return null;
            }
            catch (Exception ex) {
                return null;
            }
        }
        //选择要操作的id，根据LinkGetInfo获取id信息
        public bool LinkSelectDevice(int id)
        {
            return LinkSelect(id) == 0;
        }
        //获取当前可以操作的id
        public int LinkGetSelectedDevice()
        {
            return LinkGetSelected();
        }

        public void LinkDisConnectAllDevice()
        {
            LinkCloseAll();
        }

        public bool InventoryById(int id)
        {
           return UHFInventoryById(id)==0;
        }
        public bool StopById(int id)        
        {
            return UHFStopById(id)==0;
        }
        public bool InventoryById(int id, bool NeedPhase, bool NeedFrequencyPoint)
        {
            int result = -1;
            if (NeedFrequencyPoint && NeedPhase)
            {
                result = UHFPerformInventoryById(id, 6, null, 0);
            }
            else if (NeedPhase)
            {
                result = UHFPerformInventoryById(id, 4, null, 0);
            }
            else if (NeedFrequencyPoint)
            {
                result = UHFPerformInventoryById(id, 5, null, 0);
            }
            else
            {
                result = UHFInventoryById(id);
            }

            if (result == 0)
                return true;
            else
                return false;
        }
       
        #endregion

        #region UpDate


        public bool jump2Boot(byte flag)
        {
            int reuslt = UHFJump2Boot(flag);
            return reuslt == 0;
        }

        public bool startUpd()
        {
            int reuslt = UHFStartUpd();
            return reuslt == 0;
        }

        public bool updating(byte[] data, int len)
        {
            int reuslt = UHF_Upgrade(data, len);
            return reuslt == 0;
        }

        public bool stopUpdate()
        {
            int reuslt = UHFStopUpdate();
            return reuslt == 0;
        }
        #endregion

        #region Custom 温度标签
        public bool InitRegFile(byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData)
        {
            int result = UHFInitRegFile(FilterBank, FilterStartaddr, FilterLen, FilterData);
            if (result == 0)
            {
                return true;
            }
            return false;
        }
        public bool ReadTagTemperature(byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, float[] outtemp)
        {
            int result = UHFReadTagTemp(FilterBank, FilterStartaddr, FilterLen, FilterData, outtemp);
            if (result == 0)
            {
                return true;
            }
            return false;
        }

        public bool SetDebug(bool debug)
        {
            return SetLogLevel(debug ? 3 : 0)==0;
        }

        public bool SaveLog(bool debug)
        {
            return SaveLogFile(debug ? 1 : 0) == 0;
        }


        #endregion 温度标签end

        #region  Custom

        /// <summary>
        /// 支持32天线
        /// </summary>
        /// <param name="saveflag"></param>
        /// <param name="buf"></param>
        /// <returns></returns>
        public bool SetANTTo32(byte saveflag, byte[] buf)
        {
            if (UHFSetAntEx(saveflag, buf, buf.Length) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 支持32天线
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public bool GetANTTo32(byte[] buf)
        {
            int result = UHFGetAntEx(buf, buf.Length);
            if (result > 0)
            {
                buf = Utils.CopyArray(buf, 0, result);
                return true;
            }
            return false;
        }

        /// 
        /// <summary>
        /// 国标标签Lock
        /// </summary>
        /// <param name="uAccessPwd">4字节密码</param>
        /// <param name="FilterBank">启动过滤的bank号， 1：EPC, 2:TID, 3:USR</param>
        /// <param name="FilterStartaddr">启动过滤的起始地址， 单位：bit</param>
        /// <param name="FilterLen">启动过滤的长度， 单位：字节</param>
        /// <param name="FilterData">启动过滤的数据</param>
        /// <param name="memory"></param>
        /// <param name="config"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool GBTagLock(byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData, byte memory, byte config, byte action)
        {
            if (UHFGBTagLock(uAccessPwd, FilterBank, FilterStartaddr, FilterLen, FilterData, memory, config, action) == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 激活或失效EM4124标签
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="uAccessPwd">4字节密码</param>
        /// <param name="FilterBank">启动过滤的bank号， 1：EPC, 2:TID, 3:USR</param>
        /// <param name="FilterStartaddr">启动过滤的起始地址， 单位：bit</param>
        /// <param name="FilterLen">启动过滤的数据长度</param>
        /// <param name="FilterData">启动过滤的数据</param>
        /// <returns></returns>
        public bool Deactivate(int cmd, byte[] uAccessPwd, byte FilterBank, int FilterStartaddr, int FilterLen, byte[] FilterData)
        {
            if (UHFDeactivate(cmd, uAccessPwd, FilterBank, FilterStartaddr, FilterLen, FilterData) == 0)
                return true;
            else
                return false;

        }
        /// <summary>
        /// 获取缓冲区的标签数据
        /// Get tag data from buffer
        /// 
        ///  {@link #StartInventory() }启动识别标签之后，在子线程循环调用此函数不断获取缓冲区的标签信息，每次返回一张标签数据
        ///  {@link #startInventoryTag() } After tag reading has been enabled, call this function in sub threads to get data information continously, return one tag information for each time.<br>
        /// 
        /// </summary>
        /// <param name="epc"></param>
        /// <param name="tid"></param>
        /// <param name="rssi"></param>
        /// <param name="ant"></param>
        /// <returns></returns>
        public bool ReadTagFromBuffer(ref string epc, ref string tid, ref string rssi, ref string ant)
        {
            int uLen = 0;
            byte[] bufData = new byte[256];
            if (ReadTagFromBuffer(ref uLen, ref bufData))
            {

                string epc_data = string.Empty;
                string uii_data = string.Empty;//uii数据
                string tid_data = string.Empty; //tid数据
                string rssi_data = string.Empty;
                string ant_data = string.Empty;

                int uii_len = bufData[0];//uii长度
                int tid_leng = bufData[uii_len + 1];//tid数据长度
                int tid_idex = uii_len + 2;//tid起始位
                int rssi_index = 1 + uii_len + 1 + tid_leng;
                int ant_index = rssi_index + 2;

                string strData = BitConverter.ToString(bufData, 0, uLen).Replace("-", "");
                epc_data = strData.Substring(6, uii_len * 2 - 4);  //Epc
                tid_data = strData.Substring(tid_idex * 2, tid_leng * 2); //Tid
                string temp = strData.Substring(rssi_index * 2, 4);
                int rssiTemp = Convert.ToInt32(temp, 16) - 65535;
                rssi_data = ((float)rssiTemp / 10.0).ToString();// RSSI  =  (0xFED6   -65535)/10
                ant_data = Convert.ToInt32((strData.Substring(ant_index * 2, 2)), 16).ToString();

                epc = epc_data;
                tid = tid_data;
                rssi = rssi_data;
                ant = ant_data;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool WriteScreenBlockData(byte[] pwd, byte sector, short mask_addr, short mask_len, byte[] mask_data, byte type, short w_addr, short w_len, byte[] w_data)
        {

            return UHFWriteScreenBlockData(pwd, sector, mask_addr, mask_len, mask_data, type, w_addr, w_len, w_data) == 0;
        }

        public int CalibrationVoltage()
        {
            int[] value = new int[1];
            if (UHFVerifyVoltage(value) == 0)
            {
                int temp = value[0];
                return temp;
            }
            return -1;

        }
        #endregion

        #region 获取升级最大长度
        //#define TYPE_UPGREADE_DEVICE_TYPE             1
        //#define TYPE_UPGRADE_MINIMUM_DATA_SIZE        2
        //#define TYPE_UPGRADE_MAXIMUM_DATA_SIZE        3
        //#define TYPE_UPGRADE_MAXIMUM_FILE_SIZE        4 
        //info:TLV格式 
        //类型说明，以下数据均使用大端模式（高字节在前）
        //1、设备类型，2bytes 
        //2、最小数据单元（最后一包数据忽略此参数），2bytes
        //3、最大数据长度（仅限定数据内容，不包含数据包头尾），2bytyes
        //4、最大可接受的下载文件大小，单位字节，4bytes
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHFStartUpgrade(byte[] info, int[] retLen);

        public int UHFStartUpgrade()
        {
            int pkgLen = 64;
            byte[] info=new byte[1024];
            int[] retLen=new int[1];
            int result = UHFStartUpgrade(info, retLen);
            if (result != 0)
            {
                return -1;
            }
            if (retLen[0]>0)
            {
                int index = 0;
                byte[] data= Utils.CopyArray(info, retLen[0]);
                while (index < data.Length)
                {
                    int type = data[index++];
                    int len = data[index++];
                    switch (type)
                    {
                        case 1:
                            //设备类型，2bytes 
                            Utils.CopyArray(data, index, len);
                            break;
                        case 2:
                            //最小数据单元（最后一包数据忽略此参数），2bytes
                   
                            Utils.CopyArray(data, index, len);
                            break;
                        case 3:
                            //最大数据长度（仅限定数据内容，不包含数据包头尾），2bytyes
                            byte[] maxLenData = Utils.CopyArray(data, index, len);
                            pkgLen = ((maxLenData[0] & 0xFF) << 8) | (maxLenData[1] & 0xFF);
                            break;
                        case 4:
                            //最大可接受的下载文件大小，单位字节，4bytes
                            Utils.CopyArray(data, index, len);
                           // pkgLen = ((maxLenData[0]&0xFF) << 8) | (maxLenData[1] & 0xFF);
                            break;
                        default:
                            Utils.CopyArray(data, index, len);
                            break;

                    }
                    index += len;
                }
                
            }
    
            return pkgLen;
        }
        #endregion


    }




}






 