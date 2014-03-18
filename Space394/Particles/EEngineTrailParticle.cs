using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space394.Particles
{
    public class EEngineTrailParticle : EngineTrailParticle
    {
        public EEngineTrailParticle(Vector3 _position, EngineTrailParticleGenerator _home)
            : base(_position, _home, "Textures/EngineParticleTexture")
        {
        }
    }
}
