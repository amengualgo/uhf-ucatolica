using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinForm_Test;
using BLEDeviceAPI;
using System.Threading;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
 

namespace UHFAPP
{
    public partial class MarginReadForm : BaseForm
    {
        public string tag = "";
        private bool isLoadData = false;
        public MarginReadForm()
        {
            InitializeComponent();
        }
        public MarginReadForm(bool isOpen,string tag, int bank)
        {
            InitializeComponent();
            SetTAG(isOpen, tag, bank);              
        }
        public void SetTAG(bool isOpen, string tag, int bank)
        {
            if (isOpen)
            {
                panel1.Enabled = true;
            }
            else
            {
                panel1.Enabled = false;
            }
            if (tag != "")
            {
                this.tag = tag;
                txtFilter_EPC.Text = tag;
                txtLen.Text = (tag.Length / 2 * 8 ).ToString();
                if (bank == 1)
                {
                    rbEPC.Checked = true;
                }
                else if (bank == 2)
                {
                    rbTID.Checked = true;
                }
                else if (bank == 3) {
                    rbUser.Checked = true;
                } 

            }
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
            }
        }
 
        private void ReadWriteTagForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Common.SaveControlValues(this);
            MainForm.eventOpen -= MainForm_eventOpen;
            MainForm.eventMainSizeChanged -= MainForm_SizeChanged;
        }

        private void ReadWriteTagForm_Load(object sender, EventArgs e)
        {
            MainForm.eventMainSizeChanged += MainForm_SizeChanged;

            txtFilter_EPC.Text = Common.tag;
            if (Common.bank == 1)
            {
                rbEPC.Checked = true;
            }
            else if (Common.bank == 2)
            {
                rbTID.Checked = true;
            }
            else if (Common.bank == 3)
            {
                rbUser.Checked = true;
            }

            MainForm.eventOpen += MainForm_eventOpen;
            cmbRead_Bank.SelectedIndex = 1;
          

            txtPtr.LostFocus += new EventHandler(txt_LostFocus); //失去焦点后发生事件
            txtLen.LostFocus += new EventHandler(txt_LostFocus); //失去焦点后发生事件

            if (txtFilter_EPC.Text.Replace(" ", "").Length > 0)
            {
                txtLen.Text = (txtFilter_EPC.Text.Replace(" ", "").Length / 2 * 8) + "";
            }

            Hashtable hashtable = Common.GetControlValues(this.Name);
            if (hashtable != null)
            {
                isLoadData = true;
                new Thread(new ThreadStart(delegate {
                    Thread.Sleep(100);
                    isLoadData = false;
                })).Start();
                this.txtFilter_EPC.Text = (string)hashtable[txtFilter_EPC.Name];
                this.txtPtr.Text = (string)hashtable[txtPtr.Name];
                this.txtLen.Text = (string)hashtable[txtLen.Name];
                this.rbEPC.Checked = (bool)hashtable[rbEPC.Name];
                this.rbTID.Checked = (bool)hashtable[rbTID.Name];
                this.rbUser.Checked = (bool)hashtable[rbUser.Name];
                this.cmbRead_Bank.SelectedIndex = (int)hashtable[cmbRead_Bank.Name];
                this.txtRead_Ptr.Text = (string)hashtable[txtRead_Ptr.Name];
                this.txtRead_Length.Text = (string)hashtable[txtRead_Length.Name];
                this.txtRead_AccessPwd.Text = (string)hashtable[txtRead_AccessPwd.Name];
                string data = (string)hashtable[txtRead_Data.Name];
                this.txtRead_Data.Text = data;
                this.txtBlockWrite__AccessPwd.Text = (string)hashtable[txtBlockWrite__AccessPwd.Name];
      
            }
            LoadUI();
        }


 

   
        #region 读写数据
   
     
   
        private void txtRead_AccessPwd_TextChanged(object sender, EventArgs e)
        {
            FormatHex_PWD(txtRead_AccessPwd);
        }
        private void txtRead_Data_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FormatHex(txtRead_Data);
                string data = txtRead_Data.Text.Replace(" ", "");

                if (data.Length > 0)
                {
                    lblLeng.Text = ((data.Length / 2) + ((data.Length % 2) == 0 ? 0 : 1)).ToString();  // txtRead_Length.Text = ((data.Length / 4) + ((data.Length % 4) == 0 ? 0 : 1)).ToString();
                }
                else
                {
                    lblLeng.Text = "0";
                }
                txtASCII.Text = System.Text.ASCIIEncoding.ASCII.GetString(DataConvert.HexStringToByteArray(data));
            }
            catch (Exception ex) { }
        }
 
        private void cmbRead_Bank_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbRead_Bank.SelectedIndex;
            if (index == 1)
            {
                if (int.Parse(txtRead_Ptr.Text) < 2)
                {
                    txtRead_Ptr.Text = "2";
                }
          
            }
            else
            {
             
                txtRead_Ptr.Text = "0";
            }

            if (index == 1 || index == 2)
            {
                txtRead_Length.Text = "6";
            }
            else
            {
                txtRead_Length.Text = "4";
            }
 
 
        }
        private void TextChangedPtr(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (!StringUtils.IsNumber(txt.Text))
            {
                txt.Text = "0";
            }
            int index = cmbRead_Bank.SelectedIndex;
            if (index == 1)
            {
                if (int.Parse(txt.Text) < 2)
                {
                  //  txt.Text = "2";
                }
            }
        }
        //监听 文本
        private void TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (!StringUtils.IsNumber(txt.Text))
            {
                txt.Text = "0";
            }
            if (int.Parse(txt.Text) < 1)
            {
                txt.Text = "1";
            }
        }
        #endregion


        #region BlockWrite

 

        private void btWrite_Click(object sender, EventArgs e)
        {
            if (!DetectionFiltration())
                return;


            string pws = txtBlockWrite__AccessPwd.Text.Replace(" ", "");

            if (pws.Length != 8)
            {
                MessageBox.Show(!IsChineseSimple() ? "The length of the password must be 8!" : "密码长度必须是8位!");
                return;
            }
 
            byte[] uAccessPwd = DataConvert.HexStringToByteArray(pws);

            //------------过滤
            string filerData = txtFilter_EPC.Text.Replace(" ", "");
            int filerBank = 1;
            byte[] filerBuff = DataConvert.HexStringToByteArray(filerData);
            int filerPtr = int.Parse(txtPtr.Text);
            int filerLen = int.Parse(txtLen.Text);

            if ((filerLen / 8 + (filerLen % 8 == 0 ? 0 : 1)) * 2 > filerData.Length)
            {
                MessageBox.Show(!IsChineseSimple() ? "filter data length error!" : "过滤数据和长度不匹配!"); //to do
                return;
            }
            if (rbTID.Checked)
                filerBank = 2;
            if (rbUser.Checked)
                filerBank = 3;
            //-------------------
            string msg = "";

            int protectedMode = cbProtectedMode.Checked ? 1 : 0;
            int shortRangeMode = cbShortRangeMode.Checked ? 1 : 0;
            int time = 500;
            int result = UHFAPI.UHFSetProtectionAndShortRange(uAccessPwd, (byte)filerBank, filerPtr, filerLen, filerBuff,(byte)protectedMode,(byte)shortRangeMode);
            if (result==0)
            {
                time = 100;
                msg = !IsChineseSimple() ? "Write success!" : "写入成功!";
            }
            else
            {
                msg = !IsChineseSimple() ? "Write failure!" : "写入失败!";
            }

            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                System.Threading.Thread.Sleep(time);
            }, msg);
            f.ShowDialog(this);
        }
     
    

  

        private void txtBlockWrite__AccessPwd_TextChanged(object sender, EventArgs e)
        {
            FormatHex_PWD(txtBlockWrite__AccessPwd);
        }

        private void txtBlockWrite__Length_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (!StringUtils.IsNumber(txt.Text))
            {
                txt.Text = "0";
            }
            if (int.Parse(txt.Text) < 1)
            {
                txt.Text = "1";
            }
        }
    
        
        #endregion

  



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
                    txt.SelectionStart = SelectIndex+2; 
                //txt.Select(txt.Text.Length - 1, 0);
                
            }
        }
        private void FormatHex_PWD(TextBox txt)
        {
            if (isDelete) return;
            string data = txt.Text.Trim().Replace(" ", "");
            if (data != string.Empty)
            {
                if (data.Length > 8) {
                    data = data.Substring(0,8);
                }
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
        #region 过滤

        private void txtFilter_EPC_TextChanged(object sender, EventArgs e)
        {
            FormatHex(txtFilter_EPC);
            string data = txtFilter_EPC.Text.Replace(" ", "");
            if (data.Length > 0)
            {
                label29.Text = ((data.Length / 2) + ((data.Length % 2) == 0 ? 0 : 1)).ToString();  // txtRead_Length.Text = ((data.Length / 4) + ((data.Length % 4) == 0 ? 0 : 1)).ToString();
            }
            else
            {
                label29.Text = "0";
            }
        }

        public bool DetectionFiltration()
        {
            if (int.Parse(txtLen.Text) > 0)
            {
                string filerData = txtFilter_EPC.Text.Replace(" ", "");
                if (!StringUtils.IsHexNumber(filerData))
                {
                    MessageBox.Show(!IsChineseSimple() ? "Please input the hex filter data!" : "请输入十六进制过滤数据!");
                    return false;
                }
            }
            return true;
        }
        private void rbEPC_Click(object sender, EventArgs e)
        {
            txtPtr.Text = "32";
        }

        private void rbTID_Click(object sender, EventArgs e)
        {
            txtPtr.Text = "0";
        }

        private void rbUser_Click(object sender, EventArgs e)
        {
            txtPtr.Text = "0";
        }
        #endregion

        private void txtPtr_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPtr.Text == "")
                    return;
                string ptr = txtPtr.Text;
                if (!StringUtils.IsNumber(ptr))
                {
                    txtPtr.Text = "0";
                    return;
                }
 
            }
            catch (Exception ex)
            {
                txtPtr.Text = "0";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtLen.Text == "")
                    return;

                string ptr = txtLen.Text;
                if (!StringUtils.IsNumber(ptr))
                {
                    txtLen.Text = "0";
                    return;
                }

            }
            catch (Exception ex)
            {
                txtLen.Text = "0";
            }
        }

      


        void txt_LostFocus(object sender, EventArgs e)
        {
            TextBox text = (TextBox)sender;
            
            if (text.Text == "")
            {
                text.Text = "0";
            }

          
     
        }

        bool isDelete = false;
        private void ReadWriteTagForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                isDelete = true;
            }
            else {
                isDelete = false;
            }

        }

 

    
        private void button1_Click(object sender, EventArgs e)
        {
           
            if (!DetectionFiltration())
                return;

            string filerData = txtFilter_EPC.Text.Replace(" ", "");
            string accessPwd = txtRead_AccessPwd.Text.Replace(" ", "");


            if (!StringUtils.IsHexNumber(accessPwd) || accessPwd.Length != 8)
            {
                MessageBox.Show(!IsChineseSimple() ? "The length of the password must be 8!" : "密码长度必须是8位!");
                return;
            }

            //过滤----------------------------------
            int filerBank = 1;
            byte[] filerBuff = DataConvert.HexStringToByteArray(filerData);
            int filerPtr = int.Parse(txtPtr.Text);
            int filerLen = int.Parse(txtLen.Text);

            if ((filerLen / 8 + (filerLen % 8 == 0 ? 0 : 1)) * 2 > filerData.Length)
            {
                MessageBox.Show(!IsChineseSimple() ? "filter data length error!" : "过滤数据和长度不匹配!");  //to do
                return;
            }

            if (rbTID.Checked)
                filerBank = 2;
            if (rbUser.Checked)
                filerBank = 3;
            //-----------------------------------------
 
            bool result = uhf.InitRegFile(  (byte)filerBank, filerPtr, filerLen, filerBuff);
            string msg="";
            int time = 500;
            if (result)
            {
                time = 100;
                msg = !IsChineseSimple() ? " success!" : "初始化成功!";
            }
            else
            {
                msg = !IsChineseSimple() ? " failure!" : "初始化失败!";
            }

            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                System.Threading.Thread.Sleep(time);
            }, msg);
            f.ShowDialog(this);
        }
        private void MainForm_SizeChanged(FormWindowState state)
        {
            //判断是否选择的是最小化按钮
            panel1.Left = 308;
        }
        private void LoadUI()
        {
            if (IsChineseSimple())
            {
                groupBox4.Text = "过滤";
                //groupBox1.Text = "读-写";
                label1.Text = "存储区:";
                label2.Text = "起始地址:";
                label4.Text = "长度:";
                label5.Text = "访问密码:";
                label13.Text = "(字)";
                label8.Text = "访问密码:";          
               // btnWrite.Text = "写";
               // btWrite.Text = "写";
               // groupBox2.Text = "数据块写-擦除";
            }
        }
        private void txtASCII_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!txtRead_Data.Focused && !isLoadData)
                    txtRead_Data.Text = DataConvert.ByteArrayToHexString(System.Text.ASCIIEncoding.ASCII.GetBytes(txtASCII.Text));
            }
            catch (Exception ex)
            {

            }
        }
        private void btnMarginRead_Click(object sender, EventArgs e)
        {
            string wdata = "";
            wdata = txtRead_Data.Text.Replace(" ", "");
            if (!DetectionFiltration())
                return;

            string filerData = txtFilter_EPC.Text.Replace(" ", "");
            string accessPwd = txtRead_AccessPwd.Text.Replace(" ", "");


            if (!StringUtils.IsHexNumber(accessPwd) || accessPwd.Length != 8)
            {
                MessageBox.Show(!IsChineseSimple() ? "The length of the password must be 8!" : "密码长度必须是8位!");
                return;
            }

            //过滤--------
            int filerBank = 1;
            byte[] filerBuff = DataConvert.HexStringToByteArray(filerData);
            int filerPtr = int.Parse(txtPtr.Text);
            int filerLen = int.Parse(txtLen.Text);

            if ((filerLen / 8 + (filerLen % 8 == 0 ? 0 : 1)) * 2 > filerData.Length)
            {
                MessageBox.Show(!IsChineseSimple() ? "filter data length error!" : "过滤数据和长度不匹配!"); //to do
                return;
            }

            if (rbTID.Checked)
                filerBank = 2;
            if (rbUser.Checked)
                filerBank = 3;
            //----------
            byte[] pwd = DataConvert.HexStringToByteArray(accessPwd);
            int bank = cmbRead_Bank.SelectedIndex;
            int Ptr = int.Parse(txtRead_Ptr.Text);
            int leng = int.Parse(txtRead_Length.Text);
            string msg = "";
            string Databuf = wdata;
            if (!StringUtils.IsHexNumber(Databuf))
            {
                MessageBox.Show(!IsChineseSimple() ? "Please input hexadecimal data!" : "请输入十六进制数据!");
                return;
            }
            if (Databuf.Length % 4 != 0)
            {
                MessageBox.Show(!IsChineseSimple() ? "Write data of the length of the string must be in multiples of four!" : "写入的十六进制字符串长度必须是4的倍数!");
                return;
            }
            if (leng > (Databuf.Length / 4))
            {
                MessageBox.Show(!IsChineseSimple() ? "Write data length error! " : "写入的数据和长度不匹配!");
                return;
            }
            int time = 500;
            byte[] uDatabuf = DataConvert.HexStringToByteArray(Databuf);
 
            //TODO result = uhf.WriteData(pwd, (byte)filerBank, filerPtr, filerLen, filerBuff, (byte)bank, Ptr, (byte)leng, uDatabuf);
            int result= UHFAPI.UHFMarginRead(pwd, (byte)filerBank, filerPtr, filerLen, filerBuff, (byte)bank, Ptr, leng, uDatabuf);
            //bool result = uhf.writeDataToEpc(pwd, (byte)filerBank, filerPtr, filerLen, filerBuff, uDatabuf);  
            if (result==0)
            {
                time = 100;
                msg = !IsChineseSimple() ? "success!" : "成功!";
            }
            else
            {
                msg = !IsChineseSimple() ? "fail!" : "失败!";
            }

            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                System.Threading.Thread.Sleep(time);
            }, msg);
            f.ShowDialog(this);
        }
    }
}
