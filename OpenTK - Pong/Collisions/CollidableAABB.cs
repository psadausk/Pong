using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKPong.Collisions {

    /// <summary>
    /// Collidable Axis Aligned Bounding Box
    /// This is a quad who's two sides are aligned with both the X and Y axis. 
    /// This simplifies things a little bit
    /// </summary>
    public abstract class CollidableAABB : Collidable {
        public Vector2 Origin { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public CollidableAABB(Vector2 Origin, float width, float height) {
            this.Origin = Origin;
            this.Width = width;
            this.Height = height;
            this.Vertices = new Vector2[4];
            this.ReCalcVertices();
        }

        public override bool Collided(ICollidable other) {
            if ( other is CollidableAABB ) {
                var otherAABB = other as CollidableAABB;

                //return (
                //    this.Origin.X < otherAABB.Origin.X + otherAABB.Width &&
                //    otherAABB.Origin.X < this.Origin.X + this.Width &&
                //    this.Origin.Y < otherAABB.Origin.Y - otherAABB.Height &&
                //    otherAABB.Origin.Y < this.Origin.Y - this.Height );


            // (RectA.X1 < RectB.X2 && 
            //  RectA.X2 > RectB.X1 &&
            //  RectA.Y1 < RectB.Y2 && 
            //  RectA.Y2 > RectB.Y1) 
                var c1 = this.Origin.X < otherAABB.Origin.Y + otherAABB.Width;
                var c2 = this.Origin.X + this.Width > otherAABB.Origin.X;
                var c3 = this.Origin.Y > otherAABB.Origin.Y - otherAABB.Height;
                var c4 = this.Origin.Y - this.Height < otherAABB.Origin.Y;
                return (
                    c1 &&
                    c2 &&
                    c3 &&
                    c4
                    );

            }

            return false;
        }

        public void ReCalcVertices() {
            this.Vertices[0] = Origin;
            this.Vertices[1] = new Vector2(Origin.X + this.Width, Origin.Y);
            this.Vertices[2] = new Vector2(Origin.X + this.Width, Origin.Y + this.Height);
            this.Vertices[3] = new Vector2(Origin.X, Origin.Y + this.Height);
        }

        /// <summary>
        /// Returns true if the point lies on or inside the rectangle
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool PointInsideRect(Vector2 p) {
            return
                p.X >= this.Origin.X &&
                p.X <= this.Origin.X + this.Width &&
                p.Y <= this.Origin.Y &&
                p.Y >= this.Origin.Y - this.Height;
 
        }
    }
}
