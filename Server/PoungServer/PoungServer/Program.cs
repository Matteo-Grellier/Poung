using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoungServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Poung Server";

            Server.Start(50, 26950);

            Console.WriteLine("Launching ...");
            Console.ReadKey();
        }
    }
}
