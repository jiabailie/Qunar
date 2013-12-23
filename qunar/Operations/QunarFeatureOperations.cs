#define Paint_Green
#define Paint_Yellow

//#undef  Paint_Green
//#undef  Paint_Yellow

#if Paint_Green
#define Remove_Green_part
#undef  Remove_Green_part
#endif

#if Paint_Yellow
#define Paint_Yellow_To_Black
#undef  Paint_Yellow_To_Black
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace qunar
{
    /// <summary>
    /// Unique operations in this project.
    /// </summary>
    public class QunarFeatureOperations
    {
        /// <summary>
        /// Find long and connected black lines.
        /// right - left : (width - 1, 0, -1, source)
        /// left - right : (0, width - 1, 1, source)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="inc"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static iLine Find_Long_Connected_Lines(int start, int end, int inc, Bitmap source)
        {
            // The line ascend or descend trend
            // which equals to left - right
            // == 0 level
            // > 0 ascend
            // < 0 descend
            int trend = 0;
            int i = 0, j = 0;
            int pre_hs = -1, pre_he = -1;
            int las_hs = -1, las_he = -1;
            int s = -1, hs = -1, he = -1;
            iLine iline = new iLine();

            try
            {
                // From right to left, to find the start position
                // Find the first vertical black line, from right to left or in the reverse direction.
                for (i = start; i != end && hs == -1 && he == -1; i += inc)
                {
                    for (j = 0; j <= source.Height - 1; j++)
                    {
                        Color tmp = source.GetPixel(i, j);
                        if (tmp.R + tmp.G + tmp.B == 0)
                        {
                            // if this vertical single line is the rightmost single black line
                            if (s == -1) { s = i; }

                            // if this point is the topest point of this single black line
                            if (hs == -1) { hs = j; }
                        }
                        else
                        {
                            // if this point is the bottomest point of this single black line
                            if (hs != -1 && he == -1) { he = j - 1; break; }
                        }
                    }
                }

                pre_hs = las_hs = hs;
                pre_he = las_he = he;

                iline.Add(true, s, hs, he);

                hs = he = -1;

                Operations.find_Vertical_Black_Line_Segment(s + inc, ref hs, ref he, trend, las_hs, las_he, source);

                las_hs = hs;
                las_he = he;

                trend = (las_hs + (las_he - las_hs) / 2) - (pre_hs + (pre_he - pre_hs) / 2);

                iline.Add(true, s + inc, hs, he);

                hs = he = -1;

                for (i = s + 2 * inc; i != end; i += inc)
                {
                    Operations.find_Vertical_Black_Line_Segment(i, ref hs, ref he, trend, las_hs, las_he, source);

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

                Deal_With_ILines(iline, source);

                return iline;
            }
            catch (Exception e)
            {
                throw new Exception("Find_Long_Connected_Lines:" + e.Message);
            }
        }

        /// <summary>
        /// Deal with 1-width line operation
        /// </summary>
        /// <param name="iline"></param>
        /// <param name="source"></param>
        public static void Deal_With_ILines(iLine iline, Bitmap source)
        {
            int i = 0;
#if Paint_Green
#if Remove_Green_part
                Color green = Color.FromArgb(255, 255, 255);
#else
            Color green = Color.FromArgb(0, 255, 0);
#endif
            foreach (oneWidthLine oline in iline.OneLineSet)
            {
                for (i = oline.dStart; i <= oline.dEnd; i++)
                {
                    source.SetPixel(oline.sPos, i, green);
                }
            }
#endif

#if Paint_Yellow

#if Paint_Yellow_To_Black
                Color yellow = Color.FromArgb(0, 0, 0);
#else
            Color yellow = Color.FromArgb(255, 255, 0);
#endif
            Color up = new Color();
            Color down = new Color();
            bool fup = false;
            bool fdo = false;

            try
            {
                foreach (oneWidthLine oline in iline.OneLineSet)
                {
                    fup = false;
                    fdo = false;

                    if (oline.dStart - 1 >= 0)
                    {
                        up = source.GetPixel(oline.sPos, oline.dStart - 1);
                        if (up.R + up.G + up.B == 0)
                        {
                            fup = true;
                        }
                    }
                    if (oline.dEnd + 1 < source.Height)
                    {
                        down = source.GetPixel(oline.sPos, oline.dEnd + 1);
                        if (down.R + down.G + down.B == 0)
                        {
                            fdo = true;
                        }
                    }
                    if (fup || fdo)
                    {
                        for (i = oline.dStart; i <= oline.dEnd; i++)
                        {
                            source.SetPixel(oline.sPos, i, yellow);
                        }
                    }
                }
#endif
            }
            catch (Exception e)
            {
                throw new Exception("Deal_With_ILines:" + e.Message);
            }
        }
    }
}
