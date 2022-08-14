using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyPromoter
{
    public class Win32ApiKeyDown : IKeyDown
    {
        public void KeyDown(Keys key)
        {
            keybd_event((byte)key, 0, 0, 0);  //模拟键盘按下
            Thread.Sleep(30);
            keybd_event((byte)key, 0, 2, 0);  //模拟键盘抬起
        }


        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(
            byte bVk,    //虚拟键值
            byte bScan,// 一般为0
            int dwFlags,  //这里是整数类型  0 为按下，2为释放
            int dwExtraInfo  //这里是整数类型 一般情况下设成为 0
        );
    }
}
