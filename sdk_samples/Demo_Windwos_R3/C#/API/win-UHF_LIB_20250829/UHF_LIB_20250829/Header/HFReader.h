#ifndef _HF_READER_H_
#define _HF_READER_H_


#define EXPORT_HF_API __declspec(dllexport)
extern "C"
{
	/**
	*@brief: get HF module version
	*@param[out] pcVer:the point of HF module version
	*@param[out] pcVerLen:the point of HF module version length
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFGetVer(char *pcVer, unsigned char *pcVerLen);
	/**
	*@brief: reset rc663 reader module
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFResetModule();
	/**
	*@brief: buzzer work
	*@param[in] ms:millsecond of buzzer working
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFBuzzerWork(int ms);
	/**
	*@brief: set device buzzer volume
	*@param[out] vol:the volume value of buzzer,0~4095
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFSetVolume(int vol);

	/**
	*@brief: read rc663 register
	*@param[in] reg_addr:address of register
	*@param[out] val:1 byte value 0~255
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFReadReg(unsigned char reg_addr, unsigned char *val);

	/**
	*@brief: Turn on HF antenna
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFTurnOnRF(void);
	/**
	*@brief: Turn off HF antenna
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API  int HFTurnOffRF(void);

	/**
	*@brief: request 14443A card
	*@param[in] cMode:0x26 request idle, 0x52 request all
	*@param[out] pcCardType:card type (2 bytes)
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFRequestTypeA(int cMode, unsigned char *pcCardType);

	/**
	*@brief: Anticoll 14443A card
	*@param[out] pcSnr:the point of card serial number(less than 8 bytes)
	*@param[out] pcSnrLen:the point of card serial number length
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFAnticollTypeA(unsigned char *pcSnr, unsigned char *pcSnrLen);

	/**
	*@brief: Select 14443A card
	*@param[out] SAK:the point of card type(1 byte)
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFSelectTypeA(unsigned char *SAK);

	/**
	*@brief: Activate 14443A card,include request¡¢anticoll¡¢select,
	*@param[in] cMode:0x26 request idle, 0x52 request all
	*@param[out] pcATQA:type0~1(2bytes)+pcUid length(1bytes)+pcUid(n bytes)+type2(1bytes)
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFActivateTypeA(int cMode, unsigned char *pcATQA);

	/**
	*@brief: Halt 14443A card
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFHaltTypeA(void);

	/**
	*@brief: Authentication 14443A card
	*@Param[in] cMode:0x60 A type key, 0x61 B type key
	*@Param[in] cBlock:the cBlock of card,such as M1 card value 0~63
	*@Param[in] pcKey:6 bytes key value
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFAuthentication(unsigned char cMode, unsigned char cBlock, unsigned char *pcKey);

	/**
	*@brief: Read 14443A card
	*@Param[in] cBlock:the cBlock of card,such as S50 card value 0~63,S70 0~255
	*@Param[out] bdata:16 bytes cBlock data
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFReadBlock(unsigned char cBlock, unsigned char *bdata);
	/**
	*@brief: Write 14443A card
	*@Param[in] cBlock:the cBlock of card,such as S50 card value 0~63,S70 0~255
	*@Param[in] pcBlockData:16 bytes cBlock data
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFWriteBlock(unsigned char cBlock, unsigned char *pcBlockData);

	/**
	*@brief: initial E wallet value
	*@Param[in] cBlock:the cBlock of card,such as S50 card value 0~63,S70 0~255
	*@Param[in] lValue:32bit
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFInitValue(unsigned char cBlock, unsigned long lValue);

	/**
	*@brief: read E wallet value
	*@Param[in] cBlock:the cBlock of card,such as S50 card value 0~63,S70 0~255
	*@Param[out] plValue:point of 32bit value
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFReadValue(unsigned char cBlock, unsigned long *plValue);

	/**
	*@brief: decrease E wallet value
	*@Param[in] blockValue:the cBlock of value saved 
	*@Param[in] blockResult:the cBlock of operate
	*@Param[in] value:32bit value
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFDecValue(unsigned char blockValue, unsigned char blockResult, unsigned long value);

	/**
	*@brief: increase E wallet value
	*@Param[in] blockValue:the cBlock of value saved 
	*@Param[in] blockResult:the cBlock of operate
	*@Param[in] value:32bit value
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFIncValue(unsigned char blockValue, unsigned char blockResult, unsigned long value);

	/**
	*@brief: Restore E wallet value
	*@Param[in] cBlock:the cBlock address
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFRestore(unsigned char cBlock);

	/**
	*@brief: Transfer E wallet value
	*@Param[in] cBlock:the cBlock address
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFTransfer(unsigned char cBlock);

	/**
	*@brief: aticoll ul card
	*@Param[out] pcSnr:the point of card serial number
	*@Param[out] pcSnrLen:the point of card serial number length
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFUlAnticoll(unsigned char *pcSnr, unsigned char *pcSnrLen);

	/**
	*@brief: write ul card
	*@Param[in] cBlock:the address of card
	*@Param[in] pcWriteData:the point of write data
	*@Param[in] cWriteLen:the length of write  data
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFUlWrite(unsigned char cBlock, unsigned char *pcWriteData, unsigned char cWriteLen);

	/**
	*@brief: reset cpu card
	*@Param[out] cardType:the point of card type
	*@Param[out] pcUid:the point of write data
	*@Param[out] cUidLen:the point of pcUid length
	*@Param[out] pcATR:the point of card reset data
	*@Param[out] pcATRLen:the length of cpu card atq data
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFResetTypeA(unsigned char *cardType, unsigned char *pcUid, unsigned char *cUidLen, unsigned char *pcATR, unsigned char *pcATRLen);

	/**
	*@brief: rats typeA cpu card
	*@Param[out] pcATR:the point of card reset data
	*@Param[out] pcInfoLen:the length of cpu card atq data
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFRatsTypeA(unsigned char *pcATR, unsigned char *pcATRLen);

	/*
	*@brief: NTAG21X  get version, version[6], 0x0F-213, 0x11-215, 0x13-216
	*@param[out] version:point of version information
	*@param[out] vLen:length of version information
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFNtagGetVersion(unsigned char *version, unsigned char *vLen);

	/**
	*@brief:NTAG21X authentic 
	*@param[in] password:point of 4 bytes
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFNtagAuth(unsigned char *password);

	/**
	*@brief:NTAG21X read 4 sectors 
	*@sector[in]:start sector,0~44,read 4 sector, 16 bytes
	*@pdata[out]:the point of read data
	*@dataLen[out]:the point of read data length 
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFNtagRead(unsigned char sector, unsigned char *pdata, unsigned char *dataLen);

	/**
	*@brief: NTAG21X write one sector 
	*@param[in] sector:the sector index,2~44 sectors are allowed.
	*@param[in] pdata:point of 4 bytes to write
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFNtagWrite(unsigned char sector, unsigned char *pdata);
	/**
	*@brief: Halt 14443A card
	*@return:0->success,others->failure
	*/
	EXPORT_HF_API int HFHaltTypeB(void);

	/**
	*@brief: reset type B card
	*@Param[out] pcInfo:the point of receive command
	*@Param[out] pcInfoLen:the length of received
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFResetTypeB(unsigned char *pcInfo, unsigned char *pcInfoLen);


	/**
	*@brief: get pcUid of type B card
	*@Param[out] pcUid:the point of type B card pcUid
	*@Param[out] cUidLen:the length of pcUid
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFGetUidTypeB(unsigned char *pcUid, unsigned char *cUidLen);

	/**
	*@brief: execute cpu command
	*@Param[in] pcInCos:the point of send command
	*@Param[in] cInLen:the length of send command
	*@Param[out] pcOutCos:the point of receive command
	*@Param[out] pcOutLen:the length of received
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFCpuCommand(unsigned char *pcInCos, unsigned char cInLen, unsigned char *pcOutCos, unsigned char *pcOutLen);

	/**
	*@brief: 15693 inventory
	*@Param[in] cMode:inventory cMode,0~3
	*@Param[in] AFI:AFI value
	*@Param[out] pcData:the point of receive data
	*@Param[out] dataLen:the length of received
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693Inventory(unsigned char cMode, unsigned char AFI, unsigned char *pcData, unsigned char *dataLen);

	/**
	*@brief: ISO15693 stay quite
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693StayQuite(void);

	/**
	*@brief: ISO15693 read
	*@Param[in] cMode:less than 10
	*@Param[in] pcUid:the point of pcUid
	*@Param[in] cUidLen:the length of pcUid
	*@Param[in] iStartBlock:start address of cBlock
	*@Param[in] cBlockNum:cBlock number
	*@Param[out] pData:the point of receive data
	*@Param[out] pDataLen:the length of received data
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693Read(unsigned char cMode, unsigned char *pcUid, int cUidLen, int iStartBlock, int cBlockNum, unsigned char *pData, unsigned char *pDataLen);

	/**
	*@brief: ISO15693 Write
	*@Param[in] cMode:less than 10
	*@Param[in] pcUid:the point of pcUid
	*@Param[in] cUidLen:the length of pcUid
	*@Param[in] iStartBlock:address cBlock start
	*@Param[in] cBlockNum:cBlock number
	*@Param[in] pwData:the point of write data
	*@Param[in] wLen:the length of write data
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693Write(unsigned char cMode, unsigned char *pcUid, int cUidLen, int iStartBlock, int cBlockNum, unsigned char *pwData, unsigned char wLen);

	/**
	*@brief: ISO15693 Lock cBlock
	*@Param[in] cMode:less than 10
	*@Param[in] pcUid:the point of pcUid
	*@Param[in] cUidLen:the length of pcUid
	*@Param[in] iStartBlock:address cBlock start
	*@Param[in] cBlockNum:cBlock number
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693LockBlock(unsigned char cMode, unsigned char *pcUid, unsigned char cUidLen, int iStartBlock, unsigned char cBlockNum);

	/**
	*@brief: ISO15693 select card
	*@Param[out] pcInfo:the point of card information 
	*@Param[out] pcInfoLen:the point of card information length
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693Select(unsigned char *pcInfo, unsigned char *pcInfoLen);

	/**
	*@brief: ISO15693 reset to ready
	*@Param[out] pcInfo:the point of card information 
	*@Param[out] pcInfoLen:the point of card information length
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693ResetReady(unsigned char *pcInfo, unsigned char *pcInfoLen);

	/**
	*@brief: ISO15693 Get card system information 
	*@Param[in] cMode:less than 10
	*@Param[in] pcUid:the point of pcUid
	*@Param[in] cUidLen:the length of pcUid
	*@Param[out] pcInfo:the point of system infoemation 
	*@Param[out] pcInfoLen:the point of system information length
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693GetSystemInfo(unsigned char cMode, unsigned char *pcUid, unsigned char cUidLen, unsigned char *pcInfo, unsigned char *pcInfoLen);

	/**
	*@brief: ISO15693 Write AFI
	*@Param[in] cMode:less than 10
	*@Param[in] pcUid:the point of pcUid
	*@Param[in] cUidLen:the length of pcUid
	*@Param[in] cAFI:AFI value
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693WriteAFI(unsigned char cMode, unsigned char *pcUid, int cUidLen, unsigned char cAFI);

	/**
	*@brief: ISO15693 Lock AFI
	*@Param[in] cMode:less than 10
	*@Param[in] pcUid:the point of pcUid
	*@Param[in] cUidLen:the length of pcUid
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693LockAFI(unsigned char cMode, unsigned char *pcUid, unsigned char cUidLen);

	/**
	*@brief: ISO15693 write DSFID
	*@Param[in] cMode:less than 10
	*@Param[in] pcUid:the point of pcUid
	*@Param[in] cUidLen:the length of pcUid
	*@Param[in] cDSFID:DSFID value
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693WriteDsfid(unsigned char cMode, unsigned char *pcUid, unsigned char cUidLen, unsigned char cDSFID);

	/**
	*@brief: ISO15693 Lock DSFID
	*@Param[in] cMode:less than 10
	*@Param[in] pcUid:the point of pcUid
	*@Param[in] cUidLen:the length of pcUid
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693LockDSFID(unsigned char cMode, unsigned char *pcUid, unsigned char cUidLen);

	/**
	*@brief: ISO15693 get multiple blocks
	*@Param[in] cMode:less than 10
	*@Param[in] pcUid:the point of pcUid
	*@Param[in] cUidLen:the length of pcUid
	*@Param[in] iStartBlock:address cBlock start
	*@Param[out] cBlockNum:cBlock number
	*@Param[out] pcData:the point of receive data
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693GetMultipleBlock(unsigned char cMode, unsigned char *pcUid, unsigned char cUidLen, int iStartBlock, unsigned char cBlockNum, unsigned char *pcData, unsigned char *pcDataLen);

	/**
	*@brief: ISO15693 transfer command
	*@Param[in] pcInCmd:the point of command data 
	*@Param[in] cLen:command length
	*@Param[out] pcOutCmd:the point of out data
	*@Param[out] pcOutLen:the point of out data length
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HF15693TransferCmd(unsigned char *pcInCmd, unsigned char cLen, unsigned char *pcOutCmd, unsigned char *pcOutLen);


	/**
	*@brief: initial smart card module 
	*@Param[in] cSlotNum:slot number,value(0/1/2) 
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int SmartCardInit(unsigned char cSlotNum);

	/**
	*@brief: free smart card module 
	*@Param[in] cSlotNum:slot number,value(0/1/2) 
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int SmartCardFree(unsigned char cSlotNum);


	/**
	*@brief: reset smart card 
	*@Param[in] cSlotNum:slot number,value(0/1/2) 
	*@Param[out] pcATR:the point of out data
	*@Param[out] pcATRLen:the point of out data length
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int SmartCardReset(unsigned char cSlotNum, unsigned char *pcATR, unsigned char *pcATRLen);

	/**
	*@brief: smart card transfer command
	*@Param[in] cSlotNum:slot number,value(0/1/2) 
	*@Param[in] pcInCmd:the point of command data 
	*@Param[in] cLen:command length
	*@Param[out] pcOutCmd:the point of out data
	*@Param[out] pcOutLen:the point of out data length
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int SmartCardTransferCmd(unsigned char cSlotNum, unsigned char *pcInCmd, unsigned char cLen, unsigned char *pcOutCmd, unsigned char *pcOutLen);

	/**
	*@brief: read 4 pages data from FM11NT021,4 Bytes per page
	*@Param[in] pageAddr:address of page(00h~86h)
	*@Param[in] outData:the point of return bytes 
	*@Param[in] outLen:length of outData,should be 16
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int FM11NTRead(unsigned char pageAddr, unsigned char *outData, unsigned short *outLen);

	/**
	*@brief: read multiple pages data from FM11NT021,4 Bytes per page
	*@Param[in] startPageAddr:start address of page(00h~86h)
	*@Param[in] endPageAddr:start address of page,greater than  start address 
	*@Param[in] outData:the point of return bytes 
	*@Param[in] outLen:length of outData,should be 4*N
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int FM11NTReadFast(unsigned char startPageAddr, unsigned char endPageAddr, unsigned char *outData, unsigned short *outLen);

	/**
	*@brief: write one page data in FM11NT021,4 Bytes per page
	*@Param[in] pageAddr:address of page(00h~86h)
	*@Param[in] wData:the data to be written,4 bytes
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int FM11NTWritePage(unsigned char pageAddr, unsigned char *wData);

	/**
	*@brief: execute authentic in FM11NT021
	*@Param[in] password:4 bytes
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int FM11NTAuthen(unsigned char *password);
	/**
	*@brief: transceive command to hf module
	*@Param[in] sendCmd:point of send
	*@Param[in] sendLen:the length of send command 
	*@Param[in] recvData:the response data
	*@Param[out] recvLen:length of recvData
	*@Return:0->success,others->failure
	*/
	EXPORT_HF_API int HFCmdTransceive(unsigned char *sendCmd, unsigned short sendLen, unsigned char *recvData, unsigned short *recvLen);
}
#endif

