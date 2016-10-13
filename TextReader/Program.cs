using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accessibility;

namespace TextReader
{
    class Program
    {
        /// <summary>
        /// qq聊天窗口类名
        /// </summary>
        private const string qqWindowClassName = "TXGuiFoundation";
        static void Main(string[] args)
        {
            var reader = new MSAATextReader(qqWindowClassName, "元","消息");
            IAccessible msgWindow = reader.MsgContentWindow;
            Console.WriteLine(msgWindow.accValue);
            Console.ReadKey();
        }
    }
}
