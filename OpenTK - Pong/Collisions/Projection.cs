using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKPong.Collisions {
    public class Projection {

        public float Min { get; set; }
        public float Max { get; set; }

        public Projection(float min, float max) {
            this.Min = min;
            this.Max = max;
        }

        public bool Intersects(Projection other) {
            var i0 = Math.Max(this.Min, other.Min);
            var i1 = Math.Min(this.Max, other.Max);

            return i0 <= i1;
        }
    }
}
