using BLEDeviceAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
 
using System.Windows.Forms;
using UHFAPP;
using UHFAPP.utils;
using WinForm_Test;
using static UHFAPP.UHFAPI;
using static UHFAPP.utils.EpcInfo;

namespace UHF_BLE
{
    public partial class ExportDataForm :  BaseForm
    {
        List<string> listEPC = new List<string>();
        // 将text更新的界面控件的委托类型
        delegate void UpdateUI(List<string> infos);
        UpdateUI updateUI;
        public ExportDataForm()
        {
            InitializeComponent();
            listView2.Columns[0].Width = 60;//
            listView2.Columns[1].Width = 530;// 
            listView2.Columns[2].Width = 60;// 
            updateUI = new UpdateUI(UpdatalistView);
            UpdateUILanguage();
           
        }


        private void UpdatalistView(List<string> infos){
            if (infos != null)
            {
                for (int k = 0; k < infos.Count; k++)
                {
                    string epc = infos[k];
                    int index = 0;
                    bool[] exists=new bool[1];
                    index = listEPC.Count; //CheckUtils.getInsertIndex(listEPC,epc, exists);
                    if (!exists[0])
                    {
                        ListViewItem viewItem = new ListViewItem();
                        viewItem.Text = (listView2.Items.Count + 1).ToString();
                        viewItem.SubItems.Add(epc);
                        viewItem.SubItems.Add("1");
                        listView2.Items.Insert(index,viewItem);
                        listEPC.Insert(index,epc);
                    }
                    else
                    {
                        listView2.Items[index].SubItems[2].Text = (int.Parse(listView2.Items[index].SubItems[2].Text) + 1).ToString();
                    }
                  
                }
                label2.Text = listView2.Items.Count.ToString();
            }
        }

        private void btnGetNumber_Click(object sender, EventArgs e)
        {
            int number = UHFAPI.GetFlashTagsNumber();

            if (number>=0)
            {
                if (!MainForm.IsChineseSimple())
                {
                    MessageBox.Show("count:" + number.ToString());
                }
                else
                {
                    MessageBox.Show("标签数量:" + number.ToString());
                }
            }
            else
            {
                string msg = MainForm.IsChineseSimple()  ? "失败" : "fail" ;
                MessageBox.Show(msg);
            }

        }

        private void btnGetTag_Click(object sender, EventArgs e)
        {
            int number = UHFAPI.GetFlashTagsNumber();
            if (number<=0)
            {
                string msg = MainForm.IsChineseSimple() ? "失败" : "fail";
                MessageBox.Show(msg);
                return;
            }
            btnClean_Click(null,null);

                 frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                int startTime = Environment.TickCount;
                float temp = 0;

                while (true)
                {
                    List<string> tags = UHFAPI.UsbExportTags();
                    if (tags == null || tags.Count==0)
                    {
                        continue;
                    }
                    temp += tags.Count;
                    frmWaitingBox.message = "              " + ((Math.Round(temp / number, 2)) * 100).ToString() + "%";

                    if (updateUI != null)
                    {
                        this.BeginInvoke(updateUI, new object[] { tags });
                    }
                    if (temp >= number)
                    {
                       break;
                    }
                }
         
            }, "wait...");
            f.ShowDialog(this);
             
        }
      
   

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialog= MessageBox.Show("你确定要删除设备内部存储的标签吗？", "Error!", MessageBoxButtons.YesNoCancel);
            if (dialog == DialogResult.Yes)
            {
                bool result = UHFAPI.LocalDeleteTags() == 0;
                string msg = !MainForm.IsChineseSimple() ? (result ? "success" : "fail") : (result ? "成功" : "失败");
                MessageBox.Show(msg);
            }
            

        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            listEPC.Clear();
            label2.Text = "0";
        }
 
        public   void UpdateUILanguage()
        {
            if (!MainForm.IsChineseSimple())
            {
                btnGetNumber.Text = "Get count";
                btnGetTag.Text = "Get EPC";
                btnClean.Text = "Clear list";
                btnDelete.Text = "Delete tags";
                groupBox1.Text = "Export Data";
            }
            else
            {
                btnGetNumber.Text = "获取标签数量";
                btnGetTag.Text = "获取标签";
                btnClean.Text = "清空列表";
                btnDelete.Text = "删除所有标签";
                groupBox1.Text = "导出数据";//""
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

            ExportExcel(path, listEPC);
        }







         //Format : 0:hex  1:ascii
        public static bool ExportExcel(string FileName, List<string> list)
        {

            int rowNumber = list.Count;
            int columnNumber = 2;// dataTable.Columns.Count;
 
            if (rowNumber == 0)
            {
                return false;
            }

            //建立Excel对象
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            //excel.Application.Workbooks.Add(true);
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
             Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            //excel.Visible = false;
            //Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)excel.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range range;
 
            int index_EPC = 0;
            int index_COUNT = 1;
            excel.Cells[1, index_EPC + 1] = "EPC";
            excel.Cells[1, index_COUNT + 1] = "COUNT";
     
            object[,] objData = new object[rowNumber, columnNumber];
            for (int r = 0; r < rowNumber; r++)
            {
                objData[r, index_EPC] = list[r].Replace(" ", "");
                objData[r, index_COUNT] = "1";
            }
             worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
            // 写入Excel
            range = worksheet.get_Range(excel.Cells[2, 1], excel.Cells[rowNumber + 1, columnNumber]);
            range.NumberFormat = "@";//设置单元格为文本格式
            range.Value2 = objData;
            try
            {
                worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
                workbook.Saved = true;
                workbook.SaveCopyAs(FileName);
                //  workbooks.Close();
                if (BaseForm.IsChineseSimple())
                {
                    MessageBox.Show("文件： " + FileName + "保存成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("File： " + FileName + " save success!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
       
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
            }

            return true;
        }

        private void ExportDataForm_Load(object sender, EventArgs e)
        {
            MainForm.eventMainSizeChanged += MainForm_SizeChanged;
        }

        private void ExportDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.eventMainSizeChanged -= MainForm_SizeChanged;
        }

        private void MainForm_SizeChanged(FormWindowState state)
        {
            //判断是否选择的是最小化按钮
            groupBox1.Left = 308;
        }

    }
}
