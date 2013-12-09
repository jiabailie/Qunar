#define WATCH_PREPROCESS_RESULT
//#undef  WATCH_PREPROCESS_RESULT
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
            int w = 0, h = 0;
            Bitmap source = null;
            iLine iline1 = null;
            iLine iline2 = null;
            List<Module> modules = null;
            byte[,] matrix = null;

            if (args.Length > 0)
            {
                modules = Template.read_Templates_To_Memory(Config.Processed_Template_Path, FileType.txt);

                if (!File.Exists(args[0])) { return; }

                source = Operations.ConvertJpg2Bmp(args[0]);

                // Remove black edges
                Operations.generate_White_Edges(source);

                // Find the long black line
                iline1 = QunarFeatureOperations.Find_Long_Connected_Lines(source.Width - 1, 0, -1, source);
                iline2 = QunarFeatureOperations.Find_Long_Connected_Lines(0, source.Width - 1, 1, source);

                matrix = SetOperations.Transform_Image_To_Matrix(source);

                matrix = SetOperations.Remove_Matrix_Blank_Regions(ref w, ref h, source.Width, source.Height, matrix);

#if WATCH_PREPROCESS_RESULT
                IO.write_Matrix_To_Txt<byte>(w, h, matrix, Config.Test_Processed_Path + "/test.txt");
#endif
            }
            else
            {
                Branch.main_Branch();
            }
        }
    }
}
