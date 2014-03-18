using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space394.Particles
{
    public class HEngineTrailParticle : EngineTrailParticle
    {
        // 0.25 and above causes ships to disappear
        private const float INTERVAL = 0.20f;
        private float START_SCALE;

        private const float MORE_ALPHA_SCALE = 4.5f;

        private float currentAlpha;

        public HEngineTrailParticle(Vector3 _position, EngineTrailParticleGenerator _home, float _scale)
            : base(_position, _home, "Textures/Smoketrailtest")
        {
            setDefaultScale(_scale);
            resetScale();
            START_SCALE = _scale;
            currentAlpha = 1.25f;
        }

        public override void Update(float deltaTime)
        {
            Scale = Scale + (INTERVAL * deltaTime);
            ttl -= deltaTime;
            currentAlpha -= (MORE_ALPHA_SCALE * deltaTime);
            alpha = Math.Min(Math.Max(0.0f, currentAlpha), 1.0f);
            if (ttl <= 0)
            {
                onDie();
            }
            else { }
        }

        public override void onDie()
        {
            resetScale();
            base.onDie();
        }

        public override void respawn(Vector3 _position, Vector3 _velocity)
        {
            Scale = 1 + START_SCALE;
            currentAlpha = 2.0f;
            base.respawn(_position, _velocity);
        }
    }
}
