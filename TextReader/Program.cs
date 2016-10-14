using System;
using Accessibility;

namespace TextReader
{
    class Program
    {
        /// <summary>
        /// 窗口类名
        /// </summary>
        private const string qqWindowClassName = "TXGuiFoundation";
        static void Main(string[] args)
        {
            var reader = new MSAATextReader(qqWindowClassName, "元", "消息");
            IAccessible msgWindow = reader.MsgContentWindow;
            var pcObtain = reader.PcObtained;
            string accName = msgWindow.accName[Win32.CHILDID_SELF];
            Console.WriteLine("pcObtained={0}", pcObtain);
            Console.WriteLine("accName={0}", accName);
            Console.WriteLine("accValue={0}",msgWindow.accValue[Win32.CHILDID_SELF]);
            Console.ReadKey();
        }
    }
}
