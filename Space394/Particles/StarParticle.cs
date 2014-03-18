using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.Particles
{
    public class StarParticle : Particle
    {
        private bool dimming;

        private const float DIMMING_SPEED = 1.00f;

        private float randomMod;

        public StarParticle(Vector3 _position)
            : base("Textures/StarParticleTexture", _position)
        {
            Random random = new Random((int)(_position.X + _position.Y + _position.Z));

            if (random.Next() % 2 == 0)
            {
                dimming = true;
            }
            else
            {
                dimming = false;
            }

            randomMod = (float)random.NextDouble();

            alpha = randomMod;

            Scale = (float)random.NextDouble();
        }

        public override void Update(float deltaTime)
        {
            if (dimming)
            {
                alpha -= deltaTime * (DIMMING_SPEED * randomMod);
                if (alpha <= 0)
                {
                    alpha = 0;
                    dimming = false;
                }
                else { }
            }
            else 
            {
                alpha += deltaTime * (DIMMING_SPEED * randomMod);
                if (alpha >= 1)
                {
                    alpha = 1;
                    dimming = true;
                }
                else { }
            }
        }
    }
}
