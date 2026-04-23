#ifndef _UHFAPI_H_
#define _UHFAPI_H_

#define UHFAPI_API  __declspec(dllexport)

#define RET_ERR_OK                    0
#define RET_ERR_FAILURE               (-1)
#define RET_ERR_INVALID_PARAM         (-2)
#define RET_ERR_NO_DATA               (-3)
#define RET_ERR_TIMEOUT               (-4)
/** @brief Static/ethernet dhcp static.

	@ingroup UHFAPI
*/
#define DHCP_MODE_STATIC       1
/** @brief Static/ethernet dhcp static.

	@ingroup UHFAPI
*/
#define DHCP_MODE_DYNAMIC      2

typedef enum{CELL_INVALID=0, CELL_CONNECT_ID=1, CELL_CONNECT_IP, CELL_UHF_PC, CELL_UHF_RSSI, CELL_UHF_ANTENNA, CELL_UHF_EPC, CELL_UHF_TID, CELL_UHF_USER,CELL_UHF_RESERVE,CELL_BARCODE, CELL_UHF_SENSOR, CELL_UHF_INPUT, CELL_KEY_CODE, CELL_UHF_PHASE, CELL_UHF_FREQUENCY_POINT} CELL_DATA_TYPE;
typedef enum {ENUM_CHAR_CODE_ANSI=0, ENUM_CHAR_CODE_UTF8}ENUM_CHAR_CODE_TYPE;
typedef struct _ST_UHF_DATA_CELL
{
	unsigned char type;
	unsigned short len;
	const unsigned char* pdata;
	struct _ST_UHF_DATA_CELL* next;
} ST_UHF_DATA_CELL;

typedef  void (*OnDataReceived)(const unsigned char *recvData, unsigned short recvLen);
typedef  void (*OnBytesReceived)(int id, const unsigned char *recvData, unsigned short recvLen);
typedef  void (*OnAtCmdReceived)(const char *recvData);
typedef void (*OnEventStopInventory)(int id);

