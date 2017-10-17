using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CST238Assign4
{
    public class RiderPlayer : GameObject
    {
        public bool alive { get; set; }

        public RiderPlayer(PointF location, SizeF extents, SizeF hitExtents, PointF velocity) : base(location, extents, hitExtents, velocity)
        {
            this.alive = true;
        }
    }
}
