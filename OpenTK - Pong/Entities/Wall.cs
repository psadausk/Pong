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
using System.Drawing;
using OpenTKPong.Collisions;

namespace OpenTKPong.Entities {
    public class Wall : CollidableAABB, IDrawable {
        public Wall(Vector2 origin, float width, float height) : base(origin, width, height) {
        }

        public void Render() {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Green);

            //Top Left
            GL.Vertex2(new Vector2d(Origin.X, Origin.Y));
            //Top Right
            GL.Vertex2(this.Origin.X + this.Width, this.Origin.Y);
            //Bottom Right
            GL.Vertex2(this.Origin.X + this.Width, this.Origin.Y - this.Height);
            //Bottom Left
            GL.Vertex2(this.Origin.X, this.Origin.Y - this.Height);
            GL.End();
        }
    }
}
