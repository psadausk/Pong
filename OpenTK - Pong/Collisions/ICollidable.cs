
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKPong.Collisions {
    public interface ICollidable {
        
        bool Collided(ICollidable other);        
    }
}
