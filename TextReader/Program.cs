using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var reader = new MSAATextReader(qqWindowClassName, "元");
        }
    }
}
