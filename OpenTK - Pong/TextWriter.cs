using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace OpenTKPong {
    public class TextWriter {
        private readonly Font TextFont = new Font(FontFamily.GenericSansSerif, 16);
        private readonly Bitmap TextBitmap;
        private Vector2 m_origin;
        private string m_text;
        private Brush m_brush;
        private int _textureId;
        private Size _clientSize;
        private TextureUnit m_tu;
        public TextWriter(Vector2 origin, int width, int height, TextureUnit tu) {
            m_origin = origin;
            m_text = "";
            m_brush = Brushes.White;

            TextBitmap = new Bitmap(width, height);
            this.m_tu = tu;
            _textureId = CreateTexture();
        }

        private int CreateTexture() {
            int textureId;
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Replace);//Important, or wrong color on some computers
            var bitmap = TextBitmap;
            GL.GenTextures(1, out textureId);            
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);            
            GL.Finish();
            bitmap.UnlockBits(data);
            return textureId;
        }

        public void Dispose() {
            if ( _textureId > 0 )
                GL.DeleteTexture(_textureId);
        }

        public void UpdateText(string newText) {
            this.m_text = newText;
            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            using ( Graphics gfx = Graphics.FromImage(TextBitmap) ) {
                gfx.Clear(Color.Black);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                gfx.DrawString(m_text, TextFont, Brushes.White, 0,0);
            }

            System.Drawing.Imaging.BitmapData data = TextBitmap.LockBits(new Rectangle(0, 0, TextBitmap.Width, TextBitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, TextBitmap.Width, TextBitmap.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            TextBitmap.UnlockBits(data);
        }

        public void Draw() {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.DstColor);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _textureId);

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(this.m_origin.X, 0);
            GL.TexCoord2(1, 0); GL.Vertex2(this.m_origin.X+ TextBitmap.Width, 0);
            GL.TexCoord2(1, 1); GL.Vertex2(this.m_origin.X+ TextBitmap.Width, TextBitmap.Height);
            GL.TexCoord2(0, 1); GL.Vertex2(this.m_origin.X, TextBitmap.Height);
            GL.End();
            GL.PopMatrix();

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
        }
    }
}
