namespace UHF_BLE
{
    partial class ExportDataForm
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
            this.btnGetTag = new System.Windows.Forms.Button();
            this.listView2 = new System.Windows.Forms.ListView();
            this.Id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EPC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Count = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnClean = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnGetNumber = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetTag
            // 
            this.btnGetTag.ForeColor = System.Drawing.Color.Black;
            this.btnGetTag.Location = new System.Drawing.Point(152, 545);
            this.btnGetTag.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetTag.Name = "btnGetTag";
            this.btnGetTag.Size = new System.Drawing.Size(112, 40);
            this.btnGetTag.TabIndex = 15;
            this.btnGetTag.Text = "获取标签";
            this.btnGetTag.UseVisualStyleBackColor = true;
            this.btnGetTag.Click += new System.EventHandler(this.btnGetTag_Click);
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Id,
            this.EPC,
            this.Count});
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(13, 20);
            this.listView2.Margin = new System.Windows.Forms.Padding(2);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(652, 476);
            this.listView2.TabIndex = 14;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // Id
            // 
            this.Id.Text = "Id";
            this.Id.Width = 103;
            // 
            // EPC
            // 
            this.EPC.Text = "epc";
            this.EPC.Width = 636;
            // 
            // Count
            // 
            this.Count.Text = "Count";
            this.Count.Width = 119;
            // 
            // btnClean
            // 
            this.btnClean.ForeColor = System.Drawing.Color.Black;
            this.btnClean.Location = new System.Drawing.Point(296, 545);
            this.btnClean.Margin = new System.Windows.Forms.Padding(2);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(112, 40);
            this.btnClean.TabIndex = 17;
            this.btnClean.Text = "清空列表";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(431, 545);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(112, 40);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "删除所有标签";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnGetNumber
            // 
            this.btnGetNumber.ForeColor = System.Drawing.Color.Black;
            this.btnGetNumber.Location = new System.Drawing.Point(13, 545);
            this.btnGetNumber.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetNumber.Name = "btnGetNumber";
            this.btnGetNumber.Size = new System.Drawing.Size(112, 40);
            this.btnGetNumber.TabIndex = 16;
            this.btnGetNumber.Text = "获取标签数量";
            this.btnGetNumber.UseVisualStyleBackColor = true;
            this.btnGetNumber.Click += new System.EventHandler(this.btnGetNumber_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.btnGetNumber);
            this.groupBox1.Controls.Add(this.btnGetTag);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnClean);
            this.groupBox1.Controls.Add(this.listView2);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(9, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(873, 592);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(84, 511);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 42;
            this.label2.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(21, 511);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 41;
            this.label1.Text = "标签数：";
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("宋体", 9F);
            this.btnExport.ForeColor = System.Drawing.Color.Black;
            this.btnExport.Location = new System.Drawing.Point(565, 545);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(109, 40);
            this.btnExport.TabIndex = 40;
            this.btnExport.Text = "导出数据";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ExportDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(913, 606);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ExportDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BLETestForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportDataForm_FormClosing);
            this.Load += new System.EventHandler(this.ExportDataForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetTag;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader Id;
        private System.Windows.Forms.ColumnHeader EPC;
        private System.Windows.Forms.ColumnHeader Count;
        private System.Windows.Forms.Button btnClean;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnGetNumber;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}