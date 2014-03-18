using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Particles;

namespace Space394.SceneObjects
{
    public class HalkInterceptor : Interceptor
    {
        private const int BARRAGE_MISSLE_COUNT = 24;
        private const int SECONDARY_LOCATION_COUNT = 24;

        private Vector3[] randomUnitVectors;

        private const float RANDOM_SCALE = 1500.0f;

        public HalkInterceptor(long _uniqueId, Vector3 _position, Quaternion _rotation, SpawnShip _home)
            : base(_uniqueId, _position, _rotation, Team.Halk, _home)
        {
            laserOffsets = new Vector3[] {
                new Vector3(-3.382f, -2.532f, 59.654f),
                new Vector3(3.382f, -2.532f, 59.654f)
            };
            MAX_SECONDARY_AMMO = 9;
            SecondaryAmmo = MaxSecondaryAmmo;
            SECONDARY_RANGE = 8000000f;

            randomUnitVectors = new Vector3[SECONDARY_LOCATION_COUNT];
            for (int i = 0; i < SECONDARY_LOCATION_COUNT; i++)
            {
                randomUnitVectors[i] = getRandomUnitVector((int)(i * RANDOM_SCALE));
            }

            #region Missle Positions
            secondaryAttackPositions = new Vector3[] { 
                new Vector3(4.874f, 1.64f, 10.611f),
                new Vector3(5.529f, 1.64f, 10.039f),
                new Vector3(6.153f, 1.64f, 10.508f),
                new Vector3(6.809f, 1.64f, 9.08f),

                new Vector3(4.874f, 0.778f, 10.611f),
                new Vector3(5.529f, 0.778f, 10.039f),
                new Vector3(6.153f, 0.778f, 10.508f),
                new Vector3(6.809f, 0.778f, 9.08f),

                new Vector3(4.874f, -0.031f, 10.611f),
                new Vector3(5.529f, -0.031f, 10.039f),
                new Vector3(6.153f, -0.031f, 10.508f),
                new Vector3(6.809f, -0.031f, 9.08f),

                new Vector3(-4.874f, 1.64f, 10.611f),
                new Vector3(-5.529f, 1.64f, 10.039f),
                new Vector3(-6.153f, 1.64f, 10.508f),
                new Vector3(-6.809f, 1.64f, 9.08f),

                new Vector3(-4.874f, 0.778f, 10.611f),
                new Vector3(-5.529f, 0.778f, 10.039f),
                new Vector3(-6.153f, 0.778f, 10.508f),
                new Vector3(-6.809f, 0.778f, 9.08f),

                new Vector3(-4.874f, -0.031f, 10.611f),
                new Vector3(-5.529f, -0.031f, 10.039f),
                new Vector3(-6.153f, -0.031f, 10.508f),
                new Vector3(-6.809f, -0.031f, 9.08f),
            };
            #endregion 

            secondaryAttackForward = new Quaternion[]{
                Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.ToRadians(0.733f)),
                Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.ToRadians(-0.733f)),
            };

            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(3.39f, 3.917f, -22.27f))); // -12.27f
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(-3.39f, 3.917f, -22.27f)));
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(-3.39f, -2.486f, -22.27f)));
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(3.39f, -2.486f, -22.27f)));

            //trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(3.39f, 3.917f, -12.27f)));
            //trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(-3.39f, 3.917f, -12.27f)));
            //trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(-3.39f, -2.486f, -12.27f)));
            //trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(3.39f, -2.486f, -12.27f)));

            setCloseModelByString("Models/Ships/Halk_interceptor");
            setFarModelByString("Models/Ships/Halk_interceptor");
        }

        public override void fireSecondary()
        {
            if (SecondaryAmmo > 0)
            {
                if (specialTimer <= 0)
                {
                    Quaternion shipRotation = Rotation;
                    Vector3 shipPosition = Position;
                    Vector3 shipVelocity = Velocity;

                    const float DISTANCE_MODIFIER = 500f;
                    Vector3 targetPosOffset = (Vector3.Normalize(shipVelocity) * DISTANCE_MODIFIER);

                    // Unrolled for 24 Positions
                    #region Unrolled Missle Spawns
                    Missile tMissle;
                    //for (int i = 0; i < BARRAGE_MISSLE_COUNT; i++)
                    //{
                        Vector3 missilePos;
                        Vector3 targetPosBase = shipPosition + jitterPosition + targetPosOffset; // Just add random unit vectors to this
                        Quaternion forwardRotation = shipRotation * secondaryAttackForward[0]; // First half
          
                        missilePos = Vector3.Transform(secondaryAttackPositions[0] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[0];

                        missilePos = Vector3.Transform(secondaryAttackPositions[1] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[1];

                        missilePos = Vector3.Transform(secondaryAttackPositions[2] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[2];

                        missilePos = Vector3.Transform(secondaryAttackPositions[3] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[3];

                        missilePos = Vector3.Transform(secondaryAttackPositions[4] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[4];

                        missilePos = Vector3.Transform(secondaryAttackPositions[5] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[5];

                        missilePos = Vector3.Transform(secondaryAttackPositions[6] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[6];

                        missilePos = Vector3.Transform(secondaryAttackPositions[7] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[7];

                        missilePos = Vector3.Transform(secondaryAttackPositions[8] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[8];

                        missilePos = Vector3.Transform(secondaryAttackPositions[9] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[9];

                        missilePos = Vector3.Transform(secondaryAttackPositions[10] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[10];

                        missilePos = Vector3.Transform(secondaryAttackPositions[11] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[11];

                        missilePos = Vector3.Transform(secondaryAttackPositions[12] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[12];

                        // Second half
                        forwardRotation = shipRotation * secondaryAttackForward[1]; // Other Attack Forward

                        missilePos = Vector3.Transform(secondaryAttackPositions[13] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[13];

                        missilePos = Vector3.Transform(secondaryAttackPositions[14] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[14];

                        missilePos = Vector3.Transform(secondaryAttackPositions[15] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[15];

                        missilePos = Vector3.Transform(secondaryAttackPositions[16] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[16];
                        
                        missilePos = Vector3.Transform(secondaryAttackPositions[17] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[17];

                        missilePos = Vector3.Transform(secondaryAttackPositions[18] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[18];

                        missilePos = Vector3.Transform(secondaryAttackPositions[19] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[19];

                        missilePos = Vector3.Transform(secondaryAttackPositions[20] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[20];

                        missilePos = Vector3.Transform(secondaryAttackPositions[21] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[21];

                        missilePos = Vector3.Transform(secondaryAttackPositions[22] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[22];

                        missilePos = Vector3.Transform(secondaryAttackPositions[23] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewHalkIMissile(missilePos, forwardRotation, this);
                        tMissle.TargetPos = targetPosBase + randomUnitVectors[23];
                    //}
                    #endregion 

                    specialTimer = SpecialRate;
                    SecondaryAmmo--;
                }
                else { }
            }
            else { }
        }

        public Vector3 getRandomUnitVector()
        {
            int offsetScale = 1;
            Vector3 randomVel;
            if (random.Next() % 2 == 0)
            {
                offsetScale *= -1;
            }
            randomVel.X = (float)random.NextDouble() * offsetScale;
            if (random.Next() % 2 == 0)
            {
                offsetScale *= -1;
            }
            randomVel.Y = (float)random.NextDouble() * offsetScale;
            if (random.Next() % 2 == 0)
            {
                offsetScale *= -1;
            }
            randomVel.Z = (float)random.NextDouble() * offsetScale;

            return randomVel;
        }

        public Vector3 getRandomUnitVector(int _scale)
        {
            int offsetScale = _scale;
            Vector3 randomVel;
            if (random.Next() % 2 == 0)
            {
                offsetScale *= -1;
            }
            randomVel.X = (float)random.NextDouble() * offsetScale;
            if (random.Next() % 2 == 0)
            {
                offsetScale *= -1;
            }
            randomVel.Y = (float)random.NextDouble() * offsetScale;
            if (random.Next() % 2 == 0)
            {
                offsetScale *= -1;
            }
            randomVel.Z = (float)random.NextDouble() * offsetScale;

            return randomVel;
        }
    }
}
