using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using BLEDeviceAPI;
using UHFAPP.utils;
using UHFAPP.excel;
using WinForm_Test;
using System.IO;
using System.Xml.Linq;
using static UHFAPP.utils.EpcInfo;
using System.Security.Cryptography;
using System.Reflection;
using System.Diagnostics;
using static System.Windows.Forms.AxHost;
using System.Runtime.InteropServices;
using static UHFAPP.UHFAPI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using UHFAPP.barcode;
 

namespace UHFAPP
{
    public partial class ReadEPCForm : BaseForm
    {
        bool isPz = false;
        MainForm mainform;
        string strStart = "Start";
        string strStart2 = "开始";
        string strStop = "Stop";
        string strStop2 = "停止";
        bool isRuning = false;
        public static bool isVisible = false;
        int beginTime = 0;
        int workTime = 0;
        int total = 0;
        bool isInventory = false;


       // static bool isUIFast = true;
        List<EpcInfo> epcList = new List<EpcInfo>();
        // 将text更新的界面控件的委托类型
        delegate void SetTextCallback(string epc, string tid, string rssi, string count, string ant, string user,int phase);
        SetTextCallback setTextCallback;

    
        public ReadEPCForm(bool isOpen, MainForm mainform)
        {
            //设置窗体的双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            InitializeComponent();
            //利用反射设置DataGridView的双缓冲
            Type dgvType = this.dgData.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(this.dgData, true, null);

            if (isOpen)
            {
                panel1.Enabled = true;
            }
            else {
                panel1.Enabled = false;
            }
            this.mainform = mainform;
            cmbFormat.SelectedIndex = 0;
        }

        void MainForm_eventOpen(bool open)
        {
            if (open)
            {
                panel1.Enabled = true;
            }
            else
            {
                panel1.Enabled = false;
                if (btnScanEPC.Text == strStop || btnScanEPC.Text == strStop2)
                {
                    StopEPC(true);
                }
            }
        }

        private void LoadDataGridView()
        {
            //设置自动换行  
            dgData.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //设置自动调整高度  
            setDGVeiwFill();
            foreach (DataGridViewColumn col in dgData.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("宋体", 18F, FontStyle.Regular, GraphicsUnit.Pixel);
            }

            this.dgData.Columns[0].FillWeight = 3;
            this.dgData.Columns[1].FillWeight = 25;//EPC
            this.dgData.Columns[2].FillWeight = 22;//TID
            this.dgData.Columns[3].FillWeight = 20;//USER
            this.dgData.Columns[4].FillWeight = 5;//RSSI 
            this.dgData.Columns[5].FillWeight = 5;//COUNT
            this.dgData.Columns[6].FillWeight = 10;//ANT 
            this.dgData.Columns[7].FillWeight = 10;//Phase 
          //  this.dgData.Columns[7].Visible = false;
            //禁止排序
            foreach (DataGridViewColumn dgvc in dgData.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }

        private void ReadEPCForm_Activated(object sender, EventArgs e)
        {
            if (MainForm.currState == FormWindowState.Normal)
            {
                panel1.Size = new Size(1139 - 50, 673 - 60);
                dgData.Size = new Size(1080, 390);//new Size(1097, 390)
                panel2.Location = new Point(3, 482);
            }
            else if (MainForm.currState == FormWindowState.Maximized)
            {
                panel1.Size = new Size(Size.Width - 350, Size.Height - 50);
                dgData.Size = new Size(Size.Width - 350, Size.Height - 280);
                panel2.Location = new Point(panel2.Location.X, Size.Height - 180);
            }

            if (!first) panel1.Left = 310;// 380;
        }

        private void ScanEPCForm_Load(object sender, EventArgs e)
        {
            LoadDataGridView();
            MainForm.eventOpen += MainForm_eventOpen;
            setTextCallback = new SetTextCallback(UpdataEPC);
            MainForm.eventMainSizeChanged += MainForm_SizeChanged;
 
 
            //private void UpdataEPC(string epc, string tid, string rssi, string count,string ant)
            // UpdataEPC("112233","","","1","1");
            //sUpdataEPC("445566778899", "", "", "1", "1");
            LoadUI();

            MainForm.keyDownEventHandler -= KeyDownEventHandler;
            MainForm.keyDownEventHandler += KeyDownEventHandler;

            MainForm.keyUpEventHandler -= KeyUpEventHandler;
            MainForm.keyUpEventHandler += KeyUpEventHandler;

            Config config = Config.JsonToConfig();
            if (config != null)
            {
         
                if (config.configForm.VisibleCalibration)
                {
                    groupBox13.Visible = true;
                    groupBox13.Location = groupBox3.Location;
                }
               
            }

        }



        private void ScanEPCForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.eventOpen -= MainForm_eventOpen;
            MainForm.eventMainSizeChanged -= MainForm_SizeChanged;
            if (btnScanEPC.Text == strStop || btnScanEPC.Text == strStop2)
            {
                StopEPC(true);
            }
            StopReceiveThread();
        }
        #region  设置过滤

