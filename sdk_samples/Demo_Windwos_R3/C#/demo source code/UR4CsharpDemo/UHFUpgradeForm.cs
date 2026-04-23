using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using FileDownload;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using static System.Net.WebRequestMethods;
using System.Globalization;

namespace UHFAPP
{
    public partial class UHFUpgradeForm : BaseForm
    {
        string path = "";
        public bool isConnect = true;
        public UHFUpgradeForm(bool isEnglish)
        {
            InitializeComponent();
            if (isEnglish)
            {
                //  label1.Text = "Path:";
                btnPath.Text = "Select file";
                btnStart.Text = "Upgrade";
                rbReaderApplication.Text = "Mainboard Firmware";
                rbUHFModule.Text = "UHF firmware";
                label2.Text = "Firmware Version:";

            }
            else
            {
                // label1.Text = "文件路径:";
                btnPath.Text = "选择文件";
                btnStart.Text = "升级";
                rbReaderApplication.Text = "主板固件";
                rbUHFModule.Text = "UHF固件";
                label2.Text = "版本号:";
            }

            Config config = Config.JsonToConfig();
            if (config != null)
            {
                if (config.upgradeForm.VisibleEx10SDK)
                {
                    rbEx10SDKFirmware.Visible = true;   
                }
                if (config.upgradeForm.VisibleReaderBootloader)
                {
                    rbReaderBootloader.Visible = true;
                }
           
            }

        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "bin|*.bin";
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                // 显示文件路径名
                txtPath.Text = openDlg.FileName;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            path = txtPath.Text;
            if (path == null || path.Length == 0)
            {
                MessageBox.Show("fail");
                return;
            }
            if (!path.EndsWith(".bin"))
            {
                MessageBox.Show(!IsChineseSimple() ? "fail!" : "升级失败, 升级文件必须是.bin文件!");
                return;
            }
            btnDownload.Enabled = false;
            btnPath.Enabled = false;
            btnStart.Enabled = false;
            this.ControlBox = false;
            new Thread(new ThreadStart(startUpdate)).Start();
        }
        
