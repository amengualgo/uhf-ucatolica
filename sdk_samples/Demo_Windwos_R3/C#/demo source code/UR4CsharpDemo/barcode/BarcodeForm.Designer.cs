namespace UHFAPP.barcode
{
    partial class BarcodeForm
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
            this.btnScan = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtKeyCode = new System.Windows.Forms.TextBox();
            this.cbKeyTest = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(163, 37);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(452, 257);
            this.textBox1.TabIndex = 0;
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(237, 317);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(87, 40);
            this.btnScan.TabIndex = 1;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(381, 317);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(93, 40);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtKeyCode
            // 
            this.txtKeyCode.Location = new System.Drawing.Point(639, 37);
            this.txtKeyCode.Multiline = true;
            this.txtKeyCode.Name = "txtKeyCode";
            this.txtKeyCode.Size = new System.Drawing.Size(226, 257);
            this.txtKeyCode.TabIndex = 3;
            // 
            // cbKeyTest
            // 
            this.cbKeyTest.AutoSize = true;
            this.cbKeyTest.Location = new System.Drawing.Point(639, 300);
            this.cbKeyTest.Name = "cbKeyTest";
            this.cbKeyTest.Size = new System.Drawing.Size(72, 16);
            this.cbKeyTest.TabIndex = 4;
            this.cbKeyTest.Text = "Key Test";
            this.cbKeyTest.UseVisualStyleBackColor = true;
            // 
            // BarcodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 450);
            this.Controls.Add(this.cbKeyTest);
            this.Controls.Add(this.txtKeyCode);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.textBox1);
            this.Name = "BarcodeForm";
            this.Text = "BarcodeForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BarcodeForm_FormClosing);
            this.Load += new System.EventHandler(this.BarcodeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtKeyCode;
        private System.Windows.Forms.CheckBox cbKeyTest;
    }
}