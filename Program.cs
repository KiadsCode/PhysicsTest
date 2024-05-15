using PixoEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game window = new Game();
            window.Run(800, 480, 60);
        }
    }
}
