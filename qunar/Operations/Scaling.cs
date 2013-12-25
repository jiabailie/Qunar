using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace qunar
{
    public class Scaling
    {
        /* {0, -1, 0, -1, 5, -1, 0, -1, 0} */
        public static void Using_Sharpening_Filters(int step, int[][] kernel, Bitmap source)
        {
            int i = 0, j = 0;
            int m = 0, n = 0;
            Bitmap copy = new Bitmap(source);

            try
            {
                for (i = 1; i < source.Width - 1; i++)
                {
                    for (j = 1; j < source.Height - 1; j++)
                    {
                        int[] rgb = new int[3] { 0, 0, 0 };
                        for (m = -1; m <= 1; m++)
                        {
                            for (n = -1; n <= 1; n++)
                            {
                                Color point = copy.GetPixel(i + m, j + n);
                                rgb[0] += point.R * kernel[m + 1][n + 1];
                                rgb[1] += point.G * kernel[m + 1][n + 1];
                                rgb[2] += point.B * kernel[m + 1][n + 1];
                            }
                        }
                        source.SetPixel(i, j, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Scaling.Using_Sharpening_Filters:{0}", e.Message));
            }
        }

        public static Bitmap Image_Zoom_Out(int ratio, Bitmap source)
        {
            if (ratio == 0) { ratio = 1; }

            int i = 0, j = 0;
            int m = 0, n = 0;

            int divisor = ratio * ratio;
            int nWidth = source.Width / ratio;
            int nHeight = source.Height / ratio;

            Bitmap zoomoutImage = new Bitmap(nWidth, nHeight);

            try
            {
                for (i = 0; i < nWidth; i++)
                {
                    for (j = 0; j < nHeight; j++)
                    {
                        int[] rgb = new int[3] { 0, 0, 0 };
                        for (n = i * ratio; n < (i + 1) * ratio; n++)
                        {
                            for (m = j * ratio; m < (j + 1) * ratio; m++)
                            {
                                Color point = source.GetPixel(n, m);
                                rgb[0] += Convert.ToInt32(point.R);
                                rgb[1] += Convert.ToInt32(point.G);
                                rgb[2] += Convert.ToInt32(point.B);
                            }
                        }

                        rgb[0] /= divisor;
                        rgb[1] /= divisor;
                        rgb[2] /= divisor;

                        zoomoutImage.SetPixel(i, j, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Scaling.Image_Zoom_Out:{0}", e.Message));
            }
            return zoomoutImage;
        }
    }
}
