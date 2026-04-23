using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinForm_Test;
using System.Threading;
using System.IO;
using BLEDeviceAPI;
using UHFAPP.utils;
using System.Data;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using static UHFAPP.utils.EpcInfo;

namespace UHFAPP.excel
{
    class ExcelUtils
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="myDGV">控件DataGridView</param>
        private void ExportExcels(string fileName, DataGridView myDGV)
        {
            string saveFileName = "";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel文件|*.xls";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //被点了取消
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                return;
            }
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1
            //写入标题
            for (int i = 0; i < myDGV.ColumnCount; i++)
            {
                worksheet.Cells[1, i + 1] = myDGV.Columns[i].HeaderText;
            }
            //写入数值
            for (int r = 0; r < myDGV.Rows.Count; r++)
            {
                for (int i = 0; i < myDGV.ColumnCount; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = myDGV.Rows[r].Cells[i].Value;
                }
                System.Windows.Forms.Application.DoEvents();
            }
            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                }
            }
            xlApp.Quit();
            GC.Collect();//强行销毁
            MessageBox.Show("文件： " + fileName + ".xls 保存成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="myDGV">控件DataGridView</param>
        public static void ExportExcels(string fileName, ListView lvEPC)
        {
            if (lvEPC.Items.Count == 0)
            {
                MessageBox.Show("请先盘点标签!");
                return;
            }
            string msg = "正在导出数据到文件...";
            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                if (xlApp == null)
                {
                    frmWaitingBox.message = "无法创建Excel对象，可能您的电脑未安装Excel";
                    Thread.Sleep(1000);
                    return;
                }
                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1
                                                                                                                                      // ExcelApplication1.ActiveSheet.Rows[1].numberformat:='@';

                //写入标题
                worksheet.Cells[1, 1] = "EPC";
                worksheet.Cells[1, 2] = "TID";
                worksheet.Cells[1, 3] = "USER";
                worksheet.Cells[1, 4] = "天线";
                worksheet.Cells[1, 5] = "读取次数";
                worksheet.Cells[1, 6] = "RSSI";
                //worksheet.Cells[1, 6] = "时间";
                // xlApp.ScreenUpdating = false;


                //写入数值
                lvEPC.Invoke(new EventHandler(delegate
                {
                    int len = lvEPC.Items.Count;
                    Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range(worksheet.Cells[2, 6], worksheet.Cells[len + 1, 6]);
                    // Microsoft.Office.Interop.Excel.Range range1 = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[len + 1, 1]);
                    // Microsoft.Office.Interop.Excel.Range range2 = worksheet.get_Range(worksheet.Cells[2, 2], worksheet.Cells[len + 1, 2]);
                    range.NumberFormat = "@";//设置数字文本格式
                                             // range1.NumberFormat = "0";//设置数字文本格式

                    for (int r = 0; r < len; r++)
                    {
                        worksheet.Cells[r + 2, 1] = "\t" + lvEPC.Items[r].SubItems["EPC"].Text;//epc
                        worksheet.Cells[r + 2, 2] = "\t" + lvEPC.Items[r].SubItems["TID"].Text;//epc
                        worksheet.Cells[r + 2, 3] = "\t" + lvEPC.Items[r].SubItems["USER"].Text; ;// "1111111111111111111111";//epc
                        worksheet.Cells[r + 2, 4] = lvEPC.Items[r].SubItems["ANT"].Text;//ant
                        worksheet.Cells[r + 2, 5] = lvEPC.Items[r].SubItems["COUNT"].Text;//count
                        worksheet.Cells[r + 2, 6] = lvEPC.Items[r].SubItems["RSSI"].Text;//rssi

                        System.Windows.Forms.Application.DoEvents();
                        frmWaitingBox.message = "总标签数:" + len + ",已经保存:" + (r + 1);
                    }
                }));

                worksheet.Columns.EntireColumn.AutoFit();//列宽自适应



                if (fileName != "")
                {
                    try
                    {

                        workbook.Saved = true;
                        workbook.SaveCopyAs(fileName);
                        workbooks.Close();
                        MessageBox.Show("文件： " + fileName + "保存成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                    }
                }
                xlApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                GC.Collect();//强行销毁
            }, msg);
            f.ShowDialog();




        }



        //public static void ExportExcels(string FileName,List<EpcInfo> list)
        //{

        //    //string FileName = "d://abc.xls";
        //   // System.Data.DataTable dt = new System.Data.DataTable();
        //    FileStream objFileStream;
        //    StreamWriter objStreamWriter;

        //    objFileStream = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
        //    objStreamWriter = new StreamWriter(objFileStream, System.Text.Encoding.Unicode);

        //    string strLine = "EPC" + Convert.ToChar(9) +  "TID" + Convert.ToChar(9) +"USER" + Convert.ToChar(9)+"RSSI" + Convert.ToChar(9);
        //    objStreamWriter.WriteLine(strLine);
        //    strLine = "";

        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        strLine = list[i].Epc + Convert.ToChar(9)+ list[i].Count;
        //        objStreamWriter.WriteLine(strLine);
        //    }
        //    objStreamWriter.Close();
        //    objFileStream.Close();

        //}

        //Format : 0:hex  1:ascii
        public static bool ExportExcel(string FileName, List<EpcInfo> list,int Format,bool isShowPhase, bool isShowEPC, bool isShowTid, bool isShowUer)
        {

            int rowNumber = list.Count;
            int columnNumber = 3;// dataTable.Columns.Count;
            if (isShowPhase)
                columnNumber++;
            if (isShowEPC)
                columnNumber++;
            if (isShowTid)
                columnNumber++;
            if (isShowUer)
                columnNumber++;
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

            int index = -1;
            int index_EPC = -1;
            int index_Tid = -1;
            int index_Uer = -1;  
            int index_Phase = -1;
            int index_COUNT = -1;
            int index_ANT = -1;
            int index_RSSI = -1;
            //生成字段名称
            if (isShowEPC)
            {
                index_EPC = ++index;
                excel.Cells[1, index_EPC+1] = "EPC";
            }
            if (isShowTid)
            {
                index_Tid = ++index;
                excel.Cells[1, index_Tid + 1] = "TID";
            }  
            if (isShowUer)
            {
                index_Uer = ++index;
                excel.Cells[1, index_Uer + 1] = "USER";
            }
            index_COUNT = ++index;
            excel.Cells[1, index_COUNT + 1] = "COUNT";
            index_ANT = ++index;
            excel.Cells[1, index_ANT + 1] = "ANT";
            index_RSSI= ++index;
            excel.Cells[1, index_RSSI + 1] = "RSSI";
            if (isShowPhase)
            {
                index_Phase = ++index;
                excel.Cells[1, index_Phase + 1] = "PHASE";
            }
            


            object[,] objData = new object[rowNumber, columnNumber];

            for (int r = 0; r < rowNumber; r++)
            {
                if (Format == 1)
                {
                    if (index_EPC > -1 && list[r].Epc != null && list[r].Epc.Length > 0)
                    {
                        objData[r, index_EPC] = System.Text.Encoding.ASCII.GetString(DataConvert.HexStringToByteArray(list[r].Epc));
                    }

                    if (index_Tid > -1)
                        objData[r, index_Tid] = list[r].Tid;


                    if (index_Uer > -1 && list[r].User != null && list[r].User.Length > 0)
                    {
                        objData[r, index_Uer] = System.Text.Encoding.ASCII.GetString(DataConvert.HexStringToByteArray(list[r].User));
                    }
                }
                else if (Format == 2)
                {
                    if (index_EPC > -1 && list[r].Epc != null && list[r].Epc.Length > 0)
                    {
                        objData[r, index_EPC] = list[r].Epc + "\r\nASCII:" + System.Text.Encoding.ASCII.GetString(DataConvert.HexStringToByteArray(list[r].Epc));
                    }
                    if (index_Tid > -1)
                        objData[r, index_Tid] = list[r].Tid;

                    if (index_Uer > -1 && list[r].User != null && list[r].User.Length > 0)
                    {
                        objData[r, index_Uer] = list[r].User + "\r\nASCII:" + System.Text.Encoding.ASCII.GetString(DataConvert.HexStringToByteArray(list[r].User));
                    }
                }
                else
                {
                    if (index_EPC > -1)
                        objData[r, index_EPC] = list[r].Epc;
                    if (index_Tid > -1)
                        objData[r, index_Tid] = list[r].Tid;
                    if (index_Uer > -1)
                        objData[r, index_Uer] = list[r].User;
                }
  
                int count = 0;
                List<AntennaInfo>  antList=list[r].AntList;
                foreach (AntennaInfo antinfo in antList) {
                    if (index_RSSI > -1)
                    {
                        if (objData[r, index_RSSI] != null)
                        {
                            objData[r, index_RSSI] = objData[r, index_RSSI] + "\r\n" + antinfo.Rssi;
                        }
                        else
                        {
                            objData[r, index_RSSI] = antinfo.Rssi;
                        }
                    }
   
                    if (index_ANT > -1)
                    {
                        if (objData[r, index_ANT] != null)
                        {
                            objData[r, index_ANT] = objData[r, index_ANT] + "\r\n" + "ANT" + antinfo.AntennaPort + ":" + antinfo.Count;
                        }
                        else
                        {
                            objData[r, index_ANT] = "ANT" + antinfo.AntennaPort + ":" + antinfo.Count;
                        }
                    }
                    if (index_Phase > -1)
                    {
                        if (objData[r, index_Phase] != null)
                        {
                            objData[r, index_Phase] = objData[r, index_Phase] + "\r\n" +  antinfo.Phase ;
                        }
                        else
                        {
                            objData[r, index_Phase] = antinfo.Phase;
                        }
                    }
                    count += antinfo.Count;
                }
                if(index_COUNT>-1)
                   objData[r, index_COUNT] = count;
     

                //Application.DoEvents();
            }
             worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
            // 写入Excel
            range = worksheet.get_Range(excel.Cells[2, 1], excel.Cells[rowNumber + 1, columnNumber]);
            range.NumberFormat = "@";//设置单元格为文本格式
            range.Value2 = objData;
            // worksheet.get_Range(excel.Cells[2, 1], excel.Cells[rowNumber + 1, 1]).NumberFormat = "yyyy-m-d h:mm";


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
    }
}
