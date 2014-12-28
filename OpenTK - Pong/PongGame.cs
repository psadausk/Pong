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
                p.Run(60.0, 0.0);
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
}
