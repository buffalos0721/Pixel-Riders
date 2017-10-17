using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CST238Assign4
{
    public class GameObject
    {
        private PointF location;
        private SizeF extents;
        private PointF velocity;
        private SizeF hitExtents;

        public GameObject(PointF location, SizeF extents, SizeF hitExtents, PointF velocity)
        {
            this.location = location;
            this.extents = extents;
            this.hitExtents = hitExtents;
            this.velocity = velocity;
        }

        public PointF Location => this.location;
        public SizeF Extents => this.extents;
        public SizeF HitExtents => this.hitExtents;

        public PointF UpperLeft
        {
            get
            {
                return new PointF(location.X - extents.Width/2, location.Y + extents.Height/2);
            }

        }

        public RectangleF BoundingRect
        {
            get
            {
                return new RectangleF(UpperLeft, extents);

            }
        }


        public float Speed
        {
            get { return this.velocity.Y; }
            set { this.velocity.Y = value; }
        }

        public float TurnSpeed
        {
            get { return this.velocity.X; }
            set { velocity.X = value; }

        }

        //public void Move (TimeSpan timeIncrement)
        //{
        //    location.Y += (float)(velocity.X * timeIncrement.TotalSeconds) * -1;
        //    location.X -= (float)(velocity.Y * timeIncrement.TotalSeconds);

        //}

        public void Move(TimeSpan timeIncrement)
        {
            location.X += (float)(velocity.X * timeIncrement.TotalSeconds);
            location.Y += (float)(velocity.Y * timeIncrement.TotalSeconds);

        }

        public bool DetectCollision(GameObject otherObject)
        {
            float dx = Math.Abs((float)(this.location.X - otherObject.location.X));
            float dy = Math.Abs((float)(this.location.Y - otherObject.location.Y));

            return ((dx < ((this.hitExtents.Width + otherObject.hitExtents.Width) / 2f))
            && (dy < ((this.hitExtents.Height + otherObject.hitExtents.Height) / 2f)));
        }

    }
}