#define DEBUG
#undef  DEBUG
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
            int[] template = new int[] { 0, -1, 0, -1, 5, -1, 0, -1, 0 };

            Bitmap source = Operations.Convert_Jpg2Bmp("E:/Projects/qunar-file/input/0(64).bmp");

            Scaling.Using_Sharpening_Filters(3, template, source);

            source.Save("E:/Projects/qunar-file/test/1_using_sharpening_filters.bmp", ImageFormat.Bmp);            

            Operations.Generate_White_Edges(source);

            Operations.Fill_One_Width_Blanks(source);

            source.Save("E:/Projects/qunar-file/test/2_remove_edges.bmp", ImageFormat.Bmp);

            source = Scaling.Image_Zoom_Out(2, source);

            source.Save("E:/Projects/qunar-file/test/3_image_zoom_out.bmp", ImageFormat.Bmp);

            source = Scaling.Image_Zoom_In(source.Width * 2, source.Height * 2, source);

            source.Save("E:/Projects/qunar-file/test/4_image_zoom_in.bmp", ImageFormat.Bmp);

            Operations.Uniformization_Bmp(source);

            source.Save("E:/Projects/qunar-file/test/5_do_image_uniformization.bmp", ImageFormat.Bmp);
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
