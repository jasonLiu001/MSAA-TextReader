using System;
using Accessibility;
using System.Text;
using System.Text.RegularExpressions;

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
            string accValue = msgWindow.accValue[Win32.CHILDID_SELF];
            Console.WriteLine("pcObtained={0}", pcObtain);
            Console.WriteLine("accName={0}", accName);
            Console.WriteLine("accValue={0}", accValue);

            /*
             * 方法一：正则表达式中字符及符号编码范围
             * 1).中文汉字的正则字符编码范围 UTF-8 (Unicode) \u4e00-\u9fa5 (中文)
             * 2).大小写英文字母 a-zA-Z
             * 3).标点符号 \uFF00-\uFFEF  (全角ASCII、全角中英文标点、半宽片假名、半宽平假名、半宽韩文字母)
             * 4).数字 0-9
             * 5).空格 
             * 
             * 方法二：使用元字符 \p{P}
             * 小写 p 是 property 的意思，表示 Unicode 属性，用于 Unicode 正表达式的前缀。中括号内的“P”表示Unicode 字符集七个字符属性之一：标点字符。
             * 其他六个属性：
             * L：字母（包括中文和英文）；
             * M：标记符号（一般不会单独出现）；
             * Z：分隔符（比如空格、换行等）；
             * S：符号（比如数学符号、货币符号等）；
             * N：数字（比如阿拉伯数字、罗马数字等）；
             * C：其他字符。
             */
            var array = accValue.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in array)
            {
                Console.WriteLine("原始字符={0}", s);
                Console.WriteLine("替换以后={0}", Regex.Replace(s, @"[^\p{L}\p{M}\p{Z}\p{N}\p{P}]", string.Empty));
            }

            var temp = Regex.Replace(accValue, @"[^\p{L}\p{M}\p{Z}\p{N}\p{P}]", string.Empty);
            Console.WriteLine(temp);

            Console.ReadKey();
        }

    }
}
