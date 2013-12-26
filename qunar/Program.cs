using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.IO;

namespace qunar
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Branch.recognition_Branch(args);
            }
            else
            {
                Branch.main_Branch();
            }
        }
    }
}
