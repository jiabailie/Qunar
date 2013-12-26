using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace qunar
{
    public class Optimize
    {
        /// <summary>
        /// Remove images' black edges.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="matrix"></param>
        public static void Generate_White_Edges(int w, int h, ref byte[,] matrix)
        {
            int i = 0;
            for (i = 0; i < w; i++)
            {
                if (matrix[i, 1] == 0)
                {
                    matrix[i, 0] = 0;
                }
                if (matrix[i, h - 2] == 0)
                {
                    matrix[i, h - 1] = 0;
                }
            }
            for (i = 0; i < h; i++)
            {
                if (matrix[1, i] == 0)
                {
                    matrix[0, i] = 0;
                }
                if (matrix[w - 2, i] == 0)
                {
                    matrix[w - 1, i] = 0;
                }
            }
        }

        /// <summary>
        /// Fill all 1-width white blanks with black.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="matrix"></param>
        public static void Fill_One_Width_Blanks(int w, int h, ref byte[,] matrix)
        {
            int i = 0, j = 0;

            try
            {
                for (i = 0; i < w; i++)
                {
                    for (j = 0; j < h; j++)
                    {
                        if (matrix[i, j] == 0)
                        {
                            if (i - 1 >= 0 && i + 1 < w)
                            {
                                if ((matrix[i - 1, j] & matrix[i + 1, j]) == 1)
                                {
                                    matrix[i, j] = 1;
                                }
                            }
                            if (j - 1 >= 0 && j + 1 < h)
                            {
                                if ((matrix[i, j - 1] & matrix[i, j + 1]) == 1)
                                {
                                    matrix[i, j] = 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Optimize.Fill_One_Width_Blanks:{0}", e.Message));
            }
        }

        /// <summary>
        /// Deal with 1-width line operation
        /// </summary>
        /// <param name="iline"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="matrix"></param>
        public static void Deal_With_ILines(iLine iline, int w, int h, byte[,] matrix)
        {
            int i = 0;

            foreach (oneWidthLine oline in iline.OneLineSet)
            {
                for (i = oline.dStart; i <= oline.dEnd; i++)
                {
                    matrix[oline.sPos, i] = 0;
                }
            }

            byte up = 0, down = 0;
            bool fup = false, fdo = false;

            foreach (oneWidthLine oline in iline.OneLineSet)
            {
                fup = false;
                fdo = false;

                if (oline.dStart - 1 >= 0)
                {
                    up = matrix[oline.sPos, oline.dStart - 1];
                    if (up == 1)
                    {
                        fup = true;
                    }
                }
                if (oline.dEnd + 1 < h)
                {
                    down = matrix[oline.sPos, oline.dEnd + 1];
                    if (down == 1)
                    {
                        fdo = true;
                    }
                }
                if (fup || fdo)
                {
                    for (i = oline.dStart; i <= oline.dEnd; i++)
                    {
                        matrix[oline.sPos, i] = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Find black vertical consecutive lines
        /// </summary>
        /// <param name="vertical"></param>
        /// <param name="hs"></param>
        /// <param name="he"></param>
        /// <param name="trend"></param>
        /// <param name="las_hs"></param>
        /// <param name="las_he"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="matrix"></param>
        public static void Find_Vertical_Black_Line_Segment(int vertical, ref int hs, ref int he, int trend, int las_hs, int las_he, int w, int h, byte[,] matrix)
        {
            if (vertical < 0 || vertical >= w) { return; }
            int i = 0;
            bool sfind = false;
            int tmpDiff = 0, minDiff = int.MaxValue;
            int center = las_hs + (las_he - las_hs) / 2;
            List<int> lhs = new List<int>();
            List<int> lhe = new List<int>();

            for (i = 0; i < h; i++)
            {
                if (matrix[vertical, i] == 1)
                {
                    if (!sfind)
                    {
                        sfind = true;
                        lhs.Add(i);
                    }
                }
                else
                {
                    if (sfind)
                    {
                        sfind = false;
                        lhe.Add(i - 1);
                    }
                }
            }

            if (lhs.Count == 0) { return; }

            if (lhs.Count == 1)
            {
                hs = lhs[0];
                he = lhe[0];
            }

            for (i = 0; i < lhs.Count; i++)
            {
                tmpDiff = Math.Abs(lhs[i] + lhe[i] - las_hs - las_he);
                if (tmpDiff < minDiff)
                {
                    minDiff = tmpDiff;
                    hs = lhs[i];
                    he = lhe[i];

                    if (lhe[i] - lhs[i] >= 5)
                    {
                        hs = las_hs;
                        he = Math.Min(las_hs + 3, h - 1);
                    }
                }
            }
        }

        /// <summary>
        /// Find long and connected black lines.
        /// right - left : (width - 1, 0, -1, source)
        /// left - right : (0, width - 1, 1, source)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="inc"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static iLine Find_Long_Connected_Lines(int start, int end, int inc, int w, int h, byte[,] matrix)
        {
            int trend = 0;
            int i = 0, j = 0;
            int pre_hs = -1, pre_he = -1;
            int las_hs = -1, las_he = -1;
            int s = -1, hs = -1, he = -1;
            iLine iline = new iLine();

            for (i = start; i != end && hs == -1 && he == -1; i += inc)
            {
                for (j = 0; j <= h - 1; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        if (s == -1) { s = i; }
                        if (hs == -1) { hs = j; }
                    }
                    else
                    {
                        if (hs != -1 && he == -1) { he = j - 1; break; }
                    }
                }
            }

            pre_hs = las_hs = hs;
            pre_he = las_he = he;

            iline.Add(true, s, hs, he);

            hs = he = -1;

            Find_Vertical_Black_Line_Segment(s + inc, ref hs, ref he, trend, las_hs, las_he, w, h, matrix);

            las_hs = hs;
            las_he = he;

            trend = (las_hs + (las_he - las_hs) / 2) - (pre_hs + (pre_he - pre_hs) / 2);

            iline.Add(true, s + inc, hs, he);

            hs = he = -1;

            for (i = s + 2 * inc; i != end; i += inc)
            {
                Find_Vertical_Black_Line_Segment(i, ref hs, ref he, trend, las_hs, las_he, w, h, matrix);

                if (hs == -1 && he == -1)
                {
                    break;
                }

                pre_hs = las_hs;
                pre_he = las_he;
                las_hs = hs;
                las_he = he;
                hs = -1;
                he = -1;
                trend = (las_hs + (las_he - las_hs) / 2) - (pre_hs + (pre_he - pre_hs) / 2);

                iline.Add(true, i, las_hs, las_he);
            }

            Deal_With_ILines(iline, w, h, matrix);

            return iline;
        }
    }
}
