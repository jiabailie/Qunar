#define Using_Non_Optimize_Branch
#define Watch_Pre_Process_Result
#define Remove_Thin_Vertical_Line
#define Find_Long_Thin_Lines
#define Remove_Suspending_Points
#undef  Using_Non_Optimize_Branch
#undef  Watch_Pre_Process_Result
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
        /// Recognition branch.
        /// </summary>
        /// <param name="args"></param>
        public static void recognition_Branch(string[] args)
        {
            int w = 0, h = 0;
#if Watch_Pre_Process_Result
            int s = 0, e = 0;
#endif
            string ret = "";
            Bitmap source = null;
            iLine iline1 = null;
            iLine iline2 = null;
            byte[,] matrix = null;

            List<Module> modules = null;

            modules = Template.read_Templates_To_Memory(Config.Processed_Template_Path, FileType.txt);

            if (!File.Exists(args[0])) { return; }
#if Watch_Pre_Process_Result
            s = args[0].IndexOf('(');
            e = args[0].IndexOf(')');
#endif
            source = Operations.Convert_Jpg2Bmp(args[0]);

            w = source.Width;
            h = source.Height;

            // Do uniformization operation
            Operations.Uniformization_Bmp(source);

#if Using_Non_Optimize_Branch
            // Remove black edges
            Operations.generate_White_Edges(source);

            // Find the long black line
            iline1 = QunarFeatureOperations.Find_Long_Connected_Lines(source.Width - 1, 0, -1, source);
            iline2 = QunarFeatureOperations.Find_Long_Connected_Lines(0, source.Width - 1, 1, source);

            // Transform the processed input image into 0/1 matrix.
            matrix = SetOperations.Transform_Image_To_Matrix(source);
#else

            // Transform the processed input image into 0/1 matrix.
            matrix = SetOperations.Transform_Image_To_Matrix(source);

            // Remove black edges
            Optimize.Generate_White_Edges(w, h, ref matrix);

            // Find the long black line
            iline1 = Optimize.Find_Long_Connected_Lines(w - 1, 0, -1, w, h, matrix);

            iline2 = Optimize.Find_Long_Connected_Lines(0, w - 1, 1, w, h, matrix);

#endif
            // Remove the redundant white regions.
            matrix = SetOperations.Remove_Matrix_Blank_Regions(ref w, ref h, source.Width, source.Height, matrix);

#if Watch_Pre_Process_Result
            // For debug, output the matrix into a text file.
            IO.write_Matrix_To_Txt<byte>(w, h, matrix, Config.Test_Processed_Path + "/test" + DateTime.Now.Ticks.ToString() + ".txt");
#endif

            ret = Recognition<byte>.Do_Image_Recognition(w, h, matrix, modules);
#if Watch_Pre_Process_Result
            IO.write_Result_To_Text_File(string.Format("{0} {1}", args[0].Substring(s + 1, e - s - 1), ret), Config.Result_Save_Path);
#else
            IO.write_Result_To_Text_File(ret, Config.Result_Save_Path);
#endif
        }

        /// <summary>
        /// Programe branch.
        /// </summary>
        public static void main_Branch()
        {
            int branch = 0;

            double correctRate = 0.0;

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
                sb.Append("7. Judge how many character are correct.\n");
                sb.Append("9. Exit.\n");

                Console.Write(sb.ToString());

                branch = Convert.ToInt32(Console.ReadLine());

                switch (branch)
                {
                    case 1:
                        Branch.write_Bmp_To_Txt_Using_Threshold(Config.Sample_Path, Config.Binary_Bmp_Images_Path, FileType.jpg);
                        break;
                    case 2:
                        Branch.write_Bmp_To_Bmp_Using_Threshold(Config.Sample_Path, Config.Binary_Path, FileType.bmp);
                        break;
                    case 3:
                        Branch.write_Long_Black_Lines_Into_Files(Config.Sample_Path, Config.Long_Black_Line_Path, FileType.bmp);
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
                    case 7:
                        correctRate = CalCorrectRate.Calculate_Correct_Rate();
                        Console.WriteLine(correctRate);
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
                for (int i = 0; i < (1 << 10); i++)
                {
                    inpath = filepath + "0(" + i.ToString() + ")." + filetype.ToString();
                    outpath = savepath + "0(" + i.ToString() + ")." + FileType.txt.ToString();

                    if (!File.Exists(inpath)) { break; }

                    IO.write_Bmp_To_Avg_Number(inpath, outpath);
                }
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
            int i = 0;
            string inpath = "";
            string outpath = "";
            Bitmap bitmap = null;

            try
            {
                if (filetype != FileType.bmp && filetype != FileType.jpg)
                {
                    throw new Exception("The file type is not image.");
                }

                for (i = 0; i < (1 << 10); i++)
                {
                    inpath = filepath + "0(" + i.ToString() + ")." + filetype.ToString();
                    outpath = savepath + "0(" + i.ToString() + ")." + FileType.bmp.ToString();

                    if (!File.Exists(inpath)) { break; }

                    bitmap = Operations.Convert_Jpg2Bmp(inpath);

                    // Do uniformization operation
                    Operations.Uniformization_Bmp(bitmap);

                    // Remove black edges
                    Operations.Generate_White_Edges(bitmap);

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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void write_Bmp_To_Txt_Using_Threshold(string filepath, string savepath, FileType filetype)
        {
            int i = 0;
            string inpath = "";
            string outpath = "";
            Bitmap bitmap = null;

            try
            {
                if (filetype != FileType.bmp && filetype != FileType.jpg)
                {
                    throw new Exception("The file type is not image.");
                }

                for (i = 0; i < (1 << 10); i++)
                {
                    inpath = filepath + "0(" + i.ToString() + ")." + filetype.ToString();
                    outpath = savepath + "0(" + i.ToString() + ")." + FileType.txt.ToString();

                    if (!File.Exists(inpath)) { break; }

                    bitmap = Operations.Convert_Jpg2Bmp(inpath);

                    // Do uniformization operation
                    Operations.Uniformization_Bmp(bitmap);

                    // Remove black edges
                    Operations.Generate_White_Edges(bitmap);

#if     Remove_Suspending_Points
                    // Remove suspending points
                    Operations.Remove_Suspending_Points(bitmap);
#endif
                    IO.write_Bmp_To_Avg_Number(bitmap, outpath);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void write_Long_Black_Lines_Into_Files(string filepath, string savepath, FileType filetype)
        {
            int i = 0;
            string inpath = "";
            string outpath = "";
            Bitmap bitmap = null;

            try
            {
                if (filetype != FileType.bmp && filetype != FileType.jpg)
                {
                    throw new Exception("The file type is not image.");
                }

                for (i = 0; i < (1 << 10); i++)
                {
                    inpath = filepath + "0(" + i.ToString() + ")." + filetype.ToString();
                    outpath = savepath + "0(" + i.ToString() + ")." + FileType.bmp.ToString();

                    if (!File.Exists(inpath)) { break; }

                    bitmap = Operations.Convert_Jpg2Bmp(inpath);

                    // Do uniformization operation
                    Operations.Uniformization_Bmp(bitmap);

                    // Remove black edges
                    Operations.Generate_White_Edges(bitmap);

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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
