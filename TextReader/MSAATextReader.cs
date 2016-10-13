using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accessibility;

namespace TextReader
{
    public class MSAATextReader
    {
        private IAccessible msgContentWindow;
        /// <summary>
        /// 存储qq消息窗口的IAccessible类型引用
        /// </summary>
        public IAccessible MsgContentWindow
        {
            get
            {
                return msgContentWindow;
            }
        }

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
        /// 初始化实例
        /// </summary>
        /// <param name="className">查找的目标窗口(子项所在的主窗口)类名</param>
        /// <param name="windowTitle">查找的目标窗口(子项所在的主窗口)标题</param>
        /// <param name="destinationAccName">查找的目标子项的AccName的值</param>
        public MSAATextReader(string className, string windowTitle, string destinationAccName)
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

            if (IACurrent == null) throw new NullReferenceException(string.Format("找不到指定窗口名为\"{0}\"的窗口", windowTitle));
            //当前句柄窗口包含的直接下级子项个数
            int childCount = IACurrent.accChildCount;
            //所有子窗口集合
            object[] windowChildren = new object[childCount];
            int pcObtained;
            /*
             * 获得当前顶级窗口（特别注意不一定是hwndCurrent所指向的窗口）的第一层子项
             */
            Win32.AccessibleChildren(IACurrent, 0, childCount, windowChildren, out pcObtained);

            //获取消息窗口对应的IAccessible类型引用
            msgContentWindow = GetMessageContentWindow(windowChildren, destinationAccName);
        }

        /// <summary>
        /// 遍历所有子项
        ///     1.排除不是IAccessible类型的
        ///     2.递归获取消息窗口对应的子项IAcessible类型的引用
        /// </summary>
        /// <param name="windowChildren">需要遍历的所有子项</param>
        /// <param name="destinationAccName">查找的目标子项的AccName的值</param>
        /// <returns></returns>
        private IAccessible GetMessageContentWindow(object[] windowChildren, string destinationAccName)
        {
            if (msgContentWindow != null) return msgContentWindow;
            string accName;
            int accRole;
            foreach (object child in windowChildren)
            {
                //判定子项是否是IAcessible类型 COM对象
                var comobj = child.GetType().IsCOMObject;
                if (comobj)
                {
                    var iACurrentChild = (IAccessible)child;
                    accRole = (int)(iACurrentChild).get_accRole(Win32.CHILDID_SELF);
                    accName = (iACurrentChild).get_accName(Win32.CHILDID_SELF);
                    if (accName != destinationAccName)
                    {
                        //继续遍历子项，直到查找到匹配的目的子项
                        object[] childWindows = GetAccessibleChildren(iACurrentChild);
                        GetMessageContentWindow(childWindows, accName);
                    }
                    else
                    {
                        msgContentWindow = iACurrentChild;
                        break;
                    }
                }
            }
            return msgContentWindow;
        }
    }
}
