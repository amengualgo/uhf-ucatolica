using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLEDeviceAPI;
using System.Runtime.InteropServices;

namespace UHFAPP.barcode
{
    public partial class HidInputForm : Form
    {
        //UHFAPI uhf = UHFAPI.getInstance();
        static UHFAPP.UHFAPI.OnDataReceived ondataReceived = null;
        const byte CELL_INVALID=0;
        const byte CELL_CONNECT_ID = 1;
        const byte CELL_CONNECT_IP = 2;
        const byte CELL_UHF_PC = 3;
        const byte CELL_UHF_RSSI = 4;
        const byte CELL_UHF_ANTENNA = 5;
        const byte CELL_UHF_EPC = 6;
        const byte CELL_UHF_TID = 7;
        const byte CELL_UHF_USER = 8;
        const byte CELL_UHF_RESERVE = 9;
        const byte CELL_BARCODE = 10;
        const byte CELL_UHF_SENSOR = 11;
        static int Format = 0;
        
        public HidInputForm()
        {
            InitializeComponent();
            cmbFormat.SelectedIndex = 1;

          
           
           // ondataReceived = null;
          // UHFAPP.UHFAPI.OnDataReceived  ondataReceived = new UHFAPI.OnDataReceived(DataReceived);
      
         
        }
        //[MarshalAs(UnmanagedType.LPArray, SizeConst = 4096)] byte[]
        public static void DataReceived(IntPtr pdata, short len)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " DataReceived begin");
            short contentLen;
            short index = 0;
            byte type;

            byte[] cellData = new byte[len];
            Marshal.Copy(pdata, cellData, 0, len);
          // cellData= Utils.CopyArray(pdata,0,len);

            byte[] pcontent;
           // printf("OnReceivedData:");
            for (int i = 0; i < len; i++)
            {
                // Console.WriteLine("%02X", cellData[i]);
            }
            byte[] temp = null;
            string str;
            //  printf("\n");
            while (index < len) 
            {
	            type = cellData[index++];
                if ((cellData[index] & 0x80) == 0x80)
	            {
                    contentLen = (short)(((cellData[index] & 0x7F) << 7) | (cellData[index + 1] & 0x7F));
		            index += 2;
	            }
	            else
	            {
		            contentLen = cellData[index++];
	            }

                pcontent = Utils.CopyArray(cellData, index, contentLen);
	            switch (type)
                {
                case CELL_UHF_PC:
                   // printf("PC:%02X%02X\n", pcontent[0], pcontent[1]);
                    break;
                case CELL_UHF_RSSI:
                   // printf("RSSI:%02X%02X\n", pcontent[0], pcontent[1]);
                    break;
                case CELL_UHF_ANTENNA:
                    //  printf("Antenna:%d\n", pcontent[0]);
                    break;
                case CELL_UHF_EPC:
                    //UHFAPI.PrintTextToCursor(System.Text.ASCIIEncoding.GetEncoding("GBK").GetBytes("你好你好"));
                   // UHFAPI.PrintTextToCursor(pcontent);
                    //  printf("EPC:");
                    for (int i = 0; i < contentLen; i++)
                    {
                        //  printf("%02X", pcontent[i]);
                    }
                    //   printf("\n");

                    temp = Utils.CopyArray(pcontent, 0, contentLen);
                    str= DataConvert.ByteArrayToHexString(temp).Replace(" ", "")+"\n";
                    temp= System.Text.ASCIIEncoding.ASCII.GetBytes(str);

                     UHFAPI.PrintTextToCursor(Format, temp, (short)temp.Length);
                    break;
                case CELL_UHF_TID:
                    //  printf("TID:");
                    for (int i = 0; i < contentLen; i++)
                    {
                        //   printf("%02X", pcontent[i]);
                    }
                    //   printf("\n");
                     temp = Utils.CopyArray(pcontent, 0, contentLen);
                      str = DataConvert.ByteArrayToHexString(temp).Replace(" ", "") + "\n";
                    temp = System.Text.ASCIIEncoding.ASCII.GetBytes(str);

                    UHFAPI.PrintTextToCursor(Format, temp, (short)temp.Length);
                break;
                case CELL_UHF_USER:
                //  printf("User:");
                    for (int i = 0; i < contentLen; i++)
                    {
                        //   printf("%02X", pcontent[i]);
                    }
                    // printf("\n");
                    temp = Utils.CopyArray(pcontent, 0, contentLen);
                    str = DataConvert.ByteArrayToHexString(temp).Replace(" ", "") + "\n";
                    temp = System.Text.ASCIIEncoding.ASCII.GetBytes(str);

                    UHFAPI.PrintTextToCursor(Format, temp, (short)temp.Length);
                    break;
                case CELL_UHF_RESERVE:
                    //  printf("Reserve:");
                    for (int i = 0; i < contentLen; i++)
                    {
                        //   printf("%02X", pcontent[i]);
                    }
                    //  printf("\n");
                    break;
                case CELL_BARCODE:
                    //  printf("Barcode:");
                    for (int i = 0; i < contentLen; i++)
                    {
                        //   printf("%02X", pcontent[i]);
                    }
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " barcode");
                    UHFAPI.PrintTextToCursor(Format, Utils.CopyArray(pcontent, 0, contentLen), contentLen);
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " barcode end");
                    //   printf("\n");
                    break;
                default:
                    //  printf("unknow parameter:%d\n", type);
                    break;
                }
	            index += contentLen;
            }
        }
     //  ENUM_CHAR_CODE_ANSI=0
        //ENUM_CHAR_CODE_UTF8
   


        
        void MainForm_eventOpen(bool open)
        {
            if (open)
            {
                ondataReceived = DataReceived;
                UHFAPI.setOnDataReceived(ondataReceived);
                Format = cmbFormat.SelectedIndex;
                UHFAPI.getInstance().GetSoftwareVersion();
            }
            else
            {
                ondataReceived = null;
            }
        }

        private void HidInputForm_VisibleChanged(object sender, EventArgs e)
        {
            if (((HidInputForm)sender).Visible)
            {
        
                MainForm.eventOpen += MainForm_eventOpen;
            
            }
            else
            {
                UHFAPI.setOnDataReceived(null);
                MainForm.eventOpen -= MainForm_eventOpen;
            }
        }

        public void openState(bool isOpen)
        {
            if (isOpen && ondataReceived==null)
            {
                ondataReceived = DataReceived;
                UHFAPI.setOnDataReceived(ondataReceived);
            }

        }

        private void cmbFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Format = cmbFormat.SelectedIndex;
        }
    }
}
