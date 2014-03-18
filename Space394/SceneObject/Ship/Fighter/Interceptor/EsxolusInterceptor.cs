using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Particles;

namespace Space394.SceneObjects
{
    public class EsxolusInterceptor : Interceptor
    {
        private const float BARRAGE_MISSLE_COUNT = 38;

        public EsxolusInterceptor(long _uniqueId, Vector3 _position, Quaternion _rotation, SpawnShip _home)
            : base(_uniqueId, _position, _rotation, Team.Esxolus, _home)
        {
            laserOffsets = new Vector3[] {
                new Vector3(-3.342f, -0.594f, 35.574f),
                new Vector3(3.342f, -0.594f, 35.574f)
            };
            MAX_SECONDARY_AMMO = 9;
            SecondaryAmmo = MaxSecondaryAmmo;
            SECONDARY_RANGE = 6000000f;

            secondaryAttackPositions = new Vector3[] {
                new Vector3(1.388f, -0.801f, 6.8f),
                new Vector3(-1.388f, -0.801f, 6.8f),
            };
            secondaryAttackForward = new Quaternion[] {
                // Left Side
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-1.772f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-0.121f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(1.53f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-1.429f), MathHelper.ToRadians(-2.579f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-1.429f), MathHelper.ToRadians(-0.934f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-1.429f), MathHelper.ToRadians(0.711f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-1.429f), MathHelper.ToRadians(2.356f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-2.857f), MathHelper.ToRadians(-3.41f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-2.857f), MathHelper.ToRadians(-1.76f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-2.857f), MathHelper.ToRadians(-0.115f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-2.857f), MathHelper.ToRadians(1.545f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-2.857f), MathHelper.ToRadians(3.18f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-4.281f), MathHelper.ToRadians(-2.579f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-4.281f), MathHelper.ToRadians(-0.934f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-4.281f), MathHelper.ToRadians(0.711f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-4.281f), MathHelper.ToRadians(2.356f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-5.699f), MathHelper.ToRadians(-1.772f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-5.699f), MathHelper.ToRadians(-0.121f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-5.699f), MathHelper.ToRadians(1.53f), MathHelper.ToRadians(0f)),
                // Right Side
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-1.772f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-0.121f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(1.53f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.429f), MathHelper.ToRadians(-2.579f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.429f), MathHelper.ToRadians(-0.934f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.429f), MathHelper.ToRadians(0.711f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.429f), MathHelper.ToRadians(2.356f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(2.857f), MathHelper.ToRadians(-3.41f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(2.857f), MathHelper.ToRadians(-1.76f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(2.857f), MathHelper.ToRadians(-0.115f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(2.857f), MathHelper.ToRadians(1.545f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(2.857f), MathHelper.ToRadians(3.18f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.281f), MathHelper.ToRadians(-2.579f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.281f), MathHelper.ToRadians(-0.934f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.281f), MathHelper.ToRadians(0.711f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.281f), MathHelper.ToRadians(2.356f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(5.699f), MathHelper.ToRadians(-1.772f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(5.699f), MathHelper.ToRadians(-0.121f), MathHelper.ToRadians(0f)),
                Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(5.699f), MathHelper.ToRadians(1.53f), MathHelper.ToRadians(0f)),
            };

            trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(4.041f, -0.191f, -11.169f)));
            trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(-4.041f, -0.191f, -11.169f)));

            setCloseModelByString("Models/Ships/esxolus_interceptor_model");
            setFarModelByString("Models/Ships/esxolus_interceptor__LP_model");
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

                    // Unrolled for 38 positions
                    #region Unrolled Projectile Spawns
                    
                    EsxolusIMissile tMissile;
                    Vector3 missilePos;
                    Vector3 newTargetPosition = shipPosition + jitterPosition + targetPosOffset;

                    missilePos = Vector3.Transform(secondaryAttackPositions[1] + jitterPosition, shipRotation) + shipPosition;
                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[0], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[1], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[2], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[3], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[4], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[5], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[6], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[7], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[8], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[9], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[10], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[11], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[12], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[13], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[14], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[15], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[16], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[17], this);
                    tMissile.TargetPos = newTargetPosition;
                    
                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[18], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[19], this);
                    tMissile.TargetPos = newTargetPosition;

                    // Second half
                    missilePos = Vector3.Transform(secondaryAttackPositions[0] + jitterPosition, shipRotation) + shipPosition;
                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[20], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[21], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[22], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[23], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[24], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[25], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[26], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[27], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[28], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[29], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[30], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[31], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[32], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[33], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[34], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[35], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[36], this);
                    tMissile.TargetPos = newTargetPosition;

                    tMissile = SceneObjectFactory.createNewEsxolusIMissile(missilePos, shipRotation * secondaryAttackForward[37], this);
                    tMissile.TargetPos = newTargetPosition;
                    
                    #endregion 

                    specialTimer = SpecialRate;
                    SecondaryAmmo--;
                }
                else { }
            }
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
