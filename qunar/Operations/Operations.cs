#define USING_LINE_CENTER
#undef  USING_LINE_CENTER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace qunar
{
    public class Operations
    {
        /// <summary>
        /// Judge whether the input point is a white point.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static bool Judge_White_Point(Color color)
        {
            bool ret = true;

            if (color.R != 255 || color.G != 255 || color.B != 255)
            {
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// Do uniformization operation for certain image.
        /// </summary>
        /// <param name="source"></param>
        public static void Uniformization_Bmp(Bitmap source)
        {
            int iw = 0, ih = 0;
            int avg = 0;
            for (iw = 0; iw < source.Width; iw++)
            {
                for (ih = 0; ih < source.Height; ih++)
                {
                    Color tmp = source.GetPixel(iw, ih);
                    avg = (tmp.R + tmp.G + tmp.B) / 3;
                    if (avg >= Config.ThresHold) { avg = 255; }
                    else { avg = 0; }
                    tmp = Color.FromArgb(avg, avg, avg);
                    source.SetPixel(iw, ih, tmp);
                }
            }
        }

        /// <summary>
        /// Remove images' black edges.
        /// </summary>
        /// <param name="source"></param>
        public static void Generate_White_Edges(Bitmap source)
        {
            int i = 0;
            Color white = Color.FromArgb(255, 255, 255);
            Color neighbour = new Color();

            try
            {
                for (i = 0; i < source.Width; i++)
                {
                    neighbour = source.GetPixel(i, 1);
                    if ((neighbour.R & neighbour.G & neighbour.B) == 255)
                    {
                        source.SetPixel(i, 0, white);
                    }

                    neighbour = source.GetPixel(i, source.Height - 2);
                    if ((neighbour.R & neighbour.G & neighbour.B) == 255)
                    {
                        source.SetPixel(i, source.Height - 1, white);
                    }
                }
                for (i = 0; i < source.Height; i++)
                {
                    neighbour = source.GetPixel(1, i);
                    if ((neighbour.R & neighbour.G & neighbour.B) == 255)
                    {
                        source.SetPixel(0, i, white);
                    }

                    neighbour = source.GetPixel(source.Width - 2, i);
                    if ((neighbour.R & neighbour.G & neighbour.B) == 255)
                    {
                        source.SetPixel(source.Width - 1, i, white);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("generate_White_Edges:" + e.Message);
            }
        }

        /// <summary>
        /// Using breadth first search to find all white connected components.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="visited"></param>
        /// <param name="ccResult"></param>
        public static void BFS(int width, int height, ref bool[,] visited, ConnectedComponent ccResult, Bitmap source)
        {
            int i = 0;
            int len = 4;
            int tx = 0, ty = 0;

            int[] dirX = new int[] { 0, 1, 0, -1 };
            int[] dirY = new int[] { 1, 0, -1, 0 };

            visited[width, height] = true;

            Queue<iPoint> pointQueue = new Queue<iPoint>();

            try
            {
                pointQueue.Enqueue(new iPoint(width, height));
                while (pointQueue.Count > 0)
                {
                    iPoint tmp = pointQueue.Dequeue();
                    ccResult.PointSet.Add(new iPoint(tmp.Width_Position, tmp.Height_Position));
                    for (i = 0; i < len; i++)
                    {
                        tx = tmp.Width_Position + dirX[i];
                        ty = tmp.Height_Position + dirY[i];
                        if (tx >= 0 && tx < source.Width &&
                            ty >= 0 && ty < source.Height &&
                            !visited[tx, ty] &&
                            Judge_White_Point(source.GetPixel(tx, ty)))
                        {
                            visited[tx, ty] = true;
                            pointQueue.Enqueue(new iPoint(tx, ty));
                        }
                    }
                }
            }
            catch (Exception ecc)
            {
                throw new Exception(ecc.Message);
            }
        }

        /// <summary>
        /// According to the point information to find the left-top and right-bottom point.
        /// </summary>
        /// <param name="currentCC"></param>
        public static void Format_Connected_Component(ConnectedComponent currentCC)
        {
            int minWidth = int.MaxValue;
            int maxWidth = int.MinValue;
            int minHeight = int.MaxValue;
            int maxHeight = int.MinValue;

            foreach (iPoint point in currentCC.PointSet)
            {
                if (point.Width_Position < minWidth)
                {
                    minWidth = point.Width_Position;
                }
                if (point.Width_Position > maxWidth)
                {
                    maxWidth = point.Width_Position;
                }
                if (point.Height_Position < minHeight)
                {
                    minHeight = point.Height_Position;
                }
                if (point.Height_Position > maxHeight)
                {
                    maxHeight = point.Height_Position;
                }
            }

            currentCC.Left_Top = new iPoint(minWidth, minHeight);
            currentCC.Right_Bottom = new iPoint(maxWidth, maxHeight);
        }

        /// <summary>
        /// Remove suspending points, making this kind points as the same of its majority neighbours.
        /// </summary>
        /// <param name="source"></param>
        public static void Remove_Suspending_Points(Bitmap source)
        {
            int i = 0, j = 0, k = 0;
            int black = 0, white = 0;
            Color cblack = Color.FromArgb(0, 0, 0);
            Color cwhite = Color.FromArgb(255, 255, 255);
            int[] dx = new int[8] { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = new int[8] { -1, 0, 1, -1, 1, -1, 0, 1 };

            try
            {
                for (i = 1; i < source.Width - 1; i++)
                {
                    for (j = 1; j < source.Height - 1; j++)
                    {
                        black = 0;
                        white = 0;

                        for (k = 0; k < 8; k++)
                        {
                            if (source.GetPixel(i + dx[k], j + dy[k]).R == 0)
                            {
                                black++;
                            }
                            else
                            {
                                white++;
                            }
                        }

                        if (black > white)
                        {
                            source.SetPixel(i, j, cblack);
                        }
                        else if (black < white)
                        {
                            source.SetPixel(i, j, cwhite);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Remove (paint white) the vertical lines whose black positions are less than VERTICAL_THRESHOLD.
        /// </summary>
        /// <param name="source"></param>
        public static void Remove_Thin_Vertical_Lines(Bitmap source)
        {
            int i = 0, j = 0;
            int cnt = 0;
            Color white = Color.FromArgb(255, 255, 255);

            try
            {
                for (i = 0; i < source.Width; i++)
                {
                    cnt = 0;
                    for (j = 0; j < source.Height; j++)
                    {
                        Color color = source.GetPixel(i, j);

                        // The pixel value is (0,0,0)
                        if (color.R + color.G + color.B == 0)
                        {
                            cnt++;
                        }
                    }
                    if (cnt < Config.Vertical_Threshold)
                    {
                        for (j = 0; j < source.Height; j++)
                        {
                            source.SetPixel(i, j, white);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Remove_Thin_Vertical_Lines:" + ex.Message);
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
        /// <param name="source"></param>
        public static void Find_Vertical_Black_Line_Segment(int vertical, ref int hs, ref int he, int trend, int las_hs, int las_he, Bitmap source)
        {
            if (vertical < 0 || vertical > source.Width) { return; }

            int i = 0;
            bool sfind = false;
            int tmpDiff = 0, minDiff = int.MaxValue;
            int center = las_hs + (las_he - las_hs) / 2;
            List<int> lhs = new List<int>();
            List<int> lhe = new List<int>();

            try
            {
                for (i = 0; i < source.Height; i++)
                {
                    Color tmp = source.GetPixel(vertical, i);
                    if (tmp.R + tmp.G + tmp.B == 0)
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

                // If this vertical line is a pure white line
                if (lhs.Count == 0)
                {
                    return;
                }

                // if this vertical line only has one consecutive black segment
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
                            he = Math.Min(las_hs + 3, source.Height - 1);
                        }
                    }
                }

#if USING_LINE_CENTER
                int plen = 0, nlen = 0;

                //The new consecutive black segment should has common consistent subset.
                for (i = 0; i < lhs.Count; i++)
                {
                    if (SetOperations.If_Two_Sets_Intersection(lhs[i] - Config.Position_Threshold, lhe[i] + Config.Position_Threshold, las_hs, las_he))
                    {
                        hs = lhs[i];
                        he = lhe[i];
                        plen = las_he - las_hs + 1;
                        nlen = he - hs + 1;

                        if (Math.Abs(nlen - plen) >= Config.Vertical_Threshold || nlen >= Config.Line_Length_Threshold)
                        {
                            hs = center - 1;
                            he = center + 1;
                        }
                        break;
                    }
                }
#endif
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Using this method to convert .jpg source image into .bmp format.
        /// </summary>
        public static Bitmap Convert_Jpg2Bmp(string sourcePath)
        {
            Bitmap source = null;
            try
            {
                source = new Bitmap(sourcePath);
            }
            catch (Exception oe)
            {
                Console.WriteLine(oe.Message);
            }
            return source;
        }

        /// <summary>
        /// Write one connected component to certain bmp image.
        /// </summary>
        /// <param name="ccImage"></param>
        /// <param name="sourceImage"></param>
        public static Bitmap Save_Connected_Component_As_Bmp(ConnectedComponent ccImage, Bitmap sourceImage)
        {
            int width = ccImage.Right_Bottom.Width_Position - ccImage.Left_Top.Width_Position;
            int height = ccImage.Right_Bottom.Height_Position - ccImage.Left_Top.Height_Position;

            Bitmap ccBmp = new Bitmap(width + 1, height + 1, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Graphics ccGraphics = Graphics.FromImage(ccBmp);

            // Set the background color as black.
            ccGraphics.Clear(Color.FromArgb(0, 0, 0));

            ccGraphics.Dispose();

            foreach (iPoint point in ccImage.PointSet)
            {
                ccBmp.SetPixel(point.Width_Position - ccImage.Left_Top.Width_Position,
                               point.Height_Position - ccImage.Left_Top.Height_Position,
                               sourceImage.GetPixel(point.Width_Position, point.Height_Position)
                );
            }

            return ccBmp;
        }

        /// <summary>
        /// Find all connected components and write them into files.
        /// </summary>
        public static List<Bitmap> Find_Connected_Components(Bitmap source)
        {
            List<Bitmap> imageParts = new List<Bitmap>();
            List<ConnectedComponent> ccImage = Find_All_Connected_Components(source);

            try
            {
                foreach (ConnectedComponent cc in ccImage)
                {
                    imageParts.Add(Save_Connected_Component_As_Bmp(cc, source));
                }
            }
            catch (Exception oe)
            {
                Console.WriteLine(oe.Message);
            }
            return imageParts;
        }

        /// <summary>
        /// Find all connected-components in the image and return related information.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<ConnectedComponent> Find_All_Connected_Components(Bitmap source)
        {
            int i = 0, j = 0;
            int tw = 0, th = 0;

            int width = source.Width;
            int height = source.Height;

            // The array which tells whether the point at position[i,j] has been visited.
            bool[,] visited = new bool[width, height];

            List<ConnectedComponent> ccResult = new List<ConnectedComponent>();

            try
            {
                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        if (Judge_White_Point(source.GetPixel(i, j)) && !visited[i, j])
                        {
                            visited[i, j] = true;

                            ConnectedComponent currentCC = new ConnectedComponent();

                            currentCC.PointSet.Add(new iPoint(i, j));
                            try
                            {
                                BFS(i, j, ref visited, currentCC, source);
                            }
                            catch (Exception ea)
                            {
                                throw new Exception(ea.Message);
                            }

                            Format_Connected_Component(currentCC);

                            tw = currentCC.Right_Bottom.Width_Position - currentCC.Left_Top.Width_Position;
                            th = currentCC.Right_Bottom.Height_Position - currentCC.Left_Top.Height_Position;

                            if (tw < width / 4 && (tw >= Config.CC_Size_Threshold || th >= Config.CC_Size_Threshold))
                            {
                                ccResult.Add(currentCC);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ccResult;
        }
    }
}
