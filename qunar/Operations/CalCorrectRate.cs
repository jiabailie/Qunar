#define WATCH_FULL_MATCH_STRING
//#undef  WATCH_FULL_MATCH_STRING
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace qunar
{
    public class CalCorrectRate
    {
        /// <summary>
        /// Calculate the correct rate.
        /// </summary>
        /// <returns></returns>
        public static double calculate_Correct_Rate()
        {
            int i = 0;
            int lcs = 0;
            double rate = 0.0;
            double totalLen = 0.0;
            double correctLen = 0.0;
            List<string> result = IO.read_Text_To_Memory(Config.Result_Save_Path, FileType.txt);
            List<string> correct = IO.read_Text_To_Memory(Config.Correct_Save_Path, FileType.txt);

            for (i = 0; i < result.Count; i++)
            {
                totalLen += correct[i].Length;

#if WATCH_FULL_MATCH_STRING
                if (correct[i] == result[i])
                {
                    Console.WriteLine(string.Format("{0} {1}", i, correct[i]));
                }
#endif
                correctLen += LCS(correct[i], result[i]);
            }
            rate = correctLen / totalLen;
            return rate;
        }

        /// <summary>
        /// Get the longest common sequence of a and b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int LCS(string a, string b)
        {
            int n = a.Length;
            int m = b.Length;
            int i = 0, j = 0;
            int[,] dp = new int[n + 1, m + 1];

            for (i = 0; i < n; i++)
            {
                for (j = 0; j < m; j++)
                {
                    if (a[i] == b[j])
                    {
                        dp[i + 1, j + 1] = dp[i, j] + 1;
                    }
                    else
                    {
                        dp[i + 1, j + 1] = Math.Max(dp[i + 1, j], dp[i, j + 1]);
                    }
                }
            }
            return dp[n, m];
        }
    }
}
