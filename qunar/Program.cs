#define DEBUG
//#undef  DEBUG
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
#if DEBUG
            Bitmap source = Operations.ConvertJpg2Bmp("E:/Projects/qunar-file/input/0(64).bmp");

            source = Scaling.ImageZoomOut(3, source);

            source.Save("E:/Projects/qunar-file/test/1.bmp", ImageFormat.Bmp);
#else
            if (args.Length > 0)
            {
                Branch.recognition_Branch(args);
            }
            else
            {
                Branch.main_Branch();
            }
#endif
        }
    }
}
