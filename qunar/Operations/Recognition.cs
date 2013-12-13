#define WATCH_PREPROCESS_RESULT
#undef  WATCH_PREPROCESS_RESULT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace qunar
{
    public class Recognition<T>
    {
        /// <summary>
        /// Do image recognition.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="matrix"></param>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static string Do_Image_Recognition(int width, int height, T[,] matrix, List<Module> modules)
        {
            char tmp = '\0';
            string ret = "";
            int verticalPosition = 0;

            try
            {
                while (verticalPosition < width)
                {
                    tmp = Get_Most_Match_Character(ref verticalPosition, width, height, matrix, modules);
                    if (tmp != '\0')
                    {
                        ret += tmp;
                    }
#if WATCH_PREPROCESS_RESULT
                    Console.WriteLine(ret);
#endif
                }

                return ret;
            }
            catch (Exception e)
            {
                throw new Exception("Do_Image_Recognition:" + e.Message);
            }
        }

        /// <summary>
        /// From vertical position to find the most matching template.
        /// </summary>
        /// <param name="verticalPosition"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="matrix"></param>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static char Get_Most_Match_Character(ref int verticalPosition, int width, int height, T[,] matrix, List<Module> modules)
        {
            int i = 0, j = 0;
            int tmpScore = 0;
            int maxWidth = 0;
            double maxPercent = double.MinValue;
            double tmpPercent = 0.0;
            char maxCharacter = '\0';
            verticalPosition = Math.Max(0, verticalPosition - Config.Backward_Distance);

            try
            {
                while (maxCharacter == '\0' && verticalPosition < width)
                {
                    for (i = 0; i < height; i++)
                    {
                        for (j = 0; j < modules.Count; j++)
                        {
                            if (i + modules[j].height - 1 >= height || verticalPosition + modules[j].width - 1 >= width)
                            {
                                continue;
                            }
                            tmpScore = Calculate_Single_Score_Block_Template(verticalPosition, i, matrix, modules[j]);
                            tmpPercent = tmpScore * 1.0 / modules[j].score;
                            if (tmpPercent > maxPercent)
                            {
                                maxWidth = modules[j].width;
                                maxPercent = tmpPercent;
                                maxCharacter = modules[j].character;
                            }
                        }
                    }

                    if (maxPercent - Config.Match_Least_Score_Percent < 1e-9)
                    {
                        maxCharacter = '\0';
                    }
                    if (maxCharacter != '\0')
                    {
#if WATCH_PREPROCESS_RESULT
                        Console.WriteLine("{0} {1}", verticalPosition, maxWidth);
#endif
                        verticalPosition = verticalPosition + maxWidth - 1;
                        break;
                    }
                    verticalPosition++;
                }
                return maxCharacter;
            }
            catch (Exception e)
            {
                throw new Exception("Get_Most_Match_Character:" + e.Message);
            }
        }

        /// <summary>
        /// Calculate the score for an image block (represents by a matrix) and a template.
        /// Principle: blank is more important than black dots.
        /// if matrix is 1 and template is 1(green), adds 3.
        /// if matrix is 0 and template is 2(red), adds 7.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startWidth"></param>
        /// <param name="startHeight"></param>
        /// <param name="matrix"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public static int Calculate_Single_Score_Block_Template(int startWidth, int startHeight, T[,] matrix, Module module)
        {
            int score = 0;
            int i = 0, j = 0;

            try
            {
                for (i = 0; i < module.width; i++)
                {
                    for (j = 0; j < module.height; j++)
                    {
                        if (module.matrix[i, j] == 1 && matrix[startWidth + i, startHeight + j].ToString() == "1")
                        {
                            score += Config.Green_Score;
                        }
                        else if (module.matrix[i, j] == 2 && matrix[startWidth + i, startHeight + j].ToString() == "0")
                        {
                            score += Config.Red_Score;
                        }
                    }
                }

                return score;
            }
            catch (Exception e)
            {
                throw new Exception("Calculate_Single_Score_Block_Template:" + e.Message);
            }
        }
    }
}