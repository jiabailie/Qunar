using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qunar
{
    /// <summary>
    /// One object represents one position(w,h) in bitmap.
    /// </summary>
    public class iPoint
    {
        /// <summary>
        /// The horizontal position
        /// </summary>
        private int w;

        /// <summary>
        /// The vertical position
        /// </summary>
        private int h;

        /// <summary>
        /// Initialize the parameters of this object.
        /// </summary>
        /// <param name="_w"></param>
        /// <param name="_h"></param>
        private void iniParameter(int _w, int _h)
        {
            this.w = _w;
            this.h = _h;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public iPoint()
        {
            iniParameter(0, 0);
        }

        /// <summary>
        /// Another constructor which receives two parameters to initialize the object.
        /// </summary>
        /// <param name="_w"></param>
        /// <param name="_h"></param>
        public iPoint(int _w, int _h)
        {
            iniParameter(_w, _h);
        }

        public iPoint(iPoint _ipoint)
        {
            iniParameter(_ipoint.Width_Position, _ipoint.Height_Position);
        }

        public int Width_Position
        {
            get { return w; }
            set { this.w = value; }
        }

        public int Height_Position
        {
            get { return h; }
            set { this.h = value; }
        }
    }
}
