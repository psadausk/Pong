
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKPong.Collisions {
    public interface ICollidable {

        Vector2[] Vertices { get; set; }

        //Represent all object with a bounding box
        //That starts from the entity's origin

        bool Collided(ICollidable other);


        /// <summary>
        /// Returns a list of all normals to the shape
        /// </summary>
        /// <returns></returns>
        Vector2[] GetAxes();

        Projection ProjectShapeOnToAxes(Vector2 axes);
    }
}
