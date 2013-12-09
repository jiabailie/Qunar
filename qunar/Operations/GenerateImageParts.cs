#define SEE_REMOVE_BLANK_RESULT
//#undef  SEE_REMOVE_BLANK_RESULT
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
    /// <summary>
    /// The main branch to receive a image path and generate its' parts.
    /// </summary>
    public class GenerateImageParts
    {
        public static List<Bitmap> generateImageParts(string inpath)
        {
            Bitmap source = Operations.ConvertJpg2Bmp(inpath);
            Operations.UniformizationBmp(source);
            List<Bitmap> imageParts = Operations.FindConnectedComponents(source);
            return imageParts;
        }

        public static Bitmap removeRedundantBlanks(Bitmap source)
        {         
            int iw = 0, ih = 0;
            int w = 0, h = 0;
            Color color = new Color();
            Bitmap desti = null;            

            int lt_h = int.MaxValue, lt_w = int.MaxValue;
            int rb_h = int.MinValue, rb_w = int.MinValue;

            // Do uniformization operation
            Operations.UniformizationBmp(source);

            // Remove black edges
            Operations.generate_White_Edges(source);

            // Remove vertical lines which has less than Config.VERTICAL_THRESHOLD pixels.
            Operations.Remove_Thin_Vertical_Lines(source);

            for (iw = 0; iw < source.Width; iw++)
            {
                for (ih = 0; ih < source.Height; ih++)
                {
                    color = source.GetPixel(iw, ih);

                    if (color.R + color.G + color.B == 0)
                    {
                        if (iw < lt_w) { lt_w = iw; }
                        if (iw > rb_w) { rb_w = iw; }
                        if (ih < lt_h) { lt_h = ih; }
                        if (ih > rb_h) { rb_h = ih; }
                    }
                }
            }

            w = rb_w - lt_w + 1;
            h = rb_h - lt_h + 1;

            desti = new Bitmap(w, h);

            for (iw = 0; iw < w; iw++)
            {
                for (ih = 0; ih < h; ih++)
                {
                    color = source.GetPixel(lt_w + iw, lt_h + ih);
                    desti.SetPixel(iw, ih, color);
                }
            }

#if SEE_REMOVE_BLANK_RESULT
            desti.Save("../../waitting/test.bmp", ImageFormat.Bmp);
            Console.WriteLine("Saved.\n");
#endif
            return desti;
        }
    }
}
