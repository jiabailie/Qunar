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
        public static void UsingSharpeningFilters(int[] kernel, Bitmap source)
        {

        }

        public static Bitmap ImageZoomOut(int ratio, Bitmap source)
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
                Operations.generate_White_Edges(source);

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
                throw new Exception(string.Format("Scaling.ImageZoomOut:{0}", e.Message));
            }
            return zoomoutImage;
        }
    }
}
