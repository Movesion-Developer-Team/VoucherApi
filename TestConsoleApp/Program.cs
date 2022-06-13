using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = System.IO.Directory.GetCurrentDirectory();
            var rootPath = System.IO.Directory.GetParent(path).Parent.Parent.FullName;
            Console.WriteLine(path);
            Console.WriteLine(rootPath);
            Console.ReadLine();
        }
    }
}
