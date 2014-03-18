using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.SceneObjects;
using System.Threading;

namespace Space394.Particles
{
    public abstract class ParticleGenerator
    {
        protected List<Particle> particles;
        public virtual void clearLists() { particles.Clear(); }

        private bool readyToRemove;
        public bool ReadyToRemove
        {
            get { return readyToRemove; }
            set { readyToRemove = value; }
        }

        protected static ReaderWriterLockSlim particleLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;

        public ParticleGenerator()
            : base()
        {
            particles = new List<Particle>();

            readyToRemove = false;
        }

        public virtual void Update(float deltaTime)
        {
            foreach (Particle particle in particles)
            {
                particle.Update(deltaTime);
            }
        }

        public virtual void Draw(GameCamera camera)
        {
            foreach (Particle particle in particles)
            {
                particle.Draw(camera);
            }
        }
    }
}
