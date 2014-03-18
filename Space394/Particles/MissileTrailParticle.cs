using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.Particles
{
    public class MissileTrailParticle : Particle
    {
        protected MissileTrailParticleGenerator home;

        private Vector3 velocity;
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

        private float ttl;
        private const float TTL = 0.5f;

        private float ALPHA_SCALE = 0.5f;

        public MissileTrailParticle(Vector3 _position, MissileTrailParticleGenerator _home)
            : base("Textures/MissileParticleTexture", _position)
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
        }

        public void onDie()
        {
            if (home != null)
            {
                home.addMissileTrailParticle(this);
            }
            else { }
            Active = false;
            QueuedRemoval = true;
        }

        public void respawn(Vector3 _position, Vector3 _velocity)
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