extern "C"
{
 UHFAPI_API void setOnDataReceived(OnDataReceived onDataRecved);
 UHFAPI_API void setOnBytesReceived(OnBytesReceived onBytesRecved);
 UHFAPI_API void setOnEventStopInventory(OnEventStopInventory onEventStopInventory);

 UHFAPI_API int LinkGetInfo(char *info, int len);

 UHFAPI_API int LinkSelect(int id);

 UHFAPI_API void LinkCloseAll();

 UHFAPI_API unsigned long UHF_GetIdleMs();

 UHFAPI_API unsigned long UHF_GetIdleMsById(int id);

 UHFAPI_API  void setPassThrough(bool enable);

 UHFAPI_API int UHFInventoryById(int id);
 UHFAPI_API int UHFPerformInventoryById(int id, unsigned short mode, const unsigned char *param, unsigned short paramlen);

 UHFAPI_API int UHFStopById(int id);
//return -- id of using link, 
 UHFAPI_API int LinkGetSelected();

  UHFAPI_API int TCPConnect(const char * hostaddr,int hostport);
  UHFAPI_API void TCPDisconnect();

  UHFAPI_API int BindUDP(int bindport);
  UHFAPI_API void UnbindUDP();


 UHFAPI_API int UsbOpen();
 UHFAPI_API void UsbClose();
 UHFAPI_API int UsbReadDeviceInfo(char* info, int len);
/**********************************************************************************************************
* 功能：添加断开事件回调函数
* callback[in]：回调方法
* type[out]：断开类型，0-串口, 1-网络端口, 2-USB连接
*********************************************************************************************************/
 UHFAPI_API int SetDisconnectCallback(void (*callback)(int type));
/**********************************************************************************************************
* 功能：打开串口
* 输入：port--串口号
*********************************************************************************************************/

  UHFAPI_API int ComOpen(int port);
  UHFAPI_API int ComOpenWithBaud(int port, int baudrate);

/**********************************************************************************************************
* 功能：关闭串口
*********************************************************************************************************/
 UHFAPI_API void ClosePort();

 UHFAPI_API int SendBytes(const unsigned char *pszData, int Len);

 UHFAPI_API int RecvBytes(unsigned char *pszData, int Len);


 UHFAPI_API int UHFJump2Boot(unsigned char flag);

 UHFAPI_API int UHFStartUpd();

 UHFAPI_API int UHFStartUpgrade(unsigned char *buf, int *retLen);

 UHFAPI_API int UHFUpdating(unsigned char *buf);

 UHFAPI_API int UHF_Upgrade(const unsigned char *buf, unsigned short length);

 UHFAPI_API int UHFStopUpdate();


/**********************************************************************************************************
* 功能：获取读写器软件版本号
* 输出：version[0]--版本号长度 ,  version[1--x]--版本号
*********************************************************************************************************/
 UHFAPI_API int UHFGetReaderVersion(unsigned char *version);


/**********************************************************************************************************
* 功能：获取硬件版本号
* 输出：version[0]--版本号长度 ,  version[1--x]--版本号
*********************************************************************************************************/
 UHFAPI_API int UHFGetHardwareVersion(unsigned char *version);


/**********************************************************************************************************
* 功能：获取软件版本号
* 输出：version[0]--版本号长度 ,  version[1--x]--版本号
*********************************************************************************************************/
 UHFAPI_API int UHFGetSoftwareVersion(unsigned char *version);

 

/**********************************************************************************************************
* 功能：获取ID号
* 输出：id--整型ID号
*********************************************************************************************************/
 UHFAPI_API int UHFGetDeviceID(unsigned int *id);

/**********************************************************************************************************
* 功能：获取ID号
* 输出：id[0]--长度,id[1~N]--ID号
*********************************************************************************************************/
 UHFAPI_API int UHFGetDeviceIdEx(unsigned char *id);

/**********************************************************************************************************
* 功能：设置功率
* 输入：saveflag  -- 1:保存设置   0：不保存
* 输入：uPower -- 功率（DB）
*********************************************************************************************************/
 UHFAPI_API int UHFSetPower ( unsigned char saveflag,unsigned char uPower);

/**********************************************************************************************************
* 功能：设置天线功率
* 输入：saveflag  -- 1:保存设置   0：不保存
* 输入：num -- 天线编号(1~N)
		read_power -- 接收功率（DB）
		write_power -- 发送功率（DB）
*********************************************************************************************************/
 UHFAPI_API int UHFSetAntennaPower ( unsigned char save, unsigned char num, unsigned char read_power, unsigned char write_power);
/**********************************************************************************************************
* 功能：设置多天线功率
* 输入：save  -- 1:掉电保存   0：掉电不保存
* 输入：num  -- 设置的天线个数
* 输入：param -- 天线编号(1字节)+功率（1字节）+天线编号(1字节)+功率（1字节）+...
* 天线编号：1~N，功率范围：1DB~30DB
*********************************************************************************************************/
 UHFAPI_API int UHFSetMultiAntenna ( unsigned char save, unsigned char num, unsigned char *param);

/**********************************************************************************************************
* 功能：获取功率
* 输出：uPower -- 功率（DB）
*********************************************************************************************************/
 UHFAPI_API int UHFGetPower (unsigned char *uPower);

/**********************************************************************************************************
* 功能：获取天线功率
* 输出：ppower -- 天线功率,格式为（天线编号+读功率+写功率+天线编号+读功率+写功率+.......+天线编号+读功率+写功率）
		nBytesReturned -- ppower数据长度 
*********************************************************************************************************/
 UHFAPI_API int UHFGetAntennaPower (unsigned char *ppower, unsigned short *nBytesReturned);

/**********************************************************************************************************
* 功能：获取天线回波损耗
* 输出：rLoss -- 天线号(1 byte)+回波损耗(1 byte)+天线号+回波损耗+....。
	回波损耗解释：0表示对应端口没有使能（单端口模块为0则表示没有接天线），单位为dB，值越大说明反射越小。
	模块有N个端口则会返回N组数据，每组两个字节，总长度为：2*N
* 输出：nBytesReturned -- rLoss数据字节长度 
*********************************************************************************************************/
 UHFAPI_API int UHFGetReturnLoss(unsigned char *rLoss, unsigned short *nBytesReturned);
/**********************************************************************************************************
* 功能：设置跳频
* 输入：nums -- 跳频个数
* 输入：Freqbuf--频点数组（整型） ，如920125，921250.....
*********************************************************************************************************/
 UHFAPI_API int UHFSetJumpFrequency( unsigned char nums,unsigned int *Freqbuf);

/**********************************************************************************************************
* 功能：获取跳频
* 输出：Freqbuf[0]--频点个数， Freqbuf[1]..[x]--频点数组（整型）
*********************************************************************************************************/
 UHFAPI_API int UHFGetJumpFrequency( unsigned int *Freqbuf);

/**********************************************************************************************************
* 功能：设置Gen2参数
* 输入：
**********************************************************************************************************/
 UHFAPI_API int UHFSetGen2 (unsigned char Target,unsigned char Action, unsigned char T,unsigned char Q,
								unsigned char StartQ,unsigned char MinQ,
								unsigned char MaxQ,unsigned char D,unsigned char C,unsigned char P,
								unsigned char Sel,unsigned char Session,unsigned char G,unsigned char LF);

/**********************************************************************************************************
* 功能：获取Gen2参数
* 输入：
*********************************************************************************************************/
 UHFAPI_API int UHFGetGen2 (unsigned char *Target, unsigned char *Action,unsigned char *T,unsigned char *Q,
					unsigned char *StartQ,unsigned char *MinQ,
					unsigned char *MaxQ,unsigned char *D, unsigned char *Coding,unsigned char *P,
					unsigned char *Sel,unsigned char *Session,unsigned char *G,unsigned char *LF);

/**********************************************************************************************************
* 功能：设置CW
* 输入：flag -- 1:开CW，  0：关CW
*********************************************************************************************************/
 UHFAPI_API int UHFSetCW( unsigned char flag);


/**********************************************************************************************************
* 功能：获取CW
* 输出：flag -- 1:开CW，  0：关CW
*********************************************************************************************************/
 UHFAPI_API int UHFGetCW( unsigned char *flag);


/**********************************************************************************************************
* 功能：天线设置
* 输入：saveflag -- 1:掉电保存，  0：不保存
* 输入：buf--2bytes, 共16bits, 每bit 置1选择对应天线
*********************************************************************************************************/
 UHFAPI_API int UHFSetANT( unsigned char saveflag,unsigned char *buf);
 UHFAPI_API int UHFSetAntEx( unsigned char saveflag,unsigned char *status, unsigned short slen);

/**********************************************************************************************************
* 功能：获取天线设置
* 输出：buf--2bytes, 共16bits,
*********************************************************************************************************/
 UHFAPI_API int UHFGetANT( unsigned char *buf);
/**********************************************************************************************************
* 功能：获取天线设置
* 輸入：slen-status數組長度
* 输出：buf ,
* 返回：status状态长度，单位字节
*********************************************************************************************************/
 UHFAPI_API int UHFGetAntEx(unsigned char *status, unsigned short slen);
/**********************************************************************************************************
* 功能：区域设置
* 输入：saveflag -- 1:掉电保存，  0：不保存
* 输入：region -- 0x01(China1),0x02(China2),0x04(Europe),0x08(USA),0x16(Korea),0x32(Japan)
*********************************************************************************************************/
 UHFAPI_API int UHFSetRegion( unsigned char saveflag, unsigned char region);

/**********************************************************************************************************
* 功能：获取区域设置
* 输出：region -- 0x01(China1),0x02(China2),0x04(Europe),0x08(USA),0x16(Korea),0x32(Japan)
*********************************************************************************************************/
 UHFAPI_API int UHFGetRegion( unsigned char *region);

/**********************************************************************************************************
* 功能：获取当前温度
* 输出：temperature -- 整型
*********************************************************************************************************/
 UHFAPI_API int UHFGetTemperature( unsigned int *temperature);


/**********************************************************************************************************
* 功能：设置温度保护
* 输入：flag -- 1:温度保护， 0：无温度保护
*********************************************************************************************************/
 UHFAPI_API int UHFSetTemperatureProtect( unsigned char flag);


/**********************************************************************************************************
* 功能：获取温度保护
* 输出：flag -- 1:温度保护， 0：无温度保护
*********************************************************************************************************/
 UHFAPI_API int UHFGetTemperatureProtect( unsigned char *flag);


/**********************************************************************************************************
* 功能：设置天线工作时间
* 输入：antnum -- 天线号
* 输入：saveflag -- 1:掉电保存， 0：不保存
* 输入：WorkTime -- 工作时间 ，单位ms, 范围 10-65535ms
*********************************************************************************************************/
 UHFAPI_API int UHFSetANTWorkTime(unsigned char antnum,unsigned char saveflag,unsigned int WorkTime);


/**********************************************************************************************************
* 功能：获取天线工作时间
* 输入：antnum -- 天线号
* 输出：WorkTime -- 工作时间 ，单位ms, 范围 10-65535ms
*********************************************************************************************************/
 UHFAPI_API int UHFGetANTWorkTime(unsigned char antnum,unsigned int *WorkTime);


/**********************************************************************************************************
* 功能：设置链路组合
* 输入：saveflag -- 1:掉电保存， 0：不保存
* 输入：mode --  0:DSB_ASK/FM0/40KHZ , 1:PR_ASK/Miller4/250KHZ , 2:PR_ASK/Miller4/300KHZ, 3:DSB_ASK/FM0/400KHZ
*********************************************************************************************************/
 UHFAPI_API int UHFSetRFLink ( unsigned char saveflag,unsigned char mode);


/**********************************************************************************************************
* 功能：获取链路组合
* 输出：mode --  0:DSB_ASK/FM0/40KHZ , 1:PR_ASK/Miller4/250KHZ , 2:PR_ASK/Miller4/300KHZ, 3:DSB_ASK/FM0/400KHZ
*********************************************************************************************************/
 UHFAPI_API int UHFGetRFLink (unsigned char* uMode);


/**********************************************************************************************************
* 功能：设置FastID功能
* 输入：flag -- 1:开启， 0：关闭
*********************************************************************************************************/
 UHFAPI_API int UHFSetFastID(unsigned char flag);


/**********************************************************************************************************
* 功能：获取FastID功能
* 输出：flag -- 1:开启， 0：关闭
*********************************************************************************************************/
 UHFAPI_API int UHFGetFastID(unsigned char *flag);

/**********************************************************************************************************
* 功能：设置FastID功能
* 输入：saveFlag -- 1:掉电保存， 0：掉电不保存
* 输入：enable -- 1:开启， 0：关闭
*********************************************************************************************************/
 UHFAPI_API int UHFSetFastInventory(unsigned char saveFlag, unsigned char enable);

/**********************************************************************************************************
* 功能：获取FastID功能
* 输出：flag -- 1:开启， 0：关闭
*********************************************************************************************************/
 UHFAPI_API int UHFGetFastInventory(unsigned char *flag);

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
 UHFAPI_API int UHFSetFastInventoryEX(unsigned char saveFlag, unsigned char cr, unsigned char code, unsigned char protection, unsigned char id);
 UHFAPI_API int UHFGetFastInventoryEX(unsigned char *cr, unsigned char *code, unsigned char *protection, unsigned char *id);

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
 * @param sr:the data of the mask,Occupy N bytes,N=(dml+7)/8
 *            @arg 0:disable short mode
 *            @arg 1:enable short range
 * @retval status,0:execution successful,1:wrote failed, 34:found no tag, others:execution failed,
 */
 UHFAPI_API int UHFSetProtectionAndShortRange(unsigned char *ap, unsigned char mmb, unsigned short msa, unsigned short mdl, unsigned char *mdata, unsigned char pm, unsigned char sr);

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
 * @param dl:the length of read data,in words
 * @param wdata:the point of data,Occupy N bytes,N=dl*2

 * @retval status,0:execution successful,1:wrote failed, 34:found no tag, others:execution failed,
 */
 UHFAPI_API int UHFMarginRead(unsigned char *ap, unsigned char mmb, unsigned short msa, unsigned short mdl, unsigned char *mdata, \
	unsigned char mb, unsigned short sa, unsigned short dl, unsigned char *wdata);

/**********************************************************************************************************
* 功能：设置Tagfocus功能
* 输入：flag -- 1:开启， 0：关闭
*********************************************************************************************************/
 UHFAPI_API int UHFSetTagfocus(unsigned char flag);


/**********************************************************************************************************
* 功能：获取Tagfocus功能
* 输出：flag -- 1:开启， 0：关闭
*********************************************************************************************************/
 UHFAPI_API int UHFGetTagfocus(unsigned char *flag);


/**********************************************************************************************************
* 功能：设置软件复位
*********************************************************************************************************/
 UHFAPI_API int UHFSetSoftReset(void);


/**********************************************************************************************************
* 功能：设置Dual和Singel模式
* 输入：saveflag -- 1:掉电保存， 0：不保存
* 输入：mode -- 1:设置Singel模式， 0：设置Dual模式
*********************************************************************************************************/
 UHFAPI_API int UHFSetDualSingelMode(unsigned char saveflag,unsigned char mode);


/**********************************************************************************************************
* 功能：获取Dual和Singel模式
* 输出：mode -- 1:设置Singel模式， 0：设置Dual模式
*********************************************************************************************************/
 UHFAPI_API int UHFGetDualSingelMode(unsigned char *mode);


/**********************************************************************************************************
* 功能：设置寻标签过滤设置
* 输入：saveflag -- 1:掉电保存， 0：不保存
* 输入：bank --  0x01:EPC , 0x02:TID, 0x03:USR
* 输入：startaddr 起始地址，单位：bit
* 输入：datalen 数据长度， 单位:bit
* 输入：databuf 数据
*********************************************************************************************************/
 UHFAPI_API int UHFSetFilter(unsigned char saveflag,unsigned char bank,unsigned int startaddr,unsigned int datalen,unsigned char *databuf);



/**********************************************************************************************************
* 功能：设置EPC和TID模式
* 输入：saveflag -- 1:掉电保存， 0：不保存
* 输入：mode -- 1：开启EPC和TID， 0:关闭
*********************************************************************************************************/
 UHFAPI_API int UHFSetEPCTIDMode(unsigned char saveflag,unsigned char mode);


/**********************************************************************************************************
* 功能：获取EPC和TID模式
* 输出：mode -- 1：开启EPC和TID， 0:关闭
*********************************************************************************************************/
 UHFAPI_API int UHFGetEPCTIDMode(unsigned char  *mode);

/**********************************************************************************************************
* 功能：设置EPC TID USER模式

* 输入：saveflag -- 1:掉电保存， 0：不保存

* 输入：Memory 0x00，表示关闭； 0x01，表示开启EPC+TID模式（默认 地址为 0x00, 长 度 为 6 个 字 ） ； 0x02, 表 示 开 启EPC+TID+USER模式

* 输入：address 为USER区的起始地址（单位为字）
* 输入：为USER区的长度（单位为字）
*********************************************************************************************************/
 UHFAPI_API int UHFSetEPCTIDUSERMode(unsigned char saveflag,unsigned char memory,unsigned char address,unsigned char lenth);


/**********************************************************************************************************
* 功能：获取EPC TID USER 模式

* 输入：rev1 :保留数据，传入0
* 输入：rev2 :保留数据，传入0

* 输出：mode[0]，Memory 0x00，表示关闭； 0x01，表示开启EPC+TID模式（默认 地址为 0x00, 长 度 为 6 个 字 ） ； 0x02, 表 示 开 启EPC+TID+USER模式

* 输出：mode[1]address 为USER区的起始地址（单位为字）
* 输出：mode[2]为USER区的长度（单位为字）
*
* 返回值：3：正确，其它：错误
*********************************************************************************************************/
 UHFAPI_API int UHFGetEPCTIDUSERMode(unsigned char  rev1,unsigned char  rev2, unsigned char * mode);

/**********************************************************************************************************
* 功能：恢复出厂设置
*********************************************************************************************************/
 UHFAPI_API int UHFSetDefaultMode();



/**********************************************************************************************************
* 功能：单次盘存标签
* 输出：rLrn -- UII长度
* 输出：rData -- UII数据
*********************************************************************************************************/
 UHFAPI_API int UHFInventorySingle (unsigned char* rLrn, unsigned char* rData );


/**********************************************************************************************************
* 功能：连续盘存标签
*********************************************************************************************************/
 UHFAPI_API int UHFInventory();


/**********************************************************************************************************
* 功能：停止盘存标签
*********************************************************************************************************/
 UHFAPI_API int UHFStopGet();


/**********************************************************************************************************
* 功能：获取连续盘存标签数据
* 输出：uLenUii -- UII长度
* 输出：uUii -- UII数据
*********************************************************************************************************/
 UHFAPI_API int UHF_GetReceived_EX(int* uLenUii, unsigned char* uUii);


/**********************************************************************************************************
* 功能：读标签数据区
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：bit
* 输入：FilterLen -- 启动过滤的长度， 单位：bit
* 输入：FilterData -- 启动过滤的数据
* 输入：uBank -- 读取数据的bank
* 输入：uPtr --  读取数据的起始地址， 单位：字
* 输入：uCnt --  读取数据的长度， 单位：字
* 输出：uReadDatabuf --  读取到的数据
* 输出：uReadDataLen --  读取到的数据长度
*********************************************************************************************************/
 UHFAPI_API int UHFReadData (unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char uBank,unsigned int uPtr, unsigned int uCnt, unsigned char* uReadDatabuf, unsigned int* uReadDataLen);




/**********************************************************************************************************
* 功能：写标签数据区
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：bit
* 输入：FilterLen -- 启动过滤的长度， 单位：bit
* 输入：FilterData -- 启动过滤的数据  单位：字节
* 输入：uBank -- 读取数据的bank
* 输入：uPtr --  读取数据的起始地址， 单位：字
* 输入：uCnt --  读取数据的长度， 单位：字
* 输入：uWriteDatabuf --  写入的数据
*********************************************************************************************************/
 UHFAPI_API int UHFWriteData (unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char uBank,unsigned int uPtr, unsigned char uCnt,unsigned char *uWriteDatabuf);



/**********************************************************************************************************
* 功能：LOCK标签
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
* 输入：FilterLen -- 启动过滤的长度， 单位：字节
* 输入：FilterData -- 启动过滤的数据
* 输入：lockbuf --  3字节，第0-9位为Action位， 第10-19位为Mask位
*********************************************************************************************************/
 UHFAPI_API bool UHFLockTag(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char *lockbuf);


/**********************************************************************************************************
* 功能：KILL标签
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
* 输入：FilterLen -- 启动过滤的长度， 单位：字节
* 输入：FilterData -- 启动过滤的数据
*********************************************************************************************************/
 UHFAPI_API int UHFKillTag(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData);





/**********************************************************************************************************
* 功能：BlockWrite 特定长度的数据到标签的特定地址
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
* 输入：FilterLen -- 启动过滤的长度， 单位：字节
* 输入：FilterData -- 启动过滤的数据
* 输入：uBank -- 块号  1：EPC, 2:TID, 3:USR
* 输入：uPtr --  读取数据的起始地址， 单位：字
* 输入：uCnt --  读取数据的长度， 单位：字
* 输入：uWriteDatabuf --  写入的数据
*********************************************************************************************************/
 UHFAPI_API int UHFBlockWriteData (unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char uBank,unsigned int uPtr, unsigned int uCnt,unsigned char *uWriteDatabuf);



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
 UHFAPI_API int UHFBlockEraseData (unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char uBank,unsigned int uPtr, unsigned char uCnt);



/**********************************************************************************************************
* 功能：设置QT命令参数
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
* 输入：FilterLen -- 启动过滤的长度， 单位：字节
* 输入：FilterData -- 启动过滤的数据
* 输入：QTData --  bit0：（0：表示无近距离控制 ， 1：表示启用近距离控制）  bit1：(0:表示启用private Memory map, 1：启用public memory map)
*********************************************************************************************************/
 UHFAPI_API int UHFSetQT (unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char QTData);



/**********************************************************************************************************
* 功能：获取QT命令参数
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
* 输入：FilterLen -- 启动过滤的长度， 单位：字节
* 输入：FilterData -- 启动过滤的数据
* 输出：QTData --  bit0：（0：表示无近距离控制 ， 1：表示启用近距离控制）  bit1：(0:表示启用private Memory map, 1：启用public memory map)
*********************************************************************************************************/
 UHFAPI_API int UHFGetQT (unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char *QTData);




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
 UHFAPI_API int UHFReadQT(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									 unsigned char QTData,unsigned char uBank,unsigned int uPtr, unsigned char uCnt, unsigned  char *uReadDatabuf,unsigned char *uReadDataLen);



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
 UHFAPI_API int UHFWriteQT(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									 unsigned char QTData,unsigned char uBank,unsigned int uPtr, unsigned char uCnt,unsigned char *uWriteDatabuf);




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
 UHFAPI_API int UHFBlockPermalock(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									 unsigned char ReadLock,unsigned char uBank,unsigned int uPtr, unsigned char uRange,unsigned char *uMaskbuf);

/*
开始读取标签温度
return: 0--success, -1--unknow error, others--error code
mask_bank：掩码的数据区(0x00 为 Reserve 0x01 为 EPC， 0x02 表示 TID， 0x03 表示USR)。
mask_addr：为掩码的地址(bit为单位)，高字节在前。
mask_len：为掩码的长度(bit为单位)，高字节在前。
mask_data：为掩码数据，mask_len为0时，这里没有数据
min_temp:minum of limited temperature
max_temp:maxum of limited temperature
work_delay: start logging after delayed time
work_interval:interval of working
*/
 UHFAPI_API int UHFStartLogging (unsigned char mask_bank, unsigned short mask_addr, unsigned short mask_len, unsigned char *mask_data, 
	float min_temp, float max_temp, unsigned short work_delay, unsigned short work_interval);

/*
读取标签电压
return:0-成功，其他失败
mask_bank：掩码的数据区(0x00 为 Reserve 0x01 为 EPC， 0x02 表示 TID， 0x03 表示USR)。
mask_addr：为掩码的地址(bit为单位)，高字节在前。
mask_len：为掩码的长度(bit为单位)，高字节在前。
mask_data：为掩码数据，mask_len为0时，这里没有数据
password: password,default 0
voltage[out]：返回的标签电压值
*/
 UHFAPI_API int UHFReadTagVoltage (unsigned char mask_bank, unsigned short mask_addr, unsigned short mask_len, unsigned char *mask_data, float *voltage);

/*
读取多个定时测温温度值
return:0-成功，其他失败
mask_bank：掩码的数据区(0x00 为 Reserve 0x01 为 EPC， 0x02 表示 TID， 0x03 表示USR)。
mask_addr：为掩码的地址(bit为单位)，高字节在前。
mask_len：为掩码的长度(bit为单位)，高字节在前。
mask_data：为掩码数据，mask_len为0时，这里没有数据
password: password,default 0
totalNum[out]：温度记录总数
returnNum[out]：当前返回的温度个数
temp[out]：获取的温度数组
*/
 UHFAPI_API int UHFReadMultiTemp (unsigned char mask_bank, unsigned short mask_addr, unsigned short mask_len, unsigned char *mask_data, unsigned short t_start, 
	unsigned char t_num, unsigned short *totalNum, unsigned char *returnNum, float *temp);

/*
停止读取标签温度
return: 0--success, -1--unknow error, others--error code
mask_bank：掩码的数据区(0x00 为 Reserve 0x01 为 EPC， 0x02 表示 TID， 0x03 表示USR)。
mask_addr：为掩码的地址(bit为单位)，高字节在前。
mask_len：为掩码的长度(bit为单位)，高字节在前。
mask_data：为掩码数据，mask_len为0时，这里没有数据
password: password,default 0
*/
 UHFAPI_API int UHFStopLogging (unsigned char mask_bank, unsigned short mask_addr, unsigned short mask_len, unsigned char *mask_data, 
	unsigned long password);

/*
读取标签温度
return: 0--success, -1--unknow error, others--error code
mask_bank：掩码的数据区(0x00 为 Reserve 0x01 为 EPC， 0x02 表示 TID， 0x03 表示USR)。
mask_addr：为掩码的地址(bit为单位)，高字节在前。
mask_len：为掩码的长度(bit为单位)，高字节在前。
mask_data：为掩码数据，mask_len为0时，这里没有数据
password: password,default 0
*/
 UHFAPI_API int UHFCheckOpMode (unsigned char mask_bank, unsigned short mask_addr, unsigned short mask_len, unsigned char *mask_data);

/**********************************************************************************************************
* 功能：激活或失效EM4124标签
* 输入：cmd --整形
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
* 输入：FilterData -- 启动过滤的数据

*********************************************************************************************************/
 UHFAPI_API int UHFDeactivate (unsigned int cmd,unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr,unsigned int FilterLen,  unsigned char *FilterData);



 UHFAPI_API int UHFDwell (unsigned int dwell, unsigned int count);

/*
 * 函数功能：  设置本机 IP 和端口号
 * 输入参数：  ipbuf： IP， 
			   postbuf：端口号
			   mask:掩码，4字节
			   gate：网关，4字节

 * 返回值：   0:成功    其它：失败
 */
 UHFAPI_API int UHFSetIp (unsigned char* ipbuf, unsigned char *postbuf, unsigned char *mask, unsigned char *gate);



/*
 * 函数功能：  获取本机 IP 和端口号
 * 输出参数：  ipbuf + postbuf， IP+端口号
			   mask:掩码，4字节
			   gate:网关，4字节
 * 返回值：   0:成功    其它：失败
 */
 UHFAPI_API int UHFGetIp (unsigned char* ipbuf, unsigned char *postbuf, unsigned char *mask, unsigned char *gate);

/*
 * 函数功能：  设置本机 IP 和端口号
 * 输出参数：  ipbuf + postbuf， IP+端口号
			   mask:掩码，4字节
			   gate:网关，4字节
			   dhcp:DHCP_MODE_STATIC(1) or DHCP_MODE_DYNAMIC(2)
 * 返回值：   0:成功    其它：失败
 */
 UHFAPI_API int SetEthernetHost(unsigned char* ipbuf, unsigned char *postbuf, unsigned char *mask, unsigned char *gate, unsigned char dhcp);

/*
 * 函数功能：  获取本机 IP 和端口号
 * 输出参数：  ipbuf + postbuf， IP+端口号
			   mask:掩码，4字节
			   gate:网关，4字节
			   dhcp:DHCP_MODE_STATIC(1) or DHCP_MODE_DYNAMIC(2)
 * 返回值：   0:成功    其它：失败
 */
 UHFAPI_API int GetEthernetHost (unsigned char* ipbuf, unsigned char *postbuf, unsigned char *mask, unsigned char *gate, unsigned char *dhcp);
/*
 * 函数功能：  设置目标 IP 和端口号
 * 输入参数：  ipbuf： IP， postbuf：端口号
 * 返回值：   0:成功    其它：失败
 */
 UHFAPI_API int UHFSetDestIp (unsigned char* ipbuf, unsigned char *postbuf);



/*
 * 函数功能：  获取目标 IP 和端口号
 * 输出参数：  ipbuf + postbuf， IP+端口号
 * 返回值：   0:成功    其它：失败
 */
 UHFAPI_API int UHFGetDestIp (unsigned char* ipbuf, unsigned char *postbuf);







 UHFAPI_API int UHFSetWorkMode (unsigned char mode);


 UHFAPI_API int UHFGetWorkMode(unsigned char* mode);



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
 UHFAPI_API int UHFSetWorkModePara(unsigned char * para);


/**********************************************************************************************************
* 功能：设置触发工作模式参数
* 输出：
		para[0],	     触发IO：0x00表示触发输入1；0x01表示触发输入2。
		para[1],para[2]  触发工作时间：表示有触发输入时读卡工作时间，单位是10ms，高字节先，低字节后。
		para[3],para[4]	触发时间间隔：表示触发工作时间结束后，间隔多久再判断触发输入IO口进行触发。
		para[5]     	标签输出方式：0x00表示串口输出，0x01表示UDP输出
* 
* 返回值：   0:成功    其它：失败
*********************************************************************************************************/
 UHFAPI_API int UHFGetWorkModePara(unsigned char * para);


 UHFAPI_API int GetNetMAC(unsigned char* mac);
 UHFAPI_API int SetNetMAC(const unsigned char* mac);

/**********************************************************************************************************
* 功能：设置连续寻卡工作及等待时间
* 输入：DByte4 为掉电保存标志，0 表示不保存，1 表示保存；DByte3、DByte2 为工作时间，高字节在前，DByte1、DByte0 为等待时间，高字节在前


  返回值：0：设置成功     -1：设置失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFSetWorkTime( unsigned char DByte4, unsigned char DByte3 ,unsigned char DByte2, unsigned char DByte1 ,unsigned char DByte0);


/**********************************************************************************************************
* 功能：获取连续寻卡工作及等待时间
* 输出：DByte[0]、DByte[1] 表示工作时间；DByte[2]、DByte[3] 表示等待时间，单位为 ms，高位在前，最大 65535ms


  返回值：4：数据长度    -1：获取失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFGetWorkTime (unsigned char *DByte);



 UHFAPI_API int UHFSetBeep(unsigned char mode);
 UHFAPI_API int UHFGetBeep(unsigned char* mode);


 UHFAPI_API int UHFSetTempVal (unsigned char TempVal);
 UHFAPI_API int UHFGetTempVal(unsigned char* TempVal);



/**********************************************************************************************************
* 功能：设置协议类型
* 输入：type  0x00 表示 ISO18000-6C 协议,0x01 表示 GB/T 29768 国标协议,0x02 表示 GJB 7377.1 国军标协议
* 
*********************************************************************************************************/
 UHFAPI_API int UHFSetProtocolType( unsigned char type);



/**********************************************************************************************************
* 功能：获取协议类型
* 输出：type  0x00 表示 ISO18000-6C 协议,0x01 表示 GB/T 29768 国标协议,0x02 表示 GJB 7377.1 国军标协议

*********************************************************************************************************/
 UHFAPI_API int UHFGetProtocolType (unsigned char *type);

 


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
 UHFAPI_API int UHFGBTagLock(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char memory, unsigned char config, unsigned char action);



/**********************************************************************************************************
* 功能：继电器和 IO 控制输出设置
* 输入：output1:    0:低电平   1：高电平

        output2:    0:低电平   1：高电平

		outStatus： 0：断开    1：闭合

  返回值：0：设置成功     -1：设置失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFSetIOControl( unsigned char output1, unsigned char output2 ,unsigned char outStatus);


 UHFAPI_API int UHFSetOutputIO( unsigned char *output, unsigned char outputLen);

/**********************************************************************************************************
* 功能：获取继电器和 IO 控制输出设置状态
* 输出：statusData[0]:    0:低电平   1：高电平

        statusData[1]:    0:低电平   1：高电平


  返回值：2：数据长度    -1：获取失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFGetIOControl (unsigned char *statusData);
 UHFAPI_API int UHFGetIOStatus(unsigned char *statusData, unsigned short *len);


/**********************************************************************************************************
* 功能：读取 Sensor Code
* 输入：epc： EPC号，16个字节

       antNum:  天线号， 1个字节

	   powerValue： 功率值，2个字节

  输出：data， 2个字节

  返回值：2：数据长度    -1：获取失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFGetSensorCode (unsigned char *epc,unsigned char antNum,unsigned char *powerValue,unsigned char *data );




/**********************************************************************************************************
* 功能：读取 Calibration Data
* 输入：epc： EPC号，16个字节

       antNum:  天线号， 1个字节

	   powerValue： 功率值，2个字节

  输出：data， 8个字节

  返回值：8：数据长度    -1：获取失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFGetCalibrationData (unsigned char *epc,unsigned char antNum,unsigned char *powerValue,unsigned char *data );

/**********************************************************************************************************
* 功能：读取 Calibration Data
* 输入：mode:功能字
		epc： EPC号，16个字节
       antNum:  天线号， 1个字节
	   powerValue： 功率值，2个字节

  输出：data， 8个字节
  返回值：8：数据长度    -1：获取失败
* 
*********************************************************************************************************/
 UHFAPI_API int UHFGetCalibrationDataEX (unsigned char mode, unsigned char *epc,unsigned char antNum,unsigned char *powerValue,unsigned char *data );



/**********************************************************************************************************
* 功能：读取 On-Chip RSSI
* 输入：epc： EPC号，16个字节

       antNum:  天线号， 1个字节

	   powerValue： 功率值，2个字节

  输出：data， 2个字节

  返回值：2：数据长度    -1：获取失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFGetOnChipRSSI (unsigned char *epc,unsigned char antNum,unsigned char *powerValue,unsigned char *data );



/**********************************************************************************************************
* 功能：读取 Temperture Code
* 输入：epc： EPC号，16个字节

       antNum:  天线号， 1个字节

	   powerValue： 功率值，2个字节

  输出：data， 2个字节

  返回值：2：数据长度    -1：获取失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFGetTempertureCode (unsigned char *epc,unsigned char antNum,unsigned char *powerValue,unsigned char *data );





/**********************************************************************************************************
* 功能：读取 On-Chip RSSI+ Temp Code
* 输入：epc： EPC号，16个字节

       antNum:  天线号， 1个字节

	   powerValue： 功率值，2个字节

  输出：data， 4个字节  ,  RSSI: data[0] data[1]   ,  TempCode: data[2] data[3]  

  返回值：4：数据长度    -1：获取失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFGetOnChipRSSIAndTempCode (unsigned char *epc,unsigned char antNum,unsigned char *powerValue,unsigned char *data );


/**********************************************************************************************************
* 功能：开始   盘点 Calibration Data+ Sensor Code+ On-Chip RSSI+ Tempe Code
* 输入： 

       antNum:  天线号， 1个字节

	   powerValue： 功率值，2个字节


  返回值：0：发送成功    1：发送失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFInventoryTempTag (unsigned char antNum,unsigned char *powerValue);
 UHFAPI_API int UHFInventoryTempTagFilter (unsigned char antNum,unsigned char *powerValue);


 UHFAPI_API int UHF_GetTempTagReceived(int* uLenUii, unsigned char* uUii);


 

 UHFAPI_API int UHFGetTagData(unsigned char *tdata, int recvlen);


/**********************************************************************************************************
* 功能：写入 Calibration Data
* 输入：epc： EPC号，16个字节

       antNum:  天线号， 1个字节

	   powerValue： 功率值，2个字节

       data， Calibration Data , 8个字节

  返回值：0：成功    -1：失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFWriteCalibrationData (unsigned char *epc,unsigned char antNum,unsigned char *powerValue,unsigned char *data );


/**********************************************************************************************************
* 功能：验证标签
* 输入： 
password -- 访问密码
bank -- 掩码的数据区(0x00 为 Reserve 0x01 为 EPC，0x02 表示 TID，0x03 表示 USR)。
addr -- 掩码的地址
mDataLen -- 掩码的长度
mData -- 掩码数据
keyID -- Authenticate命令用的KeyID，默认为0x00
tLen -- IChallenge_TAM1数据长度,固定为10
tData -- IChallenge_TAM1数据
*输出
recvLen -- 输出数据长度
recvData -- 输出数据
返回值：0：执行成功    1：发送失败
* 
*********************************************************************************************************/
 UHFAPI_API int UHFAuthenticate (unsigned long password, unsigned char bank, unsigned short addr, 
	unsigned char *mData, unsigned short mDataLen,  unsigned char keyID, unsigned short tLen, unsigned char *tData, unsigned short *recvLen, unsigned char *recvData);
/*
与上述方法参数一致，区别在于返回值不同，如Impinj M775返回：Challenge：6个字节，Tags Shortened TID：8个字节，Tag Response：8个字节
*/
 UHFAPI_API int UHFAuthenticateCommon(unsigned long password, unsigned char bank, unsigned short addr,unsigned char *mData, unsigned short mDataLen, 
	unsigned char keyID, unsigned short tLen, unsigned char *tData, unsigned short *recvLen, unsigned char *recvData);
//function:AES encrypto or decrypto data
//in
//isEnc -- 1 encrypto  0,decrypto
//keylen == shoulde be 16 or  24 or 32
//in-out inbuf -- in date 
//inlen -- the length of input bytes,must be N*16
//return -1--key length error, others -- the length of inbuf return
 UHFAPI_API int AESHandle(unsigned char isEnc, unsigned char *key, unsigned short keylen, unsigned char *inbuf, unsigned long inLen);


/**********************************************************************************************************
* 功能：开始   盘点 On-Chip RSSI+ Tempe Code
* 输入： 

       antNum:  天线号， 1个字节

	   powerValue： 功率值，2个字节


  返回值：0：发送成功    1：发送失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFInventoryTempTag2 (unsigned char antNum,unsigned char *powerValue);



 UHFAPI_API int UHF_GetTempTagReceived2(int* uLenUii, unsigned char* uUii);

/**********************************************************************************************************
* 功能：设置调试日志输出级别，默认不输出
* 输入： 
*level -- 输出级别，0-不输出、1-输出致命错误、2-输出错误信息、3-输出基本日志信息、4-输出详细日志
*返回值：无 
*********************************************************************************************************/
 UHFAPI_API void SetLogLevel(int level);
 UHFAPI_API void SetLogFilePath(const char *path);
/**********************************************************************************************************
* 功能：设置是否保存传输过程中的日志文件，默认不保存
* 输入： 
*save -- 0-不保存、1-保存日志文件
*返回值：无 
*********************************************************************************************************/
 UHFAPI_API void SaveLogFile(int save);

 UHFAPI_API int UHFBTDeleteAllTagToFlash_SendData(unsigned char * outData);

 UHFAPI_API int UHFBTDeleteAllTagToFlash_RecvData(unsigned char * inData,int inLen);

 

 UHFAPI_API int UHFBTGetAllTagNumFromFlash_SendData(unsigned char * outData);

 UHFAPI_API int UHFBTGetAllTagNumFromFlash_RecvData(unsigned char * inData,int inLen);
 
 

 UHFAPI_API int UHFBTGetTagDataFromFlash_SendData(unsigned char * outData);

 UHFAPI_API int UHFBTGetTagDataFromFlash_RecvData(unsigned char * inData,int inLen, unsigned char * tagData);


 UHFAPI_API int UHFVerifyVoltage(int *value);

 UHFAPI_API int UHFJump2Boot_SendData(unsigned char flag, unsigned char * outData);

 UHFAPI_API int UHFJump2Boot_RecvData(unsigned char * inData,int inLen);



 UHFAPI_API int UHFStartUpd_SendData(unsigned char * outData);

 UHFAPI_API int UHFStartUpd_RecvData(unsigned char * inData,int inLen);



 UHFAPI_API int UHFUpdating_SendData(unsigned char *buf, unsigned char * outData);

 UHFAPI_API int UHFUpdating_RecvData(unsigned char * inData,int inLen);



 UHFAPI_API int UHFStopUpdate_SendData(unsigned char * outData);

 UHFAPI_API int UHFStopUpdate_RecvData(unsigned char * inData,int inLen);


/**********************************************************************************************************
* 功能：设置功率
* 输入：saveflag  -- 1:保存设置   0：不保存
* 输入：uPower -- 功率（DB）
*********************************************************************************************************/
 UHFAPI_API int UHFSetPower_SendData( unsigned char saveflag,unsigned char uPower, unsigned char * outData);

 UHFAPI_API int UHFSetPower_RecvData(unsigned char * inData,int inLen);




/**********************************************************************************************************
* 功能：获取功率
* 输出：uPower -- 功率（DB）
*********************************************************************************************************/

 UHFAPI_API int UHFGetPower_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetPower_RecvData(unsigned char * inData,int inLen, unsigned char *uPower);






/**********************************************************************************************************
* 功能：设置R2/R6蜂鸣器工作模式
* 输入：mode   1:打开  0：关闭

*********************************************************************************************************/
 UHFAPI_API int UHFSetBeep_SendData( unsigned char mode, unsigned char * outData);

 UHFAPI_API int UHFSetBeep_RecvData(unsigned char * inData,int inLen);

 



/**********************************************************************************************************
* 功能：设置读写器蜂鸣器工作模式
* 输入：mode   1:打开  0：关闭

*********************************************************************************************************/
 UHFAPI_API int UHFSetReaderBeep_SendData( unsigned char mode, unsigned char * outData);

 UHFAPI_API int UHFSetReaderBeep_RecvData(unsigned char * inData,int inLen);



/**********************************************************************************************************
* 功能：获取读写器蜂鸣器工作模式
* 返回值： 1:打开  0：关闭
*********************************************************************************************************/

 UHFAPI_API int UHFGetReaderBeep_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetReaderBeep_RecvData(unsigned char * inData,int inLen);








/**********************************************************************************************************
* 功能：打开2D扫描
* 输出：outData -- 输出数据
* 输出：outLen -- 数据长度
*********************************************************************************************************/

 UHFAPI_API int Open2DScan_RecvData(unsigned char * inData,int inLen, unsigned char* outData, unsigned char* outLen );
 






/**********************************************************************************************************
* 功能：单次盘存标签
* 输出：uLenUii -- UII长度
* 输出：uUii -- UII数据
*********************************************************************************************************/

 UHFAPI_API int UHFInventorySingle_SendData(unsigned char * outData);

 UHFAPI_API int UHFInventorySingle_RecvData(unsigned char * inData,int inLen, unsigned char* uLenUii, unsigned char* uUii );




/**********************************************************************************************************
* 功能：连续盘存标签
* 输出：uLenUii -- UII长度
* 输出：uUii -- UII数据
*********************************************************************************************************/

 UHFAPI_API int UHFInventory_SendData(unsigned char * outData);

 UHFAPI_API int UHFInventory_RecvData(unsigned char * inData,int inLen, unsigned char* uLenUii, unsigned char* uUii );




/**********************************************************************************************************
* 功能：停止盘存标签
* 返回值： 1:成功 0：失败
*********************************************************************************************************/

 UHFAPI_API int UHFStopInventory_SendData(unsigned char * outData);

 UHFAPI_API int UHFStopInventory_RecvData(unsigned char * inData,int inLen);




/**********************************************************************************************************
* 功能：获取标签数据
* 输出：uLenUii -- UII长度
* 输出：uUii -- UII数据
*********************************************************************************************************/

 UHFAPI_API int UHFGetTagsData_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetTagsData_RecvData(unsigned char * inData,int inLen, unsigned char* uLenUii, unsigned char* uUii );



 

 UHFAPI_API int UHF_TCP_TagsDataParse_RecvData(unsigned char * inData,int inLen, unsigned char* uLenUii, unsigned char* uUii );



/**********************************************************************************************************
* 功能：读标签数据区
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：bit
* 输入：FilterLen -- 启动过滤的长度， 单位：bit
* 输入：FilterData -- 启动过滤的数据
* 输入：uBank -- 读取数据的bank
* 输入：uPtr --  读取数据的起始地址， 单位：字
* 输入：uCnt --  读取数据的长度， 单位：字
* 输出：uReadDatabuf --  读取到的数据
* 输出：uReadDataLen --  读取到的数据长度
*********************************************************************************************************/
 UHFAPI_API int UHFReadData_SendData(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char uBank,unsigned int uPtr, unsigned int uCnt ,unsigned char * outData);

 UHFAPI_API int UHFReadData_RecvData(unsigned char * inData,int inLen, unsigned char* uReadDatabuf, unsigned int* uReadDataLen );




/**********************************************************************************************************
* 功能：写标签数据区
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：bit
* 输入：FilterLen -- 启动过滤的长度， 单位：bit
* 输入：FilterData -- 启动过滤的数据  单位：字节
* 输入：uBank -- 读取数据的bank
* 输入：uPtr --  读取数据的起始地址， 单位：字
* 输入：uCnt --  读取数据的长度， 单位：字
* 输入：uWriteDatabuf --  写入的数据
*********************************************************************************************************/

 UHFAPI_API int UHFWriteData_SendData(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char uBank,unsigned int uPtr, unsigned char uCnt,unsigned char *uDatabuf,unsigned char * outData);

 UHFAPI_API int UHFWriteData_RecvData(unsigned char * inData,int inLen);







/**********************************************************************************************************
* 功能：LOCK标签
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
* 输入：FilterLen -- 启动过滤的长度， 单位：字节
* 输入：FilterData -- 启动过滤的数据
* 输入：lockbuf --  3字节，第0-9位为Action位， 第10-19位为Mask位
*********************************************************************************************************/
 UHFAPI_API int UHFLockTag_SendData(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char *lockbuf ,unsigned char * outData);

 UHFAPI_API int UHFLockTag_RecvData(unsigned char * inData,int inLen);



/**********************************************************************************************************
* 功能：KILL标签
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
* 输入：FilterLen -- 启动过滤的长度， 单位：字节
* 输入：FilterData -- 启动过滤的数据
*********************************************************************************************************/
 UHFAPI_API int UHFKillTag_SendData(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData ,
											   unsigned char * outData);

 UHFAPI_API int UHFKillTag_RecvData(unsigned char * inData,int inLen);


 

/**********************************************************************************************************
* 功能：区域设置
* 输入：saveflag -- 1:掉电保存，  0：不保存
* 输入：region -- 0x01(China1),0x02(China2),0x04(Europe),0x08(USA),0x16(Korea),0x32(Japan)
*********************************************************************************************************/

 UHFAPI_API int UHFSetRegion_SendData( unsigned char saveflag, unsigned char region,unsigned char * outData);

 UHFAPI_API int UHFSetRegion_RecvData(unsigned char * inData,int inLen);





/**********************************************************************************************************
* 功能：获取区域设置
* 输出：region -- 0x01(China1),0x02(China2),0x04(Europe),0x08(USA),0x16(Korea),0x32(Japan)
*********************************************************************************************************/
 UHFAPI_API int UHFGetRegion_SendData( unsigned char * outData);

 UHFAPI_API int UHFGetRegion_RecvData( unsigned char * inData,int inLen, unsigned char *region);


/**********************************************************************************************************
* 功能：获取ID号
* 输出：id--整型ID号
*********************************************************************************************************/
 UHFAPI_API int UHFGetDeviceID_SendData(unsigned char * outData);


 UHFAPI_API int UHFGetDeviceID_RecvData( unsigned char * inData,int inLen, unsigned int *id);



/**********************************************************************************************************
* 功能：获取软件版本号
* 输出：version[0]--版本号长度 ,  version[1--x]--版本号
*********************************************************************************************************/
 UHFAPI_API int UHFGetSoftwareVersion_SendData(unsigned char * outData);


 UHFAPI_API int UHFGetSoftwareVersion_RecvData( unsigned char * inData,int inLen, unsigned char *version);



/**********************************************************************************************************
* 功能：获取当前温度
* 输出：temperature -- 整型
*********************************************************************************************************/
 UHFAPI_API int UHFGetTemperature_SendData( unsigned char *outData);


 UHFAPI_API int UHFGetTemperature_RecvData( unsigned char * inData,int inLen, unsigned char *temperature);






/**********************************************************************************************************
* 功能：获取硬件版本号
* 输出：version[0]--版本号长度 ,  version[1--x]--版本号
*********************************************************************************************************/
 UHFAPI_API int UHFGetHardwareVersion_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetHardwareVersion_RecvData( unsigned char * inData,int inLen, unsigned char *version);



/**********************************************************************************************************
* 功能：设置寻标签过滤设置
* 输入：saveflag -- 1:掉电保存， 0：不保存
* 输入：bank --  0x01:EPC , 0x02:TID, 0x03:USR
* 输入：startaddr 起始地址，单位：字节
* 输入：datalen 数据长度， 单位:字节
* 输入：databuf 数据
*********************************************************************************************************/
 UHFAPI_API int UHFSetFilter_SendData(unsigned char saveflag,unsigned char bank,unsigned int startaddr,unsigned int datalen,unsigned char *databuf ,unsigned char * outData);

 UHFAPI_API int UHFSetFilter_RecvData(unsigned char * inData,int inLen);




/**********************************************************************************************************
* 功能：设置协议类型
* 输入：type  0x00 表示 ISO18000-6C 协议,0x01 表示 GB/T 29768 国标协议,0x02 表示 GJB 7377.1 国军标协议
* 
*********************************************************************************************************/
 UHFAPI_API int UHFSetProtocolType_SendData(unsigned char type ,unsigned char * outData);

 UHFAPI_API int UHFSetProtocolType_RecvData(unsigned char * inData,int inLen);



/**********************************************************************************************************
* 功能：获取协议类型
* 输出：type  0x00 表示 ISO18000-6C 协议,0x01 表示 GB/T 29768 国标协议,0x02 表示 GJB 7377.1 国军标协议

*********************************************************************************************************/
 UHFAPI_API int UHFGetProtocolType_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetProtocolType_RecvData(unsigned char * inData,int inLen, unsigned char * type);





/**********************************************************************************************************
* 功能：天线设置
* 输入：saveflag -- 1:掉电保存，  0：不保存
* 输入：buf--2bytes, 共16bits, 每bit 置1选择对应天线
*********************************************************************************************************/
 UHFAPI_API int UHFSetANT_SendData(unsigned char saveflag,unsigned char *buf,unsigned char * outData);

 UHFAPI_API int UHFSetANT_RecvData(unsigned char * inData,int inLen);





/**********************************************************************************************************
* 功能：获取天线设置
* 输出：buf--2bytes, 共16bits,
*********************************************************************************************************/
 UHFAPI_API int UHFGetANT_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetANT_RecvData(unsigned char * inData,int inLen,unsigned char *buf);





/**********************************************************************************************************
* 功能：设置Gen2参数
* 输入：
**********************************************************************************************************/
 UHFAPI_API int UHFSetGen2_SendData(unsigned char Target,unsigned char Action, unsigned char T,unsigned char Q,
								unsigned char StartQ,unsigned char MinQ,
								unsigned char MaxQ,unsigned char D,unsigned char C,unsigned char P,
								unsigned char Sel,unsigned char Session,unsigned char G,unsigned char LF,unsigned char * outData);


 UHFAPI_API int UHFSetGen2_RecvData(unsigned char * inData,int inLen);




/**********************************************************************************************************
* 功能：获取Gen2参数
* 输入：
*********************************************************************************************************/
 UHFAPI_API int UHFGetGen2_SendData(unsigned char * outData);


 UHFAPI_API int UHFGetGen2_RecvData(unsigned char * inData,int inLen, unsigned char *Target, unsigned char *Action,unsigned char *T,unsigned char *Q,
					unsigned char *StartQ,unsigned char *MinQ,
					unsigned char *MaxQ,unsigned char *D, unsigned char *Coding,unsigned char *P,
					unsigned char *Sel,unsigned char *Session,unsigned char *G,unsigned char *LF);




/**********************************************************************************************************
* 功能：设置链路组合
* 输入：saveflag -- 1:掉电保存， 0：不保存
* 输入：mode --  0:DSB_ASK/FM0/40KHZ , 1:PR_ASK/Miller4/250KHZ , 2:PR_ASK/Miller4/300KHZ, 3:DSB_ASK/FM0/400KHZ
*********************************************************************************************************/
 UHFAPI_API int UHFSetRFLink_SendData(unsigned char saveflag,unsigned char mode,unsigned char * outData);

 UHFAPI_API int UHFSetRFLink_RecvData(unsigned char * inData,int inLen);




/**********************************************************************************************************
* 功能：获取链路组合
* 输出：mode --  0:DSB_ASK/FM0/40KHZ , 1:PR_ASK/Miller4/250KHZ , 2:PR_ASK/Miller4/300KHZ, 3:DSB_ASK/FM0/400KHZ
*********************************************************************************************************/
 UHFAPI_API int UHFGetRFLink_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetRFLink_RecvData(unsigned char * inData,int inLen, unsigned char* uMode);





 UHFAPI_API int UHFSetIp_SendData(unsigned char* ipbuf, unsigned char *postbuf,unsigned char * outData);

 UHFAPI_API int UHFSetIp_RecvData(unsigned char * inData,int inLen);


 UHFAPI_API int UHFGetIp_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetIp_RecvData(unsigned char * inData,int inLen,unsigned char* ipbuf, unsigned char *postbuf);




 UHFAPI_API int UHFSetDestIp_SendData(unsigned char* ipbuf, unsigned char *postbuf,unsigned char * outData);

 UHFAPI_API int UHFSetDestIp_RecvData(unsigned char * inData,int inLen);


 UHFAPI_API int UHFGetDestIp_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetDestIp_RecvData(unsigned char * inData,int inLen,unsigned char* ipbuf, unsigned char *postbuf);





/**********************************************************************************************************
* 功能：设置跳频
* 输入：nums -- 跳频个数
* 输入：Freqbuf--频点数组（整型） ，如920125，921250.....
*********************************************************************************************************/
 UHFAPI_API int UHFSetJumpFrequency_SendData( unsigned char nums,unsigned int *Freqbuf,unsigned char * outData);

 UHFAPI_API int UHFSetJumpFrequency_RecvData(unsigned char * inData,int inLen);




/**********************************************************************************************************
* 功能：获取跳频
* 输出：Freqbuf[0]--频点个数， Freqbuf[1]..[x]--频点数组（整型）
*********************************************************************************************************/
 UHFAPI_API int UHFGetJumpFrequency_SendData(unsigned char * outData);


 UHFAPI_API int UHFGetJumpFrequency_RecvData(unsigned char * inData,int inLen, unsigned int *Freqbuf);




/**********************************************************************************************************
* 功能：设置天线工作时间
* 输入：antnum -- 天线号
* 输入：saveflag -- 1:掉电保存， 0：不保存
* 输入：WorkTime -- 工作时间 ，单位ms, 范围 10-65535ms
*********************************************************************************************************/
 UHFAPI_API int UHFSetANTWorkTime_SendData(unsigned char antnum,unsigned char saveflag,unsigned int WorkTime, unsigned char * outData);

 UHFAPI_API int UHFSetANTWorkTime_RecvData(unsigned char * inData,int inLen);



/**********************************************************************************************************
* 功能：获取天线工作时间
* 输入：antnum -- 天线号
* 输出：WorkTime -- 工作时间 ，单位ms, 范围 10-65535ms
*********************************************************************************************************/
 UHFAPI_API int UHFGetANTWorkTime_SendData(unsigned char antnum, unsigned char * outData);

 UHFAPI_API int UHFGetANTWorkTime_RecvData(unsigned char * inData,int inLen,unsigned int *WorkTime);


 UHFAPI_API int UHFSetWorkMode_SendData(unsigned char mode, unsigned char * outData);

 UHFAPI_API int UHFSetWorkMode_RecvData(unsigned char * inData,int inLen);




 UHFAPI_API int UHFGetWorkMode_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetWorkMode_RecvData(unsigned char * inData,int inLen,unsigned char* mode);






/**********************************************************************************************************
* 功能：设置EPC TID USER模式

* 输入：saveflag -- 1:掉电保存， 0：不保存

* 输入：Memory 0x00，表示关闭； 0x01，表示开启EPC+TID模式（默认 地址为 0x00, 长 度 为 6 个 字 ） ； 0x02, 表 示 开 启EPC+TID+USER模式

* 输入：address 为USER区的起始地址（单位为字）
* 输入：为USER区的长度（单位为字）
*********************************************************************************************************/
 
 UHFAPI_API int UHFSetEPCTIDMode_SendData(unsigned char saveFlag, unsigned char memory,unsigned char address, unsigned char length, unsigned char * outData);

 UHFAPI_API int UHFSetEPCTIDMode_RecvData(unsigned char * inData, int inLen);




/**********************************************************************************************************
* 功能：获取EPC TID USER 模式

* 输入：rev1 :保留数据，传入0
* 输入：rev2 :保留数据，传入0

* 输出：data[0]，Memory 0x00，表示关闭； 0x01，表示开启EPC+TID模式（默认 地址为 0x00, 长 度 为 6 个 字 ） ； 0x02, 表 示 开 启EPC+TID+USER模式

* 输出：data[1]address 为USER区的起始地址（单位为字）
* 输出：data[2]为USER区的长度（单位为字）
*
* 返回值：3：正确，其它：错误
*********************************************************************************************************/
 UHFAPI_API int UHFGetEPCTIDMode_SendData(unsigned char rev1, unsigned char rev2, unsigned char * outData);

 UHFAPI_API int UHFGetEPCTIDMode_RecvData(unsigned char * inData, int inLen, unsigned char *data); 




/**********************************************************************************************************
* 功能：设置Tagfocus功能
* 输入：flag -- 1:开启， 0：关闭
*********************************************************************************************************/
 UHFAPI_API int UHFSetTagfocus_SendData(unsigned char flag, unsigned char * outData);

 UHFAPI_API int UHFSetTagfocus_RecvData(unsigned char * inData,int inLen);


/**********************************************************************************************************
* 功能：获取Tagfocus功能
* 输出：flag -- 1:开启， 0：关闭
*********************************************************************************************************/
 UHFAPI_API int UHFGetTagfocus_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetTagfocus_RecvData(unsigned char * inData,int inLen,unsigned char *flag);





/**********************************************************************************************************
* 功能：设置FastID功能
* 输入：flag -- 1:开启， 0：关闭
*********************************************************************************************************/
 UHFAPI_API int UHFSetFastID_SendData(unsigned char flag, unsigned char * outData);

 UHFAPI_API int UHFSetFastID_RecvData(unsigned char * inData,int inLen);


/**********************************************************************************************************
* 功能：获取FastID功能
* 输出：flag -- 1:开启， 0：关闭
*********************************************************************************************************/
 UHFAPI_API int UHFGetFastID_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetFastID_RecvData(unsigned char * inData,int inLen,unsigned char *flag);




/**********************************************************************************************************
* 功能：设置CW
* 输入：flag -- 1:开CW，  0：关CW
*********************************************************************************************************/
 UHFAPI_API int UHFSetCW_SendData( unsigned char flag, unsigned char * outData);

 UHFAPI_API int UHFSetCW_RecvData(unsigned char * inData,int inLen);



/**********************************************************************************************************
* 功能：获取CW
* 输出：flag -- 1:开CW，  0：关CW
*********************************************************************************************************/
 UHFAPI_API int UHFGetCW_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetCW_RecvData(unsigned char * inData,int inLen,unsigned char *flag);




/**********************************************************************************************************
* 功能：设置软件复位
*********************************************************************************************************/
 UHFAPI_API int UHFSetSoftReset_SendData(unsigned char * outData);

 UHFAPI_API int UHFSetSoftReset_RecvData(unsigned char * inData,int inLen);




/**********************************************************************************************************
* 功能：继电器和 IO 控制输出设置
* 输入：output1:    0:低电平   1：高电平

        output2:    0:低电平   1：高电平

		outStatus： 0：断开    1：闭合

  返回值：0：设置成功     -1：设置失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFSetIOControl_SendData(unsigned char output1, unsigned char output2 ,unsigned char outStatus, unsigned char * outData);

 UHFAPI_API int UHFSetIOControl_RecvData(unsigned char * inData,int inLen);




/**********************************************************************************************************
* 功能：获取继电器和 IO 控制输出设置状态
* 输出：statusData[0]:    0:低电平   1：高电平

        statusData[1]:    0:低电平   1：高电平


  返回值：2：数据长度    -1：获取失败

* 
*********************************************************************************************************/
 UHFAPI_API int UHFGetIOControl_SendData(unsigned char * outData);

 UHFAPI_API int UHFGetIOControl_RecvData(unsigned char * inData,int inLen,unsigned char *statusData);



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
 UHFAPI_API int UHFBlockEraseData_SendData(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char uBank,unsigned int uPtr, unsigned char uCnt,unsigned char * outData);

 UHFAPI_API int UHFBlockEraseData_RecvData(unsigned char * inData,int inLen);





/**********************************************************************************************************
* 功能：设置QT命令参数
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
* 输入：FilterLen -- 启动过滤的长度， 单位：字节
* 输入：FilterData -- 启动过滤的数据
* 输入：QTData --  bit0：（0：表示无近距离控制 ， 1：表示启用近距离控制）  bit1：(0:表示启用private Memory map, 1：启用public memory map)
*********************************************************************************************************/
 UHFAPI_API int UHFSetQT_SendData(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char QTData,unsigned char * outData);

 UHFAPI_API int UHFSetQT_RecvData(unsigned char * inData,int inLen);




/**********************************************************************************************************
* 功能：获取QT命令参数
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
* 输入：FilterLen -- 启动过滤的长度， 单位：字节
* 输入：FilterData -- 启动过滤的数据
* 输出：QTData --  bit0：（0：表示无近距离控制 ， 1：表示启用近距离控制）  bit1：(0:表示启用private Memory map, 1：启用public memory map)
*********************************************************************************************************/
 UHFAPI_API int UHFGetQT_SendData(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                               unsigned char * outData);

 UHFAPI_API int UHFGetQT_RecvData(unsigned char * inData,int inLen, unsigned char *QTData);





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
 UHFAPI_API int UHFBlockPermalock_SendData(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									 unsigned char ReadLock,unsigned char uBank,unsigned int uPtr, unsigned char uRange,unsigned char *uMaskbuf, unsigned char * outData);


/**********************************************************************************************************
* 输入：uRange --  Block范围，单位为16个块
* 输出：uMaskbuf -- 块掩码数据，2个字节，16bit 对应16个块
*********************************************************************************************************/
 UHFAPI_API int UHFBlockPermalock_RecvData(unsigned char uRange,unsigned char * inData,int inLen,unsigned char *uMaskbuf);




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
 UHFAPI_API int UHFGBTagLock_SendData(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char memory, unsigned char config, unsigned char action, unsigned char * outData);

 UHFAPI_API int UHFGBTagLock_RecvData(unsigned char * inData,int inLen);


/**********************************************************************************************************
* 功能：BlockWrite 特定长度的数据到标签的特定地址
* 输入：uAccessPwd -- 4字节密码
* 输入：FilterBank -- 启动过滤的bank号， 1：EPC, 2:TID, 3:USR
* 输入：FilterStartaddr -- 启动过滤的起始地址， 单位：字节
* 输入：FilterLen -- 启动过滤的长度， 单位：字节
* 输入：FilterData -- 启动过滤的数据
* 输入：uBank -- 块号  1：EPC, 2:TID, 3:USR
* 输入：uPtr --  读取数据的起始地址， 单位：字
* 输入：uCnt --  读取数据的长度， 单位：字
* 输入：uWriteDatabuf --  写入的数据
*********************************************************************************************************/
 UHFAPI_API int UHFBlockWriteData_SendData(unsigned char* uAccessPwd, unsigned char FilterBank,unsigned int FilterStartaddr, unsigned int FilterLen, unsigned char *FilterData,
									                              unsigned char uBank,unsigned int uPtr, unsigned int uCnt,unsigned char *uWriteDatabuf, unsigned char * outData);


/*
初始化温度标签
return: 0--success, -1--unknow error, others--error code
mask_bank：掩码的数据区(0x00 为 Reserve 0x01 为 EPC， 0x02 表示 TID， 0x03 表示USR)。
mask_addr：为掩码的地址(bit为单位)，高字节在前。
mask_len：为掩码的长度(bit为单位)，高字节在前。
mask_data：为掩码数据，mask_len为0时，这里没有数据
*/

 UHFAPI_API int UHFInitRegFile (unsigned char mask_bank, unsigned short mask_addr, unsigned short mask_len, unsigned char *mask_data);

/*
读取标签温度
return: 0--success, -1--unknow error, others--error code
mask_bank：掩码的数据区(0x00 为 Reserve 0x01 为 EPC， 0x02 表示 TID， 0x03 表示USR)。
mask_addr：为掩码的地址(bit为单位)，高字节在前。
mask_len：为掩码的长度(bit为单位)，高字节在前。
mask_data：为掩码数据，mask_len为0时，这里没有数据
temp:output,the point of temperature
*/
 UHFAPI_API int UHFReadTagTemp (unsigned char mask_bank, unsigned short mask_addr, unsigned short mask_len, unsigned char *mask_data, float *temp);

 UHFAPI_API int UHFGetAntennaLinkStatus(unsigned short *link_status);

 UHFAPI_API int UHFBlockWriteData_RecvData(unsigned char * inData,int inLen);


/**********************************************************************************************************
* 功能：获取STM32软件版本号
* 输出：version[0-4] --- Vx.xx
*********************************************************************************************************/
 
 UHFAPI_API int UHFGetSTM32Version_SendData(unsigned char * outData);
 
 UHFAPI_API int UHFGetSTM32Version_RecvData( unsigned char * inData,int inLen, unsigned char *version);


 UHFAPI_API  int  UHFGetVersionCode(char *datetime);



/**********************************************************************************************************
* 功能：获取HF STM32 chip id,12字节
* 返回：小于0，错误，大于0表示为返回数据sn 的长度
* 输出： sn 返回的12字节芯片id
*********************************************************************************************************/
 UHFAPI_API int HF_GetChipSn(unsigned char *sn) ;

/**********************************************************************************************************
* 功能：获取HF STM32软件版本号
* 输出： "M0_663(15/11/24)"
*********************************************************************************************************/
 UHFAPI_API int HF_GetVer(unsigned char *ver);

/*
功能：打开蜂鸣器提示音
输入：millsec 连续提示的毫秒数
返回：0正常，其他失败
*/
 UHFAPI_API int HF_BeepMs(unsigned short millsec);

/**********************************************************************************************************
* 功能：打开或关闭高频天线
* state  0，关闭天线 1，开启天线
* 返回：小于0，错误，0 正常
*********************************************************************************************************/
 UHFAPI_API int HF_SetAntenna(unsigned char state);

/*功能：获取卡片类型，并唤醒卡片
* iMode ：0 唤醒未激活卡片 1, 唤醒所有状态卡片
* 返回：表示状态码，0成功，其他失败
* 输出： outData byte[0],byte[1]表示获取的卡片类型*/
 UHFAPI_API int HF_Request(int iMode, unsigned char * outData, unsigned short *outLen);

/*功能：卡片防碰撞
* iMode ：0 唤醒未激活卡片 1, 唤醒所有状态卡片
* 返回：表示状态码，0成功，其他失败
* 输出： outData  byte[0],byte[1]表示获取的卡片类型
				 byte[2~2+N-1]表示UID，其中N为UID长度
				 byte[2+N]表示卡片第三个类型字节*/
 UHFAPI_API int HF_Anticoll(int iMode, unsigned char * outData, unsigned short *outLen);
/*功能：复位TypeA Cpu卡
* 返回：小于0，错误，大于0表示为返回数据outData 的长度
* 输出： outData CPU卡复位后返回的信息*/
 UHFAPI_API int HF_CpuResetA(unsigned char * outData, unsigned short *outLen);

/*功能：交换COS命令
* 返回：小于0，错误，大于0表示为返回数据outdata 的长度
* 输出： outdata 返回命令*/
 UHFAPI_API int HF_CpuCos(unsigned char * indata, unsigned short cosLen,unsigned char * outData, unsigned short *outLen);

/* 功能：打开设备
* 返回：小于0，错误，0 表示成功 */
  UHFAPI_API int HF_Open();


/* 功能：关闭设备*/
 UHFAPI_API void HF_Close();

//key length is 16/24/32 bytes
 UHFAPI_API void AesSetKey(const unsigned char *key, unsigned short length);

 UHFAPI_API void AesDecrypto(unsigned char *buf, unsigned long length);

 UHFAPI_API void AesEncrypto(unsigned char *buf, unsigned long length);

////key length is 8 bytes
// UHFAPI_API void DesEncrypto(unsigned char *key, unsigned char *buf, unsigned long length);
// UHFAPI_API void DesDecrypto(unsigned char *key, unsigned char *buf, unsigned long length);
////mode  0 加密   1、解密
// UHFAPI_API void TripleDes(unsigned char *pInData, unsigned char *pOutData, unsigned char *key, unsigned char mode);
//out data:32bytes
 UHFAPI_API void Sha256Gen(const unsigned char *in_data, unsigned long len, unsigned char *out_data);
 UHFAPI_API int UHFWriteScreenBlockData (unsigned char* pwd, unsigned char sector, unsigned short mask_addr, unsigned short mask_len, unsigned char *mask_data,
	unsigned char type, unsigned short w_addr, unsigned short w_len, unsigned char *w_data);

 UHFAPI_API int UHFPerformInventory(unsigned short mode, const unsigned char *param, unsigned short paramlen);

 UHFAPI_API int UHFUploadUserParam(const unsigned char *param, unsigned short paramLen);

 UHFAPI_API int UHFDownloadUserParam(unsigned char *param, unsigned short *paramLen);

 UHFAPI_API int UHFUploadWifiParam(const unsigned char *param, unsigned short paramLen);
 UHFAPI_API int UHFDownloadWifiParam(unsigned char *param, unsigned short *paramLen);
 UHFAPI_API int WIFIAtCmdTransceive(const char *in_cmd, OnAtCmdReceived onReceived);
 UHFAPI_API int ShowSceenText(const char *txt, int duration_ms);
 UHFAPI_API int BuzzerWork(int duration_ms);
 UHFAPI_API int UHFSetParams(unsigned char *params, unsigned short paramLen);
 UHFAPI_API int ZebraGetParams(unsigned char *params, unsigned short paramLen, unsigned char* outData, unsigned short* outLen);
//crypto module transmit
//pin--point of data which send to crypto mudle 
//inLen--the length of send data 
//pout--point of receive crypto returned data
//outLen--the length of received data
//wait_recv_ms--the maxinum millsecond while waiting for crypto module return 
 UHFAPI_API int CryptoTransmit(const unsigned char* pin, unsigned short inLen, unsigned char *pout, unsigned short *outLen, unsigned short wait_recv_ms);

 UHFAPI_API int BleSetParams (unsigned char *params, unsigned short paramLen);
typedef void (*OnTagReceivedCall)(char *epc);
//return:0-success,1-connect failure,2-already connected

/*
print test to cursor
type:charactic encode mode,can be ENUM_CHAR_CODE_ANSI or ENUM_CHAR_CODE_UTF8;
text:
len:length of text in byte
*/
 UHFAPI_API int PrintTextToCursor(ENUM_CHAR_CODE_TYPE type, const char *text, unsigned short len);
 UHFAPI_API int PrintKeyCodeToCursor(ENUM_CHAR_CODE_TYPE type, const char *text, unsigned short len);
 //UHFAPI_API int GeneraQRCode(const char *path, const char *szSourceSring);
 UHFAPI_API int ScannerRead (unsigned char blocking, unsigned char mode,  unsigned char *code, unsigned int *len, unsigned int timeout);
 UHFAPI_API int ScannerTriggleWork (unsigned char on_off);
 UHFAPI_API int UnlockDevice(void);
/********  export tags for R2  *********/
/*
  tdata:
  ---------------------------------------------
  |number|tag len|tag data|tag len|tag data|...
  ---------------------------------------------
  |1 byte|1 byte |N bytes |1 byte |N bytes |...
  ---------------------------------------------
  tlen:the numbre of bytes in tdata
*/
 UHFAPI_API int LocalExportTags(unsigned char *tdata, unsigned int *tlen);
//mode:0-all, 1-new tags number
 UHFAPI_API int LocalGetTagsNumber(int mode, unsigned int *number);
 UHFAPI_API int LocalDeleteTags();
//type:0-UR4,1-UR1A,2-UR-DEV,3-WeiYuDa
 UHFAPI_API int GetURDeviceType(unsigned char* type);
 UHFAPI_API int SetURDeviceType(unsigned char type);

 UHFAPI_API int GetURTagFilter(unsigned char* enable, unsigned long *timeout);
 UHFAPI_API int SetURTagFilter(unsigned char enable, unsigned long timeout);
 UHFAPI_API int GetURWorkSeconds(int* work_secs);
 UHFAPI_API int SetURWorkSeconds(int work_secs);
/*
功能：设置UHF模块通讯波特率，设置后必须重启UHF模块，重启后生效，掉电保存
输入：baud:1--57600, 2--115200, 3--460800
返回：0正常，其他失败
*/
 UHFAPI_API int UHFSetBaudrate(unsigned char baud);
/*
功能：获取UHF波特率
输出：baud:1--57600, 2--115200, 3--460800
返回：0正常，其他失败
*/
 UHFAPI_API int UHFGetBaudrate(unsigned char *baud);
/*
功能：设置主板SN
输入：sn--SN序列号，字符串类型
输入：slen--sn长度
返回：0正常，其他失败
*/
 UHFAPI_API int SetMainboardSN(const unsigned char *sn, int slen);
/*
功能：获取主板SN
输出：sn——主板SN号，字符串类型
输出：sn长度
返回：0正常，其他失败
*/
 UHFAPI_API int GetMainboardSN(unsigned char *sn, int *rlen);
/*
功能：获取主板芯片信息
输出：info——芯片信息，字符串类型
输出：信息长度
返回：0正常，其他失败
*/
 UHFAPI_API int GetChipInfo(unsigned char *info, int *rlen);
 UHFAPI_API int DownloadBootParam(unsigned char *paramBuf, unsigned int paramLen);

 UHFAPI_API int UploadBootParam(unsigned char *plist, unsigned char listLen, unsigned char *paramBuf, unsigned int paramLen, unsigned int *retLen);
 UHFAPI_API int GetScannerInfo(unsigned char *plist, unsigned int listLen, unsigned char *info, unsigned int *iLen);
 UHFAPI_API int SetBluetoothParam(unsigned char *param, unsigned int plen, unsigned char *rparam, unsigned int *rplen);
}
#endif
