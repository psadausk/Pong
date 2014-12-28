using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTKPong.Collisions;
using Tao.OpenGl;
using System.Drawing;


namespace OpenTKPong.Entities {
    public class Ball : CollidableCircle{
        private Vector2 m_velocity;
        private float m_maxVelocity = 15f;
        public Ball(Vector2 origin, float raduis) : base(origin, raduis) {
            var r = new Random();
            var x = r.Next(2) + 3;
            var sign = r.Next(10);
            if (sign < 5) {
                x = x*-1;
            }

            var y = r.Next(2) + 3;
            sign = r.Next(10);
            if (sign < 5) {
                y = y * -1;
            }


            this.m_velocity = new Vector2(x,y);
        }

        public void Render() {
            GL.Color3(Color.White);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex2(this.Origin);
            for ( var i = 0; i < 360; i++ ) {
                var degInRad = i * ( Math.PI / 180 );
                GL.Vertex2(
                    this.Origin.X + Math.Cos(degInRad) * this.Raduis,
                    this.Origin.Y + Math.Sin(degInRad) * this.Raduis);
            }
            GL.End();
        }

        public void UpdatePosition(Vector2 velocityChange) {
            var newVelocityX = this.Clamp(this.m_velocity.X * velocityChange.X, this.m_maxVelocity);
            var newVelocityY = this.Clamp(this.m_velocity.Y * velocityChange.Y, this.m_maxVelocity);
            this.m_velocity = new Vector2(newVelocityX, newVelocityY);
            var x = this.Origin.X + this.m_velocity.X;
            var y  = this.Origin.Y + this.m_velocity.Y;
            this.Origin = new Vector2(x,y);
        }

        public void Scale(float width, float height) {
            throw new NotImplementedException();
        }

        private float Clamp(float value, float absMax) {
            return value < 0 ? Math.Max(absMax*-1, value) : Math.Min(absMax, value);
        }
    }
}
