using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CST238Assign4
{
    public class Obstacle : GameObject
    {
        private bool hasScored;
        public Obstacle(PointF location, SizeF extents, SizeF hitExtents, PointF velocity) : base(location, extents, hitExtents, velocity)
        {
            this.hasScored = false;
        }

        public bool HasPassed(GameObject obj)
        {
            // if obj passes this obstacle, we're all doomed. ( <-- I must've been asleep when I wrote this... :/)

            if (((obj.Location.Y - (obj.Extents.Height / 2f)) > (base.Location.Y + (base.Extents.Height / 2f))) && !hasScored)
            {
                hasScored = true;
                return hasScored;
            }
            return false;

        }
    }
}
