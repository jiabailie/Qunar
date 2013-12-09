using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qunar
{
    /// <summary>
    /// One object which represents one character's one template
    /// </summary>
    public class Module
    {
        /// <summary>
        /// Module content.
        /// </summary>
        public byte[,] matrix;

        /// <summary>
        /// Which character this module represents.
        /// </summary>
        public char character { get; set; }

        /// <summary>
        /// The module's height.
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// The module's width.
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// How many green points in this module.
        /// </summary>
        public int green { get; set; }

        /// <summary>
        /// How many red points in this module.
        /// </summary>
        public int red { get; set; }

        /// <summary>
        /// The total score this template has.
        /// </summary>
        public int score { get; set; }

        /// <summary>
        /// Initialize the parameters of this object.
        /// </summary>
        /// <param name="_character"></param>
        /// <param name="_height"></param>
        /// <param name="_width"></param>
        /// <param name="_green"></param>
        /// <param name="_red"></param>
        /// <param name="_score"></param>
        /// <param name="_matrix"></param>
        private void iniParameter(char _character, int _height, int _width, int _green, int _red, int _score, byte[,] _matrix)
        {
            character = _character;
            height = _height;
            width = _width;
            green = _green;
            red = _red;
            score = _score;
            matrix = new byte[_width, _height];
            if (_matrix != null)
            {
                for (int i = 0; i < _width; i++)
                {
                    for (int j = 0; j < _height; j++)
                    {
                        matrix[i, j] = _matrix[i, j];
                    }
                }
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Module()
        {
            iniParameter('-', 0, 0, 0, 0, 0, null);
        }

        /// <summary>
        /// Full parameters' constructor.
        /// </summary>
        /// <param name="_character"></param>
        /// <param name="_height"></param>
        /// <param name="_width"></param>
        /// <param name="_green"></param>
        /// <param name="_red"></param>
        /// <param name="_matrix"></param>
        public Module(char _character, int _height, int _width, int _green, int _red, int _score, byte[,] _matrix)
        {
            iniParameter(_character, _height, _width, _green, _red, _score, _matrix);
        }

        /// <summary>
        /// Object input constructor.
        /// </summary>
        /// <param name="_module"></param>
        public Module(Module _module)
        {
            iniParameter(_module.character, _module.height, _module.width, _module.green, _module.red, _module.score, _module.matrix);
        }
    }
}
