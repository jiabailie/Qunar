#define WRITE_BLACK
#define GENERATE_TEMPLATE
#undef  WRITE_BLACK
//#undef  GENERATE_TEMPLATE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace qunar
{
    public class Template
    {
        private static char[] cset = new char[] { 
                    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
                    'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 
                    'u', 'v', 'w', 'x', 'y', 'z' };

        /// <summary>
        /// Write all template into text files to test if did it correctly.
        /// </summary>
        /// <param name="inpath"></param>
        /// <param name="outpath"></param>
        public static void write_Template_Into_Text_Files(string inpath, string outpath, FileType fileType)
        {
            try
            {
                int i = 0;
                string ninpath = "";
                string noutpath = "";
                string filepath = "";
                Bitmap source = null;
                foreach (char c in cset)
                {
                    try
                    {
                        ninpath = inpath + c + "/";
                        for (i = 0; i < (1 << 10); i++)
                        {
                            filepath = string.Format("{0}0({1}).{2}", ninpath, i, fileType);

                            if (!File.Exists(filepath)) { break; }

                            source = new Bitmap(filepath);

#if GENERATE_TEMPLATE
                            noutpath = string.Format("{0}/{1}({2}).txt", outpath, c, i);
                            generate_Template_Into_Text(source, noutpath);
#else
                            noutpath = string.Format("{0}/t_{1}_{2}.txt", outpath, c, i);
                            write_Template_To_Character(source, noutpath);
#endif

                        }
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
        }

        public static List<Module> read_Templates_To_Memory(string inpath, FileType fileType)
        {
            List<Module> modules = new List<Module>();
            try
            {
                int i = 0;
                string filepath = "";
                foreach (char c in cset)
                {
                    for (i = 0; i < (1 << 10); i++)
                    {
                        filepath = string.Format("{0}{1}({2}).{3}", inpath, c, i, fileType);

                        if (!File.Exists(filepath)) { break; }
                        modules.Add(read_Template_From_Text_To_Memory(c, filepath));
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("read_Templates_To_Memory:" + e.Message);
            }
            return modules;
        }

        /// <summary>
        /// Generate text format templates.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="outpath"></param>
        public static void generate_Template_Into_Text(Bitmap source, string outpath)
        {
            try
            {
                int i = 0, j = 0;
                int cG = 0, cR = 0;
                int[,] matrix = new int[source.Width, source.Height];
                int lt_w = int.MaxValue, lt_h = int.MaxValue;
                int rb_w = int.MinValue, rb_h = int.MinValue;
                StreamWriter sw = new StreamWriter(outpath);

                for (j = 0; j < source.Height; j++)
                {
                    for (i = 0; i < source.Width; i++)
                    {
                        Color color = source.GetPixel(i, j);
                        if (color.R + color.G + color.B == 255)
                        {
                            if (color.G == 255)
                            {
                                matrix[i, j] = 1;
                                cG++;
                            }
                            else if (color.R == 255)
                            {
                                matrix[i, j] = 2;
                                cR++;
                            }
                            if (i < lt_w) { lt_w = i; }
                            if (i > rb_w) { rb_w = i; }
                            if (j < lt_h) { lt_h = j; }
                            if (j > rb_h) { rb_h = j; }
                        }
                    }
                }
                sw.WriteLine(string.Format("{0} {1} {2} {3}", (rb_h - lt_h + 1).ToString(), (rb_w - lt_w + 1).ToString(), cG.ToString(), cR.ToString()));

                for (j = lt_h; j <= rb_h; j++)
                {
                    for (i = lt_w; i <= rb_w; i++)
                    {
                        sw.Write(matrix[i, j]);
                    }
                    sw.WriteLine();
                }
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Save source bitmap image as text which contains all positions' pixel value
        /// random position value = {R, G, B}
        /// </summary>
        /// <param name="source"></param>
        /// <param name="outpath"></param>
        public static void write_Template_To_Character(Bitmap source, string outpath)
        {
            try
            {
                int i = 0, j = 0;
                StreamWriter sw = new StreamWriter(outpath);

                for (j = 0; j < source.Height; j++)
                {
                    for (i = 0; i < source.Width; i++)
                    {
                        Color color = source.GetPixel(i, j);
                        if (color.R + color.G + color.B == 0)
                        {
#if WRITE_BLACK
                            sw.Write("B");
#else
                            sw.Write(" ");
#endif
                        }
                        else if (color.R + color.G + color.B == 255)
                        {
                            sw.Write("G");
                        }
                        else if ((color.R & color.G & color.B) == 255)
                        {
                            sw.Write(" ");
                        }
                    }
                    sw.WriteLine();
                }
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Read template into memory.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="inpath"></param>
        /// <returns></returns>
        public static Module read_Template_From_Text_To_Memory(char character, string inpath)
        {
            int i = 0, j = 0;
            int h = 0, w = 0;
            int g = 0, r = 0;
            string tmp = null;
            StreamReader sr = new StreamReader(inpath);

            string[] split = sr.ReadLine().Trim().Split(' ');

            h = Convert.ToByte(split[0]);
            w = Convert.ToByte(split[1]);
            g = Convert.ToByte(split[2]);
            r = Convert.ToByte(split[3]);

            Module module = new Module(character, h, w, g, r, null);

            for (i = 0; i < h; i++)
            {
                tmp = sr.ReadLine();
                for (j = 0; j < w; j++)
                {
                    module.matrix[i, j] = Convert.ToByte(tmp[j] - '0');
                }
            }

            return module;
        }
    }
}
