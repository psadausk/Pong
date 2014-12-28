using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKPong.Collisions {
    public interface IPolygon : ICollidable{

        //Represent all object with a bounding box
        //That starts from the entity's origin
        Vector2[] Vertices { get; set; }

        /// <summary>
        /// Returns a list of all normals to the shape
        /// </summary>
        /// <returns></returns>
        Vector2[] GetAxes();

        Projection ProjectShapeOnToAxes(Vector2 axes);
        
    }
}