        private void btnSet_Click(object sender, EventArgs e)
        {
            int ptr = int.Parse(txtPtr.Text);
            int leng = int.Parse(filerLen.Text);
            int save = cbSave.Checked ? 1 : 0;

            string txtPtr1 = txtPtr.Text;
            string data = txtData.Text.Replace(" ", "");
            if (!StringUtils.IsHexNumber(data) && leng > 0)
            {
                MessageBoxEx.Show(this, !IsChineseSimple() ? "Please input filter hexadecimal data!" : "请输入过滤数据!");
                return;
            }
            if ((leng / 8 + (leng % 8 == 0 ? 0 : 1)) * 2 > data.Length)
            {
                MessageBox.Show(!IsChineseSimple() ? "filter data length error!" : "过滤数据和长度不匹配!");  //to do
                return;
            }
            byte bank = 0x01;
            if (rbEPC.Checked)
            {
                bank = 0x01;
            } else if (rbTID.Checked) {
                bank = 0x02;
            } else if (rbUser.Checked) {
                bank = 0x03;
            }

            if (leng == 0) {
                data = "00";
            }

            byte[] buff = DataConvert.HexStringToByteArray(data);
            if (uhf.SetFilter((byte)save, bank, ptr, leng, buff))
            {
                MessageBoxEx.Show(this, !IsChineseSimple() ? "Success!" : "设置过滤成功!");
            }
            else {
                MessageBoxEx.Show(this, !IsChineseSimple() ? "failure!" : "设置过滤失败");
            }
        }
        private void txtData_TextChanged(object sender, EventArgs e)
        {
            FormatHex(txtData);
            string data = txtData.Text.Replace(" ", "");
            if (data.Length > 0)
            {
                label5.Text = ((data.Length / 2) + ((data.Length % 2) == 0 ? 0 : 1)).ToString();  // txtRead_Length.Text = ((data.Length / 4) + ((data.Length % 4) == 0 ? 0 : 1)).ToString();
            }
            else
            {
                label5.Text = "0";
            }

        }
        private void FormatHex(TextBox txt)
        {
            if (isDelete) return;
            string data = txt.Text.Trim().Replace(" ", "");
            if (data != string.Empty)
            {
                int SelectIndex = txt.SelectionStart - 1;
                char[] charData = data.ToCharArray(0, data.Length);
                char[] newChar = new char[charData.Length];
                int index = 0;
                for (int k = 0; k < charData.Length; k++)
                {
                    if (StringUtils.IsHexNumber2(charData[k]))
                    {
                        newChar[index] = charData[k];
                        index++;
                    }
                }
                string newData = new string(newChar, 0, index);
                StringBuilder sb = new StringBuilder();
                int count = (newData.Length / 2) + (newData.Length % 2);

                for (int k = 0; k < count; k++)
                {
                    if ((k * 2 + 2) <= newData.Length)
                    {
                        sb.Append(newData.Substring(k * 2, 2));
                    }
                    else
                    {
                        sb.Append(newData.Substring(k * 2, 1));
                    }
                    sb.Append(" ");
                }
                txt.Text = sb.ToString();

                if (SelectIndex >= 0)
                    txt.SelectionStart = SelectIndex + 2;
                //txt.Select(txt.Text.Length - 1, 0);

            }
        }
        private void txtPtr_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string ptr = txtPtr.Text;
                if (!StringUtils.IsNumber(ptr))
                {
                    if (rbEPC.Checked)
                    {
                        txtPtr.Text = "32";
                    }
                    else
                    {
                        txtPtr.Text = "0";
                    }
                    return;
                }

            }
            catch (Exception ex)
            {
                if (rbEPC.Checked)
                {
                    txtPtr.Text = "32";
                }
                else
                {
                    txtPtr.Text = "0";
                }
            }
        }
        #endregion
        //start
        private void btnScanEPC_Click(object sender, EventArgs e)
        {
            if (btnScanEPC.Text == strStop || btnScanEPC.Text == strStop2)
            {
                StopEPC(true);
                isInventory = false;
            }
            else
            {
                setDGVeiwFill();

                mainform.disableControls(true);
                bool result = false;
                if (cbPhase.Checked)
                {
                    result = uhf.StartInventoryNeedPhase();  
                }
                else
                {
                    
                    result = uhf.StartInventory();
                }
               
                if (result)
                {
                    isInventory = true;
                    groupBox3.Enabled = false;
                    groupBox8.Enabled = false;
                    cmbFormat.Enabled = false;
                    btnScanEPC.Text = !IsChineseSimple() ? strStop : strStop2;
                    StartReceiveThread();

                    //+++++++++++++++++
                    beginTime = System.Environment.TickCount;
                    workTime = 0;
                    if (StringUtils.IsNumber(txtTime.Text))
                    {
                        workTime = int.Parse(txtTime.Text) * 1000;
                    }
                    if (workTime == 0)
                    {
                        workTime = int.MaxValue;
                    }
                    new Thread(new ThreadStart(delegate { Time(); })).Start();
                    //+++++++++++++++++
                }
                else
                {
                    MessageBoxEx.Show(this, "Inventory failure!");
                    mainform.enableControls();
                }
 
            }
        }

        //Clear
        private void button1_Click(object sender, EventArgs e)
        {
            epcList.Clear();
            tempCount = 0;
            label6.Text = "0";
            epcList.Clear();
            dgData.Rows.Clear();
            lblTime.Text = "0";
            lblTotal.Text = "0";
            total = 0;
            beginTime = System.Environment.TickCount;
        }
 
        //停止读取epc
        private void StopEPC(bool isStop) {
            bool reuslt = uhf.StopInventory();
            workTime = 0;
            if (!reuslt)
            {
                MessageBox.Show(!IsChineseSimple() ? "Stop fail" : "停止失败");
            }
 
            groupBox8.Enabled = true;
            cmbFormat.Enabled = true;
            groupBox3.Enabled = true;
            btnScanEPC.Text = !IsChineseSimple() ? strStart : strStart2;
            mainform.enableControls();
            Thread.Sleep(100);
        }

        private void StartReceiveThread()
        {
            if (!isRuning)
            {
                isRuning = true;
                new Thread(new ThreadStart(delegate { ReadEPC(); })).Start();
            }
        }
        private void StopReceiveThread()
        {
            isRuning = false;
            Thread.Sleep(100);
        }
        private void Time()
        {
            Console.WriteLine("Time begin.");
            int temp = workTime;
            int elapsedTime = 0;
            while (System.Environment.TickCount - beginTime <= workTime)
            {
                Thread.Sleep(10);
                elapsedTime = (System.Environment.TickCount - beginTime);
                lblTime.Invoke(new EventHandler(delegate {
                    lblTime.Text = elapsedTime + "ms";
                }));
            }
            if (workTime > 0 && elapsedTime > temp)
            {
                lblTime.Invoke(new EventHandler(delegate {
                    lblTime.Text = temp + "ms";
                }));
            }
     
            uhf.StopInventory();
            Thread.Sleep(100);
            txtTime.Invoke(new EventHandler(delegate
            {
                cmbFormat.Enabled = true;
                gbAuto.Enabled = true;

                groupBox8.Enabled = true;
                btnScanEPC.Text = !IsChineseSimple() ? strStart : strStart2;
                mainform.enableControls();


                for (int index = 0; index < dgData.Rows.Count - 1; index++)
                {
                    this.dgData.Rows[index].Cells[0].Value = index + 1;
                }
                groupBox3.Enabled = true;
            }));
            Console.WriteLine("Time end.");
        }
        //获取epc
        private void ReadEPC()
        {
            Console.WriteLine("ReadEPC begin.");
            while (isRuning)
            {
                try
                {
                    if (!isVisible)
                    {
                        Thread.Sleep(10);
                        // Console.WriteLine("isVisible false");
                        continue;
                    }

                    TagInfo tagInfo = uhf.GetTagData();

                    if (tagInfo != null && tagInfo.UhfTagInfo != null)
                    {

                        UHFTAGInfo info = tagInfo.UhfTagInfo;
                        this.BeginInvoke(setTextCallback, new object[] { info.Epc, info.Tid, info.Rssi, "1", info.Ant, info.User, info.Phase });
                    }
                    else
                    {
                        this.BeginInvoke(setTextCallback, new object[] { null, null, null, null, null, null,0 });
                        Thread.Sleep(10);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            Console.WriteLine("ReadEPC end.");
        }

        int tempCount = 0;
        StringBuilder sb = new StringBuilder(100);
        private void UpdataEPC(string epc, string tid, string rssi, string count, string ant, string user, int phase)
        {
 
            if (epc == null)  
            {
                return;
            }
            label6.Text = (tempCount += int.Parse(count)).ToString();

            bool[] exist = new bool[1];
            int index = CheckUtils.getInsertIndex(epcList, epc, tid, exist);

      
            if (exist[0])
            {
                epcList[index].AddAntennaInfoByAnt(int.Parse(ant), rssi, phase);

                List<AntennaInfo> list = epcList[index].AntList;
                StringBuilder stringBuilderANT = new StringBuilder();
                StringBuilder stringBuilderRSSI = new StringBuilder();
                StringBuilder stringBuilderPhase = new StringBuilder();
                for (int k = 0; k < list.Count; k++)
                {
                    stringBuilderANT.Append("ANT");
                    stringBuilderANT.Append(list[k].AntennaPort);
                    stringBuilderANT.Append(": ");
                    stringBuilderANT.Append(list[k].Count);
                    stringBuilderRSSI.Append(list[k].Rssi);
                    if (list[k].Phase == -1)
                    {
                        stringBuilderPhase.Append("");
                    }
                    else
                    {
                        stringBuilderPhase.Append(list[k].Phase);
                    }
      
                    if (k != list.Count - 1)
                    {
                        stringBuilderANT.Append(System.Environment.NewLine);
                        stringBuilderRSSI.Append(System.Environment.NewLine);
                        stringBuilderPhase.Append(System.Environment.NewLine);
                    }
                }
                epcList[index].Count = epcList[index].Count + 1;
                epcList[index].User = user;
                epcList[index].Tid = tid;
                epcList[index].TidBytes = DataConvert.HexStringToByteArray(tid);
               // epcList[index].Phase = phase;

                if (cmbFormat.SelectedIndex == 2)
                {
                    if (!string.IsNullOrEmpty(epc))
                    {
                        epc = epc + System.Environment.NewLine + "Ascii:" + System.Text.Encoding.ASCII.GetString(DataConvert.HexStringToByteArray(epc));
                    }

                    if (!string.IsNullOrEmpty(user))
                    {
                        user = user + System.Environment.NewLine + "Ascii:" + System.Text.Encoding.ASCII.GetString(DataConvert.HexStringToByteArray(user));
                    }
                }
                else if (cmbFormat.SelectedIndex == 1)
                {
                    if (!string.IsNullOrEmpty(epc))
                    {
                        epc = System.Text.Encoding.ASCII.GetString(DataConvert.HexStringToByteArray(epc));
                    }

                    if (!string.IsNullOrEmpty(user))
                    {
                        user = System.Text.Encoding.ASCII.GetString(DataConvert.HexStringToByteArray(user));
                    }
                }
                this.dgData.Rows[index].Cells[1].Value = epc;
                this.dgData.Rows[index].Cells[2].Value = tid;
                this.dgData.Rows[index].Cells[3].Value = user;
                this.dgData.Rows[index].Cells[4].Value = stringBuilderRSSI.ToString();
                this.dgData.Rows[index].Cells[5].Value = epcList[index].Count;
                this.dgData.Rows[index].Cells[6].Value = stringBuilderANT.ToString();

                this.dgData.Rows[index].Cells[7].Value = stringBuilderPhase.ToString(); 
            }
            else {
                EpcInfo epcInfo = new EpcInfo(epc, tid, int.Parse(count), DataConvert.HexStringToByteArray(epc), DataConvert.HexStringToByteArray(tid), int.Parse(ant), rssi, user, phase);
                epcList.Insert(index, epcInfo);

                total++;
                if (cmbFormat.SelectedIndex == 2)
                {
                    if (!string.IsNullOrEmpty(epc))
                    {
                        epc = epc + System.Environment.NewLine + "Ascii:" + System.Text.Encoding.ASCII.GetString(DataConvert.HexStringToByteArray(epc));
                    }

                    if (!string.IsNullOrEmpty(user))
                    {
                        user = user + System.Environment.NewLine + "Ascii:" + System.Text.Encoding.ASCII.GetString(DataConvert.HexStringToByteArray(user));
                    }
                }
                else if (cmbFormat.SelectedIndex == 1)
                {
                    if (!string.IsNullOrEmpty(epc))
                    {
                        epc = System.Text.Encoding.ASCII.GetString(DataConvert.HexStringToByteArray(epc));
                    }

                    if (!string.IsNullOrEmpty(user))
                    {
                        user = System.Text.Encoding.ASCII.GetString(DataConvert.HexStringToByteArray(user));
                    }
                }

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("ANT");
                stringBuilder.Append(ant);
                stringBuilder.Append(": 1");
                object[] values = new object[] { (index + 1), epc, tid, user, rssi, 1, stringBuilder.ToString(), phase >= 0?(phase+"") :"" };
                dgData.Rows.Insert(index, values);

                lblTotal.Text = (dgData.RowCount - 1).ToString();  

            }
            //if (epc.Length > 40 || (user != null && user.Length > 40))
            //{
            //    AutoCellsWidth(true);
            //}

        }


        private void button2_Click(object sender, EventArgs e)
        {
            int save = cbSave.Checked ? 1 : 0;
            if (uhf.SetFilter((byte)save, 1, 4, 0, new byte[] { 0 }) &&
                uhf.SetFilter((byte)save, 2, 4, 0, new byte[] { 0 }) &&
                uhf.SetFilter((byte)save, 3, 4, 0, new byte[] { 0 }))
            {
                MessageBoxEx.Show(this, !IsChineseSimple() ? "Success!" : "重置成功!");
            }
            else
            {
                MessageBoxEx.Show(this, !IsChineseSimple() ? "failure!" : "重置失败!");
            }
        }



        private void dgData_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnScanEPC.Text == strStop || btnScanEPC.Text == strStop2)
            {
                StopEPC(true);
            }

            int Col_index = dgData.CurrentCell.ColumnIndex;
            if (Col_index == 1 || Col_index == 2 || Col_index == 3)
            {
                int Row_index = dgData.CurrentRow.Index;
                EpcInfo epcinfo = epcList[Row_index];
                string data = "";
                if (Col_index == 1)
                {
                    data = epcinfo.Epc;
                }
                else if (Col_index == 2)
                {
                    data = epcinfo.Tid;
                }
                else if (Col_index == 3)
                {
                    data = epcinfo.User;
                }

                if (!string.IsNullOrEmpty(data))
                {
                    mainform.ReadWriteTag(data, Col_index);
                    Common.tag = data;
                    Common.bank = Col_index;
                }
            }
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {

            int Col_index = dgData.CurrentCell.ColumnIndex;
            if (Col_index == 1 || Col_index == 2 || Col_index == 3)
            {
                int Row_index = dgData.CurrentRow.Index;
                if (epcList.Count < Row_index + 1) {
                    return;
                }
                EpcInfo epcinfo = epcList[Row_index];
                string data = "";
                if (Col_index == 1)
                {
                    data = epcinfo.Epc;
                }
                else if (Col_index == 2)
                {
                    data = epcinfo.Tid;
                }
                else if (Col_index == 3)
                {
                    data = epcinfo.User;
                }

                if (!string.IsNullOrEmpty(data))
                {

                    Clipboard.SetDataObject(data);
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string ptr = filerLen.Text;
                if (!StringUtils.IsNumber(ptr))
                {
                    filerLen.Text = "0";
                    return;
                }
            }
            catch (Exception ex)
            {
                filerLen.Text = "0";
            }
        }

        private void rbTID_Click(object sender, EventArgs e)
        {
            txtPtr.Text = "0";
        }

        private void rbUser_Click(object sender, EventArgs e)
        {
            txtPtr.Text = "0";
        }

        private void rbEPC_Click(object sender, EventArgs e)
        {
            txtPtr.Text = "32";
        }


        bool isDelete = false;
        private void ReadEPCForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                isDelete = true;
            }
            else
            {
                isDelete = false;
            }
            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2 || e.KeyCode == Keys.F3 || e.KeyCode == Keys.F4)
            {
                btnExport.Visible = true;
            }
        }

   

        private void btnExport_Click(object sender, EventArgs e)
        {

            string path = System.Environment.CurrentDirectory + "\\uhfExportData";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + "\\uhf_" + DateTime.Now.ToString("yyyy_MM_dd_HHssmm") + ".xls";

            ExcelUtils.ExportExcel(path, epcList, cmbFormat.SelectedIndex,cbPhase.Checked,true,true,true);

        }

        private void rb2s_Click(object sender, EventArgs e)
        {
            txtTime.Text = "2";
        }

        private void rb3s_Click(object sender, EventArgs e)
        {
            txtTime.Text = "3";
        }

        private void rb4s_Click(object sender, EventArgs e)
        {
            txtTime.Text = "4";
        }

        private void rb5s_Click(object sender, EventArgs e)
        {
            txtTime.Text = "5";
        }

        private void rb10_Click(object sender, EventArgs e)
        {
            txtTime.Text = "10";
        }

        static bool first = true;
        private void MainForm_SizeChanged(FormWindowState state)
        {

            //判断是否选择的是最小化按钮
            if (!first)
            {
                panel1.Left = 308;
            }
            first = false;
            // this.WindowState = FormWindowState.Maximized ;
            if (state == FormWindowState.Normal)
            {
                panel1.Size = new Size(1139 - 50, 673 - 60);
                dgData.Size = new Size(1080, 390);//  dgData.Size = new Size(1097, 390);
                panel2.Location = new Point(3, 482);
            }
            else if (state == FormWindowState.Maximized)
            {
                panel1.Size = new Size(Size.Width - 350, Size.Height - 50);
                dgData.Size = new Size(Size.Width - 350, Size.Height - 280);
                panel2.Location = new Point(panel2.Location.X, Size.Height - 180);
                if (!isInventory) {
                    setDGVeiwFill();
                }
            }
        }

        private void LoadUI()
        {

            if (!IsChineseSimple())
            {
                btnExport.Text = "Export Data";
                contextMenuStrip1.Items[0].Text = "Copy Tag";
                groupBox8.Text = "Filter";
                label29.Text = "Data:";
                label30.Text = "Ptr:";
                cbSave.Text = "Save";
                btnSet.Text = "Set";
                button2.Text = "reset";
                lto.Text = "Tag Count:";
                label7.Text = "Total:";
                label2.Text = "Time:";
                button1.Text = "Clear";
                label1.Text = "Len:";

                rb2s.Text = "2S";
                rb3s.Text = "3S";
                rb4s.Text = "4S";
                rb5s.Text = "5S";
                rb10.Text = "10S";
                label11.Text = "S";
                label10.Text = "Work Time:";
                label12.Text = "Data format:";
                btnExport.Text = "Export Data";
                if (btnScanEPC.Text == strStart2)
                {
                    btnScanEPC.Text = strStart;
                }
                else if (btnScanEPC.Text == strStop2)
                {
                    btnScanEPC.Text = strStop;
                }
            }
            else
            {
                btnScanEPC.Text = "开始";
                button1.Text = "清空";
                contextMenuStrip1.Items[0].Text = "复制标签";

            }

        }
 

        private void setDGVeiwFill()
        {
            dgData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; //  DisplayedCells
            dgData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;


            if (cmbFormat.SelectedIndex == 2)
            {
                dgData.RowTemplate.Height = 50;
            }
            else
            {
                dgData.RowTemplate.Height = 30;
            }
    
        }
        
        public void KeyDownEventHandler(int keyCode)
        {
            Console.WriteLine("KeyDownEventHandler");
        }

        public void KeyUpEventHandler(int keyCode)
        {
            Console.WriteLine("KeyUpEventHandler");
        }

        private void ReadEPCForm_VisibleChanged(object sender, EventArgs e)
        {
            if (((ReadEPCForm)sender).Visible)
            {
                isVisible = true;
            }
            else
            {
              //  isVisible =false;
            }
              
        }

        private void cbPhase_CheckedChanged(object sender, EventArgs e)
        {
            //this.dgData.Columns[7].Visible = cbPhase.Checked;
        }
        int flagC = 0;

        private void cmbFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1_Click(null,null);
        }

        private void groupBox13_Enter(object sender, EventArgs e)
        {

        }

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
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);
        }

        private void showMessage(string msg)
        {
            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                System.Threading.Thread.Sleep(500);
            }, msg);
            f.ShowDialog(this);
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
 

            byte[] ant = new byte[4] { (byte)b1, (byte)b2, (byte)b3, (byte)b4 };
            byte flag = cbAntSet.Checked ? (byte)1 : (byte)0;
            if (uhf.SetANTTo32(flag, ant))
            {
                msg = !IsChineseSimple() ? "Success" : "成功!";
            }
            showMessage(msg);
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            int result = uhf.CalibrationVoltage();
            txtCalibration.Text = result + "";
        }
    }
}


