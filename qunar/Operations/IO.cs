using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace qunar
{
    public class IO
    {
        /// <summary>
        /// Save source bitmap image as text which contains all positions' pixel value
        /// random position value = {R, G, B}
        /// </summary>
        /// <param name="source"></param>
        /// <param name="outpath"></param>
        public static void write_Bmp_To_RGB_Number(Bitmap source, string outpath)
        {
            try
            {
                int i = 0, j = 0;
                StreamWriter sw = new StreamWriter(outpath);

                for (j = 0; j < source.Height; j++)
                {
                    for (i = 0; i < source.Width; i++)
                    {

                        sw.Write("({0},{1},{2}) ",
                            source.GetPixel(i, j).R.ToString().PadLeft(3, '0'),
                            source.GetPixel(i, j).G.ToString().PadLeft(3, '0'),
                            source.GetPixel(i, j).B.ToString().PadLeft(3, '0'));
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
        /// random position value = {R, G, B} / 3
        /// </summary>
        /// <param name="inpath"></param>
        /// <param name="outpath"></param>
        public static void write_Bmp_To_Avg_Number(string inpath, string outpath)
        {
            try
            {
                Bitmap source = Operations.ConvertJpg2Bmp(inpath);
                int i = 0, j = 0;
                StreamWriter sw = new StreamWriter(outpath);

                int[,] avgImg = new int[source.Height, source.Width];

                for (i = 0; i < source.Height; i++)
                {
                    for (j = 0; j < source.Width; j++)
                    {
                        avgImg[i, j] = (source.GetPixel(j, i).R + source.GetPixel(j, i).G + source.GetPixel(j, i).B) / 3;
                    }
                }

                for (i = 0; i < source.Height; i++)
                {
                    for (j = 0; j < source.Width; j++)
                    {
                        sw.Write("{0} ", avgImg[i, j].ToString().PadLeft(3, '0'));
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
        /// Write bmp image into text files.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="outpath"></param>
        public static void write_Bmp_To_Avg_Number(Bitmap source, string outpath)
        {
            try
            {
                int i = 0, j = 0, max = 0;
                StreamWriter sw = new StreamWriter(outpath);

                int[,] avgImg = new int[source.Height, source.Width];
                int[] cnt = new int[source.Width];

                for (i = 0; i < source.Height; i++)
                {
                    for (j = 0; j < source.Width; j++)
                    {
                        avgImg[i, j] = (source.GetPixel(j, i).R + source.GetPixel(j, i).G + source.GetPixel(j, i).B) / 3;
                    }
                }

                for (i = 0; i < source.Height; i++)
                {
                    for (j = 0; j < source.Width; j++)
                    {
                        if (avgImg[i, j] == 0)
                        {
                            sw.Write("  0");
                            cnt[j]++;
                        }
                        else
                        {
                            sw.Write("   ");
                        }
                    }
                    sw.WriteLine();
                }
                foreach (int x in cnt)
                {
                    if (x > max) { max = x; }
                    sw.Write(x.ToString().PadLeft(3, ' '));
                }
                sw.WriteLine();

                for (i = 0; i < source.Width; i++)
                {
                    sw.Write(i.ToString().PadLeft(3, ' '));
                }
                sw.WriteLine();

                for (i = 0; i < max; i++)
                {
                    for (j = 0; j < source.Width; j++)
                    {
                        if (cnt[j] > 0)
                        {
                            sw.Write("---");
                        }
                        else
                        {
                            sw.Write("   ");
                        }
                        cnt[j]--;
                    }
                    sw.WriteLine();
                }
                sw.WriteLine();
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Write matrix into text files as numbers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="matrix"></param>
        /// <param name="outpath"></param>
        public static void write_Matrix_To_Txt<T>(int w, int h, T[,] matrix, string outpath) where T : IComparable<T>
        {
            try
            {
                int i = 0, j = 0;
                StreamWriter sw = new StreamWriter(outpath);

                for (i = 0; i < h; i++)
                {
                    for (j = 0; j < w; j++)
                    {
                        if (matrix[j, i].ToString() != "0")
                        {
                            sw.Write(matrix[j, i].ToString());
                        }
                        else
                        {
                            sw.Write(" ");
                        }
                    }
                    sw.WriteLine();
                }
                sw.WriteLine();

                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
