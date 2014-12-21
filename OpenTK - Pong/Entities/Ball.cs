using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Tao.OpenGl;
using System.Drawing;


namespace OpenTKPong.Entities {
    public class Ball : IDrawable {
        private Vector2d m_origin;
        private float m_raduis;
        private Vector2 m_velocity;

        public Ball(Vector2d origin, float raduis) {
            this.m_origin = origin;
            this.m_raduis = raduis;
            var r = new Random();
            this.m_velocity = new Vector2(r.Next(5), r.Next(5));
        }

        public void Render() {
            GL.Color3(Color.White);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex2(this.m_origin);
            for ( var i = 0; i < 360; i++ ) {
                var degInRad = i * ( Math.PI / 180 );
                GL.Vertex2(
                    this.m_origin.X + Math.Cos(degInRad) * this.m_raduis,
                    this.m_origin.Y + Math.Sin(degInRad) * this.m_raduis);
            }
            GL.End();
        }

        public void UpdatePosition(Vector2 velocityChange, Size Bounds) {
            this.m_origin.X = this.m_origin.X + velocityChange.X * this.m_velocity.X;
            this.m_origin.Y = this.m_origin.Y + velocityChange.Y * this.m_velocity.Y;

        }

        public void Scale(float width, float height) {
            throw new NotImplementedException();
        }
    }
}
