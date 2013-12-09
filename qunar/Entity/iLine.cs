using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qunar
{
    /// <summary>
    /// An object which represents one line in bitmap.
    /// The line color is black.
    /// </summary>
    public class iLine
    {
        /// <summary>
        /// The left top point.
        /// </summary>
        public iPoint Left_Top { set; get; }

        /// <summary>
        /// The right bottom point.
        /// </summary>
        public iPoint Right_Bottom { set; get; }

        /// <summary>
        /// The single lines' set.
        /// </summary>
        public List<oneWidthLine> OneLineSet { set; get; }

        /// <summary>
        /// Initialize the parameters of this object.
        /// </summary>
        /// <param name="_lt"></param>
        /// <param name="_rb"></param>
        /// <param name="_ols"></param>
        public void iniParameter(iPoint _lt, iPoint _rb, List<oneWidthLine> _ols)
        {
            Left_Top = new iPoint(_lt.Width_Position, _lt.Height_Position);
            Right_Bottom = new iPoint(_rb.Width_Position, _rb.Height_Position);
            OneLineSet = new List<oneWidthLine>();
            _ols.ForEach(o => OneLineSet.Add(o));
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public iLine()
        {
            Left_Top = new iPoint();
            Right_Bottom = new iPoint();
            OneLineSet = new List<oneWidthLine>();
        }

        public iLine(iPoint _lt, iPoint _rb, List<oneWidthLine> _ols)
        {
            iniParameter(_lt, _rb, _ols);
        }

        public iLine(iLine _ll)
        {
            iniParameter(_ll.Left_Top, _ll.Right_Bottom, _ll.OneLineSet);
        }

        public void Add(oneWidthLine _onewidthline)
        {
            OneLineSet.Add(_onewidthline);
        }

        public void Add(bool _directionType, int _sPos, int _dStart, int _dEnd)
        {
            OneLineSet.Add(new oneWidthLine()
            {
                directionType = _directionType,
                sPos = _sPos,
                dStart = _dStart,
                dEnd = _dEnd
            });
        }
    }
}
