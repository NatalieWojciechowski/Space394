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
    public class MissileTrailParticleGenerator : ParticleGenerator
    {
        private Projectile projectile;

        private Vector3 lastPosition;
        private Vector3 position;

        private List<MissileTrailParticle> activeParticles;
        private List<MissileTrailParticle> availableMissileTrailParticles;
        public int availableMissileTrailParticlesCount() { return availableMissileTrailParticles.Count(); }
        protected int PARTICLES = 20;

        private bool active = false;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        private const int DRAW_RANGE = 10000000;

        private const float SPAWN_DISTANCE = 10.0f;
        private const float MAX_DISTANCE = 200.0f;

        private float distance;

        private const float SIMPLE_DRAW_DISTANCE = 10000000.0f;

        public MissileTrailParticleGenerator(Projectile _projectile)
            : base()
        {
            projectile = _projectile;
            position = projectile.Position;
            lastPosition = position;

            distance = 0;

            Initialize();
        }

        public void Initialize()
        {
            if (activeParticles == null)
            {
                activeParticles = new List<MissileTrailParticle>();
            }
            else { }

            if (availableMissileTrailParticles == null)
            {
                availableMissileTrailParticles = new List<MissileTrailParticle>();
            }
            else { }

            for (int i = 0; i < PARTICLES; i++)
            {
                availableMissileTrailParticles.Add(createMissileTrailParticle(position, this));
            }
        }

        public override void Update(float deltaTime)
        {
            if (Active)
            {
                using (new ReadLock(particleLock))
                {
                    position = projectile.Position;

                    Vector3 direction = position - lastPosition;
                    distance += direction.Length();
                    if (distance > MAX_DISTANCE)
                    {
                        distance = MAX_DISTANCE;
                    }
                    else { }

                    direction.Normalize();
                    direction *= SPAWN_DISTANCE;
                    while (distance >= SPAWN_DISTANCE)
                    {
                        distance -= SPAWN_DISTANCE;
                        using (new WriteLock(particleLock))
                        {
                            spawnMissileTrailParticle(lastPosition += direction, projectile.Velocity);
                        }
                    }

                    /*foreach (MissileTrailParticle particle in activeParticles)
                    {
                        particle.Update(deltaTime);
                    }*/

                    for (int i = 0; i < activeParticles.Count; i++)
                    {
                        activeParticles[i].Update(deltaTime);
                        if (activeParticles[i].QueuedRemoval)
                        {
                            availableMissileTrailParticles.Add(activeParticles[i]);
                            activeParticles.RemoveAt(i);
                        }
                        else { }
                    }

                    lastPosition = position;
                }
            }
            else { }
        }

        public override void Draw(GameCamera camera)
        {
            if (Active && Vector3.DistanceSquared(position, camera.Position) <= SIMPLE_DRAW_DISTANCE)
            {
                using (new ReadLock(particleLock))
                {
                    if (((PlayerCamera)camera).PlayerShip != null)
                    {
                        Matrix world = Matrix.CreateFromQuaternion(((PlayerCamera)camera).PlayerShip.Rotation);
                        foreach (MissileTrailParticle particle in activeParticles)
                        {
                            particle.Draw(camera, world);
                        }
                    }
                    else
                    {
                        foreach (MissileTrailParticle particle in activeParticles)
                        {
                            particle.Draw(camera);
                        }
                    }
                }
                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else { }
        }

        public MissileTrailParticle addMissileTrailParticle(MissileTrailParticle _particle)
        {
            return _particle;
        }

        public MissileTrailParticle createMissileTrailParticle(Vector3 _position, MissileTrailParticleGenerator _home)
        {
            return new MissileTrailParticle(_position, _home);
        }

        public MissileTrailParticle spawnMissileTrailParticle(Vector3 position, Vector3 velocity)
        {
            MissileTrailParticle rParticle = null;
            if (availableMissileTrailParticles.Count != 0)
            {
                rParticle = availableMissileTrailParticles.First();
                availableMissileTrailParticles.RemoveAt(0);
            }
            else { }

            if (rParticle == null)
            {
                rParticle = createMissileTrailParticle(position, this);
            }
            else { }

            activeParticles.Add(rParticle);
            rParticle.respawn(position, Vector3.Zero);

            return rParticle;
        }

        public override void clearLists()
        {
            using (new WriteLock(particleLock))
            {
                for (int i = 0; i < activeParticles.Count; i++)
                {
                    activeParticles[i].onDie();
                    availableMissileTrailParticles.Add(activeParticles[i]);
                }
                activeParticles.Clear();
            }
        }
    }
}
