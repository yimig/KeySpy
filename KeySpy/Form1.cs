using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeySpy
{
    public partial class Form1 : Form
    {
        //private KeyEventHandler myKeyEventHandeler = null; //按键钩子
        //private KeyboardHook k_hook = new KeyboardHook();
        private KeyDeviceHook hook = new KeyDeviceHook();
        private Dictionary<string, string> DeviceDict;

        public Form1()
        {
            InitializeComponent();
            //myKeyEventHandeler = new KeyEventHandler(hook_KeyUp);
            //k_hook.KeyUpEvent += myKeyEventHandeler;
            //k_hook.Start();
            hook.RegisterWMInput(this.Handle);
            DeviceDict = KeyDeviceHook.GetHardwareDictionary();
            hook.DeviceKeyDown += Hook_DeviceKeyDown;
            hook.DeviceKeyUp += Hook_DeviceKeyUp;
        }

        private void Hook_DeviceKeyDown(string DeviceId, KeyEventArgs e)
        {
            if(cbKeyDown.Checked)KeyHandler("KeyDown",DeviceId,e);
        }

        private void Hook_DeviceKeyUp(string DeviceId, KeyEventArgs e)
        {
            if(cbKeyUp.Checked)KeyHandler("KeyUp  ",DeviceId,e);
        }

        private void KeyHandler(string prex,string DeviceId,KeyEventArgs e)
        {
            var DeviceName = DeviceDict.ContainsKey(DeviceId) ? DeviceDict[DeviceId] : "Unknown Application";
            textBox1.Text += prex + ":" + DateTime.Now.ToString("HH:mm:ss:ff") + "\t[" + Enum.GetName(typeof(Keys), e.KeyData) + "] " + DeviceName + "{" + DeviceId + "}" + "\r\n";
            textBox1.Select(textBox1.Text.Length, 0);
            textBox1.ScrollToCaret();
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == KeyDeviceHook.WM_INPUT)
                hook.processMessage(message);

            base.WndProc(ref message);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "log files (*.log)|*.log|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = "KeyLog";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //获得保存文件的路径
                string filePath = saveFileDialog.FileName;
                //保存
                using (FileStream fsWrite = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] buffer = Encoding.Default.GetBytes(textBox1.Text);
                    fsWrite.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
