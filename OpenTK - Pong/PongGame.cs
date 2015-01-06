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
}
