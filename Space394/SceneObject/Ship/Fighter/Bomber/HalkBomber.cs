using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Particles;

namespace Space394.SceneObjects
{
    public class HalkBomber : Bomber
    {
        private const int SECONDARY_LOCATION_COUNT = 22;

        private Vector3[] randomUnitVectors;

        private const float Y_ADJ = -3.0f;

        public HalkBomber(long _uniqueId, Vector3 _position, Quaternion _rotation, SpawnShip _home)
            : base(_uniqueId, _position, _rotation, Team.Halk, _home)
        {
            laserOffsets = new Vector3[] {
                new Vector3(-2.48f, -0.305f, 62.472f),
                new Vector3(2.48f, -0.305f, 62.472f)
            };
            MAX_SECONDARY_AMMO = 100;
            SecondaryAmmo = MaxSecondaryAmmo;
            SECONDARY_RANGE = 7000000f;

            randomUnitVectors = new Vector3[SECONDARY_LOCATION_COUNT];
            for (int i = 0; i < SECONDARY_LOCATION_COUNT; i++)
            {
                randomUnitVectors[i] = getRandomUnitVector(i * 30);
            }

            #region Bomb Positions
            secondaryAttackPositions = new Vector3[] { 
                new Vector3(-8.268f, -4.127f+Y_ADJ, 4.685f),
                new Vector3(-3.615f, -5.795f+Y_ADJ, 4.607f),
                new Vector3(-6.143f, -4.945f+Y_ADJ, 2.546f),

                new Vector3(-8.596f, -4.116f+Y_ADJ, 0.959f),
                new Vector3(-3.291f, -5.071f+Y_ADJ, 1.351f),
                new Vector3(-5.921f, -5.815f+Y_ADJ, -0.613f),
                
                new Vector3(-8.361f, -3.464f+Y_ADJ, -2.266f),
                new Vector3(-3.354f, -5.013f+Y_ADJ, -2.49f),
                new Vector3(-8.641f, -4.621f+Y_ADJ, -5.479f),
                
                new Vector3(-5.706f, -4.958f+Y_ADJ, -4.815f),
                new Vector3(-2.866f, -4.106f+Y_ADJ, -5.565f),
                new Vector3(3.627f,  -5.7878f+Y_ADJ, 4.771f),

                new Vector3(8.073f,  -4.125f+Y_ADJ,  4.694f),
                new Vector3(6.017f,  -4.945f+Y_ADJ,  2.546f),
                new Vector3(3.165f,  -5.071f+Y_ADJ,  1.351f),

                new Vector3( 8.47f,  -4.116f+Y_ADJ, 0.959f),
                new Vector3( 5.795f, -5.815f+Y_ADJ, -0.613f),
                new Vector3( 3.228f, -5.013f+Y_ADJ, -2.49f),

                new Vector3( 8.235f, -3.464f+Y_ADJ, -2.266f),
                new Vector3( 2.666f, -4.496f+Y_ADJ, -5.521f),
                new Vector3( 5.58f,  -4.958f+Y_ADJ, -4.815f),

                new Vector3( 8.515f, -4.621f+Y_ADJ, -5.479f),
            };
            #endregion

            #region Bomb Rotations
            secondaryAttackForward = new Quaternion[] {
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(165.8f), MathHelper.ToRadians(67.621f), MathHelper.ToRadians(6.381f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(168.184f), MathHelper.ToRadians(74.474f), MathHelper.ToRadians(9.495f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(170.968f), MathHelper.ToRadians(68.803f), MathHelper.ToRadians(5.85f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(170.423f), MathHelper.ToRadians(67.126f), MathHelper.ToRadians(0.571f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(181.767f), MathHelper.ToRadians(70.274f), MathHelper.ToRadians(-4.997f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(179.686f), MathHelper.ToRadians(69.068f), MathHelper.ToRadians(-1.972f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(181.435f), MathHelper.ToRadians(65.41f), MathHelper.ToRadians(-4.97f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(189.469f), MathHelper.ToRadians(69.009f), MathHelper.ToRadians(-13.12f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(186.14f), MathHelper.ToRadians(69.205f), MathHelper.ToRadians(-14.322f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(193.884f), MathHelper.ToRadians(65.793f), MathHelper.ToRadians(-16.371f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(208.472f), MathHelper.ToRadians(67.824f), MathHelper.ToRadians(-24.064f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(168.312f), MathHelper.ToRadians(68.03f), MathHelper.ToRadians(12.955f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(169.998f), MathHelper.ToRadians(62.666f), MathHelper.ToRadians(18.547f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(170.241f), MathHelper.ToRadians(64.076f), MathHelper.ToRadians(10.923f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(138.405f), MathHelper.ToRadians(85.942f), MathHelper.ToRadians(44.306f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(175.988f), MathHelper.ToRadians(61.855f), MathHelper.ToRadians(8.656f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(179.789f), MathHelper.ToRadians(64.408f), MathHelper.ToRadians(4.737f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(194.024f), MathHelper.ToRadians(70.003f), MathHelper.ToRadians(-7.678f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(175.682f), MathHelper.ToRadians(61.992f), MathHelper.ToRadians(9.909f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(204.705f), MathHelper.ToRadians(70.345f), MathHelper.ToRadians(-23.08f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(208.82f), MathHelper.ToRadians(75.323f), MathHelper.ToRadians(-24.299f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(189.112f), MathHelper.ToRadians(76.931f), MathHelper.ToRadians(0.881f)),
            };
            #endregion 

            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(8.455f, -0.823f, -24.89f))); // -14.89f
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(1.154f, 1.198f, -24.89f)));
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(-0.231f, 3.664f, -24.89f)));
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(-2.671f, 0.458f, -24.89f)));
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(-0.881f, -1.909f, -24.89f)));
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(1.87f, -1.911f, -24.89f)));
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(-8.455f, -0.823f, -24.89f)));

            //trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(8.455f, -0.823f, -14.89f)));
            //trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(1.154f, 1.198f, -14.89f)));
            //trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(-0.231f, 3.664f, -14.89f)));
            //trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(-2.671f, 0.458f, -14.89f)));
            //trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(-0.881f, -1.909f, -14.89f)));
            //trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(1.87f, -1.911f, -14.89f)));
            //trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(-8.455f, -0.823f, -14.89f)));
      
            setCloseModelByString("Models/Ships/Halk_Bomber");
            setFarModelByString("Models/Ships/Bomber_LoD");
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
                    Vector3 targetPosBase = shipPosition + jitterPosition + targetPosOffset; // Just add random unit vectors to this

                    // Unrolled for 22 positions
                    #region unrolled missile spawn
                    Missile tMissile;
                    //for (int i = 0; i < SECONDARY_LOCATION_COUNT; i++)
                    //{
                        Vector3 missilePos;
                        
                        missilePos = Vector3.Transform(secondaryAttackPositions[0] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[0], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[0];

                        missilePos = Vector3.Transform(secondaryAttackPositions[1] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[1], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[1];

                        missilePos = Vector3.Transform(secondaryAttackPositions[2] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[2], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[2];

                        missilePos = Vector3.Transform(secondaryAttackPositions[3] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[3], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[3];

                        missilePos = Vector3.Transform(secondaryAttackPositions[4] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[4], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[4];

                        missilePos = Vector3.Transform(secondaryAttackPositions[5] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[5], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[5];

                        missilePos = Vector3.Transform(secondaryAttackPositions[6] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[6], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[6];

                        missilePos = Vector3.Transform(secondaryAttackPositions[7] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[7], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[7];

                        missilePos = Vector3.Transform(secondaryAttackPositions[8] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[8], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[8];

                        missilePos = Vector3.Transform(secondaryAttackPositions[9] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[9], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[9];

                        missilePos = Vector3.Transform(secondaryAttackPositions[10] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[10], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[10];

                        missilePos = Vector3.Transform(secondaryAttackPositions[11] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[11], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[11];

                        missilePos = Vector3.Transform(secondaryAttackPositions[12] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[12], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[12];

                        missilePos = Vector3.Transform(secondaryAttackPositions[13] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[13], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[13];

                        missilePos = Vector3.Transform(secondaryAttackPositions[14] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[14], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[14];

                        missilePos = Vector3.Transform(secondaryAttackPositions[15] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[15], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[15];

                        missilePos = Vector3.Transform(secondaryAttackPositions[16] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[16], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[16];

                        missilePos = Vector3.Transform(secondaryAttackPositions[17] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[17], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[17];

                        missilePos = Vector3.Transform(secondaryAttackPositions[18] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[18], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[18];

                        missilePos = Vector3.Transform(secondaryAttackPositions[19] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[19], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[19];

                        missilePos = Vector3.Transform(secondaryAttackPositions[20] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[20], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[20];

                        missilePos = Vector3.Transform(secondaryAttackPositions[21] + jitterPosition, shipRotation) + shipPosition;
                        tMissile = SceneObjectFactory.createNewHalkBMissile(missilePos, shipRotation * secondaryAttackForward[21], this);
                        tMissile.TargetPos = targetPosBase + randomUnitVectors[21];
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
