using BLEDeviceAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static UHFAPP.MainForm;

namespace UHFAPP.barcode
{
    public partial class BarcodeForm : Form
    {
        public BarcodeForm()
        {
            InitializeComponent();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            byte[] code = new byte[4096];
            int[] len = new int[1];
            int result = UHFAPI.ScannerRead((byte)1, (byte)0, code, len, 3000);
            if (result != 0)
            {
                return;
            }
            byte[] data= Utils.CopyArray(code, 0, len[0]); //Arrays.copyOf(code, len[0]);
            textBox1.Text = System.Text.Encoding.UTF8.GetString(data);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            txtKeyCode.Text = "";
        }

        private void BarcodeForm_Load(object sender, EventArgs e)
        {
            MainForm.keyDownEventHandler -= KeyDownEventHandler;
            MainForm.keyDownEventHandler += KeyDownEventHandler;

            MainForm.keyUpEventHandler -= KeyUpEventHandler;
            MainForm.keyUpEventHandler += KeyUpEventHandler;
        }

        private void BarcodeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.keyDownEventHandler -= KeyDownEventHandler;
            MainForm.keyUpEventHandler -= KeyUpEventHandler;
        }

        public void KeyDownEventHandler(int keyCode)
        {
            txtKeyCode.Invoke(new EventHandler(delegate {

                Console.WriteLine("KeyDownEventHandler");
                txtKeyCode.Text = txtKeyCode.Text + "\r\nKeyDown:" + keyCode.ToString();
                if (!cbKeyTest.Checked)
                {
                    btnScan_Click(null, null);
                }
               
            }));

        }

        public void KeyUpEventHandler(int keyCode)
        {
            txtKeyCode.Invoke(new EventHandler(delegate {
                txtKeyCode.Text = txtKeyCode.Text + "\r\nKeyUp:" + keyCode.ToString();
               Console.WriteLine("KeyUpEventHandler");
            }));
        }
    }
}
