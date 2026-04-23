namespace UHFAPP
{
    partial class UHFUpgradeForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPath = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbReaderApplication = new System.Windows.Forms.RadioButton();
            this.rbUHFModule = new System.Windows.Forms.RadioButton();
            this.gbRemote = new System.Windows.Forms.GroupBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.txtHttpURL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.rbEx10SDKFirmware = new System.Windows.Forms.RadioButton();
            this.rbReaderBootloader = new System.Windows.Forms.RadioButton();
            this.gbRemote.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(34, 120);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(540, 206);
            this.textBox1.TabIndex = 12;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(34, 96);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(540, 18);
            this.progressBar1.TabIndex = 13;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(183, 356);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(186, 46);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnPath
            // 
            this.btnPath.Location = new System.Drawing.Point(450, 25);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(83, 35);
            this.btnPath.TabIndex = 10;
            this.btnPath.Text = "Select file";
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(34, 22);
            this.txtPath.Multiline = true;
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(410, 37);
            this.txtPath.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(106, 333);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(354, 20);
            this.label2.TabIndex = 15;
            this.label2.Text = "version:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // rbReaderApplication
            // 
            this.rbReaderApplication.AutoSize = true;
            this.rbReaderApplication.Location = new System.Drawing.Point(35, 74);
            this.rbReaderApplication.Name = "rbReaderApplication";
            this.rbReaderApplication.Size = new System.Drawing.Size(77, 16);
            this.rbReaderApplication.TabIndex = 21;
            this.rbReaderApplication.Text = "Mainboard";
            this.rbReaderApplication.UseVisualStyleBackColor = true;
            // 
            // rbUHFModule
            // 
            this.rbUHFModule.AutoSize = true;
            this.rbUHFModule.Checked = true;
            this.rbUHFModule.Location = new System.Drawing.Point(170, 74);
            this.rbUHFModule.Name = "rbUHFModule";
            this.rbUHFModule.Size = new System.Drawing.Size(83, 16);
            this.rbUHFModule.TabIndex = 20;
            this.rbUHFModule.TabStop = true;
            this.rbUHFModule.Text = "UHF module";
            this.rbUHFModule.UseVisualStyleBackColor = true;
            // 
            // gbRemote
            // 
            this.gbRemote.Controls.Add(this.btnDownload);
            this.gbRemote.Controls.Add(this.txtHttpURL);
            this.gbRemote.Controls.Add(this.label1);
            this.gbRemote.Location = new System.Drawing.Point(34, 565);
            this.gbRemote.Name = "gbRemote";
            this.gbRemote.Size = new System.Drawing.Size(499, 78);
            this.gbRemote.TabIndex = 22;
            this.gbRemote.TabStop = false;
            this.gbRemote.Text = "远程下载路径";
            this.gbRemote.Visible = false;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(432, 22);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(63, 32);
            this.btnDownload.TabIndex = 24;
            this.btnDownload.Text = "下载";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // txtHttpURL
            // 
            this.txtHttpURL.Location = new System.Drawing.Point(74, 17);
            this.txtHttpURL.Multiline = true;
            this.txtHttpURL.Name = "txtHttpURL";
            this.txtHttpURL.Size = new System.Drawing.Size(352, 45);
            this.txtHttpURL.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Http地址:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(34, 543);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 23;
            this.checkBox1.Text = "远程下载";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // rbEx10SDKFirmware
            // 
            this.rbEx10SDKFirmware.AutoSize = true;
            this.rbEx10SDKFirmware.Location = new System.Drawing.Point(277, 74);
            this.rbEx10SDKFirmware.Name = "rbEx10SDKFirmware";
            this.rbEx10SDKFirmware.Size = new System.Drawing.Size(125, 16);
            this.rbEx10SDKFirmware.TabIndex = 25;
            this.rbEx10SDKFirmware.Text = "Ex10 SDK firmware";
            this.rbEx10SDKFirmware.UseVisualStyleBackColor = true;
            this.rbEx10SDKFirmware.Visible = false;
            // 
            // rbReaderBootloader
            // 
            this.rbReaderBootloader.AutoSize = true;
            this.rbReaderBootloader.Location = new System.Drawing.Point(418, 74);
            this.rbReaderBootloader.Name = "rbReaderBootloader";
            this.rbReaderBootloader.Size = new System.Drawing.Size(131, 16);
            this.rbReaderBootloader.TabIndex = 24;
            this.rbReaderBootloader.Text = "Reader bootloader ";
            this.rbReaderBootloader.UseVisualStyleBackColor = true;
            this.rbReaderBootloader.Visible = false;
            // 
            // UHFUpgradeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(603, 536);
            this.Controls.Add(this.rbEx10SDKFirmware);
            this.Controls.Add(this.rbReaderBootloader);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.gbRemote);
            this.Controls.Add(this.rbReaderApplication);
            this.Controls.Add(this.rbUHFModule);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnPath);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.Name = "UHFUpgradeForm";
            this.Text = "UHFUpgradeForm";
            this.Load += new System.EventHandler(this.UHFUpgradeForm_Load);
            this.gbRemote.ResumeLayout(false);
            this.gbRemote.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbReaderApplication;
        private System.Windows.Forms.RadioButton rbUHFModule;
        private System.Windows.Forms.GroupBox gbRemote;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox txtHttpURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.RadioButton rbEx10SDKFirmware;
        private System.Windows.Forms.RadioButton rbReaderBootloader;
    }
}