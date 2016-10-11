using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new MSAATextReader((IntPtr)0x00030be4);
        }
    }
}