/**
 * 
 * 
 * 
                     //************
                    UHFTAGInfo uHFTAG = new UHFTAGInfo();
                    uHFTAG.Ant = "1";
                    uHFTAG.Epc = (flagC++) + "";
                    uHFTAG.Rssi = "-20";
                    uHFTAG.Pc = "3000";
                    if (uHFTAG.Epc.Length % 2 != 0)
                    {
                        uHFTAG.Epc = uHFTAG.Epc + "0";
                    }
                    TagInfo tagInfo = new TagInfo();
                    tagInfo.UhfTagInfo = uHFTAG;
                    

                    if (flagC % 20 == 0)
                    {
                        Thread.Sleep(1);
                    }
                    //***********



    
    List<TagInfo> listUHFTAG=new List<TagInfo>();
    int w=0;
int r=0;


               int flagC = 0;
                Thread t = new Thread(new ThreadStart(delegate {
                    while (isInventory)
                    {
                        UHFTAGInfo uHFTAG = new UHFTAGInfo();
                        uHFTAG.Ant = "1";
                        uHFTAG.Epc = (flagC++) + "";
                        uHFTAG.Rssi = "-20";
                        uHFTAG.Pc = "3000";
                        if (uHFTAG.Epc.Length % 2 != 0)
                        {
                            uHFTAG.Epc = uHFTAG.Epc + "0";
                        }
                        TagInfo tag = new TagInfo();
                        tag.UhfTagInfo = uHFTAG;
                        listUHFTAG[w++] = tag;

                        if (w % 20 == 0)
                        {
                            Thread.Sleep(1);
                        }

                    }
                }));
                t.Priority = ThreadPriority.Highest;
                t.Start();


                    TagInfo tagInfo = null; 
                    if (w > r)
                    {
                        tagInfo = listUHFTAG[r++];
                    }



private void AutoCellsWidth(bool auto)
        {
            if (!isUIFast)
            {
                if (auto)
                {
                    if (dgData.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.None)
                    {
                        dgData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                        dgData.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        // dgData.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    }
                }
                else
                {

                    if (dgData.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.Fill)
                    {
                        dgData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dgData.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                    }

                }
            }


        }
**/