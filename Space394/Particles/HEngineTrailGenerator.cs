using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.SceneObjects;
using Microsoft.Xna.Framework;

namespace Space394.Particles
{
    public class HEngineTrailGenerator : EngineTrailParticleGenerator
    {
        private Random random;
        private Vector3[] fuzzVectors;
        private const float SPREAD = 0.15f;
        private const int MAX_SIZE_MOD = 150;
        private const int MIN_SIZE_MOD = 85;
        private const int SCALE_MOD = 10000;
        private const float DIST_MOD = 3.0f;

        public HEngineTrailGenerator(Fighter _fighter, Vector3 _offset)
            : base(_fighter, _offset)
        {
            PARTICLES = 15;
            MAX_DISTANCE = 85.0f;

            if (random == null)
            {
                initializeVecs();
            }
            else { }

//            distance = (float)random.Next(1, 8);
            SPAWN_DISTANCE = 10.75f + (int)random.Next(0,5);
        }

        public void initializeVecs()
        {
            random = new Random((int)Space394Game.lastGameTime.TotalGameTime.Milliseconds);
            fuzzVectors = new Vector3[]{
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
                makeFuzzVec(),
            };
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
                        Vector3 spawnDirection = direction * SPAWN_DISTANCE;
                        float extraDistance = (float)(random.NextDouble() * DIST_MOD);
                        while (distance >= SPAWN_DISTANCE + extraDistance)
                        {
                            distance -= SPAWN_DISTANCE + extraDistance;
                            lastPosition += spawnDirection + (direction * extraDistance);
                            Vector3 next = Vector3.Transform(fuzzVectors[(random.Next() % fuzzVectors.Length)], fighter.Rotation);
                            spawnEngineTrailParticle(next + lastPosition, fighter.Velocity);
                            extraDistance = (float)(random.NextDouble() * DIST_MOD);
                        }

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

        public override void Initialize()
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

        public override EngineTrailParticle createEngineTrailParticle(Vector3 _position, EngineTrailParticleGenerator _home)
        {
            if (random == null)
            {
                initializeVecs();
            }
            else { }
            float scale = (float)random.Next(MIN_SIZE_MOD, MAX_SIZE_MOD) / SCALE_MOD;
            return new HEngineTrailParticle(_position + fuzzVectors[random.Next(0, 14)], _home, scale);
        }

        public override EngineTrailParticle spawnEngineTrailParticle(Vector3 position, Vector3 velocity)
        {
            EngineTrailParticle rParticle = null;
            if (availableEngineTrailParticles.Count != 0)
            {
                rParticle = availableEngineTrailParticles.First();
                availableEngineTrailParticles.RemoveAt(0);
            }
            else { }

            if (rParticle == null)
            {
                rParticle = createEngineTrailParticle(position, this);  // availableEngineTrailParticles.Dequeue();
            }
            else { }

            activeParticles.Add(rParticle);
            rParticle.respawn(position + (Vector3.Transform(fuzzVectors[random.Next(0, 14)], fighter.Rotation)), Vector3.Zero);

            return rParticle;
        }

        public Vector3 makeFuzzVec()
        {
            Vector3 fuzzVector = Vector3.Zero;
            if (random.Next() % 2 == 0)
            {
                fuzzVector.X = -(float)random.NextDouble();
            }
            else
            {
                fuzzVector.X = (float)random.NextDouble();
            }

            if (random.Next() % 2 == 0)
            {
                fuzzVector.Y = -(float)random.NextDouble();
            }
            else
            {
                fuzzVector.Y = (float)random.NextDouble();
            }
            fuzzVector.Z = -2 * (float)random.NextDouble();

            fuzzVector *= SPREAD;

            return fuzzVector;
        }
    }
}
