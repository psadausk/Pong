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

namespace OpenTK___Pong {
    
    public class Program {
        [STAThread]
        static void Main(string[] args) {
            using (var p = new Pong()) {
                p.Run(30.0, 0.0);
            }
        }
    }


    public class Pong : GameWindow {
        private Matrix4 m_cameraMatrix;
        private float m_paddleWidth = 25f;
        private float m_paddleHeight = 200f;

        private Paddle m_leftPaddle;
        private Paddle m_rightPaddle;
        private Ball m_ball;

        private float m_facing = 0.0f;
        private float m_pitch = 0.0f;
        private Vector3 m_cameraLocation;
        private Vector3 m_upVector = Vector3.UnitY;

        public Pong()
            : base(800, 600) {
            this.m_cameraMatrix = Matrix4.Identity;
            this.m_leftPaddle = new Paddle(
                new Vector2d(0.0f, 
                    this.Height/2 + this.m_paddleHeight/2), 
                    this.m_paddleWidth, this.m_paddleHeight);
            this.m_rightPaddle = new Paddle(
                new Vector2d(
                    this.Width - this.m_paddleWidth, 
                    this.Height/2 + this.m_paddleHeight/2), this.m_paddleWidth, this.m_paddleHeight);
            this.m_ball = new Ball(new Vector2d(this.Width/2, this.Height/2), 10);
            
            this.m_cameraLocation = new Vector3(1, 0,0);
            this.m_facing = 5.0f;
            Glu.gluOrtho2D(0.0f, (double)this.Width, 0.0, (double)this.Height);
        }

        protected override void OnResize(EventArgs e) {
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Glu.gluOrtho2D(0.0, (double)Width, 0.0, (double)Height);
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadMatrix(ref this.m_cameraMatrix);
            this.RenderObjects();
            this.SwapBuffers();
        }

        private void RenderObjects() {
            //this.DrawCube(0,0,0);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            this.m_leftPaddle.Render();
            this.m_rightPaddle.Render();
            this.m_ball.Render();
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            //var lookatPoint = new Vector3((float)Math.Cos(this.m_facing), this.m_pitch, (float)Math.Sin(this.m_facing));
            //this.m_cameraMatrix = Matrix4.LookAt(this.m_cameraLocation, this.m_cameraLocation + lookatPoint, this.m_upVector);
            //var lookatPoint = new Vector3((float)Math.Cos(this.m_facing), this.m_pitch, (float)Math.Sin(this.m_facing));
            //this.m_cameraMatrix = Matrix4.LookAt(this.m_cameraLocation, this.m_cameraLocation + lookatPoint, this.m_upVector);

        }

        protected override void OnLoad(EventArgs e) {
            //GL.ClearColor(Color.Black);
            //GL.Enable(EnableCap.DepthTest);

            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Glu.gluOrtho2D(0.0, (double)Width, 0.0, (double)Height);
        }
    }

    public interface IDrawable {
        void Render();
        void Scale(float width, float height);
    }

    public class Paddle : IDrawable {
        //Refers to the top left coordinates of the paddle
        private Vector2d m_position;

        private double m_width;
        private double m_height;

        public Paddle(Vector2d position, float width, float height) {
            this.m_position = position;
            this.m_width = width;
            this.m_height = height;
        }

        public void Render() {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.White);

            //Top Left
            GL.Vertex2(this.m_position);
            //Top Right
            GL.Vertex2(this.m_position.X + this.m_width, this.m_position.Y);
            //Bottom Right
            GL.Vertex2(this.m_position.X + this.m_width, this.m_position.Y - this.m_height);
            //Bottom Left
            GL.Vertex2(this.m_position.X, this.m_position.Y - this.m_height);
            GL.End();
        }

        public void Scale(float width, float height) {
            //GL.Scale();
        }
    }

    public class Ball : IDrawable {
        private Vector2d m_origin;
        private float m_raduis;
        private Vector2 m_velocity;

        public Ball(Vector2d origin, float raduis) {
            this.m_origin = origin;
            this.m_raduis = raduis;
        }

        public void Render() {
            GL.Color3(Color.White);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex2(this.m_origin);
            for (var i = 0; i < 360; i++) {
                var degInRad = i * (Math.PI / 180);
                GL.Vertex2(
                    this.m_origin.X + Math.Cos(degInRad) * this.m_raduis,
                    this.m_origin.Y + Math.Sin(degInRad) * this.m_raduis);
            }
            GL.End();

        }

        public void Scale(float width, float height) {
            throw new NotImplementedException();
        }
    }
}
