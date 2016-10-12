using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accessibility;

namespace TextReader
{
    public class MSAATextReader
    {
        /// <summary>
        /// 获取当前窗口的子项
        /// </summary>
        /// <param name="paccContainer"></param>
        /// <returns></returns>
        private object[] GetAccessibleChildren(IAccessible paccContainer)
        {
            object[] rgvarChildren = new object[paccContainer.accChildCount];
            int pcObtained;
            Win32.AccessibleChildren(paccContainer, 0, paccContainer.accChildCount, rgvarChildren, out pcObtained);
            return rgvarChildren;
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className">窗口类名</param>
        /// <param name="windowTitle">窗口标题</param>
        public MSAATextReader(string className, string windowTitle)
        {
            //获取指定窗口句柄
            IntPtr hwnd = Win32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, className, windowTitle);
            Guid guidCOM = new Guid(0x618736E0, 0x3C3D, 0x11CF, 0x81, 0xC, 0x0, 0xAA, 0x0, 0x38, 0x9B, 0x71);

            Accessibility.IAccessible IACurrent = null;
            /*
             * 通过指定的窗口句柄，获取IAccessible接口指针
             * 调用AccessibleObjectFromWindow
             * 获得句柄hwndCurrent指向的鼠标当前窗口所在的顶级窗口，放入IACurrent，供访问者使用
             */
            Win32.AccessibleObjectFromWindow(hwnd, (int)Win32.OBJID_CLIENT, ref guidCOM, ref IACurrent);

            /*
             * 获取当前句柄窗口的父窗口的IAccessible接口指针，指的是桌面上当前的所有窗口
             * 通过inspect查看时，这些窗口的role值为window
             */
            IACurrent = (IAccessible)IACurrent.accParent;
            int childCount = IACurrent.accChildCount;
            //所有子窗口集合
            object[] windowChildren = new object[childCount];
            int pcObtained;
            /*
             * 获得当前顶级窗口（特别注意不一定是hwndCurrent所指向的窗口）的第一层子项
             */
            Win32.AccessibleChildren(IACurrent, 0, childCount, windowChildren, out pcObtained);

            IAccessible _inputBox = null;

            string accName;
            int accRole;
            foreach (IAccessible child in windowChildren)
            {
                //首先判断子项的Role值，33为【列表】，文件为34【列表项目】，我们需要找到0x2A【输入框】
                accRole = (int)child.get_accRole(Win32.CHILDID_SELF);
                accName = child.get_accName(Win32.CHILDID_SELF);
                if (accRole == 9 && accName == windowTitle)
                {
                    object[] clientChild = GetAccessibleChildren(child);
                    IAccessible client = (IAccessible)clientChild[1];
                    clientChild = GetAccessibleChildren(client);

                    foreach (IAccessible childChild in clientChild)
                    {
                        accRole = (int)childChild.get_accRole(Win32.CHILDID_SELF);
                        accName = childChild.get_accName(Win32.CHILDID_SELF);
                        if (accName == "消息")
                        {
                            _inputBox = childChild;
                        }
                    }
                }
            }
        }
    }
}
