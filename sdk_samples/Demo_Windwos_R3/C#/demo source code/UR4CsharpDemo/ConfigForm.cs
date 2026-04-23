using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinForm_Test;
using System.Threading;
using System.Collections;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using UHFAPP.Entity;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Net;
using BLEDeviceAPI;

namespace UHFAPP
{
    public partial class ConfigForm : BaseForm
    {
        static bool isDebug = false;
        bool isAllPower = true;
        bool isFlag = false;
        public ConfigForm()
        {
            InitializeComponent();
        }
        public ConfigForm(bool isOpen)
        {
            InitializeComponent();
            if (isOpen)
            {
                GetAllConfig();
                panel1.Enabled = true;
            }
            else
            {
                panel1.Enabled = false;
            }

            //if (isAllPower)
            //{
            //    groupBox12.Location = new Point(9, 9);
            //    groupBox12.Width = 406;
            //    groupBox12.Height = 170;
            //    groupBox12.Visible = true;
            //    groupBox6.Visible = false;
            //}
        }

        void MainForm_eventOpen(bool open)
        {
            if (open)
            {
                GetAllConfig();
                panel1.Enabled = true;
                if (MainForm.MODE == 2)
                {
                    gbAnt.Enabled = true;
                    gbIP.Enabled = true;
                    gbIp2.Enabled = false;
                    bgUR4.Enabled = false;
                    gbWorkMode.Enabled = false;
                    groupBox25.Enabled = false;

                    groupBox16.Visible = false;
                    groupBox1.Visible = false;
                //    button3.Visible = false;
                  //  gbIP.Visible = false;
                    gbIp2.Visible = false;
                    gbWorkMode.Visible = false;
                    gbAnt.Visible = true;
                    bgUR4.Visible = false;
                    //btnReset.Visible = false;
                
                    groupBox19.Visible = false;
                    groupBox20.Visible = false;
                    //groupBox18.Visible = false;
                    //   groupBox7.Location = new Point(426, 384);
                    //   groupBox18.Location = new Point(426, 500);
                    gbGPIO.Visible = false;
                    groupBox19.Visible= false;
                    groupBox20.Visible= false;

                    lblIPMode.Visible = true;
                    rbStatic.Visible = true;
                    rbDHCP.Visible = true;
                }
                else
                {
                    gbAnt.Enabled = true;
                    gbIP.Enabled = true;
                    gbIp2.Enabled = true;
                    bgUR4.Enabled = true;
                    gbWorkMode.Enabled = true;
                    groupBox25.Enabled = true;
          

                   // gbIP.Visible = true;
                    gbIp2.Visible = true;
                    groupBox7.Visible = true;
                    gbWorkMode.Visible = true;
                    gbAnt.Visible = true;
                    bgUR4.Visible = true;

                    groupBox16.Visible = true;
                    groupBox1.Visible = true;
                    button3.Visible = true;

                
                    groupBox18.Visible = true;

                    groupBox19.Visible = true;
                    groupBox20.Visible = true;
                    //   groupBox7.Location = new Point(426, 566);
                    //   groupBox18.Location = new Point(426, 687);
                    gbGPIO.Visible = true;
                    groupBox19.Visible = true;
                    groupBox20.Visible= true;

                   // lblIPMode.Visible = false;
                   // rbStatic.Visible = false;
                   // rbDHCP.Visible = false;
                }
            }
            else
            {
                panel1.Enabled = false;
            }
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            //lblIPMode.Visible = true;
            //rbStatic.Visible = true;
            //rbDHCP.Visible = true;



            MainForm.eventMainSizeChanged += MainForm_SizeChanged;

            MainForm.eventOpen += MainForm_eventOpen;
            cmbLinkFrequency.SelectedIndex = 3;
 
            comboBox1.SelectedIndex = 0;
            cmbOutStatus.SelectedIndex = 0;
            cmbInput.SelectedIndex = 0;
            comRM.SelectedIndex = 0;
            cmbO2.SelectedIndex = 0;
            cmbO1.SelectedIndex = 0;
            cmbOutput1.SelectedIndex = 0;
            cmbOutput2.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
            comboBox8.SelectedIndex = 0;
            comboBox9.SelectedIndex = 0;
            comboBox19.SelectedIndex = 0;

            comboBox14.SelectedIndex = 0;
            comboBox15.SelectedIndex = 0;
            comboBox16.SelectedIndex = 0;
            comboBox17.SelectedIndex = 0;
            comboBox20.SelectedIndex = 0;
            cmbAntNumber.SelectedIndex = 0;

            gbGPIO.Size = new Size(373, 260);
            gbUR1A.Location = bgUR4.Location;
            gbWYD.Location = bgUR4.Location;
            gbDEV.Location = bgUR4.Location;

            if (MainForm.MODE == 2)
            {
                gbAnt.Enabled = true;
                gbIP.Enabled = true;
                gbIp2.Enabled = false;
                bgUR4.Enabled = false;
                gbWorkMode.Enabled = false;
                groupBox25.Enabled = false;
                //  btnReset.Enabled = false;
               // groupBox18.Visible = false;
               // gbIP.Visible = false;
                gbIp2.Visible = false;
                gbWorkMode.Visible = false;
                gbAnt.Visible = true;
                bgUR4.Visible = false;
               // btnReset.Visible = false;
         
              //  groupBox7.Location = new Point(426, 384);
              //  groupBox18.Location = new Point(426,500);

                groupBox16.Visible = false;
                groupBox1.Visible = false;
              //  button3.Visible = false;
                gbGPIO.Visible = false;
                groupBox19.Visible = false;
                groupBox20.Visible = false;

                lblIPMode.Visible = true;
                rbStatic.Visible = true;
                rbDHCP.Visible = true;
            }
            else
            {
                gbAnt.Enabled = true;
                gbIP.Enabled = true;
                gbIp2.Enabled = true;
                bgUR4.Enabled = true;
                gbWorkMode.Enabled = true;
                groupBox25.Enabled = true;
              
                groupBox18.Visible = true; 
              //  gbIP.Visible = true;
                gbIp2.Visible = true;
                groupBox7.Visible = true;
                gbWorkMode.Visible = true;
                gbAnt.Visible = true;
                bgUR4.Visible = true;
              
              
              //  groupBox7.Location = new Point(426, 566); 
              //  groupBox18.Location = new Point(426, 687);

                groupBox16.Visible = true;
                groupBox1.Visible = true;
                button3.Visible = true;
                gbGPIO.Visible = true;
                groupBox19.Visible = true;
                groupBox20.Visible= true;

               // lblIPMode.Visible = false;
               // rbStatic.Visible = false;
               // rbDHCP.Visible = false;
            }
            LoadUI();
            LoadRegionConfig();
            LoadFrequencyPoint();

            Config config = Config.JsonToConfig();
            if (config != null)
            {
                if (config.configForm.VisibleEthernetDHCP)
                {
                    lblIPMode.Visible = true;
                    rbStatic.Visible = true;
                    rbDHCP.Visible = true;
                }
                if (config.mainForm.VisibleBaudrate)
                {
                    gbBaudrate.Visible = true;
                }
                if (config.configForm.VisibleCalibration)
                {
                    groupBox25.Visible = true;
                }
                if (config.configForm.VisibleEthernetMAC)
                {
                    gbMAC.Visible = true;
                }
                if (config.configForm.VisibleEpcReserved)
                {
                    lblPWD.Visible = true;
                    txtPWD.Visible = true;
                    txtUserPtr.Text = 2+"";
                    txtUserPtr.Text = 2+"";
                    cbInventoryMode.Items.Add("EPC+RESERVED");
                }
                if (config.configForm.VisibleLBT)
                {
                    gbLBT.Visible = true;
                }
            }
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.eventMainSizeChanged -= MainForm_SizeChanged;

            MainForm.eventOpen -= MainForm_eventOpen;
     
        }
       #region 获取所有参数
        private void GetAllConfig()
        {
            string msg = "waiting...";
            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                int errorCount = 0;
                byte power = 0;

                byte region = 0;
                if (uhf.GetRegion(ref region))
                {
                    int index = GetRegionIndex(region);
                    if (index >= 0)
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            cmbRegion.SelectedIndex = index;
                        }));
                    }
                }
                else
                {
                    errorCount++;
                }
                if (errorCount >=1)
                {
                    return;
                }


                byte mode = 0;
                if (uhf.GetRFLink(ref mode))
                {
                    this.Invoke(new EventHandler(delegate
                   {
                       switch (mode)
                       {
                           case 0:
                           case 1:
                           case 2:
                           case 3:
                           case 4:
                           case 5:
                               cmbRFLink.SelectedIndex = mode;
                               break;
                           case 10:
                           case 11:
                           case 12:
                           case 13:
                           case 14:
                           case 15:
                               cmbRFLink.SelectedIndex = mode - 4;
                               break;
                       }
                   }));
                }
                else
                {
                    errorCount++;
                }

            

                this.Invoke(new EventHandler(delegate
                {
                    cbDedebug.Checked = isDebug;
                    btnGetAllPower_Click(null, null);
                }));

                byte Target = 0;
                byte Action = 0;
                byte T = 0;
                byte Q = 0;
                byte StartQ = 0;
                byte MinQ = 0;
                byte MaxQ = 0;
                byte D = 0;
                byte Coding = 0;
                byte P = 0;
                byte Sel = 0;
                byte Session = 0;
                byte G = 0;
                byte LF = 0;
                bool result = uhf.GetGen2(ref  Target, ref   Action, ref   T, ref   Q,
                         ref   StartQ, ref   MinQ,
                         ref   MaxQ, ref   D, ref   Coding, ref   P,
                         ref   Sel, ref   Session, ref   G, ref   LF);
                if (result)
                {
                    this.Invoke(new EventHandler(delegate
                        {
                            cmbTarget.SelectedIndex = Target;
                            cmbAction.SelectedIndex = Action;
                            cmbT.SelectedIndex = T;
                            cmbQ.SelectedIndex = Q;
                            cmbCoding.SelectedIndex = Coding;
                            cmbP.SelectedIndex = P;
                            cmbSel.SelectedIndex = Sel;
                            cmbStartQ.SelectedIndex = StartQ;
                            cmbMinQ.SelectedIndex = MinQ;
                            cmbMaxQ.SelectedIndex = MaxQ;
                            cmbDr.SelectedIndex = D;
                            cmbSession.SelectedIndex = Session;
                            cmbG.SelectedIndex = G;
                            cmbLinkFrequency.SelectedIndex = LF;
                        }));
                }

               
                byte flag = 0;
                if (uhf.GetTagfocus(ref flag))
                {
                    this.Invoke(new EventHandler(delegate
                     {
                         if (flag == 0)
                         {
                             rbTagfocusEnable.Checked = false;
                             rbTagfocusDisable.Checked = true;
                         }
                         else if (flag == 1)
                         {
                             rbTagfocusEnable.Checked = true;
                             rbTagfocusDisable.Checked = false;
                         }
                     }));
                }


                 byte flag1 = 0;
                 if (uhf.GetFastID(ref flag1))
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        if (flag1 == 0)
                        {
                            rbFastIDEnable.Checked = false;
                            rbFastIDDisable.Checked = true;
                        }
                        else if (flag1 == 1)
                        {
                            rbFastIDEnable.Checked = true;
                            rbFastIDDisable.Checked = false;
                        }
                    }));
                }


                byte[] mode3 = new byte[10];
                if (uhf.UHFGetBuzzer(mode3))
                {
                    this.Invoke(new EventHandler(delegate
                    {

                        if (mode3[0] == 0)
                        {

                            rbEnableBuzzer.Checked = false;
                            rbDisableBuzzer.Checked = true;
                        }
                        else if (mode3[0] == 1)
                        {
                            rbDisableBuzzer.Checked = false;
                            rbEnableBuzzer.Checked = true;
                        }
                    }));
                }

                byte userPtr = 0;
                byte userLen = 0;
                int mode4= uhf.getEPCTIDUSERMode(ref userPtr, ref userLen);
                this.Invoke(new EventHandler(delegate
               {
                   switch (mode4)
                   {
                       case 0:
                           cbInventoryMode.SelectedIndex = 0;
                           break;
                       case 1:
                           cbInventoryMode.SelectedIndex = 1;
                           break;
                       case 2:
                           cbInventoryMode.SelectedIndex = 2;
                           txtUserLen.Text = userLen + "";
                           txtUserPtr.Text = userPtr + "";
                           break;
                       default:
                           cbInventoryMode.SelectedIndex = -1;
                           break;
                   }
               }));



                if (MainForm.MODE != 2)
                {
                    byte[] ant = new byte[4];
                    if (uhf.GetANTTo32(ant))
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            cmbAnt16.Checked = (ant[0] & 128) == 128 ? true : false;
                            cmbAnt15.Checked = (ant[0] & 64) == 64 ? true : false;
                            cmbAnt14.Checked = (ant[0] & 32) == 32 ? true : false;
                            cmbAnt13.Checked = (ant[0] & 16) == 16 ? true : false;
                            cmbAnt12.Checked = (ant[0] & 8) == 8 ? true : false;
                            cmbAnt11.Checked = (ant[0] & 4) == 4 ? true : false;
                            cmbAnt10.Checked = (ant[0] & 2) == 2 ? true : false;
                            cmbAnt9.Checked = (ant[0] & 1) == 1 ? true : false;

                            cmbAnt8.Checked = (ant[1] & 128) == 128 ? true : false;
                            cmbAnt7.Checked = (ant[1] & 64) == 64 ? true : false;
                            cmbAnt6.Checked = (ant[1] & 32) == 32 ? true : false;
                            cmbAnt5.Checked = (ant[1] & 16) == 16 ? true : false;
                            cmbAnt4.Checked = (ant[1] & 8) == 8 ? true : false;
                            cmbAnt3.Checked = (ant[1] & 4) == 4 ? true : false;
                            cmbAnt2.Checked = (ant[1] & 2) == 2 ? true : false;
                            cmbAnt1.Checked = (ant[1] & 1) == 1 ? true : false;

                            if (ant.Length == 4)
                            {
                                cbANT32.Checked = (ant[2] & 128) == 128 ? true : false;
                                cbANT31.Checked = (ant[2] & 64) == 64 ? true : false;
                                cbANT30.Checked = (ant[2] & 32) == 32 ? true : false;
                                cbANT29.Checked = (ant[2] & 16) == 16 ? true : false;
                                cbANT28.Checked = (ant[2] & 8) == 8 ? true : false;
                                cbANT27.Checked = (ant[2] & 4) == 4 ? true : false;
                                cbANT26.Checked = (ant[2] & 2) == 2 ? true : false;
                                cbANT25.Checked = (ant[2] & 1) == 1 ? true : false;

                                cbANT24.Checked = (ant[3] & 128) == 128 ? true : false;
                                cbANT23.Checked = (ant[3] & 64) == 64 ? true : false;
                                cbANT22.Checked = (ant[3] & 32) == 32 ? true : false;
                                cbANT21.Checked = (ant[3] & 16) == 16 ? true : false;
                                cbANT20.Checked = (ant[3] & 8) == 8 ? true : false;
                                cbANT19.Checked = (ant[3] & 4) == 4 ? true : false;
                                cbANT18.Checked = (ant[3] & 2) == 2 ? true : false;
                                cbANT17.Checked = (ant[3] & 1) == 1 ? true : false;
                            }
                        }));
                    }

                    int startTime = Environment.TickCount;
                    StringBuilder sIP = new StringBuilder(20);
                    StringBuilder sPort = new StringBuilder(20);
                    StringBuilder mask = new StringBuilder(20);
                    StringBuilder gate = new StringBuilder(20);
                    bool[] dhcp=new bool[1]; 
                    if (uhf.GetReaderIP(sIP, sPort, mask, gate, dhcp))
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            ipControlLocal.IpData = sIP.ToString().Split('.');// txtLocalIP.Text = sIP.ToString();
                            txtLocalPort.Text = sPort.ToString();
                            ipGateway.IpData = gate.ToString().Split('.');
                            ipControlSubnetMask.IpData = mask.ToString().Split('.');
                            if (dhcp[0])
                            {
                                rbDHCP.Checked = true;
                                lblIPMode.Visible = true;
                                rbStatic.Visible = true;
                                rbDHCP.Visible = true;
                            }
                            else
                            {
                                rbStatic.Checked = true;
                            }
                        }));
                    }

                    sIP = new StringBuilder();
                    sPort = new StringBuilder();
                    if (uhf.GetDestIP(sIP, sPort))
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            ipControlDest.IpData = sIP.ToString().Split('.');
                            txtPortDest.Text = sPort.ToString();
                        }));
                    }
                    byte[] mode1 = new byte[2];
                    if (uhf.GetWorkMode(mode1))
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            if (mode1[0] == 0)
                            {
                                workMode.SelectedIndex = 0;
                            }
                            else if (mode1[0] == 1)
                            {
                                workMode.SelectedIndex = 1;
                            }
                            else if (mode1[0] == 2)
                            {
                                workMode.SelectedIndex = 2;
                            }
                            else if (mode1[0] == 3)
                            {
                                workMode.SelectedIndex = 3;
                            }
                            else if (mode1[0] == 4)
                            {
                                workMode.SelectedIndex = 4;
                            }
                        }));
                    }


                    if (uhf.GetFastInventory(ref flag))
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            if (flag >= 0 && flag <= 4)
                            {
                                cmbCR.SelectedIndex = flag;
                            }
                        }));
                    }

                }



            }, msg);
            f.ShowDialog(this);
        }
        public int GetRegionIndex(int region)
        {
            switch (region)
            {
                case 0x01:
                    return 0;
                case 0x02:
                    return 1;
                case 0x04:
                    return 2;
                case 0x08:
                    return 3;
                case 0x16:
                    return 4;
                case 0x32:
                    return 5;
                case 0x33:
                    return 6;
                case 0x34:
                    return 7;
                case 0x35:
                    return 8;
                case 0x36:
                    return 9;
                case 0x37:
                    return 10;
                case 0x3B:
                    return 11;
                case 0x3C:
                    return 12;
                case 0x3D:
                    return 13;
                case 0x3E:
                    return 14;
                case 0x3F:
                    return 15;
                case 0x40:
                    return 16;
                case 0x41:
                    return 17;
                case 0x42:
                    return 18;
                case 0x43:
                    return 19;
                case 0x44:
                    return 20;

            }
            return -1;
        }
        #endregion
        
        #region 功率
        private void btnPowerGet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            byte power =0;
            if (uhf.GetPower(ref power))
            {
                cmbPower_ANT1.SelectedIndex = power - 1;
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);
        }

        private void btnPowerSet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            if (cmbPower_ANT1.SelectedIndex >= 0)
            {
                byte power1 = (byte)(cmbPower_ANT1.SelectedIndex + 1);

                byte save = (byte)(cbPower.Checked?1:0);
                if (uhf.SetPower(save, power1))
                {
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                }

            }
            showMessage(msg);
        } 

        #endregion

        #region 工作频率
