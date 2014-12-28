using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKPong.Collisions {
    public abstract class Collidable : IPolygon {

        public virtual Vector2[] Vertices { get; set; }

        public Collidable(Vector2[] vertices) {
            this.Vertices = vertices;
        }

        public Collidable() {
        }


        public virtual bool Collided(ICollidable other) {

            if (other is IPolygon) {
                var otherP = other as IPolygon;
                var axes = this.GetAxes();
                var otherAxes = otherP.GetAxes();
                var unionAxes = axes.Union(otherAxes, new Vector2Comparator()).Distinct(new Vector2Comparator());

                for (var index = 0; index < axes.Length; index++) {
                    var p1 = this.ProjectShapeOnToAxes(axes[index]);
                    var p2 = otherP.ProjectShapeOnToAxes(axes[index]);

                    if (!p1.Intersects(p2)) {
                        return true;
                    }
                }

                for (var index = 0; index < otherAxes.Length; index++) {
                    var p1 = this.ProjectShapeOnToAxes(axes[index]);
                    var p2 = otherP.ProjectShapeOnToAxes(axes[index]);

                    if (!p1.Intersects(p2)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public Vector2[] GetAxes() {
            var axes = new Vector2[Vertices.Length];
            for ( var index = 0; index < axes.Length; index++ ) {
                var p1 = this.Vertices[index];
                var nextIndex = index + 1;
                if ( nextIndex == axes.Length ) {
                    nextIndex = 0;
                }
                var p2 = this.Vertices[nextIndex];
                Vector2 edge;
                Vector2.Subtract(ref p1, ref p2, out edge);

                axes[index] = edge.PerpendicularRight;
            }

            return axes;
        }

        public Projection ProjectShapeOnToAxes(Vector2 axis) {
            var v1 = axis.Normalized();
            var v2 = this.Vertices[0];
            var min = Vector2.Dot(v1,  v2);
            var max = min;
            for ( var index = 1; index < this.Vertices.Length; index++ ) {
                v2 = this.Vertices[index];
                var p = Vector2.Dot(v1, v2);
                if ( p < min ) {
                    min = p;
                } else if ( p > max ) {
                    max = p;
                }
            }
            return new Projection(min, max);
        }
    }

    public class Vector2Comparator : IEqualityComparer<Vector2> {

        public bool Equals(Vector2 v1, Vector2 v2) {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        public int GetHashCode(Vector2 obj) {
            return obj.GetHashCode();
        }
    }
}
