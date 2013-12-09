using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qunar
{
    /// <summary>
    /// An object which represents one line which width is 1.
    /// The line color is black.
    /// </summary>
    public class oneWidthLine
    {
        /// <summary>
        /// Line direction
        /// True: Vertical
        /// False: Horizontal
        /// </summary>
        public bool directionType { set; get; }

        /// <summary>
        /// If directionType then sPos is the horizontal position
        /// If !directionType then sPos is the vertical position
        /// </summary>
        public int sPos { set; get; }

        /// <summary>
        /// If directionType then dStart is the start position of vertical
        /// If !directionType then dStart is the start position of horizontal
        /// </summary>
        public int dStart { set; get; }

        /// <summary>
        /// If directionType then dStart is the end position of vertical
        /// If !directionType then dStart is the end position of horizontal
        /// </summary>
        public int dEnd { set; get; }

        /// <summary>
        /// Common initialize function.
        /// </summary>
        /// <param name="_directionType"></param>
        /// <param name="_sPos"></param>
        /// <param name="_dStart"></param>
        /// <param name="_dEnd"></param>
        public void iniParameter(bool _directionType, int _sPos, int _dStart, int _dEnd)
        {
            this.directionType = _directionType;
            this.sPos = _sPos;
            this.dStart = _dStart;
            this.dEnd = _dEnd;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public oneWidthLine()
        {
            directionType = true;
            sPos = 0;
            dStart = 0;
            dEnd = 0;
        }

        /// <summary>
        /// Full parameters constructor.
        /// </summary>
        /// <param name="_directionType"></param>
        /// <param name="_sPos"></param>
        /// <param name="_dStart"></param>
        /// <param name="_dEnd"></param>
        public oneWidthLine(bool _directionType, int _sPos, int _dStart, int _dEnd)
        {
            iniParameter(_directionType, _sPos, _dStart, _dEnd);
        }

        /// <summary>
        /// Using an object to initialize the object.
        /// </summary>
        /// <param name="_onewidthline"></param>
        public oneWidthLine(oneWidthLine _onewidthline)
        {
            iniParameter(_onewidthline.directionType,
                _onewidthline.sPos,
                _onewidthline.dStart,
                _onewidthline.dEnd);
        }
    }
}
