using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.Particles
{
    public abstract class EngineTrailParticle : Particle
    {
        protected EngineTrailParticleGenerator home;

        protected Vector3 velocity;
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        private bool active = false;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        protected float ttl;
        protected const float TTL = 0.5f;

        protected float ALPHA_SCALE = 2.5f;

        public EngineTrailParticle(Vector3 _position, EngineTrailParticleGenerator _home)
            : this(_position, _home, "Textures/EngineParticleTexture")
        {  }

        public EngineTrailParticle(Vector3 _position, EngineTrailParticleGenerator _home, String _texture)
            : base(_texture, _position)
        {
            home = _home;

            active = true;

            ttl = TTL;
        }

        public override void Update(float deltaTime)
        {
            ttl -= deltaTime;
            alpha = Math.Max(0, alpha - (ALPHA_SCALE * deltaTime));
            if (ttl <= 0)
            {
                onDie();
            }
            else { }
        }

        public override void Draw(GameCamera camera)
        {
            if (Active)
            {
                base.Draw(camera);
            }
            else { }
        }

        public virtual void onDie()
        {
            if (home != null)
            {
                home.addEngineTrailParticle(this);
            }
            else { }
            Active = false;
            QueuedRemoval = true;
        }

        public virtual void respawn(Vector3 _position, Vector3 _velocity)
        {
            ttl = TTL;
            velocity = _velocity;
            Position = _position;
            QueuedRemoval = false;
            Active = true;
            alpha = 1;
        }
    }
}
