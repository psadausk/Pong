using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Tao.OpenGl;
using OpenTKPong.Entities;
using System.Drawing;

namespace OpenTKPong {
    public class Pong : GameWindow {
        private Matrix4 m_cameraMatrix;
        private int m_paddleWidth = 25;
        private int m_paddleHeight = 200;
        private float m_paddleOffset = 20f;

        private Paddle m_leftPaddle;
        private Paddle m_rightPaddle;
        private Ball m_ball;
        private Wall m_topWall;
        private Wall m_bottomWall;

        private Vector3 m_upVector = Vector3.UnitY;

        public Pong()
            : base(800, 600) {
            this.m_cameraMatrix = Matrix4.Identity;
            this.m_leftPaddle = new Paddle(
                new Vector2(this.m_paddleOffset,
                    this.Height / 2 + this.m_paddleHeight / 2),
                    this.m_paddleWidth, this.m_paddleHeight);
            this.m_rightPaddle = new Paddle(
                new Vector2(
                    this.Width - this.m_paddleWidth,
                    this.Height / 2 + this.m_paddleHeight / 2), this.m_paddleWidth, this.m_paddleHeight);

            this.m_topWall = new Wall(new Vector2(0, this.Height + 50), this.Width, 5 + 50);
            this.m_bottomWall = new Wall(new Vector2(0, 5), this.Width, 5);

            this.m_ball = new Ball(new Vector2d(this.Width / 2, this.Height / 2), 10);

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

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            this.m_leftPaddle.Render();
            this.m_rightPaddle.Render();
            this.m_topWall.Render();
            this.m_bottomWall.Render();
            this.m_ball.Render();
        }

        private void DetectCollisions() {
            if ( this.m_leftPaddle.Collided(this.m_topWall) ) {
                this.m_leftPaddle.Origin = new Vector2(this.m_leftPaddle.Origin.X, this.m_topWall.Origin.Y - this.m_topWall.Height);
            } else if ( this.m_leftPaddle.Collided(this.m_bottomWall) ) {
                this.m_leftPaddle.Origin = new Vector2(this.m_leftPaddle.Origin.X, this.m_bottomWall.Origin.Y + this.m_paddleHeight);
            }

            if ( this.m_rightPaddle.Collided(this.m_topWall) ) {
                this.m_rightPaddle.Origin = new Vector2(this.m_rightPaddle.Origin.X, this.m_topWall.Origin.Y - this.m_topWall.Height);
            } else if ( this.m_rightPaddle.Collided(this.m_bottomWall) ) {
                this.m_rightPaddle.Origin = new Vector2(this.m_rightPaddle.Origin.X, this.m_bottomWall.Origin.Y + this.m_paddleHeight);
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            this.UpdateKeyboardEvents();
            this.UpdateBall();
            this.DetectCollisions();
        }

        private void UpdateKeyboardEvents() {
            if ( Keyboard[Key.W] ) {
                this.m_leftPaddle.UpdatePosition(1);
            } else if ( Keyboard[Key.S] ) {
                this.m_leftPaddle.UpdatePosition(-1);
            }

            if ( Keyboard[Key.Up] ) {
                this.m_rightPaddle.UpdatePosition(1);
            } else if ( Keyboard[Key.Down] ) {
                this.m_rightPaddle.UpdatePosition(-1);
            }
        }

        private void UpdateBall() {
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
}
