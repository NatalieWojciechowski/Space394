using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Collision;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.Particles
{
    public class ExplosionParticleGenerator : ParticleGenerator
    {
        private Vector3 position;

        private Queue<ExplosionParticle> availableExplosionParticles;
        public int availableExplosionParticlesCount() { return availableExplosionParticles.Count(); }
        protected int PARTICLES = 20;

        private bool active = false;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        private const float TTL = 5.0f;
        private float ttl;

        private Random random;

        public ExplosionParticleGenerator(Vector3 _position)
            : base()
        {
            position = _position;

            ttl = TTL;

            random = new Random((int)(System.DateTime.Now.Millisecond + position.X * 13 + position.Y * 7 + position.Z * 3));

            Initialize();
        }

        public void Initialize()
        {
            if (availableExplosionParticles == null)
            {
                availableExplosionParticles = new Queue<ExplosionParticle>();
            } 

            for (int i = 0; i < PARTICLES; i++)
            {
                availableExplosionParticles.Enqueue(createExplosionParticle(position));
            }
        }

        public override void Update(float deltaTime)
        {
            if (Active)
            {
                ttl -= deltaTime;
                if (ttl <= 0)
                {
                    ReadyToRemove = true;
                }
                else { }

                spawnExplosionParticle(position, Vector3.Up);
                
                for (int i = 0; i < particles.Count(); i++)
                {
                    particles[i].Update(deltaTime);
                }
            }
        }

        public override void Draw(GameCamera camera)
        {
            if (Active)
            {
                for (int i = 0; i < particles.Count(); i++)
                {
                    particles[i].Draw(camera);
                }
                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else { }
        }

        public ExplosionParticle addExplosionParticle(ExplosionParticle _particle)
        {
            availableExplosionParticles.Enqueue(_particle);
            particles.Remove(_particle);
            return _particle;
        }

        public ExplosionParticle createExplosionParticle(Vector3 _position)
        {
            return new ExplosionParticle(_position, this);
        }

        public ExplosionParticle spawnExplosionParticle(Vector3 position, Vector3 velocity)
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

            Vector3 tmpDir = Vector3.Up;
            tmpDir.Normalize();
            float posScale = 17;
            Vector3 tmpPositionAlt = new Vector3(roll * posScale * (float)random.NextDouble(), pitch * posScale * (float)random.NextDouble(), yaw * 3*posScale * (float)random.NextDouble());
            tmpPositionAlt += tmpDir * 15.5f;
            position += tmpPositionAlt;

            ExplosionParticle rParticle = null;
            if (availableExplosionParticles.Count() == 0)
            {
                rParticle = createExplosionParticle(position);
            }
            else
            {
                rParticle = availableExplosionParticles.Dequeue();
                particles.Add(rParticle);
                Vector3 tmpVel = Vector3.Up;
                tmpVel.Normalize();
                rParticle.respawn(position, tmpVel*0.5f);
            }
            return rParticle;
        }
    }
}
