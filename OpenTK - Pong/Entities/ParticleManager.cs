using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKPong.Entities {
    public class ParticleManager {
        public Particle[] Particles { get; set; }

        private const int ParticleDuration = 10;
        private const int ParticleWidth = 2;
        private const int ParticleHeight = 2;

        public ParticleManager(int capacity) {
            this.Particles = new Particle[capacity];

            for ( var i = 0; i < this.Particles.Length; i++ ) {
                //this.Particles[i] = new Particle();
            }
        }
    }
}
