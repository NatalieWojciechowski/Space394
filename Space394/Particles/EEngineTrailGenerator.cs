using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.SceneObjects;
using Microsoft.Xna.Framework;

namespace Space394.Particles
{
    public class EEngineTrailGenerator : EngineTrailParticleGenerator
    {
        public EEngineTrailGenerator(Fighter _fighter, Vector3 _offset)
            : base(_fighter, _offset)
        {

        }

        public override EngineTrailParticle createEngineTrailParticle(Vector3 _position, EngineTrailParticleGenerator _home)
        {
            return new EEngineTrailParticle(_position, _home);
        }

        public override EngineTrailParticle spawnEngineTrailParticle(Vector3 position, Vector3 velocity)
        {
            EngineTrailParticle rParticle = null;
            if (availableEngineTrailParticles.Count != 0)
            {
                rParticle = availableEngineTrailParticles.First();
                availableEngineTrailParticles.RemoveAt(0);
            }
            else { }

            if (rParticle == null)
            {
                rParticle = createEngineTrailParticle(position, this);  // availableEngineTrailParticles.Dequeue();
            }
            else { }

            activeParticles.Add(rParticle);
            rParticle.respawn(position, Vector3.Zero);

            return rParticle;
        }
    }
}
