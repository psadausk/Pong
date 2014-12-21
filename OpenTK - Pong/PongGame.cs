using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Tao.OpenGl;

namespace OpenTKPong {
    
    public class PongGame {
        [STAThread]
        static void Main(string[] args) {
            using (var p = new Pong()) {
                p.Run(30.0, 0.0);
            }
        }
    }

    /// <summary>
    /// All drawable objects (including the ball) are handled as quads.
    /// Tto make collision dectection easy
    /// </summary>
    public interface IDrawable {
        /// <summary>
        /// The TopLeft corner of the object
        /// </summary>        

        void Render();
        //void Collision(IDrawable other);
        //void Scale(float width, float height);
    }

    public interface ICollidable {
        int Origin { get; set; }
        int Width { get; set; }
        int Height { get; set; }

        //Vector2 Velocity { get; set; }

        bool HasCollided(ICollidable other);


    }
    public abstract class Quad : ICollidable {
        public int Origin { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool HasCollided(ICollidable other) {
            //Test horizontal lines

            //var v1 = new Line

            return false;
        }
    }

    public class Line {
        public Point P1 { get; set; }
        public Point P2 { get; set; }

        public Line(Point p1, Point p2) {
            this.P1 = p1;
            this.P2 = p2;
        }

        /// <summary>
        /// Checks to see if two lines intersect. 
        /// If they do, return the magnitude of the intersection
        /// If not return zero
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector2 Intersects(Line other) {
            //Take into account both vertical line
            if ( this.P1.X == this.P2.X  && other.P1.X == other.P2.X) {
                //two vertical lines, but not the same
                if ( this.P1.X != other.P1.X ) {
                    return Vector2.Zero;
                }

                if ( this.P2.Y > other.P1.Y ) {
                    return new Vector2(0, this.P2.Y - other.P1.Y);
                }

                if ( other.P2.Y > this.P1.Y ) {
                    return new Vector2(0, other.P2.Y - this.P1.Y);
                }

                return Vector2.Zero;
            }

            var m1 = (this.P2.Y - this.P1.Y)/(this.P2.X - this.P1.X);
            var m2 = ( other.P2.Y - other.P1.Y ) / ( other.P2.X - other.P1.X );

            var b1 = this.P1.Y - m1 * this.P1.X;
            var b2 = other.P1.Y - m2 * other.P1.X;

            //Parallel lines
            if ( m1 == m2 ) {
                //Parallel lines on the same slope
                if ( this.P1.Y == other.P2.Y ) {
                    //if(
                } else {
                    return Vector2.Zero;
                }
            } else {

            }

            return Vector2.Zero;
        }
    }
}
