#define Remove_Thin_Vertical_Line
#define Find_Long_Thin_Lines
#define Remove_Suspending_Points
#undef  Remove_Thin_Vertical_Line
#undef  Find_Long_Thin_Lines
#undef  Remove_Suspending_Points
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace qunar
{
    public class Branch
    {
        /// <summary>
        /// Programe branch.
        /// </summary>
        public static void main_Branch()
        {
            int branch = 0;

            List<Module> modules = null;

            while (branch >= 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("Select the branch what you want to do:\n");
                sb.Append("1. Write bmp into text files using threshold.\n");
                sb.Append("2. Write bmp into bmp files using threshold.\n");
                sb.Append("3. Recognize the long black lines and write them into bmp files.\n");
                sb.Append("4. Generate new templates using raw templates.\n"); 
                sb.Append("5. Wirte template files into text files.\n");
                sb.Append("6. Read template text files into memory.\n");
                sb.Append("9. Exit.\n");

                Console.Write(sb.ToString());

                branch = Convert.ToInt32(Console.ReadLine());

                switch (branch)
                {
                    case 1:
                        Branch.write_Bmp_To_Txt_Using_Threshold(Config.Sample_Path, Config.Binary_Bmp_Images_Path, FileType.jpg);
                        break;
                    case 2:
                        Branch.write_Bmp_To_Bmp_Using_Threshold(Config.Sample_Path, Config.Binary_Path, FileType.jpg);
                        break;
                    case 3:
                        Branch.write_Long_Black_Lines_Into_Files(Config.Sample_Path, Config.Long_Black_Line_Path, FileType.jpg);
                        break;
                    case 4:
                        Template.generate_Template_From_Img(Config.Raw_template_Path, Config.Template_Path, FileType.bmp);
                        break;
                    case 5:
                        Template.write_Template_Into_Text_Files(Config.Template_Path, Config.Processed_Template_Path, FileType.bmp);
                        break;
                    case 6:
                        modules = Template.read_Templates_To_Memory(Config.Processed_Template_Path, FileType.txt);
                        break;
                    case 9:
                        branch = -1;
                        break;
                    default:
                        branch = 0;
                        break;
                }
            }
        }
        /// <summary>
        /// Write the bmp images into number format to judge the threshold.
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="savepath"></param>
        /// <param name="filetype"></param>
        public static void write_Bmp_To_Txt_Batch(string filepath, string savepath, FileType filetype)
        {
            try
            {
                if (filetype != FileType.bmp && filetype != FileType.jpg)
                {
                    throw new Exception("The file type is not image.");
                }

                string inpath = "";
                string outpath = "";
                try
                {
                    for (int i = 0; i < (1 << 10); i++)
                    {
                        inpath = filepath + "0(" + i.ToString() + ")." + filetype.ToString();
                        outpath = savepath + "0(" + i.ToString() + ")." + FileType.txt.ToString();

                        if (!File.Exists(inpath)) { break; }

                        IO.write_Bmp_To_Avg_Number(inpath, outpath);
                    }
                }
                catch (Exception) { }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void write_Bmp_To_Txt_Using_Threshold(string filepath, string savepath, FileType filetype)
        {
            try
            {
                if (filetype != FileType.bmp && filetype != FileType.jpg)
                {
                    throw new Exception("The file type is not image.");
                }

                string inpath = "";
                string outpath = "";
                Bitmap bitmap = null;

                try
                {
                    for (int i = 0; i < (1 << 10); i++)
                    {
                        inpath = filepath + "0(" + i.ToString() + ")." + filetype.ToString();
                        outpath = savepath + "0(" + i.ToString() + ")." + FileType.txt.ToString();

                        if (!File.Exists(inpath)) { break; }

                        bitmap = Operations.ConvertJpg2Bmp(inpath);

                        // Do uniformization operation
                        Operations.UniformizationBmp(bitmap);

                        // Remove black edges
                        Operations.generate_White_Edges(bitmap);
#if     Remove_Suspending_Points
                        // Remove suspending points
                        Operations.Remove_Suspending_Points(bitmap);
#endif
                        IO.write_Bmp_To_Avg_Number(bitmap, outpath);
                    }
                }
                catch (Exception) { }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void write_Long_Black_Lines_Into_Files(string filepath, string savepath, FileType filetype)
        {
            try
            {
                if (filetype != FileType.bmp && filetype != FileType.jpg)
                {
                    throw new Exception("The file type is not image.");
                }

                string inpath = "";
                string outpath = "";
                Bitmap bitmap = null;
                try
                {
                    for (int i = 0; i < (1 << 10); i++)
                    {
                        inpath = filepath + "0(" + i.ToString() + ")." + filetype.ToString();
                        outpath = savepath + "0(" + i.ToString() + ")." + FileType.bmp.ToString();

                        if (!File.Exists(inpath)) { break; }

                        bitmap = Operations.ConvertJpg2Bmp(inpath);

                        // Do uniformization operation
                        Operations.UniformizationBmp(bitmap);

                        // Remove black edges
                        Operations.generate_White_Edges(bitmap);

#if     Remove_Suspending_Points
                        // Remove suspending points
                        Operations.Remove_Suspending_Points(bitmap);
#endif

                        // Find the long black line
                        iLine iline1 = QunarFeatureOperations.Find_Long_Connected_Lines(bitmap.Width - 1, 0, -1, bitmap);
                        iLine iline2 = QunarFeatureOperations.Find_Long_Connected_Lines(0, bitmap.Width - 1, 1, bitmap);

                        bitmap.Save(outpath, ImageFormat.Bmp);
                    }
                }
                catch (Exception) { }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Using Threshold to process image and write them into certain position.
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="savepath"></param>
        /// <param name="filetype"></param>
        public static void write_Bmp_To_Bmp_Using_Threshold(string filepath, string savepath, FileType filetype)
        {
            try
            {
                if (filetype != FileType.bmp && filetype != FileType.jpg)
                {
                    throw new Exception("The file type is not image.");
                }

                string inpath = "";
                string outpath = "";
                Bitmap bitmap = null;
                try
                {
                    for (int i = 0; i < (1 << 10); i++)
                    {
                        inpath = filepath + "0(" + i.ToString() + ")." + filetype.ToString();
                        outpath = savepath + "0(" + i.ToString() + ")." + FileType.bmp.ToString();

                        if (!File.Exists(inpath)) { break; }

                        bitmap = Operations.ConvertJpg2Bmp(inpath);

                        // Do uniformization operation
                        Operations.UniformizationBmp(bitmap);

                        // Remove black edges
                        Operations.generate_White_Edges(bitmap);

#if     Remove_Suspending_Points
                        // Remove suspending points
                        Operations.Remove_Suspending_Points(bitmap);
#endif

#if     Remove_Thin_Vertical_Line
                        // Remove thin vertical lines
                        Operations.Remove_Thin_Vertical_Lines(bitmap);
#endif

                        bitmap.Save(outpath, ImageFormat.Bmp);
                    }
                }
                catch (Exception) { }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
