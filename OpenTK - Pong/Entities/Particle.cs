using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKPong.Entities {
    public class Particle {
        private int m_duration;
        private Vector2 m_origin;
        private Vector2 m_velocity;
        private int m_radius;
        private int m_width;
        private int m_height;
        private Color m_color;
        public bool Alive {get;set;}

        public Particle(int life, int width, int height) {
            this.m_duration = life;
            this.m_width = width;
            this.m_height = height;
        }

        public void Emit(Vector2 origin, Vector2 velocity, Color color) {
            this.Alive = true;
            this.m_origin = origin;
            this.m_velocity = velocity;
        }

        public void UpdatePosition() {
            if ( this.Alive ) {
                var x = this.m_origin.X + this.m_velocity.X;
                var y = this.m_origin.Y + this.m_velocity.Y;
                this.m_origin = new Vector2(x, y);
            }
        }

        public void Render() {
            if ( this.Alive ) {
                GL.Color3(this.m_color);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(this.m_origin);
                for ( var i = 0; i < 360; i++ ) {
                    var degInRad = i * ( Math.PI / 180 );
                    GL.Vertex2(
                        this.m_origin.X + Math.Cos(degInRad) * this.m_radius,
                        this.m_origin.Y + Math.Sin(degInRad) * this.m_radius);
                }
                GL.End();
            }
        }
    }
}
