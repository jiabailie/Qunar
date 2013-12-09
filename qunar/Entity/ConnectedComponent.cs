using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qunar
{
    /// <summary>
    /// One object represents one connected-component which is composed by two points (left_top, right_bottom)
    /// </summary>
    public class ConnectedComponent
    {
        /// <summary>
        /// The left_top point.
        /// </summary>
        private iPoint LEFT_TOP;

        /// <summary>
        /// The right_bottom point.
        /// </summary>
        private iPoint RIGHT_BOTTOM;

        /// <summary>
        /// The point position set.
        /// </summary>
        private List<iPoint> POINT_SET;

        /// <summary>
        /// Initialize the parameters of this object.
        /// </summary>
        /// <param name="_w"></param>
        /// <param name="_h"></param>
        private void iniParameter(iPoint _lt, iPoint _rb, List<iPoint> _pointSet)
        {
            LEFT_TOP = new iPoint(_lt.Width_Position, _lt.Height_Position);
            RIGHT_BOTTOM = new iPoint(_rb.Width_Position, _rb.Height_Position);
            POINT_SET = new List<iPoint>();
            foreach (iPoint point in _pointSet)
            {
                POINT_SET.Add(new iPoint(point.Width_Position, point.Height_Position));
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ConnectedComponent()
        {
            iniParameter(new iPoint(), new iPoint(), new List<iPoint>());
        }

        public ConnectedComponent(iPoint _lt, iPoint _rb, List<iPoint> _pointSet)
        {
            iniParameter(_lt, _rb, _pointSet);
        }

        public ConnectedComponent(ConnectedComponent _cc)
        {
            iniParameter(_cc.Left_Top, _cc.Right_Bottom, _cc.PointSet);
        }

        public iPoint Left_Top
        {
            get { return LEFT_TOP; }
            set { this.LEFT_TOP = new iPoint(value); }
        }

        public iPoint Right_Bottom
        {
            get { return RIGHT_BOTTOM; }
            set { this.RIGHT_BOTTOM = new iPoint(value); }
        }

        public List<iPoint> PointSet
        {
            get
            {
                return POINT_SET;
            }
            set
            {
                this.POINT_SET.Clear();
                foreach (iPoint point in value)
                {
                    this.POINT_SET.Add(point);
                }
            }
        }
    }
}