//917.1
//917.3
//917.5
//917.7
//917.9
//918.1
//918.3
//918.5
//918.7
//918.9
//919.1
//919.3
//919.5
//919.7
//919.9
//920.1
//920.3
//920.5
//920.7
//920.9
//921.1
//921.3
//921.5
//921.7
//921.9
//922.1
//922.3
//922.5
//922.7
//922.9
//923.1
//923.3

        private void btnWorkModeSet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            try
             {
                 if (comboBox1.Text != "")
                 {
                     string frequency = comboBox1.Text.Replace(".", "");
                     if (frequency.Length == 3) {
                         frequency = frequency + "000";
                     }
                     else if (frequency.Length == 4) {
                         frequency = frequency + "00";
                     }
                     else if (frequency.Length == 5)
                     {
                         frequency = frequency + "0";
                     }
                     if (StringUtils.IsNumber(frequency))
                     {
                         int[] ifrequency = new int[] { int.Parse(frequency) };
                         if (uhf.SetJumpFrequency(1, ifrequency))
                         {
                            msg = !IsChineseSimple() ? "Success" : "成功!";
                        }
                     }
                 }
             }
             catch (Exception ex) { 
             
             }

             showMessage(msg);
        }
   
        #endregion

        #region Gen2
        private void btnGen2Get_Click(object sender, EventArgs e)
        {
            byte Target = 0;
            byte Action = 0;
            byte T = 0;
            byte Q = 0;
            byte StartQ = 0;
            byte MinQ = 0;
            byte MaxQ = 0;
            byte D = 0;
            byte Coding = 0;
            byte P = 0;
            byte Sel = 0;
            byte Session = 0;
            byte G = 0;
            byte LF = 0;
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            int start = Environment.TickCount;

            bool result=uhf.GetGen2(ref  Target, ref   Action, ref   T, ref   Q,
                     ref   StartQ, ref   MinQ,
                     ref   MaxQ, ref   D, ref   Coding, ref   P,
                     ref   Sel, ref   Session, ref   G, ref   LF);
          //  MessageBox.Show("耗时：" + (Environment.TickCount-start));
            if (result)
            {
                cmbTarget.SelectedIndex = Target;
                cmbAction.SelectedIndex = Action;
                cmbT.SelectedIndex = T;
                cmbQ.SelectedIndex = Q;
                cmbCoding.SelectedIndex = Coding;
                cmbP.SelectedIndex = P;
                cmbSel.SelectedIndex = Sel;
                cmbStartQ.SelectedIndex = StartQ;
                cmbMinQ.SelectedIndex = MinQ;
                cmbMaxQ.SelectedIndex = MaxQ;
                cmbDr.SelectedIndex = D;
                cmbSession.SelectedIndex = Session;
                cmbG.SelectedIndex = G;
                cmbLinkFrequency.SelectedIndex = LF;
                msg = !IsChineseSimple() ? "Success" : "成功!";
                btnGen2Set.Enabled = true;
            }


            showMessage(msg);
        }
        private void btnGen2Set_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            try
            {
                byte Target =(byte) cmbTarget.SelectedIndex;
                byte Action = (byte)cmbAction.SelectedIndex;
                byte T = (byte)cmbT.SelectedIndex;
                byte Q = (byte)cmbQ.SelectedIndex;
                byte StartQ = (byte)cmbStartQ.SelectedIndex;
                byte MinQ = (byte)cmbMinQ.SelectedIndex;
                byte MaxQ = (byte)cmbMaxQ.SelectedIndex;
                byte D = (byte)cmbDr.SelectedIndex;
                byte Coding = (byte)cmbCoding.SelectedIndex;
                byte P = (byte)cmbP.SelectedIndex;
                byte Sel = (byte)cmbSel.SelectedIndex;
                byte Session = (byte)cmbSession.SelectedIndex;
                byte G = (byte)cmbG.SelectedIndex;
                byte LF = (byte)cmbLinkFrequency.SelectedIndex;
                if (uhf.SetGen2(Target, Action, T, Q, StartQ, MinQ, MaxQ, D, Coding, P, Sel, Session, G, LF))
                {
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                }
                
            }
            catch (Exception ex)
            {
               
            }
            showMessage(msg);
        }
        #endregion

        #region CW
        private void btnGetCW_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            if (uhf.SetCW(1))
            {
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg,100);
        }
        private void btnSetCW_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            if (uhf.SetCW(0))
            {
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
           showMessage(msg, 100);
        }
        #endregion

        #region 天线
        private void btnGetANT_Click(object sender, EventArgs e)
        {
  
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            byte[] ant = new byte[4];
            if (uhf.GetANTTo32(ant))
            {
                cmbAnt16.Checked = (ant[0] & 128) == 128 ? true : false;
                cmbAnt15.Checked = (ant[0] & 64) == 64 ? true : false;
                cmbAnt14.Checked = (ant[0] & 32) == 32 ? true : false;
                cmbAnt13.Checked = (ant[0] & 16) == 16 ? true : false;
                cmbAnt12.Checked = (ant[0] & 8) == 8 ? true : false;
                cmbAnt11.Checked = (ant[0] & 4) == 4 ? true : false;
                cmbAnt10.Checked = (ant[0] & 2) == 2 ? true : false;
                cmbAnt9.Checked = (ant[0] & 1) == 1 ? true : false;

                cmbAnt8.Checked = (ant[1] & 128) == 128 ? true : false;
                cmbAnt7.Checked = (ant[1] & 64) == 64 ? true : false;
                cmbAnt6.Checked = (ant[1] & 32) == 32 ? true : false;
                cmbAnt5.Checked = (ant[1] & 16) == 16 ? true : false;
                cmbAnt4.Checked = (ant[1] & 8) == 8 ? true : false;
                cmbAnt3.Checked = (ant[1] & 4) == 4 ? true : false;
                cmbAnt2.Checked = (ant[1] & 2) == 2 ? true : false;
                cmbAnt1.Checked = (ant[1] & 1) == 1 ? true : false;

                if (ant.Length == 4)
                {
                    cbANT32.Checked = (ant[2] & 128) == 128 ? true : false;
                    cbANT31.Checked = (ant[2] & 64) == 64 ? true : false;
                    cbANT30.Checked = (ant[2] & 32) == 32 ? true : false;
                    cbANT29.Checked = (ant[2] & 16) == 16 ? true : false;
                    cbANT28.Checked = (ant[2] & 8) == 8 ? true : false;
                    cbANT27.Checked = (ant[2] & 4) == 4 ? true : false;
                    cbANT26.Checked = (ant[2] & 2) == 2 ? true : false;
                    cbANT25.Checked = (ant[2] & 1) == 1 ? true : false;

                    cbANT24.Checked = (ant[3] & 128) == 128 ? true : false;
                    cbANT23.Checked = (ant[3] & 64) == 64 ? true : false;
                    cbANT22.Checked = (ant[3] & 32) == 32 ? true : false;
                    cbANT21.Checked = (ant[3] & 16) == 16 ? true : false;
                    cbANT20.Checked = (ant[3] & 8) == 8 ? true : false;
                    cbANT19.Checked = (ant[3] & 4) == 4 ? true : false;
                    cbANT18.Checked = (ant[3] & 2) == 2 ? true : false;
                    cbANT17.Checked = (ant[3] & 1) == 1 ? true : false;
                }
    


                msg = !IsChineseSimple() ? "Success" : "成功!";
                //  msg = Common.isEnglish?"success":"获取天线成功!("+ DataConvert.ByteArrayToHexString(ant)+")";
            }

            showMessage(msg);
        }
        private void btnSetAnt_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            int b1 = 0;
            int b2 = 0;
             
            if (cmbAnt16.Checked) b1 = b1 | 128;
            if (cmbAnt15.Checked) b1 = b1 | 64;
            if (cmbAnt14.Checked) b1 = b1 | 32;
            if (cmbAnt13.Checked) b1 = b1 | 16;
            if (cmbAnt12.Checked) b1 = b1 | 8;
            if (cmbAnt11.Checked) b1 = b1 | 4;
            if (cmbAnt10.Checked) b1 = b1 | 2;
            if (cmbAnt9.Checked) b1 = b1 | 1;


            if (cmbAnt8.Checked) b2 = b2 | 128;
            if (cmbAnt7.Checked) b2 = b2 | 64;
            if (cmbAnt6.Checked) b2 = b2 | 32;
            if (cmbAnt5.Checked) b2 = b2 | 16;
            if (cmbAnt4.Checked) b2 = b2 | 8;
            if (cmbAnt3.Checked) b2 = b2 | 4;
            if (cmbAnt2.Checked) b2 = b2 | 2;
            if (cmbAnt1.Checked) b2 = b2 | 1;

            int b3 = 0;
            int b4 = 0;
            if (cbANT32.Checked) b3 = b3 | 128;
            if (cbANT31.Checked) b3 = b3 | 64;
            if (cbANT30.Checked) b3 = b3 | 32;
            if (cbANT29.Checked) b3 = b3 | 16;
            if (cbANT28.Checked) b3 = b3 | 8;
            if (cbANT27.Checked) b3 = b3 | 4;
            if (cbANT26.Checked) b3 = b3 | 2;
            if (cbANT25.Checked) b3 = b3 | 1;

            if (cbANT24.Checked) b4 = b4 | 128;
            if (cbANT23.Checked) b4 = b4 | 64;
            if (cbANT22.Checked) b4 = b4 | 32;
            if (cbANT21.Checked) b4 = b4 | 16;
            if (cbANT20.Checked) b4 = b4 | 8;
            if (cbANT19.Checked) b4 = b4 | 4;
            if (cbANT18.Checked) b4 = b4 | 2;
            if (cbANT17.Checked) b4 = b4 | 1;

            byte[] ant = new byte[4] { (byte)b1, (byte)b2, (byte)b3, (byte)b4 };
            byte flag = cbAntSet.Checked ? (byte)1 : (byte)0;
            if (uhf.SetANTTo32(flag, ant))
            {
                msg = !IsChineseSimple() ? "Success" : "成功!";
                // msg = Common.isEnglish ? "success" : "设置天线成功!(" + DataConvert.ByteArrayToHexString(ant) + ")"; ;
            }
            showMessage(msg);

        }


 
     
        
        
        #endregion

        #region 区域
        private void btnRegionGet_Click(object sender, EventArgs e)
        {
            if (frequencyBandHashtable != null)
            {
                string msg2 = !IsChineseSimple() ? "Failure!" : "失败!";
                byte region2 = 0;
                if (uhf.GetRegion(ref region2))
                {
                    cmbRegion.SelectedIndex = 0;
                    msg2 = !IsChineseSimple() ? "Success" : "成功!";
                }
                showMessage(msg2);
                return;
            }

            //0x01(China1),0x02(China2),0x04(Europe),0x08(USA),0x16(Korea),0x32(Japan)
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            byte region=0;
            if (uhf.GetRegion(ref region))
            {
                switch (region)
                {
                    case 0x01:
                        cmbRegion.SelectedIndex = 0;
                        break;
                    case 0x02:
                        cmbRegion.SelectedIndex = 1;
                        break;
                    case 0x04:
                        cmbRegion.SelectedIndex = 2;
                        break;
                    case 0x08:
                        cmbRegion.SelectedIndex = 3;
                        break;
                    case 0x16:
                        cmbRegion.SelectedIndex = 4;
                        break;
                    case 0x32:
                        cmbRegion.SelectedIndex = 5;
                        break;
                    case 0x33:
                        cmbRegion.SelectedIndex = 6;
                        break;
                    case 0x34:
                        cmbRegion.SelectedIndex = 7;
                        break;
                    case 0x35:
                        cmbRegion.SelectedIndex = 8;
                        break;
                    case 0x36:
                        cmbRegion.SelectedIndex = 9;
                        break;
                    case 0x37:
                        cmbRegion.SelectedIndex = 10;
                        break;
                    case 0x3B:
                        cmbRegion.SelectedIndex = 11;
                        break;
                    case 0x3C:
                        cmbRegion.SelectedIndex = 12;
                        break;
                    case 0x3D:
                        cmbRegion.SelectedIndex = 13;
                        break;
                    case 0x3E:
                        cmbRegion.SelectedIndex = 14;
                        break;
                    case 0x3F:
                        cmbRegion.SelectedIndex = 15;
                        break;
                    case 0x40:
                        cmbRegion.SelectedIndex = 16;
                        break;
                    case 0x41:
                        cmbRegion.SelectedIndex = 17;
                        break;
                    case 0x42:
                        cmbRegion.SelectedIndex = 18;
                        break;
                    case 0x43:
                        cmbRegion.SelectedIndex = 19;
                        break;
                    case 0x44:
                        cmbRegion.SelectedIndex = 20;
                        break;

                }
                msg  = !IsChineseSimple() ? "Success" : "成功!";
            }

            showMessage(msg);
        }

        private void btnRegionSet_Click(object sender, EventArgs e)
        {
            //0x01(China1),0x02(China2),0x04(Europe),0x08(USA),0x16(Korea),0x32(Japan)
            if (frequencyBandHashtable != null)
            {
                if (frequencyBandHashtable.ContainsKey(cmbRegion.Text))
                {
                    int flag2 = cbRegionSave.Checked ? 1 : 0;
                    string msg2 = !IsChineseSimple() ? "Failure!" : "失败!";
                    string value = (string)frequencyBandHashtable[cmbRegion.Text];
                    if (uhf.SetRegion((byte)flag2, (byte)Convert.ToInt32(value.Replace("0x",""), 16)))
                    {
                        msg2 = !IsChineseSimple() ? "Success" : "成功!";
                    }
                    showMessage(msg2);
                }
                return;
            }
            int flag = cbRegionSave.Checked ? 1 : 0;
            int region = -1;
            switch (cmbRegion.SelectedIndex)
            {
                case 0:
                    region = 0x01;
                    break;
                case 1:
                    region = 0x02;
                    break;
                case 2:
                    region = 0x04;
                    break;
                case 3:
                    region = 0x08;
                    break;
                case 4:
                    region = 0x16;
                    break;
                case 5:
                    region = 0x32;
                    break;
                case 6:
                    region = 0x33;
                    break;
                case 7:
                    region = 0x34;
                    break;
                case 8:
                    region = 0x35;
                    break;
                case 9:
                    region = 0x36;
                    break;
                case 10:
                    region = 0x37;
                    break;
                case 11:
                    region = 0x3B;
                    break;
                case 12:
                    region = 0x3C;
                    break;
                case 13:
                    region = 0x3D;
                    break;
                case 14:
                    region = 0x3E;
                    break;
                case 15:
                    region = 0x3F;
                    break;
                case 16:
                    region = 0x40;
                    break;
                case 17:
                    region = 0x41;
                    break;
                case 18:
                    region = 0x42;
                    break;
                case 19:
                    region = 0x43;
                    break;
                case 20:
                    region = 0x44;
                    break;

            }
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            if (region >= 0)
            {
                if (uhf.SetRegion((byte)flag, (byte)region))
                {
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                }
                 
            }
            showMessage(msg);
        }
        #endregion
 

        #region 链路组合
        private void btnRFLinkGet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            byte mode = 0;
            if (uhf.GetRFLink(ref mode))
            {
                switch (mode)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        cmbRFLink.SelectedIndex = mode;
                        break;
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        cmbRFLink.SelectedIndex = mode-4;
                        break;
                }
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }

            showMessage(msg);
        }
        private void btnRFLinkSet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            int flag = cbRFLink.Checked ? 1 : 0;
            if (cmbRFLink.SelectedIndex >= 0)
            {
                int v = cmbRFLink.SelectedIndex;
                switch (cmbRFLink.SelectedIndex)
                {
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        v = cmbRFLink.SelectedIndex+4;
                        break;
                }
                if (uhf.SetRFLink((byte)flag, (byte)v)) {
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                }
            }

            showMessage(msg);
        }

        #endregion

        #region FastID
        private void btnFastIDGet_Click(object sender, EventArgs e)
        {
            byte flag = 0;
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            if (uhf.GetFastID(ref flag))
            {
                if (flag == 0)
                {
                    rbFastIDEnable.Checked = false;
                    rbFastIDDisable.Checked = true;
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                }
                else if (flag == 1)
                {
                    rbFastIDEnable.Checked = true;
                    rbFastIDDisable.Checked = false;
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                }
            }
            showMessage(msg);
        }
        private void btnFastIDSet_Click(object sender, EventArgs e)
        {
            int flag = -1;
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            if (rbFastIDEnable.Checked)
            {
                flag = 1;
            }
            else if (rbFastIDDisable.Checked)
            {
                flag = 0;
            }

            if (flag >= 0)
            {
                if (uhf.SetFastID((byte)flag))
                {
                    msg = !IsChineseSimple() ? "Success" : "成功!";

                    if (flag == 1) {
                        if (uhf.SetTagfocus(0))
                        {
                            rbTagfocusDisable.Checked = true;
                        }
                        if (uhf.setEPCMode(false))
                        {
                            cbInventoryMode.SelectedIndex = 0;
                        }
                    }

                }
               
            }

            showMessage(msg);
        }
        #endregion

        #region Tagfocus
        private void btnrbTagfocusGet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            byte flag = 0;
            if (uhf.GetTagfocus(ref flag))
            {
                if (flag == 0)
                {
                    rbTagfocusEnable.Checked = false;
                    rbTagfocusDisable.Checked = true;
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                }
                else if (flag == 1)
                {
                    rbTagfocusEnable.Checked = true;
                    rbTagfocusDisable.Checked = false;
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                }
            }

            showMessage(msg);
        }
        private void btnrbTagfocusSet_Click(object sender, EventArgs e)
        {
            int flag = -1;
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            if (rbTagfocusEnable.Checked)
            {
                flag = 1;
            }
            else if (rbTagfocusDisable.Checked)
            {
                flag = 0;
            }

            if (flag >= 0)
            {
                if (uhf.SetTagfocus((byte)flag))
                {
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                    if (flag == 1)
                    {

                        if (uhf.SetFastID(0))
                        {
                            rbFastIDDisable.Checked = true;
                        }
                 
                        if (uhf.setEPCMode(false))
                        {
                            cbInventoryMode.SelectedIndex = 0;
                        }
                    }
                }
                
            }
            showMessage(msg);
        }
        #endregion
        #region 设置软复位
        private void btnReset_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            if (uhf.SetSoftReset())
            {
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }

            showMessage(msg);
        }
        #endregion

        #region FastInventory
        private void btnGetFastInventory_Click(object sender, EventArgs e)
        {
            byte flag = 0;
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            if (uhf.GetFastInventory(ref flag))
            {
                if (flag>=0 && flag<=4)
                {
                    cmbCR.SelectedIndex = flag;
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                }
            
            }
            showMessage(msg);
        }

        private void btnSetFastInventory_Click(object sender, EventArgs e)
        {
            int flag = cmbCR.SelectedIndex;
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            if (flag >= 0)
            {
                if (uhf.SetFastInventory(cbFastInventorySave.Checked?1:0, (byte)flag))
                {
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                }
            }
            showMessage(msg);
        }
        #endregion


   

      

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string data = textBox1.Text.Trim().Replace(" ", "");
            if (data.Length > 0) {

                
                int index=textBox1.SelectionStart-1;
                if (index >= 0)
                {
                    string charData = data.Substring(index, 1);
                    if (charData != "0" && charData != "1" && charData != "2" 
                        && charData != "3" && charData != "4" && charData != "5" && charData != "6" && charData != "7"
                        && charData != "8" && charData != "9" && charData != ".")
                    {
                        textBox1.Text = textBox1.Text.Remove(index, 1);
                        textBox1.SelectionStart = textBox1.Text.Length;
                    }
                }
            }
   
        }



    

        private void showMessage(string msg,int time) {
            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                System.Threading.Thread.Sleep(time);
            }, msg);
            f.ShowDialog(this);
        }
        private void showMessage(string msg)
        {
            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                System.Threading.Thread.Sleep(500);
            }, msg);
            f.ShowDialog(this);
        }

        #region IP
        //设置本地IP
        private void btnSetIPLocal_Click(object sender, EventArgs e)
        {
            string port = txtLocalPort.Text.Trim();

            StringBuilder ip = new StringBuilder();
            ip.Append(ipControlLocal.IpData[0]);
            ip.Append(".");
            ip.Append(ipControlLocal.IpData[1]);
            ip.Append(".");
            ip.Append(ipControlLocal.IpData[2]);
            ip.Append(".");
            ip.Append(ipControlLocal.IpData[3]);


            StringBuilder sbMask = new StringBuilder();
            sbMask.Append(ipControlSubnetMask.IpData[0]);
            sbMask.Append(".");
            sbMask.Append(ipControlSubnetMask.IpData[1]);
            sbMask.Append(".");
            sbMask.Append(ipControlSubnetMask.IpData[2]);
            sbMask.Append(".");
            sbMask.Append(ipControlSubnetMask.IpData[3]);


            StringBuilder gate = new StringBuilder();
            gate.Append(ipGateway.IpData[0]);
            gate.Append(".");
            gate.Append(ipGateway.IpData[1]);
            gate.Append(".");
            gate.Append(ipGateway.IpData[2]);
            gate.Append(".");
            gate.Append(ipGateway.IpData[3]);

            if (!StringUtils.isIP(ip.ToString()) || !StringUtils.isIP(sbMask.ToString()) || !StringUtils.isIP(gate.ToString()))
            {
                string msg = !IsChineseSimple() ? "Failure!" : "设置IP失败!";
                showMessage(msg);
                return;
            }
            if (!StringUtils.IsNumber(port))
            {
                string msg = !IsChineseSimple() ? "Failure!" : "设置IP失败!";
                showMessage(msg);
                return;
            }

            if (ipControlLocal.IpData[0] == "255")
            {
                string msg = !IsChineseSimple() ? "Failure!" : "IP地址不能255开头,设置IP失败!";
                showMessage(msg);
                return;
            }
            if (ipControlSubnetMask.IpData[0] != "255")
            {
                string msg = !IsChineseSimple() ? "Failure!" : "子网掩码必须255开头,设置IP失败!";
                showMessage(msg);
                return;
            }
            if (ipGateway.IpData[0] == "255")
            {
                string msg = !IsChineseSimple() ? "Failure!" : "网关不能255开头,设置IP失败!";
                showMessage(msg);
                return;
            }
            if (!uhf.SetReaderIP(ip.ToString(), int.Parse(port), sbMask.ToString(), gate.ToString(),rbDHCP.Checked))
            {
                string msg = !IsChineseSimple() ? "Failure!" : "设置IP失败!";
                showMessage(msg);
                return;
            }
             string msg2 = !IsChineseSimple() ? "Success" : "成功!";
             showMessage(msg2);
           }
        //获取本地IP
        private void btnGetIPLocal_Click(object sender, EventArgs e)
        {
            int startTime = Environment.TickCount;
            StringBuilder sIP=new StringBuilder(20);
            StringBuilder sPort = new StringBuilder(20);
            StringBuilder mask = new StringBuilder(20);
            StringBuilder gate = new StringBuilder(20);
            bool[] dhcp = new bool[1];
            if (uhf.GetReaderIP(sIP, sPort, mask, gate, dhcp))
            {
                ipControlLocal.IpData = sIP.ToString().Split('.');// txtLocalIP.Text = sIP.ToString();
                txtLocalPort.Text = sPort.ToString();
                ipGateway.IpData = gate.ToString().Split('.');
                ipControlSubnetMask.IpData = mask.ToString().Split('.');
                if (dhcp[0])
                {
                    rbDHCP.Checked = true;
                }
                else
                {
                    rbStatic.Checked = true;
                }
            }
            else
            {
                string msg = !IsChineseSimple() ? "Failure!" : "获取IP失败!";
                showMessage(msg);
                return;
            }
            string msg2 = !IsChineseSimple() ? "Success" : "成功!";
            showMessage(msg2);

        }
 
        //获取目标IP
        private void btnGetIpDest_Click(object sender, EventArgs e)
        {
            StringBuilder sIP = new StringBuilder();
            StringBuilder sPort = new StringBuilder();
            if (uhf.GetDestIP(sIP, sPort))
            {
               // txtIPDest.Text = sIP.ToString();
                ipControlDest.IpData = sIP.ToString().Split('.');
                txtPortDest.Text = sPort.ToString();
            }
            else
            {
                string msg = !IsChineseSimple() ? "Failure!" : "获取IP失败!";
                showMessage(msg);
                return;
            }
            string msg2 = !IsChineseSimple() ? "Success" : "成功!";
            showMessage(msg2);
        }

        //设置目标IP
        private void btnSetIpDest_Click(object sender, EventArgs e)
        {
            string port = txtPortDest.Text.Trim();

            string[] tempIp = ipControlDest.IpData;
            StringBuilder sb = new StringBuilder();
            sb.Append(tempIp[0]);
            sb.Append(".");
            sb.Append(tempIp[1]);
            sb.Append(".");
            sb.Append(tempIp[2]);
            sb.Append(".");
            sb.Append(tempIp[3]);
            string ip = sb.ToString();
  
            if (!StringUtils.isIP(ip))
            {
                string msg = !IsChineseSimple() ? "Failure!" : "设置IP失败!";
                showMessage(msg);
                return;
            }
            if (!StringUtils.IsNumber(port))
            {
                string msg = !IsChineseSimple() ? "Failure!" : "设置IP失败!";
                showMessage(msg);
                return;
            }
            if (ipControlDest.IpData[0] == "255")
            {
                string msg = !IsChineseSimple() ? "Failure!" : "IP地址不能255开头，设置IP失败!";
                showMessage(msg);
                return;
            }

            if (!uhf.SetDestIP(ip, int.Parse(port)))
            {
                string msg = !IsChineseSimple() ? "Failure!" : "设置IP失败!";
                showMessage(msg);
                return;
            }
            string msg2 = !IsChineseSimple() ? "Success" : "成功!";
            showMessage(msg2);
        }

        #endregion

        #region 蜂鸣器
        //获取蜂鸣器
        private void btnGetBuzzer_Click(object sender, EventArgs e)
        {
            byte[] mode=new byte[10];
            if (!uhf.UHFGetBuzzer(mode))
            {
                string msg = !IsChineseSimple() ? "Failure!" : "失败!";
                showMessage(msg);
                return;
            }
            else
            {
                if (mode[0] == 0)
                {

                    rbEnableBuzzer.Checked = false;
                    rbDisableBuzzer.Checked = true;
                }
                else if (mode[0] == 1)
                {
                    rbDisableBuzzer.Checked = false;
                    rbEnableBuzzer.Checked = true;
                }

            }
            string msg2 = !IsChineseSimple() ? "Success" : "成功!";
            showMessage(msg2);
        }
        //设置蜂鸣器
        private void btnSetBuzzer_Click(object sender, EventArgs e)
        {
            //0x00表示关闭蜂鸣器；0x01表示打开蜂鸣器
            byte mode =0;
            if (rbEnableBuzzer.Checked)
            {
                mode = 1;
            }
            else if (rbDisableBuzzer.Checked)
            {
                mode = 0;
            }
            else {

                string msg = !IsChineseSimple() ? "Failure" : "失败!";
                showMessage(msg);
                return;
            }

            if (!uhf.UHFSetBuzzer(mode))
            {
                string msg = !IsChineseSimple() ? "Failure" : "失败!";
                showMessage(msg);
                return;
            }
            string msg2 = !IsChineseSimple() ? "Success" : "成功!";
            showMessage(msg2);
        }
        #endregion

        #region 工作模式
        private void button2_Click(object sender, EventArgs e)
        {
            //get
            byte[] mode=new byte[2];
            if (uhf.GetWorkMode(mode))
            {
                if (mode[0] == 0)
                {
                    workMode.SelectedIndex = 0;
                }
                else if (mode[0] == 1)
                {
                    workMode.SelectedIndex = 1;
                }
                else if (mode[0] == 2)
                {
                    workMode.SelectedIndex = 2;
                }
                else if (mode[0] == 3)
                {
                    workMode.SelectedIndex = 3;
                }
                else if (mode[0] == 4)
                {
                    workMode.SelectedIndex = 4;
                }
            }
            else
            {
                string msg = !IsChineseSimple() ? "Failure" : "失败!";
                showMessage(msg);
                return;

            }
            string msg2 = !IsChineseSimple() ? "Success" : "成功!";
            showMessage(msg2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte mode=(byte) workMode.SelectedIndex;
            if (!uhf.SetWorkMode(mode))
            {
                string msg = !IsChineseSimple() ? "Failure" : "失败!";
                showMessage(msg);
                return;
             
            }
            string msg2 = !IsChineseSimple() ? "Success" : "成功!";
            showMessage(msg2);
        }

        private void btnWorkModeParaSet_Click(object sender, EventArgs e)
        {
            UHFAPI uhfAPI = uhf as UHFAPI;
            if (uhfAPI != null)
            {
                if (txtWT.Text.Trim().Length == 0)
                {
                    string msg = !IsChineseSimple() ? "Failure" : "失败!";
                    showMessage(msg);
                }
                if (txtIT.Text.Trim().Length == 0)
                {
                    string msg = !IsChineseSimple() ? "Failure" : "失败!";
                    showMessage(msg);
                }

                int input = cmbInput.SelectedIndex;
                int workTime = int.Parse(txtWT.Text);
                int waitTime = int.Parse(txtIT.Text);
                int receiveMode = comRM.SelectedIndex;
                if (!uhfAPI.SetWorkModePara((byte)input, workTime, waitTime, (byte)receiveMode))
                {
                    string msg = !IsChineseSimple() ? "Failure" : "失败!";
                    showMessage(msg);
                }
                string msg2 = !IsChineseSimple() ? "Success" : "成功!";
                showMessage(msg2);
            }

        }

        private void btnWorkModeParaGet_Click(object sender, EventArgs e)
        {
            UHFAPI uhfAPI = uhf as UHFAPI;
            if (uhfAPI != null)
            {
                 byte ioControl=0;
                 int workTime=100;
                 int intervalTime=0;
                 byte mode=0;
                if (uhfAPI.GetWorkModePara(ref  ioControl, ref  workTime, ref intervalTime, ref mode))
                {
                    cmbInput.SelectedIndex = ioControl;
                    txtWT.Text = workTime.ToString();
                    txtIT.Text = intervalTime.ToString();
                    comRM.SelectedIndex = mode;
                    string msg2 = !IsChineseSimple() ? "Success" : "成功!";
                    showMessage(msg2);

                }
                else
                {
                    string msg = !IsChineseSimple() ? "Failure" : "失败!";
                    showMessage(msg);
                }            
            }
        }

        #endregion
 

    
 
        #region GPIO
        private void button6_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[2];
            if (!uhf.getIOControl(data))
            {
                string msg = !IsChineseSimple() ? "Failure" : "失败!";
                showMessage(msg);
                return;
            }
            else {
                comboBox2.SelectedIndex = data[0];
                comboBox3.SelectedIndex = data[1];
                string msg2 = !IsChineseSimple() ? "Success" : "成功!";
                showMessage(msg2);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int status = cmbOutStatus.SelectedIndex;
            int ouput2 = cmbO2.SelectedIndex;
            int ouput1 = cmbO1.SelectedIndex;
 
            if (!uhf.setIOControl((byte)ouput1, (byte)ouput2, (byte)status))
            {
                string msg = !IsChineseSimple() ? "Failure" : "失败!";
                showMessage(msg);
                return;
            }
            string msg2 = !IsChineseSimple() ? "Success" : "成功!";
            showMessage(msg2);
        }
        #endregion

   

        #region  盘点模式设置

        private void button11_Click(object sender, EventArgs e)
        {
            byte userPtr = 0;
            byte userLen = 0;
            string msg2 = !IsChineseSimple() ? "Success" : "成功!";
            int mode = -1;
            if (txtPWD.Visible)
            {
                 byte[] m=new byte[7];
                int[] pmodelen=new int[1];
                if (UHFAPI.UHFGetInventoryMode(0, 0, m, pmodelen) == 0)
                {
                    mode = m[0];
                    userPtr = m[1];
                    userLen = m[2];
                    if (pmodelen[0] == 7)
                    {
                        m = Utils.CopyArray(m,3, 4);
                        txtPWD.Text = DataConvert.ByteArrayToHexString(m); 
                    }
                    switch (mode)
                    {
                        case 0:
                            cbInventoryMode.SelectedIndex = 0;
                            break;
                        case 1:
                            cbInventoryMode.SelectedIndex = 1;
                            break;
                        case 2:
                            cbInventoryMode.SelectedIndex = 2;
                            txtUserLen.Text = userLen + "";
                            txtUserPtr.Text = userPtr + "";
                            break;
                        case 6:
                            cbInventoryMode.SelectedIndex = 3;
                            txtUserLen.Text = userLen + "";
                            txtUserPtr.Text = userPtr + "";
                            break;
                        default:
                            msg2 = !IsChineseSimple() ? "Failure" : "失败!";
                            cbInventoryMode.SelectedIndex = -1;
                            break;
                    }
                }
            }
            else
            {
                mode = uhf.getEPCTIDUSERMode(ref userPtr, ref userLen);
                switch (mode)
                {
                    case 0:
                        cbInventoryMode.SelectedIndex = 0;
                        break;
                    case 1:
                        cbInventoryMode.SelectedIndex = 1;
                        break;
                    case 2:
                        cbInventoryMode.SelectedIndex = 2;
                        txtUserLen.Text = userLen + "";
                        txtUserPtr.Text = userPtr + "";
                        break;
                    default:
                        msg2 = !IsChineseSimple() ? "Failure" : "失败!";
                        cbInventoryMode.SelectedIndex = -1;
                        break;
                }
            }
   
        
     
            showMessage(msg2);
        }

        private void button10_Click(object sender, EventArgs e)
        {

            bool result = false;
            bool isSave = checkBox2.Checked;
            if (txtPWD.Visible) {
                int userPtr = int.Parse(txtUserPtr.Text);
                int userLen = int.Parse(txtUserLen.Text);

                int mode = cbInventoryMode.SelectedIndex;
                if (cbInventoryMode.SelectedIndex == 3)
                {
                    mode = 6;
                }
                byte[] pwd = DataConvert.HexStringToByteArray(txtPWD.Text);
                int p = ((pwd[0] & 0xFF) << 24) | ((pwd[1] & 0xFF) << 16) | ((pwd[2] & 0xFF) << 8) | ((pwd[3] & 0xFF));
                int r= UHFAPI.UHFSetInventoryMode((byte)(checkBox2.Checked ? 1 : 0), (byte)mode, (byte)userPtr, (byte)userLen, p);
                result = r == 0;
            }
            else
            {
                int mode = cbInventoryMode.SelectedIndex;
                switch (mode)
                {
                    case 0:
                        result = uhf.setEPCMode(isSave);
                        break;
                    case 1:
                        result = uhf.setEPCAndTIDMode(isSave);
                        break;
                    case 2:
                        int userPtr = int.Parse(txtUserPtr.Text);
                        int userLen = int.Parse(txtUserLen.Text);
                        result = uhf.setEPCAndTIDUSERMode(isSave, (byte)userPtr, (byte)userLen);
                        break;
                }
            }
            if (!result)
            {
                string msg = !IsChineseSimple() ? "Failure" : "失败!";
                showMessage(msg);
                return;
            }
            string msg2 = !IsChineseSimple() ? "Success" : "成功!";
            showMessage(msg2);
        }

        private void cbInventoryMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbInventoryMode.SelectedIndex == 2 || cbInventoryMode.SelectedIndex == 3)
            {
                txtUserLen.Visible = true;
                txtUserPtr.Visible = true;
                label46.Visible = true;
                label47.Visible = true;
            }
            else
            {
                txtUserLen.Visible = false;
                txtUserPtr.Visible = false;
                label46.Visible = false;
                label47.Visible = false;
            }
        }
        #endregion

    

        private void workMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (workMode.SelectedIndex == 2)
            {
                plWorkModePara.Visible = true;
                btnWorkModeParaGet_Click(null,null);
            }
            else
            {
                plWorkModePara.Visible = false;

            }
        }

        private void btnAntennaConnectionState_Click(object sender, EventArgs e)
        {
            cbANT1_state.Checked = false;
            cbANT2_state.Checked = false;
            cbANT3_state.Checked = false;
            cbANT4_state.Checked = false;
            cbANT5_state.Checked = false;
            cbANT6_state.Checked = false;
            cbANT7_state.Checked = false;
            cbANT8_state.Checked = false;

            cbANT9_state.Checked = false;
            cbANT10_state.Checked = false;
            cbANT11_state.Checked = false;
            cbANT12_state.Checked = false;
            cbANT13_state.Checked = false;
            cbANT14_state.Checked = false;
            cbANT15_state.Checked = false;
            cbANT16_state.Checked = false;

            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            short[] antstate = new short[1];
            if (uhf.GetANTLinkStatus(antstate))
            {
                short antS = (short)(antstate[0] & 0xFF);
                if (((antS >> 7) & 1) == 1) cbANT8_state.Checked = true;
                if (((antS >> 6) & 1) == 1) cbANT7_state.Checked = true;
                if (((antS >> 5) & 1) == 1) cbANT6_state.Checked = true;
                if (((antS >> 4) & 1) == 1) cbANT5_state.Checked = true;
                if (((antS >> 3) & 1) == 1) cbANT4_state.Checked = true;
                if (((antS >> 2) & 1) == 1) cbANT3_state.Checked = true;
                if (((antS >> 1) & 1) == 1) cbANT2_state.Checked = true;
                if ((antS & 1) == 1) cbANT1_state.Checked = true;

                antS = (short)(antstate[0] >> 8  );
                if (((antS >> 7) & 1) == 1) cbANT16_state.Checked = true;
                if (((antS >> 6) & 1) == 1) cbANT15_state.Checked = true;
                if (((antS >> 5) & 1) == 1) cbANT14_state.Checked = true;
                if (((antS >> 4) & 1) == 1) cbANT13_state.Checked = true;
                if (((antS >> 3) & 1) == 1) cbANT12_state.Checked = true;
                if (((antS >> 2) & 1) == 1) cbANT11_state.Checked = true;
                if (((antS >> 1) & 1) == 1) cbANT10_state.Checked = true;
                if ((antS & 1) == 1) cbANT9_state.Checked = true;

                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            int result = uhf.CalibrationVoltage();
            txtCalibration.Text = result + "";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            byte[] statusData = new byte[2];
            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            if (uhf.GetInputStatus(statusData))
            {
                cmbInput1.SelectedIndex = statusData[0];
                cmbInput2.SelectedIndex = statusData[1];
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);


           
        }

        private void button13_Click(object sender, EventArgs e)
        {
            byte[] outData = new byte[5];
            outData[3] = (byte)cmbOutput1.SelectedIndex;
            outData[4] = (byte)cmbOutput2.SelectedIndex;
            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            if (uhf.SetOutput(outData))
            {

                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);
        }


        private void MainForm_SizeChanged(FormWindowState state)
        {
            //判断是否选择的是最小化按钮
            panel1.Left = 308;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string id = uhf.GetUHFGetDeviceID();
            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            if (id !=null)
            {
                
                textBox2.Text = id;
                msg = !IsChineseSimple() ? "Success" : "成功!";
                showMessage(msg);
                return;
            }
            showMessage(msg);
        }
        private void LoadUI()
        {
            if (!IsChineseSimple())
            {
                gbBaudrate.Text = "Baud rate";
                lblBaudrate.Text = "Baud rate:";


                int index = comboBox8.SelectedIndex;
                comboBox8.Items.Clear();
                comboBox8.Items.Add("low voltage");
                comboBox8.Items.Add("high voltage");
                if (index >= 0)
                    comboBox8.SelectedIndex = index;


                index = comboBox9.SelectedIndex;
                comboBox9.Items.Clear();
                comboBox9.Items.Add("low voltage");
                comboBox9.Items.Add("high voltage");
                if (index >= 0)
                    comboBox9.SelectedIndex = index;

                index = comboBox7.SelectedIndex;
                comboBox7.Items.Clear();
                comboBox7.Items.Add("low voltage");
                comboBox7.Items.Add("high voltage");
                if (index >= 0)
                    comboBox7.SelectedIndex = index;

                index = comboBox14.SelectedIndex;
                comboBox14.Items.Clear();
                comboBox14.Items.Add("low voltage");
                comboBox14.Items.Add("high voltage");
                if (index >= 0)
                    comboBox14.SelectedIndex = index;

                index = comboBox15.SelectedIndex;
                comboBox15.Items.Clear();
                comboBox15.Items.Add("low voltage");
                comboBox15.Items.Add("high voltage");
                if (index >= 0)
                    comboBox15.SelectedIndex = index;

                index = comboBox16.SelectedIndex;
                comboBox16.Items.Clear();
                comboBox16.Items.Add("low voltage");
                comboBox16.Items.Add("high voltage");
                if (index >= 0)
                    comboBox16.SelectedIndex = index;

                index = comboBox17.SelectedIndex;
                comboBox17.Items.Clear();
                comboBox17.Items.Add("low voltage");
                comboBox17.Items.Add("high voltage");
                if (index >= 0)
                    comboBox17.SelectedIndex = index;

                index = comboBox5.SelectedIndex;
                comboBox5.Items.Clear();
                comboBox5.Items.Add("low voltage");
                comboBox5.Items.Add("high voltage");
                if (index >= 0)
                    comboBox5.SelectedIndex = index;

                index = comboBox6.SelectedIndex;
                comboBox6.Items.Clear();
                comboBox6.Items.Add("low voltage");
                comboBox6.Items.Add("high voltage");
                if (index >= 0)
                    comboBox6.SelectedIndex = index;

                index = comboBox10.SelectedIndex;
                comboBox10.Items.Clear();
                comboBox10.Items.Add("low voltage");
                comboBox10.Items.Add("high voltage");
                if (index >= 0)
                    comboBox10.SelectedIndex = index;

                index = comboBox11.SelectedIndex;
                comboBox11.Items.Clear();
                comboBox11.Items.Add("low voltage");
                comboBox11.Items.Add("high voltage");
                if (index >= 0)
                    comboBox11.SelectedIndex = index;

                index = comboBox12.SelectedIndex;
                comboBox12.Items.Clear();
                comboBox12.Items.Add("low voltage");
                comboBox12.Items.Add("high voltage");
                if (index >= 0)
                    comboBox12.SelectedIndex = index;

                index = comboBox13.SelectedIndex;
                comboBox13.Items.Clear();
                comboBox13.Items.Add("low voltage");
                comboBox13.Items.Add("high voltage");
                if (index >= 0)
                    comboBox13.SelectedIndex = index;

                index = comboBox18.SelectedIndex;
                comboBox18.Items.Clear();
                comboBox18.Items.Add("low voltage");
                comboBox18.Items.Add("high voltage");
                if (index >= 0)
                    comboBox18.SelectedIndex = index;


                int index2 = comboBox2.SelectedIndex;
                comboBox2.Items.Clear();
                comboBox2.Items.Add("low voltage");
                comboBox2.Items.Add("high voltage");
                if (index2 >= 0)
                    comboBox2.SelectedIndex = index2;

                int cmbO1Index = cmbO1.SelectedIndex;
                cmbO1.Items.Clear();
                cmbO1.Items.Add("low voltage");
                cmbO1.Items.Add("high voltage");
                if (cmbO1Index >= 0)
                    cmbO1.SelectedIndex = cmbO1Index;

                cmbO1Index = cmbO2.SelectedIndex;
                cmbO2.Items.Clear();
                cmbO2.Items.Add("low voltage");
                cmbO2.Items.Add("high voltage");
                if (cmbO1Index >= 0)
                    cmbO2.SelectedIndex = cmbO1Index;

                int index3 = comboBox3.SelectedIndex;
                comboBox3.Items.Clear();
                comboBox3.Items.Add("low voltage");
                comboBox3.Items.Add("high voltage");
                if (index3 >= 0)
                    comboBox3.SelectedIndex = index3;

                int index4 = cmbInput1.SelectedIndex;
                cmbInput1.Items.Clear();
                cmbInput1.Items.Add("low voltage");
                cmbInput1.Items.Add("high voltage");
                if (index4 >= 0)
                    cmbInput1.SelectedIndex = index4;

                int index5 = cmbInput2.SelectedIndex;
                cmbInput2.Items.Clear();
                cmbInput2.Items.Add("low voltage");
                cmbInput2.Items.Add("high voltage");
                if (index5 >= 0)
                    cmbInput2.SelectedIndex = index5;

                int index6 = cmbOutput1.SelectedIndex;
                cmbOutput1.Items.Clear();
                cmbOutput1.Items.Add("low voltage");
                cmbOutput1.Items.Add("high voltage");
                if (index6 >= 0)
                    cmbOutput1.SelectedIndex = index6;

                int index7 = cmbOutput2.SelectedIndex;
                cmbOutput2.Items.Clear();
                cmbOutput2.Items.Add("low voltage");
                cmbOutput2.Items.Add("high voltage");
                if (index7 >= 0)
                    cmbOutput2.SelectedIndex = index7;

                int index1 = cmbOutStatus.SelectedIndex;
                cmbOutStatus.Items.Clear();
                cmbOutStatus.Items.Add("Open");
                cmbOutStatus.Items.Add("Close");
                if (index1 >= 0)
                    cmbOutStatus.SelectedIndex = index1;

                comboBox19.Items.Clear();
                comboBox19.Text = "";
                comboBox19.Items.Add("Open");
                comboBox19.Items.Add("Close");
                comboBox20.Items.Clear();
                comboBox20.Text = "";
                comboBox20.Items.Add("Open");
                comboBox20.Items.Add("Close");
                

                label2.Text = "";
                groupBox6.Text = "Power";
                label24.Text = "Output Power:";
                // cbSave.Text = "Save";


                groupBox11.Text = "Region";
                label1.Text = "Region:";
                cbRegionSave.Text = "Save";

                label5.Text = "RFLink:";
                groupBox3.Text = "RFLink";
                cbRFLink.Text = "Save";

                groupBox7.Text = "Frequency";
                label28.Text = "Frequency:";

                

                gbAnt.Text = "ANT";
              
                label24.Location = new Point(8, 34);
            

                gbIP.Text = "Local IP";
                gbIp2.Text = "Destination IP";
                label9.Text = label31.Text = "IP:";
                label25.Text = label30.Text = "Port:";
                groupBox9.Text = "Buzzer";

                 index = workMode.SelectedIndex == -1 ? 0 : workMode.SelectedIndex;
                workMode.Items.Clear();
                workMode.Items.Add("command mode");
                workMode.Items.Add("auto mode");
                workMode.Items.Add("trigger mode");
                workMode.Items.Add("Serial port outputs tag characters");
                workMode.Items.Add("UDP outputs tag characters");
                groupBox1.Text = "cw";

                workMode.SelectedIndex = index;


                label39.Text = "input1:";
                label40.Text = "input2:";
                label100.Text= label99.Text= label38.Text = "Relay:";

                gbInventoryMode.Text = "Inventory Mode";
                label45.Text = "Mode:";
                label46.Text = "User Ptr:";
                label47.Text = "User Len:";

                label53.Text = "Subnet mask:";
                label54.Text = "Gateway:";

                gbFastInventory.Text = "Fast Inventory";
                button3.Text = "Factory data reset";
                groupBox19.Text = "Report unique tags";
                label81.Text = "Wait time:";
                groupBox20.Text = "Device type";
                label84.Text = "Type:";

                groupBox25.Text= "Calibration";
                btnCalibration.Text = "Calibration";
            }
            else
            {
                btnReset.Text = "软件复位";
                label53.Text = "   子网掩码:";
                label54.Text = "   网关:";

                int index1 = cmbOutStatus.SelectedIndex;
                cmbOutStatus.Items.Clear();
                cmbOutStatus.Items.Add("断开");
                cmbOutStatus.Items.Add("闭合");
                if (index1 >= 0)
                    cmbOutStatus.SelectedIndex = index1;

 

                label2.Text = "设置Gen2之前先获取";
                groupBox6.Text = "功率";
                label24.Text = "输出功率:";
                //cbSave.Text = "保存";


                groupBox11.Text = "区域";
                label1.Text = "区域:";
                cbRegionSave.Text = "保存";

                label5.Text = "链路组合:";
                groupBox3.Text = "链路";
                cbRFLink.Text = "保存";

                groupBox7.Text = "定频";
                label28.Text = "频点:";

             

                gbAnt.Text = "天线";
     

                groupBox1.Text = "连续波";


                label39.Text = "输入1:";
                label40.Text = "输入2:";
                label38.Text = "继电器:";

                label24.Location = new Point(30, 34);
                //      label1.Location = new Point(63, 39);
                //      label5.Location = new Point(40, 33);
                //       label28.Location = new Point(55, 42);

             

                gbIP.Text = "本地IP";
                gbIp2.Text = "目标IP";
                //  label9.Text = label31.Text = "IP地址:";
                //  label25.Text = label30.Text = "端口号:";
                groupBox9.Text = "蜂鸣器";


                int index = workMode.SelectedIndex == -1 ? 0 : workMode.SelectedIndex;
                workMode.Items.Clear();
                workMode.Items.Add("命令工作模式");
                workMode.Items.Add("自动工作模式");
                workMode.Items.Add("触发模式");
                workMode.Items.Add("串口输出标签字符");
                workMode.Items.Add("UDP输出标签字符");
                workMode.SelectedIndex = index;

                gbInventoryMode.Text = "盘点模式";
                label45.Text = "模式        :";
                label46.Text = "User起始地址:";
                label47.Text = "User长度    :";
            }
        }


        private Hashtable frequencyBandHashtable = null;
        private void LoadRegionConfig()
        {
            
            string path = System.Environment.CurrentDirectory + "\\frequencyBand.txt";
            if (File.Exists(path))
            {
                List<string> list = FileManage.ReadFileToArr(path);
                if (list.Count == 0)
                {
                    return;
                }
                frequencyBandHashtable = new Hashtable();
                cmbRegion.Items.Clear();
                for (int k = 0; k < list.Count; k++)
                {
                    string[] arr = list[k].Split(',');
                    frequencyBandHashtable.Add(arr[0], arr[1]);
                    cmbRegion.Items.Add(arr[0]);
                }
                cmbRegion.SelectedIndex = 0;
            }
 

        }

        private void LoadFrequencyPoint()
        {

            string path = System.Environment.CurrentDirectory + "\\frequencyPoint.txt";
            if (File.Exists(path))
            {
                List<string> list = FileManage.ReadFileToArr(path);
                if (list.Count == 0)
                {
                    return;
                }
                comboBox1.Items.Clear();
                for (int k = 0; k < list.Count; k++)
                {
                    comboBox1.Items.Add(list[k]);
                }
                comboBox1.SelectedIndex = 0;
            }


        }

        private void btnGetAllPower_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            byte[] power = uhf.GetAntennaAllPower();
            cmbPower1.Text = "";
            cmbPower2.Text = "";
            cmbPower3.Text = "";
            cmbPower4.Text = "";
            cmbPower5.Text = "";
            cmbPower6.Text = "";
            cmbPower7.Text = "";
            cmbPower8.Text = "";
            cmbPower9.Text = "";
            cmbPower10.Text = "";
            cmbPower11.Text = "";
            cmbPower12.Text = "";
            cmbPower13.Text = "";
            cmbPower14.Text = "";
            cmbPower15.Text = "";
            cmbPower16.Text = "";
            if (power != null && power.Length > 0)
            {
                for (int k = 1; k <= power.Length; k++)
                {
                    switch (k)
                    {
                        case 1:
                            cmbPower1.Text = power[k - 1]+""  ;
                            break;
                        case 2:
                            cmbPower2.Text = power[k - 1] + "";
                            break;
                        case 3:
                            cmbPower3.Text = power[k - 1] + "";
                            break;
                        case 4:
                            cmbPower4.Text = power[k - 1] + "";
                            break;
                        case 5:
                            cmbPower5.Text = power[k - 1] + "";
                            break;
                        case 6:
                            cmbPower6.Text = power[k - 1] + "";
                            break;
                        case 7:
                            cmbPower7.Text = power[k - 1] + "";
                            break;
                        case 8:
                            cmbPower8.Text = power[k - 1] + "";
                            break;
                        case 9:
                            cmbPower9.Text = power[k - 1] + "";
                            break;
                        case 10:
                            cmbPower10.Text = power[k - 1] + "";
                            break;
                        case 11:
                            cmbPower11.Text = power[k - 1] + "";
                            break;
                        case 12:
                            cmbPower12.Text = power[k - 1] + "";
                            break;
                        case 13:
                            cmbPower13.Text = power[k - 1] + "";
                            break;
                        case 14:
                            cmbPower14.Text = power[k - 1] + "";
                            break;
                        case 15:
                            cmbPower15.Text = power[k - 1] + "";
                            break;
                        case 16:
                            cmbPower16.Text = power[k - 1] + "";
                            break;
                    }
                }
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            if(sender!=null)
            showMessage(msg);
        }

        private void btnSetAllPower_Click(object sender, EventArgs e)
        {
            List<AntennaPowerInfo> list = new List<AntennaPowerInfo>();
            if (cmbPower1.Text != "")
            {
                list.Add(new AntennaPowerInfo(1, int.Parse(cmbPower1.Text)));
            }
            if (cmbPower2.Text != "")
            {
                list.Add(new AntennaPowerInfo(2, int.Parse(cmbPower2.Text)));
            }
            if (cmbPower3.Text != "")
            {
                list.Add(new AntennaPowerInfo(3, int.Parse(cmbPower3.Text)));
            }
            if (cmbPower4.Text != "")
            {
                list.Add(new AntennaPowerInfo(4, int.Parse(cmbPower4.Text)));
            }
            if (cmbPower5.Text != "")
            {
                list.Add(new AntennaPowerInfo(5, int.Parse(cmbPower5.Text)));
            }
            if (cmbPower6.Text != "")
            {
                list.Add(new AntennaPowerInfo(6, int.Parse(cmbPower6.Text)));
            }
            if (cmbPower7.Text != "")
            {
                list.Add(new AntennaPowerInfo(7, int.Parse(cmbPower7.Text)));
            }
            if (cmbPower8.Text != "")
            {
                list.Add(new AntennaPowerInfo(8, int.Parse(cmbPower8.Text)));
            }
            if (cmbPower9.Text != "")
            {
                list.Add(new AntennaPowerInfo(9, int.Parse(cmbPower9.Text)));
            }
            if (cmbPower10.Text != "")
            {
                list.Add(new AntennaPowerInfo(10, int.Parse(cmbPower10.Text)));
            }
            if (cmbPower11.Text != "")
            {
                list.Add(new AntennaPowerInfo(11, int.Parse(cmbPower11.Text)));
            }
            if (cmbPower12.Text != "")
            {
                list.Add(new AntennaPowerInfo(12, int.Parse(cmbPower12.Text)));
            }
            if (cmbPower13.Text != "")
            {
                list.Add(new AntennaPowerInfo(13, int.Parse(cmbPower13.Text)));
            }
            if (cmbPower14.Text != "")
            {
                list.Add(new AntennaPowerInfo(14, int.Parse(cmbPower14.Text)));
            }
            if (cmbPower15.Text != "")
            {
                list.Add(new AntennaPowerInfo(15, int.Parse(cmbPower15.Text)));
            }
            if (cmbPower16.Text != "")
            {
                list.Add(new AntennaPowerInfo(16, int.Parse(cmbPower16.Text)));
            }

            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            bool result = false;
            byte save =cbSaveAllPower.Checked?(byte)1: (byte)0;

            for (int k = 0; k < list.Count; k++) {
                AntennaPowerInfo antennaPower = list[k];
                int ant = antennaPower.Ant;
                int power = antennaPower.Power;
                result = uhf.SetAntennaPower(save, (byte)ant, (byte)power);
                if (!result)
                {
                    showMessage(msg);
                    return;
                }
            }
            if (!result)
            {
                showMessage(msg);
                return;

            }
            msg = !IsChineseSimple() ? "Success" : "成功!";
            showMessage(msg);
            
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPower1.Text != "")
            {
                cmbPower1.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower2.Text != "")
            {
                cmbPower2.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower3.Text != "")
            {
                cmbPower3.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower4.Text != "")
            {
                cmbPower4.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower5.Text != "")
            {
                cmbPower5.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower6.Text != "")
            {
                cmbPower6.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower7.Text != "")
            {
                cmbPower7.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower8.Text != "")
            {
                cmbPower8.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower9.Text != "")
            {
                cmbPower9.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower10.Text != "")
            {
                cmbPower10.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower11.Text != "")
            {
                cmbPower11.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower12.Text != "")
            {
                cmbPower12.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower13.Text != "")
            {
                cmbPower13.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower14.Text != "")
            {
                cmbPower14.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower15.Text != "")
            {
                cmbPower15.SelectedIndex = comboBox4.SelectedIndex;
            }
            if (cmbPower16.Text != "")
            {
                cmbPower16.SelectedIndex = comboBox4.SelectedIndex;
            }
        }

        private void cbDedebug_Click(object sender, EventArgs e)
        {
                isDebug = cbDedebug.Checked;
                UHFAPI.getInstance().SetDebug(isDebug);
                UHFAPI.getInstance().SaveLog(isDebug);
        }

        private void btnGetReturnloss_Click(object sender, EventArgs e)
        {
                txtReturnloss1.Text = "";
                txtReturnloss2.Text = "";
                txtReturnloss3.Text = "";
                txtReturnloss4.Text = "";
                txtReturnloss5.Text = "";
                txtReturnloss6.Text = "";
                txtReturnloss7.Text = "";
                txtReturnloss8.Text = "";
                txtReturnloss9.Text = "";
                txtReturnloss10.Text = "";
                txtReturnloss11.Text = "";
                txtReturnloss12.Text = "";
                txtReturnloss13.Text = "";
                txtReturnloss14.Text = "";
                txtReturnloss15.Text = "";
                txtReturnloss16.Text = "";

                string msg = !IsChineseSimple() ? "Fail!" : "失败!";
                byte[] rLoss = new byte[512];
                short[] nBytesReturned = new short[1];
                if (UHFAPI.UHFGetReturnLoss(rLoss, nBytesReturned) == 0)
                {
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                    for (int k = 0; k < nBytesReturned[0] / 2; k++)
                    {
                        int ant=rLoss[k*2];
                        int returnLoss=rLoss[k*2+1];
                        switch(ant) {
                            case 1:
                                txtReturnloss1.Text = returnLoss.ToString();
                                break; 
                            case 2:
                                txtReturnloss2.Text = returnLoss.ToString();
                                break;
                            case 3:
                                txtReturnloss3.Text = returnLoss.ToString();
                                break;
                            case 4:
                                txtReturnloss4.Text = returnLoss.ToString();
                                break;
                            case 5:
                                txtReturnloss5.Text = returnLoss.ToString();
                                break;
                            case 6:
                                txtReturnloss6.Text = returnLoss.ToString();
                                break;
                            case 7:
                                txtReturnloss7.Text = returnLoss.ToString();
                                break;
                            case 8:
                                txtReturnloss8.Text = returnLoss.ToString();
                                break;
                            case 9:
                                txtReturnloss9.Text = returnLoss.ToString();
                                break;
                            case 10:
                                txtReturnloss10.Text = returnLoss.ToString();
                                break;
                            case 11:
                                txtReturnloss11.Text = returnLoss.ToString();
                                break;
                            case 12:
                                txtReturnloss12.Text = returnLoss.ToString();
                                break;
                            case 13:
                                txtReturnloss13.Text = returnLoss.ToString();
                                break;
                            case 14:
                                txtReturnloss14.Text = returnLoss.ToString();
                                break;
                            case 15:
                                txtReturnloss15.Text = returnLoss.ToString();
                                break;
                            case 16:
                                txtReturnloss16.Text = returnLoss.ToString();
                                break;
                        }
                    } 
                }
   
            showMessage(msg);
        }

        private void button3_Click(object sender, EventArgs e)
        {
     
            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            if (UHFAPI.UHFSetDefaultMode()==0)
            {
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);
        }

        private void cbANTAll_Click(object sender, EventArgs e)
        {
            cmbAnt1.Checked = cbANTAll.Checked;
            cmbAnt2.Checked = cbANTAll.Checked;
            cmbAnt3.Checked = cbANTAll.Checked;
            cmbAnt4.Checked = cbANTAll.Checked;
            cmbAnt5.Checked = cbANTAll.Checked;
            cmbAnt6.Checked = cbANTAll.Checked;
            cmbAnt7.Checked = cbANTAll.Checked;
            cmbAnt8.Checked = cbANTAll.Checked;
            cmbAnt9.Checked = cbANTAll.Checked;
            cmbAnt10.Checked = cbANTAll.Checked;
            cmbAnt11.Checked = cbANTAll.Checked;
            cmbAnt12.Checked = cbANTAll.Checked;
            cmbAnt13.Checked = cbANTAll.Checked;
            cmbAnt14.Checked = cbANTAll.Checked;
            cmbAnt15.Checked = cbANTAll.Checked;
            cmbAnt16.Checked = cbANTAll.Checked;
        }

        private void btnFilterGet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            byte[] enable = new byte[1];
            int[] filterTime=new int[1];
            if (UHFAPI.GetURTagFilter(enable, filterTime) == 0)
            {
                msg = !IsChineseSimple() ? "Success" : "成功!";
                txtFilterTime.Text = filterTime[0].ToString();
                rbOpenFilterTag.Checked = enable[0] == 1;
                rbCloseFilterTag.Checked= enable[0] == 0;
            }
            showMessage(msg);
            return;
        }

        private void btnFilterSet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            byte enable = 255;
            if (rbOpenFilterTag.Checked)
            {
                enable = 1;
            }
            else if (rbCloseFilterTag.Checked)
            {
                enable = 0;
            }
            if (enable == 255)
            {
                showMessage(msg);
                return;
            }


            if (UHFAPI.SetURTagFilter(enable, int.Parse(txtFilterTime.Text)) == 0)
            {
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);
            return;
        }

        private void btnGetURType_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            byte[] type = new byte[1];
            if (UHFAPI.GetURDeviceType(type) == 0)
            {
                msg = !IsChineseSimple() ? "Success" : "成功!";
                cmbURType.SelectedIndex = type[0];
            }
            showMessage(msg);
            return;
        }

        private void btnSetURType_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            int type = cmbURType.SelectedIndex;
            if (type<0)
            {
                showMessage(msg);
                return;
            }
            if (UHFAPI.SetURDeviceType((byte)type) == 0)
            {
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);
            return;
        }

        private void btnEDCBV10Get_Click(object sender, EventArgs e)
        {
            byte[] statusData = new byte[3];
            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            if (uhf.GetInputStatus_DEV_WYD(statusData))
            {
                comboBox5.SelectedIndex = statusData[0];
                comboBox6.SelectedIndex = statusData[1];
                comboBox10.SelectedIndex = statusData[2];
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);
        }

        private void btnEDCBV10Set_Click(object sender, EventArgs e)
        {
            byte[] outData = new byte[7];
            outData[2] = (byte)comboBox19.SelectedIndex;//relay
            outData[3] = (byte)comboBox8.SelectedIndex;//output1
            outData[4] = (byte)comboBox7.SelectedIndex;//output2
            outData[5] = (byte)comboBox9.SelectedIndex;//output3

            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            if (uhf.SetOutput(outData))
            {

                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);
        }

        private void btnWYDGet_Click(object sender, EventArgs e)
        {
            byte[] statusData = new byte[4];
            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            if (uhf.GetInputStatus_DEV_WYD(statusData))
            {
                comboBox12.SelectedIndex = statusData[0];
                comboBox13.SelectedIndex = statusData[1];
                comboBox11.SelectedIndex = statusData[2];
                comboBox18.SelectedIndex = statusData[3];
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);
        }

        private void btnWYDSet_Click(object sender, EventArgs e)
        {
            byte[] outData = new byte[7];
            outData[2] = (byte)comboBox20.SelectedIndex;//relay
            outData[3] = (byte)comboBox16.SelectedIndex;//output1
            outData[4] = (byte)comboBox15.SelectedIndex;//output2
            outData[5] = (byte)comboBox14.SelectedIndex;//output3
            outData[6] = (byte)comboBox17.SelectedIndex;//output4
            string msg = !IsChineseSimple() ? "Failure" : "失败!";
            if (uhf.SetOutput(outData))
            {

                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            bgUR4.Visible = true;
            gbUR1A.Visible = false;
            gbWYD.Visible = false;
            gbDEV.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            bgUR4.Visible = false;
            gbUR1A.Visible = true;
            gbWYD.Visible = false;
            gbDEV.Visible = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            bgUR4.Visible = false;
            gbUR1A.Visible = false;
            gbWYD.Visible = false;
            gbDEV.Visible = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            bgUR4.Visible = false;
            gbUR1A.Visible = false;
            gbWYD.Visible = true;
            gbDEV.Visible = false;
        }

        private void btnGetBaudrate_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Fail!" : "失败!";
            byte[] data = new byte[1];
            if (UHFAPI.UHFGetBaudrate(data) == 0 && data[0] <= 3)
            {
                msg = !IsChineseSimple() ? "Success" : "成功!";
                comboBox21.SelectedIndex = data[0] - 2;
            }
            showMessage(msg);
        }

        private void btnSetBaudRate_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Fail!" : "失败!";

            if (comboBox21.SelectedIndex >= 0)
            {
                int v = comboBox21.SelectedIndex + 2;
                if (UHFAPI.UHFSetBaudrate((byte)v) == 0)
                {
                    msg = !IsChineseSimple() ? "Success" : "成功!";
                }
            }
            showMessage(msg);
        }

        private void btnGetMAC_Click(object sender, EventArgs e)
        {
            byte[] mac=new byte[6];
            int result=UHFAPI.GetNetMAC(mac);
            if (result == 0)
            {
                string macAddress = string.Format($"{mac[0]:X2}:{mac[1]:X2}:{mac[2]:X2}:{mac[3]:X2}:{mac[4]:X2}:{mac[5]:X2}");
                txtMAC.Text = macAddress;
                showMessage("success");
                return;
            }
            showMessage("Fail");
        }

        private void btnSetMAC_Click(object sender, EventArgs e)
        {
            byte[] macBytes = new byte[6];
            string[] hexMac= txtMAC.Text.Split(':');
            for (int i = 0; i < 6; i++)
            {
                // 将十六进制字符串转换为字节
                if (!byte.TryParse(hexMac[i], System.Globalization.NumberStyles.HexNumber, null, out macBytes[i]))
                {
                    showMessage("Fail");
                    return;
                }
            }
            int result = UHFAPI.SetNetMAC(macBytes);
            if (result == 0)
            {
                showMessage("success");
                return;
            }
            showMessage("Fail");
        }

        private void btnGetWorkTime_Click(object sender, EventArgs e)
        {
            int WorkTime = 0;
            int result=UHFAPI.UHFGetANTWorkTime((byte)(cmbAntNumber.SelectedIndex + 1), ref WorkTime);
            if (result == 0)
            {
                txtWorkTime.Text = WorkTime + "";
                showMessage("success");
                return;
            }
            showMessage("fail");
        }

        private void btnSetWorkTime_Click(object sender, EventArgs e)
        {
            int save= checkBox1.Checked ? 1 : 0;
            int result = UHFAPI.UHFSetANTWorkTime((byte)(cmbAntNumber.SelectedIndex + 1),(byte)save, int.Parse(txtWorkTime.Text));
            if (result == 0)
            {
                showMessage("success");
                return;
            }
            showMessage("fail");
        }

        private void btnlbtGet_Click(object sender, EventArgs e)
        {
     
        }

        private void btnLbtSet_Click(object sender, EventArgs e)
        {
            int result = -1;
            if (rbLBTOpen.Checked)
            {
                //byte saveFlag, byte cr, byte code, byte protection, byte id)
                result = UHFAPI.UHFSetFastInventoryEX(0, 1, 2,0,0);
            }
            else if (rbLBTClose.Checked)
            {
                result = UHFAPI.UHFSetFastInventoryEX(0, 0, 2, 0, 0);
            }

            if (result == 0)
            {
                showMessage("success");
                return;
            }
            showMessage("fail");
        }
    }
}




