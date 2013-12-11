﻿#define WRITE_BLACK
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
using System.Drawing.Imaging;

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
                int score = 0;
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
                            }
                            else if (color.R == 255)
                            {
                                matrix[i, j] = 2;
                            }
                            if (i < lt_w) { lt_w = i; }
                            if (i > rb_w) { rb_w = i; }
                            if (j < lt_h) { lt_h = j; }
                            if (j > rb_h) { rb_h = j; }
                        }
                    }
                }
                score = Config.Green_Point_Amount * Config.Green_Score + Config.Red_Point_Amount * Config.Red_Score;
                sw.WriteLine(string.Format("{0} {1} {2} {3} {4}", (rb_h - lt_h + 1).ToString(), (rb_w - lt_w + 1).ToString(), Config.Green_Point_Amount, Config.Red_Point_Amount, score.ToString()));

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
                throw new Exception("write_Template_To_Character:" + ex.Message);
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
            try
            {
                int i = 0, j = 0;
                int h = 0, w = 0;
                int g = 0, r = 0;
                int score = 0;
                string tmp = null;
                StreamReader sr = new StreamReader(inpath);

                string[] split = sr.ReadLine().Trim().Split(' ');

                h = Convert.ToByte(split[0]);
                w = Convert.ToByte(split[1]);
                g = Convert.ToByte(split[2]);
                r = Convert.ToByte(split[3]);
                score = Convert.ToInt32(split[4]);

                Module module = new Module(character, h, w, g, r, score, null);

                for (i = 0; i < h; i++)
                {
                    tmp = sr.ReadLine();
                    for (j = 0; j < w; j++)
                    {
                        module.matrix[j, i] = Convert.ToByte(tmp[j] - '0');
                    }
                }

                return module;
            }
            catch (Exception ex)
            {
                throw new Exception("read_Template_From_Text_To_Memory:" + ex.Message);
            }
        }

        /// <summary>
        /// Using segmented templates to draw fixed number of red and green points.
        /// </summary>
        /// <param name="inpath"></param>
        /// <param name="outpath"></param>
        /// <param name="fileType"></param>
        public static void generate_Template_From_Img(string inpath, string outpath, FileType fileType)
        {
            try
            {
                int i = 0;
                string ninpath = "";
                string noutpath = "";
                Bitmap source = null;
                foreach (char c in cset)
                {
                    for (i = 0; i < (1 << 10); i++)
                    {
                        ninpath = string.Format("{0}{1}/0({2}).{3}", inpath, c, i, fileType);
                        noutpath = string.Format("{0}{1}/0({2}).{3}", outpath, c, i, fileType);

                        if (!File.Exists(ninpath)) { break; }

                        source = new Bitmap(ninpath);
                        draw_Points_In_Template(source);
                        source.Save(noutpath, ImageFormat.Bmp);
                    }
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Draw green and red points in the template.
        /// </summary>
        /// <param name="source"></param>
        public static void draw_Points_In_Template(Bitmap source)
        {
            int rw = 0, rh = 0;
            int w = source.Width;
            int h = source.Height;
            int green = Config.Green_Point_Amount;
            int red = Config.Red_Point_Amount;
            Color tmp = new Color();
            Color cGreen = Color.FromArgb(0, 255, 0);
            Color cRed = Color.FromArgb(255, 0, 0);
            Random random = new Random();

            while (green > 0 || red > 0)
            {
                rw = random.Next(0, w - 1);
                rh = random.Next(0, h - 1);

                tmp = source.GetPixel(rw, rh);
                if (tmp.R + tmp.G + tmp.B == 0)
                {
                    // black
                    if (green > 0)
                    {
                        green--;
                        source.SetPixel(rw, rh, cGreen);
                    }
                }
                else
                {
                    // white
                    if (red > 0)
                    {
                        red--;
                        source.SetPixel(rw, rh, cRed);
                    }
                }
            }
        }
    }
}