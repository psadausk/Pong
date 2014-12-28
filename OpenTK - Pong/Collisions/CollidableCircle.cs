using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKPong.Collisions {
    public class CollidableCircle : ICollidable {

        public Vector2 Origin { get; set; }
        public float Raduis { get; set; }

        public CollidableCircle(Vector2 origin, float raduis) {
            this.Origin = origin;
            this.Raduis = raduis;
        }

        public bool Collided(ICollidable other) {
            if (other is CollidableCircle) {
                var otherC = other as CollidableCircle;
                var dis = Math.Pow(otherC.Origin.X - this.Origin.X, 2) + Math.Pow(otherC.Origin.Y - this.Origin.Y, 2);
                return dis <= Math.Pow(this.Raduis + otherC.Raduis, 2);
            } else if (other is CollidableAABB) {
                //Check to see if the origin is inside the rect.
                var otherAABB = other as CollidableAABB;

                //First  get which sides of the rect, the circle is on. This will eliminate two checks (at the least)
                //
                // If the point is left of the rect, but not above or below it, the two left points will need to be checked
                // If the Point is left of the rect, and fully above, only the top left point needs to be checked

                //But I'll do that later. Right now just check all four points, since the method is already written

                //origin
                //var d = this.DistanceSquared(this.Origin, otherAABB.Origin);
                //if (d <= Math.Pow(this.Raduis,2)) {
                //    return true;
                //}

                ////Top Right
                //d = this.DistanceSquared(this.Origin,
                //                         new Vector2(otherAABB.Origin.X + otherAABB.Width, otherAABB.Origin.Y));
                //if (d <= Math.Pow(this.Raduis, 2)) {
                //    return true;
                //}

                ////Bottom Right
                //d = this.DistanceSquared(this.Origin,
                //                         new Vector2(otherAABB.Origin.X + otherAABB.Width,
                //                                     otherAABB.Origin.Y - otherAABB.Height));
                //if (d < Math.Pow(this.Raduis, 2)) {
                //    return true;
                //}

                ////Bottom Left
                //d = this.DistanceSquared(this.Origin,
                //                         new Vector2(otherAABB.Origin.X, otherAABB.Origin.Y - otherAABB.Height));
                //if (d <= Math.Pow(this.Raduis, 2)) {
                //    return true;
                //}               

                if (otherAABB.PointInsideRect(this.Origin)) {
                    return true;
                }


                //Top
                var dist = Math.Abs(this.Origin.Y - otherAABB.Origin.Y);
                if (dist >= this.Raduis) {
                    return false;
                }

                //Right
                dist = Math.Abs(this.Origin.X - (otherAABB.Origin.X + otherAABB.Width));
                if (dist >= this.Raduis) {
                    return false;
                }

                //Bottom
                dist = Math.Abs((otherAABB.Origin.Y - otherAABB.Height) - this.Origin.Y);
                if (dist >= this.Raduis) {
                    return false;
                }

                //Left
                dist = Math.Abs((otherAABB.Origin.X) - this.Origin.X);
                if (dist >= this.Raduis) {
                    return false;
                }
                return true;

            }
            return false;
        }

        /// <summary>
        /// Returns the distance between 2 points, squared.
        /// //Distance remains squared because square rooting sucks
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private double DistanceSquared(Vector2 p1, Vector2 p2) {
            return Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2);
        }
    }
}
