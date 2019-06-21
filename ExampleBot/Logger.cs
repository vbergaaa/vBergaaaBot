using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBergaaaBot
{
    class Logger
    {
        public static void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public static void WriteLine()
        {
            WriteLine("");
        }
    }
}
