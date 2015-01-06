﻿using System;
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
using OpenTK.Audio;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace OpenTKPong {

    //
    // Layout of the game
    //  Origin is the top left
    //   -- 
    //  |  |
    //  |  |
    //  |  |
    //  |  |
    //   --
    //(0,0) point

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

        private AudioManager m_am = new AudioManager();

        private long  m_reset_delay = 500;
        private Stopwatch m_stopWatch;


        public Pong()
            : base(800, 600) {
            this.m_cameraMatrix = Matrix4.Identity;

            this.m_stopWatch = new Stopwatch();
            this.m_stopWatch.Start();
            this.Reset();

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
            //Left Paddle
            if ( this.m_leftPaddle.Collided(this.m_topWall) ) {
                this.m_leftPaddle.Origin = new Vector2(this.m_leftPaddle.Origin.X, this.m_topWall.Origin.Y - this.m_topWall.Height);
            } else if ( this.m_leftPaddle.Collided(this.m_bottomWall) ) {
                this.m_leftPaddle.Origin = new Vector2(this.m_leftPaddle.Origin.X, this.m_bottomWall.Origin.Y + this.m_paddleHeight);
            }

            //Right Paddle
            if ( this.m_rightPaddle.Collided(this.m_topWall) ) {
                this.m_rightPaddle.Origin = new Vector2(this.m_rightPaddle.Origin.X, this.m_topWall.Origin.Y - this.m_topWall.Height);
            } else if ( this.m_rightPaddle.Collided(this.m_bottomWall) ) {
                this.m_rightPaddle.Origin = new Vector2(this.m_rightPaddle.Origin.X, this.m_bottomWall.Origin.Y + this.m_paddleHeight);
            }

            //Ball
            if ( this.m_ball.Collided(this.m_topWall) ) {
                this.m_ball.UpdatePosition(new Vector2(1, -1.1f));
                this.m_am.PlaySound(SoundEnums.Boop_1, false);
            } else if ( this.m_ball.Collided(this.m_bottomWall) ) {
                this.m_ball.UpdatePosition(new Vector2(1, -1.1f));
                this.m_am.PlaySound(SoundEnums.Boop_1, false);
            } else if ( this.m_ball.Collided(this.m_leftPaddle) ) {
                this.m_ball.UpdatePosition(new Vector2(-1.1f, 1));
                this.m_am.PlaySound(SoundEnums.Boop_1, false);
            } else if ( this.m_ball.Collided(this.m_rightPaddle) ) {
                this.m_ball.UpdatePosition(new Vector2(-1.1f, 1));
                this.m_am.PlaySound(SoundEnums.Boop_1, false);
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

            if ( Keyboard[Key.R] ) {
                if ( this.m_stopWatch.ElapsedMilliseconds > this.m_reset_delay ) {
                    this.Reset();
                    this.m_stopWatch.Reset();
                    this.m_stopWatch.Start();
                }
            }
        }

        private void UpdateBall() {
            //Maintain velocity
            this.m_ball.UpdatePosition(new Vector2(1, 1));
        }

        protected override void OnLoad(EventArgs e) {
            //GL.ClearColor(Color.Black);
            //GL.Enable(EnableCap.DepthTest);

            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Glu.gluOrtho2D(0.0, (double)Width, 0.0, (double)Height);
        }

        private void Reset() {
            this.m_leftPaddle = new Paddle(
                new Vector2(this.m_paddleOffset,
                    this.Height / 2 + this.m_paddleHeight / 2),
                    this.m_paddleWidth, this.m_paddleHeight);
            this.m_rightPaddle = new Paddle(
                new Vector2(
                    this.Width - this.m_paddleWidth,
                    this.Height / 2 + this.m_paddleHeight / 2), this.m_paddleWidth, this.m_paddleHeight);

            this.m_topWall = new Wall(new Vector2(0, this.Height + 50), this.Width, 5 + 50);
            this.m_bottomWall = new Wall(new Vector2(0, 5), this.Width, 5 + 50);

            this.m_ball = new Ball(new Vector2(this.Width / 2, this.Height / 2), 10);

            this.LoadBGM();
        }

        private void LoadBGM() {
            //First clear running background music
            this.m_am.KillSounds();
            Thread.Sleep(250);
            this.m_am.PlaySound(SoundEnums.Bgm, true);
            //var nbgw = new BackgroundWorker();

            //nbgw.DoWork += ( (s, e) => at.Start() );
            //nbgw.RunWorkerCompleted += ((s,e) => at.Stop());

            //nbgw.RunWorkerAsync();

            //this.m_runningAudioTasks["bgm"] = at;

        }     
    }
}
