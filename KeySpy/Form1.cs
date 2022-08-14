using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private KeyEventHandler myKeyEventHandeler = null; //按键钩子
        private KeyboardHook k_hook = new KeyboardHook();
        private Dictionary<string, string> DeviceDict;

        public Form1()
        {
            InitializeComponent();
            //myKeyEventHandeler = new KeyEventHandler(hook_KeyUp);
            //k_hook.KeyUpEvent += myKeyEventHandeler;
            //k_hook.Start();
            RegisterWMInput();
            DeviceDict = GethardwareParameters();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void hook_KeyUp(object sender, KeyEventArgs e)
        {
            //  这里写具体实现
            textBox1.Text += "捕捉到了" + e.KeyCode.ToString() + "键释放\t\r\n";
        }

        #region ReadKeyDeviceName

        private const int RIDEV_INPUTSINK = 0x100;
        private const int RIDEV_NoLegacy = 0x30;

        private const int WM_INPUT = 0x00FF;
        private const uint RID_INPUT = 0x10000003;
        private const uint RIDI_DEVICENAME = 0x20000007;

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTDEVICE

        {

            [MarshalAs(UnmanagedType.U2)]

            public ushort usUsagePage;

            [MarshalAs(UnmanagedType.U2)]

            public ushort usUsage;

            [MarshalAs(UnmanagedType.U4)]

            public int dwFlags;

            public IntPtr hwndTarget;

        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTHEADER

        {

            [MarshalAs(UnmanagedType.U4)]

            public int dwType;

            [MarshalAs(UnmanagedType.U4)]

            public int dwSize;

            public IntPtr hDevice;

            [MarshalAs(UnmanagedType.U4)]

            public int wParam;

        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct RAWINPUT

        {

            [FieldOffset(0)]

            public RAWINPUTHEADER header;

            [FieldOffset(16)]

            public RAWMOUSE mouse;

            [FieldOffset(16)]

            public RAWKEYBOARD keyboard;

            [FieldOffset(16)]

            public RAWHID hid;

        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWKEYBOARD

        {

            [MarshalAs(UnmanagedType.U2)]

            public ushort MakeCode;

            [MarshalAs(UnmanagedType.U2)]

            public ushort Flags;

            [MarshalAs(UnmanagedType.U2)]

            public ushort Reserved;

            [MarshalAs(UnmanagedType.U2)]

            public ushort VKey;

            [MarshalAs(UnmanagedType.U4)]

            public uint Message;

            [MarshalAs(UnmanagedType.U4)]

            public uint ExtraInformation;

        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct RAWMOUSE
        {
            [MarshalAs(UnmanagedType.U2)]
            [FieldOffset(0)]
            public ushort usFlags;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(4)]
            public uint ulButtons;
            [FieldOffset(4)]
            public BUTTONSSTR buttonsStr;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(8)]
            public uint ulRawButtons;
            [FieldOffset(12)]
            public int lLastX;
            [FieldOffset(16)]
            public int lLastY;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(20)]
            public uint ulExtraInformation;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct BUTTONSSTR
        {
            [MarshalAs(UnmanagedType.U2)]
            public ushort usButtonFlags;
            [MarshalAs(UnmanagedType.U2)]
            public ushort usButtonData;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWHID
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwSizHid;
            [MarshalAs(UnmanagedType.U4)]
            public int dwCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RID_DEVICE_INFO_HID
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwVendorId;
            [MarshalAs(UnmanagedType.U4)]
            public int dwProductId;
            [MarshalAs(UnmanagedType.U4)]
            public int dwVersionNumber;
            [MarshalAs(UnmanagedType.U2)]
            public ushort usUsagePage;
            [MarshalAs(UnmanagedType.U2)]
            public ushort usUsage;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RID_DEVICE_INFO_KEYBOARD
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
            [MarshalAs(UnmanagedType.U4)]
            public int dwSubType;
            [MarshalAs(UnmanagedType.U4)]
            public int dwKeyboardMode;
            [MarshalAs(UnmanagedType.U4)]
            public int dwNumberOfFunctionKeys;
            [MarshalAs(UnmanagedType.U4)]
            public int dwNumberOfIndicators;
            [MarshalAs(UnmanagedType.U4)]
            public int dwNumberOfKeysTotal;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct RID_DEVICE_INFO
        {
            [FieldOffset(0)]
            public int cbSize;
            [FieldOffset(4)]
            public int dwType;
            [FieldOffset(8)]
            public RID_DEVICE_INFO_MOUSE mouse;
            [FieldOffset(8)]
            public RID_DEVICE_INFO_KEYBOARD keyboard;
            [FieldOffset(8)]
            public RID_DEVICE_INFO_HID hid;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RID_DEVICE_INFO_MOUSE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwId;
            [MarshalAs(UnmanagedType.U4)]
            public int dwNumberOfButtons;
            [MarshalAs(UnmanagedType.U4)]
            public int dwSampleRate;
            [MarshalAs(UnmanagedType.U4)]
            public int fHasHorizontalWheel;
        }

        [DllImport("User32.dll")]
        extern static uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

        [DllImport("User32.dll")]
        extern static bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);

        [DllImport("User32.dll")]
        extern static uint GetRawInputDeviceInfo(IntPtr hDevice, uint uiCommand, IntPtr pData, ref uint pcbSize);

        private void RegisterWMInput()
        {
            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[1];

            rid[0].usUsagePage = 0x01;

            rid[0].usUsage = 0x06;

            rid[0].dwFlags = RIDEV_INPUTSINK | RIDEV_NoLegacy;

            rid[0].hwndTarget = this.Handle;

            RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0]));
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == WM_INPUT)
                processMessage(message);

            base.WndProc(ref message);
        }

        private void processMessage(Message message)
        {
            uint dwSize = 0;

            GetRawInputData(message.LParam, RID_INPUT, IntPtr.Zero, ref dwSize, (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER)));

            IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);

            GetRawInputData(message.LParam, RID_INPUT, buffer, ref dwSize, (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER)));

            RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RAWINPUT));

            uint size = (uint)Marshal.SizeOf(typeof(RID_DEVICE_INFO));

            GetRawInputDeviceInfo(raw.header.hDevice, RIDI_DEVICENAME, IntPtr.Zero, ref size);
            IntPtr pData = Marshal.AllocHGlobal((int)size);
            GetRawInputDeviceInfo(raw.header.hDevice, RIDI_DEVICENAME, pData, ref size);

            string deviceName = (string)Marshal.PtrToStringAnsi(pData);
            string[] array = deviceName.Split(new[] {'#'});
            if (array.Length < 2)
            {
                textBox1.Text += DateTime.Now.ToString("HH:mm:ss:ff") + "\t[" + Enum.GetName(typeof(Keys), raw.keyboard.VKey) +"] Unknown Application{" + deviceName + "}" + "\r\n";
            }
            else
            {
                textBox1.Text += DateTime.Now.ToString("HH:mm:ss:ff") + "\t[" + Enum.GetName(typeof(Keys), raw.keyboard.VKey) + "] " + DeviceDict[array[1]] + "{ " + array[1] + "}" + "\r\n";
            }

            textBox1.Select(textBox1.Text.Length, 0);
            textBox1.ScrollToCaret();


            //判断deviceName来自哪个设备。做对应的处理

            //doSomething..             

            //下面两句是释放了缓存
            Marshal.FreeHGlobal(buffer);
            Marshal.FreeHGlobal(pData);

        }

        /// <summary>
        /// 获取硬件信息String，返回值为string类型
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,string> GethardwareParameters()
        {
            Dictionary<string,string> parameters = new Dictionary<string,string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Keyboard");//如果要获取别的硬件改"win32_DiskDrive";
            string ParameterInformation = "";
            foreach (ManagementObject mo in searcher.Get())
            {
                var array = mo["DeviceID"].ToString()
                    .Split(new string[] {"\\"}, StringSplitOptions.RemoveEmptyEntries);
                parameters.Add(array[1], mo["Description"].ToString());
            }
            return parameters;
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //k_hook.Stop();
        }
    }
}