        bool isOpen = true;
        private void startUpdate()
        {
            setMsg("Updating......", true);

            FileStream stream = null;
            BinaryReader binary = null;
            byte type = 0;
            try
            {
            
                Cursor.Current = Cursors.WaitCursor;
                setPprogress(0);
                stream = new FileStream(path, FileMode.Open);
                binary = new BinaryReader(stream);

             
                string strversion = "";

                this.Invoke(new EventHandler(delegate
                {
                    if (rbUHFModule.Checked)
                    {
                        strversion = "UHF version:" + uhf.GetSoftwareVersion();
                        type = 1;
                    }
                    else if (rbReaderApplication.Checked)
                    {
                        type = 0;
                        strversion = "Mainboard version:" + uhf.GetSTM32Version();
                    }
                    else if (rbReaderBootloader.Checked)
                    {
                        type = 2;
                    }
                    else if (rbEx10SDKFirmware.Checked)
                    {
                        strversion = "Ex10SDK version:" + uhf.GetHardwareVersion();
                        type = 3;
                    }
                }));

                this.Invoke(new EventHandler(delegate
                {
                    label2.Text = strversion;
                }));


                if (!uhf.jump2Boot(type))
                {
                    setMsg("uhfJump2Boot fail", true);
                    //return;
                }
                Thread.Sleep(2000);

      


                if (MainForm.MODE == 1)
                {
                    if (type != 1 && type != 3)
                    {
                        setMsg(!IsChineseSimple() ? "Disconnect!" : "断开连接", true);
                        uhf.TcpDisconnect();
                        Thread.Sleep(1000);
                        setMsg(!IsChineseSimple() ? "Connect!" : "开始连接", true);
                        bool result = uhf.TcpConnect(MainForm.ip, MainForm.portData);
                        if (!result)
                        {
                            setMsg("TcpConnect fail", true);
                            return;
                        }
                        setMsg(!IsChineseSimple() ? "Successfully connected" : "连接成功!", true);
                        Thread.Sleep(1000);
                    }
                }
                else if (MainForm.MODE == 2)
                {
                    Thread.Sleep(2000);
                    uhf.CloseUsb();
                    Thread.Sleep(2000);
                  
                    for (int k = 0; k < 2; k++)
                    {
                        isOpen = uhf.OpenUsb();
                       Console.WriteLine("uhf.OpenUsb() result="+ isOpen);
                        if (isOpen)
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    if (!isOpen) {

                        return;
                    }
                }
                else if (MainForm.MODE == 0)
                {
                    if (type != 1 && type != 3)
                    {
                        Thread.Sleep(2000);
                        uhf.Close();
                        Thread.Sleep(2000);

                        isOpen = uhf.Open(MainForm.ComPort);
                        // setMsg("Open com"+ MainForm.ComPort, true);
                        Console.WriteLine("uhf.OpenUsb() result=" + isOpen);
                        if (!isOpen)
                        {

                            return;
                        }
                    }
                }
                Console.WriteLine("uhf.startUpd()");

                int MaxLen = uhf.UHFStartUpgrade();
                if (MaxLen == -1)
                {
                    setMsg("uhfStartUpdate fail", true);
                    return;
                }
                Console.WriteLine("MaxLen=" + MaxLen);
                long uFileSize = stream.Length;
                int packageCount = (int)(uFileSize / MaxLen) + (uFileSize % MaxLen > 0 ? 1 : 0);

                //if (!uhf.startUpd())
                //{
                //    setMsg("uhfStartUpdate fail", true);
                //    return;
                //}
                Thread.Sleep(2000);
                for (int k = 0; k < packageCount; k++)
                {
                    try
                    {

                        byte[] data = binary.ReadBytes(MaxLen);
                        // setMsg("uhfUpdating  packageCount=" + packageCount + "       " + k, true);
                        if (uhf.updating(data, data.Length))
                        {

                            double r = Math.Round(((double)(k + 1) / (double)packageCount), 2) * 100;
                            setPprogress((int)r);
                        }
                        else
                        {

                            setMsg("uhfUpdating fail ,package=" + k, true);
                            uhf.stopUpdate();
                            return;
                        }
                        Thread.Sleep(5);
                    }
                    catch (Exception e)
                    {
                        setMsg("ex=" + e.Message, true);
                    }

                }
                setPprogress(100);

            }
            catch (Exception ex)
            {
                setMsg("ex=" + ex.Message, true);
            }
            finally
            {
                try
                {
                    bool isSuccess = isOpen && uhf.stopUpdate();
                    if (isOpen)
                    {
                        Thread.Sleep(4000);
                        getVersion(type);
                    }
                    isConnect = isOpen;
                    if (binary != null)
                    {
                        binary.Close();
                    }
                    if (stream != null)
                    {
                        stream.Close();
                    }
                    Cursor.Current = Cursors.Default;
                    if (isSuccess)
                    {
                        setMsg(!IsChineseSimple() ? "Upgrade completed!" : "升级完成!", true);
                    }
                    else
                    {
                        setMsg(!IsChineseSimple() ? "Upgrade failed!" : "升级失败!", true);
                    }

                 
                    btnPath.Invoke(new EventHandler(delegate
                    {
                        btnPath.Enabled = true;
                        btnStart.Enabled = true;
                        ControlBox = true;
                        btnDownload.Enabled = true;
                    }));


                }
                catch (Exception e)
                {
                    setMsg("222 ex=" + e.Message, true);

                    btnPath.Invoke(new EventHandler(delegate
                    {
                        btnPath.Enabled = true;
                        btnStart.Enabled = true;
                        ControlBox = true;
                        btnDownload.Enabled = true;
                    }));
                }

            }

            

        }

        private void setPprogress(int progress)
        {
            progressBar1.Invoke(new EventHandler(delegate
            {
                progressBar1.Value = progress;
            }));
        }

      
        private void setMsg(string msg,bool isAppend)
        {
            textBox1.Invoke(new EventHandler(delegate
            {
                if (isAppend)
                {
                    if (textBox1.Text.Length > 2000)
                    {
                        textBox1.Text = msg;
                    }
                    else
                    {
                        textBox1.AppendText("\r\n");
                        textBox1.AppendText(msg);
                      
                    }
                }
                else
                {
                    textBox1.Text = msg;
                }
            }));
        }

        private void getVersion(int type)
        {
          
            label2.Invoke(new EventHandler(delegate
            {
           
                if (type == 0)
                {
                    if (MainForm.MODE == 1)
                    {
                        if (type != 1 && type != 3)
                        {
                            setMsg(!IsChineseSimple() ? "Disconnect" : "断开连接", true);
                            uhf.TcpDisconnect();
                            Thread.Sleep(1000);
                            setMsg(!IsChineseSimple() ? "Connect" : "开始连接", true);
                            bool result = uhf.TcpConnect(MainForm.ip, MainForm.portData);
                            if (!result)
                            {
                                setMsg("TcpConnect fail", true);
                                return;
                            }
                        }
                    }
                    else if (MainForm.MODE == 0)
                    {
                        if (type != 1 && type != 3)
                        {
                            Thread.Sleep(1000);
                            uhf.Close();
                            Thread.Sleep(1000);
                            isOpen = uhf.Open(MainForm.ComPort);
                            Console.WriteLine("uhf.OpenUsb() result=" + isOpen);
                            if (!isOpen)
                            {
                                return;
                            }
                        }
                    }
                    else if (MainForm.MODE == 2)
                    {
                        Thread.Sleep(1000);
                        uhf.CloseUsb();
                        Thread.Sleep(1000);
                        isOpen = uhf.OpenUsb();
                        Console.WriteLine("uhf.OpenUsb() result=" + isOpen);
                        if (!isOpen)
                        {
                            return;
                        }
                    }

                    MessageBox.Show("uhf version:" + uhf.GetSTM32Version());
                    label2.Text = "uhf version:" + uhf.GetSTM32Version();
                }


                if (type==1)
                {
                    label2.Text = "UHF version:" + uhf.GetSoftwareVersion();
                    MessageBox.Show(label2.Text);
                }
                else if (type==0)
                {
                    label2.Text = "Mainboard version:" + uhf.GetSTM32Version();
                    MessageBox.Show(label2.Text);
                }
                else if (type == 2)
                {
                    
                }
                else if (type == 3)
                {
                    label2.Text = "Ex10SDK version:" + uhf.GetHardwareVersion();
                    MessageBox.Show(label2.Text);
                }
                 
            }));
           
        }

        void MainForm_eventOpen(bool open)
        {
             
        }

        private void UHFUpgradeForm_Load(object sender, EventArgs e)
        {
            LoadUI();
        }

        private void LoadUI()
        {
            if (!IsChineseSimple())
            {
                //label1.Text = "path:";
                btnPath.Text = "Select file";
                btnStart.Text = "Upgrade";
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            gbRemote.Visible = checkBox1.Checked;
        }
        /// <summary>
        /// 下载远程数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (txtHttpURL.Text == null || txtHttpURL.Text.Length == 0)
            {
                MessageBox.Show("下载地址不能为空!");
                return;
            }
 
            string path=System.AppDomain.CurrentDomain.BaseDirectory;
            fileName = txtHttpURL.Text.Substring(txtHttpURL.Text.LastIndexOf("/") + 1);
            FileDownload.FileInfo fileInfo = new FileDownload.FileInfo(txtHttpURL.Text, path, fileName);



            Downloader download = new Downloader(fileInfo);
            download.DownloadStartingEvent += new Downloader.DelProInfoArg(download_DownloadStartingEvent);//事件订阅，开始下载
            download.DownloadedEvent += new Downloader.DelProInfoArg(download_DownloadedEvent);//事件订阅，新获取了数据表，更新显示
            download.DownloadEndEvent += new Downloader.DelProInfoArg(download_DownloadEndEvent);//事件订阅，一个文件下载完成
            download.DownloadException += new Downloader.DeldownloadException(downloadException);//事件订阅，所有的下载任务结束
            download.Start();

            btnDownload.Enabled = false;
            btnPath.Enabled = false;
            btnStart.Enabled = false;
        }



        #region 订阅事件，显示下载进度信息

        string fileName = "";

        void downloadException(Exception ex)
        {
            setMsg("下载失败! ex="+ ex.Message, true);
  
            txtPath.Invoke(new EventHandler(delegate
            {
                btnDownload.Enabled = true;
                btnPath.Enabled = true;
                btnStart.Enabled = true;
            }));
        }



        void download_DownloadEndEvent(FileDownload.FileInfo _file)
        {
            int Downloaded = (int)_file.DownloadedSize / 1024;
            int ALLdownload = (int)_file.SumSize / 1024;

            string strDownloadingInfo = string.Format("下载进度:{0}", 100);
            setMsg(strDownloadingInfo, true);

            strDownloadingInfo = string.Format("{0}  已下载完成！", System.AppDomain.CurrentDomain.BaseDirectory+ _file.FileName);
            setMsg(strDownloadingInfo, true);

            setPprogress(100);
     
            txtPath.Invoke(new EventHandler(delegate
            {
                txtPath.Text = System.AppDomain.CurrentDomain.BaseDirectory + _file.FileName;
                btnDownload.Enabled = true;
                btnPath.Enabled = true;
                btnStart.Enabled = true;

                btnStart_Click(null,null);
            }));
           
        }



        void download_DownloadedEvent(FileDownload.FileInfo _file)
        {
            int Downloaded = (int)_file.DownloadedSize / 1024;
            int ALLdownload = (int)_file.SumSize / 1024;

            double progress = Math.Round(((double)(Downloaded) / (double)ALLdownload), 2) * 100;

            string strDownloadingInfo = string.Format("下载进度:{0}", progress);
            setPprogress((int)progress);
            setMsg(strDownloadingInfo, true);
        }



        void download_DownloadStartingEvent(FileDownload.FileInfo _file)
        {
            string strDownloadingInfo = string.Format("开始下载  {0}！", _file.FileName);
            setPprogress(0);
            setMsg(strDownloadingInfo, true);
        }
        
        #endregion
    }
}
