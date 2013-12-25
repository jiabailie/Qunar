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
            int[][] template = new int[3][];
            template[0] = new int[] { 0, -1, 0 };
            template[1] = new int[] { -1, 5, -1 };
            template[2] = new int[] { 0, -1, 0 };
            Bitmap source = Operations.ConvertJpg2Bmp("E:/Projects/qunar-file/input/0(64).bmp");

            Operations.generate_White_Edges(source);

            Scaling.Using_Sharpening_Filters(3, template, source);

            //source = Scaling.Image_Zoom_Out(3, source);

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
