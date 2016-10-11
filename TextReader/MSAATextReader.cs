using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accessibility;

namespace TextReader
{
    public class MSAATextReader
    {
        public MSAATextReader(IntPtr parentHwnd)
        {
            Guid guidCOM = new Guid(0x618736E0, 0x3C3D, 0x11CF, 0x81, 0xC, 0x0, 0xAA, 0x0, 0x38, 0x9B, 0x71);
            //可访问的窗口集合
            Accessibility.IAccessible IACurrent = null;

            Win32.AccessibleObjectFromWindow(parentHwnd, (int)Win32.OBJID_CLIENT, ref guidCOM, ref IACurrent);
            IACurrent = (IAccessible)IACurrent.accParent;
            int childCount = IACurrent.accChildCount;
            //所有子窗口集合
            object[] windowChildren = new object[childCount];
            int pcObtained;
            Win32.AccessibleChildren(IACurrent, 0, childCount, windowChildren, out pcObtained);

            string accName;
            int accRole;
            foreach (IAccessible child in windowChildren) {
                accRole = (int)child.get_accRole(Win32.CHILDID_SELF);
                accName = child.get_accName(Win32.CHILDID_SELF);
            }
        }
    }
}
