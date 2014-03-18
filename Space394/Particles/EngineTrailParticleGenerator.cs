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
    public abstract class EngineTrailParticleGenerator : ParticleGenerator
    {
        protected Fighter fighter;

        protected Vector3 lastPosition;
        protected Vector3 position;

        protected List<EngineTrailParticle> activeParticles;
        protected List<EngineTrailParticle> availableEngineTrailParticles;
        public int availableEngineTrailParticlesCount() { return availableEngineTrailParticles.Count(); }
        protected int PARTICLES = 20;

        protected bool active = false;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        protected const int DRAW_RANGE = 10000000;

        protected float SPAWN_DISTANCE = 1.0f;
        protected float MAX_DISTANCE = 50.0f;

        protected float distance;

        protected Vector3 offset;
        public Vector3 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        protected const float SIMPLE_DRAW_DISTANCE = 10000000.0f;

        public EngineTrailParticleGenerator(Fighter _fighter, Vector3 _offset)
            : base()
        {
            fighter = _fighter;
            position = fighter.Position;
            lastPosition = position;

            offset = _offset;

            distance = 0;

            Initialize();
        }

        public virtual void Initialize()
        {
            if (activeParticles == null)
            {
                activeParticles = new List<EngineTrailParticle>();
            }
            else { }

            if (availableEngineTrailParticles == null)
            {
                availableEngineTrailParticles = new List<EngineTrailParticle>();
            }
            else { }

            for (int i = 0; i < PARTICLES; i++)
            {
                availableEngineTrailParticles.Add(createEngineTrailParticle(position, this));
            }
        }

        public override void Update(float deltaTime)
        {
            if (Active)
            {
                using (new ReadLock(particleLock))
                {
                    using (new WriteLock(particleLock))
                    {
                        position = fighter.getPositionWithJitter() + Vector3.Transform(offset, fighter.Rotation);

                        Vector3 direction = position - lastPosition;
                        distance += direction.Length();
                        if (lastPosition == Vector3.Zero)
                        {
                            distance = 0;
                        }
                        else { }

                        direction.Normalize();
                        direction *= SPAWN_DISTANCE;
                        while (distance >= SPAWN_DISTANCE)
                        {
                            distance -= SPAWN_DISTANCE;
                            spawnEngineTrailParticle(lastPosition += direction, fighter.Velocity);
                        }

                        /*foreach (EngineTrailParticle particle in activeParticles)
                        {
                            particle.Update(deltaTime);
                        }*/

                        for (int i = 0; i < activeParticles.Count; i++)
                        {
                            activeParticles[i].Update(deltaTime);
                            if (activeParticles[i].QueuedRemoval)
                            {
                                availableEngineTrailParticles.Add(activeParticles[i]);
                                activeParticles.RemoveAt(i);
                            }
                            else { }
                        }

                        lastPosition = position;
                    }
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
                        foreach (EngineTrailParticle particle in activeParticles)
                        {
                            particle.Draw(camera, world);
//                            particle.Draw(camera);
                        }
                    }
                    else
                    {
                        foreach (EngineTrailParticle particle in activeParticles)
                        {
                            particle.Draw(camera);
                        }
                    }
                }
                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else { }
        }

        public EngineTrailParticle addEngineTrailParticle(EngineTrailParticle _particle)
        {
            return _particle;
        }

        public abstract EngineTrailParticle createEngineTrailParticle(Vector3 _position, EngineTrailParticleGenerator _home);

        public abstract EngineTrailParticle spawnEngineTrailParticle(Vector3 position, Vector3 velocity);

        public void cleanupList()
        {
            using (new WriteLock(particleLock))
            {
                for (int i = 0; i < activeParticles.Count; i++)
                {
                    activeParticles[i].onDie();
                    availableEngineTrailParticles.Add(activeParticles[i]);
                }
                activeParticles.Clear();
            }
        }
    }
}
