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
using System.Drawing.Imaging;

namespace qunar
{
    public class Template
    {
        private static char[] cset = new char[] { 
            '0', '1', '2', '3', '4', '5', '6', '7', '8', 
            '9', 'a', 'b', 'd', 'f', 'g', 'h', 'i', 'j', 
            'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 
            't', 'u', 'v', 'w', 'x', 'y', 'z', 'e', 'c' };

        /// <summary>
        /// Write all template into text files to test if did it correctly.
        /// </summary>
        /// <param name="inpath"></param>
        /// <param name="outpath"></param>
        public static void write_Template_Into_Text_Files(string inpath, string outpath, FileType fileType)
        {
            int i = 0;
            string ninpath = "";
            string noutpath = "";
            string filepath = "";
            Bitmap source = null;

            try
            {
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
            catch (Exception e)
            {
                throw new Exception("write_Template_Into_Text_Files:" + e.Message);
            }
        }

        public static List<Module> read_Templates_To_Memory(string inpath, FileType fileType)
        {
            int i = 0;
            string filepath = "";
            List<Module> modules = new List<Module>();

            try
            {
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
            int i = 0, j = 0;
            int score = 0;
            int[,] matrix = new int[source.Width, source.Height];
            int lt_w = int.MaxValue, lt_h = int.MaxValue;
            int rb_w = int.MinValue, rb_h = int.MinValue;
            StreamWriter sw = new StreamWriter(outpath);

            try
            {
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
            int i = 0, j = 0;
            StreamWriter sw = new StreamWriter(outpath);

            try
            {
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
            int i = 0, j = 0;
            int h = 0, w = 0;
            int g = 0, r = 0;
            int score = 0;
            string tmp = null;
            StreamReader sr = new StreamReader(inpath);

            string[] split = sr.ReadLine().Trim().Split(' ');

            try
            {
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
            int i = 0;
            string ninpath = "";
            string noutpath = "";
            Bitmap source = null;

            try
            {
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
            catch (Exception ex)
            {
                throw new Exception("generate_Template_From_Img:" + ex.Message);
            }
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
            int fail = 0;
            Color tmp = new Color();
            Color cGreen = Color.FromArgb(0, 255, 0);
            Color cRed = Color.FromArgb(255, 0, 0);
            Color cBlack = Color.FromArgb(0, 0, 0);
            Color cWhite = Color.FromArgb(255, 255, 255);
            Random random = new Random();

            while (green > 0 || red > 0)
            {
                rw = random.Next(0, w - 1);
                rh = random.Next(0, h - 1);

                tmp = source.GetPixel(rw, rh);
                if (tmp.R + tmp.G + tmp.B == 0 && green > 0)
                {
                    // black                    
                    draw_Points_In_Template_Random(rw, rh, Config.Draw_Neighbour_Black_Condition, ref green, ref fail, cGreen, cBlack, source);
                }
                else if ((tmp.R & tmp.G & tmp.B) == 255 && red > 0)
                {
                    // white
                    draw_Points_In_Template_Random(rw, rh, Config.Draw_Neighbour_White_Condition, ref red, ref fail, cRed, cWhite, source);
                }

                if (fail == Config.Maximum_Failure_Times)
                {
                    fail = 0;
                    if (green > 0)
                    {
                        green--;
                        draw_Points_In_Template_Sequence(Config.Draw_Neighbour_Black_Condition, cGreen, cBlack, source);
                    }
                    if (red > 0)
                    {
                        red--;
                        draw_Points_In_Template_Sequence(Config.Draw_Neighbour_White_Condition, cRed, cWhite, source);
                    }
                }
            }
        }

        /// <summary>
        /// When fail Maximum_Failure_Times times, finding a paintable point using loop.
        /// </summary>
        /// <param name="neighbourCondition"></param>
        /// <param name="drawColor"></param>
        /// <param name="originColor"></param>
        /// <param name="source"></param>
        public static void draw_Points_In_Template_Sequence(int neighbourCondition, Color drawColor, Color originColor, Bitmap source)
        {
            int i = 0, j = 0;
            bool unpaint = true;
            Color tmp = new Color();

            for (i = 0; i < source.Width && unpaint; i++)
            {
                for (j = 0; j < source.Height && unpaint; j++)
                {
                    tmp = source.GetPixel(i, j);
                    if (tmp.R == originColor.R && tmp.G == originColor.G && tmp.B == originColor.B && judge_Can_Draw_This_Point(i, j, neighbourCondition, drawColor, originColor, source))
                    {
                        source.SetPixel(i, j, drawColor);
                        unpaint = false;
                    }
                }
            }
        }

        /// <summary>
        /// When found a drawable position, judge if can paint this point or not.
        /// </summary>
        /// <param name="rw"></param>
        /// <param name="rh"></param>
        /// <param name="neighbourCondition"></param>
        /// <param name="count"></param>
        /// <param name="fail"></param>
        /// <param name="drawColor"></param>
        /// <param name="originColor"></param>
        /// <param name="source"></param>
        public static void draw_Points_In_Template_Random(int rw, int rh, int neighbourCondition, ref int count, ref int fail, Color drawColor, Color originColor, Bitmap source)
        {
            bool allowDraw = true;
            if (count > 0)
            {
                allowDraw = judge_Can_Draw_This_Point(rw, rh, neighbourCondition, drawColor, originColor, source);
                if (allowDraw)
                {
                    fail = 0;
                    count--;
                    source.SetPixel(rw, rh, drawColor);
                }
                else
                {
                    fail++;
                }
            }
        }

        /// <summary>
        /// Judge if this point can be drawn.
        /// Ensure its four directly neighbour haven't been paint using this color.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="neighbourCondition"></param>
        /// <param name="drawColor"></param>
        /// <param name="originColor"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool judge_Can_Draw_This_Point(int w, int h, int neighbourCondition, Color drawColor, Color originColor, Bitmap source)
        {
            int i = 0;
            bool draw = true;
            int nw = 0, nh = 0;
            int neighbour = 0;
            Color tmp = new Color();
            int[] dw = new int[] { 1, -1, 0, 0, 1, 1, -1, -1 };
            int[] dh = new int[] { 0, 0, 1, -1, 1, -1, 1, -1 };

            for (i = 0; i < 4; i++)
            {
                nw = w + dw[i];
                nh = h + dh[i];
                if (nw >= 0 && nw < source.Width && nh >= 0 && nh < source.Height)
                {
                    tmp = source.GetPixel(nw, nh);
                    if (tmp.R == drawColor.R && tmp.G == drawColor.G && tmp.B == drawColor.B)
                    {
                        draw = false;
                        break;
                    }
                }
            }

            if (draw)
            {
                for (i = 0; i < 8; i++)
                {
                    nw = w + dw[i];
                    nh = h + dh[i];
                    if (nw >= 0 && nw < source.Width && nh >= 0 && nh < source.Height)
                    {
                        tmp = source.GetPixel(nw, nh);
                        if ((tmp.R == originColor.R && tmp.G == originColor.G && tmp.B == originColor.B) || (tmp.R == drawColor.R && tmp.G == drawColor.G && tmp.B == drawColor.B))
                        {
                            neighbour++;
                        }
                    }
                }
                if (neighbour < neighbourCondition)
                {
                    draw = false;
                }
            }

            return draw;
        }

        /// <summary>
        /// Calculate the amount of each template.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static Dictionary<char, int> calculate_Template_Kinds(List<Module> modules)
        {
            Dictionary<char, int> cntModules = new Dictionary<char, int>();

            foreach (Module m in modules)
            {
                if (cntModules.ContainsKey(m.character))
                {
                    cntModules[m.character]++;
                }
                else
                {
                    cntModules.Add(m.character, 1);
                }
            }
            return cntModules;
        }
    }
}