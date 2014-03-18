using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.Particles
{
    public class DustParticle : Particle
    {
        protected DustParticleGenerator home;

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

        public DustParticle(Vector3 _position, DustParticleGenerator _home)
            : base("Textures/DustParticleTexture", _position)
        {
            home = _home;

            Random random = new Random((int)(_position.X + _position.Y + _position.Z));

            Scale = 0.1f;
        }

        public override void Update(float deltaTime)
        {
            Position += velocity * deltaTime;

            // out of collider
            if (Vector3.DistanceSquared(Position, home.getVisibleSphere().Position) >= home.getVisibleSphere().RadiusSq)
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
                home.addDustParticle(this);
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
        }
    }
}