/**
 * 
 * 
           List<AntennaPowerInfo> list = new List<AntennaPowerInfo>();
            if (cmbPower1.Text!="") {
                list.Add(new AntennaPowerInfo(1,int.Parse(cmbPower1.Text)));
            }
            if (cmbPower2.Text != "")
            {
                list.Add(new AntennaPowerInfo(2, int.Parse(cmbPower2.Text)));
            }
            if (cmbPower3.Text != "")
            {
                list.Add(new AntennaPowerInfo(3, int.Parse(cmbPower3.Text)));
            }
            if (cmbPower4.Text != "")
            {
                list.Add(new AntennaPowerInfo(4, int.Parse(cmbPower4.Text)));
            }
            if (cmbPower5.Text != "")
            {
                list.Add(new AntennaPowerInfo(5, int.Parse(cmbPower5.Text)));
            }
            if (cmbPower6.Text != "")
            {
                list.Add(new AntennaPowerInfo(6, int.Parse(cmbPower6.Text)));
            }
            if (cmbPower7.Text != "")
            {
                list.Add(new AntennaPowerInfo(7, int.Parse(cmbPower7.Text)));
            }
            if (cmbPower8.Text != "")
            {
                list.Add(new AntennaPowerInfo(8, int.Parse(cmbPower8.Text)));
            }
            if (cmbPower9.Text != "")
            {
                list.Add(new AntennaPowerInfo(9, int.Parse(cmbPower9.Text)));
            }
            if (cmbPower10.Text != "")
            {
                list.Add(new AntennaPowerInfo(10, int.Parse(cmbPower10.Text)));
            }
            if (cmbPower11.Text != "")
            {
                list.Add(new AntennaPowerInfo(11, int.Parse(cmbPower11.Text)));
            }
            if (cmbPower12.Text != "")
            {
                list.Add(new AntennaPowerInfo(12, int.Parse(cmbPower12.Text)));
            }
            if (cmbPower13.Text != "")
            {
                list.Add(new AntennaPowerInfo(13, int.Parse(cmbPower13.Text)));
            }
            if (cmbPower14.Text != "")
            {
                list.Add(new AntennaPowerInfo(14, int.Parse(cmbPower14.Text)));
            }
            if (cmbPower15.Text != "")
            {
                list.Add(new AntennaPowerInfo(15, int.Parse(cmbPower15.Text)));
            }
            if (cmbPower16.Text != "")
            {
                list.Add(new AntennaPowerInfo(16, int.Parse(cmbPower16.Text)));
            }
         

            string msg = !IsChineseSimple() ? "Failure!" : "失败!";
            bool result = false;
            byte save = cbSaveAllPower.Checked ? (byte)1 : (byte)0;

            byte[] data = new byte[list.Count*2];
            for (int k = 0; k < list.Count; k++)
            {
                data[k * 2] = (byte)list[k].Ant;
                data[k * 2 + 1] = (byte)list[k].Power;
            }
 
            result = uhf.SetMultiAntennaPower(save, (byte)list.Count, data);
            if (!result)
            {
                showMessage(msg);
                return;
            }

            if (!result)
            {
                showMessage(msg);
                return;

            }
            msg = !IsChineseSimple() ? "Success" : "成功!";
            showMessage(msg);
*/