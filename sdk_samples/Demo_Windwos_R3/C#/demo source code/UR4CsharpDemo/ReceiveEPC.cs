using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections;
using UHFAPP.interfaces;
using UHFAPP.Receive;
using UHFAPP.utils;
using BLEDeviceAPI;

namespace UHFAPP
{
    public partial class ReceiveEPC : BaseForm
    {
       private const int max =1024 * 1024;
       private byte[] uhfOriginalData = new byte[max];
 
       private bool isRuning = true;
       private bool isOpen = false;
    

       int total = 0;
       long beginTime = System.Environment.TickCount;

       List<EpcInfo> epcList = new List<EpcInfo>();
 

       // 将text更新的界面控件的委托类型
       delegate void SetTextCallback(string epc, string tid, string rssi, string count, string ant, string user);
       SetTextCallback setTextCallback;



       delegate void GetRemotelyIPCallback(string remoteip);
       GetRemotelyIPCallback RemotelyIPCallback;
        public ReceiveEPC()
        {
            InitializeComponent();
        }
        private void ReceiveEPC_Load(object sender, EventArgs e)
        {
            MainForm.eventMainSizeChanged += MainForm_SizeChanged;
            setTextCallback = new SetTextCallback(UpdataEPC);
            cmbMode.SelectedIndex = 0;
            RemotelyIPCallback = new GetRemotelyIPCallback(GetRemoteIP);
            InitIPAndSerialPort();
            if (!IsChineseSimple()) {
                label7.Text = "Count:";
            }
        }
        private void ReceiveEPC_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.eventMainSizeChanged -= MainForm_SizeChanged;
            isOpen = false;
            isRuning = false;
            DisConnect();
        }

        private void btnScanEPC_Click(object sender, EventArgs e)
        {
            if (btnScanEPC.Text == "Start")
            {
                if (Connect())
                {
                    isRuning = true;
                    cmbCom.Enabled = false;
                    cmbMode.Enabled = false;
                    btnScanEPC.Text = "Stop";
                    new Thread(new ThreadStart(delegate { ReadEPC(); })).Start();
                }
                else
                {
                    MessageBox.Show("fail");
                }
            }
            else
            {
                isRuning = false;
                cmbCom.Enabled = true;
                cmbMode.Enabled = true;
                btnScanEPC.Text = "Start";
                DisConnect();
            }
        }

    
        private void InitIPAndSerialPort()
        {
            string[] ArryPort = System.IO.Ports.SerialPort.GetPortNames();
            cmbCom.Items.Clear();
            for (int i = 0; i < ArryPort.Length; i++)
            {
                cmbCom.Items.Add(ArryPort[i]);
            }
            if (cmbCom.Items.Count > 0)
                cmbCom.SelectedIndex = cmbCom.Items.Count - 1;
           
        }

        private bool Connect()
        {
           // int ComPort = int.Parse(cmbCom.SelectedItem.ToString().ToString().Replace("COM", ""));
            if (cmbMode.SelectedIndex == 0)
            {
                bool result = UHFAPI.BindUDP(int.Parse(textBox2.Text)) == 0;
                isOpen = result;
                return result;
            }
            else
            {
                int ComPort = int.Parse(cmbCom.SelectedItem.ToString().ToString().Replace("COM", ""));
                bool result = UHFAPI.getInstance().Open(ComPort);
                isOpen = result;
                return result;

            }
        

        }

        private void DisConnect()
        {
            if (cmbMode.SelectedIndex == 0)
            {
                UHFAPI.UnbindUDP();
                isOpen = false;
            }
            else
            {
                UHFAPI.getInstance().Close();
                isOpen = false;
            }
        }




        //获取epc
        private void ReadEPC()
        {
            try
            {
                beginTime = System.Environment.TickCount;
                while (true)
                {
                    TagInfo tagInfo = uhf.GetTagData();
                    if (tagInfo != null && tagInfo.UhfTagInfo != null)
                    {
                        UHFTAGInfo info = tagInfo.UhfTagInfo;
                        this.BeginInvoke(setTextCallback, new object[] { info.Epc, info.Tid, info.Rssi, "1", info.Ant, info.User });
                    }
                    else
                    {
                        if (isRuning)
                        {
                            Thread.Sleep(5);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                lblTime.Invoke(new EventHandler(delegate
                {
                    lblTime.Text = ((System.Environment.TickCount - beginTime) / 1000) + "(s)";

                }));

            }
            catch (Exception ex)
            {

            }
        }









 
 

        private void button1_Click(object sender, EventArgs e)
        {
            epcList.Clear();
            lvEPC.Items.Clear();
            lblTime.Text = "0";
            lblTotal.Text = "0";
            label3.Text = "0";
            total = 0;
            beginTime = System.Environment.TickCount;
        }


 
        private void UpdataEPC(string epc, string tid, string rssi, string count, string ant, string user)
        {
            bool[] exist = new bool[1];
            int id = CheckUtils.getInsertIndex(epcList, epc, tid, exist);
            if (exist[0])
            {
                lvEPC.Items[id].SubItems[2].Text = tid;
                lvEPC.Items[id].SubItems[3].Text = user;
                lvEPC.Items[id].SubItems[4].Text = rssi;
                lvEPC.Items[id].SubItems[5].Text = (int.Parse(lvEPC.Items[id].SubItems[5].Text) + int.Parse(count)).ToString();
                lvEPC.Items[id].SubItems[6].Text = ant;
            }
            else
            {
                total++;
                ListViewItem lv = new ListViewItem();
                int index = lvEPC.Items.Count + 1;
                lv.Text = index.ToString();
                lv.SubItems.Add(epc);
                lv.SubItems.Add(tid);
                lv.SubItems.Add(user);
                lv.SubItems.Add(rssi);
                lv.SubItems.Add(count);
                lv.SubItems.Add(ant);
                lvEPC.Items.Insert(id, lv);
                lblTotal.Text = total.ToString();
                epcList.Insert(id, new EpcInfo(epc,tid, int.Parse(count), DataConvert.HexStringToByteArray(epc), DataConvert.HexStringToByteArray(tid)));
            }
            label3.Text = (int.Parse(label3.Text) + 1).ToString();
            lblTime.Text = ((System.Environment.TickCount - beginTime) / 1000) + "(s)";
        }

        private void GetRemoteIP(string ip)
        {
            textBox1.Text = ip;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                char[] port = textBox2.Text.ToCharArray();
                for (int k = 0; k < port.Length; k++)
                {
                    if (port[k] != '0' && port[k] != '1' && port[k] != '2' && port[k] != '3' && port[k] != '4' &&
                        port[k] != '5' && port[k] != '6' && port[k] != '7' && port[k] != '8' && port[k] != '9')
                    {
                        textBox2.Text = "";
                        return;
                    }
                }
            }
              
        }

      
        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMode.SelectedIndex == 0) {
                panel2.Visible = false;
                panel1.Visible = true;   
            }
            else if (cmbMode.SelectedIndex == 1) {
                panel1.Visible = false;
                panel2.Visible = true;
            }
        }

        private void MainForm_SizeChanged(FormWindowState state)
        {
            //判断是否选择的是最小化按钮
            panel1.Left = 308;
        }
    }
}
