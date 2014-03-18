using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space394.Particles
{
    public class ExplosionParticle : Particle
    {
        private ExplosionParticleGenerator home;

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

        private const float TTL = 1.0f;
        private float ttl;

        public ExplosionParticle(Vector3 _position, ExplosionParticleGenerator _home)
            : base("Textures/Screens/grubby_paw_logo", _position)
        {
            home = _home;

            ttl = TTL;

            Random random = new Random((int)(_position.X + _position.Y + _position.Z));

            Scale = 100.0f;
        }

        public override void Update(float deltaTime)
        {
            Position += velocity * deltaTime;

            ttl -= deltaTime;
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

        public void onDie()
        {
            if (home != null)
            {
                home.addExplosionParticle(this);
            }
            else { }
            Active = false;
            QueuedRemoval = true;
        }

        public void respawn(Vector3 _position, Vector3 _velocity)
        {
            velocity = _velocity;
            Position = _position;
            QueuedRemoval = false;
            Active = true;
            ttl = TTL;
        }
    }
}
