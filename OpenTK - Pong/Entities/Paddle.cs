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
using OpenTKPong.Collisions;

namespace OpenTKPong.Entities {

    public class Paddle : CollidableAABB, IDrawable {
        //Refers to the top left coordinates of the paddle

        private float velocity = 10;

        public Paddle(Vector2 position, float width, float height) : base(position, width, height) {
        }

        public void Render() {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.White);

            //Top Left
            GL.Vertex2(this.Origin);
            //Top Right
            GL.Vertex2(this.Origin.X + this.Width, this.Origin.Y);
            //Bottom Right
            GL.Vertex2(this.Origin.X + this.Width, this.Origin.Y - this.Height);
            //Bottom Left
            GL.Vertex2(this.Origin.X, this.Origin.Y - this.Height);
            GL.End();
        }

        public void Scale(float width, float height) {
            //GL.Scale();
        }

        public void UpdatePosition(int velocity) {
            this.Origin = new Vector2(Origin.X, (float)(Origin.Y + velocity * this.velocity));
            this.ReCalcVertices();
        }
    }
}
