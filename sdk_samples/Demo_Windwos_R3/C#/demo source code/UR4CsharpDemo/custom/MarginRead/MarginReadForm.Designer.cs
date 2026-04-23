namespace UHFAPP
{
    partial class MarginReadForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtASCII = new System.Windows.Forms.TextBox();
            this.lblLeng = new System.Windows.Forms.Label();
            this.btnWrite = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRead_Data = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRead_AccessPwd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRead_Length = new System.Windows.Forms.TextBox();
            this.txtRead_Ptr = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbRead_Bank = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txtLen = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.txtPtr = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbUser = new System.Windows.Forms.RadioButton();
            this.rbEPC = new System.Windows.Forms.RadioButton();
            this.rbTID = new System.Windows.Forms.RadioButton();
            this.label22 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.txtFilter_EPC = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btWrite = new System.Windows.Forms.Button();
            this.txtBlockWrite__AccessPwd = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbProtectedMode = new System.Windows.Forms.CheckBox();
            this.cbShortRangeMode = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Location = new System.Drawing.Point(2, -4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1127, 699);
            this.panel1.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtASCII);
            this.groupBox1.Controls.Add(this.lblLeng);
            this.groupBox1.Controls.Add(this.btnWrite);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtRead_Data);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtRead_AccessPwd);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtRead_Length);
            this.groupBox1.Controls.Add(this.txtRead_Ptr);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbRead_Bank);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(7, 105);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(508, 387);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MarginRead";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(15, 234);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(55, 16);
            this.label14.TabIndex = 39;
            this.label14.Text = "Ascii:";
            // 
            // txtASCII
            // 
            this.txtASCII.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtASCII.Location = new System.Drawing.Point(108, 212);
            this.txtASCII.Multiline = true;
            this.txtASCII.Name = "txtASCII";
            this.txtASCII.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtASCII.Size = new System.Drawing.Size(303, 73);
            this.txtASCII.TabIndex = 38;
            this.txtASCII.TextChanged += new System.EventHandler(this.txtASCII_TextChanged);
            // 
            // lblLeng
            // 
            this.lblLeng.AutoSize = true;
            this.lblLeng.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLeng.ForeColor = System.Drawing.Color.Black;
            this.lblLeng.Location = new System.Drawing.Point(417, 164);
            this.lblLeng.Name = "lblLeng";
            this.lblLeng.Size = new System.Drawing.Size(15, 16);
            this.lblLeng.TabIndex = 35;
            this.lblLeng.Text = "0";
            // 
            // btnWrite
            // 
            this.btnWrite.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWrite.ForeColor = System.Drawing.Color.Black;
            this.btnWrite.Location = new System.Drawing.Point(165, 311);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(90, 29);
            this.btnWrite.TabIndex = 33;
            this.btnWrite.Text = "MarginRead";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnMarginRead_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(417, 91);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 16);
            this.label13.TabIndex = 32;
            this.label13.Text = "(word)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(417, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 14);
            this.label3.TabIndex = 31;
            // 
            // txtRead_Data
            // 
            this.txtRead_Data.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRead_Data.Location = new System.Drawing.Point(108, 143);
            this.txtRead_Data.Multiline = true;
            this.txtRead_Data.Name = "txtRead_Data";
            this.txtRead_Data.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRead_Data.Size = new System.Drawing.Size(303, 66);
            this.txtRead_Data.TabIndex = 23;
            this.txtRead_Data.TextChanged += new System.EventHandler(this.txtRead_Data_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(15, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 16);
            this.label6.TabIndex = 22;
            this.label6.Text = "Hex:";
            // 
            // txtRead_AccessPwd
            // 
            this.txtRead_AccessPwd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRead_AccessPwd.Location = new System.Drawing.Point(108, 113);
            this.txtRead_AccessPwd.Name = "txtRead_AccessPwd";
            this.txtRead_AccessPwd.Size = new System.Drawing.Size(303, 26);
            this.txtRead_AccessPwd.TabIndex = 21;
            this.txtRead_AccessPwd.Text = "00000000";
            this.txtRead_AccessPwd.TextChanged += new System.EventHandler(this.txtRead_AccessPwd_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(14, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "Access Pwd:";
            // 
            // txtRead_Length
            // 
            this.txtRead_Length.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRead_Length.Location = new System.Drawing.Point(108, 83);
            this.txtRead_Length.Name = "txtRead_Length";
            this.txtRead_Length.Size = new System.Drawing.Size(303, 26);
            this.txtRead_Length.TabIndex = 19;
            this.txtRead_Length.Text = "6";
            this.txtRead_Length.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // txtRead_Ptr
            // 
            this.txtRead_Ptr.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRead_Ptr.Location = new System.Drawing.Point(108, 53);
            this.txtRead_Ptr.Name = "txtRead_Ptr";
            this.txtRead_Ptr.Size = new System.Drawing.Size(303, 26);
            this.txtRead_Ptr.TabIndex = 18;
            this.txtRead_Ptr.Text = "2";
            this.txtRead_Ptr.TextChanged += new System.EventHandler(this.TextChangedPtr);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(15, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "Length:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(14, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "Ptr:";
            // 
            // cmbRead_Bank
            // 
            this.cmbRead_Bank.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbRead_Bank.FormattingEnabled = true;
            this.cmbRead_Bank.Items.AddRange(new object[] {
            "RESERVED",
            "EPC",
            "TID",
            "USER"});
            this.cmbRead_Bank.Location = new System.Drawing.Point(108, 20);
            this.cmbRead_Bank.Name = "cmbRead_Bank";
            this.cmbRead_Bank.Size = new System.Drawing.Size(303, 24);
            this.cmbRead_Bank.TabIndex = 15;
            this.cmbRead_Bank.SelectedIndexChanged += new System.EventHandler(this.cmbRead_Bank_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(14, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 16);
            this.label1.TabIndex = 14;
            this.label1.Text = "Bank:";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.groupBox4.Controls.Add(this.label29);
            this.groupBox4.Controls.Add(this.txtLen);
            this.groupBox4.Controls.Add(this.label24);
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this.txtPtr);
            this.groupBox4.Controls.Add(this.groupBox3);
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.label30);
            this.groupBox4.Controls.Add(this.txtFilter_EPC);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.ForeColor = System.Drawing.Color.Black;
            this.groupBox4.Location = new System.Drawing.Point(7, 14);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1046, 72);
            this.groupBox4.TabIndex = 28;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "filter";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label29.ForeColor = System.Drawing.Color.Black;
            this.label29.Location = new System.Drawing.Point(482, 37);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(15, 16);
            this.label29.TabIndex = 36;
            this.label29.Text = "0";
            // 
            // txtLen
            // 
            this.txtLen.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLen.Location = new System.Drawing.Point(934, 31);
            this.txtLen.MaxLength = 3;
            this.txtLen.Name = "txtLen";
            this.txtLen.Size = new System.Drawing.Size(43, 26);
            this.txtLen.TabIndex = 37;
            this.txtLen.Tag = "Number";
            this.txtLen.Text = "0";
            this.txtLen.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label24.Location = new System.Drawing.Point(983, 37);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(47, 16);
            this.label24.TabIndex = 38;
            this.label24.Text = "(bit)";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label23.Location = new System.Drawing.Point(866, 36);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(63, 16);
            this.label23.TabIndex = 36;
            this.label23.Text = "Length:";
            // 
            // txtPtr
            // 
            this.txtPtr.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPtr.Location = new System.Drawing.Point(766, 31);
            this.txtPtr.MaxLength = 3;
            this.txtPtr.Name = "txtPtr";
            this.txtPtr.Size = new System.Drawing.Size(40, 26);
            this.txtPtr.TabIndex = 33;
            this.txtPtr.Tag = "Number";
            this.txtPtr.Text = "32";
            this.txtPtr.TextChanged += new System.EventHandler(this.txtPtr_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbUser);
            this.groupBox3.Controls.Add(this.rbEPC);
            this.groupBox3.Controls.Add(this.rbTID);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(542, 15);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(178, 47);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "bank";
            // 
            // rbUser
            // 
            this.rbUser.AutoSize = true;
            this.rbUser.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbUser.Location = new System.Drawing.Point(114, 20);
            this.rbUser.Name = "rbUser";
            this.rbUser.Size = new System.Drawing.Size(57, 20);
            this.rbUser.TabIndex = 12;
            this.rbUser.Text = "User";
            this.rbUser.UseVisualStyleBackColor = true;
            this.rbUser.Click += new System.EventHandler(this.rbUser_Click);
            // 
            // rbEPC
            // 
            this.rbEPC.AutoSize = true;
            this.rbEPC.Checked = true;
            this.rbEPC.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbEPC.Location = new System.Drawing.Point(11, 19);
            this.rbEPC.Name = "rbEPC";
            this.rbEPC.Size = new System.Drawing.Size(49, 20);
            this.rbEPC.TabIndex = 8;
            this.rbEPC.TabStop = true;
            this.rbEPC.Text = "EPC";
            this.rbEPC.UseVisualStyleBackColor = true;
            this.rbEPC.Click += new System.EventHandler(this.rbEPC_Click);
            // 
            // rbTID
            // 
            this.rbTID.AutoSize = true;
            this.rbTID.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbTID.Location = new System.Drawing.Point(67, 20);
            this.rbTID.Name = "rbTID";
            this.rbTID.Size = new System.Drawing.Size(49, 20);
            this.rbTID.TabIndex = 9;
            this.rbTID.Text = "TID";
            this.rbTID.UseVisualStyleBackColor = true;
            this.rbTID.Click += new System.EventHandler(this.rbTID_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label22.Location = new System.Drawing.Point(812, 37);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(47, 16);
            this.label22.TabIndex = 35;
            this.label22.Text = "(bit)";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label30.Location = new System.Drawing.Point(730, 35);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(39, 16);
            this.label30.TabIndex = 32;
            this.label30.Text = "Ptr:";
            // 
            // txtFilter_EPC
            // 
            this.txtFilter_EPC.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFilter_EPC.Location = new System.Drawing.Point(99, 15);
            this.txtFilter_EPC.Multiline = true;
            this.txtFilter_EPC.Name = "txtFilter_EPC";
            this.txtFilter_EPC.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFilter_EPC.Size = new System.Drawing.Size(372, 50);
            this.txtFilter_EPC.TabIndex = 12;
            this.txtFilter_EPC.TextChanged += new System.EventHandler(this.txtFilter_EPC_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(18, 31);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 16);
            this.label12.TabIndex = 11;
            this.label12.Text = "Data:";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.groupBox2.Controls.Add(this.cbShortRangeMode);
            this.groupBox2.Controls.Add(this.cbProtectedMode);
            this.groupBox2.Controls.Add(this.btWrite);
            this.groupBox2.Controls.Add(this.txtBlockWrite__AccessPwd);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(521, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(532, 387);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Protected And Short-Range";
            // 
            // btWrite
            // 
            this.btWrite.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btWrite.ForeColor = System.Drawing.Color.Black;
            this.btWrite.Location = new System.Drawing.Point(95, 309);
            this.btWrite.Name = "btWrite";
            this.btWrite.Size = new System.Drawing.Size(287, 31);
            this.btWrite.TabIndex = 34;
            this.btWrite.Text = "Set Protected And Short-Range";
            this.btWrite.UseVisualStyleBackColor = true;
            this.btWrite.Click += new System.EventHandler(this.btWrite_Click);
            // 
            // txtBlockWrite__AccessPwd
            // 
            this.txtBlockWrite__AccessPwd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBlockWrite__AccessPwd.Location = new System.Drawing.Point(126, 28);
            this.txtBlockWrite__AccessPwd.Name = "txtBlockWrite__AccessPwd";
            this.txtBlockWrite__AccessPwd.Size = new System.Drawing.Size(303, 26);
            this.txtBlockWrite__AccessPwd.TabIndex = 21;
            this.txtBlockWrite__AccessPwd.Text = "00000000";
            this.txtBlockWrite__AccessPwd.TextChanged += new System.EventHandler(this.txtBlockWrite__AccessPwd_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(34, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 16);
            this.label8.TabIndex = 20;
            this.label8.Text = "Access Pwd:";
            // 
            // cbProtectedMode
            // 
            this.cbProtectedMode.AutoSize = true;
            this.cbProtectedMode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbProtectedMode.Location = new System.Drawing.Point(46, 126);
            this.cbProtectedMode.Name = "cbProtectedMode";
            this.cbProtectedMode.Size = new System.Drawing.Size(124, 18);
            this.cbProtectedMode.TabIndex = 35;
            this.cbProtectedMode.Text = "Protected Mode";
            this.cbProtectedMode.UseVisualStyleBackColor = true;
            // 
            // cbShortRangeMode
            // 
            this.cbShortRangeMode.AutoSize = true;
            this.cbShortRangeMode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbShortRangeMode.Location = new System.Drawing.Point(233, 127);
            this.cbShortRangeMode.Name = "cbShortRangeMode";
            this.cbShortRangeMode.Size = new System.Drawing.Size(138, 18);
            this.cbShortRangeMode.TabIndex = 36;
            this.cbShortRangeMode.Text = "Short-Range Mode";
            this.cbShortRangeMode.UseVisualStyleBackColor = true;
            // 
            // MarginReadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(1124, 685);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "MarginReadForm";
            this.Text = "ReadWriteTagForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReadWriteTagForm_FormClosing);
            this.Load += new System.EventHandler(this.ReadWriteTagForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ReadWriteTagForm_KeyDown);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbRead_Bank;
        private System.Windows.Forms.TextBox txtRead_Data;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRead_AccessPwd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRead_Length;
        private System.Windows.Forms.TextBox txtRead_Ptr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtBlockWrite__AccessPwd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtFilter_EPC;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbUser;
        private System.Windows.Forms.RadioButton rbEPC;
        private System.Windows.Forms.RadioButton rbTID;
        private System.Windows.Forms.TextBox txtPtr;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtLen;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Label lblLeng;
        private System.Windows.Forms.Button btWrite;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtASCII;
        private System.Windows.Forms.CheckBox cbShortRangeMode;
        private System.Windows.Forms.CheckBox cbProtectedMode;
    }
}