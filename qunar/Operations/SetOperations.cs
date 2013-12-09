using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace qunar
{
    /// <summary>
    /// Sets related operations
    /// </summary>
    public class SetOperations
    {
        /// <summary>
        /// Judge if two sets have intersection
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="e1"></param>
        /// <param name="s2"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static bool If_Two_Sets_Intersection(int s1, int e1, int s2, int e2)
        {
            bool ret = false;
            int i = 0;

            for (i = s1; i <= e1; i++)
            {
                if (i >= s2 && i <= e2)
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Get two sets' intersection
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="e1"></param>
        /// <param name="s2"></param>
        /// <param name="e2"></param>
        /// <param name="rs"></param>
        /// <param name="re"></param>
        public static void Get_Sets_Intersection(int s1, int e1, int s2, int e2, ref int rs, ref int re)
        {
            rs = -1;
            re = -1;
            int i = 0;

            for (i = s1; i <= e1; i++)
            {
                if (s2 <= i && i <= e2)
                {
                    if (rs == -1)
                    {
                        rs = i;
                    }
                }
                else
                {
                    if (rs != -1)
                    {
                        re = i - 1;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Transform an image into a matrix
        /// black(0,0,0) and yellow(255,255,0) to 1
        /// the others to 0
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[,] Transform_Image_To_Matrix(Bitmap source)
        {
            int iw = 0, ih = 0;
            Color color = new Color();
            byte[,] ret = new byte[source.Width, source.Height];

            for (iw = 0; iw < source.Width; iw++)
            {
                for (ih = 0; ih < source.Height; ih++)
                {
                    color = source.GetPixel(iw, ih);
                    if ((color.R + color.G + color.B == 0) ||            // black
                        ((color.R & color.G) == 255 && color.B == 0))    // yellow
                    {
                        ret[iw, ih] = 1;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Remove the blank regions of a matrix
        /// Top, left, bottom, right.
        /// </summary>
        /// <param name="nw"></param>
        /// <param name="nh"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static byte[,] Remove_Matrix_Blank_Regions(ref int nw, ref int nh,int width, int height, byte[,] matrix)
        {
            int i = 0, j = 0;
            int lt_w = int.MaxValue, lt_h = int.MaxValue;
            int rb_w = int.MinValue, rb_h = int.MinValue;

            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        if (i < lt_w) { lt_w = i; }
                        if (i > rb_w) { rb_w = i; }
                        if (j < lt_h) { lt_h = j; }
                        if (j > rb_h) { rb_h = j; }
                    }
                }
            }

            nw = rb_w - lt_w + 1;
            nh = rb_h - lt_h + 1;

            byte[,] ret = new byte[nw, nh];
            for (i = 0; i < nw; i++)
            {
                for (j = 0; j < nh; j++)
                {
                    ret[i, j] = matrix[lt_w + i, lt_h + j];
                }
            }

            return ret;
        }
    }
}
