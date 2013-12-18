using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qunar
{
    /// <summary>
    /// File type which is ensure the input object is image.
    /// </summary>
    public enum FileType
    {
        jpg,
        bmp,
        txt
    };

    public class Config
    {
        /// <summary>
        /// The threshold sets for distinguishing the background.
        /// </summary>
        private const int THRESHOLD = 100;

        /// <summary>
        /// The largest length of verticel line segment.
        /// </summary>
        private const int LINE_LENGTH_THRESHOLD = 5;

        /// <summary>
        /// The threshold sets for filtering the connected components whose size are smaller than the threshold.
        /// </summary>
        private const int CC_SIZE_THRESHOLD = 1000;

        /// <summary>
        /// The least number of black pixels one vertical line should have.
        /// </summary>
        private const int VERTICAL_THRESHOLD = 6;

        /// <summary>
        /// The largest number of position difference between vertical 1-width lines.
        /// </summary>
        private const int POSITION_THRESHOLD = 1;

        /// <summary>
        /// The largest distance of backward operation.
        /// </summary>
        private const int BACKWARD_DISTANCE = -2;

        /// <summary>
        /// How many green points you would to draw in this template.
        /// </summary>
        private const int GREEN_POINT_AMOUNT = 30;

        /// <summary>
        /// How many red points you would to draw in this template.
        /// </summary>
        private const int RED_POINT_AMOUNT = 15;

        /// <summary>
        /// How many score one green dot has.
        /// </summary>
        private const int GREEN_SCORE = 3;

        /// <summary>
        /// How many score one red dot has.
        /// </summary>
        private const int RED_SCORE = 7;

        /// <summary>
        /// When drawing a green point
        /// The neighbour should satisfy the minimus amount.
        /// </summary>
        private const int DRAW_NEIGHBOUR_BLACK_CONDITION = 7;

        /// <summary>
        /// When drawing a red point
        /// The neighbour should satisfy the minimus amount.
        /// </summary>
        private const int DRAW_NEIGHBOUR_WHITE_CONDITION = 8;

        /// <summary>
        /// If and only if some image get score more than (MATCH_LEAST_SCORE_PERCENT * (total score of a template)),
        /// we consider they are match.
        /// total score of a template = green * 3 + red * 7
        /// </summary>
        private const double MATCH_LEAST_SCORE_PERCENT = 0.8;

        /// <summary>
        /// The maximum failure times of drawing points.
        /// </summary>
        private const int MAXIMUM_FAILURE_TIMES = 20;

        /// <summary>
        /// Using one integer to represent green.
        /// </summary>
        private const byte INT_GREEN = 2;

        /// <summary>
        /// Using one integer to represent yellow.
        /// </summary>
        private const byte INT_YELLOW = 3;

        /// <summary>
        /// Sample files path.
        /// </summary>
        private const string SAMPLE_PATH = "../../../../qunar-file/samples/";

        /// <summary>
        /// Template files path.
        /// </summary>
        private const string TEMPLATE_PATH = "../../../../qunar-file/template/";

        /// <summary>
        /// Binary processed files path.
        /// </summary>
        private const string BINARY_PATH = "../../../../qunar-file/binary/";

        /// <summary>
        /// Where saves the files which flaged the long black lines.
        /// </summary>
        private const string LONG_BLACK_LINE_PATH = "../../../../qunar-file/bmp/";

        /// <summary>
        /// Where saves the 0/1 text format images.
        /// </summary>
        private const string BINARY_BMP_IMAGES_PATH = "../../../../qunar-file/txt/";

        /// <summary>
        /// The diectory where saves the processed templates.
        /// </summary>
        private const string PROCESSED_TEMPLATE_PATH = "../../../../qunar-file/ttxt/";

        /// <summary>
        /// The diectory where saves the test processing middle results.
        /// </summary>
        private const string TEST_PROCESSED_PATH = "../../../../qunar-file/test/";

        /// <summary>
        /// Where saves the raw templates.
        /// </summary>
        private const string RAW_TEMPLATE_PATH = "../../../../qunar-file/rtemplate/";

        /// <summary>
        /// A text file which saves the results.
        /// </summary>
        private const string RESULT_SAVE_PATH = "../../../../qunar-file/result/result.txt";

        /// <summary>
        /// A text file which saves the correct results.
        /// </summary>
        private const string CORRECT_SAVE_PATH = "../../../../qunar-file/result/correct.txt";

        public static int ThresHold
        {
            get { return THRESHOLD; }
        }

        public static int CC_Size_Threshold
        {
            get { return CC_SIZE_THRESHOLD; }
        }

        public static int Vertical_Threshold
        {
            get { return VERTICAL_THRESHOLD; }
        }

        public static int Position_Threshold
        {
            get { return POSITION_THRESHOLD; }
        }

        public static int Line_Length_Threshold
        {
            get { return LINE_LENGTH_THRESHOLD; }
        }

        public static int Backward_Distance
        {
            get { return BACKWARD_DISTANCE; }
        }

        public static int Green_Point_Amount
        {
            get { return GREEN_POINT_AMOUNT; }
        }

        public static int Red_Point_Amount
        {
            get { return RED_POINT_AMOUNT; }
        }

        public static int Green_Score
        {
            get { return GREEN_SCORE; }
        }

        public static int Red_Score
        {
            get { return RED_SCORE; }
        }

        public static int Draw_Neighbour_Black_Condition
        {
            get { return DRAW_NEIGHBOUR_BLACK_CONDITION; }
        }

        public static int Draw_Neighbour_White_Condition
        {
            get { return DRAW_NEIGHBOUR_WHITE_CONDITION; }
        }

        public static double Match_Least_Score_Percent
        {
            get { return MATCH_LEAST_SCORE_PERCENT; }
        }

        public static int Maximum_Failure_Times
        {
            get { return MAXIMUM_FAILURE_TIMES; }
        }

        public static byte Int_Green
        {
            get { return INT_GREEN; }
        }

        public static byte Int_Yellow
        {
            get { return INT_YELLOW; }
        }

        public static string Sample_Path
        {
            get { return SAMPLE_PATH; }
        }

        public static string Template_Path
        {
            get { return TEMPLATE_PATH; }
        }

        public static string Binary_Path
        {
            get { return BINARY_PATH; }
        }

        public static string Long_Black_Line_Path
        {
            get { return LONG_BLACK_LINE_PATH; }
        }

        public static string Binary_Bmp_Images_Path
        {
            get { return BINARY_BMP_IMAGES_PATH; }
        }

        public static string Processed_Template_Path
        {
            get { return PROCESSED_TEMPLATE_PATH; }
        }

        public static string Test_Processed_Path
        {
            get { return TEST_PROCESSED_PATH; }
        }

        public static string Raw_template_Path
        {
            get { return RAW_TEMPLATE_PATH; }
        }

        public static string Result_Save_Path
        {
            get { return RESULT_SAVE_PATH; }
        }

        public static string Correct_Save_Path
        {
            get { return CORRECT_SAVE_PATH; }
        }
    }
}
