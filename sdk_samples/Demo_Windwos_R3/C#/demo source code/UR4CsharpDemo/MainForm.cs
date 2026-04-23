using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinForm_Test;
using System.Net;
using System.Threading;
using UHFAPP.RFID;
using UHFAPP.custom.authenticate;
using UHFAPP.custom.m775Authenticate;
using UHFAPP.barcode;
using UHFAPP.Entity;
using System.Net.Sockets;
using BLEDeviceAPI;
using System.Windows.Forms.VisualStyles;
using static UHFAPP.UHFAPI;
using System.Runtime.InteropServices;
using WindowsFormsControlLibrary1;
using System.Runtime.ConstrainedExecution;
using UHF_BLE;

namespace UHFAPP
{
    public partial class MainForm : BaseForm
    {
        public static int MODE = 1;//0:串口   1:网口    2:usb
        public static string ip = "";
        public static uint portData = 0;

        public delegate void DelegateOpen(bool open);
        public static event DelegateOpen eventOpen = null;

        public delegate void DelegateSwitchUI();
        public static event DelegateSwitchUI eventSwitchUI = null;

        public delegate void MainSizeChanged(FormWindowState state);
        public static event MainSizeChanged eventMainSizeChanged = null;
        public static int ComPort = 0;
        string strOpen= "  Open  ";
        string strClose = "  Close  ";

        private string currentFormName = "";
        private bool isOpen = false;
        public MainForm mainform = null;
        private ReadEPCForm readEPCForm = null;
        public static FormWindowState currState = FormWindowState.Normal;

        public bool isSearch = false;
        List<ReaderDeviceInfo> listIP = new List<ReaderDeviceInfo>();
        #region  OnDisconnect
        //step1：定义断开回调函数
        private void OnDisconnectCallback(int id)
        {
            System.Console.WriteLine("OnDisconnectCallback");
            if (!this.IsDisposed)
            {
                this.Invoke(new EventHandler(delegate {
                    disableControls(false);
                    toolStripOpen.Text = strOpen;
                    hIDModeToolStripMenuItem.Enabled = false;
                    isOpen = false;
                    if (eventOpen != null)
                    {
                        eventOpen(false);
                    }

                }));

            }
 
        }
        //step2：定义断开回调委托
        UHFAPP.UHFAPI.OnDisconnectCallback DisconnectCallback = null;

        #endregion


        public MainForm()
        {
            InitializeComponent();
            Common.isEnglish = !IsChineseSimple();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.IsMdiContainer = true;
            mainform = this;
            toolStripOpen.Text = "  Open  ";
            //SwitchShowUI();

            UHFAPP.IPConfig.IPEntity  ipEntity= IPConfig.getIPConfig();
            if (ipEntity != null)
            {
                txtPort.Text = ipEntity.Port.ToString();
                ipControl1.IpData = new string[] { ipEntity.Ip[0], ipEntity.Ip[1], ipEntity.Ip[2], ipEntity.Ip[3] };
            }
            //step3：给变量赋值
            DisconnectCallback = OnDisconnectCallback;

            Config config = Config.JsonToConfig();
            if (config != null)
            {
                if (config.mainForm.VisibleUsbExportDataMenuItem)
                {
                    uSBExportDataToolStripMenuItem.Visible = true;
                }
                if (config.mainForm.VisibleConnectMultipleDevicesMenuItem)
                {
                    MultiUR4ToolStripMenuItem.Visible= true;
                }
                if (config.mainForm.VisibleHidInputMenuItem)
                {
                    hIDModeToolStripMenuItem.Visible = true;
                }
                if (config.mainForm.VisibleBarcodeScanMenuItem)
                {
                    barcodeToolStripMenuItem.Visible = true;
                }

                if (config.mainForm.VisibleBaudrate) { 
                    lblBaudrate.Visible = true;
                    cmbBaudrate.Visible = true; 
                }
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            MenuItemScanEPC_Click(null,null);
            setComPort();
            disableControls(false);
            
            btnSearch_Click(null,null);
            LoadUI();
            combCommunicationMode.SelectedIndex = 1;
            if (eventMainSizeChanged != null)
            {
                eventMainSizeChanged(WindowState);
            }
        }

        public void ReadWriteTag(string tag,int bank)
        {
            Form form= ShowForm(new ReadWriteTagForm(),true);
            if (form != null)
            {
                if (form is ReadWriteTagForm)
                {
                    ((ReadWriteTagForm)form).SetTAG(isOpen,tag, bank);
                }
            }

        } 
          
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isSearch = false;
            UHFClose();
            if (eventOpen != null)
            {
                eventOpen(false);
            }
            Thread.Sleep(500);
        }
        private bool UHFClose()
        {

            if (toolStripOpen.Text.Trim() == strClose.Trim())
            {
                if (combCommunicationMode.SelectedIndex == 1)
                {
                    uhf.TcpDisconnect();
                    return true;
                }
                else if (combCommunicationMode.SelectedIndex == 0)
                {
                    return uhf.Close();
                }
                else
                {
                    uhf.CloseUsb();
                    return true;
                }
 

            }
            return false;
        }



