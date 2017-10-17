using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CST238Assign4
{
    public class Road : GameObject
    {
        public Road(PointF location, SizeF extents, SizeF hitExtents) : base(location, extents, hitExtents, new PointF(0f,0f))
        {

        }

        public bool OnRoad(GameObject obj)
        {
            //return true if player is on road
            return true;
        }
    }
}
