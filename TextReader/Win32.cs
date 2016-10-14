using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Accessibility;

namespace TextReader
{
    public class Win32
    {
        public const int WM_SETTEXT = 0x000C;
        public const int WM_CLICK = 0x00F5;
        public const int CHILDID_SELF = 0;
        public const int CHILDID_1 = 1;
        public const int OBJID_CLIENT = -4;

        [DllImport("User32.dll")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowEx(
            IntPtr parentHandle,
            IntPtr childAfter,
            string lpszClass,
            int sWindowTitle  /*HWND*/);

        /// <summary>
        /// 查找满足给定类名及窗口名的子窗口
        /// </summary>
        /// <param name="parentHandle">要查找子窗口的父窗口句柄</param>
        /// <param name="childAfter">从该指定子窗口的后面开始查找</param>
        /// <param name="className">查找的子窗口的类名</param>
        /// <param name="windowTitle">查找的子窗口的窗口名</param>
        /// <returns>返回满足条件的子窗口的句柄</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessageString(IntPtr hwnd, int wMsg, IntPtr wParam, string lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessageInt(IntPtr hwnd, int wMsg, IntPtr wParam, int lParam);

        [DllImport("Oleacc.dll")]
        public static extern int AccessibleObjectFromWindow(
        IntPtr hwnd,
        int dwObjectID,
        ref Guid refID,
        ref IAccessible ppvObject);

        [DllImport("Oleacc.dll")]
        public static extern int WindowFromAccessibleObject(
            IAccessible pacc,
            out IntPtr phwnd);

        /// <summary>
        /// 获得当前顶级窗口（特别注意不一定是hwndCurrent所指向的窗口）的第一层子项
        /// </summary>
        /// <param name="paccContainer"></param>
        /// <param name="iChildStart"></param>
        /// <param name="cChildren"><see cref="object[]"/>子项的数组指针</param>
        /// <param name="rgvarChildren"></param>
        /// <param name="pcObtained">输出参数 参数值和cChildren值一致，除非是获取的子项不存在</param>
        /// <returns></returns>
        [DllImport("Oleacc.dll")]
        public static extern int AccessibleChildren(
        Accessibility.IAccessible paccContainer,
        int iChildStart,
        int cChildren,
        [Out] object[] rgvarChildren,
        out int pcObtained);   
    }
}
