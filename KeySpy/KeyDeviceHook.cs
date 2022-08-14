using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeySpy
{

    public delegate void DeviceKeyDownEventHandler(string DeviceId, KeyEventArgs e);
    public delegate void DeviceKeyUpEventHandler(string DeviceId, KeyEventArgs e);

    internal class KeyDeviceHook
    {
        #region events

        public event DeviceKeyDownEventHandler DeviceKeyDown;
        public event DeviceKeyUpEventHandler DeviceKeyUp;

        #endregion

        #region fields
        private const int RIDEV_INPUTSINK = 0x100;
        private const int RIDEV_NoLegacy = 0x30;

        public const int WM_INPUT = 0x00FF;
        private const uint RID_INPUT = 0x10000003;
        private const uint RIDI_DEVICENAME = 0x20000007;

        #endregion

        #region structs

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

        #endregion

        #region api

        [DllImport("User32.dll")]
        extern static uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

        [DllImport("User32.dll")]
        extern static bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);

        [DllImport("User32.dll")]
        extern static uint GetRawInputDeviceInfo(IntPtr hDevice, uint uiCommand, IntPtr pData, ref uint pcbSize);

        #endregion

        #region methods

        public void RegisterWMInput(IntPtr handle)
        {
            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[1];
            rid[0].usUsagePage = 0x01;
            rid[0].usUsage = 0x06;
            rid[0].dwFlags = RIDEV_INPUTSINK | RIDEV_NoLegacy;
            rid[0].hwndTarget = handle;
            RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0]));
        }

        public void processMessage(Message message)
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

            string rawDeviceName = (string)Marshal.PtrToStringAnsi(pData);
            if(raw.keyboard.Flags == 0 && this.DeviceKeyDown != null) this.DeviceKeyDown.Invoke(GetDeviceId(rawDeviceName),new KeyEventArgs((Keys)Enum.ToObject(typeof(Keys), raw.keyboard.VKey)));
            else if(this.DeviceKeyUp != null) this.DeviceKeyUp.Invoke(GetDeviceId(rawDeviceName), new KeyEventArgs((Keys)Enum.ToObject(typeof(Keys), raw.keyboard.VKey)));

            Marshal.FreeHGlobal(buffer);
            Marshal.FreeHGlobal(pData);

        }

        private string GetDeviceId(string rawDeviceName)
        {
            string[] array = rawDeviceName.Split(new[] { '#' });
            return array.Length < 2 ? rawDeviceName : array[1];
        }

        /// <summary>
        /// 获取由硬件ID对应硬件描述字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetHardwareDictionary()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Keyboard");//如果要获取别的硬件改"win32_DiskDrive";
            foreach (ManagementObject mo in searcher.Get())
            {
                var array = mo["DeviceID"].ToString()
                    .Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                dict.Add(array[1], mo["Description"].ToString());
            }
            return dict;
        }

        #endregion
    }
}