        #region MenuItem_Click
        /// <summary>
        /// R/W
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemReadWriteTag_Click(object sender, EventArgs e)
        {
            ReadWriteTag("", 0);
        }
        /// <summary>
        /// Inventory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemScanEPC_Click(object sender, EventArgs e)
        {
            if (readEPCForm == null)
            {
                readEPCForm = new ReadEPCForm(isOpen, mainform);
            }
            Form form = ShowForm(readEPCForm, false);
        }
        /// <summary>
        /// Config
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = ShowForm(new ConfigForm(isOpen), false);
        }
        /// <summary>
        /// kill
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void killLockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(new Kill_LockForm(), true);
        }

        private void receiveEPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UHFClose())
            {
                disableControls(false);
                toolStripOpen.Text = strOpen;
                hIDModeToolStripMenuItem.Enabled = false;
                isOpen = false;
                if (eventOpen != null)
                {
                    eventOpen(false);
                }
            }
            ReadEPCForm.isVisible = false; 
            ReceiveEPC from = new ReceiveEPC();
            from.ShowDialog();
            ReadEPCForm.isVisible = true;
            //  ShowForm(new ReceiveEPC(), true);
        }
        /// <summary>
        /// getVerison
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uHFVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isOpen)
            {
                StringBuilder sb = new StringBuilder();
                frmWaitingBox f = new frmWaitingBox((obj, args) =>
                {
                    string hardwareV = uhf.GetHardwareVersion().Replace("\0", "");
                    string softWareV = uhf.GetSoftwareVersion().Replace("\0", "");
                    string mainboardVer = uhf.GetSTM32Version().Replace("\0", "");
                    string version = uhf.GetAPIVersion().Replace("\0", "");
                    //int id = uhf.GetUHFGetDeviceID();
                    if (!IsChineseSimple())
                    {
                        sb.Append("Hardware version:  ");
                        sb.Append(hardwareV);
                        sb.Append("\r\nFirmware  version:  ");
                        sb.Append(softWareV);
                        if (mainboardVer != "")
                        {
                            sb.Append("\r\nMainboard  version:  ");
                            sb.Append(mainboardVer);
                        }
                        //sb.Append("\r\nDevice ID:  ");
                        //sb.Append(id);
                    }
                    else
                    {
                        sb.Append("固件版本:  ");
                        sb.Append(softWareV);
                        sb.Append("\r\n硬件版本:  ");
                        sb.Append(hardwareV);
                        if (mainboardVer != "")
                        {
                            sb.Append("\r\n主板版本:  ");
                            sb.Append(mainboardVer);
                        }
                    }

                    if (version != null && version != "")
                    {
                        sb.Append("\r\nAPI Version:  ");
                        sb.Append(version);
                    }
                });
                f.ShowDialog(this);

                MessageBoxEx.Show(this, sb.ToString());

            }
        }

        /// <summary>
        /// Open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (toolStripOpen.Text == strOpen)
            {
                int type = combCommunicationMode.SelectedIndex;//0
                string msg = !IsChineseSimple() ? "connecting..." : "连接中...";

                object strCom = cmbComPort.SelectedItem;
                int baudrate = int.Parse(cmbBaudrate.Text);
                frmWaitingBox f = new frmWaitingBox((obj, args) =>
                {
                    bool result = false;
                    if (type == 0)
                    {
                        if (strCom != null)
                        {
                    
                            ComPort = int.Parse(strCom.ToString().Replace("COM", ""));
                            result = uhf.Open(ComPort, baudrate);
                          
                        }
                    }
                    else if (type == 1)
                    {
                        if (getIPAndPort())
                        {
                            result = uhf.TcpConnect(ip, portData);
                        }
                    }
                    else
                    {
                        result = uhf.OpenUsb();
                        UHFAPI.setOnDataReceived(onDataReceived);
                    }
                    //step4：设置回调
                    UHFAPI.SetDisconnectCallback(DisconnectCallback);

                    if (result)
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            toolStripOpen.Text = strClose;
                            isOpen = true;
                            if (eventOpen != null)
                            {
                                eventOpen(true);
                            }
                            enableControls();

                        }));

                    }
                    else
                    {
                        frmWaitingBox.message = "fail";
                        Thread.Sleep(1000);
                    }
                }, msg);
                f.ShowDialog(this);

            }
            else
            {
                if (UHFClose())
                {
                    disableControls(false);
                    toolStripOpen.Text = strOpen;
                    isOpen = false;
                    if (eventOpen != null)
                    {
                        eventOpen(false);
                    }
                }
            }

        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (isOpen)
            {
                frmWaitingBox f = new frmWaitingBox((obj, args) =>
                {
                    string Temperature = uhf.GetTemperature();
                    string temp = (!IsChineseSimple() ? "Temperature:" : "温度:") + Temperature + "℃";
                    frmWaitingBox.message = temp;
                    System.Threading.Thread.Sleep(1500);
                });
                f.ShowDialog(this);
            }
        }


        private void uHFUpgradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UHFUpgradeForm configForm = new UHFUpgradeForm(Common.isEnglish);
                configForm.StartPosition = FormStartPosition.CenterParent;
                configForm.ShowDialog();
                if (configForm.isConnect)
                {
                    if (toolStripOpen.Text == strOpen)
                    {
                        toolStripOpen.Text = strClose;
                        isOpen = true;
                        enableControls();
                        if (eventOpen != null)
                        {
                            eventOpen(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void MultiUR4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadEPCForm.isVisible = false;
            this.Hide();
            UHFClose();
            disableControls(false);
            toolStripOpen.Text = strOpen;
            isOpen = false;
            if (eventOpen != null)
            {
                eventOpen(false);
            }
            UHFAPP.MultiDevice.NetWork.API.MainForm f = new UHFAPP.MultiDevice.NetWork.API.MainForm();
            f.ShowDialog();
            this.Show();
        }

        private void SetR3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            UHFAPP.custom.SetR3Form f = new UHFAPP.custom.SetR3Form();
            f.ShowDialog();
            this.Show();
        }
        private void hFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            RFIDMainForm f = new RFIDMainForm();
            f.ShowDialog();
            this.Show();
        }

        private void hIDModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = ShowForm(new HidInputForm(), true);
            if (form != null)
            {
                if (form is HidInputForm)
                {
                    ((HidInputForm)form).openState(isOpen);
                }
            }
        }

        #endregion


        #region RS160 KeyDwon

        public delegate void KeyDownEventHandler(int keyCode);
        public static event KeyDownEventHandler keyDownEventHandler = null;

        public delegate void KeyUpEventHandler(int keyCode);
        public static event KeyUpEventHandler keyUpEventHandler = null;

        private UHFAPP.UHFAPI.OnDataReceived onDataReceived = DataReceived;
        private static void DataReceived(IntPtr pdata, short len)
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
                    case CELL_KEY_CODE:

                        Console.WriteLine("keycdoe = "+pcontent[0]);

                        if (pcontent[0] == 03)
                        {
                            int keyCode = pcontent[0];
                            if (keyDownEventHandler != null)
                            {
                                keyDownEventHandler(keyCode);
                            }
                        }
                        else if (pcontent[0] == 04)
                        {
                            int keyCode = pcontent[0];
                            if (keyUpEventHandler != null)
                            {
                                keyUpEventHandler(keyCode);
                            }
                        }
                        else if (pcontent[0] == 01)
                        {
                            int keyCode = pcontent[0];
                            if (keyDownEventHandler != null)
                            {
                                keyDownEventHandler(keyCode);
                            }
                        }
               
                        break;
                    default:
                        //  printf("unknow parameter:%d\n", type);
                        break;
                }
                index += contentLen;
            }
        }

        #endregion 


        #region Search

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string start = !IsChineseSimple() ? "Search" : "开始搜索";
            if (btnSearch.Text == start)
            {
                btnSearch.Text = !IsChineseSimple() ? "Stop" : "停止搜索";
                Thread thred = new Thread(new ThreadStart(search));
                thred.Start();
            }
            else
            {
                btnSearch.Text = !IsChineseSimple() ? "Search" : "开始搜索";
                isSearch = false;
                Thread.Sleep(500);
            }
        }

        private void search()
        {
            isSearch = true;
            UdpClient UDPrece = new UdpClient(new IPEndPoint(IPAddress.Any, 1111));
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            UDPrece.Client.ReceiveTimeout = 500;
            while (isSearch)
            {
                try
                {
                    byte[] buf = UDPrece.Receive(ref endpoint);
                    //string msg = Encoding.Default.GetString(buf);
                    if (buf != null && buf.Length >= 12)
                    {
                        //
                        byte[] macBytes = Utils.CopyArray(buf, 0, 6);
                        byte[] ipBytes = Utils.CopyArray(buf, 6, 4);

                        int port = ((buf[10] & 0xFF) << 8) | (buf[11] & 0xFF);

                        bool[] exists = new bool[1];
                        ReaderDeviceInfo info = new ReaderDeviceInfo(macBytes, ipBytes, port);
                        int index = CheckUtils.getInsertIndex(listIP, info, exists);
                        if (!exists[0])
                        {
                            listIP.Insert(index, info);
                            lvDevcies.Invoke(new EventHandler(delegate
                            {
                                ListViewItem lv = new ListViewItem();
                                lv.Text = (listIP.Count).ToString();

                                ListViewItem.ListViewSubItem itemIP = new ListViewItem.ListViewSubItem();
                                itemIP.Name = "IP";
                                itemIP.Text = info.ip + ":" + info.port;
                                lv.SubItems.Add(itemIP);

                                ListViewItem.ListViewSubItem itemPort = new ListViewItem.ListViewSubItem();
                                itemPort.Name = "MAC";
                                itemPort.Text = info.mac + "";
                                lv.SubItems.Add(itemPort);


                                ListViewItem.ListViewSubItem itemIPAndPort = new ListViewItem.ListViewSubItem();
                                itemIPAndPort.Name = "IPANDMAC";
                                itemIPAndPort.Text = info.ip + info.mac;
                                lv.SubItems.Add(itemIPAndPort);

                                lvDevcies.Items.Insert(index, lv);

                                for (int k = 0; k < lvDevcies.Items.Count; k++)
                                {
                                    lvDevcies.Items[k].Text = (k + 1) + "";
                                }

                            }));
                        }
                        else
                        {
                            listIP[index].lastTime = Environment.TickCount;
                        }

                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine("SearchNearbyDevicesForm ex=" + ex.Message);
                }



                int tempC = listIP.Count;
                for (int k = listIP.Count - 1; k >= 0; k--)
                {
                    if (Environment.TickCount - listIP[k].lastTime > 1000 * 30)
                    {
                        listIP.RemoveAt(k);
                    }
                }
                if (listIP.Count < tempC)
                {
                    if (lvDevcies.IsHandleCreated)
                    {

                        lvDevcies.Invoke(new EventHandler(delegate
                        {
                            for (int k = lvDevcies.Items.Count - 1; k >= 0; k--)
                            {
                                string ipAndMac = lvDevcies.Items[k].SubItems["IPANDMAC"].Text;
                                bool flag = false;
                                for (int m = listIP.Count - 1; m >= 0; m--)
                                {
                                    if (ipAndMac == listIP[m].ip + listIP[m].mac)
                                    {
                                        flag = true; ;
                                        break;
                                    }
                                }
                                if (!flag)
                                {
                                    lvDevcies.Items.RemoveAt(k);
                                }
                            }
                            for (int k = 0; k < lvDevcies.Items.Count; k++)
                            {
                                lvDevcies.Items[k].Text = (k + 1) + "";
                            }

                        }));
                    }
                }


            }

            try
            {
                UDPrece.Client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SearchUDPrece.Client.Close  ex=" + ex.Message);
            }


        }

        private void lvDevcies_DoubleClick(object sender, EventArgs e)
        {
            if (lvDevcies.SelectedItems.Count <= 0)
            {
                return;
            }
            MainForm mainForm = (MainForm)this.ParentForm;
            string ipAndPort = lvDevcies.SelectedItems[0].SubItems[1].Text;
            string[] ipdata = ipAndPort.Split(':');
            //  string ip = ipAndPort.Split(':')[0] ;
            //  string port = ipAndPort.Split(':')[1];  
            // mainForm.Connect(ip, port);
            if (toolStripOpen.Text != strOpen)
            {
                if (UHFClose())
                {
                    disableControls(false);
                    toolStripOpen.Text = strOpen;
                    hIDModeToolStripMenuItem.Enabled = false;
                    isOpen = false;
                    if (eventOpen != null)
                    {
                        eventOpen(false);
                    }
                }
            }

            combCommunicationMode.SelectedIndex = 1;
            txtPort.Text = ipdata[4];
            ipControl1.IpData = new string[] { ipdata[0], ipdata[1], ipdata[2], ipdata[3] };
            toolStripButton1_Click(null, null);
        }

        public void StopSearch()
        {
            btnSearch.Text = !IsChineseSimple() ? "Search" : "开始搜索";
            isSearch = false;
            Thread.Sleep(50);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listIP.Clear();
            lvDevcies.Items.Clear();
        }

        #endregion

        #region  UI
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            currState = WindowState;
            panel2.Height = this.Height - 128;
            //判断是否选择的是最小化按钮
            if (eventMainSizeChanged != null)
            {
                eventMainSizeChanged(WindowState);
            }
            // this.MdiParent = mainform;
            //  Form form = ShowForm(new ConfigForm(isOpen), false);
        }
        private void toolStripComboBox2_TextChanged(object sender, EventArgs e)
        {

            if (combCommunicationMode.SelectedIndex == 0)
            {
                setComPort();
                panel1.Visible = false;
                MODE = 0;
                cmbComPort.Visible = true;
                lblPortName.Visible = true;
                Config config = Config.JsonToConfig();
                if (config != null)
                {
                    if (config.mainForm.VisibleBaudrate)
                    {
                        lblBaudrate.Visible = true;
                        cmbBaudrate.Visible = true;
                    }
                }

                // SetR3ToolStripMenuItem.Visible = false;
                hFToolStripMenuItem.Visible = false;
                MenuItemReceiveEPC.Visible = true;
                hIDModeToolStripMenuItem.Visible = false;
             //  MultiUR4ToolStripMenuItem.Visible = true;
            }
            else if (combCommunicationMode.SelectedIndex == 1)
            {
                cmbComPort.Visible = false;
                lblPortName.Visible = false;
                cmbBaudrate.Visible = false;
                lblBaudrate.Visible = false;

                panel1.Visible = true;
                MODE = 1;

              // SetR3ToolStripMenuItem.Visible = false;
                hFToolStripMenuItem.Visible = false;
                MenuItemReceiveEPC.Visible = true;
                hIDModeToolStripMenuItem.Visible = false;
             //   MultiUR4ToolStripMenuItem.Visible = true;
            }
            else if (combCommunicationMode.SelectedIndex == 2)
            {
                MODE = 2;
                panel1.Visible = false;
                cmbComPort.Visible = false;
                lblPortName.Visible = false;
                cmbBaudrate.Visible = false;
                lblBaudrate.Visible = false;


               // SetR3ToolStripMenuItem.Visible = true;
                hFToolStripMenuItem.Visible = true;
                hIDModeToolStripMenuItem.Visible = true;
                MenuItemReceiveEPC.Visible = false;
            //   MultiUR4ToolStripMenuItem.Visible = false;
            }
        }
        private void SwitchShowUI()
        {
            if (Common.isEnglish)
            {
                toolStripStatusLabel1.Text = "";//"                                                         "+ "                                                          tip: 1. right key can copy the selected label.     2. double-click the selected label can jump to the r/w  UI.";
                MenuItemScanEPC.Text = "ReadEPC";
                MenuItemReadWriteTag.Text = "ReadWriteTag";
                configToolStripMenuItem.Text = "Configuration";
                killLockToolStripMenuItem.Text = "Kill-Lock";
                uHFVersionToolStripMenuItem.Text = "UHF Info";
                toolStripMenuItem1.Text = "Temperature";
                MenuItemReceiveEPC.Text = "UDP-ReceiveEPC";
                uHFUpgradeToolStripMenuItem.Text = "UHF Upgrade";

                toolStripLabel4.Text = "Mode";
                int index = combCommunicationMode.SelectedIndex;//记录上一次的选择记录
                combCommunicationMode.Items.Clear();
                combCommunicationMode.Items.Add("SerialPort");
                combCommunicationMode.Items.Add("network");
                combCommunicationMode.Items.Add("USB");
                combCommunicationMode.SelectedIndex = index;
                strOpen = "  Open  ";
                strClose = "  Close  ";
                MultiUR4ToolStripMenuItem.Text = "Connecting multiple devices";
            }
            else
            {
                toolStripStatusLabel1.Text = "                                                        "
                    + "                                                        提示：1.右键可以复制选中的标签    2.双击选中的标签可以跳转到读写界面";
                MenuItemScanEPC.Text = "盘点EPC";
                MenuItemReadWriteTag.Text = "读写标签";
                configToolStripMenuItem.Text = "配置";
                killLockToolStripMenuItem.Text = "锁标签";
                MenuItemReceiveEPC.Text = "UDP-ReceiveEPC";
                uHFVersionToolStripMenuItem.Text = "UHF信息";
                toolStripMenuItem1.Text = "温度";
                uHFUpgradeToolStripMenuItem.Text = "UHF固件升级";

                toolStripLabel4.Text = "通信方式";
                int index = combCommunicationMode.SelectedIndex;//记录上一次的选择记录
                combCommunicationMode.Items.Clear();
                combCommunicationMode.Items.Add("串口");
                combCommunicationMode.Items.Add("网络");
                combCommunicationMode.Items.Add("USB");
                combCommunicationMode.SelectedIndex = index;
                strOpen = " 打开 ";
                strClose = " 关闭 ";
                MultiUR4ToolStripMenuItem.Text = "连接多台UR4";
            }

            if (toolStripOpen.Text.Trim() == "Open" || toolStripOpen.Text.Trim() == "打开")
                toolStripOpen.Text = strOpen;
            else
                toolStripOpen.Text = strClose;

        }

        public void enableControls()
        {
            uSBExportDataToolStripMenuItem.Enabled = true;
            MenuItemScanEPC.Enabled = true;
            MenuItemReadWriteTag.Enabled = true;
            configToolStripMenuItem.Enabled = true;
            uHFVersionToolStripMenuItem.Enabled = true;
            killLockToolStripMenuItem.Enabled = true;
            toolStripMenuItem1.Enabled = true;
            uHFUpgradeToolStripMenuItem.Enabled = true;
            SetR3ToolStripMenuItem.Enabled = true;
            hFToolStripMenuItem.Enabled = true;
            hIDModeToolStripMenuItem.Enabled = true;
            marginReadToolStripMenuItem.Enabled = true;
            authenticateToolStripMenuItem.Enabled = true;
            barcodeToolStripMenuItem.Enabled = true;

            combCommunicationMode.Enabled = false;
            cmbComPort.Enabled = false;
            panel1.Enabled = false;

        }
        public void disableControls(bool isInventory)
        {
            uSBExportDataToolStripMenuItem.Enabled = false;
            MenuItemScanEPC.Enabled = false;
            MenuItemReadWriteTag.Enabled = false;
            configToolStripMenuItem.Enabled = false;
            uHFVersionToolStripMenuItem.Enabled = false;
            killLockToolStripMenuItem.Enabled = false;
            toolStripMenuItem1.Enabled = false;
            uHFUpgradeToolStripMenuItem.Enabled = false;
            SetR3ToolStripMenuItem.Enabled = false;
            hFToolStripMenuItem.Enabled = false;
            hIDModeToolStripMenuItem.Enabled = false;
            marginReadToolStripMenuItem.Enabled = false;
            authenticateToolStripMenuItem.Enabled= false;
            barcodeToolStripMenuItem.Enabled = false;

            combCommunicationMode.Enabled = !isInventory;
            cmbComPort.Enabled = !isInventory;
            panel1.Enabled = !isInventory;

        }
        private void menuStrip1_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            if (e.Item.Text.Length == 0             //隐藏子窗体图标
              || e.Item.Text == "最小化(&N)"      //隐藏最小化按钮
              || e.Item.Text == "还原(&R)"           //隐藏还原按钮
              || e.Item.Text == "关闭(&C)")         //隐藏关闭按钮
            {
                e.Item.Visible = false;
            }
        }
        private bool getIPAndPort()
        {
            if (txtPort.Text == "")
            {
                MessageBox.Show("fail!");
                return false;
            }
            char[] port = txtPort.Text.ToCharArray();
            for (int k = 0; k < port.Length; k++)
            {
                if (port[k] != '0' && port[k] != '1' && port[k] != '2' && port[k] != '3' && port[k] != '4' &&
                    port[k] != '5' && port[k] != '6' && port[k] != '7' && port[k] != '8' && port[k] != '9')
                {

                    MessageBox.Show(!IsChineseSimple() ? "The port number can only be entered as a number!" : "端口号只能输入数字!");
                    return false;
                }
            }
            string[] tempIp = ipControl1.IpData;
            StringBuilder sb = new StringBuilder();
            sb.Append(tempIp[0]);
            sb.Append(".");
            sb.Append(tempIp[1]);
            sb.Append(".");
            sb.Append(tempIp[2]);
            sb.Append(".");
            sb.Append(tempIp[3]);
            ip = sb.ToString();
            portData = uint.Parse(txtPort.Text);


            UHFAPP.IPConfig.IPEntity entity = new IPConfig.IPEntity();
            entity.Port = (int)portData;
            entity.Ip = tempIp;
            IPConfig.setIPConfig(entity);

            return true;
        }

        //设置串口
        private void setComPort()
        {
            cmbComPort.Text = "";
            string[] ArryPort = System.IO.Ports.SerialPort.GetPortNames();
            cmbComPort.Items.Clear();
            for (int i = 0; i < ArryPort.Length; i++)
            {
                cmbComPort.Items.Add(ArryPort[i]);
            }
            if (cmbComPort.Items.Count > 0)
                cmbComPort.SelectedIndex = cmbComPort.Items.Count - 1;

        }

        public Form ShowForm(Form nextForm, bool isCache)
        {
            isCache = false;

            toolStripStatusLabel1.Visible = false;
            Form currForm = this.ActiveMdiChild;
            Form from = nextForm;
            if (currForm != null)
            {
                if (currForm.Name == from.Name)
                {
                    return null;
                }

                if (currForm.Name != "ReadEPCForm")// (currForm.Name == "ReadEPCForm" || currForm.Name == "ConfigForm")
                {
                    //Common.SaveForm(currForm);
                    currForm.Close();
                }
                else
                {
                    currForm.Hide();
                    // from = Common.GetForm(nextForm.GetType().Namespace, nextForm.Name, this);
                }
            }

            from.WindowState = FormWindowState.Maximized;
            from.MdiParent = this;//设置当前窗体为子窗体的父窗体
            from.AutoScaleMode = AutoScaleMode.Inherit;
            if (from.Name != "ReadEPCForm")// (currForm.Name == "ReadEPCForm" || currForm.Name == "ConfigForm")
            {
                from.Left = 303;
            }
            else
            {
                if (from.Left != -8)
                {
                    from.Left = 303;
                }
            }

            from.Show();//显示窗体
            return from;
        }

        private void LoadUI()
        {
            if (!IsChineseSimple())
            {
                combCommunicationMode.Items.Clear();
                combCommunicationMode.Items.Add("SerialPort");
                combCommunicationMode.Items.Add("network");
                combCommunicationMode.Items.Add("USB");
                btnSearch.Text = "Search";
                button1.Text = "Clear";
                toolStripStatusLabel1.Text = "";//"                                                         "+ "                                                          tip: 1. right key can copy the selected label.     2. double-click the selected label can jump to the r/w  UI.";
                MenuItemScanEPC.Text = "ReadEPC";
                MenuItemReadWriteTag.Text = "ReadWriteTag";
                configToolStripMenuItem.Text = "Configuration";
                killLockToolStripMenuItem.Text = "Kill-Lock";
                uHFVersionToolStripMenuItem.Text = "UHF Info";
                toolStripMenuItem1.Text = "Temperature";
                MenuItemReceiveEPC.Text = "UDP-ReceiveEPC";
                uHFUpgradeToolStripMenuItem.Text = "UHF Upgrade";
                toolStripLabel4.Text = "Mode";
                strOpen = "  Open  ";
                strClose = "  Close  ";
                MultiUR4ToolStripMenuItem.Text = "Connect multiple devices";
            }
            else
            {
                toolStripStatusLabel1.Text = "                                                        "
                    + "                                                        提示：1.右键可以复制选中的标签    2.双击选中的标签可以跳转到读写界面";
                MenuItemScanEPC.Text = "盘点EPC";
                MenuItemReadWriteTag.Text = "读写标签";
                configToolStripMenuItem.Text = "配置";
                killLockToolStripMenuItem.Text = "锁标签";
                MenuItemReceiveEPC.Text = "UDP-ReceiveEPC";
                uHFVersionToolStripMenuItem.Text = "UHF信息";
                toolStripMenuItem1.Text = "温度";
                uHFUpgradeToolStripMenuItem.Text = "UHF固件升级";
                toolStripLabel4.Text = "通信方式";
                int index = combCommunicationMode.SelectedIndex;//记录上一次的选择记录
                combCommunicationMode.Items.Clear();
                combCommunicationMode.Items.Add("串口");
                combCommunicationMode.Items.Add("网络");
                combCommunicationMode.Items.Add("USB");
                combCommunicationMode.SelectedIndex = index;
                MultiUR4ToolStripMenuItem.Text = "连接多台UR4";
            }
            cmbBaudrate.SelectedIndex = 0;
        }

        #endregion

        private void uSBExportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //ExportDataForm f = new ExportDataForm();
            //f.ShowDialog();
            //this.Show();
            ShowForm(new ExportDataForm(), false);
        }
        //MarginRead - Protected And ShortRange
        private void marginReadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(new MarginReadForm(), true);
        }

        private void barcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            BarcodeForm f = new BarcodeForm();
            f.ShowDialog();
            this.Show();
        }

        private void authenticateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(new UCODE_DNA_Form(), true);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void combCommunicationMode_Click(object sender, EventArgs e)
        {
          
        }

        private void cmbComPort_Click(object sender, EventArgs e)
        {
            setComPort();
        }

        private void marginReadToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ShowForm(new MarginReadForm(), true);
        }
    }
}
