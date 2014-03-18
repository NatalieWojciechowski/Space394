using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.SceneObjects;
using Space394.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.Particles
{
    public class DustParticleGenerator : ParticleGenerator
    {
        private Fighter ship;

        private Vector3 position;

        private Queue<DustParticle> availableDustParticles;
        public int availableDustParticlesCount() { return availableDustParticles.Count(); }
        protected int PARTICLES = 20;

        private bool active = false;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        private CollisionSphere visibleParticles;
        public CollisionSphere getVisibleSphere() { return visibleParticles; }

        private Random random;

        private const int VISIBLE_RANGE = 40;

        public DustParticleGenerator(Fighter _ship)
            : base()
        {
            ship = _ship;
            position = ship.Position;

            random = new Random((int)(System.DateTime.Now.Millisecond + position.X * 13 + position.Y * 7 + position.Z * 3));

            visibleParticles = new CollisionSphere(ship.Position, VISIBLE_RANGE);

            Initialize();
        }

        public void Initialize()
        {
            if (availableDustParticles == null)
            {
                availableDustParticles = new Queue<DustParticle>();
            }
            else { }

            for (int i = 0; i < PARTICLES; i++)
            {
                availableDustParticles.Enqueue(createDustParticle(position, this));
            }
        }

        public override void Update(float deltaTime)
        {
            if (Active)
            {
                using (new ReadLock(particleLock))
                {
                    position = ship.Position;
                    visibleParticles.Position = position;
                    using (new WriteLock(particleLock))
                    {
                        spawnDustParticle(position, ship.Velocity);
                    }

                    for (int i = 0; i < particles.Count(); i++)
                    {
                        particles[i].Update(deltaTime);
                    }
                }
            }
        }

        public override void Draw(GameCamera camera)
        {
            if (Active)
            {
                using (new ReadLock(particleLock))
                {
                    int count = particles.Count();
                    for (int i = 0; i < count; i++)
                    {
                        particles[i].Draw(camera);
                    }
                    Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                }
            }
            else { }
        }

        public DustParticle addDustParticle(DustParticle _particle)
        {
            availableDustParticles.Enqueue(_particle);
            particles.Remove(_particle);
            return _particle;
        }

        public DustParticle createDustParticle(Vector3 _position, DustParticleGenerator _home)
        {
            return new DustParticle(_position, _home);
        }

        public DustParticle spawnDustParticle(Vector3 position, Vector3 velocity)
        {
            float yaw = ((float)random.NextDouble() * (MathHelper.Pi * 2)/2);
            if (random.Next() % 2 == 0)
            {
                yaw *= -1;
            }
            else { }

            float pitch = ((float)random.NextDouble() * (MathHelper.Pi * 2) / 2);
            if (random.Next() % 2 == 0)
            {
                pitch *= -1;
            }
            else { } 

            float roll = ((float)random.NextDouble() * (MathHelper.Pi * 2) / 2);
            if (random.Next() % 2 == 0)
            {
                roll *= -1;
            }
            else { } 
            
            Vector3 tmpDir = Vector3.Transform(Vector3.Backward, ship.Rotation);
            tmpDir.Normalize();
            float posScale = 17;
            Vector3 tmpPositionAlt = new Vector3(roll * posScale * (float)random.NextDouble(), pitch * posScale * (float)random.NextDouble(), yaw * 3*posScale * (float)random.NextDouble());
            tmpPositionAlt += tmpDir * 15.5f;
            position += tmpPositionAlt;

            DustParticle rParticle = null;
            if (availableDustParticlesCount() == 0)
            {
                rParticle = createDustParticle(position, this);
            }
            else
            {
                rParticle = availableDustParticles.Dequeue();
                particles.Add(rParticle);
                Vector3 tmpVel = ship.Velocity;
                tmpVel.Normalize();
                rParticle.respawn(position, tmpVel*0.5f);
            }
            return rParticle;
        }
    }
}
